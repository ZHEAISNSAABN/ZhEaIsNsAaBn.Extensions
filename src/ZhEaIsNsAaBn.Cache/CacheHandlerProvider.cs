using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using Unity;


    internal class CacheHandlerProvider : ICacheHandlerProvider
    {
        private readonly IUnityContainer container;

        public CacheHandlerProvider(IUnityContainer container)
        {
            this.container = container;
        }
        public ICacheHandler<T> GetCacheHandler<T>(string propertyName)
            where T : class, new()
        {
            ICacheHandler<T> returnHandler;
            try
            {

                returnHandler = container.Resolve<ICacheHandler<T>>();
                returnHandler.PrimaryKey(propertyName);
            }
            catch
            {
                container.RegisterSingleton<ICacheHandler<T>, CacheHandler<T>>();
                returnHandler = container.Resolve<ICacheHandler<T>>();
                returnHandler.PrimaryKey(propertyName);
            }

            return returnHandler;
        }
        public ISingleCacheHandler<T> GetSingleCacheHandler<T>(string propertyName)
            where T : class, new()
        {
            ISingleCacheHandler<T> returnHandler;
            try
            {

                returnHandler = container.Resolve<ISingleCacheHandler<T>>();
                returnHandler.PrimaryKey(propertyName);
            }
            catch
            {
                container.RegisterSingleton<ISingleCacheHandler<T>, SingleCacheHandler<T>>();
                returnHandler = container.Resolve<ISingleCacheHandler<T>>();
                returnHandler.PrimaryKey(propertyName);
            }

            return returnHandler;
        }
    }
}
