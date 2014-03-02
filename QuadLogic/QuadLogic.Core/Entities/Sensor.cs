using QuadLogic.Framework.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Core.Entities
{
    public class Sensor
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string FriendlyName { get; set; }
        
        public int SensorTypeId { get; set; }
        
        [MapField("Color")]
        public string ColorName { get; set; }
        
        [MapIgnore]
        public Color Color
        {
            get
            {
                return Color.FromName(ColorName);
            }
        }
    }
}
