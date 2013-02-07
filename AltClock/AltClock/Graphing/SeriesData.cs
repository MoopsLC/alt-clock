using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltClock.Graphing
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
