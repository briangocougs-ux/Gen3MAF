using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Gen3MAF.Main;

namespace Gen3MAF
{
    public partial class Vehicle : Form
    {

        private string m_VehicleName;
        private string m_ECU;
        private string m_OS;
        private int m_MinFrequency;
        private int m_MaxFrequency;
        private int m_FrequencyStep;
        private BucketStyleEnum m_BucketStyle;
        private uint m_TuneCycleSequenceNumber;


        public Vehicle()
        {


            InitializeComponent();

            FrequncyMin_UpDown.Value = Main.MIN_MAF_FREQUENCY;
            MaxFrequency_numericUpDown.Value = Main.MAX_MAF_FREQUENCY;
            FrequencyStep_numericUpDown.Value = Main.MAF_FREQUENCY_STEP;
            BucketStyle_comboBox.SelectedIndex = 0;
        }

        public string VehicleName { get { return m_VehicleName; } }
        public string ECU { get { return m_ECU; } }
        public string OS { get { return m_OS; } }
        public int MinFrequency { get { return m_MinFrequency; } }
        public int MaxFrequency { get { return m_MaxFrequency; } }
        public int FrequencyStep { get { return m_FrequencyStep; } }
        public BucketStyleEnum BucketStyle { get { return  m_BucketStyle; } }

        internal SessionClass GetSessionInfo()
        {
            SessionClass NewSession = new SessionClass(m_VehicleName, m_ECU, m_OS, m_MinFrequency, m_MaxFrequency, m_FrequencyStep, m_BucketStyle, 1);
            return NewSession;
        }



        private void FrequncyMin_UpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void MaxFrequency_label_Click(object sender, EventArgs e)
        {

        }

        private void FrequencyStep_label_Click(object sender, EventArgs e)
        {

        }

        private void FrequencyStep_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void OK_button_Click(object sender, EventArgs e)
        {

            int MinFrequency = (int)FrequncyMin_UpDown.Value;
            int MaxFrequency = (int)MaxFrequency_numericUpDown.Value;
            int FrequencyStep = (int)FrequencyStep_numericUpDown.Value;


            if (MinFrequency >= MaxFrequency)
            {

                MessageBox.Show("Min Frequency must be less than Max Frequency");
                return;
            }

            if (FrequencyStep < 1)
            {
                MessageBox.Show("Frequency step must be greater than 0");
                return;
            }

            if (((MaxFrequency - MinFrequency) % FrequencyStep) != 0)
            {
                MessageBox.Show("Frequency step must divide equally in difference between Max and Min Frequency");
                return;
            }

            if (BucketStyle_comboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a bucket style");
                return;
            }

            if (VehicleName_textBox.Text.Length == 0)
            {
                MessageBox.Show("Vehicle Name must be supplied");
                return;

            }

            m_VehicleName = VehicleName_textBox.Text;
            m_ECU = ECU_textBox.Text;
            m_OS = OS_textBox.Text;
            m_MinFrequency = MinFrequency;
            m_MaxFrequency = MaxFrequency;
            m_FrequencyStep = FrequencyStep;
            

            if (BucketStyle_comboBox.SelectedIndex == 0) 
            {
                m_BucketStyle = BucketStyleEnum.Triple;
            }
            else if (BucketStyle_comboBox.SelectedIndex == 1)
            {
                m_BucketStyle = BucketStyleEnum.Double;   
            }
            else if (BucketStyle_comboBox.SelectedIndex == 2)
            {
                m_BucketStyle = BucketStyleEnum.Single;
            }

            Close();
        }

        private void BucketStyle_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
