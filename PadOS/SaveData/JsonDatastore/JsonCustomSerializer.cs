using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.SaveData.JsonDatastore
{
    public class JsonForeignKeySerializer {
        public string Serialize(JsonTable table) {
            var bindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
            var type = table.GetType().GetGenericArguments()[0];
            var properties = type
                .GetProperties(bindingFlags)
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
                        dict[property.PropertyInfo.Name + "_Id"] = foreignIdProperty.GetValue(propertyValue);
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
                        var value = (JValue)item[property.PropertyInfo.Name];
                        property.PropertyInfo.SetValue(newInstance, value.Value);
                    }
                    else {
                        
                    }
                }
                newList.Add(newInstance);
            }

            return newList;
        }
    }
}
