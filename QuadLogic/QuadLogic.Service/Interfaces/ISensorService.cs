using QuadLogic.Core;
using QuadLogic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Service.Interfaces
{
    public interface ISensorService
    {
        List<Sensor> GetSensors();

        List<SensorLog> GetSensorLog(int sensorId);
        List<SensorLog> GetSensorLog(string logFilePath);

        void SaveSensorData(List<SensorLog> sensorData);
    }
}
