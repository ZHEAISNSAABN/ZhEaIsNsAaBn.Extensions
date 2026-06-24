using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Reflection;

    public partial class AutoMap
    {
        private void SetPropertyValue<TTo>(
            ref TTo tTo,
            Object propertyValue,
            string TToPropertyName)
            where TTo : class, new()
        {
            PropertyInfo propertyInfo;
            propertyInfo = tTo.GetType().GetProperty(TToPropertyName, BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                var propertyType = GetType(propertyInfo);
                if (propertyValue != null)
                    propertyInfo.SetValue(
                        tTo,
                        (propertyType.IsEnum)
                            ? Convert.ChangeType(Enum.Parse(propertyType, propertyValue.ToString()), propertyType)
                            : Convert.ChangeType(propertyValue, propertyType),
                        null);
            }
        }
    }
}
