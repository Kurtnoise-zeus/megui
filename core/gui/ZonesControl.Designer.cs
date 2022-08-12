namespace MeGUI
{
    partial class ZonesControl
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
            this.zonesGroupbox = new System.Windows.Forms.GroupBox();
            this.zoneListView = new System.Windows.Forms.ListView();
            this.startFrameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.endFrameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.modeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.modifierColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.startFrame = new System.Windows.Forms.TextBox();
            this.endFrame = new System.Windows.Forms.TextBox();
            this.endFrameLabel = new System.Windows.Forms.Label();
            this.zoneLabel = new System.Windows.Forms.Label();
            this.zoneModifier = new System.Windows.Forms.NumericUpDown();
            this.zoneMode = new System.Windows.Forms.ComboBox();
            this.startFrameLabel = new System.Windows.Forms.Label();
            this.modifierLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.showVideoButton = new System.Windows.Forms.Button();
            this.removeZoneButton = new System.Windows.Forms.Button();
            this.updateZoneButton = new System.Windows.Forms.Button();
            this.clearZonesButton = new System.Windows.Forms.Button();
            this.addZoneButton = new System.Windows.Forms.Button();
            this.zonesGroupbox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoneModifier)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // zonesGroupbox
            // 
            this.zonesGroupbox.Controls.Add(this.zoneListView);
            this.zonesGroupbox.Controls.Add(this.tableLayoutPanel1);
            this.zonesGroupbox.Controls.Add(this.flowLayoutPanel1);
            this.zonesGroupbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zonesGroupbox.Location = new System.Drawing.Point(0, 0);
            this.zonesGroupbox.Name = "zonesGroupbox";
            this.zonesGroupbox.Size = new System.Drawing.Size(295, 412);
            this.zonesGroupbox.TabIndex = 2;
            this.zonesGroupbox.TabStop = false;
            this.zonesGroupbox.Text = "Zones";
            // 
            // zoneListView
            // 
            this.zoneListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.startFrameColumn,
            this.endFrameColumn,
            this.modeColumn,
            this.modifierColumn});
            this.zoneListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoneListView.FullRowSelect = true;
            this.zoneListView.HideSelection = false;
            this.zoneListView.Location = new System.Drawing.Point(3, 16);
            this.zoneListView.Name = "zoneListView";
            this.zoneListView.Size = new System.Drawing.Size(289, 311);
            this.zoneListView.TabIndex = 0;
            this.zoneListView.UseCompatibleStateImageBehavior = false;
            this.zoneListView.View = System.Windows.Forms.View.Details;
            // 
            // startFrameColumn
            // 
            this.startFrameColumn.Text = "Start";
            // 
            // endFrameColumn
            // 
            this.endFrameColumn.Text = "End";
            // 
            // modeColumn
            // 
            this.modeColumn.Text = "Mode";
            this.modeColumn.Width = 80;
            // 
            // modifierColumn
            // 
            this.modifierColumn.Text = "Modifier";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.startFrame, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.endFrame, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.endFrameLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.zoneLabel, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.zoneModifier, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.zoneMode, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.startFrameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.modifierLabel, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 327);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(289, 53);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // startFrame
            // 
            this.startFrame.Location = new System.Drawing.Point(73, 3);
            this.startFrame.Name = "startFrame";
            this.startFrame.Size = new System.Drawing.Size(48, 20);
            this.startFrame.TabIndex = 1;
            // 
            // endFrame
            // 
            this.endFrame.Location = new System.Drawing.Point(73, 29);
            this.endFrame.Name = "endFrame";
            this.endFrame.Size = new System.Drawing.Size(48, 20);
            this.endFrame.TabIndex = 2;
            // 
            // endFrameLabel
            // 
            this.endFrameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endFrameLabel.Location = new System.Drawing.Point(3, 26);
            this.endFrameLabel.Name = "endFrameLabel";
            this.endFrameLabel.Size = new System.Drawing.Size(64, 27);
            this.endFrameLabel.TabIndex = 2;
            this.endFrameLabel.Text = "End Frame";
            this.endFrameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zoneLabel
            // 
            this.zoneLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoneLabel.Location = new System.Drawing.Point(127, 26);
            this.zoneLabel.Name = "zoneLabel";
            this.zoneLabel.Size = new System.Drawing.Size(56, 27);
            this.zoneLabel.TabIndex = 5;
            this.zoneLabel.Text = "Mode";
            this.zoneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zoneModifier
            // 
            this.zoneModifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoneModifier.Location = new System.Drawing.Point(189, 3);
            this.zoneModifier.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.zoneModifier.Name = "zoneModifier";
            this.zoneModifier.Size = new System.Drawing.Size(97, 20);
            this.zoneModifier.TabIndex = 3;
            this.zoneModifier.Value = new decimal(new int[] {
            26,
            0,
            0,
            0});
            // 
            // zoneMode
            // 
            this.zoneMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoneMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoneMode.Items.AddRange(new object[] {
            "Quantizer",
            "Weight"});
            this.zoneMode.Location = new System.Drawing.Point(189, 29);
            this.zoneMode.Name = "zoneMode";
            this.zoneMode.Size = new System.Drawing.Size(97, 21);
            this.zoneMode.TabIndex = 4;
            this.zoneMode.SelectedIndexChanged += new System.EventHandler(this.zoneMode_SelectedIndexChanged);
            // 
            // startFrameLabel
            // 
            this.startFrameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startFrameLabel.Location = new System.Drawing.Point(3, 0);
            this.startFrameLabel.Name = "startFrameLabel";
            this.startFrameLabel.Size = new System.Drawing.Size(64, 26);
            this.startFrameLabel.TabIndex = 1;
            this.startFrameLabel.Text = "Start Frame";
            this.startFrameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // modifierLabel
            // 
            this.modifierLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modifierLabel.Location = new System.Drawing.Point(127, 0);
            this.modifierLabel.Name = "modifierLabel";
            this.modifierLabel.Size = new System.Drawing.Size(56, 26);
            this.modifierLabel.TabIndex = 6;
            this.modifierLabel.Text = "Quantizer";
            this.modifierLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.showVideoButton);
            this.flowLayoutPanel1.Controls.Add(this.removeZoneButton);
            this.flowLayoutPanel1.Controls.Add(this.updateZoneButton);
            this.flowLayoutPanel1.Controls.Add(this.clearZonesButton);
            this.flowLayoutPanel1.Controls.Add(this.addZoneButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 380);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(289, 29);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // showVideoButton
            // 
            this.showVideoButton.AutoSize = true;
            this.showVideoButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.showVideoButton.Enabled = false;
            this.showVideoButton.Location = new System.Drawing.Point(3, 3);
            this.showVideoButton.Name = "showVideoButton";
            this.showVideoButton.Size = new System.Drawing.Size(55, 23);
            this.showVideoButton.TabIndex = 9;
            this.showVideoButton.Text = "Preview";
            this.showVideoButton.Click += new System.EventHandler(this.showVideoButton_Click);
            // 
            // removeZoneButton
            // 
            this.removeZoneButton.AutoSize = true;
            this.removeZoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.removeZoneButton.Location = new System.Drawing.Point(64, 3);
            this.removeZoneButton.Name = "removeZoneButton";
            this.removeZoneButton.Size = new System.Drawing.Size(57, 23);
            this.removeZoneButton.TabIndex = 6;
            this.removeZoneButton.Text = "Remove";
            this.removeZoneButton.Click += new System.EventHandler(this.removeZoneButton_Click);
            // 
            // updateZoneButton
            // 
            this.updateZoneButton.AutoSize = true;
            this.updateZoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.updateZoneButton.Location = new System.Drawing.Point(127, 3);
            this.updateZoneButton.Name = "updateZoneButton";
            this.updateZoneButton.Size = new System.Drawing.Size(52, 23);
            this.updateZoneButton.TabIndex = 9;
            this.updateZoneButton.Text = "Update";
            this.updateZoneButton.Click += new System.EventHandler(this.updateZoneButton_Click);
            // 
            // clearZonesButton
            // 
            this.clearZonesButton.AutoSize = true;
            this.clearZonesButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clearZonesButton.Location = new System.Drawing.Point(185, 3);
            this.clearZonesButton.Name = "clearZonesButton";
            this.clearZonesButton.Size = new System.Drawing.Size(41, 23);
            this.clearZonesButton.TabIndex = 5;
            this.clearZonesButton.Text = "Clear";
            this.clearZonesButton.Click += new System.EventHandler(this.clearZonesButton_Click);
            // 
            // addZoneButton
            // 
            this.addZoneButton.AutoSize = true;
            this.addZoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addZoneButton.Location = new System.Drawing.Point(232, 3);
            this.addZoneButton.Name = "addZoneButton";
            this.addZoneButton.Size = new System.Drawing.Size(36, 23);
            this.addZoneButton.TabIndex = 7;
            this.addZoneButton.Text = "Add";
            this.addZoneButton.Click += new System.EventHandler(this.addZoneButton_Click);
            // 
            // ZonesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.zonesGroupbox);
            this.Name = "ZonesControl";
            this.Size = new System.Drawing.Size(295, 412);
            this.Load += new System.EventHandler(this.ZonesControl_Load);
            this.zonesGroupbox.ResumeLayout(false);
            this.zonesGroupbox.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoneModifier)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox zonesGroupbox;
        private System.Windows.Forms.NumericUpDown zoneModifier;
        private System.Windows.Forms.Label modifierLabel;
        private System.Windows.Forms.Label zoneLabel;
        private System.Windows.Forms.ComboBox zoneMode;
        private System.Windows.Forms.TextBox endFrame;
        private System.Windows.Forms.TextBox startFrame;
        private System.Windows.Forms.ListView zoneListView;
        private System.Windows.Forms.ColumnHeader startFrameColumn;
        private System.Windows.Forms.ColumnHeader endFrameColumn;
        private System.Windows.Forms.ColumnHeader modeColumn;
        private System.Windows.Forms.ColumnHeader modifierColumn;
        private System.Windows.Forms.Label startFrameLabel;
        private System.Windows.Forms.Label endFrameLabel;
        private System.Windows.Forms.Button addZoneButton;
        private System.Windows.Forms.Button clearZonesButton;
        private System.Windows.Forms.Button updateZoneButton;
        private System.Windows.Forms.Button showVideoButton;
        private System.Windows.Forms.Button removeZoneButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
