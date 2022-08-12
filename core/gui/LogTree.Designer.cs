namespace MeGUI.core.gui
{
    partial class LogTree
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
            this.treeView = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editIndividualNode = new System.Windows.Forms.ToolStripMenuItem();
            this.editBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.editLog = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLog = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllSubitemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandLog = new System.Windows.Forms.ToolStripMenuItem();
            this.expandBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllSubitemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseLog = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.resetOverlayIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.contextMenu;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(596, 478);
            this.treeView.TabIndex = 0;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editTextToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.expandAllSubitemsToolStripMenuItem,
            this.collapseAllSubitemsToolStripMenuItem,
            this.resetOverlayIcon});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(200, 136);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
            // 
            // editTextToolStripMenuItem
            // 
            this.editTextToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editIndividualNode,
            this.editBranch,
            this.editLog});
            this.editTextToolStripMenuItem.Name = "editTextToolStripMenuItem";
            this.editTextToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.editTextToolStripMenuItem.Text = "Edit text";
            // 
            // editIndividualNode
            // 
            this.editIndividualNode.Name = "editIndividualNode";
            this.editIndividualNode.Size = new System.Drawing.Size(111, 22);
            this.editIndividualNode.Text = "node";
            this.editIndividualNode.Click += new System.EventHandler(this.OfIndividualNodeToolStripMenuItem_Click);
            // 
            // editBranch
            // 
            this.editBranch.Name = "editBranch";
            this.editBranch.Size = new System.Drawing.Size(111, 22);
            this.editBranch.Text = "branch";
            this.editBranch.Click += new System.EventHandler(this.OfBranchToolStripMenuItem_Click);
            // 
            // editLog
            // 
            this.editLog.Name = "editLog";
            this.editLog.Size = new System.Drawing.Size(111, 22);
            this.editLog.Text = "log";
            this.editLog.Click += new System.EventHandler(this.EditLog_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveBranch,
            this.saveLog});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveBranch
            // 
            this.saveBranch.Name = "saveBranch";
            this.saveBranch.Size = new System.Drawing.Size(111, 22);
            this.saveBranch.Text = "branch";
            this.saveBranch.Click += new System.EventHandler(this.SaveBranch_Click);
            // 
            // saveLog
            // 
            this.saveLog.Name = "saveLog";
            this.saveLog.Size = new System.Drawing.Size(111, 22);
            this.saveLog.Text = "log";
            this.saveLog.Click += new System.EventHandler(this.SaveLog_Click);
            // 
            // expandAllSubitemsToolStripMenuItem
            // 
            this.expandAllSubitemsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandLog,
            this.expandBranch});
            this.expandAllSubitemsToolStripMenuItem.Name = "expandAllSubitemsToolStripMenuItem";
            this.expandAllSubitemsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.expandAllSubitemsToolStripMenuItem.Text = "Expand all subitems";
            // 
            // expandLog
            // 
            this.expandLog.Name = "expandLog";
            this.expandLog.Size = new System.Drawing.Size(125, 22);
            this.expandLog.Text = "of log";
            this.expandLog.Click += new System.EventHandler(this.ExpandLog_Click);
            // 
            // expandBranch
            // 
            this.expandBranch.Name = "expandBranch";
            this.expandBranch.Size = new System.Drawing.Size(125, 22);
            this.expandBranch.Text = "of branch";
            this.expandBranch.Click += new System.EventHandler(this.ExpandBranch_Click);
            // 
            // collapseAllSubitemsToolStripMenuItem
            // 
            this.collapseAllSubitemsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseLog,
            this.collapseBranch});
            this.collapseAllSubitemsToolStripMenuItem.Name = "collapseAllSubitemsToolStripMenuItem";
            this.collapseAllSubitemsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.collapseAllSubitemsToolStripMenuItem.Text = "Collapse all subitems";
            // 
            // collapseLog
            // 
            this.collapseLog.Name = "collapseLog";
            this.collapseLog.Size = new System.Drawing.Size(125, 22);
            this.collapseLog.Text = "of log";
            this.collapseLog.Click += new System.EventHandler(this.CollapseLog_Click);
            // 
            // collapseBranch
            // 
            this.collapseBranch.Name = "collapseBranch";
            this.collapseBranch.Size = new System.Drawing.Size(125, 22);
            this.collapseBranch.Text = "of branch";
            this.collapseBranch.Click += new System.EventHandler(this.CollapseBranch_Click);
            // 
            // resetOverlayIcon
            // 
            this.resetOverlayIcon.Name = "resetOverlayIcon";
            this.resetOverlayIcon.Size = new System.Drawing.Size(199, 22);
            this.resetOverlayIcon.Text = "Reset Overlay Error Icon";
            this.resetOverlayIcon.ToolTipText = "Removes the applied error or warning icon from the taskbar";
            this.resetOverlayIcon.Click += new System.EventHandler(this.ResetOverlayIcon_Click);
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            this.saveDialog.FilterIndex = 0;
            this.saveDialog.Title = "Select output file";
            // 
            // LogTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.treeView);
            this.Name = "LogTree";
            this.Size = new System.Drawing.Size(596, 478);
            this.Load += new System.EventHandler(this.LogTree_Load);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem editTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editIndividualNode;
        private System.Windows.Forms.ToolStripMenuItem editBranch;
        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveBranch;
        private System.Windows.Forms.ToolStripMenuItem saveLog;
        private System.Windows.Forms.ToolStripMenuItem editLog;
        private System.Windows.Forms.ToolStripMenuItem expandAllSubitemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandLog;
        private System.Windows.Forms.ToolStripMenuItem expandBranch;
        private System.Windows.Forms.ToolStripMenuItem collapseAllSubitemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseLog;
        private System.Windows.Forms.ToolStripMenuItem collapseBranch;
        private System.Windows.Forms.ToolStripMenuItem resetOverlayIcon;
    }
}
