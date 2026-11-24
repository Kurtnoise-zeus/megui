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
        private readonly Control host;
        private readonly float scale;
        private readonly Font sectionFont;
        private SettingsPage page;
        private readonly List<SettingsPage> pages = new List<SettingsPage>();
        private readonly List<Button> buttons = new List<Button>();
        private Button[] footerButtons;
        private Control help;
        private SettingsSurface footer;

        /// <summary>
        /// For a user control (the panel of a window hosted by another form): the sections and cards are built
        /// like in a form, then AttachTheme is called; the caller puts the pages in its own containers.
        /// </summary>
        public ModernDialog(UserControl panel)
        {
            host = panel;
            scale = MainForm.Instance != null ? MainForm.Instance.Settings.DPIRescale(1000) / 1000f : 1f;
            panel.AutoScaleMode = AutoScaleMode.None;
            panel.Font = new Font("Segoe UI", 10F);
            sectionFont = new Font("Segoe UI Semibold", 10.5F);
            page = NewPage();
        }

        /// <summary>end of the construction of a panel: end strips, colours, theme changes</summary>
        public void AttachTheme()
        {
            // the scrolling area of a flow panel ignores the bottom margin of its last control
            foreach (SettingsPage p in pages)
            {
                Panel end = new Panel();
                end.Height = S(24);
                end.Margin = new Padding(0);
                p.Controls.Add(end);
            }
            footerButtons = new Button[0];
            ApplyTheme();
            ThemeManager.ThemeChanged += OnThemeChanged;
            host.HandleCreated += delegate { host.BeginInvoke(new MethodInvoker(ApplyTheme)); };
            host.Disposed += delegate { ThemeManager.ThemeChanged -= OnThemeChanged; };
        }

        public ModernDialog(Form form)
        {
            this.form = form;
            host = form;
            scale = MainForm.Instance != null ? MainForm.Instance.Settings.DPIRescale(1000) / 1000f : 1f;
            // the sizes are computed with the scale: no automatic scaling on top of it
            form.AutoScaleMode = AutoScaleMode.None;
            form.Font = new Font("Segoe UI", 10F);
            sectionFont = new Font("Segoe UI Semibold", 10.5F);
            page = NewPage();
        }

        private SettingsPage NewPage()
        {
            SettingsPage p = new SettingsPage(scale);
            p.Padding = new Padding(S(24), S(8), S(24), S(16));
            pages.Add(p);
            return p;
        }

        /// <summary>the page which receives the sections and the cards</summary>
        public SettingsPage CurrentPage
        {
            get { return page; }
        }

        /// <summary>starts another page (for a window with tabs): the following sections and cards go to it</summary>
        public SettingsPage BeginPage()
        {
            page = NewPage();
            return page;
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
            label.Dock = DockStyle.None;
            label.Size = new Size(S(width), S(height));
            SettingsCard card = page.AddCard();
            card.AddWide(null, null, label);
            return card;
        }

        /// <summary>a card with one wide control (a list, a progress bar...)</summary>
        public void Block(Control c, int width, int height, SettingsCard card)
        {
            // a control which was docked in its old container is resized by it: undock first
            c.Dock = DockStyle.None;
            c.Size = new Size(S(width), S(height));
            if (card == null)
                card = page.AddCard();
            card.AddWide(null, null, c);
        }

        public Control Wide(Control c, int width)
        {
            c.Dock = DockStyle.None;
            c.Width = S(width);
            return c;
        }

        public Button Btn(Button b)
        {
            b.Dock = DockStyle.None;
            b.AutoSize = false;
            b.Height = S(32);
            b.Width = b.Text.Length <= 2
                ? S(34)
                : Math.Max(S(96), TextRenderer.MeasureText(b.Text, host.Font).Width + S(36));
            if (!buttons.Contains(b))
                buttons.Add(b);
            return b;
        }

        public Control Bar(FileBar bar, int width)
        {
            bar.Dock = DockStyle.None;
            bar.Size = new Size(S(width), S(30));
            return bar;
        }
        #endregion

        /// <summary>
        /// Moves the controls of a group box into a card: the rows are rebuilt from the positions of the controls
        /// (a label gives the title of its row, a toggle / radio button leads its row, a track bar, a long text or a
        /// multi-line text box has a row of its own). The labels are kept as the titles of their rows, so the code
        /// which changes their text / enabled / visible state keeps working.
        /// </summary>
        public void FillCard(SettingsCard card, GroupBox box)
        {
            List<Control> items = new List<Control>();
            foreach (Control c in box.Controls)
                items.Add(c);
            box.Controls.Clear();
            items.Sort(delegate (Control a, Control b)
            {
                return a.Top != b.Top ? a.Top.CompareTo(b.Top) : a.Left.CompareTo(b.Left);
            });

            List<List<Control>> rows = new List<List<Control>>();
            foreach (Control c in items)
            {
                List<Control> last = rows.Count > 0 ? rows[rows.Count - 1] : null;
                if (last != null && Math.Abs(c.Top - last[0].Top) <= 8 && !IsBlock(c) && !IsBlock(last[0]))
                    last.Add(c);
                else
                {
                    last = new List<Control>();
                    last.Add(c);
                    rows.Add(last);
                }
            }

            // a multi-line text box a little higher than its label: the label first
            for (int i = 0; i + 1 < rows.Count; i++)
            {
                TextBox tb = rows[i].Count == 1 ? rows[i][0] as TextBox : null;
                if (tb != null && tb.Multiline && rows[i + 1].Count == 1 && rows[i + 1][0] is Label
                    && Math.Abs(rows[i + 1][0].Top - tb.Top) <= 8)
                {
                    List<Control> swap = rows[i];
                    rows[i] = rows[i + 1];
                    rows[i + 1] = swap;
                    i++;
                }
            }

            foreach (List<Control> row in rows)
            {
                row.Sort(delegate (Control a, Control b) { return a.Left.CompareTo(b.Left); });
                if (row.Count == 1 && IsBlock(row[0]))
                {
                    AddBlockRow(card, row[0]);
                    continue;
                }

                Label title = null;
                List<Control> others = new List<Control>();
                foreach (Control c in row)
                {
                    Label l = c as Label;
                    if (l != null && title == null)
                        title = l;
                    else
                        others.Add(Fit(c));
                }

                if (title != null)
                    card.AddLabelRow(title, null, others.ToArray());
                else if (others[0] is ToggleSwitch)
                {
                    ToggleSwitch toggle = (ToggleSwitch)others[0];
                    others.RemoveAt(0);
                    card.AddToggle(toggle, null, others.ToArray());
                }
                else if (others[0] is RadioButton)
                {
                    RadioButton radio = (RadioButton)others[0];
                    others.RemoveAt(0);
                    radio.AutoSize = false;
                    radio.Size = new Size(TextRenderer.MeasureText(radio.Text, host.Font).Width + S(40), S(26));
                    card.AddLeft(radio, others.ToArray());
                }
                else
                    card.AddRow("", null, others.ToArray());
            }

            if (footerButtons != null)
                ApplyTheme();
        }

        private static bool IsBlock(Control c)
        {
            TextBox t = c as TextBox;
            Label l = c as Label;
            return c is TrackBar || (t != null && t.Multiline) || (l != null && l.Text.Length > 40);
        }

        private void AddBlockRow(SettingsCard card, Control c)
        {
            Label l = c as Label;
            if (l != null)
            {
                // a text of the dialog: it wraps, its height follows the text
                l.AutoSize = false;
                int width = S(560);
                int height = TextRenderer.MeasureText(l.Text, host.Font, new Size(width, 0), TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix).Height;
                l.Size = new Size(width, height + S(6));
            }
            else if (c is TrackBar)
                c.Height = S(45);
            else
                c.Height = Math.Max(S(70), c.Height);
            card.AddWide(null, null, c);
        }

        /// <summary>size of a control placed on the right of a row</summary>
        private Control Fit(Control c)
        {
            if (c is NumericUpDown)
                c.Width = S(100);
            else if (c is ComboBox)
                c.Width = Math.Max(S(130), Math.Min(S(260), S(c.Width)));
            else if (c is ToggleSwitch)
            {
                ((ToggleSwitch)c).AutoSize = false;
                c.Size = new Size(S(96), S(26));
            }
            else if (c is TextBox)
                c.Width = Math.Max(S(150), Math.Min(S(360), S(c.Width)));
            else if (c is Button)
                Btn((Button)c);
            return c;
        }

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
                b.Width = Math.Max(S(96), TextRenderer.MeasureText(b.Text, host.Font).Width + S(36));
                footer.Controls.Add(b);
            }
            footer.Resize += delegate { LayoutFooter(); };
            LayoutFooter();

            // the docked control added last is laid out first
            form.Controls.Clear();
            form.Controls.Add(content);
            form.Controls.Add(footer);

            // the scrolling area of a flow panel ignores the bottom margin of its last control:
            // an empty strip at the end makes the last card fully reachable
            Panel end = new Panel();
            end.Height = S(24);
            end.Margin = new Padding(0);
            page.Controls.Add(end);

            // the height of the content, limited to the screen (the page scrolls)
            int height = page.Padding.Vertical;
            foreach (Control c in page.Controls)
                height += c.Height + c.Margin.Vertical;
            Rectangle area = Screen.PrimaryScreen.WorkingArea;
            // resizable borders: the cards follow the width, the page scrolls when it is too short
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.SizeGripStyle = SizeGripStyle.Show;
            form.ClientSize = new Size(Math.Min(S(clientWidth), area.Width), Math.Min(height + footer.Height, area.Height - S(100)));
            // not narrower than the initial width (the controls of the cards have a fixed width), not lower than a few cards
            form.MinimumSize = new Size(form.Width, Math.Min(form.Height, S(380)));

            ApplyTheme();
            ThemeManager.ThemeChanged += OnThemeChanged;
            // the theme engine colours the form when it is shown: restyle after it
            form.Shown += delegate { form.BeginInvoke(new MethodInvoker(ApplyTheme)); };
            form.Disposed += delegate { ThemeManager.ThemeChanged -= OnThemeChanged; };
            form.ResumeLayout(true);
        }

        /// <summary>
        /// Like Finish for a window whose content is a tab control: its pages already hold the pages built with
        /// BeginPage. clientHeight is the height of the tab control.
        /// </summary>
        public void FinishTabs(TabControl tabs, Control help, int clientWidth, int clientHeight, params Button[] footerButtons)
        {
            this.help = help;
            this.footerButtons = footerButtons;
            form.SuspendLayout();

            SettingsSurface content = new SettingsSurface(SettingsSurfaceKind.Window);
            content.Dock = DockStyle.Fill;
            tabs.Dock = DockStyle.Fill;
            content.Controls.Add(tabs);

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
                b.Width = Math.Max(S(96), TextRenderer.MeasureText(b.Text, host.Font).Width + S(36));
                footer.Controls.Add(b);
            }
            footer.Resize += delegate { LayoutFooter(); };
            LayoutFooter();

            form.Controls.Clear();
            form.Controls.Add(content);
            form.Controls.Add(footer);

            // the scrolling area of a flow panel ignores the bottom margin of its last control
            foreach (SettingsPage p in pages)
            {
                Panel end = new Panel();
                end.Height = S(24);
                end.Margin = new Padding(0);
                p.Controls.Add(end);
            }

            Rectangle area = Screen.PrimaryScreen.WorkingArea;
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.SizeGripStyle = SizeGripStyle.Show;
            form.ClientSize = new Size(Math.Min(S(clientWidth), area.Width), Math.Min(S(clientHeight) + footer.Height, area.Height - S(100)));
            form.MinimumSize = new Size(form.Width, Math.Min(form.Height, S(380)));

            ApplyTheme();
            ThemeManager.ThemeChanged += OnThemeChanged;
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
            if (host.IsHandleCreated && !host.IsDisposed)
                host.BeginInvoke(new MethodInvoker(ApplyTheme));
        }

        private void ApplyTheme()
        {
            foreach (Button b in buttons)
                StyleButton(b, false);
            for (int i = 0; i < footerButtons.Length; i++)
                StyleButton(footerButtons[i], i == 0);
            foreach (SettingsPage p in pages)
                StyleLabels(p);
            host.Invalidate(true);
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
