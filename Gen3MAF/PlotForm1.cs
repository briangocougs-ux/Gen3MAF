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
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
