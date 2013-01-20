using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Deweyvm.AltClock
{
    internal class Clock
    {
        //86,400 SI seconds
        private const int HoursPerDay = 24;

        private const int StartHour = 6;
        private const int TimerResolution = 80;
        private const int FontSize = 20;
        private const string FontName = "Consolas";

        private readonly int[] timeConversions = new[] { 100, 100, 100 };

        private readonly ClockLabel label = new ClockLabel();
        private readonly Timer timer = new Timer();
        private readonly MainForm parent;

        

        public Clock(MainForm form)
        {
            this.parent = form;

            initLabel();
            initTimer();
        }

        private void initLabel()
        {
            parent.Controls.Add(label);
            label.Font = new Font(FontName, FontSize);

            //allow the label text to fill the entire form
            label.Height = int.MaxValue;
            label.Width = int.MaxValue;
        }

        private void initTimer()
        {
            timer.Tick += timerTick;
            timer.Interval = TimerResolution;
            timer.Enabled = true;
        }

        private static DateTime getNow()
        {
            TimeSpan offset = new TimeSpan(StartHour, 0, 0);
            return DateTime.Now.Subtract(offset);
        }

        private static float getTotalHours()
        {
            var now = DateTime.Now;
            int hour = ((now.Hour - StartHour) + HoursPerDay) % HoursPerDay;
            int minute = now.Minute;
            int second = now.Second;
            int ms = now.Millisecond;

            return (ms/1000f + second + minute*60 + hour*60*60)/3600f;
        }

        private bool isMouseInside()
        {
            return label.ClientRectangle.Contains(label.PointToClient(Cursor.Position));
        }

        private void timerTick(object sender, EventArgs e)
        {
            adjustSize();
            if (isMouseInside())
            {
                var now = DateTime.Now;
                string weekday = now.DayOfWeek.ToString().Substring(0,1);
                int hour = now.Hour;
                if (hour > 12)
                {
                    hour -= 12;
                }
                int minute = now.Minute;
                int second = now.Second;
                string timeString = makeTimeString(new float[] {hour, minute, second});
                label.Text = string.Format("{0} {1}", weekday, timeString);
            }
            else
            {
                float totalHours = getTotalHours();
                float normalizedHours = totalHours / HoursPerDay;
                float[] converted = convert(normalizedHours, timeConversions);
                string weekday = getWeekday();

                string timeString = makeTimeString(converted);

                label.Text = string.Format("{0} {1}", weekday, timeString);
            }

            

            
        }

        private void adjustSize()
        {
            var size = TextRenderer.MeasureText(label.Text, label.Font);
            parent.UpdatePosition(size.Width, size.Height);
        }

        private static string getWeekday()
        {
            var now = getNow();
            int day = now.DayOfYear - 1;
            const int daysPerWeek = 5;
            string[] weekdayPrefixes = new[] {"A", "B", "C", "D", "E", "F"};
            if (day == 365) //LEAPYEAR
            {
                return weekdayPrefixes[5];
            }
            return weekdayPrefixes[(day%daysPerWeek)];
        }

        private static string makeTimeString(float[] values)
        {
            var result = new List<string>();
            const string format = "{0:00}";

            foreach (float val in values)
            {
                result.Add(string.Format(format, (int)val));
            }
            return string.Join(":", result);
        }


        /// <summary>
        /// Converts normalized time to the given output format
        /// </summary>
        /// <param name="input">
        /// current time of day, in the interval [0,1)
        /// </param>
        /// <param name="conversions">
        /// time denominations. new[]{24,60,60} would give normal wall-clock time
        /// </param>
        /// <returns></returns>
        private static float[] convert(float input, int[] conversions)
        {
            float prev = input;
            float[] result = new float[conversions.Length];
            for (int i = 0; i < conversions.Length; i += 1)
            {
                float next = conversions[i]*prev;
                prev = next - (int)next;
                result[i] = next;
            }
            return result;
        }
    }
}
