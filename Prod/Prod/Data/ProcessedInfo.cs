#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Prod.Data
{
    public struct ProcessedInfo
    {
        public readonly ProgramInfo Program;
        public readonly bool DidActivity;
        public readonly double Time;

        public ProcessedInfo(Info info)
        {
            this.Program = ProgramInfo.FromRaw(info);
            this.DidActivity = info.NumKeys > 0 || info.Mouse.HasValue;
            this.Time = info.Time;
        }

        public override string ToString()
        {
            return string.Format("Program({0})", Program.Name);
        }
    }
}