namespace Gen3MAF
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            contextMenuStrip1 = new ContextMenuStrip(components);
            pasteToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            AirFlow_richTextBox = new RichTextBox();
            Process_button = new Button();
            MAF_dataGridView = new DataGridView();
            Buckets_richTextBox = new RichTextBox();
            label1 = new Label();
            AdjustmentBuckets_richTextBox = new RichTextBox();
            label2 = new Label();
            button1 = new Button();
            AdjustmentPercent_trackBar = new TrackBar();
            AdjustmentPercent_label = new Label();
            AdjustedAirflow_dataGridView = new DataGridView();
            Airflow_label = new Label();
            AverageWithOriginal_checkBox = new CheckBox();
            menuStrip1 = new MenuStrip();
            session_ToolStripMenuItem = new ToolStripMenuItem();
            create_ToolStripMenuItem = new ToolStripMenuItem();
            open_ToolStripMenuItem = new ToolStripMenuItem();
            tuneToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            lastToolStripMenuItem = new ToolStripMenuItem();
            CurrentMafCurve_label = new Label();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MAF_dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AdjustmentPercent_trackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AdjustedAirflow_dataGridView).BeginInit();
            menuStrip1.SuspendLayout();
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
            // Process_button
            // 
            Process_button.Enabled = false;
            Process_button.Location = new Point(71, 204);
            Process_button.Name = "Process_button";
            Process_button.Size = new Size(188, 29);
            Process_button.TabIndex = 2;
            Process_button.Text = "Process Airflow Data";
            Process_button.UseVisualStyleBackColor = true;
            Process_button.Click += Process_button_Click;
            // 
            // MAF_dataGridView
            // 
            MAF_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MAF_dataGridView.Location = new Point(71, 280);
            MAF_dataGridView.Name = "MAF_dataGridView";
            MAF_dataGridView.ReadOnly = true;
            MAF_dataGridView.RowHeadersWidth = 51;
            MAF_dataGridView.Size = new Size(1065, 88);
            MAF_dataGridView.TabIndex = 3;
            // 
            // Buckets_richTextBox
            // 
            Buckets_richTextBox.ContextMenuStrip = contextMenuStrip1;
            Buckets_richTextBox.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Buckets_richTextBox.Location = new Point(68, 52);
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
            AdjustmentBuckets_richTextBox.Location = new Point(71, 552);
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
            label2.Location = new Point(73, 529);
            label2.Name = "label2";
            label2.Size = new Size(319, 20);
            label2.TabIndex = 10;
            label2.Text = "Paste bucket data from scanner app histogram.";
            // 
            // button1
            // 
            button1.Location = new Point(73, 616);
            button1.Name = "button1";
            button1.Size = new Size(158, 29);
            button1.TabIndex = 11;
            button1.Text = "Apply Adjustments";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // AdjustmentPercent_trackBar
            // 
            AdjustmentPercent_trackBar.LargeChange = 10;
            AdjustmentPercent_trackBar.Location = new Point(273, 616);
            AdjustmentPercent_trackBar.Maximum = 100;
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
            AdjustmentPercent_label.Location = new Point(438, 618);
            AdjustmentPercent_label.Name = "AdjustmentPercent_label";
            AdjustmentPercent_label.Size = new Size(0, 20);
            AdjustmentPercent_label.TabIndex = 13;
            // 
            // AdjustedAirflow_dataGridView
            // 
            AdjustedAirflow_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AdjustedAirflow_dataGridView.ContextMenuStrip = contextMenuStrip1;
            AdjustedAirflow_dataGridView.Location = new Point(76, 672);
            AdjustedAirflow_dataGridView.Name = "AdjustedAirflow_dataGridView";
            AdjustedAirflow_dataGridView.RowHeadersWidth = 51;
            AdjustedAirflow_dataGridView.ScrollBars = ScrollBars.Horizontal;
            AdjustedAirflow_dataGridView.Size = new Size(1057, 146);
            AdjustedAirflow_dataGridView.TabIndex = 14;
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
            // AverageWithOriginal_checkBox
            // 
            AverageWithOriginal_checkBox.AutoSize = true;
            AverageWithOriginal_checkBox.Location = new Point(510, 621);
            AverageWithOriginal_checkBox.Name = "AverageWithOriginal_checkBox";
            AverageWithOriginal_checkBox.Size = new Size(286, 24);
            AverageWithOriginal_checkBox.TabIndex = 23;
            AverageWithOriginal_checkBox.Text = "Avergage Adjusted value with Original";
            AverageWithOriginal_checkBox.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { session_ToolStripMenuItem, tuneToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1191, 28);
            menuStrip1.TabIndex = 24;
            menuStrip1.Text = "menuStrip1";
            // 
            // session_ToolStripMenuItem
            // 
            session_ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { create_ToolStripMenuItem, open_ToolStripMenuItem });
            session_ToolStripMenuItem.Name = "session_ToolStripMenuItem";
            session_ToolStripMenuItem.Size = new Size(72, 24);
            session_ToolStripMenuItem.Text = "Session";
            // 
            // create_ToolStripMenuItem
            // 
            create_ToolStripMenuItem.Name = "create_ToolStripMenuItem";
            create_ToolStripMenuItem.Size = new Size(135, 26);
            create_ToolStripMenuItem.Text = "Create";
            create_ToolStripMenuItem.Click += create_ToolStripMenuItem_Click;
            // 
            // open_ToolStripMenuItem
            // 
            open_ToolStripMenuItem.Name = "open_ToolStripMenuItem";
            open_ToolStripMenuItem.Size = new Size(135, 26);
            open_ToolStripMenuItem.Text = "Open";
            // 
            // tuneToolStripMenuItem
            // 
            tuneToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, lastToolStripMenuItem });
            tuneToolStripMenuItem.Name = "tuneToolStripMenuItem";
            tuneToolStripMenuItem.Size = new Size(94, 24);
            tuneToolStripMenuItem.Text = "Tune Cycle";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(122, 26);
            newToolStripMenuItem.Text = "New";
            // 
            // lastToolStripMenuItem
            // 
            lastToolStripMenuItem.Name = "lastToolStripMenuItem";
            lastToolStripMenuItem.Size = new Size(122, 26);
            lastToolStripMenuItem.Text = "Last";
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1191, 888);
            Controls.Add(CurrentMafCurve_label);
            Controls.Add(menuStrip1);
            Controls.Add(AverageWithOriginal_checkBox);
            Controls.Add(Airflow_label);
            Controls.Add(AdjustedAirflow_dataGridView);
            Controls.Add(AdjustmentPercent_label);
            Controls.Add(AdjustmentPercent_trackBar);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(AdjustmentBuckets_richTextBox);
            Controls.Add(label1);
            Controls.Add(Buckets_richTextBox);
            Controls.Add(MAF_dataGridView);
            Controls.Add(Process_button);
            Controls.Add(AirFlow_richTextBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Generation 3 LS MAF tuning";
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MAF_dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)AdjustmentPercent_trackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)AdjustedAirflow_dataGridView).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox AirFlow_richTextBox;
        private Button Process_button;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private DataGridView MAF_dataGridView;
        private RichTextBox Buckets_richTextBox;
        private Label label1;
        private RichTextBox AdjustmentBuckets_richTextBox;
        private Label label2;
        private Button button1;
        private TrackBar AdjustmentPercent_trackBar;
        private Label AdjustmentPercent_label;
        private DataGridView AdjustedAirflow_dataGridView;
        private Label Airflow_label;
        private ToolStripMenuItem copyToolStripMenuItem;
        private CheckBox AverageWithOriginal_checkBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem session_ToolStripMenuItem;
        private ToolStripMenuItem create_ToolStripMenuItem;
        private ToolStripMenuItem open_ToolStripMenuItem;
        private Label CurrentMafCurve_label;
        private ToolStripMenuItem tuneToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem lastToolStripMenuItem;
    }
}
