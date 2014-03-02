using Microsoft.Practices.Unity;
using QuadLogic.Core;
using QuadLogic.Core.Entities;
using QuadLogic.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Configuring Unity");
            IUnityContainer container = UnityConfig.GetConfiguredContainer();

            Console.WriteLine("Generating Data");
            //simulate reading 10000 points per second for 60 seconds
            DataGenerator dataGenerator = new DataGenerator();            
            List<SensorLog> sensorData = dataGenerator.GenerateData(600000);

            Console.WriteLine("Saving data");
            ISensorService sensorService = container.Resolve<ISensorService>();
            sensorService.SaveSensorData(sensorData);

            Console.WriteLine("Listener complete...Press any key to terminate");
            Console.ReadLine();
        }
    }
}
