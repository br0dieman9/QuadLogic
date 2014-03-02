using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using QuadLogic.Framework.Data.Mapping;

namespace QuadLogic.Framework.Data.Mapping
{
    public class Map
    {
        static char[] _trimArray = new char[0];

        private static IMapDataSource GetDataSource(object obj)
        {
            
            if (obj is IMapDataSource)
                return (IMapDataSource)obj;

            if (obj is DataRow)
                return new DataRowReader((DataRow)obj);

            if (obj is DataTable)
                return new DataRowReader(((DataTable)(obj)).Rows[0]);

            if (obj is IDataReader)
                return new DataReaderSource((IDataReader)obj);

            return TypeDescriptorFactory.CreateTypeDescriptor(obj.GetType());
        }

        public static T ToObject<T>(object source) where T : new()
        {
            Type targetType = typeof(T);
            
            IMapDataSource dataSource = GetDataSource(source);

            T targetObject = new T(); //Activator.CreateInstance(targetType);

            if (targetObject != null && dataSource != null)
            {
                TypeDescriptor targetDescriptor = TypeDescriptorFactory.CreateTypeDescriptor(targetType) as TypeDescriptor;

                ArrayList dataDescList = GetTypeMappingList(targetDescriptor, dataSource);

                //The Source has to be passed as the last parameter in ToObjectInternal call!
                targetObject = ToObjectInternal(targetObject, dataSource, dataDescList, source);
            }

            return targetObject;
        }

        public static object ToObject(object source, object targetObject)
        {
            IMapDataSource dataSource = GetDataSource(source);
                        
            if (targetObject != null && dataSource != null)
            {
                if ((targetObject is IMapDataSource) && (((IMapDataSource)targetObject).IsCollection() == true))
                {
                    targetObject = ToObjectInternal(targetObject, dataSource, source, true);                    
                }
                else
                {
                    Type targetType = targetObject.GetType();

                    TypeDescriptor targetDescriptor = TypeDescriptorFactory.CreateTypeDescriptor(targetType) as TypeDescriptor;

                    ArrayList dataDescList = GetTypeMappingList(targetDescriptor, dataSource);

                    targetObject = ToObjectInternal(targetObject, dataSource, dataDescList, source);
                }
            }

            return targetObject;
        }

        public static object ToObject(object source, Type targetType)
        {
            IMapDataSource dataSource = GetDataSource(source);

            object targetObject = Activator.CreateInstance(targetType);

            if (targetObject != null && dataSource != null)
            {
                TypeDescriptor targetDescriptor = TypeDescriptorFactory.CreateTypeDescriptor(targetType) as TypeDescriptor;

                ArrayList dataDescList = GetTypeMappingList(targetDescriptor, dataSource);

                //The Source has to be passed as the last parameter in ToObjectInternal call!
                targetObject = ToObjectInternal(targetObject, dataSource, dataDescList, source);
            }

            return targetObject;
        }

        public static List<T> ToList<T>(IDataReader reader)
        {
            Type type = typeof(T);

            List<T> arrayList = new List<T>();

            ToList(reader, arrayList, type);

            return arrayList;
        }

        public static ArrayList ToList(IDataReader reader, Type type)
        {
            ArrayList arrayList = new ArrayList();

            ToList(reader, arrayList, type);

            return arrayList;
        }

        public static Hashtable ToDictionary(IDataReader reader, string keyFileName, Type type)
        {
            Hashtable ht = new Hashtable();
            ToDictionaryInternal(reader, keyFileName, ht, type);
            return ht;
        }

        public static SortedList ToSortedList(IDataReader reader, string keyFileName, Type type)
        {
            SortedList st = new SortedList();
            ToDictionaryInternal(reader, keyFileName, st, type);
            return st;
        }

        private static IDictionary ToDictionaryInternal(IDataReader reader, string keyFileName, IDictionary dictionary, Type targetType)
        {
            IMapDataSource dataSource = GetDataSource(reader);

            TypeDescriptor targetDescriptor = TypeDescriptorFactory.CreateTypeDescriptor(targetType) as TypeDescriptor;

            ArrayList dataDescList = GetTypeMappingList(targetDescriptor, dataSource);

            if (dictionary is ISupportInitialize)
                ((ISupportInitialize)dictionary).BeginInit();

            while (reader.Read())
            {
                object targetObject = Activator.CreateInstance(targetType);
                ToObjectInternal(targetObject, dataSource, dataDescList,null);
                if (targetObject != null)
                {
                    dictionary.Add(dataSource.GetFieldValue(keyFileName),targetObject);
                }
            }
            if (dictionary is ISupportInitialize)
                ((ISupportInitialize)dictionary).EndInit();

            return dictionary;
        }

