namespace MeGUI.core.gui
{
    partial class IndividualWorkerSummary
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
            this.workerNameAndJob = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startEncodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showProgressWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseResumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workerNameAndJob.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // workerNameAndJob
            // 
            this.workerNameAndJob.Controls.Add(this.progressBar1);
            this.workerNameAndJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workerNameAndJob.Location = new System.Drawing.Point(0, 0);
            this.workerNameAndJob.Name = "workerNameAndJob";
            this.workerNameAndJob.Padding = new System.Windows.Forms.Padding(5);
            this.workerNameAndJob.Size = new System.Drawing.Size(292, 47);
            this.workerNameAndJob.TabIndex = 0;
            this.workerNameAndJob.TabStop = false;
            this.workerNameAndJob.Text = "[Name]: [status]";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(5, 18);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(282, 24);
            this.progressBar1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startEncodingToolStripMenuItem,
            this.pauseResumeToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.abortToolStripMenuItem,
            this.toolStripSeparator1,
            this.showProgressWindowToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(288, 230);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // startEncodingToolStripMenuItem
            // 
            this.startEncodingToolStripMenuItem.Name = "startEncodingToolStripMenuItem";
            this.startEncodingToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.startEncodingToolStripMenuItem.Text = "Start worker";
            this.startEncodingToolStripMenuItem.Click += new System.EventHandler(this.StartEncodingToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.stopToolStripMenuItem.Text = "Stop worker after the current job";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.StopToolStripMenuItem_Click);
            // 
            // abortToolStripMenuItem
            // 
            this.abortToolStripMenuItem.Name = "abortToolStripMenuItem";
            this.abortToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.abortToolStripMenuItem.Text = "Abort job and stop worker immediately";
            this.abortToolStripMenuItem.Click += new System.EventHandler(this.AbortToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(278, 6);
            // 
            // showProgressWindowToolStripMenuItem
            // 
            this.showProgressWindowToolStripMenuItem.Name = "showProgressWindowToolStripMenuItem";
            this.showProgressWindowToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.showProgressWindowToolStripMenuItem.Text = "Show progress window";
            this.showProgressWindowToolStripMenuItem.Click += new System.EventHandler(this.ShowProgressWindowToolStripMenuItem_Click);
            // 
            // pauseResumeToolStripMenuItem
            // 
            this.pauseResumeToolStripMenuItem.Name = "pauseResumeToolStripMenuItem";
            this.pauseResumeToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.pauseResumeToolStripMenuItem.Text = "Suspend worker";
            this.pauseResumeToolStripMenuItem.Click += new System.EventHandler(this.PauseResumeToolStripMenuItem_Click);
            // 
            // IndividualWorkerSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.workerNameAndJob);
            this.MaximumSize = new System.Drawing.Size(1000, 47);
            this.MinimumSize = new System.Drawing.Size(0, 47);
            this.Name = "IndividualWorkerSummary";
            this.Size = new System.Drawing.Size(292, 47);
            this.workerNameAndJob.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox workerNameAndJob;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startEncodingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showProgressWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem pauseResumeToolStripMenuItem;
    }
}
