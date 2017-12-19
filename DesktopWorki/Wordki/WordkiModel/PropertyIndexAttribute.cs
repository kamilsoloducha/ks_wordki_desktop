using System;
using System.Reflection;

namespace WordkiModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class PropertyIndexAttribute : Attribute
    {
        public int Index { get; private set; }
        public PropertyIndexAttribute(int index)
        {
            Index = index;
        }

        public static int GetPropertyIndex(Type type, string name)
        {
            PropertyInfo property = type.GetProperty(name);
            if (property == null)
            {
                return -1;
            }
            PropertyIndexAttribute attribute = property.GetCustomAttribute<PropertyIndexAttribute>();
            if (attribute == null)
            {
                return -1;
            }
            return attribute.Index;
        }
    }
}
