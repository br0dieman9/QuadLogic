using System;

namespace QuadLogic.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class MapIgnoreAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapIgnoreAttribute()
		{
		}
	}
}
