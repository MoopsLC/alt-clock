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
    class SettingsInfo
    {
        public int UpdateFrequency = 5*60;
        public int HistoryLength = 10;
        public int IdleTime = 120;
        public bool TrackExes = false;
        public bool AlwaysOnTop = false;
        public int Width = 800;
        public int Height = 600;
        public int Left = 0;
        public int Top = 0;
        public List<ProgramInfo> Programs = new List<ProgramInfo>();
        public List<ProgramInfo> Websites = new List<ProgramInfo>();
    }
}