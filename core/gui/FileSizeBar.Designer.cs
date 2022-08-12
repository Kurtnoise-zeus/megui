namespace MeGUI.core.gui
{
    partial class FileSizeBar
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
            this.units = new System.Windows.Forms.ComboBox();
            this.number = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.number)).BeginInit();
            this.SuspendLayout();
            // 
            // units
            // 
            this.units.Dock = System.Windows.Forms.DockStyle.Right;
            this.units.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.units.Items.AddRange(new object[] {
            "B",
            "KB",
            "MB",
            "GB"});
            this.units.Location = new System.Drawing.Point(143, 0);
            this.units.Name = "units";
            this.units.Size = new System.Drawing.Size(42, 21);
            this.units.TabIndex = 0;
            this.units.SelectedIndexChanged += new System.EventHandler(this.units_SelectedIndexChanged);
            // 
            // number
            // 
            this.number.Dock = System.Windows.Forms.DockStyle.Fill;
            this.number.Location = new System.Drawing.Point(0, 0);
            this.number.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.number.Name = "number";
            this.number.Size = new System.Drawing.Size(143, 20);
            this.number.TabIndex = 1;
            this.number.ValueChanged += new System.EventHandler(this.number_ValueChanged);
            // 
            // FileSizeBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.number);
            this.Controls.Add(this.units);
            this.Name = "FileSizeBar";
            this.Size = new System.Drawing.Size(185, 24);
            this.EnabledChanged += new System.EventHandler(this.FileSizeBar_EnabledChanged);
            ((System.ComponentModel.ISupportInitialize)(this.number)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox units;
        private System.Windows.Forms.NumericUpDown number;
    }
}
