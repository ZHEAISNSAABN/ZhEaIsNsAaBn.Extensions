using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public partial class AutoMap
    {
        public async Task<T> DataRow<T>( 
             DataRow row,
             string language = null,
             Func<DataRow, T, object[], T> overrideFunc = null,
             params object[] paramsObject) where T : class, new()
        {
            T MyTEntity = new T();

            var propertiesDictionary = (await GetPropertiesName<T>()).propertiesDictionary;

            foreach (DataColumn column in row.Table.Columns)
            {

                try
                {
                    var TToPropertyName = propertiesDictionary[column.ColumnName];
                    var propertyValue = row[column];

                    SetPropertyValue(ref MyTEntity, propertyValue, TToPropertyName);
                }
                catch (Exception ex)
                {

                }
            }


            if (language != null
                && (await GetPropertiesName<T>()).LocalizedPropertieDictionary.ContainsKey(language))
            {
                List<string> localizedDictionary = (await this.GetPropertiesName<T>()).LocalizedPropertieDictionary[language];


                foreach (var Property in localizedDictionary)
                {

                    if (row.Table.Columns.Contains(Property) && propertiesDictionary.ContainsKey(Property + "_" + language))
                    {

                        try
                        {
                            var TToPropertyName = Property + "_" + language;
                            var TFromPropertyName = propertiesDictionary[TToPropertyName];
                            var propertyValue = row[Property];

                            SetPropertyValue(ref MyTEntity, propertyValue, TToPropertyName);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }

            }

            overrideFunc?.Invoke(row, MyTEntity, paramsObject);

            return MyTEntity;
        }

    }
}
