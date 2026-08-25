using System;
using System.ComponentModel;
using TSFramework.App.Processors;

namespace Cores.Base.Helpers
{
    public static class ExtendEnumHelper
    {
        public static T GetValueFromDesc<T>(string description)
        {
            foreach (var field in typeof(T).GetFields())
            {
                var attribute =
                    Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (AppProcessor.Messagor.GetMessage(attribute.Description) == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            //throw new ArgumentException("Not found.", nameof(description));
            return default;
        }

        public static T GetEnumValueFromDesc<T>(string description)
        {
            var fis = typeof(T).GetFields();

            foreach (var fi in fis)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0 &&
                    AppProcessor.Messagor.GetMessage(attributes[0]?.Description) == description)
                    return (T)Enum.Parse(typeof(T), fi.Name);
            }

            throw new Exception("Not found");
        }
    }
}