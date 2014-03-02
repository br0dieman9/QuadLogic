using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace QuadLogic.Framework.Data.Mapping
{
    public class TypeDescriptorFactory
    {
        private static Hashtable _typeDescriptorList = new Hashtable();
        private TypeDescriptorFactory()
        {            
        }

        public static IMapDataSource CreateTypeDescriptor(Type targetType)
        {
            IMapDataSource instance = null;
            lock(_typeDescriptorList)
            {
                instance = _typeDescriptorList[targetType.Name] as IMapDataSource;
                if(instance == null)
                {
                    instance = new TypeDescriptor(targetType) ;
                    if (instance != null)
                    {
                        _typeDescriptorList.Add(targetType.Name, instance);
                    }
                }
           }

            return instance;
        }
    }
}
