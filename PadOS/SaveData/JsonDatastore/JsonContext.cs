using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Text;

namespace PadOS.SaveData.JsonDatastore
{
    public abstract class JsonContext : IDisposable {
        private JsonTable[] _tables;
        public string DirectoryName { get; private set; }

        public JsonContext(string directory) {
            _tables =  GetType()
                .GetProperties()
                .Where(p => typeof(JsonTable).IsAssignableFrom(p.PropertyType))
                .Select(p=> {
                    var instance = (JsonTable)Activator.CreateInstance(p.PropertyType);
                    p.SetValue(this, instance);
                    instance.Name = p.Name;
                    return instance;
                })
                .ToArray();

            DirectoryName = directory;
            LoadData();
        }

        private void LoadData() {
            foreach (var table in _tables) {
                var path = Path.Combine(DirectoryName, table.Name + ".json");
                if (File.Exists(path) == false)
                    return;
                var text = File.ReadAllText(path);
                if (string.IsNullOrEmpty(text) || text == "[]")
                    return;
                var typeArg = table.GetType().GetGenericArguments()[0];
                var genericType = typeof(List<>).MakeGenericType(typeArg);
                var methodDef = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast), BindingFlags.Static | BindingFlags.Public);
                var EnumerableCastObject = methodDef.MakeGenericMethod(typeof(object));
                var data = Deserialize(text, table);
                var typedResult = (IEnumerable<object>)EnumerableCastObject.Invoke(null, new object[] { data });
                var listResult = typedResult.ToList();
                table.SetList(listResult);
            }

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            foreach (var table in _tables) {
                var typeArg = table.GetType().GetGenericArguments()[0];
                foreach (var item in table) {
                    var properties = typeArg.GetProperties(bindingFlags);
                    foreach (var property in properties) {
                        var value = property.GetValue(item);
                        var proxy = table.Proxies.FirstOrDefault(p => ReferenceEquals(value, p));
                        if(proxy == null)
                            continue;
                        var idProperty = property.PropertyType.GetProperty("Id", bindingFlags);
                        var proxyId = Convert.ToInt32(idProperty.GetValue(proxy));
                        if (proxy != null) {
                            var foreignTable = _tables
                                .Where(p => p.GetType().GetGenericArguments()[0] == property.PropertyType)
                                .First();
                            var foreignRow = foreignTable
                                .Cast<object>()
                                .Where(p => Convert.ToInt32(idProperty.GetValue(p)) == proxyId)
                                .First();

                            property.SetValue(item, foreignRow);
                        }
                    }
                }
                table.Proxies.Clear();
            }
        }

        public Task SaveChangesAsync() {
            return Task.Run((Action)SaveChanges);
        }

        private void AutoIncrement<T>(IEnumerable<T> table) {
            var type = table.GetType().GetGenericArguments()[0];
            var idProperty = type.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
            var maxId = table.Cast<object>().Max(p => (Int64)idProperty.GetValue(p));
            if (maxId == 0)
                maxId = 1;
            var noIdElements = table.Cast<object>().Where(p => (Int64)idProperty.GetValue(p) == 0);
            foreach (var item in noIdElements) 
                idProperty.SetValue(item, maxId++);
        }

        public void SaveChanges() {
            foreach (var table in _tables) {
                if (table.HasChanged == false)
                    continue;
                AutoIncrement(table.Cast<object>());
                var serialized = Serialize(table);
                File.WriteAllText(Path.Combine(DirectoryName, table.Name + ".json"), serialized, Encoding.UTF8);
            }
        }

        private string Serialize(JsonTable table) {
            return new JsonForeignKeySerializer().Serialize(table);
        }

        private object Deserialize(string text, JsonTable table) {
            return new JsonForeignKeySerializer().Deserialize(text, table);
        }

        public void Dispose() {
            
        }
    }
}