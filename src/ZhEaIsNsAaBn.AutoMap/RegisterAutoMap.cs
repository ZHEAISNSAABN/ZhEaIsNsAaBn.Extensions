using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace ZhEaIsNsAaBn.Extensions
{
    using Microsoft.AspNetCore.Hosting;

    using Unity;

    public static class RegisterAutoMap
    {
        public static void AddAutoMap(this IUnityContainer container)
        {
            container.RegisterSingleton<IAutoMap, AutoMap>();
        }
    }
}
