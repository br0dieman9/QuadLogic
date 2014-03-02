using System;

namespace QuadLogic.Framework.Data.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMapDataSource
    {
        /// <summary>
        /// 
        /// </summary>
        int FieldCount { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        string GetFieldName(int i);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        object GetFieldValue(int i);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        object GetFieldValue(string name);

        object GetFieldValue(string colName,PropertyDescriptor propDescriptor,object objEntity);

        object GetFieldValue(int colpos, PropertyDescriptor propDescriptor, object objEntity);

        bool IsCollection();
    }
}
