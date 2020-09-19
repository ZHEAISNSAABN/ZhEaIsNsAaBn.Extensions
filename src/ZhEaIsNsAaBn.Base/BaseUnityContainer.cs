using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Base
{
    using Unity;

    public class BaseUnityContainer
    {
        private static object ContainerLocker = new object();

        public static IUnityContainer Container { get; private set; } = new UnityContainer();

        public BaseUnityContainer(IUnityContainer container)
        {
            if (Container == null)
                lock (ContainerLocker)
                    if (Container == null)
                        Container = container;
        }
    }
}
