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

        public async Task<IEnumerable<T>> DataTable<T>(DataTable table,
                                                       string language = null,
                                                       Func<DataRow, T, object[], T> overrideFunc = null,
                                                       params object[] paramsObject) where T : class, new()
        {
            List<T> Rows = new List<T>();


            foreach (DataRow row in table.Rows)
            {
                Rows.Add(await DataRow<T>(row, language, overrideFunc, paramsObject));
            }

            return Rows;

        }

    }
}
