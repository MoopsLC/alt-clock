#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using System.Text;
using NDde.Client;
using Prod.Data;
using SHDocVw;

namespace Prod.Backend
{
    /// <summary>
    /// Utility methods for the WindowsProcessWatcher, performing functions
    /// such as detecting the URL of an active browser for the three major
    /// browsers (at least, as of writing this ;) )
    /// </summary>
    abstract class WindowsProcessWatcherBase : IProcessWatcher
    {
        public WindowInfo CurrentWindow { get; private set; }
        public abstract void Handle(Object myObject, EventArgs myEventArgs);



        // /////////// Url detection in active browsers ///////////

        private const int WM_GETTEXTLENGTH = 0x000E;
        private const int WM_GETTEXT = 0x000D;


        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        public static string GetChromeUrl()
        {
            var procs = Process.GetProcessesByName("chrome");
            if (procs.Length == 0)
            {
                return null;
            }
            foreach (var proc in procs)
            {
                var hAddressBox = FindWindowEx(proc.MainWindowHandle, IntPtr.Zero, "Chrome_OmniboxView", null);
                var sb = new StringBuilder(256);
                SendMessage(hAddressBox, 0x000D, 256, sb);
                string url = sb.ToString();
                if (!string.IsNullOrEmpty(url))
                {
                    return url;
                }
            }
            return string.Empty;

        }

        public static string GetFirefoxUrl()
        {
            var dde = new DdeClient("Firefox", "WWW_GetWindowInfo");
            dde.Connect();
            string url = dde.Request("URL", int.MaxValue);
            dde.Disconnect();
            return url;
        }

        /// <summary>
        /// Returns the URL in the Internet Explorer instance in focus.
        /// If the URL cannot be gotten for any reason, returns string.Empty.
        /// </summary>
        /// <returns></returns>
        public static string GetInternetExplorerUrl()
        {
            IntPtr foreground = GetForegroundWindow();
            foreach (InternetExplorer ieInst in new ShellWindows())
            {
                if (foreground == (IntPtr)ieInst.HWND)
                {
                    return ieInst.LocationURL;
                }
            }
            return string.Empty;
        }

        // ///////////////////////////////



        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private static WinEventDelegate myDuder;

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        public void WinEventProc(IntPtr hWinEventHook,
                                 uint eventType,
                                 IntPtr hwnd,
                                 int idObject,
                                 int idChild,
                                 uint dwEventThread,
                                 uint dwmsEventTime)
        {
            CurrentWindow = getFocussedName();
            Handle(null, null);
        }

        
        protected WindowsProcessWatcherBase()
        {
            myDuder = WinEventProc;
            IntPtr retval = SetWinEventHook(EVENT_SYSTEM_FOREGROUND,
                                            EVENT_SYSTEM_FOREGROUND,
                                            IntPtr.Zero,
                                            myDuder,
                                            0,
                                            0,
                                            WINEVENT_OUTOFCONTEXT);
            if (retval == IntPtr.Zero)
            {
                throw new Exception("Failed to hook focus change event.");
            }
        }

        private static string getWindowTitle(IntPtr window)
        {
            const int count = 512;
            StringBuilder buff = new StringBuilder(count);
            string title = "";
            if (GetWindowText(window, buff, count) > 0)
            {
                title = buff.ToString();
            }
            return title;
        }

        private string getExeName(uint pid)
        {
            Process p = Process.GetProcessById((int)pid);
            return p.ProcessName;
        }

        private static Option<string> getUrl(string browser)
        {
            browser = browser.ToLower();
            string url = null;
            if (browser.Contains("firefox"))
            {
                url = GetFirefoxUrl();
                url = Utils.ExtractUrl(url);
            }
            else if (browser.Contains("chrome"))
            {
                url = GetChromeUrl();
            }
            else if (browser.Contains("iexplore"))
            {
                url = GetInternetExplorerUrl();
            }
            if (string.IsNullOrEmpty(url))
            {
                return Option<string>.None;
            }
            return new Option<string>(url);
        }

        private static readonly string[] browsers
            = new[]
                  {
                      "firefox",
                      "chrome",
                      "iexplore"
                  };

        private static bool isBrowser(string exename)
        {
            return browsers.Contains(exename.ToLower());
        }

        protected WindowInfo getFocussedName()
        {
            uint pid;
            string exename;
            string title;
            Option<string> url = Option<string>.None;
            try
            {
                IntPtr window = GetForegroundWindow();
                GetWindowThreadProcessId(window, out pid);
                exename = getExeName(pid);
                title = getWindowTitle(window);
                if (isBrowser(exename))
                {
                    url = getUrl(exename);
                }
            }
            catch (Exception e)
            {
                exename = "error:" + e.Message;
                title = e.Message;//Unable to enumerate, Access is denied
                pid = unchecked((uint)(-1));
            }

            return new WindowInfo(pid, exename, url, title);
        }


    }
}