using System;
using System.Collections;
using System.Collections.Generic;

namespace PadOS.SaveData.JsonDatastore
{
    public abstract class JsonTable : IEnumerable {
        protected List<object> _innerList = new List<object>();
        public bool HasChanged { get; protected set; }
        public string Name { get; set; }
        public bool LazyLoadData;
        public Func<List<object>> LoadData;

        protected void LazyLoad() {
            if (LazyLoadData) {
                _innerList = LoadData?.Invoke();
                if (_innerList == null)
                    _innerList = new List<object>();
                LazyLoadData = false;
            }
        }

        public void AddRange(IEnumerable<object> items) {
            LazyLoad();
            foreach (var item in items) {
                HasChanged = true;
                LazyLoadData = false;
                _innerList.Add(item);
            }
        }

        public void RemoveRange(IEnumerable<object> existing) {
            LazyLoad();
            foreach (var item in existing) {
                HasChanged = true;
                LazyLoadData = false;
                _innerList.Remove(item);
            }
        }

        public IEnumerator GetEnumerator() {
            LazyLoad();
            return _innerList.GetEnumerator();
        }

    }
    public class JsonTable<T> : JsonTable, IEnumerable<T> {
        public void AddRange(IEnumerable<T> items) {
            LazyLoad();
            foreach (var item in items) {
                HasChanged = true;
                LazyLoadData = false;
                _innerList.Add(item);
            }
        }

        public void RemoveRange(IEnumerable<T> existing) {
            LazyLoad();
            foreach (var item in existing) {
                HasChanged = true;
                LazyLoadData = false;
                _innerList.Remove(item);
            }
        }

        public new IEnumerator<T> GetEnumerator() {
            LazyLoad();
            foreach (var item in _innerList)
                yield return (T)item;
        }
    }
}