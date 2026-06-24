namespace ZhEaIsNsAaBn.Extensions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICacheHandler<T>
        where T : class, new()
    {
        bool HasValue { get; }

        void PrimaryKey(string propertyName);

        Task Add(T item);

        Task AddRange(IEnumerable<T> List);

        Task<List<T>> GetListOfItem(string Key);

        Task<List<T>> GetListOfItemWithRange(string Key, int From, int To);

        Task<List<string>> GetAllKeys();

        Task<Dictionary<string, List<T>>> GetAllCache();

        Task<T> GetLatestValue(string Key);

        Task ClearItemValues(string Key);

        Task ClearCach();
    }
}