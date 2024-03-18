namespace MeGUI.core.gui
{
    partial class JobQueue
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
            System.Windows.Forms.Button abortButton;
            System.Windows.Forms.Button deleteButton;
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.queueContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PostponedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WaitingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AbortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.startStopButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.editJobButton = new System.Windows.Forms.Button();
            this.jobColumHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inputColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.outputColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.codecHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.modeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.startColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.endColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fpsColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.queueListView = new System.Windows.Forms.ListView();
            abortButton = new System.Windows.Forms.Button();
            deleteButton = new System.Windows.Forms.Button();
            this.queueContextMenu.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // abortButton
            // 
            abortButton.AutoSize = true;
            abortButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            abortButton.Location = new System.Drawing.Point(123, 3);
            abortButton.Margin = new System.Windows.Forms.Padding(3, 3, 7, 3);
            abortButton.Name = "abortButton";
            abortButton.Size = new System.Drawing.Size(42, 23);
            abortButton.TabIndex = 3;
            abortButton.Text = "Abort";
            abortButton.UseVisualStyleBackColor = true;
            abortButton.Click += new System.EventHandler(this.AbortButton_Click);
            // 
            // deleteButton
            // 
            deleteButton.AutoSize = true;
            deleteButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            deleteButton.Location = new System.Drawing.Point(316, 3);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new System.Drawing.Size(48, 23);
            deleteButton.TabIndex = 8;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += new System.EventHandler(this.DeleteJobButton_Click);
            // 
            // upButton
            // 
            this.upButton.AutoSize = true;
            this.upButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.upButton.Enabled = false;
            this.upButton.Location = new System.Drawing.Point(224, 3);
            this.upButton.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(31, 23);
            this.upButton.TabIndex = 6;
            this.upButton.Text = "Up";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // downButton
            // 
            this.downButton.AutoSize = true;
            this.downButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.downButton.Enabled = false;
            this.downButton.Location = new System.Drawing.Point(261, 3);
            this.downButton.Margin = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(45, 23);
            this.downButton.TabIndex = 7;
            this.downButton.Text = "Down";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // queueContextMenu
            // 
            this.queueContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteMenuItem,
            this.StatusMenuItem,
            this.AbortMenuItem,
            this.EditMenuItem,
            this.OpenMenuItem});
            this.queueContextMenu.Name = "queueContextMenu";
            this.queueContextMenu.Size = new System.Drawing.Size(150, 114);
            this.queueContextMenu.Opened += new System.EventHandler(this.QueueContextMenu_Opened);
            // 
            // DeleteMenuItem
            // 
            this.DeleteMenuItem.Name = "DeleteMenuItem";
            this.DeleteMenuItem.ShortcutKeyDisplayString = "";
            this.DeleteMenuItem.Size = new System.Drawing.Size(149, 22);
            this.DeleteMenuItem.Text = "&Delete";
            this.DeleteMenuItem.ToolTipText = "Delete this job";
            this.DeleteMenuItem.Click += new System.EventHandler(this.DeleteJobButton_Click);
            // 
            // StatusMenuItem
            // 
            this.StatusMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PostponedMenuItem,
            this.WaitingMenuItem});
            this.StatusMenuItem.Name = "StatusMenuItem";
            this.StatusMenuItem.Size = new System.Drawing.Size(149, 22);
            this.StatusMenuItem.Text = "&Change status";
            // 
            // PostponedMenuItem
            // 
            this.PostponedMenuItem.Name = "PostponedMenuItem";
            this.PostponedMenuItem.Size = new System.Drawing.Size(131, 22);
            this.PostponedMenuItem.Text = "&Postponed";
            this.PostponedMenuItem.Click += new System.EventHandler(this.PostponeMenuItem_Click);
            // 
            // WaitingMenuItem
            // 
            this.WaitingMenuItem.Name = "WaitingMenuItem";
            this.WaitingMenuItem.Size = new System.Drawing.Size(131, 22);
            this.WaitingMenuItem.Text = "&Waiting";
            this.WaitingMenuItem.Click += new System.EventHandler(this.WaitingMenuItem_Click);
            // 
            // AbortMenuItem
            // 
            this.AbortMenuItem.Name = "AbortMenuItem";
            this.AbortMenuItem.ShortcutKeyDisplayString = "";
            this.AbortMenuItem.Size = new System.Drawing.Size(149, 22);
            this.AbortMenuItem.Text = "&Abort";
            this.AbortMenuItem.ToolTipText = "Abort this job";
            this.AbortMenuItem.Click += new System.EventHandler(this.AbortMenuItem_Click);
            // 
            // EditMenuItem
            // 
            this.EditMenuItem.Enabled = false;
            this.EditMenuItem.Name = "EditMenuItem";
            this.EditMenuItem.ShortcutKeyDisplayString = "";
            this.EditMenuItem.Size = new System.Drawing.Size(149, 22);
            this.EditMenuItem.Text = "&Edit";
            this.EditMenuItem.ToolTipText = "Edit job\r\nOnly possible if only one job is selected which is waiting or postponed" +
    "";
            this.EditMenuItem.Click += new System.EventHandler(this.editJobButton_Click);
            // 
            // OpenMenuItem
            // 
            this.OpenMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputFileToolStripMenuItem,
            this.inputFolderToolStripMenuItem,
            this.outputFileToolStripMenuItem,
            this.outputFolderToolStripMenuItem});
            this.OpenMenuItem.Name = "OpenMenuItem";
            this.OpenMenuItem.Size = new System.Drawing.Size(149, 22);
            this.OpenMenuItem.Text = "&Open";
            // 
            // inputFileToolStripMenuItem
            // 
            this.inputFileToolStripMenuItem.Name = "inputFileToolStripMenuItem";
            this.inputFileToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.inputFileToolStripMenuItem.Text = "Input File";
            this.inputFileToolStripMenuItem.Click += new System.EventHandler(this.InputFileToolStripMenuItem_Click);
            // 
            // inputFolderToolStripMenuItem
            // 
            this.inputFolderToolStripMenuItem.Name = "inputFolderToolStripMenuItem";
            this.inputFolderToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.inputFolderToolStripMenuItem.Text = "Input Folder";
            this.inputFolderToolStripMenuItem.Click += new System.EventHandler(this.InputFolderToolStripMenuItem_Click);
            // 
            // outputFileToolStripMenuItem
            // 
            this.outputFileToolStripMenuItem.Name = "outputFileToolStripMenuItem";
            this.outputFileToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.outputFileToolStripMenuItem.Text = "Output File";
            this.outputFileToolStripMenuItem.Click += new System.EventHandler(this.OutputFileToolStripMenuItem_Click);
            // 
            // outputFolderToolStripMenuItem
            // 
            this.outputFolderToolStripMenuItem.Name = "outputFolderToolStripMenuItem";
            this.outputFolderToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.outputFolderToolStripMenuItem.Text = "Output Folder";
            this.outputFolderToolStripMenuItem.Click += new System.EventHandler(this.OutputFolderToolStripMenuItem_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.startStopButton);
            this.flowLayoutPanel1.Controls.Add(this.stopButton);
            this.flowLayoutPanel1.Controls.Add(this.pauseButton);
            this.flowLayoutPanel1.Controls.Add(abortButton);
            this.flowLayoutPanel1.Controls.Add(this.editJobButton);
            this.flowLayoutPanel1.Controls.Add(this.upButton);
            this.flowLayoutPanel1.Controls.Add(this.downButton);
            this.flowLayoutPanel1.Controls.Add(deleteButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 513);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(692, 29);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // startStopButton
            // 
            this.startStopButton.AutoSize = true;
            this.startStopButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.startStopButton.Location = new System.Drawing.Point(3, 3);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(39, 23);
            this.startStopButton.TabIndex = 0;
            this.startStopButton.Text = "Start";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.AutoSize = true;
            this.stopButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.stopButton.Location = new System.Drawing.Point(48, 3);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(39, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(93, 3);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(24, 23);
            this.pauseButton.TabIndex = 2;
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Visible = false;
            // 
            // editJobButton
            // 
            this.editJobButton.AutoSize = true;
            this.editJobButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.editJobButton.Enabled = false;
            this.editJobButton.Location = new System.Drawing.Point(179, 3);
            this.editJobButton.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.editJobButton.Name = "editJobButton";
            this.editJobButton.Size = new System.Drawing.Size(35, 23);
            this.editJobButton.TabIndex = 4;
            this.editJobButton.Text = "Edit";
            this.editJobButton.UseVisualStyleBackColor = true;
            this.editJobButton.Click += new System.EventHandler(this.editJobButton_Click);
            // 
            // jobColumHeader
            // 
            this.jobColumHeader.Text = "Name";
            // 
            // inputColumnHeader
            // 
            this.inputColumnHeader.Text = "Input";
            // 
            // outputColumnHeader
            // 
            this.outputColumnHeader.Text = "Output";
            // 
            // codecHeader
            // 
            this.codecHeader.Text = "Encoder";
            // 
            // modeHeader
            // 
            this.modeHeader.Text = "Mode";
            // 
            // statusColumn
            // 
            this.statusColumn.Text = "Status";
            // 
            // startColumn
            // 
            this.startColumn.Text = "Start";
            // 
            // endColumn
            // 
            this.endColumn.Text = "End";
            // 
            // fpsColumn
            // 
            this.fpsColumn.Text = "Speed";
            // 
            // queueListView
            // 
            this.queueListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.jobColumHeader,
            this.inputColumnHeader,
            this.outputColumnHeader,
            this.codecHeader,
            this.modeHeader,
            this.statusColumn,
            this.startColumn,
            this.endColumn,
            this.fpsColumn});
            this.queueListView.ContextMenuStrip = this.queueContextMenu;
            this.queueListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueListView.FullRowSelect = true;
            this.queueListView.HideSelection = false;
            this.queueListView.Location = new System.Drawing.Point(0, 0);
            this.queueListView.Name = "queueListView";
            this.queueListView.Size = new System.Drawing.Size(692, 513);
            this.queueListView.TabIndex = 0;
            this.queueListView.UseCompatibleStateImageBehavior = false;
            this.queueListView.View = System.Windows.Forms.View.Details;
            this.queueListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.QueueListView_ColumnWidthChanged);
            this.queueListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.QueueListView_ItemSelectionChanged);
            this.queueListView.VisibleChanged += new System.EventHandler(this.QueueListView_VisibleChanged);
            this.queueListView.DoubleClick += new System.EventHandler(this.QueueListView_DoubleClick);
            this.queueListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.QueueListView_KeyDown);
            // 
            // JobQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.queueListView);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "JobQueue";
            this.Size = new System.Drawing.Size(692, 542);
            this.Load += new System.EventHandler(this.JobQueue_Load);
            this.queueContextMenu.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button startStopButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.ContextMenuStrip queueContextMenu;
        private System.Windows.Forms.ToolStripMenuItem DeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StatusMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PostponedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WaitingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AbortMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditMenuItem;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button editJobButton;
        private System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputFolderToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader jobColumHeader;
        private System.Windows.Forms.ColumnHeader inputColumnHeader;
        private System.Windows.Forms.ColumnHeader outputColumnHeader;
        private System.Windows.Forms.ColumnHeader codecHeader;
        private System.Windows.Forms.ColumnHeader modeHeader;
        private System.Windows.Forms.ColumnHeader statusColumn;
        private System.Windows.Forms.ColumnHeader startColumn;
        private System.Windows.Forms.ColumnHeader endColumn;
        private System.Windows.Forms.ColumnHeader fpsColumn;
        private System.Windows.Forms.ListView queueListView;
    }
}
