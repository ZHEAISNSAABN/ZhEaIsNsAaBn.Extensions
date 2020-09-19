using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;


    internal class CacheHandler<T> : ICacheHandler<T>
        where T : class, new()
    {
        private readonly DeepClone DeepClone;

        private readonly PrimaryKeyHandler primaryKeyHandler;

        public CacheHandler(DeepClone deepClone, PrimaryKeyHandler primaryKeyHandler)
        {
            DeepClone = deepClone;
            this.primaryKeyHandler = primaryKeyHandler;
        }
        private readonly Dictionary<string, List<T>> _cachedItems = new Dictionary<string, List<T>>();
        private string _PrimaryKey;
        private readonly object LockCach = new object();
        public bool HasValue { get; private set; } = false;
        public void PrimaryKey(string PropertyName)
        {
            lock (this.LockCach)
            {

                if (PropertyName != _PrimaryKey)
                    ClearCach().GetAwaiter().GetResult();



                if (string.IsNullOrEmpty(PropertyName))
                    _PrimaryKey = nameof(IDynamicCache.Key);
                else
                    _PrimaryKey = PropertyName;

                //object TObj = new T();
                //List<PropertyInfo> PropertyInfoList = TObj.GetType().GetProperties().ToList();
                //this._PrimaryKey = PropertyInfoList.Find(PI => PI.Name == propertyName);

            }
        }
        public async Task Add(T item)
        {
            lock (this.LockCach)
            {
                HasValue = true;
                string Key = this.primaryKeyHandler.GetTheKey(item, this._PrimaryKey);
                if (ContainsKey(Key))
                    _cachedItems[Key].Insert(0, item);
                else
                    _cachedItems.Add(Key, new List<T>() { item });
            }
        }
        public async Task AddRange(IEnumerable<T> List)
        {
            foreach (var item in List)
                Add(item);
        }
        public async Task<List<T>> GetListOfItem(string Key)
        {
            if (HasValue)
                lock (this.LockCach)
                {
                    if (ContainsKey(Key))
                        return DeepClone.ListEntity(_cachedItems[Key]).ToList();
                }
            return null;
        }
        public async Task<List<T>> GetListOfItemWithRange(string Key, int From, int To)
        {
            if (HasValue)
                lock (this.LockCach)
                {
                    if (ContainsKey(Key))
                        return DeepClone.ListEntity(_cachedItems[Key].GetRange(From, To)).ToList();
                }
            return null;
        }
        public async Task<List<string>> GetAllKeys()
        {
            if (HasValue)
                lock (this.LockCach)
                {
                    return _cachedItems.Keys.ToList();
                }
            return null;
        }

        public async Task<List<T>> GetAllValues()
        {
            if (HasValue)
                lock (this.LockCach)
                {
                    return DeepClone.ListEntity(_cachedItems).ToList();
                }
            return null;
        }
        public async Task<Dictionary<string, List<T>>> GetAllCache()
        {
            if (HasValue)
                lock (this.LockCach)
                    return DeepClone.DictionaryEntity(_cachedItems);
                
            return null;
        }
        public async Task<T> GetLatestValue(string Key)
        {
            if (HasValue)
            {
                lock (this.LockCach)
                {
                    if (ContainsKey(Key))
                        return DeepClone.Entity(_cachedItems[Key].FirstOrDefault());
                }
            }
            return null;
        }
        public async Task ClearItemValues(string Key)
        {
            if (HasValue)
                lock (this.LockCach)
                {
                    if (ContainsKey(Key))
                        _cachedItems.Remove(Key);
                }
        }

        public bool ContainsKey(string Key) => HasValue && _cachedItems.ContainsKey(Key);
        public async Task ClearCach()
        {
            if (HasValue)
            {
                lock (this.LockCach)
                    _cachedItems.Clear();

                HasValue = false;
            }
        }
    }
}
