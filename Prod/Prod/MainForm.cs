﻿#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Deweyvm.Prod.Clock;
using Prod;
using Prod.Gui;

namespace Deweyvm.Prod
{
    public class MainForm : ProcessWatchingForm
    {
        private const int TimerResolution = 80;
        private const string IconFile = "16x16.ico";
        private SplitContainer split;
        private AreaChart chart;
        private PercentClock percentClock;
        private Timer timer;
        private NotifyIcon notifyIcon;

        public MainForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint 
                | ControlStyles.UserPaint 
                | ControlStyles.DoubleBuffer, true);
            this.DoubleBuffered = true;
            this.percentClock = new PercentClock();
            this.chart = new AreaChart();
            this.split = createSplitter();


            split.Panel1.Controls.Add(chart);
            split.Panel2.Controls.Add(percentClock.GetControl());
            
            grapher.SeriesAction = (data) => AreaChart.AddPoints(chart.GetChart(), data);
            
            
            Controls.Add(split);
            initTimer();
        }

        private static SplitContainer createSplitter()
        {
            DoubleBufferedSplitter splitter = new DoubleBufferedSplitter();
            splitter.Panel2MinSize = 10;
            splitter.SplitterDistance = 999;

            splitter.Orientation = Orientation.Horizontal;
            splitter.Dock = DockStyle.Fill;
            splitter.IsSplitterFixed = true;
            return splitter;
        }

        private void initTimer()
        {
            timer = new Timer();
            timer.Tick += (e, o) => timerTick();
            timer.Interval = TimerResolution;
            timer.Enabled = true;
        }

        public int ThingWidth { get { return Math.Max(percentClock.Size.Width, chart.Width); } }

        /// <summary>
        /// Updates clock to proper size in the case of non-fixed width fonts.
        /// </summary>
        /// <param name="height">height of displayed text</param>
        /// <param name="isMinimized">whether or not the chart is minimized</param>
        public void UpdateSize(int height, bool isMinimized=false)
        {
            this.Height = percentClock.Size.Height + split.Panel1.Height;
            if (isMinimized)
            {
                percentClock.Width = percentClock.Size.Width;
                this.Width = percentClock.Width;
            }
            else
            {
                percentClock.Width = ThingWidth;
                this.Width = ThingWidth;
            }
            
        }

        private void updatePosition(int x, int y)
        {
            this.Left = x;
            this.Top = y;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.ContextMenu = initContextMenu();
            this.Icon = initIcon();
        }

        //create a right click menu for the tray icon
        private ContextMenu initContextMenu()
        {
            MenuItem exitItem = new MenuItem("&Exit");
            exitItem.Click += (sender, e) => exit();
            ContextMenu menu = new ContextMenu(new[]{ exitItem });
            return menu;
        }

        //create an icon in the tray
        private Icon initIcon()
        {
            this.notifyIcon = new NotifyIcon();
            Icon icon = new Icon(IconFile);
            notifyIcon.Icon = icon;
            notifyIcon.ContextMenu = ContextMenu;
            notifyIcon.Visible = true;
            return icon;
        }

        private Rectangle getRect()
        {
            var screenRect = Screen.PrimaryScreen.WorkingArea;
            return new Rectangle(0, 0, screenRect.Width, screenRect.Height);
        }

        private void mouseEnter()
        {
            split.Panel1Collapsed = false;
            var clockSize = percentClock.Size;
            
            var height = clockSize.Height + chart.Height;
            UpdateSize(height);
            Rectangle rect = getRect();
            updatePosition(rect.Width - Width, rect.Height - height);
        }

        private void mouseLeave()
        {
            split.Panel1Collapsed = true;
            var clockSize = percentClock.Size;
            
            split.Panel1MinSize = chart.Height;
            split.SplitterDistance = chart.Height;
            var height = clockSize.Height;
            UpdateSize(height, true);
            var rect = getRect();
            updatePosition(rect.Width - Width, rect.Height - height);
        }

        private void timerTick()
        {
            SuspendLayout();
            var mouseInside = RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position);
            if (mouseInside)
            {
                mouseEnter();
                percentClock.MouseEnter();
            }
            else
            {
                mouseLeave();
                percentClock.MouseLeave();
            }
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
            notifyIcon.Dispose();
            Application.Exit();
        }

    }
}
