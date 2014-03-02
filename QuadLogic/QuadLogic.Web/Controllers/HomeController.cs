using QuadLogic.Core;
using QuadLogic.Core.DomainServices;
using QuadLogic.Core.Entities;
//using QuadLogic.Framework.Visualization;
using QuadLogic.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace QuadLogic.Web.Controllers
{
    public class HomeController : Controller
    {
        private ISensorService _sensorService;
        public HomeController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //List<SensorLog> log = _sensorService.GetSensorLog(1);
            //List<SensorLog> log2 = _sensorService.GetSensorLog(2);

            List<SensorLog> log = new List<SensorLog>();
            List<SensorLog> log2 = new List<SensorLog>();
            string dataFile = Server.MapPath("~/Data/datalog.txt");
            // Read the file and display it line by line.
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(dataFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] row = line.Split(':');

                int microSecond = int.Parse(row[0]);
                string[] values = row[1].Split(';');

                int pot1 = int.Parse(values[0]);
                int pot2 = int.Parse(values[1]);
                int pot3 = int.Parse(values[2]);

                SensorLog logRecord = new SensorLog();
                logRecord.SensorId = 1;
                logRecord.MicroSecond = microSecond;
                logRecord.Value = pot1;
                log.Add(logRecord);

                SensorLog logRecord2 = new SensorLog();
                logRecord2.SensorId = 2;
                logRecord2.MicroSecond = microSecond;
                logRecord2.Value = pot2;
                log2.Add(logRecord2);
            }

            file.Close();

            SensorChart chart = new SensorChart();
            SensorSeries fl = new SensorSeries();
            fl.SensorData = log;
            fl.XField = "MicroSecond";
            fl.YField = "Value";
            fl.LineColor = "Red";
            chart.FrontLeftSensor = fl;

            SensorSeries fr = new SensorSeries();
            fr.SensorData = log2;
            fr.XField = "MicroSecond";
            fr.YField = "Value";
            fr.LineColor = "Blue";
            chart.FrontRightSensor = fr;

            ChartGenerator chartGenerator = new ChartGenerator();
            string imagePath = Server.MapPath("~/images/charts/sensorLog.png");
            chartGenerator.GenerateChart(imagePath, chart, "Milli Seconds", "Sensor Value");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
