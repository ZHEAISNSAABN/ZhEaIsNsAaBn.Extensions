using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml;

    public partial class AutoMap
    {
        public async Task<IEnumerable<T>> XmlReader<T>(XmlReader reader) where T : class, new()
        {
            List<T> Rows = new List<T>();
            var dictionary = (await GetPropertiesName<T>()).propertiesDictionary;

            var list = new List<T>();
            reader.Read();

            T tempObj = (T)Activator.CreateInstance(typeof(T));
            while (reader.IsStartElement())
            {
                try
                {
                    var TToPropertyName = dictionary[reader.Name];
                    var propertyValue = reader.ReadElementContentAsString();

                    SetPropertyValue(ref tempObj, propertyValue, TToPropertyName);
                }
                catch (Exception ex)
                {

                }
            }
            Rows.Add(tempObj);
            reader.Close();
            return Rows;
        }

    }
}
