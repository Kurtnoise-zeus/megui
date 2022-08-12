// ****************************************************************************
// 
// Copyright (C) 2005-2018 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System.Windows.Forms;

namespace MeGUI
{
    public partial class QuantizerMatrixDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuantizerMatrixDialog));
            this.predefinedMatrixLabel = new System.Windows.Forms.Label();
            this.predefinedMatrix = new System.Windows.Forms.ComboBox();
            this.mat1x1 = new System.Windows.Forms.TextBox();
            this.mat1x2 = new System.Windows.Forms.TextBox();
            this.mat1x3 = new System.Windows.Forms.TextBox();
            this.mat1x4 = new System.Windows.Forms.TextBox();
            this.mat2x4 = new System.Windows.Forms.TextBox();
            this.mat2x3 = new System.Windows.Forms.TextBox();
            this.mat2x2 = new System.Windows.Forms.TextBox();
            this.mat2x1 = new System.Windows.Forms.TextBox();
            this.mat4x4 = new System.Windows.Forms.TextBox();
            this.mat4x3 = new System.Windows.Forms.TextBox();
            this.mat4x2 = new System.Windows.Forms.TextBox();
            this.mat4x1 = new System.Windows.Forms.TextBox();
            this.mat3x4 = new System.Windows.Forms.TextBox();
            this.mat3x3 = new System.Windows.Forms.TextBox();
            this.mat3x2 = new System.Windows.Forms.TextBox();
            this.mat3x1 = new System.Windows.Forms.TextBox();
            this.mat8x4 = new System.Windows.Forms.TextBox();
            this.mat8x3 = new System.Windows.Forms.TextBox();
            this.mat8x2 = new System.Windows.Forms.TextBox();
            this.mat8x1 = new System.Windows.Forms.TextBox();
            this.mat7x4 = new System.Windows.Forms.TextBox();
            this.mat7x3 = new System.Windows.Forms.TextBox();
            this.mat7x2 = new System.Windows.Forms.TextBox();
            this.mat7x1 = new System.Windows.Forms.TextBox();
            this.mat6x4 = new System.Windows.Forms.TextBox();
            this.mat6x3 = new System.Windows.Forms.TextBox();
            this.mat6x2 = new System.Windows.Forms.TextBox();
            this.mat6x1 = new System.Windows.Forms.TextBox();
            this.mat5x4 = new System.Windows.Forms.TextBox();
            this.mat5x3 = new System.Windows.Forms.TextBox();
            this.mat5x2 = new System.Windows.Forms.TextBox();
            this.mat5x1 = new System.Windows.Forms.TextBox();
            this.matrixGroupbox = new System.Windows.Forms.GroupBox();
            this.mat8x8 = new System.Windows.Forms.TextBox();
            this.mat8x7 = new System.Windows.Forms.TextBox();
            this.mat8x6 = new System.Windows.Forms.TextBox();
            this.mat8x5 = new System.Windows.Forms.TextBox();
            this.mat7x8 = new System.Windows.Forms.TextBox();
            this.mat7x7 = new System.Windows.Forms.TextBox();
            this.mat7x6 = new System.Windows.Forms.TextBox();
            this.mat7x5 = new System.Windows.Forms.TextBox();
            this.mat6x8 = new System.Windows.Forms.TextBox();
            this.mat6x7 = new System.Windows.Forms.TextBox();
            this.mat6x6 = new System.Windows.Forms.TextBox();
            this.mat6x5 = new System.Windows.Forms.TextBox();
            this.mat5x8 = new System.Windows.Forms.TextBox();
            this.mat5x7 = new System.Windows.Forms.TextBox();
            this.mat5x6 = new System.Windows.Forms.TextBox();
            this.mat5x5 = new System.Windows.Forms.TextBox();
            this.mat4x8 = new System.Windows.Forms.TextBox();
            this.mat4x7 = new System.Windows.Forms.TextBox();
            this.mat4x6 = new System.Windows.Forms.TextBox();
            this.mat4x5 = new System.Windows.Forms.TextBox();
            this.mat3x8 = new System.Windows.Forms.TextBox();
            this.mat3x7 = new System.Windows.Forms.TextBox();
            this.mat3x6 = new System.Windows.Forms.TextBox();
            this.mat3x5 = new System.Windows.Forms.TextBox();
            this.mat2x8 = new System.Windows.Forms.TextBox();
            this.mat2x7 = new System.Windows.Forms.TextBox();
            this.mat2x6 = new System.Windows.Forms.TextBox();
            this.mat2x5 = new System.Windows.Forms.TextBox();
            this.mat1x8 = new System.Windows.Forms.TextBox();
            this.mat1x7 = new System.Windows.Forms.TextBox();
            this.mat1x6 = new System.Windows.Forms.TextBox();
            this.mat1x5 = new System.Windows.Forms.TextBox();
            this.matrixType = new System.Windows.Forms.ComboBox();
            this.matrixSize = new System.Windows.Forms.ComboBox();
            this.matrixSizeLabel = new System.Windows.Forms.Label();
            this.matrixTypeLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.loadMatrixButton = new System.Windows.Forms.Button();
            this.saveMatrixButton = new System.Windows.Forms.Button();
            this.operationsGroupbox = new System.Windows.Forms.GroupBox();
            this.okButton = new System.Windows.Forms.Button();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.matrixGroupbox.SuspendLayout();
            this.operationsGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // predefinedMatrixLabel
            // 
            this.predefinedMatrixLabel.Location = new System.Drawing.Point(12, 15);
            this.predefinedMatrixLabel.Name = "predefinedMatrixLabel";
            this.predefinedMatrixLabel.Size = new System.Drawing.Size(72, 13);
            this.predefinedMatrixLabel.TabIndex = 0;
            this.predefinedMatrixLabel.Text = "Select Matrix";
            // 
            // predefinedMatrix
            // 
            this.predefinedMatrix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.predefinedMatrix.Items.AddRange(new object[] {
            "None",
            "jvt",
            "flat",
            "Custom"});
            this.predefinedMatrix.Location = new System.Drawing.Point(90, 12);
            this.predefinedMatrix.Name = "predefinedMatrix";
            this.predefinedMatrix.Size = new System.Drawing.Size(121, 21);
            this.predefinedMatrix.TabIndex = 1;
            this.predefinedMatrix.SelectedIndexChanged += new System.EventHandler(this.predefinedMatrix_SelectedIndexChanged);
            // 
            // mat1x1
            // 
            this.mat1x1.Location = new System.Drawing.Point(8, 16);
            this.mat1x1.MaxLength = 3;
            this.mat1x1.Name = "mat1x1";
            this.mat1x1.Size = new System.Drawing.Size(24, 21);
            this.mat1x1.TabIndex = 0;
            this.mat1x1.Text = "16";
            // 
            // mat1x2
            // 
            this.mat1x2.Location = new System.Drawing.Point(32, 16);
            this.mat1x2.MaxLength = 3;
            this.mat1x2.Name = "mat1x2";
            this.mat1x2.Size = new System.Drawing.Size(24, 21);
            this.mat1x2.TabIndex = 1;
            this.mat1x2.Text = "16";
            // 
            // mat1x3
            // 
            this.mat1x3.Location = new System.Drawing.Point(56, 16);
            this.mat1x3.MaxLength = 3;
            this.mat1x3.Name = "mat1x3";
            this.mat1x3.Size = new System.Drawing.Size(24, 21);
            this.mat1x3.TabIndex = 2;
            this.mat1x3.Text = "16";
            // 
            // mat1x4
            // 
            this.mat1x4.Location = new System.Drawing.Point(80, 16);
            this.mat1x4.MaxLength = 3;
            this.mat1x4.Name = "mat1x4";
            this.mat1x4.Size = new System.Drawing.Size(24, 21);
            this.mat1x4.TabIndex = 3;
            this.mat1x4.Text = "16";
            // 
            // mat2x4
            // 
            this.mat2x4.Location = new System.Drawing.Point(80, 38);
            this.mat2x4.MaxLength = 3;
            this.mat2x4.Name = "mat2x4";
            this.mat2x4.Size = new System.Drawing.Size(24, 21);
            this.mat2x4.TabIndex = 11;
            this.mat2x4.Text = "16";
            // 
            // mat2x3
            // 
            this.mat2x3.Location = new System.Drawing.Point(56, 38);
            this.mat2x3.MaxLength = 3;
            this.mat2x3.Name = "mat2x3";
            this.mat2x3.Size = new System.Drawing.Size(24, 21);
            this.mat2x3.TabIndex = 10;
            this.mat2x3.Text = "16";
            // 
            // mat2x2
            // 
            this.mat2x2.Location = new System.Drawing.Point(32, 38);
            this.mat2x2.MaxLength = 3;
            this.mat2x2.Name = "mat2x2";
            this.mat2x2.Size = new System.Drawing.Size(24, 21);
            this.mat2x2.TabIndex = 9;
            this.mat2x2.Text = "16";
            // 
            // mat2x1
            // 
            this.mat2x1.Location = new System.Drawing.Point(8, 38);
            this.mat2x1.MaxLength = 3;
            this.mat2x1.Name = "mat2x1";
            this.mat2x1.Size = new System.Drawing.Size(24, 21);
            this.mat2x1.TabIndex = 8;
            this.mat2x1.Text = "16";
            // 
            // mat4x4
            // 
            this.mat4x4.Location = new System.Drawing.Point(80, 82);
            this.mat4x4.MaxLength = 3;
            this.mat4x4.Name = "mat4x4";
            this.mat4x4.Size = new System.Drawing.Size(24, 21);
            this.mat4x4.TabIndex = 27;
            this.mat4x4.Text = "16";
            // 
            // mat4x3
            // 
            this.mat4x3.Location = new System.Drawing.Point(56, 82);
            this.mat4x3.MaxLength = 3;
            this.mat4x3.Name = "mat4x3";
            this.mat4x3.Size = new System.Drawing.Size(24, 21);
            this.mat4x3.TabIndex = 26;
            this.mat4x3.Text = "16";
            // 
            // mat4x2
            // 
            this.mat4x2.Location = new System.Drawing.Point(32, 82);
            this.mat4x2.MaxLength = 3;
            this.mat4x2.Name = "mat4x2";
            this.mat4x2.Size = new System.Drawing.Size(24, 21);
            this.mat4x2.TabIndex = 25;
            this.mat4x2.Text = "16";
            // 
            // mat4x1
            // 
            this.mat4x1.Location = new System.Drawing.Point(8, 82);
            this.mat4x1.MaxLength = 3;
            this.mat4x1.Name = "mat4x1";
            this.mat4x1.Size = new System.Drawing.Size(24, 21);
            this.mat4x1.TabIndex = 24;
            this.mat4x1.Text = "16";
            // 
            // mat3x4
            // 
            this.mat3x4.Location = new System.Drawing.Point(80, 60);
            this.mat3x4.MaxLength = 3;
            this.mat3x4.Name = "mat3x4";
            this.mat3x4.Size = new System.Drawing.Size(24, 21);
            this.mat3x4.TabIndex = 19;
            this.mat3x4.Text = "16";
            // 
            // mat3x3
            // 
            this.mat3x3.Location = new System.Drawing.Point(56, 60);
            this.mat3x3.MaxLength = 3;
            this.mat3x3.Name = "mat3x3";
            this.mat3x3.Size = new System.Drawing.Size(24, 21);
            this.mat3x3.TabIndex = 18;
            this.mat3x3.Text = "16";
            // 
            // mat3x2
            // 
            this.mat3x2.Location = new System.Drawing.Point(32, 60);
            this.mat3x2.MaxLength = 3;
            this.mat3x2.Name = "mat3x2";
            this.mat3x2.Size = new System.Drawing.Size(24, 21);
            this.mat3x2.TabIndex = 17;
            this.mat3x2.Text = "16";
            // 
            // mat3x1
            // 
            this.mat3x1.Location = new System.Drawing.Point(8, 60);
            this.mat3x1.MaxLength = 3;
            this.mat3x1.Name = "mat3x1";
            this.mat3x1.Size = new System.Drawing.Size(24, 21);
            this.mat3x1.TabIndex = 16;
            this.mat3x1.Text = "16";
            // 
            // mat8x4
            // 
            this.mat8x4.Location = new System.Drawing.Point(80, 170);
            this.mat8x4.MaxLength = 3;
            this.mat8x4.Name = "mat8x4";
            this.mat8x4.Size = new System.Drawing.Size(24, 21);
            this.mat8x4.TabIndex = 59;
            this.mat8x4.Text = "16";
            // 
            // mat8x3
            // 
            this.mat8x3.Location = new System.Drawing.Point(56, 170);
            this.mat8x3.MaxLength = 3;
            this.mat8x3.Name = "mat8x3";
            this.mat8x3.Size = new System.Drawing.Size(24, 21);
            this.mat8x3.TabIndex = 58;
            this.mat8x3.Text = "16";
            // 
            // mat8x2
            // 
            this.mat8x2.Location = new System.Drawing.Point(32, 170);
            this.mat8x2.MaxLength = 3;
            this.mat8x2.Name = "mat8x2";
            this.mat8x2.Size = new System.Drawing.Size(24, 21);
            this.mat8x2.TabIndex = 57;
            this.mat8x2.Text = "16";
            // 
            // mat8x1
            // 
            this.mat8x1.Location = new System.Drawing.Point(8, 170);
            this.mat8x1.MaxLength = 3;
            this.mat8x1.Name = "mat8x1";
            this.mat8x1.Size = new System.Drawing.Size(24, 21);
            this.mat8x1.TabIndex = 56;
            this.mat8x1.Text = "16";
            // 
            // mat7x4
            // 
            this.mat7x4.Location = new System.Drawing.Point(80, 148);
            this.mat7x4.MaxLength = 3;
            this.mat7x4.Name = "mat7x4";
            this.mat7x4.Size = new System.Drawing.Size(24, 21);
            this.mat7x4.TabIndex = 51;
            this.mat7x4.Text = "16";
            // 
            // mat7x3
            // 
            this.mat7x3.Location = new System.Drawing.Point(56, 148);
            this.mat7x3.MaxLength = 3;
            this.mat7x3.Name = "mat7x3";
            this.mat7x3.Size = new System.Drawing.Size(24, 21);
            this.mat7x3.TabIndex = 50;
            this.mat7x3.Text = "16";
            // 
            // mat7x2
            // 
            this.mat7x2.Location = new System.Drawing.Point(32, 148);
            this.mat7x2.MaxLength = 3;
            this.mat7x2.Name = "mat7x2";
            this.mat7x2.Size = new System.Drawing.Size(24, 21);
            this.mat7x2.TabIndex = 49;
            this.mat7x2.Text = "16";
            // 
            // mat7x1
            // 
            this.mat7x1.Location = new System.Drawing.Point(8, 148);
            this.mat7x1.MaxLength = 3;
            this.mat7x1.Name = "mat7x1";
            this.mat7x1.Size = new System.Drawing.Size(24, 21);
            this.mat7x1.TabIndex = 48;
            this.mat7x1.Text = "16";
            // 
            // mat6x4
            // 
            this.mat6x4.Location = new System.Drawing.Point(80, 126);
            this.mat6x4.MaxLength = 3;
            this.mat6x4.Name = "mat6x4";
            this.mat6x4.Size = new System.Drawing.Size(24, 21);
            this.mat6x4.TabIndex = 43;
            this.mat6x4.Text = "16";
            // 
            // mat6x3
            // 
            this.mat6x3.Location = new System.Drawing.Point(56, 126);
            this.mat6x3.MaxLength = 3;
            this.mat6x3.Name = "mat6x3";
            this.mat6x3.Size = new System.Drawing.Size(24, 21);
            this.mat6x3.TabIndex = 42;
            this.mat6x3.Text = "16";
            // 
            // mat6x2
            // 
            this.mat6x2.Location = new System.Drawing.Point(32, 126);
            this.mat6x2.MaxLength = 3;
            this.mat6x2.Name = "mat6x2";
            this.mat6x2.Size = new System.Drawing.Size(24, 21);
            this.mat6x2.TabIndex = 41;
            this.mat6x2.Text = "16";
            // 
            // mat6x1
            // 
            this.mat6x1.Location = new System.Drawing.Point(8, 126);
            this.mat6x1.MaxLength = 3;
            this.mat6x1.Name = "mat6x1";
            this.mat6x1.Size = new System.Drawing.Size(24, 21);
            this.mat6x1.TabIndex = 40;
            this.mat6x1.Text = "16";
            // 
            // mat5x4
            // 
            this.mat5x4.Location = new System.Drawing.Point(80, 104);
            this.mat5x4.MaxLength = 3;
            this.mat5x4.Name = "mat5x4";
            this.mat5x4.Size = new System.Drawing.Size(24, 21);
            this.mat5x4.TabIndex = 35;
            this.mat5x4.Text = "16";
            // 
            // mat5x3
            // 
            this.mat5x3.Location = new System.Drawing.Point(56, 104);
            this.mat5x3.MaxLength = 3;
            this.mat5x3.Name = "mat5x3";
            this.mat5x3.Size = new System.Drawing.Size(24, 21);
            this.mat5x3.TabIndex = 34;
            this.mat5x3.Text = "16";
            // 
            // mat5x2
            // 
            this.mat5x2.Location = new System.Drawing.Point(32, 104);
            this.mat5x2.MaxLength = 3;
            this.mat5x2.Name = "mat5x2";
            this.mat5x2.Size = new System.Drawing.Size(24, 21);
            this.mat5x2.TabIndex = 33;
            this.mat5x2.Text = "16";
            // 
            // mat5x1
            // 
            this.mat5x1.Location = new System.Drawing.Point(8, 104);
            this.mat5x1.MaxLength = 3;
            this.mat5x1.Name = "mat5x1";
            this.mat5x1.Size = new System.Drawing.Size(24, 21);
            this.mat5x1.TabIndex = 32;
            this.mat5x1.Text = "16";
            // 
            // matrixGroupbox
            // 
            this.matrixGroupbox.Controls.Add(this.mat8x8);
            this.matrixGroupbox.Controls.Add(this.mat8x7);
            this.matrixGroupbox.Controls.Add(this.mat8x6);
            this.matrixGroupbox.Controls.Add(this.mat8x5);
            this.matrixGroupbox.Controls.Add(this.mat7x8);
            this.matrixGroupbox.Controls.Add(this.mat7x7);
            this.matrixGroupbox.Controls.Add(this.mat7x6);
            this.matrixGroupbox.Controls.Add(this.mat7x5);
            this.matrixGroupbox.Controls.Add(this.mat6x8);
            this.matrixGroupbox.Controls.Add(this.mat6x7);
            this.matrixGroupbox.Controls.Add(this.mat6x6);
            this.matrixGroupbox.Controls.Add(this.mat6x5);
            this.matrixGroupbox.Controls.Add(this.mat5x8);
            this.matrixGroupbox.Controls.Add(this.mat5x7);
            this.matrixGroupbox.Controls.Add(this.mat5x6);
            this.matrixGroupbox.Controls.Add(this.mat5x5);
            this.matrixGroupbox.Controls.Add(this.mat4x8);
            this.matrixGroupbox.Controls.Add(this.mat4x7);
            this.matrixGroupbox.Controls.Add(this.mat4x6);
            this.matrixGroupbox.Controls.Add(this.mat4x5);
            this.matrixGroupbox.Controls.Add(this.mat3x8);
            this.matrixGroupbox.Controls.Add(this.mat3x7);
            this.matrixGroupbox.Controls.Add(this.mat3x6);
            this.matrixGroupbox.Controls.Add(this.mat3x5);
            this.matrixGroupbox.Controls.Add(this.mat2x8);
            this.matrixGroupbox.Controls.Add(this.mat2x7);
            this.matrixGroupbox.Controls.Add(this.mat2x6);
            this.matrixGroupbox.Controls.Add(this.mat2x5);
            this.matrixGroupbox.Controls.Add(this.mat1x8);
            this.matrixGroupbox.Controls.Add(this.mat1x7);
            this.matrixGroupbox.Controls.Add(this.mat1x6);
            this.matrixGroupbox.Controls.Add(this.mat1x5);
            this.matrixGroupbox.Controls.Add(this.mat5x3);
            this.matrixGroupbox.Controls.Add(this.mat7x2);
            this.matrixGroupbox.Controls.Add(this.mat6x1);
            this.matrixGroupbox.Controls.Add(this.mat6x2);
            this.matrixGroupbox.Controls.Add(this.mat3x4);
            this.matrixGroupbox.Controls.Add(this.mat3x3);
            this.matrixGroupbox.Controls.Add(this.mat7x1);
            this.matrixGroupbox.Controls.Add(this.mat4x4);
            this.matrixGroupbox.Controls.Add(this.mat4x3);
            this.matrixGroupbox.Controls.Add(this.mat4x2);
            this.matrixGroupbox.Controls.Add(this.mat7x3);
            this.matrixGroupbox.Controls.Add(this.mat5x4);
            this.matrixGroupbox.Controls.Add(this.mat3x2);
            this.matrixGroupbox.Controls.Add(this.mat2x3);
            this.matrixGroupbox.Controls.Add(this.mat7x4);
            this.matrixGroupbox.Controls.Add(this.mat4x1);
            this.matrixGroupbox.Controls.Add(this.mat3x1);
            this.matrixGroupbox.Controls.Add(this.mat1x1);
            this.matrixGroupbox.Controls.Add(this.mat8x4);
            this.matrixGroupbox.Controls.Add(this.mat6x3);
            this.matrixGroupbox.Controls.Add(this.mat1x3);
            this.matrixGroupbox.Controls.Add(this.mat1x2);
            this.matrixGroupbox.Controls.Add(this.mat6x4);
            this.matrixGroupbox.Controls.Add(this.mat1x4);
            this.matrixGroupbox.Controls.Add(this.mat2x4);
            this.matrixGroupbox.Controls.Add(this.mat8x3);
            this.matrixGroupbox.Controls.Add(this.mat2x2);
            this.matrixGroupbox.Controls.Add(this.mat2x1);
            this.matrixGroupbox.Controls.Add(this.mat8x2);
            this.matrixGroupbox.Controls.Add(this.mat8x1);
            this.matrixGroupbox.Controls.Add(this.mat5x1);
            this.matrixGroupbox.Controls.Add(this.mat5x2);
            this.matrixGroupbox.Enabled = false;
            this.matrixGroupbox.Location = new System.Drawing.Point(8, 39);
            this.matrixGroupbox.Name = "matrixGroupbox";
            this.matrixGroupbox.Size = new System.Drawing.Size(208, 200);
            this.matrixGroupbox.TabIndex = 2;
            this.matrixGroupbox.TabStop = false;
            this.matrixGroupbox.Text = "Matrix Coefficients";
            // 
            // mat8x8
            // 
            this.mat8x8.Location = new System.Drawing.Point(176, 170);
            this.mat8x8.MaxLength = 3;
            this.mat8x8.Name = "mat8x8";
            this.mat8x8.Size = new System.Drawing.Size(24, 21);
            this.mat8x8.TabIndex = 63;
            this.mat8x8.Text = "16";
            // 
            // mat8x7
            // 
            this.mat8x7.Location = new System.Drawing.Point(152, 170);
            this.mat8x7.MaxLength = 3;
            this.mat8x7.Name = "mat8x7";
            this.mat8x7.Size = new System.Drawing.Size(24, 21);
            this.mat8x7.TabIndex = 62;
            this.mat8x7.Text = "16";
            // 
            // mat8x6
            // 
            this.mat8x6.Location = new System.Drawing.Point(128, 170);
            this.mat8x6.MaxLength = 3;
            this.mat8x6.Name = "mat8x6";
            this.mat8x6.Size = new System.Drawing.Size(24, 21);
            this.mat8x6.TabIndex = 61;
            this.mat8x6.Text = "16";
            // 
            // mat8x5
            // 
            this.mat8x5.Location = new System.Drawing.Point(104, 170);
            this.mat8x5.MaxLength = 3;
            this.mat8x5.Name = "mat8x5";
            this.mat8x5.Size = new System.Drawing.Size(24, 21);
            this.mat8x5.TabIndex = 60;
            this.mat8x5.Text = "16";
            // 
            // mat7x8
            // 
            this.mat7x8.Location = new System.Drawing.Point(176, 148);
            this.mat7x8.MaxLength = 3;
            this.mat7x8.Name = "mat7x8";
            this.mat7x8.Size = new System.Drawing.Size(24, 21);
            this.mat7x8.TabIndex = 55;
            this.mat7x8.Text = "16";
            // 
            // mat7x7
            // 
            this.mat7x7.Location = new System.Drawing.Point(152, 148);
            this.mat7x7.MaxLength = 3;
            this.mat7x7.Name = "mat7x7";
            this.mat7x7.Size = new System.Drawing.Size(24, 21);
            this.mat7x7.TabIndex = 54;
            this.mat7x7.Text = "16";
            // 
            // mat7x6
            // 
            this.mat7x6.Location = new System.Drawing.Point(128, 148);
            this.mat7x6.MaxLength = 3;
            this.mat7x6.Name = "mat7x6";
            this.mat7x6.Size = new System.Drawing.Size(24, 21);
            this.mat7x6.TabIndex = 53;
            this.mat7x6.Text = "16";
            // 
            // mat7x5
            // 
            this.mat7x5.Location = new System.Drawing.Point(104, 148);
            this.mat7x5.MaxLength = 3;
            this.mat7x5.Name = "mat7x5";
            this.mat7x5.Size = new System.Drawing.Size(24, 21);
            this.mat7x5.TabIndex = 52;
            this.mat7x5.Text = "16";
            // 
            // mat6x8
            // 
            this.mat6x8.Location = new System.Drawing.Point(176, 126);
            this.mat6x8.MaxLength = 3;
            this.mat6x8.Name = "mat6x8";
            this.mat6x8.Size = new System.Drawing.Size(24, 21);
            this.mat6x8.TabIndex = 47;
            this.mat6x8.Text = "16";
            // 
            // mat6x7
            // 
            this.mat6x7.Location = new System.Drawing.Point(152, 126);
            this.mat6x7.MaxLength = 3;
            this.mat6x7.Name = "mat6x7";
            this.mat6x7.Size = new System.Drawing.Size(24, 21);
            this.mat6x7.TabIndex = 46;
            this.mat6x7.Text = "16";
            // 
            // mat6x6
            // 
            this.mat6x6.Location = new System.Drawing.Point(128, 126);
            this.mat6x6.MaxLength = 3;
            this.mat6x6.Name = "mat6x6";
            this.mat6x6.Size = new System.Drawing.Size(24, 21);
            this.mat6x6.TabIndex = 45;
            this.mat6x6.Text = "16";
            // 
            // mat6x5
            // 
            this.mat6x5.Location = new System.Drawing.Point(104, 126);
            this.mat6x5.MaxLength = 3;
            this.mat6x5.Name = "mat6x5";
            this.mat6x5.Size = new System.Drawing.Size(24, 21);
            this.mat6x5.TabIndex = 44;
            this.mat6x5.Text = "16";
            // 
            // mat5x8
            // 
            this.mat5x8.Location = new System.Drawing.Point(176, 104);
            this.mat5x8.MaxLength = 3;
            this.mat5x8.Name = "mat5x8";
            this.mat5x8.Size = new System.Drawing.Size(24, 21);
            this.mat5x8.TabIndex = 39;
            this.mat5x8.Text = "16";
            // 
            // mat5x7
            // 
            this.mat5x7.Location = new System.Drawing.Point(152, 104);
            this.mat5x7.MaxLength = 3;
            this.mat5x7.Name = "mat5x7";
            this.mat5x7.Size = new System.Drawing.Size(24, 21);
            this.mat5x7.TabIndex = 38;
            this.mat5x7.Text = "16";
            // 
            // mat5x6
            // 
            this.mat5x6.Location = new System.Drawing.Point(128, 104);
            this.mat5x6.MaxLength = 3;
            this.mat5x6.Name = "mat5x6";
            this.mat5x6.Size = new System.Drawing.Size(24, 21);
            this.mat5x6.TabIndex = 37;
            this.mat5x6.Text = "16";
            // 
            // mat5x5
            // 
            this.mat5x5.Location = new System.Drawing.Point(104, 104);
            this.mat5x5.MaxLength = 3;
            this.mat5x5.Name = "mat5x5";
            this.mat5x5.Size = new System.Drawing.Size(24, 21);
            this.mat5x5.TabIndex = 36;
            this.mat5x5.Text = "16";
            // 
            // mat4x8
            // 
            this.mat4x8.Location = new System.Drawing.Point(176, 82);
            this.mat4x8.MaxLength = 3;
            this.mat4x8.Name = "mat4x8";
            this.mat4x8.Size = new System.Drawing.Size(24, 21);
            this.mat4x8.TabIndex = 31;
            this.mat4x8.Text = "16";
            // 
            // mat4x7
            // 
            this.mat4x7.Location = new System.Drawing.Point(152, 82);
            this.mat4x7.MaxLength = 3;
            this.mat4x7.Name = "mat4x7";
            this.mat4x7.Size = new System.Drawing.Size(24, 21);
            this.mat4x7.TabIndex = 30;
            this.mat4x7.Text = "16";
            // 
            // mat4x6
            // 
            this.mat4x6.Location = new System.Drawing.Point(128, 82);
            this.mat4x6.MaxLength = 3;
            this.mat4x6.Name = "mat4x6";
            this.mat4x6.Size = new System.Drawing.Size(24, 21);
            this.mat4x6.TabIndex = 29;
            this.mat4x6.Text = "16";
            // 
            // mat4x5
            // 
            this.mat4x5.Location = new System.Drawing.Point(104, 82);
            this.mat4x5.MaxLength = 3;
            this.mat4x5.Name = "mat4x5";
            this.mat4x5.Size = new System.Drawing.Size(24, 21);
            this.mat4x5.TabIndex = 28;
            this.mat4x5.Text = "16";
            // 
            // mat3x8
            // 
            this.mat3x8.Location = new System.Drawing.Point(176, 60);
            this.mat3x8.MaxLength = 3;
            this.mat3x8.Name = "mat3x8";
            this.mat3x8.Size = new System.Drawing.Size(24, 21);
            this.mat3x8.TabIndex = 23;
            this.mat3x8.Text = "16";
            // 
            // mat3x7
            // 
            this.mat3x7.Location = new System.Drawing.Point(152, 60);
            this.mat3x7.MaxLength = 3;
            this.mat3x7.Name = "mat3x7";
            this.mat3x7.Size = new System.Drawing.Size(24, 21);
            this.mat3x7.TabIndex = 22;
            this.mat3x7.Text = "16";
            // 
            // mat3x6
            // 
            this.mat3x6.Location = new System.Drawing.Point(128, 60);
            this.mat3x6.MaxLength = 3;
            this.mat3x6.Name = "mat3x6";
            this.mat3x6.Size = new System.Drawing.Size(24, 21);
            this.mat3x6.TabIndex = 21;
            this.mat3x6.Text = "16";
            // 
            // mat3x5
            // 
            this.mat3x5.Location = new System.Drawing.Point(104, 60);
            this.mat3x5.MaxLength = 3;
            this.mat3x5.Name = "mat3x5";
            this.mat3x5.Size = new System.Drawing.Size(24, 21);
            this.mat3x5.TabIndex = 20;
            this.mat3x5.Text = "16";
            // 
            // mat2x8
            // 
            this.mat2x8.Location = new System.Drawing.Point(176, 38);
            this.mat2x8.MaxLength = 3;
            this.mat2x8.Name = "mat2x8";
            this.mat2x8.Size = new System.Drawing.Size(24, 21);
            this.mat2x8.TabIndex = 15;
            this.mat2x8.Text = "16";
            // 
            // mat2x7
            // 
            this.mat2x7.Location = new System.Drawing.Point(152, 38);
            this.mat2x7.MaxLength = 3;
            this.mat2x7.Name = "mat2x7";
            this.mat2x7.Size = new System.Drawing.Size(24, 21);
            this.mat2x7.TabIndex = 14;
            this.mat2x7.Text = "16";
            // 
            // mat2x6
            // 
            this.mat2x6.Location = new System.Drawing.Point(128, 38);
            this.mat2x6.MaxLength = 3;
            this.mat2x6.Name = "mat2x6";
            this.mat2x6.Size = new System.Drawing.Size(24, 21);
            this.mat2x6.TabIndex = 13;
            this.mat2x6.Text = "16";
            // 
            // mat2x5
            // 
            this.mat2x5.Location = new System.Drawing.Point(104, 38);
            this.mat2x5.MaxLength = 3;
            this.mat2x5.Name = "mat2x5";
            this.mat2x5.Size = new System.Drawing.Size(24, 21);
            this.mat2x5.TabIndex = 12;
            this.mat2x5.Text = "16";
            // 
            // mat1x8
            // 
            this.mat1x8.Location = new System.Drawing.Point(176, 16);
            this.mat1x8.MaxLength = 3;
            this.mat1x8.Name = "mat1x8";
            this.mat1x8.Size = new System.Drawing.Size(24, 21);
            this.mat1x8.TabIndex = 7;
            this.mat1x8.Text = "16";
            // 
            // mat1x7
            // 
            this.mat1x7.Location = new System.Drawing.Point(152, 16);
            this.mat1x7.MaxLength = 3;
            this.mat1x7.Name = "mat1x7";
            this.mat1x7.Size = new System.Drawing.Size(24, 21);
            this.mat1x7.TabIndex = 6;
            this.mat1x7.Text = "16";
            // 
            // mat1x6
            // 
            this.mat1x6.Location = new System.Drawing.Point(128, 16);
            this.mat1x6.MaxLength = 3;
            this.mat1x6.Name = "mat1x6";
            this.mat1x6.Size = new System.Drawing.Size(24, 21);
            this.mat1x6.TabIndex = 5;
            this.mat1x6.Text = "16";
            // 
            // mat1x5
            // 
            this.mat1x5.Location = new System.Drawing.Point(104, 16);
            this.mat1x5.MaxLength = 3;
            this.mat1x5.Name = "mat1x5";
            this.mat1x5.Size = new System.Drawing.Size(24, 21);
            this.mat1x5.TabIndex = 4;
            this.mat1x5.Text = "16";
            // 
            // matrixType
            // 
            this.matrixType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.matrixType.Items.AddRange(new object[] {
            "Global",
            "Luma/Chroma"});
            this.matrixType.Location = new System.Drawing.Point(72, 47);
            this.matrixType.Name = "matrixType";
            this.matrixType.Size = new System.Drawing.Size(98, 21);
            this.matrixType.TabIndex = 0;
            this.matrixType.SelectedIndexChanged += new System.EventHandler(this.matrixType_SelectedIndexChanged);
            // 
            // matrixSize
            // 
            this.matrixSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.matrixSize.Items.AddRange(new object[] {
            "4x4",
            "8x8"});
            this.matrixSize.Location = new System.Drawing.Point(72, 20);
            this.matrixSize.Name = "matrixSize";
            this.matrixSize.Size = new System.Drawing.Size(98, 21);
            this.matrixSize.TabIndex = 1;
            this.matrixSize.SelectedIndexChanged += new System.EventHandler(this.matrixSize_SelectedIndexChanged);
            // 
            // matrixSizeLabel
            // 
            this.matrixSizeLabel.Location = new System.Drawing.Point(16, 23);
            this.matrixSizeLabel.Name = "matrixSizeLabel";
            this.matrixSizeLabel.Size = new System.Drawing.Size(48, 23);
            this.matrixSizeLabel.TabIndex = 2;
            this.matrixSizeLabel.Text = "Size";
            // 
            // matrixTypeLabel
            // 
            this.matrixTypeLabel.Location = new System.Drawing.Point(16, 50);
            this.matrixTypeLabel.Name = "matrixTypeLabel";
            this.matrixTypeLabel.Size = new System.Drawing.Size(48, 23);
            this.matrixTypeLabel.TabIndex = 3;
            this.matrixTypeLabel.Text = "Type";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "txt";
            this.openFileDialog.Filter = "Quantizer Matrix Files (*.cfg)|*.cfg|All Files (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Quantizer Matrix Files (*.cfg)|*.cfg|All Files (*.*)|*.*";
            // 
            // loadMatrixButton
            // 
            this.loadMatrixButton.Location = new System.Drawing.Point(305, 126);
            this.loadMatrixButton.Name = "loadMatrixButton";
            this.loadMatrixButton.Size = new System.Drawing.Size(48, 23);
            this.loadMatrixButton.TabIndex = 4;
            this.loadMatrixButton.Text = "Load";
            this.loadMatrixButton.Click += new System.EventHandler(this.loadMatrixButton_Click);
            // 
            // saveMatrixButton
            // 
            this.saveMatrixButton.Location = new System.Drawing.Point(359, 126);
            this.saveMatrixButton.Name = "saveMatrixButton";
            this.saveMatrixButton.Size = new System.Drawing.Size(48, 23);
            this.saveMatrixButton.TabIndex = 5;
            this.saveMatrixButton.Text = "Save";
            this.saveMatrixButton.Click += new System.EventHandler(this.saveMatrixButton_Click);
            // 
            // operationsGroupbox
            // 
            this.operationsGroupbox.Controls.Add(this.matrixSize);
            this.operationsGroupbox.Controls.Add(this.matrixType);
            this.operationsGroupbox.Controls.Add(this.matrixTypeLabel);
            this.operationsGroupbox.Controls.Add(this.matrixSizeLabel);
            this.operationsGroupbox.Enabled = false;
            this.operationsGroupbox.Location = new System.Drawing.Point(224, 39);
            this.operationsGroupbox.Name = "operationsGroupbox";
            this.operationsGroupbox.Size = new System.Drawing.Size(183, 81);
            this.operationsGroupbox.TabIndex = 3;
            this.operationsGroupbox.TabStop = false;
            this.operationsGroupbox.Text = "Operations";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(375, 245);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(32, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "x264 Quantizer matrix editor";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(8, 245);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 6;
            // 
            // QuantizerMatrixDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(415, 277);
            this.Controls.Add(this.operationsGroupbox);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.matrixGroupbox);
            this.Controls.Add(this.predefinedMatrix);
            this.Controls.Add(this.predefinedMatrixLabel);
            this.Controls.Add(this.loadMatrixButton);
            this.Controls.Add(this.saveMatrixButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "QuantizerMatrixDialog";
            this.Text = "MeGUI - Quantizer Matrix Editor";
            this.Load += new System.EventHandler(this.QuantizerMatrixDialog_Load);
            this.matrixGroupbox.ResumeLayout(false);
            this.matrixGroupbox.PerformLayout();
            this.operationsGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ComboBox predefinedMatrix;
        private System.Windows.Forms.Label predefinedMatrixLabel;
        private System.Windows.Forms.TextBox mat1x1;
        private System.Windows.Forms.TextBox mat1x3;
        private System.Windows.Forms.TextBox mat1x4;
        private System.Windows.Forms.TextBox mat2x4;
        private System.Windows.Forms.TextBox mat2x3;
        private System.Windows.Forms.TextBox mat2x2;
        private System.Windows.Forms.TextBox mat2x1;
        private System.Windows.Forms.TextBox mat4x4;
        private System.Windows.Forms.TextBox mat4x3;
        private System.Windows.Forms.TextBox mat4x2;
        private System.Windows.Forms.TextBox mat4x1;
        private System.Windows.Forms.TextBox mat3x4;
        private System.Windows.Forms.TextBox mat3x3;
        private System.Windows.Forms.TextBox mat3x2;
        private System.Windows.Forms.TextBox mat3x1;
        private System.Windows.Forms.TextBox mat8x4;
        private System.Windows.Forms.TextBox mat8x3;
        private System.Windows.Forms.TextBox mat8x2;
        private System.Windows.Forms.TextBox mat8x1;
        private System.Windows.Forms.TextBox mat7x4;
        private System.Windows.Forms.TextBox mat7x3;
        private System.Windows.Forms.TextBox mat7x2;
        private System.Windows.Forms.TextBox mat7x1;
        private System.Windows.Forms.TextBox mat6x4;
        private System.Windows.Forms.TextBox mat6x3;
        private System.Windows.Forms.TextBox mat6x2;
        private System.Windows.Forms.TextBox mat6x1;
        private System.Windows.Forms.TextBox mat5x4;
        private System.Windows.Forms.TextBox mat5x3;
        private System.Windows.Forms.TextBox mat5x2;
        private System.Windows.Forms.TextBox mat5x1;
        private System.Windows.Forms.GroupBox matrixGroupbox;
        private System.Windows.Forms.TextBox mat1x8;
        private System.Windows.Forms.TextBox mat1x7;
        private System.Windows.Forms.TextBox mat1x6;
        private System.Windows.Forms.TextBox mat1x5;
        private System.Windows.Forms.TextBox mat2x8;
        private System.Windows.Forms.TextBox mat2x7;
        private System.Windows.Forms.TextBox mat2x6;
        private System.Windows.Forms.TextBox mat2x5;
        private System.Windows.Forms.TextBox mat3x8;
        private System.Windows.Forms.TextBox mat3x7;
        private System.Windows.Forms.TextBox mat3x6;
        private System.Windows.Forms.TextBox mat3x5;
        private System.Windows.Forms.TextBox mat4x8;
        private System.Windows.Forms.TextBox mat4x7;
        private System.Windows.Forms.TextBox mat4x6;
        private System.Windows.Forms.TextBox mat4x5;
        private System.Windows.Forms.TextBox mat5x8;
        private System.Windows.Forms.TextBox mat5x7;
        private System.Windows.Forms.TextBox mat5x6;
        private System.Windows.Forms.TextBox mat5x5;
        private System.Windows.Forms.TextBox mat6x8;
        private System.Windows.Forms.TextBox mat6x7;
        private System.Windows.Forms.TextBox mat6x6;
        private System.Windows.Forms.TextBox mat6x5;
        private System.Windows.Forms.TextBox mat7x8;
        private System.Windows.Forms.TextBox mat7x7;
        private System.Windows.Forms.TextBox mat7x6;
        private System.Windows.Forms.TextBox mat7x5;
        private System.Windows.Forms.TextBox mat8x8;
        private System.Windows.Forms.TextBox mat8x7;
        private System.Windows.Forms.TextBox mat8x6;
        private System.Windows.Forms.TextBox mat8x5;
        private System.Windows.Forms.ComboBox matrixType;
        private System.Windows.Forms.ComboBox matrixSize;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label matrixSizeLabel;
        private System.Windows.Forms.Label matrixTypeLabel;
        private System.Windows.Forms.Button loadMatrixButton;
        private System.Windows.Forms.Button saveMatrixButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox operationsGroupbox;
        private System.Windows.Forms.TextBox mat1x2;
        private MeGUI.core.gui.HelpButton helpButton1;
    }
}
