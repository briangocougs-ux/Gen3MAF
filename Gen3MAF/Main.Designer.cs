namespace Gen3MAF
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            contextMenuStrip1 = new ContextMenuStrip(components);
            pasteToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            AirFlow_richTextBox = new RichTextBox();
            ProcessOriginalAirflow_button = new Button();
            MAF_dataGridView = new DataGridView();
            Buckets_richTextBox = new RichTextBox();
            label1 = new Label();
            AdjustmentBuckets_richTextBox = new RichTextBox();
            label2 = new Label();
            ApplyAdjustments = new Button();
            AdjustmentPercent_trackBar = new TrackBar();
            AdjustmentPercent_label = new Label();
            AdjustedAirflow_dataGridView = new DataGridView();
            Airflow_label = new Label();
            menuStrip1 = new MenuStrip();
            session_ToolStripMenuItem = new ToolStripMenuItem();
            NewSession_ToolStripMenuItem = new ToolStripMenuItem();
            open_ToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            tuneToolStripMenuItem = new ToolStripMenuItem();
            NewTuneCycle_toolStripMenuItem = new ToolStripMenuItem();
            Continue_ToolStripMenuItem = new ToolStripMenuItem();
            plotAllToolStripMenuItem = new ToolStripMenuItem();
            CurrentMafCurve_label = new Label();
            CompleteCycle_button = new Button();
            Discard_button = new Button();
            Pause_button = new Button();
            Plot_button = new Button();
            AdjustmentThreshold_trackBar = new TrackBar();
            ThresholdValue_label = new Label();
            AdjustText_label = new Label();
            ThresholdText_label = new Label();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MAF_dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AdjustmentPercent_trackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AdjustedAirflow_dataGridView).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AdjustmentThreshold_trackBar).BeginInit();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { pasteToolStripMenuItem, copyToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(113, 52);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new Size(112, 24);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(112, 24);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // AirFlow_richTextBox
            // 
            AirFlow_richTextBox.ContextMenuStrip = contextMenuStrip1;
            AirFlow_richTextBox.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AirFlow_richTextBox.Location = new Point(71, 137);
            AirFlow_richTextBox.Name = "AirFlow_richTextBox";
            AirFlow_richTextBox.ScrollBars = RichTextBoxScrollBars.Horizontal;
            AirFlow_richTextBox.Size = new Size(1072, 61);
            AirFlow_richTextBox.TabIndex = 1;
            AirFlow_richTextBox.Text = "";
            AirFlow_richTextBox.WordWrap = false;
            // 
            // ProcessOriginalAirflow_button
            // 
            ProcessOriginalAirflow_button.Enabled = false;
            ProcessOriginalAirflow_button.Location = new Point(71, 204);
            ProcessOriginalAirflow_button.Name = "ProcessOriginalAirflow_button";
            ProcessOriginalAirflow_button.Size = new Size(188, 29);
            ProcessOriginalAirflow_button.TabIndex = 2;
            ProcessOriginalAirflow_button.Text = "Process Airflow Data";
            ProcessOriginalAirflow_button.UseVisualStyleBackColor = true;
            ProcessOriginalAirflow_button.Click += Process_button_Click;
            // 
            // MAF_dataGridView
            // 
            MAF_dataGridView.AllowUserToAddRows = false;
            MAF_dataGridView.AllowUserToDeleteRows = false;
            MAF_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MAF_dataGridView.Location = new Point(71, 280);
            MAF_dataGridView.MultiSelect = false;
            MAF_dataGridView.Name = "MAF_dataGridView";
            MAF_dataGridView.ReadOnly = true;
            MAF_dataGridView.RowHeadersWidth = 51;
            MAF_dataGridView.Size = new Size(1065, 88);
            MAF_dataGridView.TabIndex = 3;
            MAF_dataGridView.CellContentClick += MAF_dataGridView_CellDoubleClick;
            MAF_dataGridView.CellContentDoubleClick += MAF_dataGridView_CellDoubleClick;
            MAF_dataGridView.CellDoubleClick += MAF_dataGridView_CellDoubleClick;
            MAF_dataGridView.MouseClick += MAF_dataGridView_MouseClick;
            MAF_dataGridView.MouseDoubleClick += MAF_dataGridView_MouseDoubleClick;
            // 
            // Buckets_richTextBox
            // 
            Buckets_richTextBox.ContextMenuStrip = contextMenuStrip1;
            Buckets_richTextBox.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Buckets_richTextBox.Location = new Point(71, 51);
            Buckets_richTextBox.Name = "Buckets_richTextBox";
            Buckets_richTextBox.ReadOnly = true;
            Buckets_richTextBox.Size = new Size(1065, 49);
            Buckets_richTextBox.TabIndex = 4;
            Buckets_richTextBox.Text = "";
            Buckets_richTextBox.WordWrap = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(68, 28);
            label1.Name = "label1";
            label1.Size = new Size(439, 20);
            label1.TabIndex = 8;
            label1.Text = "Copy Buckets to Paste into scanner app Histogram Value text box";
            // 
            // AdjustmentBuckets_richTextBox
            // 
            AdjustmentBuckets_richTextBox.ContextMenuStrip = contextMenuStrip1;
            AdjustmentBuckets_richTextBox.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AdjustmentBuckets_richTextBox.Location = new Point(71, 472);
            AdjustmentBuckets_richTextBox.Name = "AdjustmentBuckets_richTextBox";
            AdjustmentBuckets_richTextBox.ScrollBars = RichTextBoxScrollBars.Horizontal;
            AdjustmentBuckets_richTextBox.Size = new Size(1062, 54);
            AdjustmentBuckets_richTextBox.TabIndex = 9;
            AdjustmentBuckets_richTextBox.Text = "";
            AdjustmentBuckets_richTextBox.WordWrap = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(71, 449);
            label2.Name = "label2";
            label2.Size = new Size(319, 20);
            label2.TabIndex = 10;
            label2.Text = "Paste bucket data from scanner app histogram.";
            // 
            // ApplyAdjustments
            // 
            ApplyAdjustments.Location = new Point(71, 571);
            ApplyAdjustments.Name = "ApplyAdjustments";
            ApplyAdjustments.Size = new Size(158, 29);
            ApplyAdjustments.TabIndex = 11;
            ApplyAdjustments.Text = "Apply Adjustments";
            ApplyAdjustments.UseVisualStyleBackColor = true;
            ApplyAdjustments.Click += button1_Click;
            // 
            // AdjustmentPercent_trackBar
            // 
            AdjustmentPercent_trackBar.Enabled = false;
            AdjustmentPercent_trackBar.LargeChange = 10;
            AdjustmentPercent_trackBar.Location = new Point(276, 571);
            AdjustmentPercent_trackBar.Maximum = 100;
            AdjustmentPercent_trackBar.Minimum = 1;
            AdjustmentPercent_trackBar.Name = "AdjustmentPercent_trackBar";
            AdjustmentPercent_trackBar.Size = new Size(156, 56);
            AdjustmentPercent_trackBar.TabIndex = 12;
            AdjustmentPercent_trackBar.TickFrequency = 10;
            AdjustmentPercent_trackBar.Value = 50;
            AdjustmentPercent_trackBar.Scroll += AdjustmentPercent_trackBar_Scroll;
            // 
            // AdjustmentPercent_label
            // 
            AdjustmentPercent_label.AutoSize = true;
            AdjustmentPercent_label.Location = new Point(438, 575);
            AdjustmentPercent_label.Name = "AdjustmentPercent_label";
            AdjustmentPercent_label.Size = new Size(25, 20);
            AdjustmentPercent_label.TabIndex = 13;
            AdjustmentPercent_label.Text = "99";
            // 
            // AdjustedAirflow_dataGridView
            // 
            AdjustedAirflow_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AdjustedAirflow_dataGridView.ContextMenuStrip = contextMenuStrip1;
            AdjustedAirflow_dataGridView.Location = new Point(71, 619);
            AdjustedAirflow_dataGridView.Name = "AdjustedAirflow_dataGridView";
            AdjustedAirflow_dataGridView.RowHeadersWidth = 51;
            AdjustedAirflow_dataGridView.ScrollBars = ScrollBars.Horizontal;
            AdjustedAirflow_dataGridView.Size = new Size(1057, 169);
            AdjustedAirflow_dataGridView.TabIndex = 14;
            AdjustedAirflow_dataGridView.CellContentClick += AdjustedAirflow_dataGridView_CellContentClick;
            AdjustedAirflow_dataGridView.CellDoubleClick += AdjustedAirflow_dataGridView_CellDoubleClick;
            AdjustedAirflow_dataGridView.KeyDown += AdjustedAirflow_dataGridView_KeyDown;
            // 
            // Airflow_label
            // 
            Airflow_label.AutoSize = true;
            Airflow_label.Location = new Point(71, 114);
            Airflow_label.Name = "Airflow_label";
            Airflow_label.Size = new Size(257, 20);
            Airflow_label.TabIndex = 22;
            Airflow_label.Text = "Paste Airflow values from Tuning App";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { session_ToolStripMenuItem, tuneToolStripMenuItem, plotAllToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1191, 28);
            menuStrip1.TabIndex = 24;
            menuStrip1.Text = "menuStrip1";
            // 
            // session_ToolStripMenuItem
            // 
            session_ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { NewSession_ToolStripMenuItem, open_ToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, closeToolStripMenuItem });
            session_ToolStripMenuItem.Name = "session_ToolStripMenuItem";
            session_ToolStripMenuItem.Size = new Size(72, 24);
            session_ToolStripMenuItem.Text = "&Session";
            // 
            // NewSession_ToolStripMenuItem
            // 
            NewSession_ToolStripMenuItem.Name = "NewSession_ToolStripMenuItem";
            NewSession_ToolStripMenuItem.Size = new Size(143, 26);
            NewSession_ToolStripMenuItem.Text = "&New";
            NewSession_ToolStripMenuItem.Click += create_ToolStripMenuItem_Click;
            // 
            // open_ToolStripMenuItem
            // 
            open_ToolStripMenuItem.Name = "open_ToolStripMenuItem";
            open_ToolStripMenuItem.Size = new Size(143, 26);
            open_ToolStripMenuItem.Text = "&Open";
            open_ToolStripMenuItem.Click += open_ToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(143, 26);
            saveToolStripMenuItem.Text = "&Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(143, 26);
            saveAsToolStripMenuItem.Text = "Save &As";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(143, 26);
            closeToolStripMenuItem.Text = "&Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // tuneToolStripMenuItem
            // 
            tuneToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { NewTuneCycle_toolStripMenuItem, Continue_ToolStripMenuItem });
            tuneToolStripMenuItem.Enabled = false;
            tuneToolStripMenuItem.Name = "tuneToolStripMenuItem";
            tuneToolStripMenuItem.Size = new Size(94, 24);
            tuneToolStripMenuItem.Text = "&Tune Cycle";
            // 
            // NewTuneCycle_toolStripMenuItem
            // 
            NewTuneCycle_toolStripMenuItem.Name = "NewTuneCycle_toolStripMenuItem";
            NewTuneCycle_toolStripMenuItem.Size = new Size(151, 26);
            NewTuneCycle_toolStripMenuItem.Text = "&New";
            NewTuneCycle_toolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // Continue_ToolStripMenuItem
            // 
            Continue_ToolStripMenuItem.Name = "Continue_ToolStripMenuItem";
            Continue_ToolStripMenuItem.Size = new Size(151, 26);
            Continue_ToolStripMenuItem.Text = "Continue";
            Continue_ToolStripMenuItem.Click += Continue_ToolStripMenuItem_Click;
            // 
            // plotAllToolStripMenuItem
            // 
            plotAllToolStripMenuItem.Enabled = false;
            plotAllToolStripMenuItem.Name = "plotAllToolStripMenuItem";
            plotAllToolStripMenuItem.Size = new Size(71, 24);
            plotAllToolStripMenuItem.Text = "Plot All";
            plotAllToolStripMenuItem.Click += plotAllToolStripMenuItem_Click;
            // 
            // CurrentMafCurve_label
            // 
            CurrentMafCurve_label.AutoSize = true;
            CurrentMafCurve_label.Location = new Point(76, 257);
            CurrentMafCurve_label.Name = "CurrentMafCurve_label";
            CurrentMafCurve_label.Size = new Size(239, 20);
            CurrentMafCurve_label.TabIndex = 25;
            CurrentMafCurve_label.Text = "Verify MAF Curve from Tuning app.";
            // 
            // CompleteCycle_button
            // 
            CompleteCycle_button.Location = new Point(76, 844);
            CompleteCycle_button.Name = "CompleteCycle_button";
            CompleteCycle_button.Size = new Size(94, 29);
            CompleteCycle_button.TabIndex = 26;
            CompleteCycle_button.Text = "Complete";
            CompleteCycle_button.UseVisualStyleBackColor = true;
            CompleteCycle_button.Click += CompleteCycle_button_Click;
            // 
            // Discard_button
            // 
            Discard_button.Location = new Point(234, 846);
            Discard_button.Name = "Discard_button";
            Discard_button.Size = new Size(156, 29);
            Discard_button.TabIndex = 27;
            Discard_button.Text = "Discard Changes";
            Discard_button.UseVisualStyleBackColor = true;
            Discard_button.Click += Discard_button_Click;
            // 
            // Pause_button
            // 
            Pause_button.Enabled = false;
            Pause_button.Location = new Point(73, 396);
            Pause_button.Name = "Pause_button";
            Pause_button.Size = new Size(94, 29);
            Pause_button.TabIndex = 28;
            Pause_button.Text = "Pause Tune";
            Pause_button.UseVisualStyleBackColor = true;
            Pause_button.Click += Pasue_button_Click;
            // 
            // Plot_button
            // 
            Plot_button.Enabled = false;
            Plot_button.Location = new Point(977, 575);
            Plot_button.Name = "Plot_button";
            Plot_button.Size = new Size(94, 29);
            Plot_button.TabIndex = 29;
            Plot_button.Text = "Plot";
            Plot_button.UseVisualStyleBackColor = true;
            Plot_button.Click += Plot_button_Click;
            // 
            // AdjustmentThreshold_trackBar
            // 
            AdjustmentThreshold_trackBar.Location = new Point(572, 571);
            AdjustmentThreshold_trackBar.Maximum = 30;
            AdjustmentThreshold_trackBar.Minimum = 1;
            AdjustmentThreshold_trackBar.Name = "AdjustmentThreshold_trackBar";
            AdjustmentThreshold_trackBar.Size = new Size(130, 56);
            AdjustmentThreshold_trackBar.TabIndex = 30;
            AdjustmentThreshold_trackBar.Value = 1;
            AdjustmentThreshold_trackBar.Scroll += AdjustmentThreshold_trackBar_Scroll;
            // 
            // ThresholdValue_label
            // 
            ThresholdValue_label.AutoSize = true;
            ThresholdValue_label.Location = new Point(708, 575);
            ThresholdValue_label.Name = "ThresholdValue_label";
            ThresholdValue_label.Size = new Size(69, 20);
            ThresholdValue_label.TabIndex = 31;
            ThresholdValue_label.Text = "label32.0";
            // 
            // AdjustText_label
            // 
            AdjustText_label.AutoSize = true;
            AdjustText_label.Location = new Point(301, 543);
            AdjustText_label.Name = "AdjustText_label";
            AdjustText_label.Size = new Size(121, 20);
            AdjustText_label.TabIndex = 32;
            AdjustText_label.Text = "Correction factor";
            // 
            // ThresholdText_label
            // 
            ThresholdText_label.AutoSize = true;
            ThresholdText_label.Location = new Point(605, 543);
            ThresholdText_label.Name = "ThresholdText_label";
            ThresholdText_label.Size = new Size(147, 20);
            ThresholdText_label.TabIndex = 33;
            ThresholdText_label.Text = "Correction Threshold";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1191, 888);
            Controls.Add(ThresholdText_label);
            Controls.Add(AdjustText_label);
            Controls.Add(ThresholdValue_label);
            Controls.Add(AdjustmentThreshold_trackBar);
            Controls.Add(Plot_button);
            Controls.Add(Pause_button);
            Controls.Add(Discard_button);
            Controls.Add(CompleteCycle_button);
            Controls.Add(CurrentMafCurve_label);
            Controls.Add(menuStrip1);
            Controls.Add(Airflow_label);
            Controls.Add(AdjustedAirflow_dataGridView);
            Controls.Add(AdjustmentPercent_label);
            Controls.Add(AdjustmentPercent_trackBar);
            Controls.Add(ApplyAdjustments);
            Controls.Add(label2);
            Controls.Add(AdjustmentBuckets_richTextBox);
            Controls.Add(label1);
            Controls.Add(Buckets_richTextBox);
            Controls.Add(MAF_dataGridView);
            Controls.Add(ProcessOriginalAirflow_button);
            Controls.Add(AirFlow_richTextBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Main";
            Text = "Generation 3 LS MAF tuning";
            FormClosing += Main_FormClosing;
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MAF_dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)AdjustmentPercent_trackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)AdjustedAirflow_dataGridView).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AdjustmentThreshold_trackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox AirFlow_richTextBox;
        private Button ProcessOriginalAirflow_button;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private DataGridView MAF_dataGridView;
        private RichTextBox Buckets_richTextBox;
        private Label label1;
        private RichTextBox AdjustmentBuckets_richTextBox;
        private Label label2;
        private Button ApplyAdjustments;
        private TrackBar AdjustmentPercent_trackBar;
        private Label AdjustmentPercent_label;
        private DataGridView AdjustedAirflow_dataGridView;
        private Label Airflow_label;
        private ToolStripMenuItem copyToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem session_ToolStripMenuItem;
        private ToolStripMenuItem NewSession_ToolStripMenuItem;
        private ToolStripMenuItem open_ToolStripMenuItem;
        private Label CurrentMafCurve_label;
        private ToolStripMenuItem tuneToolStripMenuItem;
        private ToolStripMenuItem NewTuneCycle_toolStripMenuItem;
        private ToolStripMenuItem Continue_ToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private Button CompleteCycle_button;
        private Button Discard_button;
        private Button Pause_button;
        private ToolStripMenuItem closeToolStripMenuItem;
        private Button Plot_button;
        private ToolStripMenuItem plotAllToolStripMenuItem;
        private TrackBar AdjustmentThreshold_trackBar;
        private Label ThresholdValue_label;
        private Label AdjustText_label;
        private Label ThresholdText_label;
    }
}
