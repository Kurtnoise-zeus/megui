namespace MeGUI
{
    partial class AutoEncodeDefaults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoEncodeDefaults));
            this.AutomaticEncodingGroup = new System.Windows.Forms.GroupBox();
            this.fileSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.noTargetRadio = new System.Windows.Forms.RadioButton();
            this.averageBitrateRadio = new System.Windows.Forms.RadioButton();
            this.FileSizeRadio = new System.Windows.Forms.RadioButton();
            this.projectedBitrateKBits = new System.Windows.Forms.TextBox();
            this.AverageBitrateLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.OutputGroupBox = new System.Windows.Forms.GroupBox();
            this.DeviceLabel = new System.Windows.Forms.Label();
            this.device = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.container = new System.Windows.Forms.ComboBox();
            this.containerLabel = new System.Windows.Forms.Label();
            this.addSubsNChapters = new System.Windows.Forms.CheckBox();
            this.AutomaticEncodingGroup.SuspendLayout();
            this.OutputGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutomaticEncodingGroup
            // 
            this.AutomaticEncodingGroup.Controls.Add(this.fileSize);
            this.AutomaticEncodingGroup.Controls.Add(this.noTargetRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.averageBitrateRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.FileSizeRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.projectedBitrateKBits);
            this.AutomaticEncodingGroup.Controls.Add(this.AverageBitrateLabel);
            this.AutomaticEncodingGroup.Location = new System.Drawing.Point(3, 103);
            this.AutomaticEncodingGroup.Name = "AutomaticEncodingGroup";
            this.AutomaticEncodingGroup.Size = new System.Drawing.Size(424, 104);
            this.AutomaticEncodingGroup.TabIndex = 18;
            this.AutomaticEncodingGroup.TabStop = false;
            this.AutomaticEncodingGroup.Text = "Size and Bitrate";
            // 
            // fileSize
            // 
            this.fileSize.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.fileSize.Location = new System.Drawing.Point(101, 12);
            this.fileSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.fileSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.fileSize.Name = "fileSize";
            this.fileSize.NullString = null;
            this.fileSize.SaveCustomValues = false;
            this.fileSize.SelectedIndex = 0;
            this.fileSize.Size = new System.Drawing.Size(171, 29);
            this.fileSize.TabIndex = 23;
            // 
            // noTargetRadio
            // 
            this.noTargetRadio.Location = new System.Drawing.Point(16, 72);
            this.noTargetRadio.Name = "noTargetRadio";
            this.noTargetRadio.Size = new System.Drawing.Size(208, 18);
            this.noTargetRadio.TabIndex = 22;
            this.noTargetRadio.TabStop = true;
            this.noTargetRadio.Text = "No Target Size (use profile settings)";
            this.noTargetRadio.UseVisualStyleBackColor = true;
            this.noTargetRadio.CheckedChanged += new System.EventHandler(this.FileSizeRadio_CheckedChanged);
            // 
            // averageBitrateRadio
            // 
            this.averageBitrateRadio.Location = new System.Drawing.Point(16, 47);
            this.averageBitrateRadio.Name = "averageBitrateRadio";
            this.averageBitrateRadio.Size = new System.Drawing.Size(79, 18);
            this.averageBitrateRadio.TabIndex = 16;
            this.averageBitrateRadio.Text = "Average Bitrate";
            this.averageBitrateRadio.CheckedChanged += new System.EventHandler(this.FileSizeRadio_CheckedChanged);
            // 
            // FileSizeRadio
            // 
            this.FileSizeRadio.Checked = true;
            this.FileSizeRadio.Location = new System.Drawing.Point(16, 20);
            this.FileSizeRadio.Name = "FileSizeRadio";
            this.FileSizeRadio.Size = new System.Drawing.Size(100, 18);
            this.FileSizeRadio.TabIndex = 15;
            this.FileSizeRadio.TabStop = true;
            this.FileSizeRadio.Text = "File Size";
            this.FileSizeRadio.CheckedChanged += new System.EventHandler(this.FileSizeRadio_CheckedChanged);
            // 
            // projectedBitrateKBits
            // 
            this.projectedBitrateKBits.Enabled = false;
            this.projectedBitrateKBits.Location = new System.Drawing.Point(101, 44);
            this.projectedBitrateKBits.Name = "projectedBitrateKBits";
            this.projectedBitrateKBits.Size = new System.Drawing.Size(64, 21);
            this.projectedBitrateKBits.TabIndex = 9;
            this.projectedBitrateKBits.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // AverageBitrateLabel
            // 
            this.AverageBitrateLabel.Location = new System.Drawing.Point(171, 47);
            this.AverageBitrateLabel.Name = "AverageBitrateLabel";
            this.AverageBitrateLabel.Size = new System.Drawing.Size(37, 23);
            this.AverageBitrateLabel.TabIndex = 10;
            this.AverageBitrateLabel.Text = "kbit/s";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(379, 215);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 20;
            this.cancelButton.Text = "Cancel";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(311, 215);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(48, 23);
            this.saveButton.TabIndex = 19;
            this.saveButton.Text = "Save";
            // 
            // OutputGroupBox
            // 
            this.OutputGroupBox.Controls.Add(this.DeviceLabel);
            this.OutputGroupBox.Controls.Add(this.device);
            this.OutputGroupBox.Controls.Add(this.label1);
            this.OutputGroupBox.Controls.Add(this.splitSize);
            this.OutputGroupBox.Controls.Add(this.container);
            this.OutputGroupBox.Controls.Add(this.containerLabel);
            this.OutputGroupBox.Location = new System.Drawing.Point(3, 12);
            this.OutputGroupBox.Name = "OutputGroupBox";
            this.OutputGroupBox.Size = new System.Drawing.Size(424, 85);
            this.OutputGroupBox.TabIndex = 21;
            this.OutputGroupBox.TabStop = false;
            this.OutputGroupBox.Text = "Output Options";
            // 
            // DeviceLabel
            // 
            this.DeviceLabel.AutoSize = true;
            this.DeviceLabel.Location = new System.Drawing.Point(16, 54);
            this.DeviceLabel.Name = "DeviceLabel";
            this.DeviceLabel.Size = new System.Drawing.Size(39, 13);
            this.DeviceLabel.TabIndex = 40;
            this.DeviceLabel.Text = "Device";
            // 
            // device
            // 
            this.device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device.FormattingEnabled = true;
            this.device.Items.AddRange(new object[] {
            "Standard"});
            this.device.Location = new System.Drawing.Point(101, 51);
            this.device.Name = "device";
            this.device.Size = new System.Drawing.Size(85, 21);
            this.device.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Splitting:";
            // 
            // splitSize
            // 
            this.splitSize.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.splitSize.Location = new System.Drawing.Point(270, 20);
            this.splitSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.splitSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.splitSize.Name = "splitSize";
            this.splitSize.NullString = "No splitting";
            this.splitSize.SaveCustomValues = false;
            this.splitSize.SelectedIndex = 0;
            this.splitSize.Size = new System.Drawing.Size(148, 29);
            this.splitSize.TabIndex = 26;
            // 
            // container
            // 
            this.container.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.container.FormattingEnabled = true;
            this.container.Location = new System.Drawing.Point(101, 24);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(85, 21);
            this.container.TabIndex = 25;
            this.container.SelectedIndexChanged += new System.EventHandler(this.container_SelectedIndexChanged);
            // 
            // containerLabel
            // 
            this.containerLabel.AutoSize = true;
            this.containerLabel.Location = new System.Drawing.Point(16, 28);
            this.containerLabel.Name = "containerLabel";
            this.containerLabel.Size = new System.Drawing.Size(54, 13);
            this.containerLabel.TabIndex = 24;
            this.containerLabel.Text = "Container";
            // 
            // addSubsNChapters
            // 
            this.addSubsNChapters.Location = new System.Drawing.Point(19, 215);
            this.addSubsNChapters.Name = "addSubsNChapters";
            this.addSubsNChapters.Size = new System.Drawing.Size(256, 24);
            this.addSubsNChapters.TabIndex = 22;
            this.addSubsNChapters.Text = "Add additional content (audio, subs, chapters)";
            // 
            // AutoEncodeDefaults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(433, 241);
            this.Controls.Add(this.addSubsNChapters);
            this.Controls.Add(this.OutputGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.AutomaticEncodingGroup);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoEncodeDefaults";
            this.ShowInTaskbar = false;
            this.Text = "MeGUI - Auto Encode Defaults";
            this.AutomaticEncodingGroup.ResumeLayout(false);
            this.AutomaticEncodingGroup.PerformLayout();
            this.OutputGroupBox.ResumeLayout(false);
            this.OutputGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox AutomaticEncodingGroup;
        private System.Windows.Forms.RadioButton noTargetRadio;
        private System.Windows.Forms.RadioButton averageBitrateRadio;
        private System.Windows.Forms.RadioButton FileSizeRadio;
        private System.Windows.Forms.TextBox projectedBitrateKBits;
        private System.Windows.Forms.Label AverageBitrateLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox OutputGroupBox;
        private System.Windows.Forms.ComboBox container;
        private System.Windows.Forms.Label containerLabel;
        private System.Windows.Forms.CheckBox addSubsNChapters;
        private MeGUI.core.gui.TargetSizeSCBox fileSize;
        private MeGUI.core.gui.TargetSizeSCBox splitSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox device;
        private System.Windows.Forms.Label DeviceLabel;
    }
}