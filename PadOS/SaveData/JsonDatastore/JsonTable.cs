using System;
using System.Collections;
using System.Collections.Generic;

namespace PadOS.SaveData.JsonDatastore
{
    public abstract class JsonTable : IEnumerable {
        protected List<object> _innerList = new List<object>();
        public bool HasChanged { get; protected set; }
        public string Name { get; set; }
        public List<object> Proxies { get; private set; } = new List<object>();

        public void UpdateOrInsert(object item) {
            var index = _innerList.IndexOf(item);
            if (index == -1) {
                var prop = item.GetType().GetProperty("Id");
                var idVal = prop.GetValue(item);
                if(((Int64)0).Equals(idVal)){ // Insert
                    _innerList.Add(item);
                    HasChanged = true;
                    return;
                }
                else { // Update
                    var row = _innerList.Find(p => prop.GetValue(p).Equals(idVal));
                    index = _innerList.IndexOf(row);
                }
            }
            if (index == -1) // Neither
                throw new Exception("Row not does not exist");

            _innerList[index] = item;
            HasChanged = true;
        }

        public void AddRange(IEnumerable<object> items) {
            foreach (var item in items) {
                HasChanged = true;
                _innerList.Add(item);
            }
        }

        public void RemoveRange(IEnumerable<object> existing) {
            foreach (var item in existing) {
                HasChanged = true;
                _innerList.Remove(item);
            }
        }

        public IEnumerator GetEnumerator() {
            return _innerList.GetEnumerator();
        }

        internal void SetList(List<object> list) {
            _innerList = list;
        }
    }
    public class JsonTable<T> : JsonTable, IEnumerable<T> {
        public void AddRange(IEnumerable<T> items) {
            foreach (var item in items) {
                HasChanged = true;
                _innerList.Add(item);
            }
        }

        public void RemoveRange(IEnumerable<T> existing) {
            foreach (var item in existing) {
                HasChanged = true;
                _innerList.Remove(item);
            }
        }

        public new IEnumerator<T> GetEnumerator() {
            foreach (var item in _innerList)
                yield return (T)item;
        }
    }
}