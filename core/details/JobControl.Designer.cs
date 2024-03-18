namespace MeGUI.core.details
{
    partial class JobControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAfterEncoding = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbAfterEncoding = new System.Windows.Forms.ComboBox();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.globalJobQueue = new MeGUI.core.gui.JobQueue();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAfterEncoding
            // 
            this.lblAfterEncoding.AutoSize = true;
            this.lblAfterEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAfterEncoding.Location = new System.Drawing.Point(3, 0);
            this.lblAfterEncoding.Name = "lblAfterEncoding";
            this.lblAfterEncoding.Size = new System.Drawing.Size(79, 29);
            this.lblAfterEncoding.TabIndex = 1;
            this.lblAfterEncoding.Text = "After encoding:";
            this.lblAfterEncoding.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblAfterEncoding, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbAfterEncoding, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.helpButton1, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 521);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(553, 29);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // cbAfterEncoding
            // 
            this.cbAfterEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAfterEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAfterEncoding.FormattingEnabled = true;
            this.cbAfterEncoding.Items.AddRange(new object[] {
            "Do Nothing",
            "Shutdown",
            "Run Command",
            "Close MeGUI"});
            this.cbAfterEncoding.Location = new System.Drawing.Point(88, 4);
            this.cbAfterEncoding.Name = "cbAfterEncoding";
            this.cbAfterEncoding.Size = new System.Drawing.Size(417, 21);
            this.cbAfterEncoding.TabIndex = 5;
            this.cbAfterEncoding.SelectedIndexChanged += new System.EventHandler(this.CbAfterEncoding_SelectedIndexChanged);
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.helpButton1.ArticleName = "Main Window#Queue";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(511, 3);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(39, 23);
            this.helpButton1.TabIndex = 3;
            // 
            // jobQueue
            // 
            this.globalJobQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalJobQueue.Location = new System.Drawing.Point(0, 0);
            this.globalJobQueue.Name = "jobQueue";
            this.globalJobQueue.Size = new System.Drawing.Size(553, 521);
            this.globalJobQueue.StartStopMode = MeGUI.core.gui.StartStopMode.Start;
            this.globalJobQueue.TabIndex = 0;
            this.globalJobQueue.AbortClicked += new System.EventHandler(this.JobQueue_AbortClicked);
            this.globalJobQueue.StartClicked += new System.EventHandler(this.JobQueue_StartClicked);
            this.globalJobQueue.StopClicked += new System.EventHandler(this.JobQueue_StopClicked);
            // 
            // JobControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.globalJobQueue);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "JobControl";
            this.Size = new System.Drawing.Size(553, 550);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAfterEncoding;
        private MeGUI.core.gui.HelpButton helpButton1;
        private MeGUI.core.gui.JobQueue globalJobQueue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cbAfterEncoding;
    }
}
