namespace MeGUI.core.gui
{
    partial class ProfileImporter
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileImporter));
            this.statusCheck = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PresetImporterToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusCheck.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.Text = "Select the presets to import";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(266, 323);
            this.button2.Size = new System.Drawing.Size(49, 23);
            this.button2.Text = "Import";
            this.button2.Click += new System.EventHandler(this.import_Click);
            // 
            // profileList
            // 
            this.profileList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.profileList.ContextMenuStrip = this.statusCheck;
            // 
            // statusCheck
            // 
            this.statusCheck.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAllToolStripMenuItem,
            this.checkNoneToolStripMenuItem});
            this.statusCheck.Name = "statusCheck";
            this.statusCheck.Size = new System.Drawing.Size(140, 48);
            // 
            // checkAllToolStripMenuItem
            // 
            this.checkAllToolStripMenuItem.Name = "checkAllToolStripMenuItem";
            this.checkAllToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.checkAllToolStripMenuItem.Text = "Check All";
            this.checkAllToolStripMenuItem.Click += new System.EventHandler(this.checkAllToolStripMenuItem_Click);
            // 
            // checkNoneToolStripMenuItem
            // 
            this.checkNoneToolStripMenuItem.Name = "checkNoneToolStripMenuItem";
            this.checkNoneToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.checkNoneToolStripMenuItem.Text = "Check None";
            this.checkNoneToolStripMenuItem.Click += new System.EventHandler(this.checkNoneToolStripMenuItem_Click);
            // 
            // PresetImporterToolTip
            // 
            this.PresetImporterToolTip.AutoPopDelay = 5000;
            this.PresetImporterToolTip.InitialDelay = 1000;
            this.PresetImporterToolTip.ReshowDelay = 100;
            // 
            // ProfileImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(383, 358);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProfileImporter";
            this.Text = "Preset Importer";
            this.Shown += new System.EventHandler(this.ProfileImporter_Shown);
            this.Controls.SetChildIndex(this.profileList, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.button2, 0);
            this.statusCheck.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip statusCheck;
        private System.Windows.Forms.ToolStripMenuItem checkAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkNoneToolStripMenuItem;
        private System.Windows.Forms.ToolTip PresetImporterToolTip;

    }
}
