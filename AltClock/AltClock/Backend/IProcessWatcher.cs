#region License
// Copyright 2012 deweyvm, see also AUTHORS file.
// Licenced under GPL v3
// see COPYING file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion Licence

using System.Collections.Generic;
using System.Linq;
using System;

namespace AltClock.Backend
{
    interface IProcessWatcher
    {
        WindowInfo CurrentWindow { get; }
        void Handle(object sender, EventArgs args);
    }
}