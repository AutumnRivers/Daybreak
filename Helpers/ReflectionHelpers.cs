using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

using MPGraph;

namespace Daybreak_Midnight.Helpers
{
    internal static class ReflectionHelpers
    {
        private static readonly BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

        public static void SetPrivateField<T, T2>(this T item, string fieldName, T2 newValue)
        {
            var type = item.GetType();
            var field = type.GetField(fieldName, flags);
            field.SetValue(item, newValue);
        }

        public static void SetPrivateMPGraphNodeField<T, T2>(this T item, string fieldName, T2 newValue)
        {
            var type = item.GetType().BaseType.BaseType;
            var field = type.GetField(fieldName, flags);
            field.SetValue(item, newValue);
        }

        public static object GetPrivateProperty(this object item, string propName)
        {
            PropertyInfo prop = item.GetType().GetProperty(propName, flags);
            return prop.GetValue(item);
        }

        public static object GetPrivateField(this object item, string fieldName)
        {
            FieldInfo field = item.GetType().GetField(fieldName, flags);
            return field.GetValue(item);
        }

        public static object InvokePrivateMethod<T>(this T item, string methodName, object[] parameters)
        {
            MethodInfo method = item.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return method.Invoke(item, parameters);
        }
    }
}
