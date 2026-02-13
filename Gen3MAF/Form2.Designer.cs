namespace Gen3MAF
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            VehicleName_textBox = new TextBox();
            ECU_textBox = new TextBox();
            OS_textBox = new TextBox();
            FrequncyMin_UpDown = new NumericUpDown();
            MaxFrequency_numericUpDown = new NumericUpDown();
            FrequencyStep_numericUpDown = new NumericUpDown();
            OK_button = new Button();
            Cancel_button = new Button();
            VehicleName_label = new Label();
            ECUName_label = new Label();
            OS_label = new Label();
            MinFrequency_label = new Label();
            MaxFrequency_label = new Label();
            FrequencyStep_label = new Label();
            BucketStyle_comboBox = new ComboBox();
            BucketStyle_label = new Label();
            ((System.ComponentModel.ISupportInitialize)FrequncyMin_UpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MaxFrequency_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FrequencyStep_numericUpDown).BeginInit();
            SuspendLayout();
            // 
            // VehicleName_textBox
            // 
            VehicleName_textBox.Location = new Point(43, 37);
            VehicleName_textBox.Name = "VehicleName_textBox";
            VehicleName_textBox.Size = new Size(179, 27);
            VehicleName_textBox.TabIndex = 0;
            // 
            // ECU_textBox
            // 
            ECU_textBox.Location = new Point(42, 103);
            ECU_textBox.Name = "ECU_textBox";
            ECU_textBox.Size = new Size(180, 27);
            ECU_textBox.TabIndex = 1;
            // 
            // OS_textBox
            // 
            OS_textBox.Location = new Point(43, 166);
            OS_textBox.Name = "OS_textBox";
            OS_textBox.Size = new Size(179, 27);
            OS_textBox.TabIndex = 2;
            // 
            // FrequncyMin_UpDown
            // 
            FrequncyMin_UpDown.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            FrequncyMin_UpDown.Location = new Point(43, 234);
            FrequncyMin_UpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            FrequncyMin_UpDown.Name = "FrequncyMin_UpDown";
            FrequncyMin_UpDown.Size = new Size(150, 27);
            FrequncyMin_UpDown.TabIndex = 3;
            FrequncyMin_UpDown.ValueChanged += FrequncyMin_UpDown_ValueChanged;
            // 
            // MaxFrequency_numericUpDown
            // 
            MaxFrequency_numericUpDown.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            MaxFrequency_numericUpDown.Location = new Point(216, 234);
            MaxFrequency_numericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            MaxFrequency_numericUpDown.Name = "MaxFrequency_numericUpDown";
            MaxFrequency_numericUpDown.Size = new Size(150, 27);
            MaxFrequency_numericUpDown.TabIndex = 4;
            // 
            // FrequencyStep_numericUpDown
            // 
            FrequencyStep_numericUpDown.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            FrequencyStep_numericUpDown.Location = new Point(387, 234);
            FrequencyStep_numericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            FrequencyStep_numericUpDown.Name = "FrequencyStep_numericUpDown";
            FrequencyStep_numericUpDown.Size = new Size(150, 27);
            FrequencyStep_numericUpDown.TabIndex = 5;
            FrequencyStep_numericUpDown.ValueChanged += FrequencyStep_numericUpDown_ValueChanged;
            // 
            // OK_button
            // 
            OK_button.DialogResult = DialogResult.OK;
            OK_button.Location = new Point(49, 362);
            OK_button.Name = "OK_button";
            OK_button.Size = new Size(94, 29);
            OK_button.TabIndex = 6;
            OK_button.Text = "Ok";
            OK_button.UseVisualStyleBackColor = true;
            OK_button.Click += OK_button_Click;
            // 
            // Cancel_button
            // 
            Cancel_button.DialogResult = DialogResult.Cancel;
            Cancel_button.Location = new Point(170, 362);
            Cancel_button.Name = "Cancel_button";
            Cancel_button.Size = new Size(94, 29);
            Cancel_button.TabIndex = 7;
            Cancel_button.Text = "Cancel";
            Cancel_button.UseVisualStyleBackColor = true;
            // 
            // VehicleName_label
            // 
            VehicleName_label.AutoSize = true;
            VehicleName_label.Location = new Point(43, 9);
            VehicleName_label.Name = "VehicleName_label";
            VehicleName_label.Size = new Size(100, 20);
            VehicleName_label.TabIndex = 8;
            VehicleName_label.Text = "Vehicle Name";
            // 
            // ECUName_label
            // 
            ECUName_label.AutoSize = true;
            ECUName_label.Location = new Point(43, 80);
            ECUName_label.Name = "ECUName_label";
            ECUName_label.Size = new Size(36, 20);
            ECUName_label.TabIndex = 9;
            ECUName_label.Text = "ECU";
            // 
            // OS_label
            // 
            OS_label.AutoSize = true;
            OS_label.Location = new Point(47, 143);
            OS_label.Name = "OS_label";
            OS_label.Size = new Size(28, 20);
            OS_label.TabIndex = 10;
            OS_label.Text = "OS";
            // 
            // MinFrequency_label
            // 
            MinFrequency_label.AutoSize = true;
            MinFrequency_label.Location = new Point(43, 210);
            MinFrequency_label.Name = "MinFrequency_label";
            MinFrequency_label.Size = new Size(105, 20);
            MinFrequency_label.TabIndex = 11;
            MinFrequency_label.Text = "Min Frequency";
            // 
            // MaxFrequency_label
            // 
            MaxFrequency_label.AutoSize = true;
            MaxFrequency_label.Location = new Point(216, 210);
            MaxFrequency_label.Name = "MaxFrequency_label";
            MaxFrequency_label.Size = new Size(108, 20);
            MaxFrequency_label.TabIndex = 12;
            MaxFrequency_label.Text = "Max Frequency";
            MaxFrequency_label.Click += MaxFrequency_label_Click;
            // 
            // FrequencyStep_label
            // 
            FrequencyStep_label.AutoSize = true;
            FrequencyStep_label.Location = new Point(387, 210);
            FrequencyStep_label.Name = "FrequencyStep_label";
            FrequencyStep_label.Size = new Size(110, 20);
            FrequencyStep_label.TabIndex = 13;
            FrequencyStep_label.Text = "Frequency Step";
            FrequencyStep_label.Click += FrequencyStep_label_Click;
            // 
            // BucketStyle_comboBox
            // 
            BucketStyle_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            BucketStyle_comboBox.FormattingEnabled = true;
            BucketStyle_comboBox.Items.AddRange(new object[] { "Double", "Single" });
            BucketStyle_comboBox.Location = new Point(42, 301);
            BucketStyle_comboBox.Name = "BucketStyle_comboBox";
            BucketStyle_comboBox.Size = new Size(151, 28);
            BucketStyle_comboBox.TabIndex = 14;
            BucketStyle_comboBox.SelectedIndexChanged += BucketStyle_comboBox_SelectedIndexChanged;
            // 
            // BucketStyle_label
            // 
            BucketStyle_label.AutoSize = true;
            BucketStyle_label.Location = new Point(42, 278);
            BucketStyle_label.Name = "BucketStyle_label";
            BucketStyle_label.Size = new Size(89, 20);
            BucketStyle_label.TabIndex = 15;
            BucketStyle_label.Text = "Bucket Style";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BucketStyle_label);
            Controls.Add(BucketStyle_comboBox);
            Controls.Add(FrequencyStep_label);
            Controls.Add(MaxFrequency_label);
            Controls.Add(MinFrequency_label);
            Controls.Add(OS_label);
            Controls.Add(ECUName_label);
            Controls.Add(VehicleName_label);
            Controls.Add(Cancel_button);
            Controls.Add(OK_button);
            Controls.Add(FrequencyStep_numericUpDown);
            Controls.Add(MaxFrequency_numericUpDown);
            Controls.Add(FrequncyMin_UpDown);
            Controls.Add(OS_textBox);
            Controls.Add(ECU_textBox);
            Controls.Add(VehicleName_textBox);
            Name = "Form2";
            Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)FrequncyMin_UpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)MaxFrequency_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)FrequencyStep_numericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox VehicleName_textBox;
        private TextBox ECU_textBox;
        private TextBox OS_textBox;
        private NumericUpDown FrequncyMin_UpDown;
        private NumericUpDown MaxFrequency_numericUpDown;
        private NumericUpDown FrequencyStep_numericUpDown;
        private Button OK_button;
        private Button Cancel_button;
        private Label VehicleName_label;
        private Label ECUName_label;
        private Label OS_label;
        private Label MinFrequency_label;
        private Label MaxFrequency_label;
        private Label FrequencyStep_label;
        private ComboBox BucketStyle_comboBox;
        private Label BucketStyle_label;
    }
}