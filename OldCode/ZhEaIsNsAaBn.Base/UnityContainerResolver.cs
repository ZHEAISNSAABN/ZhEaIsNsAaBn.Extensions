using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Base
{
    using Unity;

    public static class UnityContainerResolver
    {
        public static IUnityContainer container { get; set; }


        public static bool TryResolve<T>(out T instance)
        {
            instance = default(T);
            if(container != null)
                try
                {
                    
                    instance = container.Resolve<T>();
                    return true;
                }
                catch
                {
                    
                }

            return false;
        }
    }
}
