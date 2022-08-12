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
using System.Drawing;
using System.Windows.Forms;

namespace MeGUI.core.gui.MessageBoxExLib
{
    /// <summary>
    /// An extended MessageBox with lot of customizing capabilities.
    /// 
    /// This library provides an advanced framework for message boxes. 
    /// Some salient features are - 
    /// 1. Ability to associate text with buttons, thus you can specify a
    /// text for each button that tells the user what will happen when the button
    /// is pressed.This is currently shown as a simple tooltip.
    /// 2. Ability to specify an option that allows the user to always use the same response
    /// for the specified messagebox.
    /// 3. Ability to save/load user preferences for various messageboxes. (Not implemented in this release)
    /// 4. Ability to customize buttons, icon, sounds.
    /// 5. Ability to specify a Timeout for the message box.
    /// 6. Standard button texts are localized based on current culture.
    /// </summary>

    #region Enum
    /// <summary>
    /// Standard MessageBoxEx buttons
    /// </summary>
    public enum MessageBoxExButtons
    {
        Ok = 0,
        Cancel = 1,
        Yes = 2,
        No = 4,
        Abort = 8,
        Retry = 16,
        Ignore = 32
    }

    /// <summary>
    /// Standard MessageBoxEx icons
    /// </summary>
    public enum MessageBoxExIcon
    {
        None,
        Asterisk,
        Error,
        Exclamation,
        Hand,
        Information,
        Question,
        Stop,
        Warning
    }

    /// <summary>
    /// Standard MessageBoxEx results
    /// </summary>
    public struct MessageBoxExResult
    {
        public const string Ok = "Ok";
        public const string Cancel = "Cancel";
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Abort = "Abort";
        public const string Retry = "Retry";
        public const string Ignore = "Ignore";
        public const string Timeout = "Timeout";
    }

    /// <summary>
    /// Enumerates the kind of results that can be returned when a
    /// message box times out
    /// </summary>
    public enum TimeoutResult
    {
        /// <summary>
        /// On timeout the value associated with the default button is set as the result.
        /// This is the default action on timeout.
        /// </summary>
        Default,

        /// <summary>
        /// On timeout the value associated with the cancel button is set as the result. If
        /// the messagebox does not have a cancel button then the value associated with 
        /// the default button is set as the result.
        /// </summary>
        Cancel,

        /// <summary>
        /// On timeout MessageBoxExResult.Timeout is set as the result.
        /// </summary>
        Timeout
    }
    #endregion

    public class MessageBoxEx
	{
		#region Fields
		private MessageBoxExForm _msgBox = new MessageBoxExForm();
		#endregion

		#region Properties
		/// <summary>
		/// Sets the caption of the message box
		/// </summary>
		public string Caption
		{
			set{_msgBox.Caption = value;}
		}

        /// <summary>
		/// Sets the text of the message box
		/// </summary>
		public string Text
		{
			set{_msgBox.Message = value;}
		}

		/// <summary>
		/// Sets the icon to show in the message box
		/// </summary>
		public Icon CustomIcon
		{
			set{_msgBox.CustomIcon = value;}
		}

		/// <summary>
		/// Sets the icon to show in the message box
		/// </summary>
		public MessageBoxIcon Icon
		{
			set{ _msgBox.StandardIcon = value;}
		}
		
		/// <summary>
		/// Sets the font for the text of the message box
		/// </summary>
		public Font Font
		{
			set{_msgBox.Font = value;}
		}

		/// <summary>
		/// Sets or Gets the ability of the  user to save his/her response
		/// </summary>
		public bool AllowSaveResponse
		{
			get{ return _msgBox.AllowSaveResponse; }
			set{ _msgBox.AllowSaveResponse = value; }
		}

		/// <summary>
		/// Sets the text to show to the user when saving his/her response
		/// </summary>
		public string SaveResponseText
		{
			set{_msgBox.SaveResponseText = value; }
		}

		/// <summary>
		/// Sets or Gets wether an alert sound is played while showing the message box.
		/// The sound played depends on the the Icon selected for the message box
		/// </summary>
		public bool PlayAlertSound
		{
			get{ return _msgBox.PlayAlertSound; }
			set{ _msgBox.PlayAlertSound = value; }
		}

        /// <summary>
        /// Sets or Gets the time in milliseconds for which the message box is displayed.
        /// </summary>
        public int Timeout
        {
            get{ return _msgBox.Timeout; }
            set{ _msgBox.Timeout = value; }
        }

        /// <summary>
        /// Controls the result that will be returned when the message box times out.
        /// </summary>
        public TimeoutResult TimeoutResult
        {
            get{ return _msgBox.TimeoutResult; }
            set{ _msgBox.TimeoutResult = value; }
        }

        /// <summary>
        /// Gets or sets the value of the save response checkbox
        /// </summary>
        public bool SaveResponseChecked
        {
            get { return _msgBox.SaveResponse; }
            set { _msgBox.SaveResponse = value; }
        }
		#endregion

		#region Methods
		/// <summary>
		/// Shows the message box
		/// </summary>
		/// <returns></returns>
		public string Show()
		{
			return Show(null);
		}

		/// <summary>
		/// Shows the messsage box with the specified owner
		/// </summary>
		/// <param name="owner"></param>
		/// <returns></returns>
		public string Show(IWin32Window owner)
		{


			if(owner == null)
			{
				_msgBox.ShowDialog();
			}
			else
			{
				_msgBox.ShowDialog(owner);
			}
            Dispose();

			return _msgBox.Result;
		}

