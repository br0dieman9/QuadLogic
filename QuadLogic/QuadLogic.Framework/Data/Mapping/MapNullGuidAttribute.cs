
using System;

namespace QuadLogic.Framework.Data.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class MapNullGuidAttribute : MapNullValueAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public MapNullGuidAttribute()
            : base(Guid.Empty)
        {
        }
    }
}
