using System.Diagnostics;
using System.Windows.Forms;
using static Gen3MAF.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Gen3MAF
{
    public partial class Form1 : Form
    {
        public const uint MIN_MAF_FREQUENCY = 1500;
        public const uint MAX_MAF_FREQUENCY = 12000;
        public const uint MAF_FREQUENCY_STEP = 125;

        public const int DATA_GRID_ROW_FREQUENCY = 0;
        public const int DATA_GRID_ROW_AIRFLOW = 1;
        public const int DATA_GRID_ROW_AIRFLOW_ADJUSTMENT = 2;
        public const int DATA_GRID_ROW_AIRFLOW_ADJUSTED = 3;

        public enum BucketStyleEnum
        {
            Single=1,
            Double=2
        }

        public struct MafDataPoint
        {
            public uint Frequency;
            public bool HasUpdatedAirFlow;

            public double AirFlow;
            public double AirFlowAdjustment;
            public double AirFlowAdjusted;

            public double AirFlowLeft;
            public double AirFlowLeftAdjustment;
            public double AirFlowLeftAdjusted;


            public double AirFlowRight;
            public double AirFlowRightAdjustment;
            public double AirFlowRightAdjusted;
        }


        TuneCycle m_CurrentTuneCycle;

        MafDataPoint[] mafDataPoints;

        int m_FirstUpdatedBucketIndex = -1;
        int m_LastUpdatedBucketIndex = -1;

        int m_MinMAFFrequency = 0;
        int m_MaxMAFFrequency = 0;
        int m_MAFFrequencyStep = 0;
        BucketStyleEnum m_BucketStyle = BucketStyleEnum.Double; 

        uint m_BucketCount = 0;

        int m_mafFrequencyCount = 0;

        public Form1()
        {

            InitializeComponent();

            mafDataPoints = new MafDataPoint[1];

            MAF_dataGridView.RowHeadersVisible = false;
            MAF_dataGridView.ColumnHeadersVisible = false;
            MAF_dataGridView.AllowUserToAddRows = false;
            MAF_dataGridView.AllowUserToResizeRows = false;
            MAF_dataGridView.AllowUserToResizeColumns = false;
            MAF_dataGridView.ReadOnly = true;
            MAF_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            AdjustedAirflow_dataGridView.RowHeadersVisible = false;
            AdjustedAirflow_dataGridView.ColumnHeadersVisible = false;
            AdjustedAirflow_dataGridView.AllowUserToAddRows = false;
            AdjustedAirflow_dataGridView.AllowUserToResizeRows = false;
            AdjustedAirflow_dataGridView.AllowUserToResizeColumns = false;
            AdjustedAirflow_dataGridView.ReadOnly = true;
            AdjustedAirflow_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            

            AdjustmentPercent_label.Text = $"{AdjustmentPercent_trackBar.Value}%";
        }

        private void Process_button_Click(object sender, EventArgs e)
        {
            string[] mafAirflowStrings;
            double[] mafAirflowValues=new double[m_mafFrequencyCount];

            m_CurrentTuneCycle =new TuneCycle(m_mafFrequencyCount, (int)m_BucketCount);

            mafAirflowStrings = AirFlow_richTextBox.Text.Split(
                                                    new[] { ' ', '\t', ',', '\r', '\n' },
                                                    StringSplitOptions.RemoveEmptyEntries
                                                    );
            if (mafAirflowStrings.Length != m_mafFrequencyCount)
            {
                MessageBox.Show($"Please enter {m_mafFrequencyCount} airflow values, separated by spaces, tabs, commas, or newlines.");
                return;
            }

            //
            //  Validate and parse airflow values
            //
            try
            {
                for (uint i = 0; i < m_mafFrequencyCount; i++)
                {
                    mafAirflowValues[i] = double.Parse(mafAirflowStrings[i]);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please ensure all airflow values are valid numbers index={i}.");
                return;
            }
            catch (OverflowException)
            {
                MessageBox.Show("Please ensure all airflow values are within a valid range index={i}.");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
                return;
            }

            //  Populate the initial airflow values in the TuneCycle object, which will be used for adjustments and calculations later on.
            //
            m_CurrentTuneCycle.PopulateInitialAirflow(mafAirflowValues);

            //  Populate the MafDataPoint array with the frequency and airflow values from the TuneCycle object, and calculate the left and right airflow values for each data point based on the defined frequency step.
            //  This allows us to have a structured representation of the airflow data for each frequency point, which can be used for adjustments and displaying in the data grid views.
            //
            for (uint i = 0; i < m_mafFrequencyCount; i++)
            {
                mafDataPoints[i].AirFlow = m_CurrentTuneCycle.GetAirflowAtIndex((int)i);
            }

            for (int i = 0; i < (mafDataPoints.Length - 1); i++)
            {
                int j = i + 1;
                double slope = (mafDataPoints[j].AirFlow - mafDataPoints[i].AirFlow) / (mafDataPoints[j].Frequency - mafDataPoints[i].Frequency);

                mafDataPoints[i].AirFlowRight = mafDataPoints[i].AirFlow + (slope * (m_MAFFrequencyStep * 0.25f));
                mafDataPoints[j].AirFlowLeft = mafDataPoints[i].AirFlow + (slope * (m_MAFFrequencyStep * 0.75f));
            }

            // set the left airflow of the first point and the right airflow of the last point to 0, as they are outside the defined frequency range
            // This is a design choice, as we don't have data points outside the defined frequency range to calculate these values.
            //
            mafDataPoints[0].AirFlowLeft = 0.0f;
            mafDataPoints[m_mafFrequencyCount - 1].AirFlowRight = 0.0f;
           

            MAF_dataGridView.ColumnCount = (int)m_mafFrequencyCount;
            MAF_dataGridView.RowCount = 2;


            for (int i = 0; i < m_mafFrequencyCount; i++)
            {
                MAF_dataGridView.Rows[0].Cells[i].Value = mafDataPoints[i].Frequency.ToString();
                MAF_dataGridView.Rows[1].Cells[i].Value = mafDataPoints[i].AirFlow.ToString("f3");

   
            }

        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuStrip1.SourceControl is TextBoxBase AirFlow_richTextBox)
            {
                AirFlow_richTextBox.Clear();
                AirFlow_richTextBox.Paste();
            }
            else if (contextMenuStrip1.SourceControl is TextBoxBase AdjustmentBuckets_richTextBox)
            {
                AdjustmentBuckets_richTextBox.Clear();
                AdjustmentBuckets_richTextBox.Paste();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] AdjustmentData;


            AdjustmentData =
                AdjustmentBuckets_richTextBox.Text.Split(new[] { ' ', '\t', ',', '\r', '\n' })
                .Select(s => double.TryParse(s, out var v) ? v : double.NaN)
                .ToArray();

            if (AdjustmentData.Length != m_BucketCount)
            {
                MessageBox.Show($"Adjustment data count must match bucket count {AdjustmentData.Length} != {m_BucketCount}");
                return;
            }

            //  the tune cycle object is the owner of the adjustment data, so we need to populate the adjustment data in the tune cycle object before we can read it out and apply it to the MafDataPoint array.
            //
            m_CurrentTuneCycle.PopulateAirflowAdjustment(AdjustmentData);

            for (int i = 0; i < mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref mafDataPoints[i];

                Current.AirFlowAdjusted = 0.0f;

                // read the adjustment data out of the tune cycle object and populate the MafDataPoint array with the adjustment values for each data point, based on the defined bucket style (single or double).
                //
                if ( m_BucketStyle == BucketStyleEnum.Single   )
                {
                    Current.AirFlowAdjustment = m_CurrentTuneCycle.GetAdjustmentDataAtIndex(i);
                }
                else
                {
                    Current.AirFlowLeftAdjustment = m_CurrentTuneCycle.GetAdjustmentDataAtIndex(i * 2);
                    Current.AirFlowRightAdjustment = m_CurrentTuneCycle.GetAdjustmentDataAtIndex((i * 2) + 1);

                }
            }
            ProcessAdjustmentData();




        }

        private void AdjustmentPercent_trackBar_Scroll(object sender, EventArgs e)
        {
            AdjustmentPercent_label.Text = $"{AdjustmentPercent_trackBar.Value}%";
        }

        

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Default: hide everything
            copyToolStripMenuItem.Visible = false;
            pasteToolStripMenuItem.Visible = false;

            if (contextMenuStrip1.SourceControl == Buckets_richTextBox)
            {
                // Output box: Copy only
                copyToolStripMenuItem.Visible = true;
                copyToolStripMenuItem.Enabled = Buckets_richTextBox.TextLength > 0;
            }
            else if (contextMenuStrip1.SourceControl == AirFlow_richTextBox)
            {
                // Input box: Paste only
                pasteToolStripMenuItem.Visible = true;
                pasteToolStripMenuItem.Enabled = Clipboard.ContainsText();
            }
            else if (contextMenuStrip1.SourceControl == AdjustmentBuckets_richTextBox)
            {
                // Input box: Paste only
                pasteToolStripMenuItem.Visible = true;
                pasteToolStripMenuItem.Enabled = Clipboard.ContainsText();
            }
            else if (contextMenuStrip1.SourceControl == AdjustedAirflow_dataGridView)
            {
                // Output box: Copy only
                copyToolStripMenuItem.Visible = true;
                copyToolStripMenuItem.Enabled = Buckets_richTextBox.TextLength > 0;
            }
        }

        private void CopyFourthRow(DataGridView dgv)
        {
            if (dgv.Rows.Count <= 3)
                return; // not enough rows

            var row = dgv.Rows[3];

            var values = row.Cells
                .Cast<DataGridViewCell>()
                .Select(c => c.Value?.ToString() ?? "");

            string text = string.Join("\t", values);
            Clipboard.SetText(text);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (contextMenuStrip1.SourceControl is RichTextBox rtb)
            {
                rtb.SelectAll();
                rtb.Copy();
            }
            else if (contextMenuStrip1.SourceControl == AdjustedAirflow_dataGridView)
            {
                CopyFourthRow(AdjustedAirflow_dataGridView);
            }


        }

        private void AdjustedAirflow_dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyFourthRow(AdjustedAirflow_dataGridView);
                e.Handled = true;
            }
        }

        private void ProcessAdjustmentData()
        {
            // This method can be used to process the adjustment data if needed, such as applying additional transformations or validations before updating the adjusted airflow values.
            // For now, the processing is done directly in the button1_Click event handler, but this method can be called from there if we want to separate concerns and keep the event handler cleaner.

            for (int i = 0; i < mafDataPoints.Length; i++)
            {
                if (m_BucketStyle == BucketStyleEnum.Single)
                {
                    mafDataPoints[i].AirFlowAdjusted = mafDataPoints[i].AirFlow
                                                        * (mafDataPoints[i].AirFlow * (mafDataPoints[i].AirFlowAdjustment / 100));

                }
                else
                {
                    //  For double bucket, we need to calculate the adjusted airflow for the left and right sides separately, and then average them to get the final adjusted airflow value for the bucket
                    //  This allows for more granular adjustments to the airflow values, as the user can specify different adjustments for the left and right sides of the bucket
                    //
                    double AdjustmentPercent = AdjustmentPercent_trackBar.Value / 100.0f;

                    double ModifiedAdjustmentPercentLeft = mafDataPoints[i].AirFlowLeftAdjustment * AdjustmentPercent;
                    double ModifiedAdjustmentPercentRight = mafDataPoints[i].AirFlowRightAdjustment * AdjustmentPercent;

                    mafDataPoints[i].AirFlowLeftAdjusted = mafDataPoints[i].AirFlowLeft + (mafDataPoints[i].AirFlowLeft * (ModifiedAdjustmentPercentLeft / 100));
                    mafDataPoints[i].AirFlowRightAdjusted = mafDataPoints[i].AirFlowRight + (mafDataPoints[i].AirFlowRight * (ModifiedAdjustmentPercentRight / 100));

                    mafDataPoints[i].AirFlowAdjusted = (mafDataPoints[i].AirFlowLeftAdjusted + mafDataPoints[i].AirFlowRightAdjusted) / 2.0f;


                }

                mafDataPoints[i].HasUpdatedAirFlow = !double.IsNaN(mafDataPoints[i].AirFlowAdjusted);

                if ((m_FirstUpdatedBucketIndex == -1) && !double.IsNaN(mafDataPoints[i].AirFlowAdjusted))
                {
                    //
                    //  Set the first updated bucket index to the first index where we have a valid adjusted airflow value
                    //
                    m_FirstUpdatedBucketIndex = i;
                }

                if (!double.IsNaN(mafDataPoints[i].AirFlowAdjusted))
                {
                    //
                    // Set the last updated bucket index to the last index where we have a valid adjusted airflow value
                    //
                    m_LastUpdatedBucketIndex = i;
                }
            }

            if (m_FirstUpdatedBucketIndex != -1)
            {
                //  We have at least one updated bucket, so we can adjust the airflow values for all buckets between the first and last updated bucket
                //

                for (int i = 0; i < m_FirstUpdatedBucketIndex; i++)
                {
                    //  If the current index is less than the first updated bucket index, set the adjusted airflow to the original airflow value of the first updated bucket
                    //
                    mafDataPoints[i].AirFlowAdjusted = mafDataPoints[i].AirFlow;

                }

            }

            if (m_LastUpdatedBucketIndex != -1)
            {
                for (int i = m_LastUpdatedBucketIndex + 1; i < m_mafFrequencyCount; i++)
                {
                    //  If the current index is greater than the last updated bucket index, set the adjusted airflow to the original airflow value of the last updated bucket
                    //
                    mafDataPoints[i].AirFlowAdjusted = mafDataPoints[i].AirFlow;
                }
            }

            for (int i = m_FirstUpdatedBucketIndex; i <= m_LastUpdatedBucketIndex; i++)
            {
                if (!mafDataPoints[i].HasUpdatedAirFlow)
                {
                    //  If we have a bucket that doesn't have an updated airflow value, but is between the first and last updated bucket, we need to interpolate the adjusted airflow value for that bucket
                    //
                    int j = i;
                    //  Find the next bucket index that has an updated airflow value
                    //
                    while (j <= m_LastUpdatedBucketIndex && !mafDataPoints[j].HasUpdatedAirFlow)
                    {
                        j++;
                    }
                    if (j > m_LastUpdatedBucketIndex)
                    {
                        //  We didn't find any more buckets with updated airflow values, so we can break out of the loop
                        //
                        break;
                    }
                    //  We found the next bucket index with an updated airflow value, so we can interpolate the adjusted airflow value for the current bucket
                    //
                    double slope = (mafDataPoints[j].AirFlowAdjusted - mafDataPoints[i - 1].AirFlowAdjusted) / (mafDataPoints[j].Frequency - mafDataPoints[i - 1].Frequency);
                    mafDataPoints[i].AirFlowAdjusted = mafDataPoints[i - 1].AirFlowAdjusted + (slope * (mafDataPoints[i].Frequency - mafDataPoints[i - 1].Frequency));
                }
            }

            if (AverageWithOriginal_checkBox.Checked)
            {
                // 
                //  If the user has selected to average with the original airflow values, we need to average the adjusted airflow values with the original airflow values for all buckets
                //
                for (int i = 0; i < mafDataPoints.Length; i++)
                {
                    mafDataPoints[i].AirFlowAdjusted = (mafDataPoints[i].AirFlow + mafDataPoints[i].AirFlowAdjusted) / 2.0f;
                }
            }

            AdjustedAirflow_dataGridView.ColumnCount = (int)m_mafFrequencyCount;
            AdjustedAirflow_dataGridView.RowCount = 4;

            for (int i = 0; i < mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref mafDataPoints[i];
                double ChangeAmountPercent = ((Current.AirFlowAdjusted - Current.AirFlow) / Current.AirFlow) * 100;

                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_FREQUENCY].Cells[i].Value = Current.Frequency.ToString();
                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_AIRFLOW].Cells[i].Value = Current.AirFlow.ToString("f3");
                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_AIRFLOW_ADJUSTMENT].Cells[i].Value = ChangeAmountPercent.ToString("f2");
                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_AIRFLOW_ADJUSTED].Cells[i].Value = Current.AirFlowAdjusted.ToString("f3");

                //
                // 
                if (Current.HasUpdatedAirFlow)
                {
                    //
                    // If we had apdated flow data, color the cell red/or greeen depending if it is more or less than
                    //  the original airflow
                    //
                    if (Current.AirFlow < Current.AirFlowAdjusted)
                    {
                        AdjustedAirflow_dataGridView.Rows[3].Cells[i].Style.BackColor = Color.LightGreen;
                    }
                    else if (Current.AirFlow > Current.AirFlowAdjusted)
                    {
                        AdjustedAirflow_dataGridView.Rows[3].Cells[i].Style.BackColor = Color.LightCoral;
                    }
                }
                else
                {
                    //  No updated data, color it yellow
                    //
                    AdjustedAirflow_dataGridView.Rows[3].Cells[i].Style.BackColor = Color.LightYellow;
                }
            }

        }

        private void create_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form2 frm = new Form2())
            {
                frm.ShowDialog(this);

                if (frm.DialogResult == DialogResult.OK)
                {
                    //  We can access the properties of the form to get the user input values and use them to populate the main form's controls or data structures as needed.
                    //
                   
                    m_MinMAFFrequency = frm.MinFrequency;   
                    m_MaxMAFFrequency = frm.MaxFrequency;   
                    m_MAFFrequencyStep = frm.FrequencyStep;

                    

                    m_mafFrequencyCount = (m_MaxMAFFrequency - m_MinMAFFrequency) / m_MAFFrequencyStep + 1;

                    mafDataPoints = new MafDataPoint[m_mafFrequencyCount];



                    for (uint i = 0; i < m_mafFrequencyCount; i++)
                    {
                        mafDataPoints[i].Frequency = (uint)(m_MinMAFFrequency + (i * m_MAFFrequencyStep));
                        mafDataPoints[i].AirFlow = 0.0f; // Placeholder for actual airflow values


                    }


                    Buckets_richTextBox.Clear();

                    for (int i = 0; i < m_mafFrequencyCount; i++)
                    {
                       
                        if (m_BucketStyle == BucketStyleEnum.Single)
                        {
                            Buckets_richTextBox.AppendText((mafDataPoints[i].Frequency - (m_MAFFrequencyStep / 2)).ToString());
                            Buckets_richTextBox.AppendText(" ");
                            m_BucketCount++;
                        }
                        else if (m_BucketStyle == BucketStyleEnum.Double)
                        {
                            Buckets_richTextBox.AppendText((mafDataPoints[i].Frequency - (m_MAFFrequencyStep / 2)).ToString());
                            Buckets_richTextBox.AppendText(" ");

                            Buckets_richTextBox.AppendText((mafDataPoints[i].Frequency).ToString());
                            Buckets_richTextBox.AppendText("  ");
                            m_BucketCount += 2;
                        }
                        else
                        {
                            Debug.Assert(true, "No bucket type");

                            return;

                        }
                    }


                    Process_button.Enabled = true;


                }
            }
        }
    }

}
