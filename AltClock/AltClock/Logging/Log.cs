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

namespace AltClock.Logging
{
    class Log
    {
        private const string Dir = "C:/Users/M/Desktop/logs";
        private string logFile;
        public Log()
        {
            verifyDirectory();
            makeLogfile();
        }

        private void verifyDirectory()
        {
            if (File.Exists(Dir) && !Directory.Exists(Dir))
            {
                throw new Exception("'Dir' refers to a regular file on disk rather than a directory.");
            }

            Directory.CreateDirectory(Dir);

        }

        private void makeLogfile()
        {
            string name = string.Format("text-{0:yyyy-MM-dd_HH-mm-ss}.txt",
        DateTime.UtcNow);
            logFile = Dir + '/' + name;
            if (File.Exists(logFile))
            {
                throw new Exception("'logFile' exists which should be impossible");
            }

        }

        public void AcceptInfo(object sender, InfoEventArgs args)
        {
            string line = args.Info.ToString();
            //Utils.WriteLine(line);
            File.AppendAllLines(logFile, new[] { line });
        }

    }
}
