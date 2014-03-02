

using System;

namespace QuadLogic.Framework.Data.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class MapNullDecimalAttribute : MapNullValueAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public MapNullDecimalAttribute()
            : base(0m)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nullValue"></param>
        public MapNullDecimalAttribute(decimal nullValue)
            : base(nullValue)
        {
        }
    }
}
