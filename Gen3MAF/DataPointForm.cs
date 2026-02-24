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
            AdjustmentDataPoint DefaultDataPoint;

            DefaultDataPoint.TargetFrequency = 0x7ffffff;
            DefaultDataPoint.BucketStart = 0x7000000;
            DefaultDataPoint.BucketEnd = 0x80000000;
            DefaultDataPoint.Airflow = double.NaN;
            DefaultDataPoint.AirFlowAdjustment = double.NaN;
            DefaultDataPoint.AirFlowAdjusted = double.NaN;  
            



            int Left= 0;
            int Center = 0;
            int Right= 0;

            InitializeComponent();
            Frequency_label.Text = DataPoint.Frequency.ToString() + " Hz";
            Airflow_label.Text = DataPoint.AirFlow.ToString("f3") + " g/s";

            

            if (DataPoint.DataPoints.Length == 1)
            {
                //
                //   single bucket, just apply the adjustment to the orignal airflow
                //
                Center = 0;
                Left = 0;
                Right= 0;
               

            }
            else if (DataPoint.DataPoints.Length == 2)
            {
                //
                // since the left and right values are an equal distance from the target frequency, just average them together
                //
                Left = 0;
                Right = 1;
                
            }
            else if (DataPoint.DataPoints.Length == 3)
            {
                Left = 0;
                Center = 1;
                Right = 2;
            }

            LeftFrequncy_label.Text = DataPoint.DataPoints[Left].TargetFrequency.ToString() + " Hz";
            RightFrequency_label.Text = DataPoint.DataPoints[Right].TargetFrequency.ToString() + " Hz";

            CenterBucket_label.Text = DataPoint.DataPoints[Center].BucketStart.ToString() + " Hz";
            CenterBucketEnd_label.Text = DataPoint.DataPoints[Center].BucketEnd.ToString() + " Hz";

            LeftBucket_label.Text = DataPoint.DataPoints[Left].BucketStart.ToString() + " Hz";
            LeftBucketEnd_label.Text = DataPoint.DataPoints[Left].BucketEnd.ToString() + " Hz";

            RightBucket_label.Text = DataPoint.DataPoints[Right].BucketStart.ToString() + " Hz";
            RightBucketEnd_label.Text = DataPoint.DataPoints[Right].BucketEnd.ToString() + " Hz";


            SlopeLeft_label.Text = DataPoint.LeftAirFlowSlope.ToString("f4");
            SlopeRight_label.Text = DataPoint.RightAirFlowSlope.ToString("f4");
            

            LeftAirflow_label.Text = DataPoint.DataPoints[Left].Airflow.ToString("f3") + " g/s";
            LeftAirflowAdjustment_label.Text = DataPoint.DataPoints[Left].AirFlowAdjustment.ToString("f3") + "%";
            AdjustedAirflowLeft_label.Text = DataPoint.DataPoints[Left].AirFlowAdjusted.ToString("f3") + " g/s";


            RightAirflow_label.Text = DataPoint.DataPoints[Right].Airflow.ToString("f3") + " g/s";
            RightAirflowAdjustment_label.Text = DataPoint.DataPoints[Right].AirFlowAdjustment.ToString("f3") + "%";
            AdjustedAirflowRight_label.Text = DataPoint.DataPoints[Right].AirFlowAdjusted.ToString("f3") + " g/s";

            AirflowAdjustment_label.Text = DataPoint.DataPoints[Center].AirFlowAdjustment.ToString("f3") + "%";
            IntermediateAdjustedAirflow_label.Text = DataPoint.DataPoints[Center].AirFlowAdjustment.ToString("f3") + "%";

            
            AdjustedAirflow_label.Text = DataPoint.AirFlowAdjusted.ToString("f3") + " g/s";

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
    }
}
