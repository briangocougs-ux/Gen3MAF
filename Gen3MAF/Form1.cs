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
        public const int DATA_GRID_ROW_ENABLE = 4;



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

        SessionClass m_SessionClass;

        TuneCycle m_CurrentTuneCycle;

        string m_CurrentFilePath = "";

        bool m_IsDirty;
        MafDataPoint[] m_mafDataPoints;

        //       int m_FirstUpdatedBucketIndex = -1;
        //       int m_LastUpdatedBucketIndex = -1;

        int m_MinMAFFrequency = 0;
        int m_MaxMAFFrequency = 0;
        int m_MAFFrequencyStep = 0;
        BucketStyleEnum m_BucketStyle = BucketStyleEnum.Double;

        uint m_BucketCount = 0;

        int m_mafFrequencyCount = 0;

        public Form1()
        {

            InitializeComponent();

            m_mafDataPoints = new MafDataPoint[1];

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

        private void UpdateTitle()
        {
            string fileName = m_CurrentFilePath != null
                ? Path.GetFileName(m_CurrentFilePath)
                : "Untitled";

            this.Text = $"MAF Tuning Tool - {fileName}" +
                        (m_IsDirty ? " *" : "");
        }

        void ResetStateOfForm()
        {
            m_CurrentTuneCycle = null;
            m_mafDataPoints = new MafDataPoint[1];
            //            m_FirstUpdatedBucketIndex = -1;
            //            m_LastUpdatedBucketIndex = -1;
            m_BucketCount = 0;
            m_mafFrequencyCount = 0;
            Buckets_richTextBox.Clear();
            AirFlow_richTextBox.Clear();
            AdjustmentBuckets_richTextBox.Clear();
            MAF_dataGridView.ColumnCount = 0;
            MAF_dataGridView.RowCount = 0;
            AdjustedAirflow_dataGridView.ColumnCount = 0;
            AdjustedAirflow_dataGridView.RowCount = 0;
            Process_button.Enabled = false;
            ApplyAdjustments.Enabled = false;
            AdjustmentPercent_trackBar.Enabled = false;
            
            CompleteCycle_button.Enabled = false;
        }

        private void Process_button_Click(object sender, EventArgs e)
        {
            string[] mafAirflowStrings;
            double[] mafAirflowValues = new double[m_mafFrequencyCount];



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
                m_mafDataPoints[i].AirFlow = m_CurrentTuneCycle.GetAirflowAtIndex((int)i);
            }

            for (int i = 0; i < (m_mafDataPoints.Length - 1); i++)
            {
                int j = i + 1;
                double slope = (m_mafDataPoints[j].AirFlow - m_mafDataPoints[i].AirFlow) / (m_mafDataPoints[j].Frequency - m_mafDataPoints[i].Frequency);

                m_mafDataPoints[i].AirFlowRight = m_mafDataPoints[i].AirFlow + (slope * (m_MAFFrequencyStep * 0.25f));
                m_mafDataPoints[j].AirFlowLeft = m_mafDataPoints[i].AirFlow + (slope * (m_MAFFrequencyStep * 0.75f));
            }

            // set the left airflow of the first point and the right airflow of the last point to 0, as they are outside the defined frequency range
            // This is a design choice, as we don't have data points outside the defined frequency range to calculate these values.
            //
            m_mafDataPoints[0].AirFlowLeft = 0.0f;
            m_mafDataPoints[m_mafFrequencyCount - 1].AirFlowRight = 0.0f;


            MAF_dataGridView.ColumnCount = (int)m_mafFrequencyCount;
            MAF_dataGridView.RowCount = 2;


            for (int i = 0; i < m_mafFrequencyCount; i++)
            {
                MAF_dataGridView.Rows[0].Cells[i].Value = m_mafDataPoints[i].Frequency.ToString();
                MAF_dataGridView.Rows[1].Cells[i].Value = m_mafDataPoints[i].AirFlow.ToString("f3");


            }

            ApplyAdjustments.Enabled = true;

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

            var values = AdjustmentData; // IEnumerable<double> from your paste parser
            var analysis = AnalyzePastedNumbers(values);

            if (LooksLikeCounts(analysis))
            {
                DialogResult result=MessageBox.Show(
                    $"This paste is {analysis.WholeRatio:P0} whole numbers " +
                    $"({analysis.WholeLikeCount}/{analysis.ValidCount}).\n\n" +
                    "That looks like *Histogram Counts*, not averaged values.\n" +
                    "In VCM Scanner, copy the 'Average' or 'Mean' values (not 'Count').",
                    "Possible wrong histogram data",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                    );

                if (result == DialogResult.Cancel)
                {
                    return;   // stop processing
                }
            }

            //  the tune cycle object is the owner of the adjustment data, so we need to populate the adjustment data in the tune cycle object before we can read it out and apply it to the MafDataPoint array.
            //
            m_CurrentTuneCycle.PopulateAirflowAdjustment(AdjustmentData);

            for (int i = 0; i < m_mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];

                Current.AirFlowAdjusted = 0.0f;

                // read the adjustment data out of the tune cycle object and populate the MafDataPoint array with the adjustment values for each data point, based on the defined bucket style (single or double).
                //
                if (m_BucketStyle == BucketStyleEnum.Single)
                {
                    Current.AirFlowAdjustment = m_CurrentTuneCycle.GetAdjustmentDataAtIndex(i);
                }
                else
                {
                    Current.AirFlowLeftAdjustment = m_CurrentTuneCycle.GetAdjustmentDataAtIndex(i * 2);
                    Current.AirFlowRightAdjustment = m_CurrentTuneCycle.GetAdjustmentDataAtIndex((i * 2) + 1);

                }
            }

            //
            //  enable the adjustment percent track bar and the complete cycle button, as we now have the necessary data to apply adjustments and complete the tuning cycle.
            //
            AdjustmentPercent_trackBar.Enabled = true;
            CompleteCycle_button.Enabled = true;    

            ProcessAdjustmentData();

        }

        private void AdjustmentPercent_trackBar_Scroll(object sender, EventArgs e)
        {
            AdjustmentPercent_label.Text = $"{AdjustmentPercent_trackBar.Value}%";
            ProcessAdjustmentData();
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
            int FirstUpdatedBucketIndex = -1;
            int LastUpdatedBucketIndex = -1;

            double AdjustmentPercent = AdjustmentPercent_trackBar.Value / 100.0f;

            // This method can be used to process the adjustment data if needed, such as applying additional transformations or validations before updating the adjusted airflow values.
            // For now, the processing is done directly in the button1_Click event handler, but this method can be called from there if we want to separate concerns and keep the event handler cleaner.

            double[] AdjustedAirflowArray = new double[m_mafDataPoints.Length];

            for (int i = 0; i < m_mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];

                Current.AirFlowAdjusted = 0.0;
                Current.AirFlowLeftAdjusted = 0.0;
                Current.AirFlowRightAdjusted = 0.0;
                Current.HasUpdatedAirFlow = false;

                if (m_BucketStyle == BucketStyleEnum.Single)
                {
                    //
                    //   single bucket, just apply the adjustment to the orignal airflow
                    //
                    double ModifiedAdjustmentPercent = Current.AirFlowLeftAdjustment * AdjustmentPercent;

                    Current.AirFlowAdjusted = Current.AirFlow
                                                        * (Current.AirFlow * (ModifiedAdjustmentPercent / 100));

                }
                else
                {
                    //  For double bucket, we need to calculate the adjusted airflow for the left and right sides separately, and then average them to get the final adjusted airflow value for the bucket
                    //  This allows for more granular adjustments to the airflow values, as the user can specify different adjustments for the left and right sides of the bucket
                    //

                    double ModifiedAdjustmentPercentLeft = Current.AirFlowLeftAdjustment * AdjustmentPercent;
                    double ModifiedAdjustmentPercentRight = Current.AirFlowRightAdjustment * AdjustmentPercent;

                    Current.AirFlowLeftAdjusted = Current.AirFlowLeft + (Current.AirFlowLeft * (ModifiedAdjustmentPercentLeft / 100));
                    Current.AirFlowRightAdjusted = Current.AirFlowRight + (Current.AirFlowRight * (ModifiedAdjustmentPercentRight / 100));

                    //
                    // since the left and right values are an equal distance from the target frequency, just average them together
                    //
                    Current.AirFlowAdjusted = (Current.AirFlowLeftAdjusted + Current.AirFlowRightAdjusted) / 2.0f;

                }

                //  
                //  if the adjusted airflow is NaN, it means we got not updated data from the scanner app. THis means no datapoints were collected
                //  for this frequency. '
                //  
                //  we will scan through the array to find the first and last frequncies with valid data. No adjustments will be applied to the value before and after
                //
                Current.HasUpdatedAirFlow = !double.IsNaN(Current.AirFlowAdjusted);

                if ((FirstUpdatedBucketIndex == -1) && !double.IsNaN(Current.AirFlowAdjusted))
                {
                    //
                    //  Set the first updated bucket index to the first index where we have a valid adjusted airflow value
                    //
                    FirstUpdatedBucketIndex = i;
                }

                if (!double.IsNaN(Current.AirFlowAdjusted))
                {
                    //
                    // Set the last updated bucket index to the last index where we have a valid adjusted airflow value
                    //
                    LastUpdatedBucketIndex = i;
                }
            }

            //
            //  Ajusted Airflow has been computed for the whole array;
            //   
            //  Now we will deal with parts that did not get updated air flow data
            //  Basically, just use the original data upto the first update value, Same after the last update value.
            //  If there are missing data points inbetween udated data points, interpolate
            //
            if (FirstUpdatedBucketIndex != -1)
            {
                //  We have at least one updated bucket, so we can adjust the airflow values for all buckets between the first and last updated bucket
                //

                for (int i = 0; i < FirstUpdatedBucketIndex; i++)
                {
                    ref MafDataPoint Current = ref m_mafDataPoints[i];
                    //
                    //  If the current index is less than the first updated bucket index, set the adjusted airflow to the original airflow value of the first updated bucket
                    //
                    Current.AirFlowAdjusted = Current.AirFlow;

                }

            }

            if (LastUpdatedBucketIndex != -1)
            {
                //
                //  We have atleast one updated data point, and we are at that index, just put the original flow back.
                //
                for (int i = LastUpdatedBucketIndex + 1; i < m_mafFrequencyCount; i++)
                {
                    ref MafDataPoint Current = ref m_mafDataPoints[i];

                    //
                    //  If the current index is greater than the last updated bucket index, set the adjusted airflow to the original airflow value of the last updated bucket
                    //
                    Current.AirFlowAdjusted = Current.AirFlow;
                }
            }

            //  now we will go through the region with some updated data points
            //  if there are gaps, interpolate between updated data points
            //
            for (int i = (FirstUpdatedBucketIndex + 1); i <= LastUpdatedBucketIndex; i++)
            {

                if (!m_mafDataPoints[i].HasUpdatedAirFlow)
                {
                    //  If we have a bucket that doesn't have an updated airflow value, but is between the first and last updated bucket, we need to interpolate the adjusted airflow value for that bucket
                    //
                    int j = i;
                    //  Find the next bucket index that has an updated airflow value
                    //
                    while (j <= LastUpdatedBucketIndex && !m_mafDataPoints[j].HasUpdatedAirFlow)
                    {
                        j++;
                    }
                    if (j > LastUpdatedBucketIndex)
                    {
                        //  We didn't find any more buckets with updated airflow values, so we can break out of the loop
                        //
                        break;
                    }

                    //  i is the data point with missing updated airflow value, i-1 is the last data point with an updated airflow value, and j is the next data point with an updated airflow value. We can use these three points to interpolate the adjusted airflow value for the current bucket at index i.    
                    //  We will use linear interpolation to calculate the adjusted airflow value for the current bucket at index i, based on the adjusted airflow values of the buckets at index i-1 and j, and the frequencies of those buckets.   

                    double slope = (m_mafDataPoints[j].AirFlowAdjusted - m_mafDataPoints[i - 1].AirFlowAdjusted)
                                   / (m_mafDataPoints[j].Frequency - m_mafDataPoints[i - 1].Frequency);


                    for (int k = i; k < j; k++)
                    {
                        m_mafDataPoints[k].AirFlowAdjusted = m_mafDataPoints[i - 1].AirFlowAdjusted
                                                             + (slope * (m_mafDataPoints[k].Frequency - m_mafDataPoints[i - 1].Frequency));

                    }

                    //  Move the index i to the next bucket index with an updated airflow value, which is j, so that we can continue processing the next buckets with missing updated airflow values
                    //

                    i = j;

                }
            }

            
            // build array to send to tune cycle object
            //
            for (int i = 0; i < m_mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];

                AdjustedAirflowArray[i] = Current.AirFlowAdjusted;
            }

            m_CurrentTuneCycle.PopulatedAdjustedAirflow(AdjustedAirflowArray);

            UpdateAdjustedAirflowGrid();

            return;
        }

        void UpdateAdjustedAirflowGrid()
        {
            AdjustedAirflow_dataGridView.ColumnCount = (int)m_mafFrequencyCount;
            AdjustedAirflow_dataGridView.RowCount = 5;

            for (int i = 0; i < m_mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];

                double ChangeAmountPercent = ((Current.AirFlowAdjusted - Current.AirFlow) / Current.AirFlow) * 100;

                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_FREQUENCY].Cells[i].Value = Current.Frequency.ToString();
                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_AIRFLOW].Cells[i].Value = Current.AirFlow.ToString("f3");
                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_AIRFLOW_ADJUSTMENT].Cells[i].Value = ChangeAmountPercent.ToString("f2");
                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_AIRFLOW_ADJUSTED].Cells[i].Value = Current.AirFlowAdjusted.ToString("f3");

                var cell = new DataGridViewCheckBoxCell
                {
                    ThreeState = false,
                    Value = true,          // default: apply adjustments everywhere
                    Style = { Alignment = DataGridViewContentAlignment.MiddleCenter }
                };

                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_ENABLE].Cells[i] = cell;


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
            using (Vehicle frm = new Vehicle())
            {
                frm.ShowDialog(this);

                if (frm.DialogResult == DialogResult.OK)
                {
                    //  We can access the properties of the form to get the user input values and use them to populate the main form's controls or data structures as needed.
                    //

                    m_SessionClass = frm.GetSessionInfo();

                    m_MinMAFFrequency = m_SessionClass.MinFrequency;
                    m_MaxMAFFrequency = m_SessionClass.MaxFrequency;
                    m_MAFFrequencyStep = m_SessionClass.FrequencyStep;
                    m_BucketStyle = m_SessionClass.BucketStyle;

                    tuneToolStripMenuItem.Enabled = true;

                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  
            //  disable the buttons. They will be enable as the user goes through the process of creating a new tune cycle. This is to prevent the user from trying to process data before we have the necessary information to do so, such as the maf frequency points and the bucket style.
            //

            ApplyAdjustments.Enabled = false;
            CompleteCycle_button.Enabled = false;

            m_mafFrequencyCount = (m_MaxMAFFrequency - m_MinMAFFrequency) / m_MAFFrequencyStep + 1;

            m_mafDataPoints = new MafDataPoint[m_mafFrequencyCount];



            for (uint i = 0; i < m_mafFrequencyCount; i++)
            {
                m_mafDataPoints[i].Frequency = (uint)(m_MinMAFFrequency + (i * m_MAFFrequencyStep));
                m_mafDataPoints[i].AirFlow = 0.0f; // Placeholder for actual airflow values

            }

            //
            // fill the text box for buckets so the user can paste them into the tuning app.
            //  all we need to know is the frequency values
            //
            Buckets_richTextBox.Clear();
           

            for (int i = 0; i < m_mafFrequencyCount; i++)
            {

                if (m_BucketStyle == BucketStyleEnum.Single)
                {
                    Buckets_richTextBox.AppendText((m_mafDataPoints[i].Frequency - (m_MAFFrequencyStep / 2)).ToString());
                    Buckets_richTextBox.AppendText(" ");
                    m_BucketCount++;
                }
                else if (m_BucketStyle == BucketStyleEnum.Double)
                {
                    //  for double buckets we create two buckets for each frequncy. The first bucket starts half the distance to the previous frequnce.
                    //  The second bucket starts at this frequency and goes the midpoint half way to the next frequency. The average point is half way in
                    //  the span of the bucket
                    //
                    Buckets_richTextBox.AppendText((m_mafDataPoints[i].Frequency - (m_MAFFrequencyStep / 2)).ToString());
                    Buckets_richTextBox.AppendText(" ");

                    Buckets_richTextBox.AppendText((m_mafDataPoints[i].Frequency).ToString());
                    Buckets_richTextBox.AppendText("  ");
                    m_BucketCount += 2;
                }
                else
                {
                    Debug.Assert(true, "unknown bucket type");

                    return;

                }
            }

            m_CurrentTuneCycle = m_SessionClass.CreateNewTuneCycle(m_mafFrequencyCount, (int)m_BucketCount);

            Process_button.Enabled = true;

            //
            //  it there is a previos tune cycle, populate the maf data in the text box
            //

            TuneCycle PreviousTuneCycle = null;
            try
            {
                PreviousTuneCycle = m_SessionClass.GetLastTuneCycle();
            }
            catch (Exception)
            {
                //  No previous tune cycle, ignore
                return;
            }

            if (PreviousTuneCycle != null)
            {
                if (PreviousTuneCycle.IsCompleted())
                {
                    double[] PreviousAirflow = new double[m_mafFrequencyCount];

                    for (int i = 0; i < m_mafFrequencyCount; i++)
                    {
                        PreviousAirflow[i] = PreviousTuneCycle.GetAdjustedAirflowAtIndex(i);
                    }

                    AirFlow_richTextBox.Text = string.Join("\t", PreviousAirflow.Select(v => v.ToString()));
                }
            }
            return;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_SessionClass == null)
                return;

            if (string.IsNullOrEmpty(m_CurrentFilePath))
            {
                saveAsToolStripMenuItem_Click(sender, e);
                return;
            }

            SessionFileStore.Save(m_CurrentFilePath, m_SessionClass);
            m_IsDirty = false;
            UpdateTitle();
        }

        private void CompleteCycle_button_Click(object sender, EventArgs e)
        {
            m_CurrentTuneCycle.MarkAsCompleted(AdjustmentPercent_trackBar.Value, false);

            m_SessionClass.AddTuneCycle(m_CurrentTuneCycle);
            m_CurrentTuneCycle = null;
            Process_button.Enabled = false;
            ApplyAdjustments.Enabled = false;
            m_IsDirty = true;
            ResetStateOfForm();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_SessionClass == null)
                return;

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "MAF Session (*.json)|*.json|All Files (*.*)|*.*";
                dlg.Title = "Save Session As";
                dlg.DefaultExt = "json";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_CurrentFilePath = dlg.FileName;
                    SessionFileStore.Save(m_CurrentFilePath, m_SessionClass);
                    m_IsDirty = false;

                    UpdateTitle();
                }
            }
        }

        private void open_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardIfDirty())
                return;

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "MAF Session (*.json)|*.json|All Files (*.*)|*.*";
                dlg.Title = "Open Session";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_SessionClass = SessionFileStore.Load(dlg.FileName);
                    m_CurrentFilePath = dlg.FileName;
                    m_IsDirty = false;

                    UpdateTitle();

                    m_MinMAFFrequency = m_SessionClass.MinFrequency;
                    m_MaxMAFFrequency = m_SessionClass.MaxFrequency;
                    m_MAFFrequencyStep = m_SessionClass.FrequencyStep;
                    m_BucketStyle = m_SessionClass.BucketStyle;

                    tuneToolStripMenuItem.Enabled = true;
                }
            }
        }
        private bool ConfirmDiscardIfDirty()
        {
            if (!m_IsDirty)
                return true;

            var result = MessageBox.Show(
                "You have unsaved changes. Save now?",
                "Unsaved Changes",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Cancel)
                return false;

            if (result == DialogResult.Yes)
                saveToolStripMenuItem_Click(this, EventArgs.Empty);

            return true;
        }

        private void AverageWithOriginal_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            ProcessAdjustmentData();
        }

        private void AdjustedAirflow_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private static bool IsNearlyWhole(double x, double tol = 1e-6)
        {
            // handles negative too; tol is absolute error from nearest integer
            return Math.Abs(x - Math.Round(x)) <= tol;
        }

        public sealed record WholeNumberHeuristicResult(
            int ValidCount,
            int WholeLikeCount,
            int ZeroLikeCount,
            double WholeRatio
        );

        public static WholeNumberHeuristicResult AnalyzePastedNumbers(IEnumerable<double> values)
        {
            int valid = 0, whole = 0, zero = 0;

            foreach (var v in values)
            {
                if (double.IsNaN(v) || double.IsInfinity(v))
                    continue;

                valid++;

                if (IsNearlyWhole(v))
                    whole++;

                if (Math.Abs(v) <= 1e-9) // treat "0" or "0.0" as zero
                    zero++;
            }

            double ratio = valid > 0 ? (double)whole / valid : 0.0;
            return new WholeNumberHeuristicResult(valid, whole, zero, ratio);
        }

        public static bool LooksLikeCounts(WholeNumberHeuristicResult r)
        {
            // Tune thresholds to your data:
            // - counts paste: almost all integers and often many zeros
            // - averages paste: lots of fractional values
            if (r.ValidCount < 8) return false;               // too little data to judge
            if (r.WholeRatio < 0.90) return false;            // not "mostly integer"
            if (r.ZeroLikeCount < 2) return true;             // integer-heavy but not many zeros -> still warn
            return true;
        }
    }

}
