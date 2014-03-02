using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;

namespace QuadLogic.Framework.Visualization
{
    public class ChartGenerator
    {
        public void GenerateChart<T>(string imagePath, T chartData, string xAxisTitle, string yAxisTitle)
        {
            Chart c = new Chart();
            c.AntiAliasing = AntiAliasingStyles.All;
            c.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            c.Width = 775;
            c.Height = 250;

            ChartArea ca = new ChartArea();
            ca.BackColor = Color.FromArgb(248, 248, 248);
            ca.BackSecondaryColor = Color.FromArgb(255, 255, 255);
            ca.BackGradientStyle = GradientStyle.TopBottom;

            ca.AxisY.IsMarksNextToAxis = true;
            ca.AxisY.Title = yAxisTitle;
            ca.AxisY.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisY.LabelStyle.Enabled = true;            
            ca.AxisY.MajorTickMark.Enabled = false;
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(234, 234, 234);
            ca.AxisY.Minimum = 0;
            ca.AxisY.Maximum = 255;
            ca.AxisY.Interval = 25;

            ca.AxisX.IsMarksNextToAxis = true;
            ca.AxisX.Title = xAxisTitle;
            ca.AxisX.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisX.MajorTickMark.LineColor = Color.White;
            ca.AxisX.MajorGrid.LineColor = Color.FromArgb(234, 234, 234);

            ca.AxisX.Minimum = 1;
            ca.AxisX.Maximum = 120;

            ca.AxisX.Interval = 20;
            ca.AxisX.LabelStyle.Enabled = true;

            c.ChartAreas.Add(ca);

            GenerateSeries(c, chartData);
            
            try
            {
                c.SaveImage(imagePath, ChartImageFormat.Png);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error saving image: " + ex.ToString());
            }
        }

        public void GenerateSeries<T>(Chart chart, T chartData)
        {
            for (int i = 0; i < 2; i++)
            {
                Series s = new Series()
                {
                    ChartType = SeriesChartType.Line
                };
                s.Font = new Font("Lucida Sans Unicode", 6f);
                s.Color = Color.FromArgb(215, 47, 6);
                s.BorderColor = Color.FromArgb(159, 27, 13);
                s.BackSecondaryColor = Color.FromArgb(173, 32, 11);
                s.BackGradientStyle = GradientStyle.LeftRight;
                s.XValueType = ChartValueType.Int32;
                s.IsXValueIndexed = true;
                //s.Points.DataBind(seriesData, xField, yField, "");

                chart.Series.Add(s);                
            }
        }
    }
}
