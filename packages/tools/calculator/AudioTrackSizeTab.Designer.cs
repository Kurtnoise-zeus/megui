namespace MeGUI.packages.tools.calculator
{
    partial class AudioTrackSizeTab
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioTrackSizeTab));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.audio1Bitrate = new System.Windows.Forms.NumericUpDown();
            this.selectButton = new System.Windows.Forms.Button();
            this.audio1Type = new System.Windows.Forms.ComboBox();
            this.audio1TypeLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.audioLabel = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.size = new System.Windows.Forms.TextBox();
            this.removeLink = new System.Windows.Forms.LinkLabel();
            this.removalToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.audio1Bitrate)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(354, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Size";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Bitrate";
            // 
            // audio1Bitrate
            // 
            this.audio1Bitrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.audio1Bitrate.Enabled = false;
            this.audio1Bitrate.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.audio1Bitrate.Location = new System.Drawing.Point(282, 28);
            this.audio1Bitrate.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.audio1Bitrate.Name = "audio1Bitrate";
            this.audio1Bitrate.Size = new System.Drawing.Size(66, 20);
            this.audio1Bitrate.TabIndex = 2;
            this.audio1Bitrate.ThousandsSeparator = true;
            this.audio1Bitrate.ValueChanged += new System.EventHandler(this.audio1Bitrate_ValueChanged);
            // 
            // selectButton
            // 
            this.selectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectButton.Location = new System.Drawing.Point(160, 27);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(24, 21);
            this.selectButton.TabIndex = 0;
            this.selectButton.Text = "...";
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // audio1Type
            // 
            this.audio1Type.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.audio1Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audio1Type.Location = new System.Drawing.Point(197, 27);
            this.audio1Type.Name = "audio1Type";
            this.audio1Type.Size = new System.Drawing.Size(75, 21);
            this.audio1Type.Sorted = true;
            this.audio1Type.TabIndex = 1;
            this.audio1Type.SelectedIndexChanged += new System.EventHandler(this.audio1Type_SelectedIndexChanged);
            // 
            // audio1TypeLabel
            // 
            this.audio1TypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.audio1TypeLabel.Location = new System.Drawing.Point(194, 8);
            this.audio1TypeLabel.Name = "audio1TypeLabel";
            this.audio1TypeLabel.Size = new System.Drawing.Size(40, 16);
            this.audio1TypeLabel.TabIndex = 26;
            this.audio1TypeLabel.Text = "Type";
            // 
            // audioLabel
            // 
            this.audioLabel.Location = new System.Drawing.Point(30, 8);
            this.audioLabel.Name = "audioLabel";
            this.audioLabel.Size = new System.Drawing.Size(40, 16);
            this.audioLabel.TabIndex = 31;
            this.audioLabel.Text = "Audio";
            // 
            // name
            // 
            this.name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.name.Location = new System.Drawing.Point(8, 28);
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Size = new System.Drawing.Size(150, 20);
            this.name.TabIndex = 32;
            this.name.TabStop = false;
            this.name.Enter += new System.EventHandler(this.selectButton_Click);
            // 
            // size
            // 
            this.size.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.size.Location = new System.Drawing.Point(357, 28);
            this.size.Name = "size";
            this.size.ReadOnly = true;
            this.size.Size = new System.Drawing.Size(70, 20);
            this.size.TabIndex = 3;
            this.size.TabStop = false;
            // 
            // removeLink
            // 
            this.removeLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.removeLink.Image = ((System.Drawing.Image)(resources.GetObject("removeLink.Image")));
            this.removeLink.Location = new System.Drawing.Point(5, 3);
            this.removeLink.Name = "removeLink";
            this.removeLink.Padding = new System.Windows.Forms.Padding(16, 0, 3, 3);
            this.removeLink.Size = new System.Drawing.Size(27, 23);
            this.removeLink.TabIndex = 4;
            this.removeLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removalToolTip.SetToolTip(this.removeLink, "Audio track");
            this.removeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.removeLink_LinkClicked);
            this.removeLink.Click += new System.EventHandler(this.removeLink_LinkClicked);
            // 
            // removalToolTip
            // 
            this.removalToolTip.AutomaticDelay = 300;
            this.removalToolTip.ToolTipTitle = "Remove";
            // 
            // AudioTrackSizeTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.removeLink);
            this.Controls.Add(this.size);
            this.Controls.Add(this.name);
            this.Controls.Add(this.audioLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.audio1Bitrate);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.audio1Type);
            this.Controls.Add(this.audio1TypeLabel);
            this.Name = "AudioTrackSizeTab";
            this.Size = new System.Drawing.Size(435, 50);
            this.Enter += new System.EventHandler(this.AudioTrackSizeTab_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.audio1Bitrate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown audio1Bitrate;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.ComboBox audio1Type;
        private System.Windows.Forms.Label audio1TypeLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label audioLabel;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox size;
        private System.Windows.Forms.LinkLabel removeLink;
        private System.Windows.Forms.ToolTip removalToolTip;
    }
}
