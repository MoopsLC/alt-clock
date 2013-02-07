#region License
// Copyright 2012 deweyvm, see also AUTHORS file.
// Licenced under GPL v3
// see COPYING file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion Licence

using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Forms;
using AltClock.Data;
using AltClock.Graphing;
using AltClock.Logging;

namespace AltClock.Gui
{
    public class ProcessWatchingForm : Form
    {
        protected Grapher grapher;
        public ProcessWatchingForm()
        {
            Settings.Load();
            //Log log = new Log();
            initForm();
            InfoProcessor m = new InfoProcessor();
            grapher = new Grapher();
            //m.InfoReceived += log.AcceptInfo;
            m.InfoReceived += grapher.AcceptInfo;

            m.Begin();
        }

        private void initForm()
        {
            StartPosition = FormStartPosition.Manual;
            Width = Settings.Options.Width;
            Height = Settings.Options.Height;
            Left = Settings.Options.Left;
            Top = Settings.Options.Top;
            TopMost = Settings.Options.AlwaysOnTop;
            FormBorderStyle = FormBorderStyle.None;

            SetStyle(ControlStyles.AllPaintingInWmPaint
                          | ControlStyles.UserPaint
                          | ControlStyles.DoubleBuffer, true);
            KeyPreview = true;
        }

        public void Add(Control control)
        {
            Controls.Add(control);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
            base.OnKeyDown(e);
        }
    }
}