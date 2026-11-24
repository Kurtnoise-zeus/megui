namespace MeGUI
{
    partial class DemuxWindow
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
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.btnBrowseInput = new MeGUI.core.gui.RoundedButton();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.grpTracks = new System.Windows.Forms.GroupBox();
            this.clbTracks = new System.Windows.Forms.CheckedListBox();
            this.btnRemove = new MeGUI.core.gui.RoundedButton();
            this.btnQueue = new MeGUI.core.gui.RoundedButton();
            this.chkCloseOnQueue = new MeGUI.core.gui.ToggleSwitch();
            this.grpInput.SuspendLayout();
            this.grpTracks.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpInput
            // 
            this.grpInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInput.Controls.Add(this.btnBrowseInput);
            this.grpInput.Controls.Add(this.txtInputFile);
            this.grpInput.Location = new System.Drawing.Point(12, 12);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(460, 55);
            this.grpInput.TabIndex = 0;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Input File";
            // 
            // btnBrowseInput
            // 
            this.btnBrowseInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseInput.Location = new System.Drawing.Point(424, 21);
            this.btnBrowseInput.Name = "btnBrowseInput";
            this.btnBrowseInput.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseInput.TabIndex = 1;
            this.btnBrowseInput.Text = "...";
            this.btnBrowseInput.UseVisualStyleBackColor = true;
            this.btnBrowseInput.Click += new System.EventHandler(this.btnBrowseInput_Click);
            // 
            // txtInputFile
            // 
            this.txtInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFile.Location = new System.Drawing.Point(6, 22);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.ReadOnly = true;
            this.txtInputFile.Size = new System.Drawing.Size(412, 20);
            this.txtInputFile.TabIndex = 0;
            // 
            // grpTracks
            // 
            this.grpTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTracks.Controls.Add(this.clbTracks);
            this.grpTracks.Location = new System.Drawing.Point(12, 73);
            this.grpTracks.Name = "grpTracks";
            this.grpTracks.Size = new System.Drawing.Size(460, 230);
            this.grpTracks.TabIndex = 1;
            this.grpTracks.TabStop = false;
            this.grpTracks.Text = "Tracks";
            // 
            // clbTracks
            // 
            this.clbTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbTracks.CheckOnClick = true;
            this.clbTracks.FormattingEnabled = true;
            this.clbTracks.Location = new System.Drawing.Point(6, 19);
            this.clbTracks.Name = "clbTracks";
            this.clbTracks.Size = new System.Drawing.Size(448, 199);
            this.clbTracks.TabIndex = 0;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point(12, 315);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnQueue
            // 
            this.btnQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQueue.Location = new System.Drawing.Point(397, 315);
            this.btnQueue.Name = "btnQueue";
            this.btnQueue.Size = new System.Drawing.Size(75, 23);
            this.btnQueue.TabIndex = 3;
            this.btnQueue.Text = "Queue";
            this.btnQueue.UseVisualStyleBackColor = true;
            this.btnQueue.Click += new System.EventHandler(this.btnQueue_Click);
            // 
            // chkCloseOnQueue
            // 
            this.chkCloseOnQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCloseOnQueue.AutoSize = true;
            this.chkCloseOnQueue.Checked = true;
            this.chkCloseOnQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCloseOnQueue.Location = new System.Drawing.Point(282, 319);
            this.chkCloseOnQueue.Name = "chkCloseOnQueue";
            this.chkCloseOnQueue.Size = new System.Drawing.Size(109, 17);
            this.chkCloseOnQueue.TabIndex = 4;
            this.chkCloseOnQueue.Text = "Close on Queue";
            this.chkCloseOnQueue.UseVisualStyleBackColor = true;
            // 
            // DemuxWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(484, 350);
            this.Controls.Add(this.chkCloseOnQueue);
            this.Controls.Add(this.btnQueue);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.grpTracks);
            this.Controls.Add(this.grpInput);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "DemuxWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MeGUI - Demuxer";
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.grpTracks.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpInput;
        private MeGUI.core.gui.RoundedButton btnBrowseInput;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.GroupBox grpTracks;
        private System.Windows.Forms.CheckedListBox clbTracks;
        private MeGUI.core.gui.RoundedButton btnRemove;
        private MeGUI.core.gui.RoundedButton btnQueue;
        private MeGUI.core.gui.ToggleSwitch chkCloseOnQueue;
    }
}
