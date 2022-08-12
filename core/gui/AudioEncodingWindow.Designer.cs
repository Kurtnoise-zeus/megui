namespace MeGUI.core.gui
{
    partial class AudioEncodingWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioEncodingWindow));
            this.audioEncodingTab1 = new MeGUI.core.gui.AudioEncodingTab();
            this.SuspendLayout();
            // 
            // audioEncodingTab1
            // 
            this.audioEncodingTab1.AudioContainer = "";
            this.audioEncodingTab1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.audioEncodingTab1.Location = new System.Drawing.Point(0, 2);
            this.audioEncodingTab1.Name = "audioEncodingTab1";
            this.audioEncodingTab1.QueueButtonText = "Update";
            this.audioEncodingTab1.ShowReset = false;
            this.audioEncodingTab1.Size = new System.Drawing.Size(484, 173);
            this.audioEncodingTab1.TabIndex = 0;
            // 
            // AudioEncodingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(484, 175);
            this.Controls.Add(this.audioEncodingTab1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1000, 214);
            this.MinimumSize = new System.Drawing.Size(450, 214);
            this.Name = "AudioEncodingWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audio Encoding Window";
            this.ResumeLayout(false);

        }

        #endregion

        private AudioEncodingTab audioEncodingTab1;
    }
}