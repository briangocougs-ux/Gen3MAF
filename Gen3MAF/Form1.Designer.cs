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
            contextMenuStrip1 = new ContextMenuStrip(components);
            pasteToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            AirFlow_richTextBox = new RichTextBox();
            Process_button = new Button();
            MAF_dataGridView = new DataGridView();
            Buckets_richTextBox = new RichTextBox();
            SingleBucket_radioButton = new RadioButton();
            DounbleBucket_radioButton = new RadioButton();
            BucketType_groupBox = new GroupBox();
            label1 = new Label();
            AdjustmentBuckets_richTextBox = new RichTextBox();
            label2 = new Label();
            button1 = new Button();
            AdjustmentPercent_trackBar = new TrackBar();
            AdjustmentPercent_label = new Label();
            AdjustedAirflow_dataGridView = new DataGridView();
            MinFrequency_numericUpDown = new NumericUpDown();
            MaxFrequency_numericUpDown = new NumericUpDown();
            FrequencyStep_numericUpDown = new NumericUpDown();
            ValidateMAF_button = new Button();
            MinFrequency_label = new Label();
            MaxFrequency_label = new Label();
            FrequencyStep_label = new Label();
            Airflow_label = new Label();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MAF_dataGridView).BeginInit();
            BucketType_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AdjustmentPercent_trackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AdjustedAirflow_dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MinFrequency_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MaxFrequency_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FrequencyStep_numericUpDown).BeginInit();
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
            Process_button.Location = new Point(71, 220);
            Process_button.Name = "Process_button";
            Process_button.Size = new Size(94, 29);
            Process_button.TabIndex = 2;
            Process_button.Text = "Process";
            Process_button.UseVisualStyleBackColor = true;
            Process_button.Click += Process_button_Click;
            // 
            // MAF_dataGridView
            // 
            MAF_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MAF_dataGridView.Location = new Point(71, 268);
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
            Buckets_richTextBox.Location = new Point(71, 458);
            Buckets_richTextBox.Name = "Buckets_richTextBox";
            Buckets_richTextBox.ReadOnly = true;
            Buckets_richTextBox.Size = new Size(1065, 49);
            Buckets_richTextBox.TabIndex = 4;
            Buckets_richTextBox.Text = "";
            Buckets_richTextBox.WordWrap = false;
            // 
            // SingleBucket_radioButton
            // 
            SingleBucket_radioButton.AutoSize = true;
            SingleBucket_radioButton.Location = new Point(20, 22);
            SingleBucket_radioButton.Name = "SingleBucket_radioButton";
            SingleBucket_radioButton.Size = new Size(119, 24);
            SingleBucket_radioButton.TabIndex = 5;
            SingleBucket_radioButton.Text = "Single Bucket";
            SingleBucket_radioButton.UseVisualStyleBackColor = true;
            // 
            // DounbleBucket_radioButton
            // 
            DounbleBucket_radioButton.AutoSize = true;
            DounbleBucket_radioButton.Checked = true;
            DounbleBucket_radioButton.Location = new Point(161, 22);
            DounbleBucket_radioButton.Name = "DounbleBucket_radioButton";
            DounbleBucket_radioButton.Size = new Size(127, 24);
            DounbleBucket_radioButton.TabIndex = 6;
            DounbleBucket_radioButton.TabStop = true;
            DounbleBucket_radioButton.Text = "Double Bucket";
            DounbleBucket_radioButton.UseVisualStyleBackColor = true;
            // 
            // BucketType_groupBox
            // 
            BucketType_groupBox.Controls.Add(SingleBucket_radioButton);
            BucketType_groupBox.Controls.Add(DounbleBucket_radioButton);
            BucketType_groupBox.Location = new Point(71, 375);
            BucketType_groupBox.Name = "BucketType_groupBox";
            BucketType_groupBox.Size = new Size(299, 52);
            BucketType_groupBox.TabIndex = 7;
            BucketType_groupBox.TabStop = false;
            BucketType_groupBox.Text = "Bucket Type";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(71, 435);
            label1.Name = "label1";
            label1.Size = new Size(115, 20);
            label1.TabIndex = 8;
            label1.Text = "Buckets to Paste";
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
            label2.Size = new Size(158, 20);
            label2.TabIndex = 10;
            label2.Text = "Average Bucket Values";
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
            // MinFrequency_numericUpDown
            // 
            MinFrequency_numericUpDown.Location = new Point(73, 48);
            MinFrequency_numericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            MinFrequency_numericUpDown.Name = "MinFrequency_numericUpDown";
            MinFrequency_numericUpDown.Size = new Size(150, 27);
            MinFrequency_numericUpDown.TabIndex = 15;
            // 
            // MaxFrequency_numericUpDown
            // 
            MaxFrequency_numericUpDown.Location = new Point(242, 48);
            MaxFrequency_numericUpDown.Maximum = new decimal(new int[] { 20000, 0, 0, 0 });
            MaxFrequency_numericUpDown.Name = "MaxFrequency_numericUpDown";
            MaxFrequency_numericUpDown.Size = new Size(150, 27);
            MaxFrequency_numericUpDown.TabIndex = 16;
            // 
            // FrequencyStep_numericUpDown
            // 
            FrequencyStep_numericUpDown.Location = new Point(418, 48);
            FrequencyStep_numericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            FrequencyStep_numericUpDown.Name = "FrequencyStep_numericUpDown";
            FrequencyStep_numericUpDown.Size = new Size(150, 27);
            FrequencyStep_numericUpDown.TabIndex = 17;
            // 
            // ValidateMAF_button
            // 
            ValidateMAF_button.Location = new Point(595, 46);
            ValidateMAF_button.Name = "ValidateMAF_button";
            ValidateMAF_button.Size = new Size(188, 29);
            ValidateMAF_button.TabIndex = 18;
            ValidateMAF_button.Text = "Validate MAF config";
            ValidateMAF_button.UseVisualStyleBackColor = true;
            ValidateMAF_button.Click += ValidateMAF_button_Click;
            // 
            // MinFrequency_label
            // 
            MinFrequency_label.AutoSize = true;
            MinFrequency_label.Location = new Point(74, 15);
            MinFrequency_label.Name = "MinFrequency_label";
            MinFrequency_label.Size = new Size(105, 20);
            MinFrequency_label.TabIndex = 19;
            MinFrequency_label.Text = "Min Frequency";
            // 
            // MaxFrequency_label
            // 
            MaxFrequency_label.AutoSize = true;
            MaxFrequency_label.Location = new Point(245, 18);
            MaxFrequency_label.Name = "MaxFrequency_label";
            MaxFrequency_label.Size = new Size(108, 20);
            MaxFrequency_label.TabIndex = 20;
            MaxFrequency_label.Text = "Max Frequency";
            // 
            // FrequencyStep_label
            // 
            FrequencyStep_label.AutoSize = true;
            FrequencyStep_label.Location = new Point(420, 17);
            FrequencyStep_label.Name = "FrequencyStep_label";
            FrequencyStep_label.Size = new Size(110, 20);
            FrequencyStep_label.TabIndex = 21;
            FrequencyStep_label.Text = "Frequency Step";
            // 
            // Airflow_label
            // 
            Airflow_label.AutoSize = true;
            Airflow_label.Location = new Point(72, 104);
            Airflow_label.Name = "Airflow_label";
            Airflow_label.Size = new Size(257, 20);
            Airflow_label.TabIndex = 22;
            Airflow_label.Text = "Paste Airflow values from Tuning App";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1191, 888);
            Controls.Add(Airflow_label);
            Controls.Add(FrequencyStep_label);
            Controls.Add(MaxFrequency_label);
            Controls.Add(MinFrequency_label);
            Controls.Add(ValidateMAF_button);
            Controls.Add(FrequencyStep_numericUpDown);
            Controls.Add(MaxFrequency_numericUpDown);
            Controls.Add(MinFrequency_numericUpDown);
            Controls.Add(AdjustedAirflow_dataGridView);
            Controls.Add(AdjustmentPercent_label);
            Controls.Add(AdjustmentPercent_trackBar);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(AdjustmentBuckets_richTextBox);
            Controls.Add(label1);
            Controls.Add(BucketType_groupBox);
            Controls.Add(Buckets_richTextBox);
            Controls.Add(MAF_dataGridView);
            Controls.Add(Process_button);
            Controls.Add(AirFlow_richTextBox);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MAF_dataGridView).EndInit();
            BucketType_groupBox.ResumeLayout(false);
            BucketType_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AdjustmentPercent_trackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)AdjustedAirflow_dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)MinFrequency_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)MaxFrequency_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)FrequencyStep_numericUpDown).EndInit();
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
        private RadioButton SingleBucket_radioButton;
        private RadioButton DounbleBucket_radioButton;
        private GroupBox BucketType_groupBox;
        private Label label1;
        private RichTextBox AdjustmentBuckets_richTextBox;
        private Label label2;
        private Button button1;
        private TrackBar AdjustmentPercent_trackBar;
        private Label AdjustmentPercent_label;
        private DataGridView AdjustedAirflow_dataGridView;
        private NumericUpDown MinFrequency_numericUpDown;
        private NumericUpDown MaxFrequency_numericUpDown;
        private NumericUpDown FrequencyStep_numericUpDown;
        private Button ValidateMAF_button;
        private Label MinFrequency_label;
        private Label MaxFrequency_label;
        private Label FrequencyStep_label;
        private Label Airflow_label;
        private ToolStripMenuItem copyToolStripMenuItem;
    }
}
