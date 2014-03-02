using QuadLogic.Core;
using QuadLogic.Core.Entities;
using QuadLogic.Data.Interfaces;
using QuadLogic.Framework.Utilities;
using QuadLogic.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Service
{
    public class SensorService : ISensorService
    {
        private ISensorRepository _sensorRepository;
        public SensorService(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        public List<Sensor> GetSensors()
        {
            return _sensorRepository.GetSensors();
        }

        public List<SensorLog> GetSensorLog(int sensorId)
        {
            return _sensorRepository.GetSensorLog(sensorId);
        }

        public List<SensorLog> GetSensorLog(string logFilePath)
        {
            return _sensorRepository.GetSensorLog(logFilePath);
        }

        public void SaveSensorData(List<SensorLog> sensorData)
        {
            Console.WriteLine("Serializing Data");
            string sensorDataXml = ObjectSerializer.SerializeObject(sensorData, Encoding.Unicode);

            Console.WriteLine("Saving xml to database");
            _sensorRepository.SaveSensorData(sensorDataXml);
        }
    }
}
