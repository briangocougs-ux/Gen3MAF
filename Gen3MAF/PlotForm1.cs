using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gen3MAF
{
    public partial class PlotForm1 : Form
    {
        public PlotForm1()
        {
            InitializeComponent();
        }

        public PlotForm1(double[] x, double[] y)
        {
            InitializeComponent();

            chart1.Series.Clear();

            var series = new Series("Data")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Double,
                YValueType = ChartValueType.Double,
                BorderWidth = 2
            };

            int n = Math.Min(x.Length, y.Length);
            for (int i = 0; i < n; i++)
            {
                series.Points.AddXY(x[i], y[i]);
            }

            chart1.Series.Add(series);
        }


        public PlotForm1(double[] frequency, List<double[]> airflowRows)
        {
            InitializeComponent();

            chart1.Series.Clear();

            var area = chart1.ChartAreas[0];
            area.AxisX.Title = "Frequency (Hz)";
            area.AxisY.Title = "Airflow";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.Enabled = true;

            int curveIndex = 1;

            foreach (var airflow in airflowRows)
            {
                var series = new Series($"Curve {curveIndex++}")
                {
                    ChartType = SeriesChartType.Line,
                    XValueType = ChartValueType.Double,
                    YValueType = ChartValueType.Double,
                    BorderWidth = 2
                };

                int n = Math.Min(frequency.Length, airflow.Length);
                for (int i = 0; i < n; i++)
                {
                    series.Points.AddXY(frequency[i], airflow[i]);
                }

                chart1.Series.Add(series);
            }

            if (chart1.Legends.Count == 0)
                chart1.Legends.Add(new Legend());
        }


        public PlotForm1(double[] xFreq, double[] oldAir, double[] newAir, string title)
        {
            InitializeComponent();

            Text = $"% Change - {title}";

            chart1.Series.Clear();

            var area = chart1.ChartAreas[0];
            area.AxisX.Title = "Frequency (Hz)";
            area.AxisY.Title = "Percent Change (%)";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.Enabled = true;

            var s = new Series("% Change")
            {
                ChartType = SeriesChartType.Line,   // or Point for scatter
                XValueType = ChartValueType.Double,
                YValueType = ChartValueType.Double,
                BorderWidth = 2
            };

            int n = Math.Min(xFreq.Length, Math.Min(oldAir.Length, newAir.Length));
            for (int i = 0; i < n; i++)
            {
                double denom = oldAir[i];
                if (denom == 0) continue; // or s.Points.AddXY(xFreq[i], 0);

                double pct = (newAir[i] - oldAir[i]) / denom * 100.0;
                s.Points.AddXY(xFreq[i], pct);
            }

            chart1.Series.Add(s);

            // Optional: a zero reference line (makes it easier to read)
            AddZeroLine();
        }

        public PlotForm1(
                    double[] xFreq,
                    double[] baseAir,
                    double[] prevAir,
                    double[] curAir,
                    string title)
        {
            InitializeComponent();
            Text = $"% Change - {title}";

            chart1.Series.Clear();

            var area = chart1.ChartAreas[0];
            area.AxisX.Title = "Frequency (Hz)";
            area.AxisY.Title = "Percent Change (%)";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.Enabled = true;

            var (start, end) = FindNonZeroRange(baseAir, curAir);

            // var area = chart1.ChartAreas[0];

            area.AxisX.Minimum = xFreq[start];
            area.AxisX.Maximum = xFreq[end];

            if (chart1.Legends.Count == 0)
                chart1.Legends.Add(new Legend());

            // Step: current vs previous
            AddPercentSeries("% vs Prev", xFreq, baseAir, prevAir, ChartDashStyle.Solid, borderWidth: 2);

            // Cumulative: current vs baseline (cycle 1)
            AddPercentSeries("% vs Base", xFreq, baseAir, curAir, ChartDashStyle.Dash, borderWidth: 2);

            AddZeroLine(area);
            chart1.ResetAutoValues();
        }

        public PlotForm1(
            double[] freq,
            double[] baseAir,
            IReadOnlyList<double[]> adjustedAirSets,
            IReadOnlyList<string>? seriesNames = null
            )
        {
            InitializeComponent();
            Text = $"% Change - Airflow Differnce percentage";

            chart1.Series.Clear();

            MafPlotter.PlotPercentDiffs(
               chart1,
               freq,
               baseAir,
               adjustedAirSets,
               seriesNames,
               true,
               true
               );
            return;
        }
        private void AddPercentSeries(string name, double[] x, double[] denom, double[] num,
                                  ChartDashStyle dash, int borderWidth)
        {
            var s = new Series(name)
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Double,
                YValueType = ChartValueType.Double,
                BorderWidth = borderWidth,
                BorderDashStyle = dash
            };

            s.MarkerStyle = MarkerStyle.Circle;
            s.MarkerSize = 6;
            s.MarkerBorderWidth = 1;

            int n = Math.Min(x.Length, Math.Min(denom.Length, num.Length));
            for (int i = 0; i < n; i++)
            {
                double d = denom[i];
                if (d == 0) continue; // avoid div-by-zero; or plot 0

                double pct = (num[i] - d) / d * 100.0;
                s.Points.AddXY(x[i], pct);
            }

            chart1.Series.Add(s);
        }
        private void AddZeroLine()
        {
            var zero = new StripLine
            {
                Interval = 0,
                IntervalOffset = 0,
                StripWidth = 0,
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dash,
            };

            chart1.ChartAreas[0].AxisY.StripLines.Add(zero);
        }

        private void AddZeroLine(ChartArea area)
        {
            area.AxisY.StripLines.Clear();
            area.AxisY.StripLines.Add(new StripLine
            {
                Interval = 0,
                IntervalOffset = 0,
                StripWidth = 0,
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot
            });
        }

        private (int start, int end) FindNonZeroRange(double[] baseAir, double[] curAir)
        {
            int n = Math.Min(baseAir.Length, curAir.Length);

            int start = 0;
            int end = n - 1;

            // find first non-zero
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(baseAir[i] - curAir[i]) > .0001)
                {
                    start = i;
                    break;
                }
            }

            // find last non-zero
            for (int i = n - 1; i >= 0; i--)
            {
                if (Math.Abs(baseAir[i] - curAir[i]) > .0001)
                {
                    end = i;
                    break;
                }
            }

            if (start > 0)
            {
                start--;
            }

            if (end + 1 < n)
            {
                end++;
            }

            return (start, end);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Plot_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
