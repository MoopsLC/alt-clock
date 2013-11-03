#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltClock.Graphing
{
    class SeriesEntry
    {
        // ReSharper disable InconsistentNaming
        // ReSharper disable NotAccessedField.Local
        // ReSharper disable MemberCanBePrivate.Local
        public string name = "";


        public string color = "";
        public List<double> data = new List<double>();

        public SeriesEntry(string name, List<double> data)
        {
            this.name = name;
            this.data = data;
            this.color = HashColor(name);
        }

        public static string HashColor(string value)
        {
            if (value == "Productive")
            {
                return "#228b22";
            }
            else if (value == "Unproductive")
            {
                return "#cb4154";
            }
            else if (value == "Neutral")
            {
                return "#b0b7C6";
            }
            else if (value == "Idle")
            {
                return "#000000";
            }
            int hash = value.GetHashCode();
            int r = (hash >> 8) % 128;
            int g = (hash >> 16) % 128;
            int b = (hash >> 24) % 128;

            r = 128 - r + 32;
            g = 128 - g + 32;
            b = 128 - b + 32;

            return string.Format("#{0:X}{1:X}{2:X}", r, g, b);
        }

        public override string ToString()
        {
            return Utils.ToJson(this);
        }
        // ReSharper restore MemberCanBePrivate.Local
        // ReSharper restore NotAccessedField.Local
        // ReSharper restore InconsistentNaming

    }
}
