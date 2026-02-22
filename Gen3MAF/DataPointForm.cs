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
            InitializeComponent();
            Frequency_label.Text = DataPoint.Frequency.ToString() + " Hz";
            LeftFrequncy_label.Text = DataPoint.FrequencyLeft.ToString() + " Hz";
            RightFrequency_label.Text = DataPoint.FrequencyRight.ToString() + " Hz";

            SlopeLeft_label.Text = DataPoint.SlopeLeft.ToString("f3") ;  
            SlopeRight_label.Text = DataPoint.SlopeRight.ToString("f3");

            Airflow_label.Text = DataPoint.AirFlow.ToString("f3") + " g/s";
            LeftAirflow_label.Text = DataPoint.AirFlowLeft.ToString("f3") + " g/s";
            RightAirflow_label.Text = DataPoint.AirFlowRight.ToString("f3") + " g/s";

            LeftAirflowAdjustment_label.Text = DataPoint.AirFlowLeftAdjustment.ToString("f3") + "%";
            RightAirflowAdjustment_label.Text = DataPoint.AirFlowRightAdjustment.ToString("f3") + "%";

            AdjustedAirflowLeft_label.Text = DataPoint.AirFlowLeftAdjusted.ToString("f3") + " g/s";
            AdjustedAirflowRight_label.Text = DataPoint.AirFlowRightAdjusted.ToString("f3") + " g/s";

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
