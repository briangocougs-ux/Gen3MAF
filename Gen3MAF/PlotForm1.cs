using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private bool _panning;
#if false
        private double _panStartX;
        private double _viewStartX;
        private double _viewSizeX;
#else
        private double _panStartXVal, _panStartYVal;
        private double _startPosX, _startPosY;
        private double _sizeX, _sizeY;
        private bool m_InMouseMove=false;   
        private bool m_DirectionPicked=false;   
        private bool m_Xpanning=false;  
#endif

        public PlotForm1()
        {
            InitializeComponent();
        }

        public PlotForm1(double[] x, double[] y)
        {
            InitializeComponent();

            int start = 0;
            int end = x.Length - 1;

            chart1.Series.Clear();

            var area = chart1.ChartAreas[0];
            area.AxisX.Title = "Frequency (Hz)";
            area.AxisY.Title = "Airflow";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.Enabled = true;

            var series = new Series("Data")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Double,
                YValueType = ChartValueType.Double,
                BorderWidth = 2
            };


            series.MarkerStyle = MarkerStyle.Circle;
            series.MarkerSize = 6;
            series.MarkerBorderWidth = 1;
            series.ToolTip = "Hz: #VALX\n%: #VAL";

            int n = Math.Min(x.Length, y.Length);
            for (int i = 0; i < n; i++)
            {
                series.Points.AddXY(x[i], y[i]);


                if (Math.Abs(y[i]) > 0.001)
                {
                    if (start == 0)
                    {
                        start = i;

                    }
                    end = i;
                }
            }

            if (start > 1)
            {
                start--;
            }

            if (end < n - 1)
            {
                end++;
            }



            double xMin = x[start];
            double xMax = x[end];

            area.AxisX.Minimum = xMin;
            area.AxisX.Maximum = xMax;

            chart1.Series.Add(series);
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
            Text = $" Change - Airflow Differnce ";

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

        private void chart1_DoubleClick(object sender, EventArgs e)
        {

            var area = chart1.ChartAreas[0];

            area.AxisX.ScaleView.ZoomReset();
            area.AxisY.ScaleView.ZoomReset();
        }

        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            var area = chart1.ChartAreas[0];

            try
            {
                double xMin = area.AxisX.ScaleView.ViewMinimum;
                double xMax = area.AxisX.ScaleView.ViewMaximum;

                double yMin = area.AxisY.ScaleView.ViewMinimum;
                double yMax = area.AxisY.ScaleView.ViewMaximum;

                double cursorX = area.AxisX.PixelPositionToValue(e.Location.X);
                double cursorY = area.AxisY.PixelPositionToValue(e.Location.Y);

                if (e.Delta > 0) // zoom in
                {
                    double newSize = (xMax - xMin) / 2;
                    area.AxisX.ScaleView.Zoom(cursorX - newSize / 2,
                                              cursorX + newSize / 2);

                    newSize = (yMax - yMin) / 2;
                    area.AxisY.ScaleView.Zoom(cursorY - newSize / 2,
                                              cursorY + newSize / 2);
                }
                else // zoom out
                {
                    area.AxisX.ScaleView.ZoomReset();
                    area.AxisY.ScaleView.ZoomReset();
                }
            }
            catch { }
        }