        public static List<T> ToList<T>(IDataReader reader, List<T> list) where T : new()
        {
            Type type = typeof(T);
            return ToListInternal<T>(reader, list);
        }

        public static IList ToList(IDataReader reader,IList list, Type type)
        {
            return ToListInternal(reader, list, type);
        }

        private static List<T> ToListInternal<T>(IDataReader reader, List<T> list) where T : new()
        {
            Type targetType = typeof(T);

            IMapDataSource dataSource = GetDataSource(reader);

            TypeDescriptor targetDescriptor = TypeDescriptorFactory.CreateTypeDescriptor(targetType) as TypeDescriptor;

            ArrayList dataDescList = GetTypeMappingList(targetDescriptor, dataSource);

            if (list is ISupportInitialize)
                ((ISupportInitialize)list).BeginInit();

            while (reader.Read())
            {
                T targetObject = new T(); //(T)Activator.CreateInstance(targetType);
                ToObjectInternal(targetObject, dataSource, dataDescList, null);
                if (targetObject != null)
                {
                    list.Add(targetObject);
                }
            }
            if (list is ISupportInitialize)
                ((ISupportInitialize)list).EndInit();



            return list;
        }
        
        private static IList ToListInternal(IDataReader reader, IList list, Type targetType)
        {            
            IMapDataSource dataSource = GetDataSource(reader);

            TypeDescriptor targetDescriptor = TypeDescriptorFactory.CreateTypeDescriptor(targetType) as TypeDescriptor;

            ArrayList dataDescList = GetTypeMappingList(targetDescriptor, dataSource);

            if (list is ISupportInitialize)
                ((ISupportInitialize)list).BeginInit();

            while (reader.Read())
            {
                object targetObject = Activator.CreateInstance(targetType);
                ToObjectInternal(targetObject, dataSource, dataDescList,null);
                if (targetObject != null)
                {
                    list.Add(targetObject);
                }
            }
            if (list is ISupportInitialize)
                ((ISupportInitialize)list).EndInit();

        

            return list;
        }

        public static ArrayList ToList(DataTable table, Type type)
        {
            ArrayList arrayList = new ArrayList();

            ToList(table, arrayList, type);
            
            return arrayList;
        }

        public static IList ToList(DataTable table, IList list, Type type)
        {
            return ToListInternal(table, list, type);
        }

        private static IList ToListInternal(DataTable table, IList list, Type targetType)
        {
            IMapDataSource dataSource = GetDataSource(table);

            TypeDescriptor targetDescriptor = TypeDescriptorFactory.CreateTypeDescriptor(targetType) as TypeDescriptor;

            ArrayList dataDescList = GetTypeMappingList(targetDescriptor, dataSource);

            if (list is ISupportInitialize)
                ((ISupportInitialize)list).BeginInit();


            foreach (DataRow row in table.Rows)
            {
                ((DataRowReader)dataSource).DataRow = row;

                object targetObject = Activator.CreateInstance(targetType);

                ToObjectInternal(targetObject, dataSource, dataDescList,null);

                if (targetObject != null)
                {
                    list.Add(targetObject);
                }
            }
            if (list is ISupportInitialize)
                ((ISupportInitialize)list).EndInit();

            return list;
        }

        private static T ToObjectInternal<T>(T targetObject, IMapDataSource dataSource, ArrayList dataDescList, object objEntity)
        {
            if (targetObject != null && dataSource != null && dataDescList != null)
            {
                foreach (DataDescriptor dataDesc in dataDescList)
                {
                    object sourceData = dataSource.GetFieldValue(dataDesc.ColumnPosition,dataDesc.PropertyDescription, objEntity);
                    if (sourceData != null || dataDesc.PropertyDescription.IsEnum)
                    {
                        dataDesc.PropertyDescription.SetValue(targetObject, sourceData);
                    }
                }
            }
            return targetObject;
        }

        private static object ToObjectInternal(object targetObject, IMapDataSource dataSource, ArrayList dataDescList, object objEntity)
        {
            if (targetObject != null && dataSource != null && dataDescList != null)
            {
                foreach (DataDescriptor dataDesc in dataDescList)
                {
                    object sourceData = dataSource.GetFieldValue(dataDesc.ColumnPosition, dataDesc.PropertyDescription, objEntity);
                    if (sourceData != null || dataDesc.PropertyDescription.IsEnum)
                    {
                        dataDesc.PropertyDescription.SetValue(targetObject, sourceData);
                    }
                }
            }
            return targetObject;
        }

