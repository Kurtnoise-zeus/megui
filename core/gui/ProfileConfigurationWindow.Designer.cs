using System.Windows.Forms;

namespace MeGUI.core.gui
{
    partial class ProfileConfigurationWindow<TSettings, TPanel>
    {
        protected GroupBox presetsGroupbox;
        private Panel panel1;
        protected Button cancelButton;
        protected Button okButton;
        private Button updateButton;
        private Button loadDefaultsButton;
        private ComboBox videoProfile;
        private Button newVideoProfileButton;
        private Button deleteVideoProfileButton;


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
            this.presetsGroupbox = new System.Windows.Forms.GroupBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.loadDefaultsButton = new System.Windows.Forms.Button();
            this.videoProfile = new System.Windows.Forms.ComboBox();
            this.newVideoProfileButton = new System.Windows.Forms.Button();
            this.deleteVideoProfileButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.presetsGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // presetsGroupbox
            // 
            this.presetsGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.presetsGroupbox.Controls.Add(this.updateButton);
            this.presetsGroupbox.Controls.Add(this.loadDefaultsButton);
            this.presetsGroupbox.Controls.Add(this.videoProfile);
            this.presetsGroupbox.Controls.Add(this.newVideoProfileButton);
            this.presetsGroupbox.Controls.Add(this.deleteVideoProfileButton);
            this.presetsGroupbox.Location = new System.Drawing.Point(6, 405);
            this.presetsGroupbox.Name = "presetsGroupbox";
            this.presetsGroupbox.Size = new System.Drawing.Size(400, 48);
            this.presetsGroupbox.TabIndex = 44;
            this.presetsGroupbox.TabStop = false;
            this.presetsGroupbox.Text = " Presets ";
            // 
            // updateButton
            // 
            this.updateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateButton.Location = new System.Drawing.Point(235, 18);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(50, 23);
            this.updateButton.TabIndex = 15;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // loadDefaultsButton
            // 
            this.loadDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadDefaultsButton.Location = new System.Drawing.Point(291, 18);
            this.loadDefaultsButton.Name = "loadDefaultsButton";
            this.loadDefaultsButton.Size = new System.Drawing.Size(103, 23);
            this.loadDefaultsButton.TabIndex = 14;
            this.loadDefaultsButton.Text = "Load Defaults";
            this.loadDefaultsButton.UseVisualStyleBackColor = true;
            this.loadDefaultsButton.Click += new System.EventHandler(this.loadDefaultsButton_Click);
            // 
            // videoProfile
            // 
            this.videoProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoProfile.Location = new System.Drawing.Point(8, 19);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.Size = new System.Drawing.Size(121, 21);
            this.videoProfile.Sorted = true;
            this.videoProfile.TabIndex = 11;
            this.videoProfile.SelectedIndexChanged += new System.EventHandler(this.videoProfile_SelectedIndexChanged);
            // 
            // newVideoProfileButton
            // 
            this.newVideoProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newVideoProfileButton.Location = new System.Drawing.Point(189, 18);
            this.newVideoProfileButton.Name = "newVideoProfileButton";
            this.newVideoProfileButton.Size = new System.Drawing.Size(40, 23);
            this.newVideoProfileButton.TabIndex = 12;
            this.newVideoProfileButton.Text = "New";
            this.newVideoProfileButton.Click += new System.EventHandler(this.newVideoProfileButton_Click);
            // 
            // deleteVideoProfileButton
            // 
            this.deleteVideoProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteVideoProfileButton.Location = new System.Drawing.Point(135, 18);
            this.deleteVideoProfileButton.Name = "deleteVideoProfileButton";
            this.deleteVideoProfileButton.Size = new System.Drawing.Size(48, 23);
            this.deleteVideoProfileButton.TabIndex = 13;
            this.deleteVideoProfileButton.Text = "Delete";
            this.deleteVideoProfileButton.Click += new System.EventHandler(this.deleteVideoProfileButton_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(411, 399);
            this.panel1.TabIndex = 45;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(358, 459);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 47;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(312, 459);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(40, 23);
            this.okButton.TabIndex = 46;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // ProfileConfigurationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(414, 490);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.presetsGroupbox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileConfigurationWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileConfigurationWindow_FormClosing);
            this.presetsGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
