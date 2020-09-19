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
        public async Task<IEnumerable<TEntitySecond>> ListEntityToEntity<TEntityFirst, TEntitySecond>(IEnumerable<TEntityFirst> entityFirstsList,
                                                                                                      string language = null,
                                                                                                      Func<TEntityFirst, TEntitySecond, object[], TEntitySecond> overrideFunc = null,
                                                                                                      params object[] paramsObject)
            where TEntityFirst : class, new()
            where TEntitySecond : class, new()
        {

            List<TEntitySecond> entitySecondsList = new List<TEntitySecond>();
            foreach (var item in entityFirstsList)
            {
                var NewItem = await EntityToEntity(
                    item,
                    language,
                    overrideFunc,
                    paramsObject);

                if (NewItem != null)
                {
                    entitySecondsList.Add(NewItem);
                }
            }

            return entitySecondsList;
        }


    }
}
