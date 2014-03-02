using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace QuadLogic.Framework.Data.Mapping
{
    public class DataDescriptor
    {
        private string _dataColName;
        private int _dataColPos;
        private PropertyDescriptor _propDesc;

        public DataDescriptor(string colName, int colPos, PropertyDescriptor propDesc)
        {
            _dataColName = colName;
            _propDesc = propDesc;
            _dataColPos = colPos;
        }

        public int ColumnPosition
        {
            get
            {
                return _dataColPos;
            }
        }

        public PropertyDescriptor PropertyDescription
        {
            get
            {
                return _propDesc;
            }
        }
    }
}
