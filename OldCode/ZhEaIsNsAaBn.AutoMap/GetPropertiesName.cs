using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public partial class AutoMap
    {
        internal async Task<AutoMapProperties> GetPropertiesName<TEntity>() //where TEntity : class, new()
        {
            var TypeName = TypeName<TEntity>();
            AutoMapProperties returnAutoMapProperties = await GetCachedPropertiesName(TypeName);

            if (returnAutoMapProperties == null)
            {
                returnAutoMapProperties = new AutoMapProperties();

                returnAutoMapProperties.TypeName = TypeName;

                var Properties = typeof(TEntity).GetProperties();
                foreach (var property in Properties)
                {
                    string Value = property.Name;
                    var autoMapAttributes = property.GetCustomAttributes<AutoMapAttribute>().ToList();
                    if (autoMapAttributes.Any())
                    {
                        if (autoMapAttributes.Count == 1)
                        {
                            var autoMapAttribute = autoMapAttributes[0];
                            GetFromAutoMapAttribute(autoMapAttribute, Value, ref returnAutoMapProperties);
                        }
                        else
                            foreach (var autoMapAttribute in autoMapAttributes)
                            {
                                GetFromAutoMapAttribute(autoMapAttribute, Value, ref returnAutoMapProperties);
                            }
                    }
                    else if (property.GetCustomAttribute<ColumnAttribute>() != null)
                    {
                        returnAutoMapProperties.propertiesDictionary.Add(
                            property.GetCustomAttribute<ColumnAttribute>().Name,
                            Value);
                    }
                    else
                        returnAutoMapProperties.propertiesDictionary.Add(Value, Value);
                }
            }

            return  returnAutoMapProperties;
        }

        internal async Task<AutoMapProperties> GetPropertiesNameWithoutAttribute<TEntity>()
        {

            var TypeName = TypeName<TEntity>();
            AutoMapProperties returnAutoMapProperties = await GetCachedPropertiesName(TypeName);

            if (returnAutoMapProperties == null)
            {
                returnAutoMapProperties = new AutoMapProperties();

                returnAutoMapProperties.TypeName = TypeName;

                var Properties = typeof(TEntity).GetProperties();
                foreach (var property in Properties)
                {
                    string Value = property.Name;
                    returnAutoMapProperties.propertiesDictionary.Add(Value, Value);
                }
            }

            return returnAutoMapProperties;
        }

        internal void GetFromAutoMapAttribute(AutoMapAttribute autoMapAttribute, string Value, ref AutoMapProperties returnAutoMapProperties)
        {
            string Key;
            if (autoMapAttribute.Language != null)
            {
                Key = autoMapAttribute.Name + "_" + autoMapAttribute.Language;

                returnAutoMapProperties.LocalizedPropertieDictionary.Add(autoMapAttribute.Language, autoMapAttribute.Name);
            }
            else
                Key = autoMapAttribute.Name;

            returnAutoMapProperties.propertiesDictionary.Add(Key, Value);
        }
    }
}
