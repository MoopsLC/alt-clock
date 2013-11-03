#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using Prod.Data;

namespace Prod.Backend
{
    /// <summary>
    /// Implementation of IProcessWatcher for windows.
    /// </summary>
    class WindowsProcessWatcher : WindowsProcessWatcherBase
    {
        private IActivityMonitor monitor;
        private ThreadQueue<Info> queue;
        
        public WindowsProcessWatcher(IActivityMonitor monitor, ThreadQueue<Info> queue)
        {
            this.monitor = monitor;
            this.queue = queue;
        }

        public override void Handle(Object myObject, EventArgs myEventArgs)
        {
            int keys = monitor.ActionCount;
            Option<Point2i> moved = monitor.HasMoved;
            WindowInfo window;
            if (myObject == null) //manually triggered
            {
                window = CurrentWindow;
            }
            else //timer invoked
            {
                window = getFocussedName();
            }
            uint pid = window.ProcessId;
            string exename = window.ExeName;
            string title = window.Title;
            double time = Utils.GetTime();
            Option<string> url = window.Url;
            Info info = new Info(time, moved, pid, exename, title, url, keys);
            queue.Add(info);
        }
    }
}
