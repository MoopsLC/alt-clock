#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prod.Graphing
{
    public class SeriesData
    {
        public readonly IList<IDictionary<string, double>> Data;
        public readonly ICollection<string> AllNames;
        public readonly double TimeSpan;
        public SeriesData(IList<IDictionary<string, double>> data, ICollection<string> allNames, double timeSpan)
        {
            this.Data = data;
            this.AllNames = allNames;
            this.TimeSpan = timeSpan;
        }
    }
}
