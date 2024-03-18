namespace MeGUI.core.gui
{
    partial class WorkerSummary
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkerSummary));
            this.panel1 = new System.Windows.Forms.Panel();
            this.individualWorkerSummary3 = new MeGUI.core.gui.IndividualWorkerSummary();
            this.individualWorkerSummary2 = new MeGUI.core.gui.IndividualWorkerSummary();
            this.individualWorkerSummary1 = new MeGUI.core.gui.IndividualWorkerSummary();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.individualWorkerSummary3);
            this.panel1.Controls.Add(this.individualWorkerSummary2);
            this.panel1.Controls.Add(this.individualWorkerSummary1);
            this.panel1.Location = new System.Drawing.Point(0, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(348, 147);
            this.panel1.TabIndex = 0;
            // 
            // individualWorkerSummary3
            // 
            this.individualWorkerSummary3.AutoSize = true;
            this.individualWorkerSummary3.Dock = System.Windows.Forms.DockStyle.Top;
            this.individualWorkerSummary3.Location = new System.Drawing.Point(3, 97);
            this.individualWorkerSummary3.MaximumSize = new System.Drawing.Size(1000, 47);
            this.individualWorkerSummary3.MinimumSize = new System.Drawing.Size(200, 47);
            this.individualWorkerSummary3.Name = "individualWorkerSummary3";
            this.individualWorkerSummary3.Size = new System.Drawing.Size(342, 47);
            this.individualWorkerSummary3.TabIndex = 2;
            // 
            // individualWorkerSummary2
            // 
            this.individualWorkerSummary2.AutoSize = true;
            this.individualWorkerSummary2.Dock = System.Windows.Forms.DockStyle.Top;
            this.individualWorkerSummary2.Location = new System.Drawing.Point(3, 50);
            this.individualWorkerSummary2.MaximumSize = new System.Drawing.Size(1000, 47);
            this.individualWorkerSummary2.MinimumSize = new System.Drawing.Size(200, 47);
            this.individualWorkerSummary2.Name = "individualWorkerSummary2";
            this.individualWorkerSummary2.Size = new System.Drawing.Size(342, 47);
            this.individualWorkerSummary2.TabIndex = 1;
            // 
            // individualWorkerSummary1
            // 
            this.individualWorkerSummary1.AutoSize = true;
            this.individualWorkerSummary1.Dock = System.Windows.Forms.DockStyle.Top;
            this.individualWorkerSummary1.Location = new System.Drawing.Point(3, 3);
            this.individualWorkerSummary1.MaximumSize = new System.Drawing.Size(1000, 47);
            this.individualWorkerSummary1.MinimumSize = new System.Drawing.Size(200, 47);
            this.individualWorkerSummary1.Name = "individualWorkerSummary1";
            this.individualWorkerSummary1.Size = new System.Drawing.Size(342, 47);
            this.individualWorkerSummary1.TabIndex = 0;
            // 
            // WorkerSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(352, 151);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(368, 190);
            this.Name = "WorkerSummary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Worker Overview";
            this.VisibleChanged += new System.EventHandler(this.WorkerSummary_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private IndividualWorkerSummary individualWorkerSummary3;
        private IndividualWorkerSummary individualWorkerSummary2;
        private IndividualWorkerSummary individualWorkerSummary1;


    }
}