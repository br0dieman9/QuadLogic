
using System;
using System.Data;

namespace QuadLogic.Framework.Data.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class DataReaderSource : IMapDataSource
    {
        IDataReader _dataReader;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        public DataReaderSource(IDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        #region IDataSource Members

        int IMapDataSource.FieldCount
        {
            get { return _dataReader.FieldCount; }
        }

        string IMapDataSource.GetFieldName(int i)
        {
            return _dataReader.GetName(i);
        }

        object IMapDataSource.GetFieldValue(int i)
        {
            return _dataReader.GetValue(i);
        }

        object IMapDataSource.GetFieldValue(string name)
        {
            return _dataReader[name];
        }

        object IMapDataSource.GetFieldValue(string colName, PropertyDescriptor propDescriptor,object objEntity)
        {
            object val = _dataReader[colName];
            
            if (val == propDescriptor.NullValue)
            {
                val = DBNull.Value;
            }
            if (val == DBNull.Value)
            {
                val = propDescriptor.DefaultValue;
            }
            return val;
        }

        object IMapDataSource.GetFieldValue(int colpos, PropertyDescriptor propDescriptor, object objEntity)
        {
            object val = _dataReader.GetValue(colpos);

            if (val == propDescriptor.NullValue)
            {
                val = DBNull.Value;
            }
            if (val == DBNull.Value)
            {
                val = propDescriptor.DefaultValue;
            }
            return val;
        }

        bool IMapDataSource.IsCollection()
        {
            //DataReader is not a collection. Return false.
            return false;
        }

        #endregion
    }
}
