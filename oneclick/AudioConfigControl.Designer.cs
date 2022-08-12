namespace MeGUI.packages.tools.oneclick
{
    partial class AudioConfigControl
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
            this.label3 = new System.Windows.Forms.Label();
            this.delay = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAudioEncoding = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.audioProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            ((System.ComponentModel.ISupportInitialize)(this.delay)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(249, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "ms";
            // 
            // delay
            // 
            this.delay.Location = new System.Drawing.Point(107, 64);
            this.delay.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.delay.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.delay.Name = "delay";
            this.delay.Size = new System.Drawing.Size(136, 20);
            this.delay.TabIndex = 43;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Delay";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Codec";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbAudioEncoding
            // 
            this.cbAudioEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAudioEncoding.FormattingEnabled = true;
            this.cbAudioEncoding.Items.AddRange(new object[] {
            "always",
            "if codec does not match",
            "never"});
            this.cbAudioEncoding.Location = new System.Drawing.Point(188, 31);
            this.cbAudioEncoding.Name = "cbAudioEncoding";
            this.cbAudioEncoding.Size = new System.Drawing.Size(178, 21);
            this.cbAudioEncoding.TabIndex = 46;
            this.cbAudioEncoding.SelectedIndexChanged += new System.EventHandler(this.dontEncodeAudio_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 47;
            this.label4.Text = "encode audio: ";
            // 
            // audioProfile
            // 
            this.audioProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.audioProfile.Location = new System.Drawing.Point(107, 3);
            this.audioProfile.Name = "audioProfile";
            this.audioProfile.ProfileSet = "Audio";
            this.audioProfile.Size = new System.Drawing.Size(295, 22);
            this.audioProfile.TabIndex = 45;
            this.audioProfile.SelectedProfileChanged += new System.EventHandler(this.ProfileChanged);
            // 
            // AudioConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbAudioEncoding);
            this.Controls.Add(this.audioProfile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.delay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AudioConfigControl";
            this.Size = new System.Drawing.Size(416, 90);
            ((System.ComponentModel.ISupportInitialize)(this.delay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown delay;
        private System.Windows.Forms.Label label2;
        private MeGUI.core.gui.ConfigableProfilesControl audioProfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbAudioEncoding;
        private System.Windows.Forms.Label label4;
    }
}
