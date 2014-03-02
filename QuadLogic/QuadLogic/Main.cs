using Microsoft.Practices.Unity;
using QuadLogic.Core.Entities;
using QuadLogic.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuadLogic
{
    public partial class Main : Form
    {
        IUnityContainer _container = UnityConfig.GetConfiguredContainer();
        ISensorService _sensorService;
        List<SensorLog> _sensorLog = new List<SensorLog>();
        List<SensorLog> _smoothSensorLog = new List<SensorLog>();
        public Main()
        {
            InitializeComponent();
            _sensorService = _container.Resolve<ISensorService>();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            StartMaximized();
            PopulateSmoothing();
        }

        private void StartMaximized()
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void PopulateSmoothing()
        {
            List<int> smoothingFactors = new List<int>();
            for (int i = 1; i <= 25; i++)
            {
                smoothingFactors.Add(i);
            }
            comboBox1.DataSource = smoothingFactors;            
        }

        private void GenerateChart(List<SensorLog> sensorData)
        {
            Rectangle screen = GetScreen();
            chart1.Width = screen.Width - 200;
            chart1.Height = screen.Height - 100;

            // Enable range selection and zooming UI by default
            ChartArea area = chart1.ChartAreas["Default"];
            area.CursorX.IsUserEnabled = true;
            area.CursorX.IsUserSelectionEnabled = true;
            area.AxisX.ScaleView.Zoomable = true;

            var sensors = _sensorService.GetSensors();
            int i = 0;
            foreach (Sensor sensor in sensors)
            {                
                Series series = GenerateSeries(sensor);
                GenerateSeriesData(sensorData, series);
                SetSeriesAppearance(series, sensor.Color);
                GenerateSeriesLegend(series, sensor, i);
                i++;
            }

            area.AxisX.Title = "Seconds";
            area.AxisY.Title = "Sensor Value";

            //area.AxisX.Interval = 2000;
            //area.AxisX.IntervalType = DateTimeIntervalType.Milliseconds;

            //area.AxisX.LabelStyle.Format = "ss";
        }

        public Rectangle GetScreen()
        {
            return Screen.FromControl(this).Bounds;
        }

        private Series GenerateSeries(Sensor sensor)
        {
            Series series;
            //if (chart1.Series[sensor.Name] != null)
            var seriesExists = chart1.Series.Where(s=>s.Name == sensor.Name).Count();
            if (seriesExists == 0)
            {
                series = new Series();
                series.Name = sensor.Name;
                chart1.Series.Add(series);
            }
            else
            {
                series = chart1.Series[sensor.Name];
            }
            
            //series.IsXValueIndexed = true;
            //string period = "5";
            //chart1.DataManipulator.IsStartFromFirst = true;
            //chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, period, series.Name, series.Name);

            return series;
        }

        private void GenerateSeriesLegend(Series series, Sensor sensor, int index)
        {
            var seriesExists = chart1.Legends["Default"].CustomItems.Where(s=>s.Name == sensor.FriendlyName).Count();
            if (seriesExists == 0)
            {
                LegendItem item = new LegendItem();
                item.Name = sensor.FriendlyName;
                item.SeriesName = series.Name;
                item.Color = sensor.Color;
                item.Tag = series;
                chart1.Legends["Default"].CustomItems.Add(item);
            }
        }

        private void GenerateSeriesData(List<SensorLog> sensorData, Series series)
        {
            DateTime baseDate = new DateTime(1900, 1, 1, 0, 0, 0);

            var seriesData = (from data in sensorData
                              where data.SensorName == series.Name
                              select data).ToList();

            series.Points.Clear();
            for (int index = 0; index < seriesData.Count(); index++)
            {
                // Generate the first point
                var x = index; // baseDate.AddMilliseconds(seriesData[index].Millisecond);
                series.Points.AddXY(x, seriesData[index].Value);
            }

            chart1.Invalidate();
        }

        private int CalculateAvgRecordsPerSecond(List<SensorLog> seriesData)
        {
            int highestMilli = (from s in seriesData
                                select s.Millisecond).Max();
            int totalSeconds = Convert.ToInt32(Math.Round((double)highestMilli / 1000));
            int totalRecords = seriesData.Count();
            int numSensors = (from s in seriesData
                              select s.SensorName).Distinct().Count();



            return 0;
        }

        // Helper method for setting series appearance
        private void SetSeriesAppearance(Series series, Color seriesColor)
        {
            series.ChartArea = "Default";
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;
            series.ShadowOffset = 1;
            series.IsVisibleInLegend = false;
            series.Color = seriesColor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {                
                string file = openFileDialog1.FileName;
                //byte[] fileData = File.ReadAllBytes(file);
                //string fileString = Convert.ToBase64String(fileData);


                try
                {
                    _sensorLog = _sensorService.GetSensorLog(file);
                    GenerateChart(_sensorLog);
                    chart1.Visible = true;
                    chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                    chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();

                    RunStats stats = new RunStats(_sensorLog);
                    label1.Text = stats.ToString();
                }
                catch (IOException)
                {
                }
            }
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            if (result != null && result.Object != null)
            {
                // When user hits the LegendItem
                if (result.Object is LegendItem)
                {
                    // Legend item result
                    LegendItem legendItem = (LegendItem)result.Object;

                    // series item selected
                    Series selectedSeries = (Series)legendItem.Tag;

                    if (selectedSeries != null)
                    {
                        if (selectedSeries.Enabled)
                        {
                            selectedSeries.Enabled = false;
                            selectedSeries.IsVisibleInLegend = false;
                        }

                        else
                        {
                            selectedSeries.Enabled = true;
                            selectedSeries.IsVisibleInLegend = false;
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _sensorLog.Count; i++)
            {
                
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var step = Convert.ToInt32(comboBox1.SelectedValue);

            //generate averge for every x data points
            SmoothData(_sensorLog, step);

            GenerateChart(_smoothSensorLog);
        }

        private void SmoothData(List<SensorLog> rawData, int factor)
        {
            _smoothSensorLog.Clear();
            var sensors = (from r in rawData
                           select r.SensorName).Distinct();

            foreach (string sensor in sensors)
            {
                var currentSensorData = from r in rawData
                                            where r.SensorName == sensor
                                            select r;
                int total = 0;
                int i = 0;
                foreach(SensorLog log in currentSensorData)
                {
                    total += log.Value;
                    
                    if (i % factor == 0)
                    {
                        _smoothSensorLog.Add(new SensorLog
                        {
                            Millisecond = log.Millisecond,
                            SensorId = log.SensorId,
                            SensorName = log.SensorName,
                            Value = total/factor
                        });
                        total = 0;
                    }
                    i++;
                }
            }
        }
    }
}
