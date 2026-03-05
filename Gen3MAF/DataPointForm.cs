using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gen3MAF
{
    public partial class DataPointForm : Form
    {
        public DataPointForm()
        {
            InitializeComponent();
        }

        public DataPointForm(MafDataPoint DataPoint)
        {
            string Hertz = " Hz";
            string Flow = " g/s";
            string Percent = " %";
            int Left = 0;
            int Center = 0;
            int Right = 0;

            InitializeComponent();
            Frequency_label.Text = DataPoint.Frequency.ToString() + Hertz;
            Airflow_label.Text = DataPoint.AirFlow.ToString("f3") + " g/s";



            if (DataPoint.SubDataPoints.Length == 1)
            {
                //
                //   single bucket, just apply the adjustment to the orignal airflow
                //
                Center = 0;
                Left = 0;
                Right = 0;


            }
            else if (DataPoint.SubDataPoints.Length == 2)
            {
                //
                // since the left and right values are an equal distance from the target frequency, just average them together
                //
                Left = 0;
                Right = 1;

            }
            else if (DataPoint.SubDataPoints.Length == 3)
            {
                Left = 0;
                Center = 1;
                Right = 2;
            }

            LeftFrequncy_label.Text = DataPoint.SubDataPoints[Left].TargetFrequency.ToString() + Hertz;
            RightFrequency_label.Text = DataPoint.SubDataPoints[Right].TargetFrequency.ToString() + Hertz;
            CenterTargetFrequncy_label.Text = DataPoint.SubDataPoints[Center].TargetFrequency.ToString() + Hertz;

            CenterBucket_label.Text = DataPoint.SubDataPoints[Center].BucketStart.ToString() + Hertz;
            CenterBucketEnd_label.Text = DataPoint.SubDataPoints[Center].BucketEnd.ToString() + Hertz;

            LeftBucket_label.Text = DataPoint.SubDataPoints[Left].BucketStart.ToString() + Hertz;
            LeftBucketEnd_label.Text = DataPoint.SubDataPoints[Left].BucketEnd.ToString() + Hertz;

            RightBucket_label.Text = DataPoint.SubDataPoints[Right].BucketStart.ToString() + Hertz;
            RightBucketEnd_label.Text = DataPoint.SubDataPoints[Right].BucketEnd.ToString() + Hertz;





            LeftAirflow_label.Text = DataPoint.SubDataPoints[Left].Airflow.ToString("f3") + Flow;
            LeftAirflowAdjustment_label.Text = DataPoint.SubDataPoints[Left].AirFlowAdjustment.ToString("f3") + Percent;
            AdjustedAirflowLeft_label.Text = DataPoint.SubDataPoints[Left].AirFlowAdjusted.ToString("f3") + Flow;


            RightAirflow_label.Text = DataPoint.SubDataPoints[Right].Airflow.ToString("f3") + Flow;
            RightAirflowAdjustment_label.Text = DataPoint.SubDataPoints[Right].AirFlowAdjustment.ToString("f3") + Percent;
            AdjustedAirflowRight_label.Text = DataPoint.SubDataPoints[Right].AirFlowAdjusted.ToString("f3") + Flow;

            AirflowAdjustment_label.Text = DataPoint.SubDataPoints[Center].AirFlowAdjustment.ToString("f3") + Percent;
            IntermediateAdjustedAirflow_label.Text = DataPoint.SubDataPoints[Center].AirFlowAdjusted.ToString("f3") + Flow;

            SlopeLeft_label.Text = DataPoint.LeftAirFlowSlope.ToString("f4");
            SlopeRight_label.Text = DataPoint.RightAirFlowSlope.ToString("f4");
            LeftTuneFrequency_label.Text = DataPoint.LeftFrequency.ToString() + Hertz;
            RightTuneFrequency_label.Text = DataPoint.RightFrequency.ToString() + Hertz;
            LeftTuneAirflow_label.Text = DataPoint.LeftAirFlow.ToString("f3") + Flow;
            RightTuneAirflow_label.Text = DataPoint.RightAirFlow.ToString("f3") + Flow;
            TuneAirflow_label.Text = DataPoint.AirFlow.ToString("f3") + Flow;
            Bias_label.Text = DataPoint.Bias.ToString("f5") + Flow;

            LeftRightAverage_label.Text = DataPoint.LeftRightAverageAirflowAdjusted.ToString("f3") + Flow;
            AdjustedAirflow_label.Text = DataPoint.AirFlowAdjusted.ToString("f3") + Flow;

        }

        private void DataPointForm_Load(object sender, EventArgs e)
        {

        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Airflow_label_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void AdjustedAirflow_label_Click(object sender, EventArgs e)
        {

        }
    }
}
