#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Prod.Data;
using Prod.Graphing;

namespace Prod
{
    public class AreaChart : UserControl
    {

        private Chart chart;
        private const string ChartName = "Default";//attempting to change this breaks the chart
        private static readonly IDictionary<string, Color> colors =
            new DefaultDict<string, Color>(Color.Gray)
                {
                    {"Productive", Color.Green},
                    {"Unproductive", Color.Red},
                    {"Idle", Color.White}
                };

        public AreaChart()
        {
            this.chart = createChart();
            this.Controls.Add(chart);
            this.Size = chart.Size;
        }

        private static ChartArea createArea()
        {
            var area = new ChartArea(ChartName);
            area.AxisX.Enabled = AxisEnabled.False;
            area.AxisY.Enabled = AxisEnabled.False;
            area.BackColor = SystemColors.Control;
            return area;
        }

        private static Chart createChart()
        {
            var chart = new Chart();
            chart.BeginInit();

            chart.BackColor = SystemColors.Control;
            //chart.BorderlineColor = Color.Black;
            //chart.BorderlineDashStyle = ChartDashStyle.Solid;
            //chart.BorderlineWidth = 1;

            var area = createArea();
            chart.ChartAreas.Add(area);


            chart.Size = new Size(380, 150);

            chart.EndInit();
            return chart;
        }

        
        private static Series initSeries(string name)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.StackedArea100;
            series.Color = colors[name];
            series.Name = name;
            return series;
        }

        public Chart GetChart()
        {
            return chart;
        }


        public static void AddPoints(Chart chart, SeriesData data)
        {
            chart.Series.Clear();

            //hack adjustment to make the chartArea fill up more of the chart
            chart.ChartAreas[0].InnerPlotPosition = new ElementPosition(-7, 0, 114, 100);

            var order = data.AllNames;
            foreach (var name in order)
            {
                Series s = initSeries(name);
                chart.Series.Add(s);
            }

            foreach (var dict in data.Data)
            {
                foreach (var name in order)
                {
                    Series series = chart.Series[name];
                    double value = dict[name];
                    series.Points.AddY(value);
                }
            }
        }


    }
    

}
