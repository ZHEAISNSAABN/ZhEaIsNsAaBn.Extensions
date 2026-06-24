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

        public async Task<TTo> EntityToEntity<TFrom, TTo>(TFrom entity,
                                                                                     string language = null,
                                                                                     Func<TFrom, TTo, object[], TTo> overrideFunc = null,
                                                                                     params object[] paramsObject)
            where TFrom : class, new()
            where TTo : class, new()
        {
            var dictionaryTEntityFirst = (await GetPropertiesNameWithoutAttribute<TFrom>()).propertiesDictionary;


            var dictionaryTEntitySecond = (await GetPropertiesName<TTo>()).propertiesDictionary;


            TTo tempObj = (TTo)Activator.CreateInstance(typeof(TTo));


            foreach (var item in dictionaryTEntitySecond)
            {
                if (dictionaryTEntityFirst.ContainsKey(item.Key))
                {
                    try
                    {

                        var TFromPropertyName = item.Key;
                        var TToPropertyName = item.Value;
                        var propertyValue = entity.GetType().GetProperty(TFromPropertyName)?.GetValue(entity);

                        SetPropertyValue(ref tempObj, propertyValue, TToPropertyName);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            if (language != null
                && (await GetPropertiesName<TTo>()).LocalizedPropertieDictionary.ContainsKey(language))
            {
                List<string> localizedDictionary = (await this.GetPropertiesName<TTo>()).LocalizedPropertieDictionary[language];


                foreach (var Property in localizedDictionary)
                {

                    if (dictionaryTEntityFirst.ContainsKey(Property) && dictionaryTEntitySecond.ContainsKey(Property + "_" + language))
                    {

                        try
                        {
                            var TToPropertyName = Property + "_" + language;
                            var TFromPropertyName = dictionaryTEntitySecond[TToPropertyName];
                            var propertyValue = entity.GetType().GetProperty(TFromPropertyName)?.GetValue(entity);

                            SetPropertyValue(ref tempObj, propertyValue, TToPropertyName);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }

            }

            overrideFunc?.Invoke(entity, tempObj, paramsObject);

            return tempObj;
        }

    }
}
