#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AltClock.Data;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;

namespace AltClock.Backend
{
    class WindowsActivityMonitor : IActivityMonitor
    {

        private MouseHookListener mouseListener;

        private KeyboardHookListener kbListener;
        private List<string> pressed = new List<string>();
        private Option<Point2i> hasMoved = Option<Point2i>.None;
        private object mylock = new object();

        public int ActionCount
        {
            get
            {
                lock (mylock)
                {
                    List<string> saved = pressed;
                    pressed = new List<string>();
                    return saved.Count;
                }
            }
        }


        public Option<Point2i> HasMoved
        {
            get
            {
                lock (mylock)
                {
                    Option<Point2i> saved = hasMoved;
                    hasMoved = Option<Point2i>.None;
                    return saved;
                }
            }
        }

        public WindowsActivityMonitor()
        {
            
            mouseListener = new MouseHookListener(new GlobalHooker());
            mouseListener.Enabled = true;
            mouseListener.MouseMove += onMouseMove;
            mouseListener.MouseClick += onMouseClick;
            mouseListener.MouseWheel += onMouseClick;
            kbListener = new KeyboardHookListener(new GlobalHooker());
            kbListener.Enabled = true;
            kbListener.KeyDown += onKeyPress;

        }

        private void onKeyPress(object sender, KeyEventArgs e)
        {
            lock (mylock)
            {
                pressed.Add(e.KeyCode.ToString());
            }
        }

        private void onMouseClick(object sender, MouseEventArgs e)
        {

            lock (mylock)
            {
                if (e.Delta != 0)
                {
                    pressed.Add("WHEEL" + e.Delta);
                }
                else
                {
                    pressed.Add(e.Button.ToString());
                }
                
            }
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            Point2i p = new Point2i(e.X, e.Y);
            hasMoved = new Option<Point2i>(p);
        }
    }
}
