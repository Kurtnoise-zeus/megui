// ****************************************************************************
// 
// Copyright (C) 2005-2026 Doom9 & al
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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    /// <summary>
    /// Gives a small dialog the look of the Windows 11 settings: sections of rounded cards (title on the
    /// left, the existing controls on the right) and a footer with the buttons. The controls of the dialog
    /// are only moved, so their names, events and the properties of the form are unchanged.
    /// Usage (end of the constructor of the form): create it, add sections / rows, then call Finish.
    /// </summary>
    public sealed class ModernDialog
    {
        private readonly Form form;
        private readonly float scale;
        private readonly Font sectionFont;
        private readonly SettingsPage page;
        private readonly List<Button> buttons = new List<Button>();
        private Button[] footerButtons;
        private Control help;
        private SettingsSurface footer;

        public ModernDialog(Form form)
        {
            this.form = form;
            scale = MainForm.Instance != null ? MainForm.Instance.Settings.DPIRescale(1000) / 1000f : 1f;
            // the sizes are computed with the scale: no automatic scaling on top of it
            form.AutoScaleMode = AutoScaleMode.None;
            form.Font = new Font("Segoe UI", 10F);
            sectionFont = new Font("Segoe UI Semibold", 10.5F);
            page = new SettingsPage(scale);
            page.Padding = new Padding(S(24), S(8), S(24), S(16));
        }

        public int S(int value)
        {
            return (int)Math.Round(value * scale);
        }

        /// <summary>"maximum parallel jobs:" becomes "Maximum parallel jobs"</summary>
        public static string Cap(string text)
        {
            text = text.Replace("  ", " ").Trim().TrimEnd(':').Trim();
            return text.Length == 0 ? text : char.ToUpper(text[0]) + text.Substring(1);
        }

        #region content
        public Label Section(string title)
        {
            return page.AddSection(title, sectionFont);
        }

        public SettingsCard Card()
        {
            return page.AddCard();
        }

        /// <summary>a card with a title and the controls of the setting on the right (listed from left to right)</summary>
        public void Row(string title, string description, params Control[] right)
        {
            page.AddCard().AddRow(title, description, right);
        }

        /// <summary>a card with a toggle switch (its Text is the title); other controls can go on the left of the switch</summary>
        public void Toggle(ToggleSwitch toggle, string description, params Control[] leftOfSwitch)
        {
            toggle.AutoSize = false;
            toggle.Size = new Size(S(96), S(26));
            page.AddCard().AddToggle(toggle, description, leftOfSwitch);
        }

        /// <summary>the radio buttons of a group (one per row, the controls of a radio button follow it); all have the same width so that the controls are aligned</summary>
        public SettingsCard Radios(int radioWidth, params KeyValuePair<RadioButton, Control[]>[] rows)
        {
            SettingsCard card = page.AddCard();
            foreach (KeyValuePair<RadioButton, Control[]> row in rows)
            {
                row.Key.AutoSize = false;
                row.Key.Size = new Size(S(radioWidth), S(26));
                card.AddLeft(row.Key, row.Value);
            }
            return card;
        }

        /// <summary>a toggle switch as a row of an existing card</summary>
        public void ToggleIn(SettingsCard card, ToggleSwitch toggle, string description)
        {
            toggle.AutoSize = false;
            toggle.Size = new Size(S(96), S(26));
            card.AddToggle(toggle, description);
        }

        /// <summary>a radio button and the controls which follow it (for <see cref="Radios"/>)</summary>
        public static KeyValuePair<RadioButton, Control[]> R(RadioButton radio, params Control[] after)
        {
            return new KeyValuePair<RadioButton, Control[]>(radio, after);
        }

        /// <summary>a card with a text only (the message of a dialog, set by the code of the form), the text wraps</summary>
        public SettingsCard Message(Label label, int width, int height)
        {
            label.AutoSize = false;
            label.Size = new Size(S(width), S(height));
            SettingsCard card = page.AddCard();
            card.AddWide(null, null, label);
            return card;
        }

        /// <summary>a card with one wide control (a list, a progress bar...)</summary>
        public void Block(Control c, int width, int height, SettingsCard card)
        {
            c.Size = new Size(S(width), S(height));
            if (card == null)
                card = page.AddCard();
            card.AddWide(null, null, c);
        }

        public Control Wide(Control c, int width)
        {
            c.Width = S(width);
            return c;
        }

        public Button Btn(Button b)
        {
            b.AutoSize = false;
            b.Height = S(32);
            b.Width = b.Text.Length <= 2
                ? S(34)
                : Math.Max(S(96), TextRenderer.MeasureText(b.Text, form.Font).Width + S(36));
            if (!buttons.Contains(b))
                buttons.Add(b);
            return b;
        }

        public Control Bar(FileBar bar, int width)
        {
            bar.Size = new Size(S(width), S(30));
            return bar;
        }
        #endregion

        /// <summary>
        /// Puts the content and the footer in the form. footerButtons: from left to right, the first one is the
        /// main one (accent colour); help (can be null) is placed at the left of the footer.
        /// </summary>
        public void Finish(Control help, int clientWidth, params Button[] footerButtons)
        {
            this.help = help;
            this.footerButtons = footerButtons;
            form.SuspendLayout();

            SettingsSurface content = new SettingsSurface(SettingsSurfaceKind.Window);
            content.Dock = DockStyle.Fill;
            page.Dock = DockStyle.Fill;
            content.Controls.Add(page);

            footer = new SettingsSurface(SettingsSurfaceKind.Footer);
            footer.Dock = DockStyle.Bottom;
            footer.Height = S(64);
            if (help != null)
            {
                help.Dock = DockStyle.None;
                help.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                footer.Controls.Add(help);
            }
            foreach (Button b in footerButtons)
            {
                b.Dock = DockStyle.None;
                b.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                b.AutoSize = false;
                b.Size = new Size(S(96), S(34));
                b.Width = Math.Max(S(96), TextRenderer.MeasureText(b.Text, form.Font).Width + S(36));
                footer.Controls.Add(b);
            }
            footer.Resize += delegate { LayoutFooter(); };
            LayoutFooter();

            // the docked control added last is laid out first
            form.Controls.Clear();
            form.Controls.Add(content);
            form.Controls.Add(footer);

            // the height of the content, limited to the screen (the page scrolls)
            int height = page.Padding.Vertical;
            foreach (Control c in page.Controls)
                height += c.Height + c.Margin.Vertical;
            Rectangle area = Screen.PrimaryScreen.WorkingArea;
            form.ClientSize = new Size(Math.Min(S(clientWidth), area.Width), Math.Min(height + footer.Height, area.Height - S(100)));

            ApplyTheme();
            ThemeManager.ThemeChanged += OnThemeChanged;
            // the theme engine colours the form when it is shown: restyle after it
            form.Shown += delegate { form.BeginInvoke(new MethodInvoker(ApplyTheme)); };
            form.Disposed += delegate { ThemeManager.ThemeChanged -= OnThemeChanged; };
            form.ResumeLayout(true);
        }

        private void LayoutFooter()
        {
            int top = (footer.Height - S(34)) / 2;
            int right = footer.Width - S(24);
            for (int i = footerButtons.Length - 1; i >= 0; i--)
            {
                Button b = footerButtons[i];
                b.Location = new Point(right - b.Width, top);
                right = b.Left - S(10);
            }
            if (help != null)
                help.Location = new Point(S(24), (footer.Height - help.Height) / 2);
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            // may be raised from another thread (system colour change)
            if (form.IsHandleCreated && !form.IsDisposed)
                form.BeginInvoke(new MethodInvoker(ApplyTheme));
        }

        private void ApplyTheme()
        {
            foreach (Button b in buttons)
                StyleButton(b, false);
            for (int i = 0; i < footerButtons.Length; i++)
                StyleButton(footerButtons[i], i == 0);
            StyleLabels(page);
            form.Invalidate(true);
        }

        /// <summary>the labels inside the cards take the colour of the card</summary>
        private static void StyleLabels(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                Label l = c as Label;
                if (l != null && c.Parent is SettingsCard)
                {
                    l.BackColor = Color.Transparent;
                    l.ForeColor = ThemeManager.Palette.Text;
                }
                StyleLabels(c);
            }
        }

        /// <summary>flat button with the colours of the palette (accent colour for the main button)</summary>
        public static void StyleButton(Button b, bool primary)
        {
            ThemePalette p = ThemeManager.Palette;
            bool dark = ThemeManager.IsDark;
            b.UseVisualStyleBackColor = false;
            b.FlatStyle = FlatStyle.Flat;
            if (primary)
            {
                b.BackColor = p.Accent;
                b.ForeColor = p.OnAccent;
                b.FlatAppearance.BorderColor = p.Accent;
                b.FlatAppearance.MouseOverBackColor = ThemeManager.Blend(p.Accent, dark ? Color.White : Color.Black, 0.12);
                b.FlatAppearance.MouseDownBackColor = ThemeManager.Blend(p.Accent, dark ? Color.Black : Color.White, 0.15);
            }
            else
            {
                b.BackColor = ThemeManager.Blend(p.Surface, p.Text, dark ? 0.08 : 0.02);
                b.ForeColor = p.Text;
                b.FlatAppearance.BorderColor = ThemeManager.Blend(p.Surface, p.Text, 0.18);
                b.FlatAppearance.MouseOverBackColor = ThemeManager.Blend(p.Surface, p.Text, 0.14);
                b.FlatAppearance.MouseDownBackColor = ThemeManager.Blend(p.Surface, p.Text, 0.10);
            }
        }
    }
}
