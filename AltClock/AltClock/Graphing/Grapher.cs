#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AltClock.Data;

namespace AltClock.Graphing
{
    public class Grapher
    {
        public enum Type
        {
            File,
            Stream
        }

        private int categoryIndex;
        private List<Info> infos = new List<Info>();
        private readonly int partitions = 15;
        private readonly int updateFrequency = 60;
        public Action<SeriesData> SeriesAction;

        public Grapher()
        {
            updateFrequency = Settings.Options.UpdateFrequency;
            partitions = Settings.Options.HistoryLength;
        }

        public void AcceptInfo(object sender, InfoEventArgs args)
        {
            Info info = args.Info;
            infos.Add(info);
            if (infos.Count % updateFrequency == 0)
            {
                var data = GenerateSeries(infos.Select(i => new ProcessedInfo(i)).ToList());
                if (SeriesAction != null)
                {
                    SeriesAction(data);
                }
            }
        }

        
        public SeriesData GenerateSeries(IList<ProcessedInfo> infos)
        {
            bool isTrackingExes = Settings.Options.TrackExes;
            infos = infos.OrderBy(i => i.Time).ToList();
            double minTime = infos[0].Time;
            double totalTime = infos[infos.Count - 1].Time - infos[0].Time;
            if (totalTime < 0)
            {
                throw new Exception("backwards sort");
            }
            var allNames = new HashSet<string>();
            var all = new List<IDictionary<string, double>>();

            var next = new DefaultDict<string, double>();
            double time = minTime;
            double timeSpan;// =  totalTime / partitions;
            timeSpan = 60 * 14.4; //one percent hour
            if (timeSpan > updateFrequency)
            {
                timeSpan = updateFrequency;
            }

            foreach (var info in infos)
            {
                if (info.Program.Categories.Count != 0)
                {
                    allNames.Add(info.Program.Categories[categoryIndex]);
                }
            }

            double lastTime = time;
            double lastActiveTime = -1;

            foreach (var info in infos)
            {
                if (info.DidActivity || lastActiveTime < 0)
                {
                    lastActiveTime = lastTime;
                }

                string key;
                if (info.Time - lastActiveTime > Settings.Options.IdleTime)
                {
                    var program = ProgramInfo.Idle;
                    allNames.Add(program.Name);
                    key = program.Name;
                }
                else
                {
                    try
                    {
                        key = info.Program.Categories[categoryIndex];
                    } 
                    catch
                    {
                        key = "Neutral";
                    }
                }


                next[key] += info.Time - lastTime;

                lastTime = info.Time;
                if (info.Time - time > timeSpan)
                {
                    time = info.Time;
                    all.Add(next);
                    next = new DefaultDict<string, double>();
                }
            }

            return new SeriesData(all.Select(normalize).ToList(), allNames, timeSpan);
        }

        private string makeJavascriptGraph(ICollection<string> allNames, IList<IDictionary<string, double>> all)
        {
            List<string> entries = new List<string>();

            foreach (string name in allNames)
            {
                List<double> values = new List<double>();
                foreach (var map in all)
                {
                    double value = map[name];
                    values.Add(value);
                }

                SeriesEntry entry = new SeriesEntry(name, values);
                entries.Add(entry.ToString());
            }


            return string.Join(",\n", entries);
        }

        private static IDictionary<string, double> normalize(IDictionary<string, double> map)
        {
            var result = new DefaultDict<string, double>();
            var total = map.Values.Sum();
            foreach(var pair in map)
            {
                result[pair.Key] = pair.Value/total;
            }
            return result;
        }

        

    }
}
