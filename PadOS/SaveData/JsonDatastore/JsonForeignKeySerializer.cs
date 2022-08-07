using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.SaveData.JsonDatastore
{
    public class JsonForeignKeySerializer {
        public string GetFieldName(System.Reflection.PropertyInfo property, System.Reflection.PropertyInfo foreignIdProperty)
            => property.Name + "_Fk" + foreignIdProperty.DeclaringType.Name;

        public string Serialize(JsonTable table) {
            var bindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
            var type = table.GetType().GetGenericArguments()[0];
            var properties = type
                .GetProperties(bindingFlags)
                .Where(p => p.GetCustomAttributes(true).Any(pp=>pp is Newtonsoft.Json.JsonIgnoreAttribute)==false)
                .Select(p => new {
                    PropertyInfo = p,
                    IsVirtual = type.GetMethod("get_"+p.Name).IsVirtual
                })
                .ToArray();

            var list = new List<Dictionary<string, object>>();
            foreach (var item in table.Cast<object>()) {
                var dict = new Dictionary<string, object>();
                foreach (var property in properties) {
                    if (property.IsVirtual == false)
                        dict[property.PropertyInfo.Name] = property.PropertyInfo.GetValue(item);
                    else {
                        var foreignIdProperty = property.PropertyInfo.PropertyType.GetProperty("Id", bindingFlags);
                        var propertyValue = property.PropertyInfo.GetValue(item);
                        var propName = GetFieldName(property.PropertyInfo, foreignIdProperty);
                        // property.PropertyInfo.Name + "_Fk" + property.PropertyInfo.PropertyType.Name
                        dict[propName] = foreignIdProperty.GetValue(propertyValue);
                    }
                }
                list.Add(dict);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
        }

        public object Deserialize(string text, JsonTable table) {
            var type = table.GetType().GetGenericArguments()[0];
            var bindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
            var properties = type
                .GetProperties(bindingFlags)
                .Where(p => p.GetCustomAttributes(true).Any(pp=>pp is Newtonsoft.Json.JsonIgnoreAttribute)==false)
                .Select(p => new {
                    PropertyInfo = p,
                    IsVirtual = type.GetMethod("get_" + p.Name).IsVirtual
                })
                .ToArray();

            var items = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(text);
            var newList = new List<object>();
            foreach (var item in items) {
                var newInstance = Activator.CreateInstance(type);
                foreach (var property in properties) {
                    if (property.IsVirtual == false) {
                        var jvalue = (JValue)item[property.PropertyInfo.Name];
                        if (jvalue == null)
                            continue;
                        object value;
                        if (jvalue.Type == JTokenType.Integer)
                            value = (Int32)(Int64)jvalue.Value;
                        else
                            value = jvalue.Value;
                        property.PropertyInfo.SetValue(newInstance, value);
                    }
                    else {
                        var proxy = Activator.CreateInstance(property.PropertyInfo.PropertyType);
                        property.PropertyInfo.SetValue(newInstance, proxy);
                        var idProperty = property.PropertyInfo.PropertyType.GetProperty("Id", bindingFlags);
                        var propName = GetFieldName(property.PropertyInfo, idProperty);
                        // property.PropertyInfo.Name + "_Fk" + property.PropertyInfo.PropertyType.Name
                        var jsonValue = (JValue)item[propName];
                        idProperty.SetValue(proxy, (Int32)(Int64)jsonValue.Value);
                        table.Proxies.Add(proxy);
                    }
                }
                newList.Add(newInstance);
            }

            return newList;
        }
    }
}
