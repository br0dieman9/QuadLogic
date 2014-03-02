using QuadLogic.Core;
using QuadLogic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Listener
{
    public class DataGenerator
    {
        public List<SensorLog> GenerateData(int maxDataPoints)
        {
            Random random = new Random();
            List<SensorLog> logData = new List<SensorLog>();
                        
            for (int i = 0; i < maxDataPoints; i++)
            {
                SensorLog log = new SensorLog();
                log.Millisecond = i;
                log.SensorId = 1;
                log.Value = random.Next(0, 255);
                logData.Add(log);
            }

            return logData;
        }
    }
}
