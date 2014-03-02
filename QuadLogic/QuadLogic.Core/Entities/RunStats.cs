using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Core.Entities
{
    public class RunStats
    {
        private List<SensorLog> _data;
        public RunStats(List<SensorLog> data)
        {
            _data = data;
        }

        public int HighestMillisecond
        {
            get
            {
                return (from s in _data
                        select s.Millisecond).Max();
            }
        }

        public int TotalSeconds
        {
            get
            {
                return Convert.ToInt32(Math.Round((double)HighestMillisecond / 1000));
            }
        }

        public int TotalRecords
        {
            get
            {
                return _data.Count();
            }
        }

        public int SensorCount
        {
            get
            {
                return (from s in _data
                        select s.SensorName).Distinct().Count();
            }
        }

        public int RecordsPerSensor
        {
            get
            {
                return TotalRecords / SensorCount;
            }
        }


        public int AverageRecordsPerSecond
        {
            get
            {
                int recordsPerSensor = TotalRecords / SensorCount;
                int recordsPerSecond = recordsPerSensor / TotalSeconds;
                return recordsPerSecond;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Log Duration: {0} sec{1}", this.TotalSeconds, Environment.NewLine);
            builder.AppendFormat("Total Log Records: {0}{1}", this.TotalRecords, Environment.NewLine);
            builder.AppendFormat("Sensor Count: {0}{1}", this.SensorCount, Environment.NewLine);
            builder.AppendFormat("Records per Sensor: {0}{1}", this.RecordsPerSensor, Environment.NewLine);            
            builder.AppendFormat("Average rec/sec: {0}{1}", this.AverageRecordsPerSecond, Environment.NewLine);

            return builder.ToString();
        }
    }
}
