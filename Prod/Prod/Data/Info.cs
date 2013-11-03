#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;

namespace Prod.Data
{
    public class InfoEventArgs : EventArgs
    {
        public readonly Info Info;
        public InfoEventArgs(Info info)
        {
            this.Info = info;
        }
    }

    delegate void InfoEventHandler(object sender, InfoEventArgs args);

    public struct Info
    {
        public readonly double Time;
        public readonly Option<Point2i> Mouse;
        public readonly string ExeName;
        public readonly string Title;
        public readonly Option<string> Url;
        public readonly int NumKeys;
        public readonly uint ProcessId;

        public Info(double time,
                    Option<Point2i> mouse,
                    uint pid,
                    string exename,
                    string title,
                    Option<string> url,
                    int numKeys)
        {
            this.Time = time;
            Debug.Assert(mouse != null, "mouse != null");
            this.Mouse = mouse;
            Debug.Assert(url != null, "url != null");
            this.Url = url;
            this.ProcessId = pid;
            this.ExeName = exename;
            this.Title = title;
            this.NumKeys = numKeys;
        }

        public override string ToString()
        {
            string movedString = Mouse.HasValue ? Mouse.Value.ToString() : "false";
            return  String.Format("{0}:{1},{2},{3},{4}", ExeName, ProcessId, Time, movedString, NumKeys);
        }

    }
}