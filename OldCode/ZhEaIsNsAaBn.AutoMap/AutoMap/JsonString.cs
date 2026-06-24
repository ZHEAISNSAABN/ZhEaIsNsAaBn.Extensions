using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.IO;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    public partial class AutoMap
    {
        public async Task<T> JsonString<T>(
            string JSONString,
            string language = null,
            Func<JObject, T, object[], T> overrideFunc = null,
            params object[] paramsObject)
            where T : class, new()
        {

            var dictionaryTEntity = (await GetPropertiesName<T>()).propertiesDictionary;

            JObject JSONObject = JObject.Parse(JSONString);

            T tempObj = default;

            if (JSONObject.Count > 0)
            {
                foreach (var item in dictionaryTEntity)
                {
                    JToken Value;
                    if (JSONObject.TryGetValue(item.Key, out Value))
                    {
                        try
                        {

                            SetPropertyValue(ref tempObj, Value, item.Key);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                if (language != null
                    && (await GetPropertiesName<T>()).LocalizedPropertieDictionary.ContainsKey(language))
                {
                    List<string> localizedDictionary =
                        (await this.GetPropertiesName<T>()).LocalizedPropertieDictionary[language];


                    foreach (var Property in localizedDictionary)
                    {

                        JToken Value;
                        if (JSONObject.TryGetValue(Property,out Value)
                            && dictionaryTEntity.ContainsKey(Property + "_" + language))
                        {

                            try
                            {
                                var TToPropertyName = Property + "_" + language;
                                
                                SetPropertyValue(ref tempObj, Value, TToPropertyName);
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                    }

                }

                overrideFunc?.Invoke(JSONObject, tempObj, paramsObject);
            }

            return tempObj;
        }
    }
}
