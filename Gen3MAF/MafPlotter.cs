using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gen3MAF
{
    

        public static class MafPlotter
        {
        /// <summary>
        /// Plots N lines: percent difference between baseAir and each adjustedAir array.
        /// percentDiff[i] = 100 * (adjusted[i] / base[i] - 1)
        /// </summary>
        public static void PlotPercentDiffs(
            Chart chart,
            double[] freq,
            double[] baseAir,
            IReadOnlyList<double[]> adjustedAirSets,
            IReadOnlyList<string>? seriesNames = null,
            bool zoomToNonZeroBase = true,
            bool showMarkers = false)
        {
            if (chart is null) throw new ArgumentNullException(nameof(chart));
            if (freq is null) throw new ArgumentNullException(nameof(freq));
            if (baseAir is null) throw new ArgumentNullException(nameof(baseAir));
            if (adjustedAirSets is null) throw new ArgumentNullException(nameof(adjustedAirSets));
            if (freq.Length != baseAir.Length)
                throw new ArgumentException("freq and baseAir must have the same length.");

            int n = adjustedAirSets.Count;
            if (n == 0) throw new ArgumentException("adjustedAirSets must contain at least one array.");

            for (int s = 0; s < n; s++)
            {
                if (adjustedAirSets[s] is null)
                    throw new ArgumentException($"adjustedAirSets[{s}] is null.");
                if (adjustedAirSets[s].Length != freq.Length)
                    throw new ArgumentException($"adjustedAirSets[{s}] length must match freq/baseAir length.");
            }

            // Ensure chart area exists
            if (chart.ChartAreas.Count == 0)
                chart.ChartAreas.Add(new ChartArea("Main"));

            var area = chart.ChartAreas[0];

            // Setup axes labels (optional)
            area.AxisX.Title = "Frequency (Hz)";
            area.AxisY.Title = "Percent difference vs base (%)";

            // Clear existing series
            chart.Series.Clear();
            
            int[] NonZeroIndex = new int[freq.Length];

            // Create N series
            for (int s = 0; s < n; s++)
            {
                string name =
                    (seriesNames != null && s < seriesNames.Count && !string.IsNullOrWhiteSpace(seriesNames[s]))
                    ? seriesNames[s]
                    : $"Adj {s + 1}";

                var series = new Series(name)
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    XValueType = ChartValueType.Double,
                    YValueType = ChartValueType.Double,
                    IsVisibleInLegend = true,
                    MarkerStyle = showMarkers ? MarkerStyle.Circle : MarkerStyle.None,
                    MarkerSize = showMarkers ? 5 : 0,
                };

                double[] adj = adjustedAirSets[s];

                for (int i = 0; i < freq.Length; i++)
                {
                    double b = baseAir[i];
                    double y;

                    // Avoid infinities / bogus huge spikes when base is 0 or extremely close to 0
                    if (Math.Abs(b) < 1e-12)
                    {
                        y = double.NaN; // chart will treat as a gap
                    }
                    else
                    {
#if false
                        y = 100.0 * (adj[i] / b - 1.0);
#else
                        y = adj[i] - b;
#endif
                        if (Math.Abs(y) > .01)
                        {
                            NonZeroIndex[i] = 1;

                        }
                    }

                    series.Points.AddXY(freq[i], y);
                }

                chart.Series.Add(series);
            }

            // Nice behavior for NaNs: don't draw connecting line through gaps
            foreach (var s in chart.Series)
                s.EmptyPointStyle.BorderDashStyle = ChartDashStyle.NotSet;

            // Optional zoom to where baseAir is non-zero
            if (zoomToNonZeroBase)
            {
                int start = 0;
                int end = NonZeroIndex.Length-1;    

                for (int k = 0; k < NonZeroIndex.Length; k++)
                {
                    if ((start == 0) && (NonZeroIndex[k] > 0))
                    {
                        start = k;

                    }

                    if (NonZeroIndex[k] > 0)
                    {
                        end = k;
                    }
                }

                if (start >0)
                { 
                    start = start - 1; 
                }

                if (end < NonZeroIndex.Length-1)
                {
                    end = end + 1;
                }   

                // Expand slightly for aesthetics
                double xMin = freq[start];
                double xMax = freq[end];

                area.AxisX.Minimum = xMin;
                area.AxisX.Maximum = xMax;

            }
            else
            {


                // Let chart autoscale X
                area.AxisX.Minimum = double.NaN;
                area.AxisX.Maximum = double.NaN;
            }

            // Let Y autoscale
            area.AxisY.Minimum = double.NaN;
            area.AxisY.Maximum = double.NaN;

            chart.Invalidate();
        }
            

            /// <summary>
            /// Finds the first and last indices where array value is non-zero (above epsilon).
            /// Returns false if no values are non-zero.
            /// </summary>
            private static bool TryFindNonZeroRange(double[] arr, out int start, out int end, double epsilon = 1e-12)
            {
                start = -1;
                end = -1;

                for (int i = 0; i < arr.Length; i++)
                {
                    if (Math.Abs(arr[i]) > epsilon)
                    {
                        start = i;
                        break;
                    }
                }

                if (start < 0) return false;

                for (int i = arr.Length - 1; i >= 0; i--)
                {
                    if (Math.Abs(arr[i]) > epsilon)
                    {
                        end = i;
                        break;
                    }
                }

                return end >= start;
            }
        }
   
}
