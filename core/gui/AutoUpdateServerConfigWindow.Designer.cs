namespace MeGUI.core.gui
{
    partial class AutoUpdateServerConfigWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoUpdateServerConfigWindow));
            this.serverList = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.addServerButton = new System.Windows.Forms.Button();
            this.removeSelectedServersButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.subList = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverList
            // 
            this.serverList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverList.FormattingEnabled = true;
            this.serverList.Location = new System.Drawing.Point(0, 28);
            this.serverList.Name = "serverList";
            this.serverList.Size = new System.Drawing.Size(379, 293);
            this.serverList.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 321);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(379, 0);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // addServerButton
            // 
            this.addServerButton.AutoSize = true;
            this.addServerButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addServerButton.Location = new System.Drawing.Point(87, 3);
            this.addServerButton.Name = "addServerButton";
            this.addServerButton.Size = new System.Drawing.Size(48, 23);
            this.addServerButton.TabIndex = 0;
            this.addServerButton.Text = "Add...";
            this.addServerButton.UseVisualStyleBackColor = true;
            this.addServerButton.Click += new System.EventHandler(this.addServerButton_Click);
            // 
            // removeSelectedServersButton
            // 
            this.removeSelectedServersButton.AutoSize = true;
            this.removeSelectedServersButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.removeSelectedServersButton.Location = new System.Drawing.Point(141, 3);
            this.removeSelectedServersButton.Name = "removeSelectedServersButton";
            this.removeSelectedServersButton.Size = new System.Drawing.Size(99, 23);
            this.removeSelectedServersButton.TabIndex = 1;
            this.removeSelectedServersButton.Text = "Remove selected";
            this.removeSelectedServersButton.UseVisualStyleBackColor = true;
            this.removeSelectedServersButton.Click += new System.EventHandler(this.removeSelectedServersButton_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.cancelButton);
            this.flowLayoutPanel2.Controls.Add(this.okButton);
            this.flowLayoutPanel2.Controls.Add(this.helpButton1);
            this.flowLayoutPanel2.Controls.Add(this.removeSelectedServersButton);
            this.flowLayoutPanel2.Controls.Add(this.addServerButton);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 321);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(379, 29);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // cancelButton
            // 
            this.cancelButton.AutoSize = true;
            this.cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(327, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(49, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.AutoSize = true;
            this.okButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(290, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(31, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Presentation/AutoUpdate";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(246, 3);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 2;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.label1);
            this.flowLayoutPanel3.Controls.Add(this.subList);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(379, 28);
            this.flowLayoutPanel3.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(114, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "update server group";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // subList
            // 
            this.subList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.subList.FormattingEnabled = true;
            this.subList.Location = new System.Drawing.Point(123, 3);
            this.subList.Name = "subList";
            this.subList.Size = new System.Drawing.Size(222, 21);
            this.subList.TabIndex = 3;
            this.subList.SelectedIndexChanged += new System.EventHandler(this.subList_SelectedIndexChanged);
            // 
            // AutoUpdateServerConfigWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(379, 350);
            this.Controls.Add(this.serverList);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutoUpdateServerConfigWindow";
            this.Text = "Update Server Configuration";
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox serverList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button addServerButton;
        private System.Windows.Forms.Button removeSelectedServersButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox subList;
        private HelpButton helpButton1;
    }
}
