using System;
using System.Collections;
using System.Text;

namespace QuadLogic.Framework.Data.Mapping
{
    internal class EnumDescriptorFactory
    {
        private static Hashtable _enumDescriptorList = new Hashtable();
        private EnumDescriptorFactory()
        {            
        }

        public static object CreateEnumDescriptor(Type targetType)
        {
            object instance = null;
            lock (_enumDescriptorList)
            {
                instance = _enumDescriptorList[targetType.Name];
                if(instance == null)
                {
                    instance = new EnumDescriptor(targetType) ;
                    if (instance != null)
                    {
                        _enumDescriptorList.Add(targetType.Name, instance);
                    }
                }
           }

            return instance;
        }
    }
}
