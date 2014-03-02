using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Core.Entities
{
    public class SensorSeries
    {
        public string XField { get; set; }
        public string YField { get; set; }
        public string LineColor { get; set; }
        public List<SensorLog> SensorData { get; set; }
    }
}
