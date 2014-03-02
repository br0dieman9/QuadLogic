using System;
using System.Data;

namespace QuadLogic.Framework.Data.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class DataRowReader : IMapDataSource
    {
        bool _createColumns;
        DataRowVersion _version;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        public DataRowReader(DataRow dataRow)
            : this(dataRow, DataRowVersion.Default)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="version"></param>
        public DataRowReader(DataRow dataRow, DataRowVersion version)
        {
            _version = version;

            Init(dataRow);
        }

        void Init(DataRow dataRow)
        {
            if (_dataRow == null && dataRow != null)
            {
                _createColumns = dataRow.Table.Columns.Count == 0;
            }

            _dataRow = dataRow;
        }

        private DataRow _dataRow;
        /// <summary>
        /// 
        /// </summary>
        public DataRow DataRow
        {
            get { return _dataRow; }
            set { Init(value); }
        }

        #region IDataSource Members

        int IMapDataSource.FieldCount
        {
            get { return _dataRow.Table.Columns.Count; }
        }

        string IMapDataSource.GetFieldName(int i)
        {
            return _dataRow.Table.Columns[i].ColumnName;
        }

        object IMapDataSource.GetFieldValue(int i)
        {
            return _dataRow[i, _version];
        }

        object IMapDataSource.GetFieldValue(string name)
        {
            return _dataRow[name, _version];
        }

        object IMapDataSource.GetFieldValue(string colName, PropertyDescriptor propDescriptor, object objEntity)
        {
            object val = propDescriptor.DefaultValue;
            if (_dataRow.Table.Columns.Contains(propDescriptor.PropertyName))
            {
                val = _dataRow[propDescriptor.PropertyName, _version];
                if (val == propDescriptor.NullValue)
                {
                    val = DBNull.Value;
                }
                if (val == DBNull.Value)
                {
                    val = propDescriptor.DefaultValue;
                }
            }

            return val;
        }
        object IMapDataSource.GetFieldValue(int colPos, PropertyDescriptor propDescriptor, object objEntity)
        {
            object val = _dataRow[colPos];

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
            //DataRowReader is not a collection. Return false.
            return false;
        }

        #endregion

    }
}
