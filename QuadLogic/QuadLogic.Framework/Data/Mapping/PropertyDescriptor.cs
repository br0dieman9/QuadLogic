using System;
using System.Collections;
using System.Text;
using System.Reflection;

namespace QuadLogic.Framework.Data.Mapping
{
    public class PropertyDescriptor
    {
        private string _propertyName;
        private string _mappedDBName;
        private object _defaultValue;
        private object _nullValue;
        private PropertyInfo _propInfo;
        private bool _isClass;
        EnumDescriptor _enumTypeDescriptor = null;
        private FieldInfo _fieldInfo;
        private Type _type;

        private bool _isIgnore = false;

        public PropertyDescriptor(PropertyInfo propInfo)
        {
            _propInfo = propInfo;

            if (propInfo != null)
            {
                _propertyName = propInfo.Name.ToUpper();

                _mappedDBName = _propertyName; // By default the mapped Db name and property name are same

                _type = propInfo.PropertyType;

                _isClass = !IsBaseType(_type);

                if (_type.IsEnum)
                {
                    _enumTypeDescriptor = EnumDescriptorFactory.CreateEnumDescriptor(_type) as EnumDescriptor;
                }
                object[] attrList = propInfo.GetCustomAttributes(true);
                _isIgnore = false;
                if (attrList != null && attrList.Length > 0)
                {
                    foreach (object attrib in attrList)
                    {
                        Type attribType = attrib.GetType();
                        if (attribType == typeof(MapFieldAttribute))
                        {
                            if(((MapFieldAttribute)attrib).SourceName != null)
                            _mappedDBName = ((MapFieldAttribute)attrib).SourceName.ToUpper(); // Mapped db column name
                        }
                        else if (attribType == typeof(MapDefaultValueAttribute))
                        {
                            _defaultValue = ((MapDefaultValueAttribute)attrib).TypeValue;
                        }
                        else if (attribType == typeof(MapNullValueAttribute))
                        {
                            _defaultValue = ((MapNullValueAttribute)attrib).TypeValue;
                            _nullValue = _defaultValue;

                        } if (attribType == typeof(MapIgnoreAttribute))
                        {
                            _isIgnore = true;
                        }

                    }
                }

                if (_nullValue == null)
                {
                    _nullValue = SetDefatulValue(_type);
                }

                if (_defaultValue == null)
                {
                    _defaultValue = SetDefatulValue(_type);
                }
            }
        }

        public bool IsIgnore
        {
            get
            {
                return _isIgnore;
            }
        }
        public PropertyDescriptor(FieldInfo fldInfo)
        {
            _fieldInfo = fldInfo;

            if (fldInfo != null)
            {
                _propertyName = fldInfo.Name.ToUpper();

                _mappedDBName = _propertyName; // By default the mapped Db name and property name are same

                _type = fldInfo.FieldType;

                _isClass = !IsBaseType(_type);

                if (_type.IsEnum)
                {
                    _enumTypeDescriptor = EnumDescriptorFactory.CreateEnumDescriptor(_type) as EnumDescriptor;
                }
                object[] attrList = fldInfo.GetCustomAttributes(true);

                if (attrList != null && attrList.Length > 0)
                {
                    foreach (object attrib in attrList)
                    {
                        Type attribType = attrib.GetType();
                        if (attribType == typeof(MapFieldAttribute))
                        {
                            if (((MapFieldAttribute)attrib).SourceName != null)
                                _mappedDBName = ((MapFieldAttribute)attrib).SourceName.ToUpper();
                        }
                        else if (attribType == typeof(MapDefaultValueAttribute))
                        {
                            _defaultValue = ((MapDefaultValueAttribute)attrib).DefaultValue;
                        }
                        else if (attribType == typeof(MapNullValueAttribute))
                        {
                            _defaultValue = ((MapNullValueAttribute)attrib).TypeValue;
                            _nullValue = _defaultValue;

                        } if (attribType == typeof(MapIgnoreAttribute))
                        {
                            _isIgnore = true;
                        }
                    }
                }

                if (_nullValue == null)
                {
                    _nullValue = SetDefatulValue(_type);
                }

                if (_defaultValue == null)
                {
                    _defaultValue = SetDefatulValue(_type);
                }
            }
        }

        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }
        public string MappedDBName
        {
            get
            {
                return _mappedDBName;
            }
        }
        public bool IsEnum
        {
            get
            {
                return _type.IsEnum;
            }
        }
        public object DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        public object NullValue
        {
            get
            {
                return _nullValue;
            }
        }

