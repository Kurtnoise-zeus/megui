namespace MeGUI.packages.demuxer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemuxWindow));
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.btnRemove = new MeGUI.core.gui.RoundedButton();
            this.btnOpen = new MeGUI.core.gui.RoundedButton();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.grpTracks = new System.Windows.Forms.GroupBox();
            this.btnSelectNone = new MeGUI.core.gui.RoundedButton();
            this.btnSelectAll = new MeGUI.core.gui.RoundedButton();
            this.clbTracks = new System.Windows.Forms.CheckedListBox();
            this.btnQueue = new MeGUI.core.gui.RoundedButton();
            this.chkCloseOnQueue = new MeGUI.core.gui.ToggleSwitch();
            this.grpInput.SuspendLayout();
            this.grpTracks.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.btnRemove);
            this.grpInput.Controls.Add(this.btnOpen);
            this.grpInput.Controls.Add(this.txtInputFile);
            this.grpInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpInput.Location = new System.Drawing.Point(5, 5);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(524, 55);
            this.grpInput.TabIndex = 0;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Input File";
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(441, 20);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(358, 20);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open...";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtInputFile
            // 
            this.txtInputFile.AllowDrop = true;
            this.txtInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFile.Location = new System.Drawing.Point(10, 22);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.ReadOnly = true;
            this.txtInputFile.Size = new System.Drawing.Size(340, 20);
            this.txtInputFile.TabIndex = 0;
            this.txtInputFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtInputFile_DragDrop);
            this.txtInputFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtInputFile_DragEnter);
            // 
            // grpTracks
            // 
            this.grpTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTracks.Controls.Add(this.btnSelectNone);
            this.grpTracks.Controls.Add(this.btnSelectAll);
            this.grpTracks.Controls.Add(this.clbTracks);
            this.grpTracks.Location = new System.Drawing.Point(5, 65);
            this.grpTracks.Name = "grpTracks";
            this.grpTracks.Size = new System.Drawing.Size(524, 260);
            this.grpTracks.TabIndex = 1;
            this.grpTracks.TabStop = false;
            this.grpTracks.Text = "Tracks";
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectNone.Location = new System.Drawing.Point(91, 228);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(75, 23);
            this.btnSelectNone.TabIndex = 2;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(10, 228);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // clbTracks
            // 
            this.clbTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbTracks.CheckOnClick = true;
            this.clbTracks.FormattingEnabled = true;
            this.clbTracks.IntegralHeight = false;
            this.clbTracks.Location = new System.Drawing.Point(10, 22);
            this.clbTracks.Name = "clbTracks";
            this.clbTracks.Size = new System.Drawing.Size(504, 200);
            this.clbTracks.TabIndex = 0;
            // 
            // btnQueue
            // 
            this.btnQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQueue.Enabled = false;
            this.btnQueue.Location = new System.Drawing.Point(449, 335);
            this.btnQueue.Name = "btnQueue";
            this.btnQueue.Size = new System.Drawing.Size(80, 23);
            this.btnQueue.TabIndex = 2;
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
            this.chkCloseOnQueue.Location = new System.Drawing.Point(332, 339);
            this.chkCloseOnQueue.Name = "chkCloseOnQueue";
            this.chkCloseOnQueue.Size = new System.Drawing.Size(111, 17);
            this.chkCloseOnQueue.TabIndex = 3;
            this.chkCloseOnQueue.Text = "Close after Queue";
            this.chkCloseOnQueue.UseVisualStyleBackColor = true;
            // 
            // DemuxWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 366);
            this.Controls.Add(this.chkCloseOnQueue);
            this.Controls.Add(this.btnQueue);
            this.Controls.Add(this.grpTracks);
            this.Controls.Add(this.grpInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(450, 350);
            this.Name = "DemuxWindow";
            this.Padding = new System.Windows.Forms.Padding(5);
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
        private MeGUI.core.gui.RoundedButton btnRemove;
        private MeGUI.core.gui.RoundedButton btnOpen;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.GroupBox grpTracks;
        private MeGUI.core.gui.RoundedButton btnSelectNone;
        private MeGUI.core.gui.RoundedButton btnSelectAll;
        private System.Windows.Forms.CheckedListBox clbTracks;
        private MeGUI.core.gui.RoundedButton btnQueue;
        private MeGUI.core.gui.ToggleSwitch chkCloseOnQueue;
    }
}