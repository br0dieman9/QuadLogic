using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;

namespace QuadLogic.Framework.Data.Mapping
{
    public class TypeDescriptor:IMapDataSource
    {
        private ArrayList _propInfoList ;

        internal TypeDescriptor(Type type)
        {
            _propInfoList = new ArrayList();
            
            FieldInfo[] fldInfoArray = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            {
                foreach (FieldInfo fldInfo in fldInfoArray)
                {
                    //Ingore Private fields. There is no BindingFlags.Private to filter out Private fields, so, doing it here.
                    if (!fldInfo.IsPrivate)
                    {
                        PropertyDescriptor propDesc = new PropertyDescriptor(fldInfo);

                        _propInfoList.Add(propDesc);
                    }
                }
            }

            PropertyInfo[] propInfoArray = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
             {
                 foreach (PropertyInfo propInfo in propInfoArray)
                 {
                     PropertyDescriptor propDesc = new PropertyDescriptor(propInfo);

                     if (!propDesc.IsIgnore)
                     {
                         _propInfoList.Add(propDesc);
                     }
                 }
             }
        }

        public PropertyDescriptor[] GetOrdinalByDBColnName(string mappedDBNameInUpper)
        {
            ArrayList descList = new ArrayList();
            PropertyDescriptor desc = null;
            for (int cnt = 0; cnt < _propInfoList.Count; cnt++)
            {
                desc = _propInfoList[cnt] as PropertyDescriptor;
                if (desc != null && desc.MappedDBName.ToUpper() == mappedDBNameInUpper)
                {
                    descList.Add(desc);
                }
            }
            PropertyDescriptor[] retDescList = new PropertyDescriptor[descList.Count];
            descList.CopyTo(retDescList);
            return retDescList;
        }

        public PropertyDescriptor[] GetOrdinalByPropName(string propName)
        {
            ArrayList descList = new ArrayList();
            PropertyDescriptor desc = null;
            for (int cnt = 0; cnt < _propInfoList.Count; cnt++)
            {
                desc = _propInfoList[cnt] as PropertyDescriptor;
                if (desc != null && desc.PropertyName.ToUpper() == propName.ToUpper())
                {
                    descList.Add(desc);
                }
            }
            PropertyDescriptor[] retDescList = new PropertyDescriptor[descList.Count];
            descList.CopyTo(retDescList);
            return retDescList;
        }
        
        #region IMapDataSource Implementation

        int IMapDataSource.FieldCount
        {
            get { return _propInfoList.Count; }
        }

        string IMapDataSource.GetFieldName(int i)
        {
            PropertyDescriptor desc = _propInfoList[i] as PropertyDescriptor;
            return desc == null ? "" : desc.PropertyName;
        }

        object IMapDataSource.GetFieldValue(int i)
        {
            throw new Exception("Method not implemented = " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            //return _dataReader.GetValue(i);
        }

        object IMapDataSource.GetFieldValue(string name)
        {
            throw new Exception("Method not implemented = " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        object IMapDataSource.GetFieldValue(string colName, PropertyDescriptor propDescriptor, object objEntity)
        {
            throw new Exception("Method not implemented = " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        object IMapDataSource.GetFieldValue(int colpos, PropertyDescriptor propDescriptor, object objEntity)
        {
            return propDescriptor.GetValue(objEntity);
        }

        bool IMapDataSource.IsCollection()
        {
            //TypeDescriptor is not a collection. Return false.
            return false;
        }

        public ArrayList PropInfoList
        {
            get { return _propInfoList; }
        }

        #endregion
    }
}