        public PropertyInfo PropInfo
        {
            get
            {
                return _propInfo;
            }
        }

        public FieldInfo FldInfo
        {
            get
            {
                return _fieldInfo;
            }
        }

        public bool IsClass
        {
            get
            {
                return _isClass;
            }
        }

        private object SetDefatulValue(Type type)
        {
            object retValue = null;
            if (type == typeof(System.Int32) ||  type == typeof(System.Byte))
            {
                retValue=  0;
            }
            else if (type == typeof(System.String))
            {
                retValue = "";
            }
            else if (type == typeof(System.Decimal))
            {
                retValue = 0m;
            }
            else if (type == typeof(System.Boolean))
            {
                retValue = false;
            }
            else if (type == typeof(System.Single))
            {
                retValue = 0.0;
            }
            else if (type == typeof(System.DateTime))
            {
                retValue = System.DateTime.MinValue;
            }

            return retValue;
        }

        public void SetValue(object target,object sourcedata)
        {
            object targetData = null;

            try
            {                
                if (IsClass)
                {
                    targetData = Activator.CreateInstance(_type, new object[] { sourcedata });
                }
                else
                {
                    if (sourcedata != null && sourcedata.GetType() == _type)
                    {
                        targetData = sourcedata;
                    }else if (_type.IsEnum)
                    {
                        if (_enumTypeDescriptor != null)
                        {
                            //If the SourceData is null and an NullValues is defined via MapNullValueAttribute, use it!
                            if ((sourcedata == null) && (NullValue != null))
                            { targetData = NullValue; }
                            else
                            { targetData = _enumTypeDescriptor.MapFrom(sourcedata); }
                        }
                    }
                    else
                    {
                        targetData = Convert.ChangeType(sourcedata, _type);
                    }
                }

                if (_propInfo != null && _propInfo.CanWrite)
                {                    
                    _propInfo.SetValue(target, targetData, null);
                }
                else if(_fieldInfo != null)
                {
                    _fieldInfo.SetValue(target, targetData);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error reading value {0}({1}) into {2} for {3}", sourcedata,
                    (sourcedata != null) ? sourcedata.GetType().Name : "null", _type.Name, _propertyName);
                //throw new MappingException(msg, ex);
                throw new Exception(msg, ex);
            }
        }


        public object GetValue(object sourcedata)
        {
            object returnData = null;

            try
            {

                if (_propInfo != null)
                {
                   returnData =  _propInfo.GetValue(sourcedata, null);
                }
                else
                {
                    returnData = _fieldInfo.GetValue(sourcedata);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error reading value {0}({1}) into {2} for {3}", sourcedata, 
                    (sourcedata != null)?sourcedata.GetType().Name : "null", _type.Name, _propertyName);
                throw new Exception(msg, ex);
            }

            return returnData;
        }


        private bool IsBaseType(Type type)
        {
            return
                type == typeof(string) || type == typeof(bool) ||
                type == typeof(byte) || type == typeof(char) ||
                type == typeof(DateTime) || type == typeof(decimal) ||
                type == typeof(double) || type == typeof(Int16) ||
                type == typeof(Int32) || type == typeof(Int64) ||
                type == typeof(sbyte) || type == typeof(float) ||
                type == typeof(UInt16) || type == typeof(UInt32) ||
                type == typeof(UInt64) || type == typeof(Guid) ||
                type.IsEnum;
        }
    }
}
