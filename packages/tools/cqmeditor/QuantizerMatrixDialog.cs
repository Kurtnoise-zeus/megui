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

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    /// <summary>
	/// Summary description for QuantizerMatrixDialog.
	/// </summary>
	public partial class QuantizerMatrixDialog : System.Windows.Forms.Form
	{
		private int[,] currentMatrix;
		private int[,] I8x8, P8x8, I4x4L, I4x4CU, I4x4CY, P4x4L, P4x4CU, P4x4CY;
		private int[,] jvtI8x8, jvtP8x8, jvtI4x4, jvtP4x4, flat8x8, flat4x4;
		private bool doEvents = true;
		private StringBuilder sb;
		private MatrixConfig currentConfig;

		#region start/stop
		public QuantizerMatrixDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			currentMatrix = new int[16, 16];
			jvtI4x4 = new int[,] {{6,13,20,28}, {13,20,28,32}, {20,28,32,37},
					{28,32,37,42}};
			jvtP4x4 = new int[,] {{10,14,20,24}, {14,20,24,27}, {20,24,27,30}, 
					{24,27,30,34}};
			jvtI8x8 = new int[,] {{6,10,13,16,18,23,25,27}, {10,11,16,18,23,25,27,29}, 
				{13,16,18,23,25,27,29,31}, {16,18,23,25,27,29,31,33},
				{18,23,25,27,29,31,33,36}, {23,25,27,29,31,33,36,38}, 
				{25,27,29,31,33,36,38,40}, {27,29,31,33,36,38,40,42}};
			jvtP8x8 = new int[,] {{9,13,15,17,19,21,22,24}, {13,13,17,19,21,22,24,25},
				{15,17,19,21,22,24,25,27}, {17,19,21,22,24,25,27,28}, 
				{19,21,22,24,25,27,28,30}, {21,22,24,25,27,28,30,32}, 
				{22,24,25,27,28,30,32,33}, {24,25,27,28,30,32,33,35}};
			flat4x4 = new int[,] {{16,16,16,16},{16,16,16,16},{16,16,16,16},
				{16,16,16,16}};
			flat8x8 = new int[,] {{16,16,16,16,16,16,16,16},{16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16},{16,16,16,16,16,16,16,16}, 
				{16,16,16,16,16,16,16,16},{16,16,16,16,16,16,16,16}, 
				{16,16,16,16,16,16,16,16},{16,16,16,16,16,16,16,16}};
			sb = new StringBuilder();
		}

		private void QuantizerMatrixDialog_Load(object sender, System.EventArgs e)
		{
			this.predefinedMatrix.SelectedIndex = 0;
		}
		#endregion
		#region loading & saving matrices
		public void blankMatrix()
		{
			mat1x5.Text = "0";
			mat1x6.Text = "0";
			mat1x7.Text = "0";
			mat1x8.Text = "0";
			mat2x5.Text = "0";
			mat2x6.Text = "0";
			mat2x7.Text = "0";
			mat2x8.Text = "0";
			mat3x5.Text = "0";
			mat3x6.Text = "0";
			mat3x7.Text = "0";
			mat3x8.Text = "0";
			mat4x5.Text = "0";
			mat4x6.Text = "0";
			mat4x7.Text = "0";
			mat4x8.Text = "0";
			mat5x1.Text = "0";
			mat5x2.Text = "0";
			mat5x3.Text = "0";
			mat5x4.Text = "0";
			mat5x5.Text = "0";
			mat5x6.Text = "0";
			mat5x7.Text = "0";
			mat5x8.Text = "0";
			mat6x1.Text = "0";
			mat6x2.Text = "0";
			mat6x3.Text = "0";
			mat6x4.Text = "0";
			mat6x5.Text = "0";
			mat6x6.Text = "0";
			mat6x7.Text = "0";
			mat6x8.Text = "0";
			mat7x1.Text = "0";
			mat7x2.Text = "0";
			mat7x3.Text = "0";
			mat7x4.Text = "0";
			mat7x5.Text = "0";
			mat7x6.Text = "0";
			mat7x7.Text = "0";
			mat7x8.Text = "0";
			mat8x1.Text = "0";
			mat8x2.Text = "0";
			mat8x3.Text = "0";
			mat8x4.Text = "0";
			mat8x5.Text = "0";
			mat8x6.Text = "0";
			mat8x7.Text = "0";
			mat8x8.Text = "0";

			mat1x1.Text = "0";
			mat1x2.Text = "0";
			mat1x3.Text = "0";
			mat1x4.Text = "0";
			mat2x1.Text = "0";
			mat2x2.Text = "0";
			mat2x3.Text = "0";
			mat2x4.Text = "0";
			mat3x1.Text = "0";
			mat3x2.Text = "0";
			mat3x3.Text = "0";
			mat3x4.Text = "0";
			mat4x1.Text = "0";
			mat4x2.Text = "0";
			mat4x3.Text = "0";
			mat4x4.Text = "0";
		}
		/// <summary>
		/// loads a matrix into the GUI
		/// </summary>
		/// <param name="matrix"></param>
		private void loadMatrix(int[,] matrix)
		{
			switch (matrix.Length)
			{
				case 64:
					mat1x5.Text = matrix[0,4].ToString();
					mat1x6.Text = matrix[0,5].ToString();
					mat1x7.Text = matrix[0,6].ToString();
					mat1x8.Text = matrix[0,7].ToString();
					mat2x5.Text = matrix[1,4].ToString();
					mat2x6.Text = matrix[1,5].ToString();
					mat2x7.Text = matrix[1,6].ToString();
					mat2x8.Text = matrix[1,7].ToString();
					mat3x5.Text = matrix[2,4].ToString();
					mat3x6.Text = matrix[2,5].ToString();
					mat3x7.Text = matrix[2,6].ToString();
					mat3x8.Text = matrix[2,7].ToString();
					mat4x5.Text = matrix[3,4].ToString();
					mat4x6.Text = matrix[3,5].ToString();
					mat4x7.Text = matrix[3,6].ToString();
					mat4x8.Text = matrix[3,7].ToString();
					mat5x1.Text = matrix[4,0].ToString();
					mat5x2.Text = matrix[4,1].ToString();
					mat5x3.Text = matrix[4,2].ToString();
					mat5x4.Text = matrix[4,3].ToString();
					mat5x5.Text = matrix[4,4].ToString();
					mat5x6.Text = matrix[4,5].ToString();
					mat5x7.Text = matrix[4,6].ToString();
					mat5x8.Text = matrix[4,7].ToString();
					mat6x1.Text = matrix[4,0].ToString();
					mat6x2.Text = matrix[5,1].ToString();
					mat6x3.Text = matrix[5,2].ToString();
					mat6x4.Text = matrix[5,3].ToString();
					mat6x5.Text = matrix[5,4].ToString();
					mat6x6.Text = matrix[5,5].ToString();
					mat6x7.Text = matrix[5,6].ToString();
					mat6x8.Text = matrix[5,7].ToString();
					mat7x1.Text = matrix[6,0].ToString();
					mat7x2.Text = matrix[6,1].ToString();
					mat7x3.Text = matrix[6,2].ToString();
					mat7x4.Text = matrix[6,3].ToString();
					mat7x5.Text = matrix[6,4].ToString();
					mat7x6.Text = matrix[6,5].ToString();
					mat7x7.Text = matrix[6,6].ToString();
					mat7x8.Text = matrix[6,7].ToString();
					mat8x1.Text = matrix[7,0].ToString();
					mat8x2.Text = matrix[7,1].ToString();
					mat8x3.Text = matrix[7,2].ToString();
					mat8x4.Text = matrix[7,3].ToString();
					mat8x5.Text = matrix[7,4].ToString();
					mat8x6.Text = matrix[7,5].ToString();
					mat8x7.Text = matrix[7,6].ToString();
					mat8x8.Text = matrix[7,7].ToString();

					mat1x1.Text = matrix[0,0].ToString();
					mat1x2.Text = matrix[0,1].ToString();
					mat1x3.Text = matrix[0,2].ToString();
					mat1x4.Text = matrix[0,3].ToString();
					mat2x1.Text = matrix[1,0].ToString();
					mat2x2.Text = matrix[1,1].ToString();
					mat2x3.Text = matrix[1,2].ToString();
					mat2x4.Text = matrix[1,3].ToString();
					mat3x1.Text = matrix[2,0].ToString();
					mat3x2.Text = matrix[2,1].ToString();
					mat3x3.Text = matrix[2,2].ToString();
					mat3x4.Text = matrix[2,3].ToString();
					mat4x1.Text = matrix[3,0].ToString();
					mat4x2.Text = matrix[3,1].ToString();
					mat4x3.Text = matrix[3,2].ToString();
					mat4x4.Text = matrix[3,3].ToString();
					break;
				case 16:
					this.blankMatrix();
					mat1x1.Text = matrix[0,0].ToString();
					mat1x2.Text = matrix[0,1].ToString();
					mat1x3.Text = matrix[0,2].ToString();
					mat1x4.Text = matrix[0,3].ToString();
					mat2x1.Text = matrix[1,0].ToString();
					mat2x2.Text = matrix[1,1].ToString();
					mat2x3.Text = matrix[1,2].ToString();
					mat2x4.Text = matrix[1,3].ToString();
					mat3x1.Text = matrix[2,0].ToString();
					mat3x2.Text = matrix[2,1].ToString();
					mat3x3.Text = matrix[2,2].ToString();
					mat3x4.Text = matrix[2,3].ToString();
					mat4x1.Text = matrix[3,0].ToString();
					mat4x2.Text = matrix[3,1].ToString();
					mat4x3.Text = matrix[3,2].ToString();
					mat4x4.Text = matrix[3,3].ToString();
					break;
			}
		}
		/// <summary>
		/// returns the currently configured matrix as an integer array
		/// </summary>
		/// <returns></returns>
		private int[,] getCurrentMatrix()
		{
			if (matrixSize.SelectedIndex == 0) // 4x4
			{
				int[,] retval = new int[4,4];
				if (!mat1x1.Text.Equals(""))
					retval[0,0] = Int32.Parse(mat1x1.Text);
				if (!mat1x2.Text.Equals(""))
					retval[0,1] = Int32.Parse(mat1x2.Text);
				if (!mat1x3.Text.Equals(""))
					retval[0,2] = Int32.Parse(mat1x3.Text);
				if (!mat1x4.Text.Equals(""))
					retval[0,3] = Int32.Parse(mat1x4.Text);
				if (!mat2x1.Text.Equals(""))
					retval[1,0] = Int32.Parse(mat2x1.Text);
				if (!mat2x2.Text.Equals(""))
					retval[1,1] = Int32.Parse(mat2x2.Text);
				if (!mat2x3.Text.Equals(""))
					retval[1,2] = Int32.Parse(mat2x3.Text);
				if (!mat2x4.Text.Equals(""))
					retval[1,3] = Int32.Parse(mat2x4.Text);
				if (!mat3x1.Text.Equals(""))
					retval[2,0] = Int32.Parse(mat3x1.Text);
				if (!mat3x2.Text.Equals(""))
					retval[2,1] = Int32.Parse(mat3x2.Text);
				if (!mat3x3.Text.Equals(""))
					retval[2,2] = Int32.Parse(mat3x3.Text);
				if (!mat3x4.Text.Equals(""))
					retval[2,3] = Int32.Parse(mat3x4.Text);
				if (!mat4x1.Text.Equals(""))
					retval[3,0] = Int32.Parse(mat4x1.Text);
				if (!mat4x2.Text.Equals(""))
					retval[3,1] = Int32.Parse(mat4x2.Text);
				if (!mat4x3.Text.Equals(""))
					retval[3,2] = Int32.Parse(mat4x3.Text);
				if (!mat4x4.Text.Equals(""))
					retval[3,3] = Int32.Parse(mat4x4.Text);
				return retval;
			}
			else // 8x8 matrix
			{
				int[,] retval = new int[8,8];
				if (!mat1x1.Text.Equals(""))
					retval[0,0] = Int32.Parse(mat1x1.Text);
				if (!mat1x2.Text.Equals(""))
					retval[0,1] = Int32.Parse(mat1x2.Text);
				if (!mat1x3.Text.Equals(""))
					retval[0,2] = Int32.Parse(mat1x3.Text);
				if (!mat1x4.Text.Equals(""))
					retval[0,3] = Int32.Parse(mat1x4.Text);
				if (!mat1x5.Text.Equals(""))
					retval[0,4] = Int32.Parse(mat1x5.Text);
				if (!mat1x6.Text.Equals(""))
					retval[0,5] = Int32.Parse(mat1x6.Text);
				if (!mat1x7.Text.Equals(""))
					retval[0,6] = Int32.Parse(mat1x7.Text);
				if (!mat1x8.Text.Equals(""))
					retval[0,7] = Int32.Parse(mat1x8.Text);
				if (!mat2x1.Text.Equals(""))
					retval[1,0] = Int32.Parse(mat2x1.Text);
				if (!mat2x2.Text.Equals(""))
					retval[1,1] = Int32.Parse(mat2x2.Text);
				if (!mat2x3.Text.Equals(""))
					retval[1,2] = Int32.Parse(mat2x3.Text);
				if (!mat2x4.Text.Equals(""))
					retval[1,3] = Int32.Parse(mat2x4.Text);
				if (!mat2x5.Text.Equals(""))
					retval[1,4] = Int32.Parse(mat2x5.Text);
				if (!mat2x6.Text.Equals(""))
					retval[1,5] = Int32.Parse(mat2x6.Text);
				if (!mat2x7.Text.Equals(""))
					retval[1,6] = Int32.Parse(mat2x7.Text);
				if (!mat2x8.Text.Equals(""))
					retval[1,7] = Int32.Parse(mat2x8.Text);
				if (!mat3x1.Text.Equals(""))
					retval[2,0] = Int32.Parse(mat3x1.Text);
				if (!mat3x2.Text.Equals(""))
					retval[2,1] = Int32.Parse(mat3x2.Text);
				if (!mat3x3.Text.Equals(""))
					retval[2,2] = Int32.Parse(mat3x3.Text);
				if (!mat3x4.Text.Equals(""))
					retval[2,3] = Int32.Parse(mat3x4.Text);
				if (!mat3x5.Text.Equals(""))
					retval[2,4] = Int32.Parse(mat3x5.Text);
				if (!mat3x6.Text.Equals(""))
					retval[2,5] = Int32.Parse(mat3x6.Text);
				if (!mat3x7.Text.Equals(""))
					retval[2,6] = Int32.Parse(mat3x7.Text);
				if (!mat3x8.Text.Equals(""))
					retval[2,7] = Int32.Parse(mat3x8.Text);
				if (!mat4x1.Text.Equals(""))
					retval[3,0] = Int32.Parse(mat4x1.Text);
				if (!mat4x2.Text.Equals(""))
					retval[3,1] = Int32.Parse(mat4x2.Text);
				if (!mat4x3.Text.Equals(""))
					retval[3,2] = Int32.Parse(mat4x3.Text);
				if (!mat4x4.Text.Equals(""))
					retval[3,3] = Int32.Parse(mat4x4.Text);
				if (!mat4x5.Text.Equals(""))
					retval[3,4] = Int32.Parse(mat4x5.Text);
				if (!mat4x6.Text.Equals(""))
					retval[3,5] = Int32.Parse(mat4x6.Text);
				if (!mat4x7.Text.Equals(""))
					retval[3,6] = Int32.Parse(mat4x7.Text);
				if (!mat4x8.Text.Equals(""))
					retval[3,7] = Int32.Parse(mat4x8.Text);
				if (!mat5x1.Text.Equals(""))
					retval[4,0] = Int32.Parse(mat5x1.Text);
				if (!mat5x2.Text.Equals(""))
					retval[4,1] = Int32.Parse(mat5x2.Text);
				if (!mat5x3.Text.Equals(""))
					retval[4,2] = Int32.Parse(mat5x3.Text);
				if (!mat5x4.Text.Equals(""))
					retval[4,3] = Int32.Parse(mat5x4.Text);
				if (!mat5x5.Text.Equals(""))
					retval[4,4] = Int32.Parse(mat5x5.Text);
				if (!mat5x6.Text.Equals(""))
					retval[4,5] = Int32.Parse(mat5x6.Text);
				if (!mat5x7.Text.Equals(""))
					retval[4,6] = Int32.Parse(mat5x7.Text);
				if (!mat5x8.Text.Equals(""))
					retval[4,7] = Int32.Parse(mat5x8.Text);
				if (!mat6x1.Text.Equals(""))
					retval[5,0] = Int32.Parse(mat6x1.Text);
				if (!mat6x2.Text.Equals(""))
					retval[5,1] = Int32.Parse(mat6x2.Text);
				if (!mat6x3.Text.Equals(""))
					retval[5,2] = Int32.Parse(mat6x3.Text);
				if (!mat6x4.Text.Equals(""))
					retval[5,3] = Int32.Parse(mat6x4.Text);
				if (!mat6x5.Text.Equals(""))
					retval[5,4] = Int32.Parse(mat6x5.Text);
				if (!mat6x6.Text.Equals(""))
					retval[5,5] = Int32.Parse(mat6x6.Text);
				if (!mat6x7.Text.Equals(""))
					retval[5,6] = Int32.Parse(mat6x7.Text);
				if (!mat6x8.Text.Equals(""))
					retval[5,7] = Int32.Parse(mat6x8.Text);
				if (!mat7x1.Text.Equals(""))
					retval[6,0] = Int32.Parse(mat7x1.Text);
				if (!mat7x3.Text.Equals(""))
					retval[6,1] = Int32.Parse(mat7x2.Text);
				if (!mat7x4.Text.Equals(""))
					retval[6,2] = Int32.Parse(mat7x3.Text);
				if (!mat7x4.Text.Equals(""))
					retval[6,3] = Int32.Parse(mat7x4.Text);
				if (!mat7x5.Text.Equals(""))
					retval[6,4] = Int32.Parse(mat7x5.Text);
				if (!mat7x6.Text.Equals(""))
					retval[6,5] = Int32.Parse(mat7x6.Text);
				if (!mat7x7.Text.Equals(""))
					retval[6,6] = Int32.Parse(mat7x7.Text);
				if (!mat7x8.Text.Equals(""))
					retval[6,7] = Int32.Parse(mat7x8.Text);
				if (!mat8x1.Text.Equals(""))
					retval[7,0] = Int32.Parse(mat8x1.Text);
				if (!mat8x2.Text.Equals(""))
					retval[7,1] = Int32.Parse(mat8x2.Text);
				if (!mat8x3.Text.Equals(""))
					retval[7,2] = Int32.Parse(mat8x3.Text);
				if (!mat8x4.Text.Equals(""))
					retval[7,3] = Int32.Parse(mat8x4.Text);
				if (!mat8x5.Text.Equals(""))
					retval[7,4] = Int32.Parse(mat8x5.Text);
				if (!mat8x6.Text.Equals(""))
					retval[7,5] = Int32.Parse(mat8x6.Text);
				if (!mat8x7.Text.Equals(""))
					retval[7,6] = Int32.Parse(mat8x7.Text);
				if (!mat8x8.Text.Equals(""))
					retval[7,7] = Int32.Parse(mat8x8.Text);
				return retval;
			}
		}

		#endregion
		#region dropdowns
		private void predefinedMatrix_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			doEvents = false; // block other event handlers until everything is set up
			switch (predefinedMatrix.SelectedIndex)
			{
				case 0: // none
					disableMatrix();
					matrixGroupbox.Enabled = false;
					operationsGroupbox.Enabled = false;
					break;
				case 1: // jvt
					disableMatrix();
					this.I8x8 = this.jvtI8x8;
					this.P8x8 = this.jvtP8x8;
					this.I4x4L = this.jvtI4x4;
					this.I4x4CU = this.jvtI4x4;
					this.I4x4CY = this.jvtI4x4;
					this.P4x4L = this.jvtP4x4;
					this.P4x4CU = this.jvtP4x4;
					this.P4x4CY = this.jvtP4x4;
					matrixGroupbox.Enabled = true;
					operationsGroupbox.Enabled = true;
					matrixSize.Enabled = true;
					matrixSize.SelectedIndex = 0;
					matrixType.Enabled = true;
					matrixType.SelectedIndex = 0;
					this.loadMatrix(I4x4L);
					this.i4x4Matrix();
					break;
				case 2: // flat
					disableMatrix();
					this.I8x8 = this.flat8x8;
					this.P8x8 = this.flat8x8;
					this.I4x4L = this.flat4x4;
					this.I4x4CU = this.flat4x4;
					this.I4x4CY = this.flat4x4;
					this.P4x4L = this.flat4x4;
					this.P4x4CU = this.flat4x4;
					this.P4x4CY = this.flat4x4;
					matrixGroupbox.Enabled = true;
					operationsGroupbox.Enabled = true;
					matrixSize.Enabled = true;
					matrixSize.SelectedIndex = 0;
					matrixType.Enabled = true;
					matrixType.SelectedIndex = 0;
					this.loadMatrix(I4x4L);
					this.i4x4Matrix();
					break;
				case 3: // custom
					matrixGroupbox.Enabled = true;
					operationsGroupbox.Enabled = true;
					matrixSize.Enabled = true;
					matrixSize.SelectedIndex = 0;
					matrixType.Enabled = true;
					matrixType.SelectedIndex = 0;
					enable4x4Matrix();
                    this.I8x8 = this.flat8x8;
                    this.P8x8 = this.flat8x8;
                    this.I4x4L = this.flat4x4;
                    this.I4x4CU = this.flat4x4;
                    this.I4x4CY = this.flat4x4;
                    this.P4x4L = this.flat4x4;
                    this.P4x4CU = this.flat4x4;
                    this.P4x4CY = this.flat4x4;
                    this.i4x4Matrix();
					break;
			}
			doEvents = true;
		}
		#endregion
		#region matrix size / type changing
		private void matrixSize_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (doEvents)
			{
				if (predefinedMatrix.SelectedIndex == 3) // save previous matrix before showing the new one
					this.saveMatrix(this.currentConfig);
				MatrixConfig conf = getCurrentConfig();
				if (matrixSize.SelectedIndex == 0) // 4x4
				{
					if (predefinedMatrix.SelectedIndex == 3) // custom 4x4
						enable4x4Matrix();
					i4x4Matrix();
					loadMatrix(this.I4x4L);
				}
				else if (matrixSize.SelectedIndex == 1) // 8x8
				{
					if (predefinedMatrix.SelectedIndex == 3)
						enable8x8Matrix();
					matrixType.Items.Clear();
					matrixType.Items.Add("I");
					matrixType.Items.Add("P");
					matrixType.SelectedIndex = 0;
					loadMatrix(this.I8x8);
				}
			}
			this.currentConfig = this.getCurrentConfig();
		}
		private void matrixType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (doEvents)
			{
				if (predefinedMatrix.SelectedIndex == 3) // save previous matrix before showing the new one
					this.saveMatrix(this.currentConfig);
				if (matrixSize.SelectedIndex == 0) // 4x4
				{
					switch (matrixType.SelectedIndex)
					{
						case 0: // I luma
							loadMatrix(this.I4x4L);
							break;
						case 1:
							loadMatrix(this.I4x4CU);
							break;
						case 2:
							loadMatrix(this.I4x4CY);
							break;
						case 3:
							loadMatrix(this.P4x4L);
							break;
						case 4:
							loadMatrix(this.P4x4CU);
							break;
						case 5:
							loadMatrix(this.P4x4CY);
							break;
					}
				}
				else // 8x8
				{
					switch (matrixType.SelectedIndex)
					{
						case 0:
							this.loadMatrix(this.I8x8);
							break;
						case 1:
							this.loadMatrix(this.P8x8);
							break;
					}
				}
				this.currentConfig = this.getCurrentConfig();
			}
		}
		#endregion
		#region enable/disable matrix
		/// <summary>
		/// enables all quantizer input fields
		/// </summary>
		private void enable8x8Matrix()
		{
			mat1x1.Enabled = true;
			mat1x2.Enabled = true;
			mat1x3.Enabled = true;
			mat1x4.Enabled = true;
			mat1x5.Enabled = true;
			mat1x6.Enabled = true;
			mat1x7.Enabled = true;
			mat1x8.Enabled = true;
			mat2x5.Enabled = true;
			mat2x6.Enabled = true;
			mat2x7.Enabled = true;
			mat2x8.Enabled = true;
			mat3x5.Enabled = true;
			mat3x6.Enabled = true;
			mat3x7.Enabled = true;
			mat3x8.Enabled = true;
			mat4x5.Enabled = true;
			mat4x6.Enabled = true;
			mat4x7.Enabled = true;
			mat4x8.Enabled = true;
			mat5x1.Enabled = true;
			mat5x2.Enabled = true;
			mat5x3.Enabled = true;
			mat5x4.Enabled = true;
			mat5x5.Enabled = true;
			mat5x6.Enabled = true;
			mat5x7.Enabled = true;
			mat5x8.Enabled = true;
			mat6x1.Enabled = true;
			mat6x2.Enabled = true;
			mat6x3.Enabled = true;
			mat6x4.Enabled = true;
			mat6x5.Enabled = true;
			mat6x6.Enabled = true;
			mat6x7.Enabled = true;
			mat6x8.Enabled = true;
			mat7x1.Enabled = true;
			mat7x2.Enabled = true;
			mat7x3.Enabled = true;
			mat7x4.Enabled = true;
			mat7x5.Enabled = true;
			mat7x6.Enabled = true;
			mat7x7.Enabled = true;
			mat7x8.Enabled = true;
			mat8x1.Enabled = true;
			mat8x2.Enabled = true;
			mat8x3.Enabled = true;
			mat8x4.Enabled = true;
			mat8x5.Enabled = true;
			mat8x6.Enabled = true;
			mat8x7.Enabled = true;
			mat8x8.Enabled = true;
		}
		/// <summary>
		/// disables but the upper left quadrant of the quantizer matrix
		/// </summary>
		private void enable4x4Matrix()
		{
			mat1x1.Enabled = true;
			mat1x2.Enabled = true;
			mat1x3.Enabled = true;
			mat1x4.Enabled = true;
			mat2x1.Enabled = true;
			mat2x2.Enabled = true;
			mat2x3.Enabled = true;
			mat2x4.Enabled = true;
			mat3x1.Enabled = true;
			mat3x2.Enabled = true;
			mat3x3.Enabled = true;
			mat3x4.Enabled = true;
			mat4x1.Enabled = true;
			mat4x2.Enabled = true;
			mat4x3.Enabled = true;
			mat4x4.Enabled = true;
			mat1x5.Enabled = false;
			mat1x6.Enabled = false;
			mat1x7.Enabled = false;
			mat1x8.Enabled = false;
			mat2x5.Enabled = false;
			mat2x6.Enabled = false;
			mat2x7.Enabled = false;
			mat2x8.Enabled = false;
			mat3x5.Enabled = false;
			mat3x6.Enabled = false;
			mat3x7.Enabled = false;
			mat3x8.Enabled = false;
			mat4x5.Enabled = false;
			mat4x6.Enabled = false;
			mat4x7.Enabled = false;
			mat4x8.Enabled = false;
			mat5x1.Enabled = false;
			mat5x2.Enabled = false;
			mat5x3.Enabled = false;
			mat5x4.Enabled = false;
			mat5x5.Enabled = false;
			mat5x6.Enabled = false;
			mat5x7.Enabled = false;
			mat5x8.Enabled = false;
			mat6x1.Enabled = false;
			mat6x2.Enabled = false;
			mat6x3.Enabled = false;
			mat6x4.Enabled = false;
			mat6x5.Enabled = false;
			mat6x6.Enabled = false;
			mat6x7.Enabled = false;
			mat6x8.Enabled = false;
			mat7x1.Enabled = false;
			mat7x2.Enabled = false;
			mat7x3.Enabled = false;
			mat7x4.Enabled = false;
			mat7x5.Enabled = false;
			mat7x6.Enabled = false;
			mat7x7.Enabled = false;
			mat7x8.Enabled = false;
			mat8x1.Enabled = false;
			mat8x2.Enabled = false;
			mat8x3.Enabled = false;
			mat8x4.Enabled = false;
			mat8x5.Enabled = false;
			mat8x6.Enabled = false;
			mat8x7.Enabled = false;
			mat8x8.Enabled = false;
		}

		/// <summary>
		/// disables all quantizer input fields
		/// </summary>
		private void disableMatrix()
		{
			this.enable4x4Matrix();
			mat1x1.Enabled = false;
			mat1x2.Enabled = false;
			mat1x3.Enabled = false;
			mat1x4.Enabled = false;
			mat2x1.Enabled = false;
			mat2x2.Enabled = false;
			mat2x3.Enabled = false;
			mat2x4.Enabled = false;
			mat3x1.Enabled = false;
			mat3x2.Enabled = false;
			mat3x3.Enabled = false;
			mat3x4.Enabled = false;
			mat4x1.Enabled = false;
			mat4x2.Enabled = false;
			mat4x3.Enabled = false;
			mat4x4.Enabled = false;
		}
		#endregion
		#region helper methods
		/// <summary>
		/// converts a twodimensional matrix into a string that can be used for saving a matrix to file
		/// </summary>
		/// <param name="matrix">the matrix to be converted</param>
		/// <returns>result of the conversion</returns>
		private string convertMatrixToString(int[,] matrix)
		{
			string retval = "";
			int wrapAfterElements = 4;
			if (matrix.Length == 64) // 8x8 matrix
				wrapAfterElements = 8;
			int count = 1;
			foreach (int val in matrix)
			{
				retval += val + ",";
				if (count == wrapAfterElements)
				{
					retval += "\r\n";
					count = 1;
				}
				else
					count++;
			}
			retval = retval.Substring(0, retval.Length - 3); // strip last newline
			return retval;
		}
		/// <summary>
		/// calles when the 4x4 matrix size has been selected
		/// </summary>
		private void i4x4Matrix()
		{
			matrixType.Items.Clear();
			matrixType.Items.Add("I Luma");
			matrixType.Items.Add("I Chroma U");
			matrixType.Items.Add("I Chroma Y");
			matrixType.Items.Add("P Luma");
			matrixType.Items.Add("P Chroma U");
			matrixType.Items.Add("P Chroma Y");
			matrixType.SelectedIndex = 0;
		}
		/// <summary>
		/// converts a comma separated 
		/// </summary>
		/// <param name="matrix"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		private int[,] convertStringToMatrix(string matrix, int size)
		{
			int[,] retval = new int[size, size];
			char[] splitChars = {','};
			matrix = matrix.Replace("\r", "");// removes newlines
			matrix = matrix.Replace("\n", "");
			matrix = matrix.Replace("\t", ""); // remove tabs
			string[] quantizers = matrix.Split(splitChars);
			int column = 0;
			int line = 0;
			foreach (string quantizer in quantizers)
			{
				int val = 16;
				try
				{
					val = Int32.Parse(quantizer);
				}
				catch (Exception e)
				{
                    MeGUI.core.util.LogItem _oLog = MainForm.Instance.Log.Info("Error");
                    _oLog.LogValue("convertStringToMatrix: " + matrix, e, MeGUI.core.util.ImageType.Error);
					MessageBox.Show("Invalid quantizer detected: " + quantizer + "\nUsing 16 instead", "Invalid quantizer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				retval[line, column] = val;
				if (column == size - 1)
				{
					line++;
					column = 0;
				}
				else
					column++;
			}
			return retval;
		}

		#endregion
		#region loading / saving matrices for switching between size / type
		/// <summary>
		/// gets all the data from the GUI to identify which type of matrix we're currently looking at
		/// </summary>
		/// <returns>the struct containing all the data</returns>
		private MatrixConfig getCurrentConfig()
		{
			MatrixConfig conf = new MatrixConfig();
			conf.size = matrixSize.SelectedIndex;
			conf.type = matrixType.SelectedIndex;
			return conf;
		}
		/// <summary>
		/// loads the matrix specified in the current configuration
		/// </summary>
		/// <param name="config">the configuration pointing out which matrix is to be loaded</param>
		private void loadMatrix(MatrixConfig config)
		{
			if (config.size == 0) // 4x4 matrix	
			{
				switch (config.type)
				{
					case 0: // I4x4L
						this.loadMatrix(this.I4x4L);
						break;
					case 1: // I4x4CU
						this.loadMatrix(this.I4x4CU);
						break;
					case 2: // I4x4CY
						this.loadMatrix(this.I4x4CY);
						break;
					case 3:
						this.loadMatrix(this.P4x4L);
						break;
					case 4:
						this.loadMatrix(this.P4x4CU);
						break;
					case 5:
						this.loadMatrix(this.P4x4CY);
						break;
				}
			}
			else if (config.size == 1) // 8x8 matrix
			{
				if (config.type == 0) // I8x8
					this.loadMatrix(this.I8x8);
				else if (config.type == 1) // P8x8
					this.loadMatrix(this.P8x8);
			}
		}
		/// <summary>
		/// saves the matrix currently configured in the gui into the appropriate internal matrix
		/// used when changing between a matrix size / subtype
		/// </summary>
		/// <param name="config">the current matrix configuration</param>
		private void saveMatrix(MatrixConfig config)
		{
			if (config.size == 0) // 4x4 matrix	
			{
				switch (config.type)
				{
					case 0: // I4x4L
						this.I4x4L = this.getCurrentMatrix();
						break;
					case 1: // I4x4CU
						this.I4x4CU = this.getCurrentMatrix();
						break;
					case 2: // I4x4CY
						this.I4x4CY = this.getCurrentMatrix();
						break;
					case 3:
						this.P4x4L = this.getCurrentMatrix();
						break;
					case 4:
						this.P4x4CU = this.getCurrentMatrix();
						break;
					case 5:
						this.P4x4CY = this.getCurrentMatrix();
						break;
					case -1: // first time, load I4x4L
						this.I4x4L = this.getCurrentMatrix();
						break;
				}
			}
			else if (config.size == 1) // 8x8 matrix
			{
				if (config.type == 0) // I8x8
					this.I8x8 = this.getCurrentMatrix();
				else if (config.type == 1) // P8x8
					this.P8x8 = this.getCurrentMatrix();
				else
					this.I8x8 = this.getCurrentMatrix();
			}
		}
		#endregion
		#region loading / saving matrix from / to file
		/// <summary>
		/// saves the currently configured matrix to a file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveMatrixButton_Click(object sender, System.EventArgs e)
		{
			if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				StreamWriter sw = null;
				try
				{
					sb = new StringBuilder();
					sb.Append("#" + Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + "\r\n\r\nINTRA4X4_LUMA =\r\n");
					sb.Append(convertMatrixToString(this.I4x4L));
					sb.Append("\r\n\r\nINTRA4X4_CHROMAU =\r\n");
					sb.Append(convertMatrixToString(this.I4x4CU));
					sb.Append("\r\n\r\nINTRA4X4_CHROMAV =\r\n");
					sb.Append(convertMatrixToString(this.I4x4CY));
					sb.Append("\r\n\r\nINTER4X4_LUMA =\r\n");
					sb.Append(convertMatrixToString(this.P4x4L));
					sb.Append("\r\n\r\nINTER4X4_CHROMAU =\r\n");
					sb.Append(convertMatrixToString(this.P4x4CU));
					sb.Append("\r\n\r\nINTER4X4_CHROMAV =\r\n");
					sb.Append(convertMatrixToString(this.P4x4CY));
					sb.Append("\r\n\r\nINTRA8X8_LUMA =\r\n");
					sb.Append(convertMatrixToString(this.I8x8));
					sb.Append("\r\n\r\nINTER8X8_LUMA =\r\n");
					sb.Append(convertMatrixToString(this.P8x8));
					
					sw = new StreamWriter(saveFileDialog.FileName, false);
					sw.Write(sb.ToString());
				}
				catch (Exception f)
				{
                    MeGUI.core.util.LogItem _oLog = MainForm.Instance.Log.Info("Error");
                    _oLog.LogValue("Error", f, MeGUI.core.util.ImageType.Error);
				}
				finally
				{
					if (sw != null)
					{
						try
						{
							sw.Close();
						}
						catch (Exception f)
						{
                            MeGUI.core.util.LogItem _oLog = MainForm.Instance.Log.Info("Error");
                            _oLog.LogValue("Error", f, MeGUI.core.util.ImageType.Error);
						}
					}
				}
			}
		}
		/// <summary>
		/// loads a matrix from file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void loadMatrixButton_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				StreamReader sr = null;
				string matrix = "", line = null;
				try
				{
					sr = new StreamReader(openFileDialog.FileName);
					while ((line = sr.ReadLine()) != null)
					{
						if (line.IndexOf("4X4") != -1) // 4x3 matrix
						{
							matrix = sr.ReadLine();
							matrix += sr.ReadLine();
							matrix += sr.ReadLine();
							matrix += sr.ReadLine();
							int[,] matrixRead = convertStringToMatrix(matrix, 4);
							if (line.IndexOf("INTRA4X4_LUMA") != -1) // I4x4L matrix
								this.I4x4L = matrixRead;
							else if (line.IndexOf("INTRA4X4_CHROMAU") != -1)
								this.I4x4CU = matrixRead;
							else if (line.IndexOf("INTRA4X4_CHROMAV") != -1)
								this.I4x4CY = matrixRead;
							else if (line.IndexOf("INTER4X4_LUMA") != -1)
								this.P4x4L = matrixRead;
							else if (line.IndexOf("INTER4X4_CHROMAU") != -1)
								this.P4x4CU = matrixRead;
							else if (line.IndexOf("INTER4X4_CHROMAV") != -1)
								this.P4x4CY = matrixRead;
						}
						if (line.IndexOf("8X8") != -1) // 8x8 matrix
						{
							matrix = sr.ReadLine();
							matrix += sr.ReadLine();
							matrix += sr.ReadLine();
							matrix += sr.ReadLine();
							matrix += sr.ReadLine();
							matrix += sr.ReadLine();
							matrix += sr.ReadLine();
							matrix += sr.ReadLine();
							int[,] matrixRead = convertStringToMatrix(matrix, 8);
							if (line.IndexOf("INTRA8X8_LUMA") != -1)
								this.I8x8 = matrixRead;
							else if (line.IndexOf("INTER8X8_LUMA") != -1)
								this.P8x8 = matrixRead;
						}
					}
					this.loadMatrix(getCurrentConfig());
					this.predefinedMatrix.SelectedIndex = 3;

				}
				catch (Exception f)
				{
                    MeGUI.core.util.LogItem _oLog = MainForm.Instance.Log.Info("Error");
                    _oLog.LogValue("Error", f, MeGUI.core.util.ImageType.Error);
				}
				finally
				{
					if (sr != null)
					{
						try
						{
							sr.Close();
						}
						catch (Exception f)
						{
                            MeGUI.core.util.LogItem _oLog = MainForm.Instance.Log.Info("Error");
                            _oLog.LogValue("Error", f, MeGUI.core.util.ImageType.Error);
						}
					}
				}
			}
		}
		#endregion
	}
	#region helper structs
	public struct MatrixConfig
	{
		public int size;
		public int type;
		public bool onePerSize;
	}
	#endregion

    public class CQMEditorTool : MeGUI.core.plugins.interfaces.ITool
    {
        #region ITool Members

        public string Name
        {
            get { return "AVC Quant Matrix Editor"; }
        }

        public void Run(MainForm info)
        {
            new QuantizerMatrixDialog().Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlQ }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "cqmEditor"; }
        }

        #endregion
    }
}
