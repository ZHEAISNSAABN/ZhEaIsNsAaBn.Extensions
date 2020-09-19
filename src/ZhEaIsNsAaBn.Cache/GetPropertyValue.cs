using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Linq;
    using System.Reflection;

    internal class GetPropertyValue
    {
        public string Get<T>(
            ref T TObj,
            string PropertyName)
            where T : class, new()
        {
            return Convert.ToString(
                TObj.GetType().GetProperties().ToList().Find(PI => PI.Name == PropertyName).GetValue(TObj));
        }
    }
}
