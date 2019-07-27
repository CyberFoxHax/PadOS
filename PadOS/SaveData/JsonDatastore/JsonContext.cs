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

            foreach (var table in _tables) {
                table.LoadData = LoadData(table);
                table.LazyLoadData = true;
            }

            DirectoryName = directory;
        }

        private Func<List<object>> LoadData(JsonTable table) {
            return () => {
                var path = Path.Combine(DirectoryName, table.Name + ".json");
                if (File.Exists(path) == false)
                    return null;
                var text = File.ReadAllText(path);
                if (string.IsNullOrEmpty(text) || text == "[]")
                    return null;
                var typeArg = table.GetType().GetGenericArguments()[0];
                var genericType = typeof(List<>).MakeGenericType(typeArg);
                var methodDef = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast), BindingFlags.Static | BindingFlags.Public);
                var EnumerableCastObject = methodDef.MakeGenericMethod(typeof(object));
                var data = Deserialize(text, table);
                var typedResult = (IEnumerable<object>)EnumerableCastObject.Invoke(null, new object[] { data });
                var listResult = typedResult.ToList();
                return listResult;
            };
        }

        public Task SaveChangesAsync() {
            return Task.Run((Action)SaveChanges);
        }

        private void AutoIncrement(JsonTable table) {
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
                AutoIncrement(table);
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