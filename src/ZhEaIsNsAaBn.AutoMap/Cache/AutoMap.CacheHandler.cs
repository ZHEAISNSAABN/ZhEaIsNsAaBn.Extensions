using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Threading.Tasks;

    public partial class AutoMap
    {
        private readonly ISingleCacheHandler<AutoMapProperties> CachedAutoMapProperties;
        private async Task CachePropertiesName(AutoMapProperties autoMapProperties)
        {
            await CachedAutoMapProperties.Add(autoMapProperties);
        }
        private async Task<AutoMapProperties> GetCachedPropertiesName(string TypeName)
        {
            if (CachedAutoMapProperties.HasValue && CachedAutoMapProperties.ContainsKey(TypeName))
                return await CachedAutoMapProperties.GetItem(TypeName);

            return null;
        }

        private string TypeName<T>() => typeof(T).Name + "_AutoMap";
    }
}
