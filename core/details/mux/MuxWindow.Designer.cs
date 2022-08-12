namespace MeGUI
{
    partial class MuxWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MuxWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.muxedInput = new MeGUI.FileBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Muxed Input";
            // 
            // muxedInput
            // 
            this.muxedInput.Filename = "";
            this.muxedInput.Filter = null;
            this.muxedInput.FilterIndex = 0;
            this.muxedInput.FolderMode = false;
            this.muxedInput.Location = new System.Drawing.Point(118, 45);
            this.muxedInput.Name = "muxedInput";
            this.muxedInput.ReadOnly = true;
            this.muxedInput.SaveMode = false;
            this.muxedInput.Size = new System.Drawing.Size(289, 23);
            this.muxedInput.TabIndex = 3;
            this.muxedInput.Title = null;
            this.muxedInput.FileSelected += new MeGUI.FileBarEventHandler(this.muxedInput_FileSelected);
            // 
            // MuxWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(444, 562);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MuxWindow";
            this.Text = "MeGUI - Muxer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FileBar muxedInput;
    }
}