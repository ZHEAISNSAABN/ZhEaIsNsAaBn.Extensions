using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using Unity;

    public class UnityProvider : IGenericGetTypes
    {
        private readonly IUnityContainer container;

        public UnityProvider(IUnityContainer container)
        {
            this.container = container;
        }
        public void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            container.RegisterType<TFrom, TTo>();
        }

        public void RegisterSingleton<TFrom, TTo>() where TTo : TFrom
        {
            container.RegisterSingleton<TFrom, TTo>();
        }

        public void RegisterSingleton<T>()
        {
            container.RegisterSingleton<T>();
        }

        public void RegisterInstance<TInterface>(TInterface instance)
        {
            container.RegisterInstance<TInterface>(instance);
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
