using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    internal class SingleCacheHandler<T> : ISingleCacheHandler<T>
        where T : class, new()
    {
        private readonly DeepClone DeepClone;

        private readonly PrimaryKeyHandler primaryKeyHandler;

        public SingleCacheHandler(DeepClone deepClone, PrimaryKeyHandler primaryKeyHandler)
        {
            DeepClone = deepClone;
            this.primaryKeyHandler = primaryKeyHandler;
        }
        private Dictionary<string, T> CachedItems = new Dictionary<string, T>();
        private string _PrimaryKey;
        private readonly object LockCach = new object();
        public bool HasValue { get; private set; } = false;

        public void PrimaryKey(string PropertyName)
        {
            lock (LockCach)
            {
                if(PropertyName != _PrimaryKey)
                    ClearCach().GetAwaiter().GetResult();

                if (string.IsNullOrEmpty(PropertyName))
                    _PrimaryKey = nameof(IDynamicCache.Key);
                else
                    _PrimaryKey = PropertyName;

                //    object TObj = new T();
                //    List<PropertyInfo> propertyInfoList = TObj.GetType().GetProperties().ToList();
                //    _PrimaryKey = propertyInfoList.Find(PI => PI.Name == PropertyName);
            }
        }
        public async Task Add(T item)
        {
            lock (LockCach)
            {
                HasValue = true;
                string Key = this.primaryKeyHandler.GetTheKey(item, this._PrimaryKey);
                if (ContainsKey(Key))
                    CachedItems[Key] = item;
                else
                    CachedItems.Add(Key, item);
            }
        }
        public async Task AddRange(IEnumerable<T> List)
        {
            foreach (var item in List)
                Add(item);
        }
        public async Task<List<string>> GetAllKeys()
        {
            if (HasValue)
                lock (LockCach)
                {
                    return CachedItems.Keys.ToList();
                }
            return null;
        }


        public async Task<List<T>> GetAllValues()
        {
            if (HasValue)
                lock (LockCach)
                    return DeepClone.ListEntity(CachedItems).ToList();
            return null;
        }
        public async Task<Dictionary<string, T>> GetAllCache()
        {
            if (HasValue)
                lock (LockCach)
                    return DeepClone.DictionaryEntity(CachedItems);
            return null;
        }
        public async Task<T> GetItem(string Key)
        {
            if (HasValue)
            {
                lock (LockCach)
                {
                    if (ContainsKey(Key))
                        return DeepClone.Entity(CachedItems[Key]);
                }
            }
            return null;
        }

        public bool ContainsKey(string Key) => HasValue && CachedItems.ContainsKey(Key);

        public async Task ClearItemValues(string Key)
        {
            if (HasValue)
                lock (LockCach)
                {
                    if (ContainsKey(Key))
                        CachedItems.Remove(Key);
                }
        }
        public async Task ClearCach()
        {
            if (HasValue)
            {
                lock (LockCach)
                    CachedItems.Clear();

                HasValue = false;
            }
        }
    }
}
