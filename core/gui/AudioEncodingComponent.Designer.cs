namespace MeGUI
{
    partial class AudioEncodingComponent
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.audioEncodingTab1 = new MeGUI.core.gui.AudioEncodingTab();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(515, 230);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Audio encoding ";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTrackToolStripMenuItem,
            this.removeTrackToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // newTrackToolStripMenuItem
            // 
            this.newTrackToolStripMenuItem.Name = "newTrackToolStripMenuItem";
            this.newTrackToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newTrackToolStripMenuItem.Text = "New track";
            this.newTrackToolStripMenuItem.Click += new System.EventHandler(this.newTrackToolStripMenuItem_Click);
            // 
            // removeTrackToolStripMenuItem
            // 
            this.removeTrackToolStripMenuItem.Name = "removeTrackToolStripMenuItem";
            this.removeTrackToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.removeTrackToolStripMenuItem.Text = "Remove track";
            this.removeTrackToolStripMenuItem.Click += new System.EventHandler(this.removeTrackToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(509, 211);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.audioEncodingTab1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(501, 185);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Track 1";
            // 
            // audioEncodingTab1
            // 
            this.audioEncodingTab1.AudioContainer = "";
            this.audioEncodingTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioEncodingTab1.Location = new System.Drawing.Point(3, 3);
            this.audioEncodingTab1.Name = "audioEncodingTab1";
            this.audioEncodingTab1.QueueButtonText = "Queue";
            this.audioEncodingTab1.ShowReset = true;
            this.audioEncodingTab1.Size = new System.Drawing.Size(495, 179);
            this.audioEncodingTab1.TabIndex = 0;
            // 
            // AudioEncodingComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox1);
            this.Name = "AudioEncodingComponent";
            this.Size = new System.Drawing.Size(515, 230);
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private MeGUI.core.gui.AudioEncodingTab audioEncodingTab1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newTrackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTrackToolStripMenuItem;


    }
}
