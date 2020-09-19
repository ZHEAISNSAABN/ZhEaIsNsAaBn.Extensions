namespace ZhEaIsNsAaBn.Extensions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISingleCacheHandler<T>
        where T : class, new()
    {
        bool HasValue { get; }

        void PrimaryKey(string PropertyName);

        Task Add(T item);

        Task AddRange(IEnumerable<T> List);

        Task<List<string>> GetAllKeys();

        Task<Dictionary<string, T>> GetAllCache();

        Task<T> GetItem(string Key);
        bool ContainsKey(string Key);

        Task ClearItemValues(string Key);

        Task ClearCach();
    }
}