#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Prod.Backend;
using Prod.Data;
using Timer = System.Windows.Forms.Timer;

namespace Prod.Gui
{
    /// <summary>
    /// Exposes an event of type InfoEventHandler which asyncronously 
    /// recieves activity info and processes it in the specified way,
    /// such as logging or graphing.
    /// </summary>
    class ActivityInfoProcessor
    {
        private IDictionary<string, double> activeTimeMap = 
            new DefaultDict<string, double>(); 

        private IDictionary<string, double> idleTimeMap =
            new DefaultDict<string, double>(); 

        private IDictionary<string, int> actionMap =
            new DefaultDict<string, int>();

        private Timer myTimer;
        private Timer consumeTimer;

        private IActivityMonitor monitor;
        private IProcessWatcher watcher;

        private ThreadQueue<Info> queue = new ThreadQueue<Info>();
        
        public event InfoEventHandler InfoReceived;

        public ActivityInfoProcessor()
        {

            this.monitor = new WindowsActivityMonitor();
            this.watcher = new WindowsProcessWatcher(monitor, queue);
            
            myTimer = new Timer {Interval = 1000};
            myTimer.Tick += watcher.Handle;

            consumeTimer = new Timer {Interval = 100};
            consumeTimer.Tick += consume;
        }

        public void Begin()
        {
            myTimer.Enabled = true;
            consumeTimer.Enabled = true;
        }

        private void refreshTop()
        {
            //grid.Update(activeTimeMap, idleTimeMap, actionMap);
        }

        private Info last;
        private double lastIdleCommitted;
        private double lastFocusCommitted;
        private double lastChangedProgram;
        private double lastAction;
        private int actions;
        private double idleTime;

        private void changeProgram(Info info)
        {
            lastChangedProgram = info.Time;
            actions = 0;
        }

        private void addTime(string exename, double time)
        {
            if (isBad(exename)) return;
            activeTimeMap[exename] += time;
        }

        private void addIdleTime(double time)
        {
            if (isBad(last.ExeName)) return;
            idleTimeMap[last.ExeName] += time;
        }

        private void addActions(string exename, int actions)
        {
            if (isBad(exename)) return;
            actionMap[exename] += actions;
        }

        private bool isBad(string name)
        {
            return string.IsNullOrEmpty(name);
        }

        private void seeAction(Info info)
        {
            lastAction = info.Time;
            actions += info.NumKeys;
            addActions(info.ExeName, info.NumKeys);
        }

        private void updateText(Info info)
        {
            if (last.ProcessId != info.ProcessId)
            {
                changeProgram(info);
            }
            
            commitIdleTime(info);
            refreshTop();
            updateActive(info);
            commitFocusTime(info);
        }

        private void commitIdleTime(Info info)
        {
            if (info.NumKeys == 0 && !info.Mouse.HasValue)
            {
                var diff = info.Time - lastIdleCommitted;
                lastIdleCommitted = info.Time;
                addIdleTime(diff);
                if (lastAction == 0)
                {
                    lastAction = info.Time;
                    idleTime = 0;
                }
                else
                {
                    idleTime = info.Time - lastAction;
                }
            }
            else
            {
                lastIdleCommitted = info.Time;
                seeAction(info);
                idleTime = 0;
            }
        }

        private void commitFocusTime(Info info)
        {
            if (lastFocusCommitted != 0)
            {
                double diff = info.Time - lastFocusCommitted;
                addTime(info.ExeName, diff);
                lastFocusCommitted = info.Time;
            }
            else
            {
                lastFocusCommitted = info.Time;
            }
        }

        private void updateActive(Info info)
        {
            
        }

        /// <summary>
        /// Returns how much time has been spent in the currently focussed
        /// application. Accurate to one second.
        /// </summary>
        private double getTimeInApplication()
        {
            double result = last.Time - lastChangedProgram;
            if (result < 0) return 0;
            return result;
        }

        private void takeInfo(Info info)
        {
            updateText(info);
            if (InfoReceived != null)
            {
                InfoReceived.Invoke(this, new InfoEventArgs(info));
            }
            last = info;
        }

        private void consume(object sender, EventArgs args)
        {
            Option<Info> next = queue.TryTake();
            while(next.HasValue)
            {
                Info info = next.Value;
                takeInfo(info);
                next = queue.TryTake();
            }
        }
    }
}
