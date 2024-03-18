namespace MeGUI
{
    partial class ProgressWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressWindow));
            this.currentVideoFrameLabel = new System.Windows.Forms.Label();
            this.currentVideoFrame = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.currentFPS = new System.Windows.Forms.TextBox();
            this.estimatedFileSize = new System.Windows.Forms.TextBox();
            this.totalVideoFrame = new System.Windows.Forms.TextBox();
            this.positionInClipTotal = new System.Windows.Forms.TextBox();
            this.totalTime = new System.Windows.Forms.TextBox();
            this.progressLabel = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.timeElapsed = new System.Windows.Forms.TextBox();
            this.timeElapsedLabel = new System.Windows.Forms.Label();
            this.overallFPS = new System.Windows.Forms.TextBox();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.currentFileSize = new System.Windows.Forms.TextBox();
            this.videoDataLabel = new System.Windows.Forms.Label();
            this.currentPositionLabel = new System.Windows.Forms.Label();
            this.positionInClip = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.jobNameLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.actionLabel = new System.Windows.Forms.Label();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.priority = new System.Windows.Forms.ComboBox();
            this.abortButton = new System.Windows.Forms.Button();
            this.btnSuspend = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentVideoFrameLabel
            // 
            this.currentVideoFrameLabel.AutoSize = true;
            this.currentVideoFrameLabel.Location = new System.Drawing.Point(8, 44);
            this.currentVideoFrameLabel.Name = "currentVideoFrameLabel";
            this.currentVideoFrameLabel.Size = new System.Drawing.Size(118, 13);
            this.currentVideoFrameLabel.TabIndex = 2;
            this.currentVideoFrameLabel.Text = "Current / Total frames:";
            // 
            // currentVideoFrame
            // 
            this.currentVideoFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentVideoFrame.Location = new System.Drawing.Point(160, 41);
            this.currentVideoFrame.Name = "currentVideoFrame";
            this.currentVideoFrame.ReadOnly = true;
            this.currentVideoFrame.Size = new System.Drawing.Size(70, 21);
            this.currentVideoFrame.TabIndex = 17;
            this.currentVideoFrame.TabStop = false;
            this.currentVideoFrame.Text = "---";
            this.currentVideoFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.currentFPS);
            this.groupBox1.Controls.Add(this.estimatedFileSize);
            this.groupBox1.Controls.Add(this.totalVideoFrame);
            this.groupBox1.Controls.Add(this.positionInClipTotal);
            this.groupBox1.Controls.Add(this.totalTime);
            this.groupBox1.Controls.Add(this.progressLabel);
            this.groupBox1.Controls.Add(this.progress);
            this.groupBox1.Controls.Add(this.timeElapsed);
            this.groupBox1.Controls.Add(this.timeElapsedLabel);
            this.groupBox1.Controls.Add(this.overallFPS);
            this.groupBox1.Controls.Add(this.fpsLabel);
            this.groupBox1.Controls.Add(this.currentFileSize);
            this.groupBox1.Controls.Add(this.videoDataLabel);
            this.groupBox1.Controls.Add(this.currentPositionLabel);
            this.groupBox1.Controls.Add(this.currentVideoFrame);
            this.groupBox1.Controls.Add(this.currentVideoFrameLabel);
            this.groupBox1.Controls.Add(this.positionInClip);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 177);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            //
            // currentFPS
            // 
            this.currentFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentFPS.Location = new System.Drawing.Point(160, 95);
            this.currentFPS.Name = "currentFPS";
            this.currentFPS.ReadOnly = true;
            this.currentFPS.Size = new System.Drawing.Size(69, 21);
            this.currentFPS.TabIndex = 29;
            this.currentFPS.TabStop = false;
            this.currentFPS.Text = "---";
            this.currentFPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // estimatedFileSize
            // 
            this.estimatedFileSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.estimatedFileSize.Location = new System.Drawing.Point(233, 68);
            this.estimatedFileSize.Name = "estimatedFileSize";
            this.estimatedFileSize.ReadOnly = true;
            this.estimatedFileSize.Size = new System.Drawing.Size(69, 21);
            this.estimatedFileSize.TabIndex = 28;
            this.estimatedFileSize.TabStop = false;
            this.estimatedFileSize.Text = "---";
            this.estimatedFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // totalVideoFrame
            // 
            this.totalVideoFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalVideoFrame.Location = new System.Drawing.Point(233, 41);
            this.totalVideoFrame.Name = "totalVideoFrame";
            this.totalVideoFrame.ReadOnly = true;
            this.totalVideoFrame.Size = new System.Drawing.Size(69, 21);
            this.totalVideoFrame.TabIndex = 27;
            this.totalVideoFrame.TabStop = false;
            this.totalVideoFrame.Text = "---";
            this.totalVideoFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // positionInClipTotal
            // 
            this.positionInClipTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.positionInClipTotal.Location = new System.Drawing.Point(233, 14);
            this.positionInClipTotal.Name = "positionInClipTotal";
            this.positionInClipTotal.ReadOnly = true;
            this.positionInClipTotal.Size = new System.Drawing.Size(69, 21);
            this.positionInClipTotal.TabIndex = 26;
            this.positionInClipTotal.TabStop = false;
            this.positionInClipTotal.Text = "---";
            this.positionInClipTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // totalTime
            // 
            this.totalTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalTime.Location = new System.Drawing.Point(233, 122);
            this.totalTime.Name = "totalTime";
            this.totalTime.ReadOnly = true;
            this.totalTime.Size = new System.Drawing.Size(69, 21);
            this.totalTime.TabIndex = 25;
            this.totalTime.TabStop = false;
            this.totalTime.Text = "---";
            this.totalTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // progressLabel
            // 
            this.progressLabel.Location = new System.Drawing.Point(8, 151);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(100, 15);
            this.progressLabel.TabIndex = 1;
            this.progressLabel.Text = "Progress";
            // 
            // progress
            // 
            this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progress.Location = new System.Drawing.Point(160, 149);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(142, 21);
            this.progress.TabIndex = 1;
            // 
            // timeElapsed
            // 
            this.timeElapsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timeElapsed.Location = new System.Drawing.Point(160, 122);
            this.timeElapsed.Name = "timeElapsed";
            this.timeElapsed.ReadOnly = true;
            this.timeElapsed.Size = new System.Drawing.Size(70, 21);
            this.timeElapsed.TabIndex = 23;
            this.timeElapsed.TabStop = false;
            this.timeElapsed.Text = "00:00:00";
            this.timeElapsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timeElapsedLabel
            // 
            this.timeElapsedLabel.AutoSize = true;
            this.timeElapsedLabel.Location = new System.Drawing.Point(8, 125);
            this.timeElapsedLabel.Name = "timeElapsedLabel";
            this.timeElapsedLabel.Size = new System.Drawing.Size(130, 13);
            this.timeElapsedLabel.TabIndex = 8;
            this.timeElapsedLabel.Text = "Elapsed / Remaining time:";
            // 
            // overallFPS
            // 
            this.overallFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.overallFPS.Location = new System.Drawing.Point(233, 95);
            this.overallFPS.Name = "overallFPS";
            this.overallFPS.ReadOnly = true;
            this.overallFPS.Size = new System.Drawing.Size(70, 21);
            this.overallFPS.TabIndex = 21;
            this.overallFPS.TabStop = false;
            this.overallFPS.Text = "---";
            this.overallFPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(8, 98);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(131, 13);
            this.fpsLabel.TabIndex = 6;
            this.fpsLabel.Text = "Current / Average speed:";
            // 
            // currentFileSize
            // 
            this.currentFileSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentFileSize.Location = new System.Drawing.Point(160, 68);
            this.currentFileSize.Name = "currentFileSize";
            this.currentFileSize.ReadOnly = true;
            this.currentFileSize.Size = new System.Drawing.Size(70, 21);
            this.currentFileSize.TabIndex = 19;
            this.currentFileSize.TabStop = false;
            this.currentFileSize.Text = "---";
            this.currentFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // videoDataLabel
            // 
            this.videoDataLabel.AutoSize = true;
            this.videoDataLabel.Location = new System.Drawing.Point(8, 71);
            this.videoDataLabel.Name = "videoDataLabel";
            this.videoDataLabel.Size = new System.Drawing.Size(140, 13);
            this.videoDataLabel.TabIndex = 4;
            this.videoDataLabel.Text = "Current / Estimated filesize:";
            // 
            // currentPositionLabel
            // 
            this.currentPositionLabel.AutoSize = true;
            this.currentPositionLabel.Location = new System.Drawing.Point(8, 17);
            this.currentPositionLabel.Name = "currentPositionLabel";
            this.currentPositionLabel.Size = new System.Drawing.Size(115, 13);
            this.currentPositionLabel.TabIndex = 0;
            this.currentPositionLabel.Text = "Current / Total length:";
            // 
            // positionInClip
            // 
            this.positionInClip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.positionInClip.Location = new System.Drawing.Point(160, 14);
            this.positionInClip.Name = "positionInClip";
            this.positionInClip.ReadOnly = true;
            this.positionInClip.Size = new System.Drawing.Size(70, 21);
            this.positionInClip.TabIndex = 15;
            this.positionInClip.TabStop = false;
            this.positionInClip.Text = "---";
            this.positionInClip.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jobNameLabel,
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 261);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(321, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // jobNameLabel
            // 
            this.jobNameLabel.Name = "jobNameLabel";
            this.jobNameLabel.Size = new System.Drawing.Size(38, 17);
            this.jobNameLabel.Text = "[job1]";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(45, 17);
            this.statusLabel.Text = "Status: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.actionLabel);
            this.groupBox2.Controls.Add(this.priorityLabel);
            this.groupBox2.Controls.Add(this.priority);
            this.groupBox2.Controls.Add(this.abortButton);
            this.groupBox2.Controls.Add(this.btnSuspend);
            this.groupBox2.Location = new System.Drawing.Point(8, 181);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 74);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            // 
            // actionLabel
            // 
            this.actionLabel.Location = new System.Drawing.Point(8, 51);
            this.actionLabel.Name = "actionLabel";
            this.actionLabel.Size = new System.Drawing.Size(100, 15);
            this.actionLabel.TabIndex = 13;
            this.actionLabel.Text = "Job action";
            // 
            // priorityLabel
            // 
            this.priorityLabel.Location = new System.Drawing.Point(8, 23);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(100, 15);
            this.priorityLabel.TabIndex = 12;
            this.priorityLabel.Text = "Priority";
            // 
            // priority
            // 
            this.priority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priority.Items.AddRange(new object[] {
            "LOW",
            "BELOW NORMAL",
            "NORMAL",
            "ABOVE NORMAL"});
            this.priority.Location = new System.Drawing.Point(160, 20);
            this.priority.Name = "priority";
            this.priority.Size = new System.Drawing.Size(142, 21);
            this.priority.TabIndex = 11;
            this.priority.SelectedIndexChanged += new System.EventHandler(this.priority_SelectedIndexChanged);
            //
            // abortButton
            // 
            this.abortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.abortButton.Location = new System.Drawing.Point(233, 47);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(69, 21);
            this.abortButton.TabIndex = 10;
            this.abortButton.Text = "Abort";
            this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // btnSuspend
            // 
            this.btnSuspend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSuspend.Location = new System.Drawing.Point(160, 47);
            this.btnSuspend.Name = "btnSuspend";
            this.btnSuspend.Size = new System.Drawing.Size(67, 21);
            this.btnSuspend.TabIndex = 9;
            this.btnSuspend.Text = "Suspend";
            this.btnSuspend.Click += new System.EventHandler(this.btnSuspend_Click);
            // 
            // ProgressWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(321, 283);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProgressWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Status";
            this.VisibleChanged += new System.EventHandler(this.ProgressWindow_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public event AbortCallback Abort; // event fired if the abort button has been pressed
        public event SuspendCallback Suspend; // event fired if the suspend button has been pressed
        public event PriorityChangedCallback PriorityChanged; // event fired if the priority dropdown has changed
        private System.Windows.Forms.Label currentVideoFrameLabel;
        private System.Windows.Forms.TextBox currentVideoFrame;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label videoDataLabel;
        private System.Windows.Forms.TextBox currentFileSize;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.TextBox overallFPS;
        private System.Windows.Forms.Label timeElapsedLabel;
        private System.Windows.Forms.TextBox timeElapsed;
        private System.Windows.Forms.TextBox totalTime;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.TextBox positionInClip;
        private System.Windows.Forms.Label currentPositionLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private ITaskbarList3 taskbarProgress;
        private System.Windows.Forms.ToolStripStatusLabel jobNameLabel;
        private System.Windows.Forms.TextBox estimatedFileSize;
        private System.Windows.Forms.TextBox totalVideoFrame;
        private System.Windows.Forms.TextBox positionInClipTotal;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label priorityLabel;
        private System.Windows.Forms.ComboBox priority;
        private System.Windows.Forms.Button abortButton;
        private System.Windows.Forms.Button btnSuspend;
        private System.Windows.Forms.TextBox currentFPS;
        private System.Windows.Forms.Label actionLabel;
    }
}