        private static object ToObjectInternal(object targetObject, IMapDataSource dataSource, object objEntity, bool isTargetCollection)
        {
            if (targetObject != null && dataSource != null && targetObject is IMapDataSource)
            {
                IMapDataReceiver targetDataSource = (IMapDataReceiver)targetObject;

                Type sourceType = objEntity.GetType();

                TypeDescriptor sourceDescriptor = TypeDescriptorFactory.CreateTypeDescriptor(sourceType) as TypeDescriptor;

                ArrayList propDescList = sourceDescriptor.PropInfoList; 

                int iCnt = 0;

                foreach (PropertyDescriptor propDesc in propDescList)
                {

                    object sourceData = dataSource.GetFieldValue(iCnt, propDesc, objEntity);
                    if (sourceData != null)
                    {
                        targetDataSource.SetFieldValue(iCnt++, propDesc.MappedDBName, null, sourceData);
                    }
                }
            }
            return targetObject;
        }
        
        private static ArrayList GetTypeMappingList(TypeDescriptor targetDescriptor, IMapDataSource dataSource)
        {
            ArrayList dataDescList = new ArrayList();
            if (dataSource != null)
            {
                for (int cnt = 0; cnt < dataSource.FieldCount; cnt++)
                {
                    string sourceColName = dataSource.GetFieldName(cnt);

                    PropertyDescriptor[] propDescList = null;

                    if ((dataSource is DataRowReader) || (dataSource is DataReaderSource) || 
                        ( (dataSource is IMapDataSource) && ((IMapDataSource)dataSource).IsCollection()) )
                    {
                        propDescList = targetDescriptor.GetOrdinalByDBColnName(sourceColName.ToUpper());
                    }
                    else
                    {
                        propDescList = targetDescriptor.GetOrdinalByPropName(sourceColName.ToUpper());
                    }
                    

                    if (propDescList != null && propDescList.Length > 0)
                    {
                        foreach (PropertyDescriptor desc in propDescList)
                        {
                            DataDescriptor dataDesc = new DataDescriptor(sourceColName.ToUpper(), cnt, desc);

                            dataDescList.Add(dataDesc);
                        }
                    }
                }
            }

            return dataDescList;
        }

        public static bool IsNull(object value)
        {
            return
                value == null ||
                value is string && ((string)value).TrimEnd(_trimArray).Length == 0 ||
                value is DateTime && ((DateTime)value) == DateTime.MinValue ||
                value is Int16 && ((Int16)value) == 0 ||
                value is Int32 && ((Int32)value) == 0 ||
                value is Int64 && ((Int64)value) == 0 ||
                value is double && ((double)value) == 0 ||
                value is float && ((float)value) == 0 ||
                value is decimal && ((decimal)value) == 0 ||
                value is Guid && ((Guid)value) == Guid.Empty;
        }

        public static object FromEnum(Enum enumValue)
        {
            EnumDescriptor enumTypeDescriptor = EnumDescriptorFactory.CreateEnumDescriptor(enumValue.GetType()) as EnumDescriptor;

            return enumTypeDescriptor.MapTo(enumValue);
        }

        public static object ToEnum(object sourceValue, Type type)
        {
            EnumDescriptor enumTypeDescriptor = EnumDescriptorFactory.CreateEnumDescriptor(type) as EnumDescriptor;
            return (Enum) enumTypeDescriptor.MapFrom(sourceValue);

        }

        // This function is to get value of object field.
        public static object GetFieldValue(object obj, string FieldName)
        {

            if (obj == null) { return null; }
            try
            {
                if (obj is DataRowView) { return (((DataRowView)obj)[FieldName]); }

                if (obj is DataRow) { return (((DataRow)obj)[FieldName]); }

                else
                {
                    if (obj is ValueType && obj.GetType().IsPrimitive)
                    {
                        return obj;
                    }
                    else
                    {
                        Type SourceType = obj.GetType();
                      
                        PropertyInfo prop = obj.GetType().GetProperty(FieldName);
                        if (prop == null || !prop.CanRead)
                        {
                            FieldInfo field = SourceType.GetField(FieldName);
                            if (field == null)
                            {
                                //return null
                                throw new Exception(FieldName + "  does not defined in object ");
                            }
                            else
                            {
                                return field.GetValue(obj);
                            }
                        }
                        else
                        {
                            return prop.GetValue(obj, null);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception(FieldName + "  is not defined in object ");
            }

        }

    }
}
