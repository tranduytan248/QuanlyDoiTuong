using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TSFramework.Core.Utils
{
    public static class ObjectExtensions
    {
        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
                someObjectType
                    .GetProperty(item.Key)
                    ?.SetValue(someObject, item.Value, null);

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source,
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }

        public static Dictionary<string, object> ToDictionary(this object source,
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }

        public static Dictionary<string, object> ToDictionaryWithJson(this object source, string template = "",
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            var jsonString = JsonConvert.SerializeObject(source);
            var jsonObject = JObject.Parse(jsonString);

            return jsonObject
                .Descendants()
                .OfType<JValue>()
                .ToDictionary(p => string.IsNullOrEmpty(template) ? p.Path : string.Format(template, p.Path),
                    p => p.Value);
        }

        public static Dictionary<string, T> ToDictionary<T>(this object source,
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => (T)propInfo.GetValue(source, null)
            );
        }

        public static string ToQueryString(this object source)
        {
            var properties = JObject.FromObject(source)
                .Descendants()
                .OfType<JValue>()
                .Where(p => p.Value != null)
                .Select(p => $"{HttpUtility.UrlEncode(p.Path)}={HttpUtility.UrlEncode(p.Value?.ToString())}")
                .ToList();
            //.ToDictionary(jv => jv.Path, jv => jv.Value<object>());
            //var properties = from p in source?
            //        .GetType()
            //        .GetProperties()
            //    where p.GetValue(source, null) != null
            //    select $"{HttpUtility.UrlEncode(p.Name)}" +
            //           $"={HttpUtility.UrlEncode(p.GetValue(source)?.ToString())}";
            return string.Join("&", properties);
        }
    }
}