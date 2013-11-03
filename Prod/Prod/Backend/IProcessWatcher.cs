#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System.Collections.Generic;
using System.Linq;
using System;

namespace Prod.Backend
{
    /// <summary>
    /// A generic interface for watching the foremost window and its
    /// associated process.
    /// </summary>
    interface IProcessWatcher
    {
        /// <summary>
        /// Provides information about the current window and process.
        /// </summary>
        WindowInfo CurrentWindow { get; }

        /// <summary>
        /// Event handler that is invoked on a timer tick, or upon changing window focus
        /// to signal that the current window should be updated.
        /// </summary>
        void Handle(object sender, EventArgs args);
    }
}