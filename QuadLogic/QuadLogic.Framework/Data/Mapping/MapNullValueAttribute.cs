

using System;

namespace QuadLogic.Framework.Data.Mapping
{
    /// <summary>
    /// Is applied to any members that should be mapped to recordset field. 
    /// </summary>
    //[AttributeUsage(
    //	AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Class,
    //	AllowMultiple = true
    //)]
    public class MapNullValueAttribute : MapValueAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public MapNullValueAttribute()
            : base(null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeValue"></param>
        public MapNullValueAttribute(object typeValue)
            : base(typeValue)
        {
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="typeValue"></param>
        ///// <param name="mappedValue"></param>
        //public MapNullValueAttribute(object typeValue, Type mappedValue)
        //    : base(typeValue, typeValue)
        //{
        //}
    }
}
