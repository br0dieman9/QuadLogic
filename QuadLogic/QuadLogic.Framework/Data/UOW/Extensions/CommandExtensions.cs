using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QuadLogic.Framework.Data.Mapping;

namespace QuadLogic.Framework.Data.UOW.Extensions
{
    public static class CommandExtensions
    {
        public static void AddParameter(this IDbCommand command, string name, object value)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (name == null) throw new ArgumentNullException("name");

            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            command.Parameters.Add(p);
        }

        public static List<T> ToList<T>(this IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            using (IDataReader reader = command.ExecuteReader())
            {
                return Map.ToList<T>(reader);
            }
        }

        public static T ToLists<T>(this IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            Type wrapperType = typeof(T);
            T wrapper = (T)Activator.CreateInstance(wrapperType);
            var wrapperProperties = wrapperType.GetProperties();

            using (IDataReader reader = command.ExecuteReader())
            {
                int i = 0;
                do
                {
                    PropertyInfo currentProp = wrapperProperties[i];

                    if (typeof(IList).IsAssignableFrom(currentProp.PropertyType) && currentProp.PropertyType.IsGenericType)
                    {
                        MethodInfo method = typeof(Map).GetMethod("ToList", new[] { typeof(IDataReader) });
                        //property of wrapper should be List<T>, get T from list to pass to mapping class
                        Type type = currentProp.PropertyType.GetGenericArguments()[0];
                        MethodInfo generic = method.MakeGenericMethod(type);

                        object[] mappingParams = new object[1];
                        mappingParams[0] = reader;
                        var mappingResult = generic.Invoke(new Map(), mappingParams);
                        currentProp.SetValue(wrapper, mappingResult, null);
                    }
                    else
                    {
                        Type type = currentProp.PropertyType;
                        
                        if (type.IsPrimitive || type.Equals(typeof(string)))
                        {
                            //simple property
                            MethodInfo method = typeof(Map).GetMethod("ToList", new[] { typeof(IDataReader) });
                            MethodInfo generic = method.MakeGenericMethod(wrapperType);
                            object[] mappingParams = new object[1];
                            mappingParams[0] = reader;
                            //this should return
                            var mappingResult = generic.Invoke(new Map(), mappingParams);
                            IEnumerable items = (IEnumerable)mappingResult;
                            object first = items.Cast<object>().FirstOrDefault();
                            var propValue = first.GetType().GetProperty(currentProp.Name).GetValue(first, null);
                            currentProp.SetValue(wrapper, propValue, null);
                        }
                        else
                        {
                            //complex object
                            MethodInfo method = typeof(Map).GetMethod("ToList", new[] { typeof(IDataReader) });
                            MethodInfo generic = method.MakeGenericMethod(type);
                            object[] mappingParams = new object[1];
                            mappingParams[0] = reader;
                            var mappingResult = generic.Invoke(new Map(), mappingParams);
                            IEnumerable items = (IEnumerable)mappingResult;
                            object first = items.Cast<object>().FirstOrDefault();
                            currentProp.SetValue(wrapper, first, null);
                        }                        
                    }
                    i++;
                }
                while (reader.NextResult());
            }

            return wrapper;
        }
    }
}

