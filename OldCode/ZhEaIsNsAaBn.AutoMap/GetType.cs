using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Reflection;

    public partial class AutoMap
    {
        private Type GetType(PropertyInfo PropertyInfo)
        {
            var t = PropertyInfo.PropertyType;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                return Nullable.GetUnderlyingType(t);
            }

            return PropertyInfo.PropertyType;
        }
    }
}
