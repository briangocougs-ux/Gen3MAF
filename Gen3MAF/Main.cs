using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Design;
using static Gen3MAF.Main;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Gen3MAF
{
    public partial class Main : Form
    {
        public const uint MIN_MAF_FREQUENCY = 1500;
        public const uint MAX_MAF_FREQUENCY = 12000;
        public const uint MAF_FREQUENCY_STEP = 125;

        public const int DATA_GRID_ROW_FREQUENCY = 0;
        public const int DATA_GRID_ROW_AIRFLOW = 1;
        public const int DATA_GRID_ROW_AIRFLOW_ADJUSTMENT = 2;
        public const int DATA_GRID_ROW_AIRFLOW_ADJUSTED = 3;
        public const int DATA_GRID_ROW_ENABLE = 4;
        public const int DEFAULT_ADJUSTMENT_PERCENT = 100;
        public const double DEFAULT_THRESHOLD_VALUE = 0.10;
        public const double THRESHOLD_TRACKBAR_TICK_VALUE = 0.05;




        SessionClass m_SessionClass;

        TuneCycle m_CurrentTuneCycle;
        bool m_TuneCycleReOpened = false;

        string m_CurrentFilePath = "";


        AdjustClass m_AdjustObject;

        double m_AdjustThreshold = DEFAULT_THRESHOLD_VALUE;

        ToolStripMenuItem _tuneCyclesMenu;

        private readonly ToolStripSeparator _dynSep = new ToolStripSeparator();

        public Main()
        {

            InitializeComponent();

            m_AdjustObject = null;

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


            ResetStateOfForm();


        }

        private void UpdateTitle()
        {
            string fileName = m_CurrentFilePath != null
                ? Path.GetFileName(m_CurrentFilePath)
                : "Untitled";

            this.Text = $"MAF Tuning Tool - {fileName}" +
                        (m_SessionClass.IsDirty() ? " *" : "");
        }

        void ResetStateOfForm()
        {

            Buckets_richTextBox.Clear();
            AirFlow_richTextBox.Clear();
            AdjustmentBuckets_richTextBox.Clear();

            MAF_dataGridView.ColumnCount = 0;
            MAF_dataGridView.RowCount = 0;

            AdjustedAirflow_dataGridView.ColumnCount = 0;
            AdjustedAirflow_dataGridView.RowCount = 0;

            ProcessOriginalAirflow_button.Enabled = false;
            ApplyAdjustments.Enabled = false;

            Pause_button.Enabled = false;
            Plot_button.Enabled = false;
            Discard_button.Enabled = false;
            GetAirFlowFromLast_button.Enabled = false;

            AdjustmentPercent_trackBar.Enabled = false;
            AdjustmentPercent_trackBar.Value = DEFAULT_ADJUSTMENT_PERCENT;
            AdjustmentPercent_label.Text = $"{AdjustmentPercent_trackBar.Value}%";

            m_AdjustThreshold = DEFAULT_THRESHOLD_VALUE;
            AdjustmentThreshold_trackBar.Enabled = false;
            AdjustmentThreshold_trackBar.Value = (int)(m_AdjustThreshold / THRESHOLD_TRACKBAR_TICK_VALUE);
            ThresholdValue_label.Text = m_AdjustThreshold.ToString("f2") + "%";

            InterpolateMissingData_checkBox.Enabled = false;
            InterpolateMissingData_checkBox.Checked = true;

            MinFrequency_trackBar.Enabled = false;
            MaxFrequency_trackBar.Enabled = false;


            CompleteCycle_button.Enabled = false;
        }

        void InitializeFormForNewSession(SessionClass NewSession)
        {

            ResetStateOfForm();

            m_SessionClass = NewSession;

            InitRangeTrackBars(m_SessionClass.MinFrequency, m_SessionClass.MaxFrequency, m_SessionClass.FrequencyStep);

            m_AdjustObject = new AdjustClass(m_SessionClass.MinFrequency, m_SessionClass.MaxFrequency, m_SessionClass.FrequencyStep, m_SessionClass.BucketStyle);

            return;

        }

        private void Process_button_Click(object sender, EventArgs e)
        {
            string[] mafAirflowStrings;
            double[] mafAirflowValues = new double[m_AdjustObject.GetFrequencyCount()];

            //
            //  user enter airflow value in the text box, parse them and put them in an array of doubles
            //


            mafAirflowStrings = AirFlow_richTextBox.Text.Split(
                                                    new[] { ' ', '\t', ',', '\r', '\n' },
                                                    StringSplitOptions.RemoveEmptyEntries
                                                    );
            if (mafAirflowStrings.Length != m_AdjustObject.GetFrequencyCount())
            {
                MessageBox.Show($"Please enter {m_AdjustObject.GetFrequencyCount()} airflow values, separated by spaces, tabs, commas, or newlines.");
                return;
            }

            //
            //  Validate and parse airflow values
            //
            try
            {
                for (uint i = 0; i < m_AdjustObject.GetFrequencyCount(); i++)
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

            ProcessOriginalAirflowData();

        }

        void ProcessOriginalAirflowData()
        {
            //
            //  Give the tune cycle to the adjust object
            //

            m_AdjustObject.InitializeAirFlowFromTuneObject(m_CurrentTuneCycle);


            //  
            //  fill the maf data grid view with the original airflow values for each frequency point, so the user can see the original data and compare it to the adjusted data as they make adjustments.
            //
            MAF_dataGridView.ColumnCount = (int)m_AdjustObject.GetFrequencyCount(); ;
            MAF_dataGridView.RowCount = 2;


            for (int i = 0; i < m_AdjustObject.GetFrequencyCount(); i++)
            {
                ReturnDataPoint DataPoint;

                DataPoint = m_AdjustObject.GetDataPointAtIndex(i);

                MAF_dataGridView.Rows[0].Cells[i].Value = DataPoint.Frequency.ToString();
                MAF_dataGridView.Rows[1].Cells[i].Value = DataPoint.Airflow.ToString("f3");
            }

            ApplyAdjustments.Enabled = true;
            Pause_button.Enabled = true;

            return;
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

            // 
            //  parse out the adjustment data from the text box
            //
            AdjustmentData =
                AdjustmentBuckets_richTextBox.Text.Split(new[] { ' ', '\t', ',', '\r', '\n' })
                .Select(s => double.TryParse(s, out var v) ? v : double.NaN)
                .ToArray();

            if (AdjustmentData.Length != m_AdjustObject.GetBucketCount())
            {
                MessageBox.Show($"Adjustment data count must match bucket count {AdjustmentData.Length} != {m_AdjustObject.GetBucketCount()}");
                return;
            }

            var values = AdjustmentData; // IEnumerable<double> from your paste parser
            var analysis = AnalyzePastedNumbers(values);

            if (LooksLikeCounts(analysis))
            {
                DialogResult result = MessageBox.Show(
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

            m_AdjustObject.ReadAdjustmentDataFromTuneObject(m_CurrentTuneCycle);


            //
            //  enable the adjustment percent track bar and the complete cycle button, as we now have the necessary data to apply adjustments and complete the tuning cycle.
            //
            AdjustmentPercent_trackBar.Enabled = true;
            AdjustmentThreshold_trackBar.Enabled = true;
            CompleteCycle_button.Enabled = true;
            Plot_button.Enabled = true;
            InterpolateMissingData_checkBox.Enabled = true;
            MinFrequency_trackBar.Enabled = true;
            MaxFrequency_trackBar.Enabled = true;

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
            double AdjustmentPercent = AdjustmentPercent_trackBar.Value / 100.0f;

            bool InterpolateMissingData = InterpolateMissingData_checkBox.Checked;
            int MinFrequency = (MinFrequency_trackBar.Value * m_SessionClass.FrequencyStep) + m_SessionClass.MinFrequency;
            int MaxFrequency = (MaxFrequency_trackBar.Value * m_SessionClass.FrequencyStep) + m_SessionClass.MinFrequency;

            if (m_AdjustObject != null)
            {

                m_AdjustObject.ProcessAdjustmentData(
                    AdjustmentPercent,
                    m_AdjustThreshold,
                    InterpolateMissingData,
                    MinFrequency,
                    MaxFrequency
                    );


                UpdateAdjustedAirflowGrid();
            }

            return;
        }

        void UpdateAdjustedAirflowGrid()
        {
            AdjustedAirflow_dataGridView.ColumnCount = (int)m_AdjustObject.GetFrequencyCount(); ;
            AdjustedAirflow_dataGridView.RowCount = 5;

            for (int i = 0; i < m_AdjustObject.GetFrequencyCount(); i++)
            {
                ReturnDataPoint DataPoint = m_AdjustObject.GetDataPointAtIndex(i);


                double ChangeAmountPercent = ((DataPoint.AdjustedAirflow - DataPoint.Airflow) / DataPoint.Airflow) * 100;

                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_FREQUENCY].Cells[i].Value = DataPoint.Frequency.ToString();
                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_AIRFLOW].Cells[i].Value = DataPoint.Airflow.ToString("f3");
                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_AIRFLOW_ADJUSTMENT].Cells[i].Value = ChangeAmountPercent.ToString("f2");
                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_AIRFLOW_ADJUSTED].Cells[i].Value = DataPoint.AdjustedAirflow.ToString("f3");
#if false
                var cell = new DataGridViewCheckBoxCell
                {
                    ThreeState = false,
                    Value = true,          // default: apply adjustments everywhere
                    Style = { Alignment = DataGridViewContentAlignment.MiddleCenter }
                };

                AdjustedAirflow_dataGridView.Rows[DATA_GRID_ROW_ENABLE].Cells[i] = cell;
#endif

                //
                // 
                if (DataPoint.HasUpdatedAirflow)
                {
                    //
                    // If we had apdated flow data, color the cell red/or greeen depending if it is more or less than
                    //  the original airflow
                    //
                    if (DataPoint.Airflow < DataPoint.AdjustedAirflow)
                    {
                        AdjustedAirflow_dataGridView.Rows[3].Cells[i].Style.BackColor = Color.LightGreen;
                    }
                    else if (DataPoint.Airflow > DataPoint.AdjustedAirflow)
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
                    SessionClass Session = null;

                    Session = frm.GetSessionInfo();

                    InitializeFormForNewSession(Session);

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

            

            //
            //  create a new tunecycle object
            //
            m_CurrentTuneCycle = m_SessionClass.CreateNewTuneCycle(m_AdjustObject.GetFrequencyCount(), (int)m_AdjustObject.GetBucketCount());

            //
            //  it there is a previous tune cycle, Enable a button to populate the maf data in the text box
            //

            if (m_SessionClass.HasCompletedTuneCycle())
            {
                GetAirFlowFromLast_button.Enabled = true;
            }

            InitializeBucketsTextbox();

            ProcessOriginalAirflow_button.Enabled = true;
            Discard_button.Enabled = true;

            return;
        }

        private void Continue_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //  
            //  disable the buttons. They will be enable as the user goes through the process of creating a new tune cycle. This is to prevent the user from trying to process data before we have the necessary information to do so, such as the maf frequency points and the bucket style.
            //

            ApplyAdjustments.Enabled = false;
            CompleteCycle_button.Enabled = false;


            m_CurrentTuneCycle = m_SessionClass.RemoveLastTuneCycle();
            m_CurrentTuneCycle.ChangePausedToAirflowPopulated();

            InitializeBucketsTextbox();

            double[] OriginalAirflow = new double[m_AdjustObject.GetFrequencyCount()];

            for (int i = 0; i < m_AdjustObject.GetFrequencyCount(); i++)
            {
                OriginalAirflow[i] = m_CurrentTuneCycle.GetAirflowAtIndex(i);
            }

            AirFlow_richTextBox.Text = string.Join("\t", OriginalAirflow.Select(v => v.ToString()));

            ProcessOriginalAirflowData();
        }

        void InitializeBucketsTextbox()
        {
 
            //
            // fill the text box for buckets so the user can paste them into the tuning app.
            //  all we need to know is the frequency values
            //
            Buckets_richTextBox.Clear();


            for (int i = 0; i < m_AdjustObject.GetBucketCount(); i++)
            {
                uint Bucket = m_AdjustObject.GetBucketAtIndex(i);

                Buckets_richTextBox.AppendText(Bucket.ToString());
                Buckets_richTextBox.AppendText(" ");

                if (m_SessionClass.BucketStyle == BucketStyleEnum.Single)
                {


                }
                else if (m_SessionClass.BucketStyle == BucketStyleEnum.Double)
                {
                    //  for double buckets we create two buckets for each frequncy. The first bucket starts half the distance to the previous frequnce.
                    //  The second bucket starts at this frequency and goes the midpoint half way to the next frequency. The average point is half way in
                    //  the span of the bucket
                    //
                    if ((i+1) % 2 == 0)
                    {
                        //  group them by 2s so we have a visual separation in the text box for each frequency point, as each frequency point will have 2 buckets for double bucket style and 3 buckets for triple bucket style. This is just for visual clarity for the user when they are looking at the buckets in the text box and comparing them to the frequencies in the maf data grid view.
                        //
                        Buckets_richTextBox.AppendText(" ");

                    }

                }
                else if (m_SessionClass.BucketStyle == BucketStyleEnum.Triple)
                {
                    if ((i+1) % 3 == 0)
                    {
                        //
                        //  group them by 3 so you can see which frequency the group belongs to
                        //
                        Buckets_richTextBox.AppendText(" ");

                    }

                }
                else
                {
                    Debug.Assert(true, "unknown bucket type");

                    return;

                }
            }
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

            UpdateTitle();
        }

        private void CompleteCycle_button_Click(object sender, EventArgs e)
        {


            if (!m_TuneCycleReOpened)
            {
                double[] AdjustedAirflowArray = new double[m_AdjustObject.GetFrequencyCount()];

                //
                // build array to send to tune cycle object
                //
                for (int i = 0; i < m_AdjustObject.GetFrequencyCount(); i++)
                {
                    ReturnDataPoint DataPoint;

                    DataPoint = m_AdjustObject.GetDataPointAtIndex(i);

                    AdjustedAirflowArray[i] = DataPoint.AdjustedAirflow;
                }

                m_CurrentTuneCycle.PopulateAdjustedAirflow(AdjustedAirflowArray);

                m_CurrentTuneCycle.MarkAsCompleted(AdjustmentPercent_trackBar.Value, false);

                m_SessionClass.AddTuneCycle(m_CurrentTuneCycle);
            }

            m_TuneCycleReOpened = false;
            m_CurrentTuneCycle = null;

            ProcessOriginalAirflow_button.Enabled = false;
            ApplyAdjustments.Enabled = false;
            plotAllToolStripMenuItem.Enabled = true;
            tuneToolStripMenuItem.Enabled = true;
            ResetStateOfForm();
        }

        private void Discard_button_Click(object sender, EventArgs e)
        {
            m_TuneCycleReOpened = false;
            m_CurrentTuneCycle = null;
            ProcessOriginalAirflow_button.Enabled = false;
            ApplyAdjustments.Enabled = false;
            tuneToolStripMenuItem.Enabled = true;
            ResetStateOfForm();
        }

        private void Pasue_button_Click(object sender, EventArgs e)
        {

            m_CurrentTuneCycle.MarkAsPaused();
            m_SessionClass.AddTuneCycle(m_CurrentTuneCycle);


            m_CurrentTuneCycle = null;
            ProcessOriginalAirflow_button.Enabled = false;
            ApplyAdjustments.Enabled = false;

            // 
            //  if the user pauses the tune cycle, we want to enable the continue menu and disable the new menu, as they can either continue where they left off or start a new tune cycle, but they can't do both. 
            //
            Continue_ToolStripMenuItem.Enabled = true;
            NewTuneCycle_toolStripMenuItem.Enabled = false;

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


                    UpdateTitle();
                }
            }
        }

        private void open_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_SessionClass != null)
            {
                if (!ConfirmDiscardIfDirty())
                {
                    return;
                }
            }
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "MAF Session (*.json)|*.json|All Files (*.*)|*.*";
                dlg.Title = "Open Session";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SessionClass Session = null;

                    Session = SessionFileStore.Load(dlg.FileName);
                    m_CurrentFilePath = dlg.FileName;

                    InitializeFormForNewSession(Session);

                    UpdateTitle();


                    //
                    // assume it is a new session and enable menu here, if there is a previous tune cycle, we will adjust the menu state in the code below  
                    //
                    NewTuneCycle_toolStripMenuItem.Enabled = true;
                    Continue_ToolStripMenuItem.Enabled = false;

                    TuneCycle PreviousTuneCycle = null;

                    PreviousTuneCycle = m_SessionClass.GetLastTuneCycle();


                    if (PreviousTuneCycle != null)
                    {
                        if (PreviousTuneCycle.IsPaused())
                        {
                            //
                            //  the last tune cycle was paused, enable the continue menu and disable new menu
                            //

                            Continue_ToolStripMenuItem.Enabled = true;
                            NewTuneCycle_toolStripMenuItem.Enabled = false;

                        }
                    }

                    // 
                    //  enable the top level menu in any case
                    //
                    tuneToolStripMenuItem.Enabled = true;
                    if (m_SessionClass.HasCompletedTuneCycle())
                    {
                        plotAllToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        plotAllToolStripMenuItem.Enabled = false;
                    }

                }
            }
        }
        private bool ConfirmDiscardIfDirty()
        {
            if (!m_SessionClass.IsDirty())
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
            //
            if (r.ValidCount < 8) return false;               // too little data to judge
            if (r.WholeRatio < 0.90) return false;            // not "mostly integer"
            if (r.ZeroLikeCount < 2) return true;             // integer-heavy but not many zeros -> still warn
            return true;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_CurrentTuneCycle != null)
            {
                DialogResult result;

                result = MessageBox.Show("There is a tuning session active, Do you really want to exit?",
                                         "Closing Confirm",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Hand
                                         );

                if (result == DialogResult.No)
                {
                    e.Cancel = true;   // Stops the form from closing
                    return;
                }

            }

            if (m_SessionClass != null)
            {

                ConfirmDiscardIfDirty();


            }


        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Plot_button_Click(object sender, EventArgs e)
        {


            double[] x = new double[m_AdjustObject.GetFrequencyCount()];
            double[] y = new double[m_AdjustObject.GetFrequencyCount()];

            for (int i = 0; i < m_AdjustObject.GetFrequencyCount(); i++)
            {
                ReturnDataPoint DataPoint = m_AdjustObject.GetDataPointAtIndex(i);

                x[i] = DataPoint.Frequency;
                y[i] = ((DataPoint.AdjustedAirflow - DataPoint.Airflow) / DataPoint.Airflow) * 100.0;
            }

            var plot = new PlotForm1(x, y);

            plot.ShowDialog();
        }

        private void plotAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] freq = new double[m_AdjustObject.GetFrequencyCount()];
            double[] baseAir = new double[m_AdjustObject.GetFrequencyCount()];
            var adjusted = new List<double[]>();

            string[] labels = new string[m_SessionClass.GetTuneCycleCount()];


            //
            //  Create an AdjustClass instance to  to use to recompute the adjusted airflow values based on the original airflow and adjustment data stored in the selected tune cycle. This allows us to display the original, adjusted, and base airflow values for comparison in the plot.
            //  
            //

            for (int i = 0; i < m_SessionClass.GetTuneCycleCount(); i++)
            {
                var tc = m_SessionClass.GetTuneCycleAtIndex(i);
                labels[i] = tc.GetTimeStamp().ToLocalTime().ToString();

                AdjustClass AdjustObject = new AdjustClass(m_SessionClass.MinFrequency, m_SessionClass.MaxFrequency, m_SessionClass.FrequencyStep, m_SessionClass.BucketStyle);

                //
                //  recompute the adjusted airflow values based on the original airflow and adjustment data stored in the selected tune cycle. This allows us to display the original, adjusted, and base airflow values for comparison in the plot.
                // 
                AdjustObject.InitializeAirFlowFromTuneObject(tc);
                AdjustObject.ReadAdjustmentDataFromTuneObject(tc);
                AdjustObject.ProcessAdjustmentData(
                    1.0,
                    m_AdjustThreshold,
                    true,
                    m_SessionClass.MinFrequency,
                    m_SessionClass.MaxFrequency
                    );

                double[] newAir = new double[m_AdjustObject.GetFrequencyCount()];

                for (int j = 0; j < m_AdjustObject.GetFrequencyCount(); j++)
                {
                    ReturnDataPoint Point = AdjustObject.GetDataPointAtIndex(j);

                    if (i == 0)
                    {
                        //  capure the base airflow from index 0, we will get the frequency as well while we are here
                        //
                        freq[j] = (double)Point.Frequency;
                        baseAir[j] = Point.Airflow;
                    }

                    newAir[j] = Point.AdjustedAirflow;
                }

                // add the adjusted airflow values for this tune cycle to the list of adjusted airflow arrays, which we will use to plot all the tune cycles together for comparison.
                //
                adjusted.Add(newAir);




            }

            var f = new PlotForm1(
                freq,
                baseAir,
                adjusted,
                labels
                );

            f.Show(this);
        }

        private void MAF_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("cell content click in maf datagrid");
        }

        private void MAF_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;
            MessageBox.Show("cell click in maf datagrid");

        }

        private void MAF_dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;

            MessageBox.Show("cell double click in maf datagrid");
            DataPointForm form = new DataPointForm(m_AdjustObject.GetFullDataPointAtIndex(col));

            form.ShowDialog(this);
        }

        private void MAF_dataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("mouse double click in maf datagrid");
        }

        private void MAF_dataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            var hit = MAF_dataGridView.HitTest(e.X, e.Y);
            MessageBox.Show($"Hit: {hit.Type}, r={hit.RowIndex}, c={hit.ColumnIndex}");
        }

        private void AdjustedAirflow_dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;


            DataPointForm form = new DataPointForm(m_AdjustObject.GetFullDataPointAtIndex(col));

            form.ShowDialog(this);
        }

        private void AdjustmentThreshold_trackBar_Scroll(object sender, EventArgs e)
        {
            m_AdjustThreshold = ((double)AdjustmentThreshold_trackBar.Value * THRESHOLD_TRACKBAR_TICK_VALUE);
            ThresholdValue_label.Text = m_AdjustThreshold.ToString("f2") + "%";

            ProcessAdjustmentData();
        }

        private void GetAirFlowFromLast_button_Click(object sender, EventArgs e)
        {
            TuneCycle PreviousTuneCycle = null;

            PreviousTuneCycle = m_SessionClass.GetLastTuneCycle();

            if (PreviousTuneCycle != null)
            {
                if (PreviousTuneCycle.IsCompleted())
                {
                    double[] PreviousAirflow = new double[m_AdjustObject.GetFrequencyCount()];

                    for (int i = 0; i < m_AdjustObject.GetFrequencyCount(); i++)
                    {
                        PreviousAirflow[i] = PreviousTuneCycle.GetAdjustedAirflowAtIndex(i);
                    }

                    AirFlow_richTextBox.Text = string.Join("\t", PreviousAirflow.Select(v => v.ToString()));
                }
            }
        }

        private void AirFlow_richTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ThresholdValue_label_Click(object sender, EventArgs e)
        {

        }

        private void InterpolateMissingData_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            ProcessAdjustmentData();
        }

        private void PlotRaw_button_Click(object sender, EventArgs e)
        {
            double[] Frequency = new double[1];
            double[] AirFlow = new double[1];
            double[] AdjustedAirflow = new double[1];
            var adjusted = new List<double[]>();



            m_AdjustObject.ReadRawAirflowData(ref Frequency, ref AirFlow, ref AdjustedAirflow);

            adjusted.Add(AirFlow);
            adjusted.Add(AdjustedAirflow);

            var f = new PlotForm1(
                Frequency,
                adjusted
                );

            f.Show(this);
        }

        private void MinFrequency_trackBar_Scroll(object sender, EventArgs e)
        {

        }

        private void MaxFrequency_trackBar_Scroll(object sender, EventArgs e)
        {

        }

        private void MinFrequency_trackBar_ValueChanged(object sender, EventArgs e)
        {
            if (MinFrequency_trackBar.Value > MaxFrequency_trackBar.Value)
                MinFrequency_trackBar.Value = MaxFrequency_trackBar.Value;

            UpdateRange();
        }

        private void MaxFrequency_trackBar_ValueChanged(object sender, EventArgs e)
        {
            if (MaxFrequency_trackBar.Value < MinFrequency_trackBar.Value)
                MaxFrequency_trackBar.Value = MinFrequency_trackBar.Value;

            UpdateRange();
        }

        private void UpdateRange()
        {
            int min = (MinFrequency_trackBar.Value * m_SessionClass.FrequencyStep) + m_SessionClass.MinFrequency;
            int max = (MaxFrequency_trackBar.Value * m_SessionClass.FrequencyStep) + m_SessionClass.MinFrequency;

            MinMax_label.Text = $"{min} - {max}";
            ProcessAdjustmentData();
        }

        private void InitRangeTrackBars(int MinFrequency, int MaxFrequency, int Step)
        {
            MinFrequency_trackBar.Minimum = 0;
            MaxFrequency_trackBar.Minimum = 0;

            MinFrequency_trackBar.Maximum = ((MaxFrequency - MinFrequency) / Step);
            MaxFrequency_trackBar.Maximum = ((MaxFrequency - MinFrequency) / Step);

            MinFrequency_trackBar.TickFrequency = 5;
            MaxFrequency_trackBar.TickFrequency = 5;

            MinFrequency_trackBar.Value = 0;
            MaxFrequency_trackBar.Value = ((MaxFrequency - MinFrequency) / Step);

            MinFrequency_label.Text = MinFrequency.ToString();
            MaxFrequency_label.Text = MaxFrequency.ToString();


        }

        private void tuneToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var root = tuneToolStripMenuItem;

            // 1) Remove old dynamic items (everything after our separator)
            int sepIndex = root.DropDownItems.IndexOf(_dynSep);
            if (sepIndex >= 0)
            {
                // remove items after separator
                for (int i = root.DropDownItems.Count - 1; i > sepIndex; i--)
                    root.DropDownItems.RemoveAt(i);
            }
            else
            {
                // 2) Insert separator after the static items (New/Continue)
                root.DropDownItems.Add(_dynSep);
                sepIndex = root.DropDownItems.Count - 1;
            }

            if (m_SessionClass == null || m_SessionClass.GetTuneCycleCount() == 0)
            {
                _tuneCyclesMenu.DropDownItems.Add(new ToolStripMenuItem("(none)") { Enabled = false });
                return;
            }

            for (int i = 0; i < m_SessionClass.GetTuneCycleCount(); i++)
            {
                var tc = m_SessionClass.GetTuneCycleAtIndex(i);

                // Pick whatever label makes sense in your app
                DateTime Utc = tc.GetTimeStamp();
                DateTime Local = Utc.ToLocalTime();

                string label = Local.ToString();

                var mi = new ToolStripMenuItem(label)
                {
                    Tag = tc // stash the object
                };

                mi.Click += RecentTuneCycle_Click;
                root.DropDownItems.Add(mi);
            }


            // Optional: disable separator if no dynamic items
            _dynSep.Visible = root.DropDownItems.Count > sepIndex + 1;
        }

        private void RecentTuneCycle_Click(object? sender, EventArgs e)
        {
            if (sender is not ToolStripMenuItem mi) return;
            if (mi.Tag is not TuneCycle tc) return;

           
            ApplyAdjustments.Enabled = false;
            CompleteCycle_button.Enabled = false;
            Pause_button.Enabled = false;

            
            

            m_TuneCycleReOpened = true;
            m_CurrentTuneCycle = tc;

            InitializeBucketsTextbox();

            //  fill out the airflow text box like it would be in the normal work flow so it looks the same to the user.
            //
            //
            double[] OriginalAirflow = new double[m_AdjustObject.GetFrequencyCount()];

            for (int i = 0; i < m_AdjustObject.GetFrequencyCount(); i++)
            {
                OriginalAirflow[i] = m_CurrentTuneCycle.GetAirflowAtIndex(i);
            }

            AirFlow_richTextBox.Text = string.Join("\t", OriginalAirflow.Select(v => v.ToString()));

            //  adjustment object will read the airflow data from the tune object.
            //  It will alos populate the gridview with frequency and airflow data.
            //
            ProcessOriginalAirflowData();

            ApplyAdjustments.Enabled = false;
            Pause_button.Enabled = false;

            //
            //  read the adjustment data backout of the tune object into the adjust object.
            //
            m_AdjustObject.ReadAdjustmentDataFromTuneObject(m_CurrentTuneCycle);


            //
            //  enable the adjustment percent track bar and the complete cycle button, as we now have the necessary data to apply adjustments and complete the tuning cycle.
            //
            AdjustmentPercent_trackBar.Enabled = true;
            AdjustmentThreshold_trackBar.Enabled = true;
            CompleteCycle_button.Enabled = true;
            Plot_button.Enabled = true;
            InterpolateMissingData_checkBox.Enabled = true;
            MinFrequency_trackBar.Enabled = true;
            MaxFrequency_trackBar.Enabled = true;
            tuneToolStripMenuItem.Enabled = false; // disable the tune menu to prevent opening another tune cycle while we have one open, the user needs to complete or discard the current tune cycle before they can open another one.

            ProcessAdjustmentData();
        }

        private void MinMax_label_Click(object sender, EventArgs e)
        {

        }
    }

}
