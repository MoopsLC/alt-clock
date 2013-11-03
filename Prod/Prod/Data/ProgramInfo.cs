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
    /// <summary>
    /// A user specification associated with a particular program or website.
    /// </summary>
    public struct ProgramInfo
    {
        public static readonly ProgramInfo Idle = new ProgramInfo("Idle", new List<string>(new[]{"Idle"}));
        public static readonly ProgramInfo Uncategorized = new ProgramInfo("Uncategorized Program", new List<string>(new[]{"Neutral"}));
        
        /*NOTE these two fields are not readonly so that we can load them 
          NOTE   from JSON automatically*/
        public string Name;
        public List<string> Categories;

        public ProgramInfo(string name, List<string> categories)
        {
            this.Name = name;
            this.Categories = categories;
        }

        private static bool isUrlMatching(string domain, string rawUrl)
        {
            return rawUrl.ToLower().Contains(domain.ToLower());
        }

        public static ProgramInfo FromRaw(TickInfo info)
        {
            if (info.Url.HasValue)
            {
                foreach(var site in Settings.Options.Websites)
                {
                    if (isUrlMatching(site.Name, info.Url.Value))
                    {
                        return site;
                    }
                }
                return new ProgramInfo(info.ExeName, new List<string>(new[]{"Neutral"}));
            }
            else
            {
                if (Settings.Options.Programs.ContainsKey(info.ExeName))
                {
                    return Settings.Options.Programs[info.ExeName];
                }
            }
            return ProgramInfo.Uncategorized;
        }

        public static bool operator==(ProgramInfo lhs, ProgramInfo rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ProgramInfo lhs, ProgramInfo rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (ProgramInfo)) return false;
            return Equals((ProgramInfo)obj);
        }

        public bool Equals(ProgramInfo other)
        {
            return Equals(other.Name, Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return String.Format("Program: {0}, Category: {1}",Name,String.Join(",", Categories));
        }
    }
}