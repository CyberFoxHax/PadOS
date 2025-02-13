using System;
using System.Collections;
using System.Collections.Generic;

namespace PadOS.SaveData.JsonDatastore
{
    public abstract class JsonTable : IEnumerable, IDisposable {
        protected List<object> _innerList = new List<object>();
        public bool HasChanged { get; protected set; }
        public string Name { get; set; }
        public List<object> Proxies { get; private set; } = new List<object>();

        protected void UpdateOrInsert(object item) {
            var index = _innerList.IndexOf(item);
            /*if (index == -1 && item is IHasId hasId) {
                if (hasId.Id == 0) {
                    HasChanged = true;
                    DispatchChangedEvent();
                    return;
                }
                else {
                    var row = _innerList.Find(p => ((IHasId)p).Id == hasId.Id);
                    index = _innerList.IndexOf(row);
                }
            }*/
            if (index == -1) {
                var prop = item.GetType().GetProperty("Id");
                var idVal = prop.GetValue(item);
                if(((Int64)0).Equals(idVal)){ // Insert
                    _innerList.Add(item);
                    HasChanged = true;
                    DispatchChangedEvent();
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
                _innerList.Add(item);
            }
            HasChanged = true;
        }

        public void RemoveRange(IEnumerable<object> existing) {
            System.Reflection.PropertyInfo prop = null;
            foreach (var item in existing) {
                if(prop == null)
                    prop = item.GetType().GetProperty("Id");

                var idVal = prop.GetValue(item);
                _innerList.RemoveAll(p=>idVal.Equals(prop.GetValue(p)));
            }
            HasChanged = true;
        }

        public IEnumerator GetEnumerator() {
            return _innerList.GetEnumerator();
        }

        internal void SetList(List<object> list) {
            _innerList = list;
        }

        
        internal void DispatchChangedEvent() {
            var type = GetType();
            if (OnGlobalUnderlyingDataChanged.ContainsKey(type))
                OnGlobalUnderlyingDataChanged[type](this);
        }

        private static Dictionary<Type, Action<JsonTable>> OnGlobalUnderlyingDataChanged = new Dictionary<Type, Action<JsonTable>>();

        public event Action<JsonTable> OnUnderlyingDataChanged {
            add {
                var type = GetType();
                if(OnGlobalUnderlyingDataChanged.ContainsKey(type) == false)
                    OnGlobalUnderlyingDataChanged[type] = value;
                else
                    OnGlobalUnderlyingDataChanged[type] += value;
            }
            remove {
                var type = GetType();
                if (OnGlobalUnderlyingDataChanged.ContainsKey(type))
                    OnGlobalUnderlyingDataChanged[type] -= value;
            }
        }

        void IDisposable.Dispose(){
        }
    }
    public class JsonTable<T> : JsonTable, IEnumerable<T> {
        public void UpdateOrInsert(T item) {
            base.UpdateOrInsert(item);
        }

        public void UpdateOrInsert(IEnumerable<T> items) {
            foreach (var item in items)
                base.UpdateOrInsert(item);
        }

        public void AddRange(IEnumerable<T> items) {
            foreach (var item in items)
                _innerList.Add(item);
            HasChanged = true;
        }

        public void RemoveRange(IEnumerable<T> existing) {
            System.Reflection.PropertyInfo prop = null;
            foreach (var item in existing) {
                if(prop == null)
                    prop = item.GetType().GetProperty("Id");

                var idVal = prop.GetValue(item);
                _innerList.RemoveAll(p=>idVal.Equals(prop.GetValue(p)));
            }
            HasChanged = true;
        }

        public new IEnumerator<T> GetEnumerator() {
            foreach (var item in _innerList)
                yield return (T)item;
        }
    }
}