namespace MeGUI.core.gui
{
    partial class VideoPlayerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer generated code

        /// <summary> 
        /// Required for windows forms designer
        /// Do not change manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.videoPreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.videoPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // videoPreview
            // 
            this.videoPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoPreview.Location = new System.Drawing.Point(0, 0);
            this.videoPreview.Name = "videoPreview";
            this.videoPreview.Size = new System.Drawing.Size(150, 150);
            this.videoPreview.TabIndex = 0;
            this.videoPreview.TabStop = false;
            // 
            // VideoPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.videoPreview);
            this.Name = "VideoPlayerControl";
            this.Load += new System.EventHandler(this.VideoPlayerControl_Load);
            this.Resize += new System.EventHandler(this.VideoPlayerControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.videoPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox videoPreview;
    }
}
