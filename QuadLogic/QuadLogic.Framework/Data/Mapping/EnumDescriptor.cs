using System;
using System.Collections;
using System.Text;
using System.Reflection;

namespace QuadLogic.Framework.Data.Mapping
{
   
    internal class EnumDescriptor
    {
        private Hashtable _attributeList;
        private Type _currentenumType = null;
        const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

        private object _defaultValue;
        private object _nullValue;

        const string NULL_VALUE = "enum_null";
        const string DEFAULT = "enum_default";

        public EnumDescriptor(Type enumType)
        {
            _currentenumType = enumType;

            _attributeList = new Hashtable();

            FieldInfo[] fields = enumType.GetFields();
        
            foreach (FieldInfo fi in fields)
            {
               if ((fi.Attributes & EnumField) == EnumField)
               {
                    Attribute[] enumAttributes = 
						Attribute.GetCustomAttributes(fi, typeof(MapValueAttribute));

                    if (enumAttributes.Length > 0)
                    {
                        foreach (MapValueAttribute attribute in enumAttributes)
                        {
                            Type attribType = attribute.GetType();

                            //attr.TypeValue = Enum.Parse(enumType, fi.Name);
                            if (attribType == typeof(MapDefaultValueAttribute))
                            {
                                _defaultValue = ((MapDefaultValueAttribute)attribute).DefaultValue;

                                //If default value is not specified as part of the MapAttribute, 
                                //take the Enum feild as default value agains which the MapDefaultValue is declared
                                if (_defaultValue == null)
                                {
                                    _defaultValue = Enum.Parse(enumType, fi.Name);
                                    _attributeList.Add(DEFAULT, attribute);
                                }
                            }
                            else if (attribType == typeof(MapNullValueAttribute))
                            {
                                _nullValue = ((MapNullValueAttribute)attribute).TypeValue;

                                //If default value is not specified as part of the MapAttribute, 
                                //take the Enum feild as default value agains which the MapDefaultValue is declared
                                if (_nullValue == null)
                                {
                                    _nullValue = Enum.Parse(enumType, fi.Name);
                                    _attributeList.Add(NULL_VALUE, attribute);
                                }
                            }
                            else
                            {
                                _attributeList.Add(Enum.Parse(enumType, fi.Name), attribute);
                            }
                        }
                    }
                    else
                    {
                        //No Map Attributes defined. Trreat Enum value itself as mapped value.
                        _attributeList.Add( 
                            Enum.Parse(enumType, fi.Name), 
                            new MapValueAttribute( Convert.ToInt32( Enum.Parse(enumType, fi.Name)) ) );

                    }
               }

            }
        }

        public object MapFrom(object value)
        {

            if (value is string)
                value = ((string)value).Trim();
            
            object returnValue = GetDefaultValue(value);
            
            if (_attributeList.Count > 0)
            {
                foreach (object key in _attributeList.Keys)
                {
                    object attribute = _attributeList[key];

                    IComparable comparableValue = value as IComparable;

                    if ((comparableValue != null) && (attribute is MapValueAttribute) && 
                        (comparableValue.CompareTo( ((MapValueAttribute)attribute).TypeValue) == 0))
                    {
                        returnValue = Convert.ChangeType(key, _currentenumType);
                        break;
                    }

                }

                // There is no value in the map list.
                if (returnValue == null)
                {
                    if (_attributeList.Count > 0)
                    {
                        throw new Exception(
                            value != null ?
                            string.Format("Cannot map '{0}' value '{1}' to '{2}'.", value.GetType().FullName, value, _currentenumType.Name) :
                            string.Format("Cannot map 'null' value to '{0}'.", _currentenumType.Name));
                    }
                }
            }

            return returnValue;
        }

        public object MapTo(object value)
        {

            object returnValue;

            //Initialize to same as input            
            returnValue = value;

            if (_attributeList.Count > 0)
            {
                object attribute = _attributeList[value];

                if ((attribute != null) && (attribute is MapValueAttribute) && ((MapValueAttribute)attribute).TypeValue != null)
                {
                    returnValue = ((MapValueAttribute)attribute).TypeValue;
                }
            }

            return returnValue;
        }

        public object GetDefaultValue(object inputValue)
        {
            object defaultValue = null;

            if (_attributeList.Contains(DEFAULT))
            {
                defaultValue = _defaultValue;
            }
            else if (_attributeList.Contains(NULL_VALUE))
            {
                defaultValue = _nullValue;
            }
            else {
                defaultValue = inputValue;
            }

            return defaultValue;
        }
    }
}
