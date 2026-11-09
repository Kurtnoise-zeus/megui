namespace MeGUI.packages.audio.gcl
{
    partial class GCLConfigurationPanel
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblEncoderPath = new System.Windows.Forms.Label();
            this.tbEncoderPath = new System.Windows.Forms.TextBox();
            this.btnBrowseEncoder = new System.Windows.Forms.Button();
            this.lblExtension = new System.Windows.Forms.Label();
            this.tbExtension = new System.Windows.Forms.TextBox();
            this.lblInputHeader = new System.Windows.Forms.Label();
            this.cbInputHeader = new System.Windows.Forms.ComboBox();
            this.lblCommandLine = new System.Windows.Forms.Label();
            this.tbCommandLine = new System.Windows.Forms.TextBox();
            this.lblHelp = new System.Windows.Forms.Label();
            this.encoderGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // encoderGroupBox
            //
            this.encoderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.encoderGroupBox.Controls.Add(this.lblHelp);
            this.encoderGroupBox.Controls.Add(this.tbCommandLine);
            this.encoderGroupBox.Controls.Add(this.lblCommandLine);
            this.encoderGroupBox.Controls.Add(this.cbInputHeader);
            this.encoderGroupBox.Controls.Add(this.lblInputHeader);
            this.encoderGroupBox.Controls.Add(this.tbExtension);
            this.encoderGroupBox.Controls.Add(this.lblExtension);
            this.encoderGroupBox.Controls.Add(this.btnBrowseEncoder);
            this.encoderGroupBox.Controls.Add(this.tbEncoderPath);
            this.encoderGroupBox.Controls.Add(this.lblEncoderPath);
            this.encoderGroupBox.Size = new System.Drawing.Size(402, 184);
            this.encoderGroupBox.Text = " Generic Command Line Options ";
            //
            // lblEncoderPath
            //
            this.lblEncoderPath.AutoSize = true;
            this.lblEncoderPath.Location = new System.Drawing.Point(10, 25);
            this.lblEncoderPath.Name = "lblEncoderPath";
            this.lblEncoderPath.Size = new System.Drawing.Size(47, 13);
            this.lblEncoderPath.TabIndex = 0;
            this.lblEncoderPath.Text = "Encoder";
            //
            // tbEncoderPath
            //
            this.tbEncoderPath.Location = new System.Drawing.Point(88, 22);
            this.tbEncoderPath.Name = "tbEncoderPath";
            this.tbEncoderPath.Size = new System.Drawing.Size(230, 20);
            this.tbEncoderPath.TabIndex = 1;
            //
            // btnBrowseEncoder
            //
            this.btnBrowseEncoder.Location = new System.Drawing.Point(324, 20);
            this.btnBrowseEncoder.Name = "btnBrowseEncoder";
            this.btnBrowseEncoder.Size = new System.Drawing.Size(70, 23);
            this.btnBrowseEncoder.TabIndex = 2;
            this.btnBrowseEncoder.Text = "Browse...";
            this.btnBrowseEncoder.UseVisualStyleBackColor = true;
            this.btnBrowseEncoder.Click += new System.EventHandler(this.btnBrowseEncoder_Click);
            //
            // lblExtension
            //
            this.lblExtension.AutoSize = true;
            this.lblExtension.Location = new System.Drawing.Point(10, 53);
            this.lblExtension.Name = "lblExtension";
            this.lblExtension.Size = new System.Drawing.Size(76, 13);
            this.lblExtension.TabIndex = 3;
            this.lblExtension.Text = "Output extension";
            //
            // tbExtension
            //
            this.tbExtension.Location = new System.Drawing.Point(88, 50);
            this.tbExtension.Name = "tbExtension";
            this.tbExtension.Size = new System.Drawing.Size(90, 20);
            this.tbExtension.TabIndex = 4;
            this.tbExtension.Leave += new System.EventHandler(this.tbExtension_Leave);
            //
            // lblInputHeader
            //
            this.lblInputHeader.AutoSize = true;
            this.lblInputHeader.Location = new System.Drawing.Point(190, 53);
            this.lblInputHeader.Name = "lblInputHeader";
            this.lblInputHeader.Size = new System.Drawing.Size(66, 13);
            this.lblInputHeader.TabIndex = 5;
            this.lblInputHeader.Text = "Input header";
            //
            // cbInputHeader
            //
            this.cbInputHeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInputHeader.FormattingEnabled = true;
            this.cbInputHeader.Location = new System.Drawing.Point(262, 50);
            this.cbInputHeader.Name = "cbInputHeader";
            this.cbInputHeader.Size = new System.Drawing.Size(132, 21);
            this.cbInputHeader.TabIndex = 6;
            //
            // lblCommandLine
            //
            this.lblCommandLine.AutoSize = true;
            this.lblCommandLine.Location = new System.Drawing.Point(10, 81);
            this.lblCommandLine.Name = "lblCommandLine";
            this.lblCommandLine.Size = new System.Drawing.Size(71, 13);
            this.lblCommandLine.TabIndex = 7;
            this.lblCommandLine.Text = "Command line";
            //
            // tbCommandLine
            //
            this.tbCommandLine.AcceptsReturn = false;
            this.tbCommandLine.Location = new System.Drawing.Point(88, 78);
            this.tbCommandLine.Multiline = true;
            this.tbCommandLine.Name = "tbCommandLine";
            this.tbCommandLine.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCommandLine.Size = new System.Drawing.Size(306, 54);
            this.tbCommandLine.TabIndex = 8;
            //
            // lblHelp
            //
            this.lblHelp.Location = new System.Drawing.Point(10, 138);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(384, 40);
            this.lblHelp.TabIndex = 9;
            this.lblHelp.Text = "The audio is sent to the standard input of the encoder with the selected input header. Placeholders: {output} {samplerate} {bits} {channels} {samples} {size}";
            //
            // GCLConfigurationPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "GCLConfigurationPanel";
            this.Size = new System.Drawing.Size(411, 408);
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblEncoderPath;
        private System.Windows.Forms.TextBox tbEncoderPath;
        private System.Windows.Forms.Button btnBrowseEncoder;
        private System.Windows.Forms.Label lblExtension;
        private System.Windows.Forms.TextBox tbExtension;
        private System.Windows.Forms.Label lblInputHeader;
        private System.Windows.Forms.ComboBox cbInputHeader;
        private System.Windows.Forms.Label lblCommandLine;
        private System.Windows.Forms.TextBox tbCommandLine;
        private System.Windows.Forms.Label lblHelp;
    }
}
