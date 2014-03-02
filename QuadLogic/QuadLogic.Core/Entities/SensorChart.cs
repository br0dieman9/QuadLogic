using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Core.Entities
{
    public class SensorChart
    {
        public SensorSeries FrontRightSensor { get; set; }
        public SensorSeries FrontLeftSensor { get; set; }
    }
}
