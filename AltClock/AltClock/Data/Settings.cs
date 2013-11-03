#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AltClock.Data
{
    class Settings
    {
        public IDictionary<string, ProgramInfo> Programs;
        public ICollection<ProgramInfo> Websites;

        private const string ConfigFile = "config.cfg";

        public static Settings Options { get { return options; } }
        private static Settings options;
        public readonly int UpdateFrequency;
        public readonly int HistoryLength;
        public readonly int IdleTime;
        public readonly int Width;
        public readonly int Height;
        public readonly int Top;
        public readonly int Left;
        public readonly bool TrackExes;
        public readonly bool AlwaysOnTop;
        public static void Load()
        {
            //try
            {
                string data = File.ReadAllText(ConfigFile);
                SettingsInfo settings = Utils.FromInlineJson<SettingsInfo>(data);
                options = new Settings(settings);

            }
            /*catch (Exception e)
            {
                Utils.WriteLine(e.Message);
                Utils.WriteLine("Loading default configuration");
                options = makeDefault();
                throw e;
            }*/
        }

        private Settings(SettingsInfo info)
        {
            this.UpdateFrequency = info.UpdateFrequency;
            this.HistoryLength = info.HistoryLength;
            this.IdleTime = info.IdleTime;
            this.TrackExes = info.TrackExes;
            this.AlwaysOnTop = info.AlwaysOnTop;
            this.Width = info.Width;
            this.Height = info.Height;
            this.Left = info.Left;
            this.Top = info.Top;
            this.Websites = new List<ProgramInfo>(info.Websites);
            this.Programs = new Dictionary<string, ProgramInfo>();
            foreach(ProgramInfo program in info.Programs)
            {
                Programs[program.Name] = program;
            }

            
        }

        private static Settings makeDefault()
        {
            SettingsInfo info = new SettingsInfo();
            return new Settings(info);
        }
    }
}
