using System;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.ComponentModel.DataAnnotations.Schema;

    using ZhEaIsNsAaBn.Exceptions;

    public partial class AutoMap : IAutoMap
    {
        private readonly ITry @try;
        public AutoMap(ITry _Try, ICacheHandlerProvider cacheHandlerProvider)
        {
            this.@try = _Try;
            this.CachedAutoMapProperties =
                cacheHandlerProvider.GetSingleCacheHandler<AutoMapProperties>(nameof(AutoMapProperties.TypeName));
        }

    }
}