		/// <summary>
		/// Add a custom button to the message box
		/// </summary>
		/// <param name="button">The button to add</param>
		public void AddButton(MessageBoxExButton button)
		{
			if(button == null)
				throw new ArgumentNullException("button","A null button cannot be added");

			_msgBox.Buttons.Add(button);

			if(button.IsCancelButton)
			{
				_msgBox.CustomCancelButton = button;
			}
		}

		/// <summary>
		/// Add a custom button to the message box
		/// </summary>
		/// <param name="text">The text of the button</param>
		/// <param name="val">The return value in case this button is clicked</param>
		public void AddButton(string text, string val)
		{
			if(text == null)
				throw new ArgumentNullException("text","Text of a button cannot be null");

			if(val == null)
				throw new ArgumentNullException("val","Value of a button cannot be null");

			MessageBoxExButton button = new MessageBoxExButton();
			button.Text = text;
			button.Value = val;

			AddButton(button);
		}
        
		/// <summary>
		/// Add a standard button to the message box
		/// </summary>
		/// <param name="buttons">The standard button to add</param>
		public void AddButton(MessageBoxExButtons button)
		{
            string buttonText = MessageBoxExManager.GetLocalizedString(button.ToString());
            if(buttonText == null)
            {
                buttonText = button.ToString();
            }

            string buttonVal = button.ToString();

            MessageBoxExButton btn = new MessageBoxExButton();
            btn.Text = buttonText;
            btn.Value = buttonVal;

            if(button == MessageBoxExButtons.Cancel)
            {
                btn.IsCancelButton = true;
            }

			AddButton(btn);
		}

		/// <summary>
		/// Add standard buttons to the message box.
		/// </summary>
		/// <param name="buttons">The standard buttons to add</param>
		public void AddButtons(MessageBoxButtons buttons)
		{
			switch(buttons)
			{
				case MessageBoxButtons.OK:
					AddButton(MessageBoxExButtons.Ok);
					break;

				case MessageBoxButtons.AbortRetryIgnore:
					AddButton(MessageBoxExButtons.Abort);
					AddButton(MessageBoxExButtons.Retry);
					AddButton(MessageBoxExButtons.Ignore);
					break;

				case MessageBoxButtons.OKCancel:
					AddButton(MessageBoxExButtons.Ok);
					AddButton(MessageBoxExButtons.Cancel);
					break;

				case MessageBoxButtons.RetryCancel:
					AddButton(MessageBoxExButtons.Retry);
					AddButton(MessageBoxExButtons.Cancel);
					break;

				case MessageBoxButtons.YesNo:
					AddButton(MessageBoxExButtons.Yes);
					AddButton(MessageBoxExButtons.No);
					break;

				case MessageBoxButtons.YesNoCancel:
					AddButton(MessageBoxExButtons.Yes);
					AddButton(MessageBoxExButtons.No);
					AddButton(MessageBoxExButtons.Cancel);
					break;
			}
		}
		#endregion

		#region Ctor
		/// <summary>
		/// Ctor is internal because this can only be created by MBManager
		/// </summary>
		public MessageBoxEx()
		{
		}

		/// <summary>
		/// Called by the manager when it is disposed
		/// </summary>
		internal void Dispose()
		{
			if(_msgBox != null)
			{
				_msgBox.Dispose();
			}
		}
		#endregion
	}

    /// <summary>
    /// Internal DataStructure used to represent a button
    /// </summary>
    public class MessageBoxExButton
    {
        private string _text = null;
        /// <summary>
        /// Gets or Sets the text of the button
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private string _value = null;
        /// <summary>
        /// Gets or Sets the return value when this button is clicked
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _helpText = null;
        /// <summary>
        /// Gets or Sets the tooltip that is displayed for this button
        /// </summary>
        public string HelpText
        {
            get { return _helpText; }
            set { _helpText = value; }
        }

        private bool _isCancelButton = false;
        /// <summary>
        /// Gets or Sets wether this button is a cancel button. i.e. the button
        /// that will be assumed to have been clicked if the user closes the message box
        /// without pressing any button.
        /// </summary>
        public bool IsCancelButton
        {
            get { return _isCancelButton; }
            set { _isCancelButton = value; }
        }
    }

    /// <summary>
    /// Manages a collection of MessageBoxes. Basically manages the
    /// saved response handling for messageBoxes.
    /// </summary>
    public class MessageBoxExManager
    {
        #region Fields
        private static System.Collections.Hashtable _standardButtonsText = new System.Collections.Hashtable();
        #endregion

        #region Static ctor
        static MessageBoxExManager()
        {
            _standardButtonsText[MessageBoxExButtons.Ok.ToString()] = "Ok";
            _standardButtonsText[MessageBoxExButtons.Cancel.ToString()] = "Cancel";
            _standardButtonsText[MessageBoxExButtons.Yes.ToString()] = "Yes";
            _standardButtonsText[MessageBoxExButtons.No.ToString()] = "No";
            _standardButtonsText[MessageBoxExButtons.Abort.ToString()] = "Abort";
            _standardButtonsText[MessageBoxExButtons.Retry.ToString()] = "Retry";
            _standardButtonsText[MessageBoxExButtons.Ignore.ToString()] = "Ignore";
        }
        #endregion

        #region internal methods
        /// <summary>
        /// Returns the localized string for standard button texts like,
        /// "Ok", "Cancel" etc.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string GetLocalizedString(string key)
        {
            if (_standardButtonsText.ContainsKey(key))
            {
                return (string)_standardButtonsText[key];
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
