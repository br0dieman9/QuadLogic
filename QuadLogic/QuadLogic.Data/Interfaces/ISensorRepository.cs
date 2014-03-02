using QuadLogic.Core;
using QuadLogic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Data.Interfaces
{
    public interface ISensorRepository
    {
        List<Sensor> GetSensors();

        List<SensorLog> GetSensorLog(string logFilePath);
        List<SensorLog> GetSensorLog(int sensorId);

        void SaveSensorData(string sensorData);
    }
}
