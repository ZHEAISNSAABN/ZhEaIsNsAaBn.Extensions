using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using Unity;

    public static class RegisterCacheHandlerProvider
    {
        public static void AddCacheHandlerProvider(this IUnityContainer container)
        {
            container.RegisterSingleton<ICacheHandlerProvider, CacheHandlerProvider>();
            container.RegisterSingleton<PrimaryKeyHandler>();
        }
    }
}
