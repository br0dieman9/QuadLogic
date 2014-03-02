

using System;

namespace QuadLogic.Framework.Data.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class MapNullDateTimeAttribute : MapNullValueAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public MapNullDateTimeAttribute()
            : base(DateTime.MinValue)
        {
        }
    }
}
