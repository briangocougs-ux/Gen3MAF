namespace Gen3MAF
{
    partial class AboutForm
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
            AppName_label = new Label();
            email_linkLabel = new LinkLabel();
            Ok_button = new Button();
            Contact_label = new Label();
            SuspendLayout();
            // 
            // AppName_label
            // 
            AppName_label.AutoSize = true;
            AppName_label.Location = new Point(80, 52);
            AppName_label.Name = "AppName_label";
            AppName_label.Size = new Size(272, 20);
            AppName_label.TabIndex = 0;
            AppName_label.Text = "Generation 3 LS MAF tuning application";
            // 
            // email_linkLabel
            // 
            email_linkLabel.AutoSize = true;
            email_linkLabel.Location = new Point(129, 128);
            email_linkLabel.Name = "email_linkLabel";
            email_linkLabel.Size = new Size(234, 20);
            email_linkLabel.TabIndex = 1;
            email_linkLabel.TabStop = true;
            email_linkLabel.Text = "mailto:gmt800.owner@gmail.com";
            email_linkLabel.LinkClicked += email_linkLabel_LinkClicked;
            // 
            // Ok_button
            // 
            Ok_button.Location = new Point(167, 227);
            Ok_button.Name = "Ok_button";
            Ok_button.Size = new Size(94, 29);
            Ok_button.TabIndex = 2;
            Ok_button.Text = "Ok";
            Ok_button.UseVisualStyleBackColor = true;
            Ok_button.Click += Ok_button_Click;
            // 
            // Contact_label
            // 
            Contact_label.AutoSize = true;
            Contact_label.Location = new Point(45, 128);
            Contact_label.Name = "Contact_label";
            Contact_label.Size = new Size(63, 20);
            Contact_label.TabIndex = 3;
            Contact_label.Text = "Contact:";
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(482, 323);
            Controls.Add(Contact_label);
            Controls.Add(Ok_button);
            Controls.Add(email_linkLabel);
            Controls.Add(AppName_label);
            Name = "AboutForm";
            Text = "About";
            Load += about_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label AppName_label;
        private LinkLabel email_linkLabel;
        private Button Ok_button;
        private Label Contact_label;
    }
}