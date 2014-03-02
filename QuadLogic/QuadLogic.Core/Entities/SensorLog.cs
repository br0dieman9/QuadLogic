using QuadLogic.Framework.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Core.Entities
{
    public class SensorLog
    {
        public int Id { get; set; }

        public string SensorName { get; set; }
        
        public int SensorId { get; set; }        
        
        public int Value { get; set; }
        
        [MapField("MilliSecond")]
        public int Millisecond { get; set; }
    }
}
