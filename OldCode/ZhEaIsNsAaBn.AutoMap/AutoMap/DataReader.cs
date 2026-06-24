using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public partial class AutoMap
    {
        public async Task<IEnumerable<T>> DataReader<T>(IDataReader reader,
                                                        Func<IDataReader, T, object[], T> overrideFunc = null,
                                                        params object[] paramsObject) where T : class, new()
        {
            List<T> Rows = new List<T>();
            Dictionary<string, string> dictionary;
            dictionary = (await GetPropertiesName<T>()).propertiesDictionary;

            while (reader.Read())
            {
                T tempObj = (T)Activator.CreateInstance(typeof(T));

                foreach (var key in dictionary.Keys)
                {
                    try
                    {
                        var TToPropertyName = dictionary[key];
                        var propertyValue = reader[key];

                        SetPropertyValue(ref tempObj, propertyValue, TToPropertyName);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                overrideFunc?.Invoke(reader, tempObj, paramsObject);
                Rows.Add(tempObj);
            }
            reader.Close();
            return Rows;
        }

    }
}
