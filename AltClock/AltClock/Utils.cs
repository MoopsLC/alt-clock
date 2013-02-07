#region License
// Copyright 2012 deweyvm, see also AUTHORS file.
// Licenced under GPL v3
// see COPYING file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion Licence

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace AltClock
{
    static class Utils
    {
        private static Action<string> logger;
        static Utils()
        {
            initLogger();
        }

        private static void initLogger()
        {
            logger = Console.WriteLine;
        }

        public static double GetTime()
        {
            DateTime epoch = new DateTime(1970, 1, 1);
            TimeSpan now = DateTime.UtcNow - epoch;
            return now.TotalSeconds;
        }

        public static void AttachLogger(Action<string> logger)
        {
            Utils.logger = logger;
            if (logger == null)
            {
                initLogger();
            }
        }

        public static void WriteLine(string line)
        {
            if (logger == null)
            {
                initLogger();
            }
            Debug.Assert(logger != null, "logger != null");
            logger(line + "\n");
        }

        public static IEnumerable<T> TakeLastN<T>(ICollection<T> list, int n)
        {
            int count = list.Count();
            if (count < n)
            {
                return list;
            }
            return list.Skip(count - n);
        }

        public static void WriteLine(string fmt, params object[] args)
        {
            string line;
            try
            {
                line = String.Format(fmt, args);
            }
            catch (FormatException)
            {
                line = "IMPROPER FORMAT:" + fmt;
            }
            catch(ArgumentNullException)
            {
                string[] stringargs = args.Select(obj => obj == null ? "null" : obj.ToString()).ToArray();
                line = String.Format(fmt, stringargs);
            }
            WriteLine(line);
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T FromInlineJson<T>(string inline)
        {
            return FromJson<T>("{" + inline + "}");
        }

        public static string ToJson(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented, false);
        }

        public static string ExtractUrl(string csv)
        {
            const string pattern = "^\"(?<url>[^\"]*)\".*$";
            var matches = Regex.Match(csv, pattern);
            string url = matches.Groups["url"].Value;
            return url;
        }
    }
}
