#region License
// Copyright 2012 deweyvm, see also AUTHORS file.
// Licenced under GPL v3
// see COPYING file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion Licence

using System.Collections.Generic;
using System.Linq;
using System;
using AltClock.Data;

namespace AltClock.Backend
{
    class WindowInfo
    {
        public readonly uint ProcessId;
        public readonly string ExeName;
        public readonly string Title;
        public readonly Option<string> Url; 
        public WindowInfo(uint pid, string exe, Option<string> url, string title)
        {
            this.ProcessId = pid;
            this.ExeName = exe;
            this.Title = title;
            this.Url = url;
        }
    }
}