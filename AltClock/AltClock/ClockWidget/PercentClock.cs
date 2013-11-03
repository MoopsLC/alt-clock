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
using System.Windows.Forms;

namespace Deweyvm.AltClock.Clock
{
    internal class PercentClock
    {
        //86,400 SI seconds
        private const int HoursPerDay = 24;
        private const int StartHour = 6;
        private const int FontSize = 20;
        private const string FontName = "Consolas";
        private static readonly string[] weekdayPrefixes = new[] { "A", "B", "C", "D", "E", "F" };
        private readonly int[] timeConversions = new[] { 100, 100, 100 };

        private readonly ClockLabel label = new ClockLabel();

        public int Width
        {
            get { return label.Width; }
            set { label.Width = value; }
        }


        public PercentClock()
        {
            //this.locker = locker;
            this.label = initLabel();
            resetSize();
        }

        private static ClockLabel initLabel()
        {
            var label = new ClockLabel();
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Font = new Font(FontName, FontSize);
            
            return label;
        }

        public Control GetControl()
        {
            return label;
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

        public void MouseEnter()
        {
            var now = DateTime.Now;
            string weekday = now.DayOfWeek.ToString().Substring(0, 3);
            string date = weekday + ", " + now.ToString("MMM") + " " + now.Day;
            int hour = now.Hour;
            if (hour > 12)
            {
                hour -= 12;
            }
            int minute = now.Minute;
            int second = now.Second;
            string timeString = makeTimeString(new float[] { hour, minute, second });
            label.Text = string.Format("{0} {1}", date, timeString);
            resetSize();
        }

        public void MouseLeave()
        {
            float totalHours = getTotalHours();
            float normalizedHours = totalHours / HoursPerDay;
            float[] converted = convert(normalizedHours, timeConversions);
            string weekday = getWeekday();

            string timeString = makeTimeString(converted);

            label.Text = string.Format("{0} {1}", weekday, timeString);
            resetSize();
        }

        private void resetSize()
        {
            var size = Size;

            //label.Width = size.Width;
            label.Height = size.Height;
        }

        public Size Size { get { return TextRenderer.MeasureText(label.Text, label.Font); } }

        private static string getWeekday()
        {
            var now = getNow();
            int day = now.DayOfYear - 1;
            const int daysPerWeek = 5;
            
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
