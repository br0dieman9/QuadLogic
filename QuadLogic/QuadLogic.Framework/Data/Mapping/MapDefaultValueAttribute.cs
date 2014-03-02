
using System;

namespace QuadLogic.Framework.Data.Mapping
{
    /// <summary>
    /// Is applied to any members that should be mapped to recordset field. 
    /// </summary>
    public class MapDefaultValueAttribute : MapValueAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public MapDefaultValueAttribute()
            : base(null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeValue"></param>
        public MapDefaultValueAttribute(object typeValue)
            : base(typeValue)
        {
        }
    }
}
