using QuadLogic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuadLogic.Core.DomainServices
{
    public class ChartGenerator
    {
        public Chart GenerateChart(SensorChart chartData, string xAxisTitle, string yAxisTitle)
        {
            Chart c = new Chart();
            c.AntiAliasing = AntiAliasingStyles.All;
            c.TextAntiAliasingQuality = TextAntiAliasingQuality.High;

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
            ca.AxisY.Maximum = 1023;
            ca.AxisY.Interval = 100;

            ca.AxisX.IsMarksNextToAxis = true;
            ca.AxisX.Title = xAxisTitle;
            ca.AxisX.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisX.MajorTickMark.LineColor = Color.White;
            ca.AxisX.MajorGrid.LineColor = Color.FromArgb(234, 234, 234);

            ca.AxisX.Minimum = 1;
            ca.AxisX.Maximum = 2500;

            ca.AxisX.LabelStyle.Enabled = true;

            c.ChartAreas.Add(ca);

            GenerateSeries(c, chartData);           

            return c;
        }

        public void GenerateSeries(Chart chart, SensorChart sensorChart)
        {
            if (sensorChart.FrontLeftSensor !=null){
                Series s = new Series()
                {
                    ChartType = SeriesChartType.Line
                };
                s.Font = new Font("Lucida Sans Unicode", 6f);
                s.Color = Color.FromArgb(215, 47, 6);
                s.BorderColor = Color.FromName(sensorChart.FrontLeftSensor.LineColor); //Color.FromArgb(159, 27, 13);
                s.BackSecondaryColor = Color.FromArgb(173, 32, 11);
                s.BackGradientStyle = GradientStyle.LeftRight;
                s.XValueType = ChartValueType.Int32;
                s.IsXValueIndexed = true;
                s.Points.DataBind(sensorChart.FrontLeftSensor.SensorData, sensorChart.FrontLeftSensor.XField, sensorChart.FrontLeftSensor.YField, "");

                chart.Series.Add(s);                
            }

            if (sensorChart.FrontRightSensor != null)
            {
                Series s = new Series()
                {
                    ChartType = SeriesChartType.Line
                };
                s.Font = new Font("Lucida Sans Unicode", 6f);
                s.Color = Color.FromName(sensorChart.FrontRightSensor.LineColor);
                s.BorderColor = Color.FromArgb(159, 27, 13);
                s.BackSecondaryColor = Color.FromArgb(173, 32, 11);
                s.BackGradientStyle = GradientStyle.LeftRight;
                s.XValueType = ChartValueType.Int32;
                s.IsXValueIndexed = true;
                s.Points.DataBind(sensorChart.FrontRightSensor.SensorData, sensorChart.FrontRightSensor.XField, sensorChart.FrontRightSensor.YField, "");

                chart.Series.Add(s);
            } 
        }
    }
}
