namespace MeGUI
{
    partial class VobSubIndexWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VobSubIndexWindow));
            this.inputGroupbox = new System.Windows.Forms.GroupBox();
            this.input = new MeGUI.FileBar();
            this.inputLabel = new System.Windows.Forms.Label();
            this.outputGroupbox = new System.Windows.Forms.GroupBox();
            this.output = new MeGUI.FileBar();
            this.nameLabel = new System.Windows.Forms.Label();
            this.subtitleGroupbox = new System.Windows.Forms.GroupBox();
            this.chkExtractForced = new System.Windows.Forms.CheckBox();
            this.chkKeepAllStreams = new System.Windows.Forms.CheckBox();
            this.chkSingleFileExport = new System.Windows.Forms.CheckBox();
            this.chkShowAllStreams = new System.Windows.Forms.CheckBox();
            this.subtitleTracks = new System.Windows.Forms.CheckedListBox();
            this.closeOnQueue = new System.Windows.Forms.CheckBox();
            this.queueButton = new System.Windows.Forms.Button();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.inputGroupbox.SuspendLayout();
            this.outputGroupbox.SuspendLayout();
            this.subtitleGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputGroupbox
            // 
            this.inputGroupbox.Controls.Add(this.input);
            this.inputGroupbox.Controls.Add(this.inputLabel);
            this.inputGroupbox.Location = new System.Drawing.Point(2, 2);
            this.inputGroupbox.Name = "inputGroupbox";
            this.inputGroupbox.Size = new System.Drawing.Size(424, 48);
            this.inputGroupbox.TabIndex = 1;
            this.inputGroupbox.TabStop = false;
            this.inputGroupbox.Text = "Input";
            // 
            // input
            // 
            this.input.Filename = "";
            this.input.Filter = "IFO Files|*.ifo";
            this.input.Location = new System.Drawing.Point(62, 16);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(344, 23);
            this.input.TabIndex = 5;
            this.input.FileSelected += new MeGUI.FileBarEventHandler(this.input_FileSelected);
            // 
            // inputLabel
            // 
            this.inputLabel.Location = new System.Drawing.Point(16, 20);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(100, 13);
            this.inputLabel.TabIndex = 0;
            this.inputLabel.Text = "Input";
            // 
            // outputGroupbox
            // 
            this.outputGroupbox.Controls.Add(this.output);
            this.outputGroupbox.Controls.Add(this.nameLabel);
            this.outputGroupbox.Location = new System.Drawing.Point(2, 309);
            this.outputGroupbox.Name = "outputGroupbox";
            this.outputGroupbox.Size = new System.Drawing.Size(424, 49);
            this.outputGroupbox.TabIndex = 13;
            this.outputGroupbox.TabStop = false;
            this.outputGroupbox.Text = "Output";
            // 
            // output
            // 
            this.output.Filename = "";
            this.output.Filter = "VobSub Files|*.idx";
            this.output.Location = new System.Drawing.Point(62, 17);
            this.output.Name = "output";
            this.output.SaveMode = true;
            this.output.Size = new System.Drawing.Size(344, 23);
            this.output.TabIndex = 5;
            this.output.Title = "Choose an output file";
            this.output.FileSelected += new MeGUI.FileBarEventHandler(this.output_FileSelected);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(16, 20);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(100, 13);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Ouput";
            // 
            // subtitleGroupbox
            // 
            this.subtitleGroupbox.Controls.Add(this.chkExtractForced);
            this.subtitleGroupbox.Controls.Add(this.chkKeepAllStreams);
            this.subtitleGroupbox.Controls.Add(this.chkSingleFileExport);
            this.subtitleGroupbox.Controls.Add(this.chkShowAllStreams);
            this.subtitleGroupbox.Controls.Add(this.subtitleTracks);
            this.subtitleGroupbox.Location = new System.Drawing.Point(2, 56);
            this.subtitleGroupbox.Name = "subtitleGroupbox";
            this.subtitleGroupbox.Size = new System.Drawing.Size(424, 247);
            this.subtitleGroupbox.TabIndex = 14;
            this.subtitleGroupbox.TabStop = false;
            this.subtitleGroupbox.Text = "Subtitles";
            // 
            // chkExtractForced
            // 
            this.chkExtractForced.AutoSize = true;
            this.chkExtractForced.Location = new System.Drawing.Point(19, 46);
            this.chkExtractForced.Name = "chkExtractForced";
            this.chkExtractForced.Size = new System.Drawing.Size(225, 17);
            this.chkExtractForced.TabIndex = 13;
            this.chkExtractForced.Text = "Extract forced subtitles in separate file(s)";
            this.chkExtractForced.UseVisualStyleBackColor = true;
            // 
            // chkKeepAllStreams
            // 
            this.chkKeepAllStreams.AutoSize = true;
            this.chkKeepAllStreams.Location = new System.Drawing.Point(19, 25);
            this.chkKeepAllStreams.Name = "chkKeepAllStreams";
            this.chkKeepAllStreams.Size = new System.Drawing.Size(142, 17);
            this.chkKeepAllStreams.TabIndex = 12;
            this.chkKeepAllStreams.Text = "Keep all subtitle streams";
            this.chkKeepAllStreams.UseVisualStyleBackColor = true;
            this.chkKeepAllStreams.CheckedChanged += new System.EventHandler(this.chkKeepAllStreams_CheckedChanged);
            // 
            // chkSingleFileExport
            // 
            this.chkSingleFileExport.AutoSize = true;
            this.chkSingleFileExport.Location = new System.Drawing.Point(262, 46);
            this.chkSingleFileExport.Name = "chkSingleFileExport";
            this.chkSingleFileExport.Size = new System.Drawing.Size(106, 17);
            this.chkSingleFileExport.TabIndex = 11;
            this.chkSingleFileExport.Text = "Single file output";
            this.chkSingleFileExport.UseVisualStyleBackColor = true;
            // 
            // chkShowAllStreams
            // 
            this.chkShowAllStreams.AutoSize = true;
            this.chkShowAllStreams.Location = new System.Drawing.Point(262, 25);
            this.chkShowAllStreams.Name = "chkShowAllStreams";
            this.chkShowAllStreams.Size = new System.Drawing.Size(144, 17);
            this.chkShowAllStreams.TabIndex = 10;
            this.chkShowAllStreams.Text = "Show all subtitle streams";
            this.chkShowAllStreams.UseVisualStyleBackColor = true;
            this.chkShowAllStreams.CheckedChanged += new System.EventHandler(this.chkShowAllStreams_CheckedChanged);
            // 
            // subtitleTracks
            // 
            this.subtitleTracks.CheckOnClick = true;
            this.subtitleTracks.FormattingEnabled = true;
            this.subtitleTracks.Location = new System.Drawing.Point(19, 72);
            this.subtitleTracks.Name = "subtitleTracks";
            this.subtitleTracks.Size = new System.Drawing.Size(387, 164);
            this.subtitleTracks.TabIndex = 9;
            // 
            // closeOnQueue
            // 
            this.closeOnQueue.Checked = true;
            this.closeOnQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.closeOnQueue.Location = new System.Drawing.Point(272, 364);
            this.closeOnQueue.Name = "closeOnQueue";
            this.closeOnQueue.Size = new System.Drawing.Size(72, 24);
            this.closeOnQueue.TabIndex = 16;
            this.closeOnQueue.Text = "and close";
            // 
            // queueButton
            // 
            this.queueButton.Location = new System.Drawing.Point(352, 364);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(74, 23);
            this.queueButton.TabIndex = 15;
            this.queueButton.Text = "Queue";
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Tools/VobSubber";
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(7, 364);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(47, 23);
            this.helpButton1.TabIndex = 17;
            // 
            // VobSubIndexWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(434, 393);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.closeOnQueue);
            this.Controls.Add(this.queueButton);
            this.Controls.Add(this.subtitleGroupbox);
            this.Controls.Add(this.outputGroupbox);
            this.Controls.Add(this.inputGroupbox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VobSubIndexWindow";
            this.Text = "MeGUI - VobSub Indexer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VobSubIndexWindow_FormClosed);
            this.inputGroupbox.ResumeLayout(false);
            this.outputGroupbox.ResumeLayout(false);
            this.subtitleGroupbox.ResumeLayout(false);
            this.subtitleGroupbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox inputGroupbox;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.GroupBox outputGroupbox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.GroupBox subtitleGroupbox;
        private System.Windows.Forms.CheckBox closeOnQueue;
        private System.Windows.Forms.Button queueButton;
        private System.Windows.Forms.CheckedListBox subtitleTracks;
        private MeGUI.core.gui.HelpButton helpButton1;
        private FileBar input;
        private FileBar output;
        private System.Windows.Forms.CheckBox chkShowAllStreams;
        private System.Windows.Forms.CheckBox chkSingleFileExport;
        private System.Windows.Forms.CheckBox chkKeepAllStreams;
        private System.Windows.Forms.CheckBox chkExtractForced;
    }
}