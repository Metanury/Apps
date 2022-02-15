using System;
using System.Reflection;

namespace Metanury.Apps.Core
{
    public class StringValueAttribute : Attribute
    {
        public string enumStringValue { get; set; }

        public StringValueAttribute(string Value) : base()
        {
            this.enumStringValue = Value;
        }

        public string Value { get { return enumStringValue; } }
    }

    public static class ExtendEnum
    {
        public static string GetStringValue<T>(this T enumValue) where T : struct
        {
            string result = String.Empty;

            Type type = enumValue.GetType();
            if (type.IsEnum)
            {
                FieldInfo fi = type.GetField(enumValue.ToString());
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                {
                    result = attrs[0].Value;
                }
            }

            return result;
        }
    }
}