private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var area = chart1.ChartAreas[0];
            var ax = area.AxisX;
            var ay = area.AxisY;

            // Need an active view window to pan; easiest rule: pan only when zoomed.
            if (!ax.ScaleView.IsZoomed && !ay.ScaleView.IsZoomed)
                return;

            m_DirectionPicked = false;
            _panning = true;
            chart1.Capture = true;

            // Record where the mouse is (in axis units)
            _panStartXVal = ax.PixelPositionToValue(e.X);
            _panStartYVal = ay.PixelPositionToValue(e.Y);

            // Record current view window
            _startPosX = ax.ScaleView.Position;
            _sizeX = ax.ScaleView.Size;

            _startPosY = ay.ScaleView.Position;
            _sizeY = ay.ScaleView.Size;

            // IMPORTANT: prevent left-zoom selection cursors from also trying to select
            // while we're right-dragging.
            area.CursorX.IsUserSelectionEnabled = false;
            area.CursorY.IsUserSelectionEnabled = false;
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_panning)
                return;

            if (m_InMouseMove)
                return;

            m_InMouseMove = true;

            try
            {
                var area = chart1.ChartAreas[0];
                var ax = area.AxisX;
                var ay = area.AxisY;

                // Only convert mouse coordinates if the pointer is inside the plot rectangle.
                var inner = area.InnerPlotPosition;
                var pos = area.Position;

                double left = chart1.ClientSize.Width * (pos.X + inner.X * pos.Width / 100.0) / 100.0;
                double top = chart1.ClientSize.Height * (pos.Y + inner.Y * pos.Height / 100.0) / 100.0;
                double width = chart1.ClientSize.Width * (inner.Width * pos.Width / 100.0) / 100.0;
                double height = chart1.ClientSize.Height * (inner.Height * pos.Height / 100.0) / 100.0;

                if (e.X < left || e.X > left + width || e.Y < top || e.Y > top + height)
                    return;

                double curXVal = ax.PixelPositionToValue(e.X);
                double curYVal = ay.PixelPositionToValue(e.Y);

                double dx = _panStartXVal - curXVal;
                double dy = _panStartYVal - curYVal;

                if (!m_DirectionPicked)
                {
                    m_Xpanning = Math.Abs(dx) > Math.Abs(dy);
                    m_DirectionPicked = true;
                }

                if (m_Xpanning)
                {
                    if (ax.ScaleView.IsZoomed && _sizeX > 0)
                    {
                        double newPosX = ClampViewPosition(ax, _startPosX + dx, _sizeX);
                        ax.ScaleView.Position = newPosX;
                    }
                }
                else
                {
                    if (ay.ScaleView.IsZoomed && _sizeY > 0)
                    {
                        double newPosY = ClampViewPosition(ay, _startPosY + dy, _sizeY);
                        ay.ScaleView.Position = newPosY;
                    }
                }
            }
            finally
            {
                m_InMouseMove = false;
            }
            return;
        }

        private static double ClampViewPosition(Axis axis, double proposedPos, double viewSize)
        {
            // Determine clamp bounds in axis units.
            // If Minimum/Maximum are NaN (autoscale), use the current view min/max.
            double min = axis.Minimum;
            double max = axis.Maximum;

            if (double.IsNaN(min)) min = axis.ScaleView.ViewMinimum;
            if (double.IsNaN(max)) max = axis.ScaleView.ViewMaximum;

            double maxPos = max - viewSize;

            if (proposedPos < min) proposedPos = min;
            if (proposedPos > maxPos) proposedPos = maxPos;

            return proposedPos;
        }

        

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || !_panning)
                return;
            m_DirectionPicked = false;
            _panning = false;
            chart1.Capture = false;

            // Re-enable left-drag zoom selection
            var area = chart1.ChartAreas[0];
            area.CursorX.IsUserSelectionEnabled = true;
            area.CursorY.IsUserSelectionEnabled = true;
        }


        public PlotForm1(double[] frequency, int BuckestPerFrquency, List<double[]> airflowRows)
        {
            InitializeComponent();
            // Make mouse wheel / right-drag feel responsive (chart must have focus)
            chart1.MouseEnter += (s, e) => chart1.Focus();

            chart1.MouseWheel += chart1_MouseWheel;
            chart1.MouseDown += chart1_MouseDown;
            chart1.MouseMove += chart1_MouseMove;
            chart1.MouseUp += chart1_MouseUp;

            chart1.Series.Clear();

            var area = chart1.ChartAreas[0];
            area.AxisX.Title = "Frequency (Hz)";
            area.AxisY.Title = "Airflow";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.Enabled = true;

            area.AxisX.ScaleView.Zoomable = true;
            area.AxisY.ScaleView.Zoomable = true;

            area.CursorX.IsUserEnabled = true;
            area.CursorX.IsUserSelectionEnabled = true;

            area.CursorY.IsUserEnabled = true;
            area.CursorY.IsUserSelectionEnabled = true;
            area.AxisX.LabelStyle.Format = "F0";  // 2 decimal places
            area.AxisY.LabelStyle.Format = "F0";  // 1 decimal place


            area.InnerPlotPosition.Auto = false;

            // 3. Define the plot area (relative to the ChartArea's size)
            // This leaves 15% space on the left/bottom for labels and 5% on top/right
            area.InnerPlotPosition.X = 7;      // Left margin
            area.InnerPlotPosition.Y = 1;       // Top margin
            area.InnerPlotPosition.Width = 93;  // 100 - 15 (left) - 5 (right)
            area.InnerPlotPosition.Height = 92; // 100 - 5 (top) - 15 (bottom)

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

                series.MarkerStyle = MarkerStyle.Circle;
                series.MarkerSize = 6;
                series.MarkerBorderWidth = 1;

                series.ToolTip = "Hz: #VALX{f0}\nFlow: #VAL{f3}";

                int n = Math.Min(frequency.Length, airflow.Length);
                for (int i = 0; i < n; i++)
                {
                    series.Points.AddXY(frequency[i], airflow[i]);

                    if ((i + (BuckestPerFrquency -1)) % BuckestPerFrquency == 0)
                    {
                        series.Points[i].MarkerStyle = MarkerStyle.Diamond;
                        series.Points[i].MarkerSize = 12;
                    }
                }

                chart1.Series.Add(series);
            }

            if (chart1.Legends.Count == 0)
                chart1.Legends.Add(new Legend());
        }

    }
}

