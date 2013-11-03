#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System.Collections.Generic;
using System.Linq;
using System;
using Prod.Data;

namespace Prod.Backend
{
    /// <summary>
    /// A monitor of user activity (such as mouse movement).
    /// </summary>
    interface IActivityMonitor
    {
        /// <summary>
        /// The number of actions since the last tick.
        /// </summary>
        int ActionCount { get; }

        /// <summary>
        /// If the mouse has moved, the point will contain its position.
        /// </summary>
        Option<Point2i> HasMoved { get; }
    }
}