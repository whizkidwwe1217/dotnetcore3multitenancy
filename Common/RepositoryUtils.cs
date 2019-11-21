using System;
using System.IO;
using System.Linq;
using System.Text;
using HordeFlow.Models;
using HordeFlow.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace HordeFlow.Common
{
    public static class RepositoryUtils
    {
        public static object SerializeSelect(this object data, string fields)
        {
            return Serialize(data, fields);
        }

        public static object SerializeSelect<TEntity>(this object data, string fields)
        {
            return Serialize<TEntity>(data, fields);
        }

        public static object Serialize<TEntity>(object data, string fields)
        {
            return Serialize(data, fields).ToObject<TEntity>();
        }

        // Only supports 2nd level lowercasing. Needs to be recursive to support nested levels
        public static JObject Serialize(object data, string fields)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                var serializer = new JsonSerializer
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    //ContractResolver = new LowercaseContractResolver()
                };
                serializer.Converters.Add(new StringEnumConverter());
                serializer.Serialize(writer, data);
            }

            var field = fields.Split(',', StringSplitOptions.RemoveEmptyEntries);
            string[] fieldsArray = field.Select(e => e.Pascalize()).ToArray();
            var json = JObject.Parse(sb.ToString());
            var newJson = new JObject();

            foreach (var f in fieldsArray)
            {
                var value = json.SelectToken(f);
                var fieldName = ToLowerCamelCase(f, '.');

                if (value != null && value.Type == JTokenType.Object)
                {
                    var obj = new JObject();
                    var children = value.Children();
                    foreach (JProperty property in children)
                    {
                        obj.Add(ToLowerCamelCase(property.Name, '.'), property.Value);
                    }
                    newJson.Add(fieldName, obj);
                }
                if (value != null && value.Type == JTokenType.Array)
                {
                    var array = new JArray();
                    foreach (var arrayItem in value.Children())
                    {
                        var arrayItemObj = new JObject();
                        var arrayItemChildren = arrayItem.Children();
                        foreach (JProperty arrayItemProperty in arrayItemChildren)
                        {
                            arrayItemObj.Add(ToLowerCamelCase(arrayItemProperty.Name, '.'), arrayItemProperty.Value);
                        }
                        array.Add(arrayItemObj);
                    }
                    newJson.Add(fieldName, array);
                }
                else
                {
                    if (!newJson.ContainsKey(fieldName)) newJson.Add(fieldName, value);
                }
            }

            return newJson;
        }

        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }

        public static string ToLowerCamelCase(string path, char separator)
        {
            var names = path.Split(separator);
            StringBuilder sb = new StringBuilder();
            foreach (var name in names)
            {
                sb.Append(ToLowerCamelCase(name));
                sb.Append(separator);
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        public static string ToLowerCamelCase(string input)
        {
            string[] words = input.Split(' ');
            StringBuilder sb = new StringBuilder();
            foreach (string s in words)
            {
                string firstLetter = s.Substring(0, 1);
                string rest = s.Substring(1, s.Length - 1);
                sb.Append(firstLetter.ToLower() + rest);
                sb.Append(' ');

            }
            return sb.ToString().Substring(0, sb.ToString().Length - 1);
        }

        public static string ToCamelCase(string input)
        {
            string[] words = input.Split(' ');
            StringBuilder sb = new StringBuilder();
            foreach (string s in words)
            {
                string firstLetter = s.Substring(0, 1);
                string rest = s.Substring(1, s.Length - 1);
                sb.Append(firstLetter.ToUpper() + rest);
                sb.Append(' ');

            }
            return sb.ToString().Substring(0, sb.ToString().Length - 1);
        }

        public static Exception CreateInnerException<TEntity, TKey>(this BaseRepository<TKey, TEntity> repository, string message, Exception exception)
            where TEntity : class, IBaseEntity<TKey>, new()
        {
            return new Exception($"{message}. Error details: {exception.Message} {(exception.InnerException != null ? " " + exception.InnerException.Message : "")}");
        }
    }
}