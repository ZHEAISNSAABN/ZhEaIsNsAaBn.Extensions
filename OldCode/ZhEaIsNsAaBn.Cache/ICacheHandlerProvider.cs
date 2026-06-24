namespace ZhEaIsNsAaBn.Extensions
{
    public interface ICacheHandlerProvider
    {
        ICacheHandler<T> GetCacheHandler<T>(string propertyName)
            where T : class, new();

        ISingleCacheHandler<T> GetSingleCacheHandler<T>(string propertyName)
            where T : class, new();
    }
}