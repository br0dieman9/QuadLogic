using QuadLogic.Core;
using QuadLogic.Data.Interfaces;
using QuadLogic.Framework.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuadLogic.Framework.Data.UOW.Extensions;
using QuadLogic.Core.References;
using QuadLogic.Core.Entities;
using System.IO;

namespace QuadLogic.Data
{
    public class SensorRepository : ISensorRepository
    {
        private IAdoNetContext _context;

        public SensorRepository(IAdoNetContext context)
        {
            _context = context;
        }

        public List<Sensor> GetSensors()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"GetSensors";
                command.CommandTimeout = Constants.DbCommandTimeout;

                return command.ToList<Sensor>();
            }
        }

        public List<SensorLog> GetSensorLog(string logFilePath)
        {
            List<SensorLog> logRecords = new List<SensorLog>();

            List<Sensor> sensors = GetSensors();
            var frl = (from sensor in sensors
                       where sensor.Id == 1
                       select sensor).FirstOrDefault();
            var fll = (from sensor in sensors
                       where sensor.Id == 2
                       select sensor).FirstOrDefault();
            var rl = (from sensor in sensors
                       where sensor.Id == 3
                       select sensor).FirstOrDefault();

            string line;
            using (StreamReader file = new System.IO.StreamReader(logFilePath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] row = line.Split(',');

                    int millisecond = int.Parse(row[0]);
                    int pot1 = int.Parse(row[1]);
                    int pot2 = int.Parse(row[2]);
                    int pot3 = int.Parse(row[3]);

                    SensorLog logRecord = new SensorLog();
                    logRecord.SensorId = frl.Id;
                    logRecord.SensorName = frl.Name;
                    logRecord.Millisecond = millisecond;
                    logRecord.Value = pot1;
                    logRecords.Add(logRecord);

                    SensorLog logRecord2 = new SensorLog();
                    logRecord2.SensorId = fll.Id;
                    logRecord2.SensorName = fll.Name;
                    logRecord2.Millisecond = millisecond;
                    logRecord2.Value = pot2;
                    logRecords.Add(logRecord2);

                    SensorLog logRecord3 = new SensorLog();
                    logRecord3.SensorId = rl.Id;
                    logRecord3.SensorName = rl.Name;
                    logRecord3.Millisecond = millisecond;
                    logRecord3.Value = pot3;
                    logRecords.Add(logRecord3);
                }
                file.Close();
            }

            return logRecords;
        }

        public List<SensorLog> GetSensorLog(int sensorId)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"GetSensorLog";
                command.CommandTimeout = Constants.DbCommandTimeout;

                command.AddParameter("@sensorId", sensorId);

                return command.ToList<SensorLog>();
            }
        }

        public void SaveSensorData(string sensorData)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"SaveSensorData";
                command.CommandTimeout = Constants.DbCommandTimeout;

                command.AddParameter("@sensorData", sensorData);

                command.ExecuteNonQuery();
            }
        }       
    }
}
