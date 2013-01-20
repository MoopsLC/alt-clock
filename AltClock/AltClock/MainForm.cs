using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Deweyvm.AltClock
{
    public class MainForm : Form
    {
        private const string IconFile = "16x16.ico";
        public MainForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.DoubleBuffered = true;
            new Clock(this);
        }

        /// <summary>
        /// Updates clock to proper sizein the case of non-fixed width fonts.
        /// </summary>
        /// <param name="width">width of displayed text</param>
        /// <param name="height">height of displayed text</param>
        public void UpdatePosition(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            lockToCorner();
        }

        /// <summary>
        /// Locks the form to the bottom right corner of the screen.
        /// </summary>
        private void lockToCorner()
        {
            Rectangle rect = Screen.FromControl(this).WorkingArea;
            this.Left = rect.Right - Width;
            this.Top = rect.Bottom - Height;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.ContextMenu = initContextMenu();
            this.Icon = initIcon();
        }

        private ContextMenu initContextMenu()
        {
            MenuItem exitItem = new MenuItem("&Exit");
            exitItem.Click += (sender, e) => exit();
            ContextMenu menu = new ContextMenu(new[]{ exitItem });
            return menu;
        }

        private Icon initIcon()
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            Icon icon = new Icon(IconFile);
            notifyIcon.Icon = icon;
            notifyIcon.ContextMenu = ContextMenu;
            notifyIcon.Visible = true;
            return icon;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            //easy exit for debugging purposes
            if (e.KeyCode == Keys.Escape)
            {
                exit();
            }
        }

        private void exit()
        {
            Application.Exit();
        }

    }
}
