namespace ZhEaIsNsAaBn.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using System.Xml;

    public interface IAutoMap
    {
        Task<IEnumerable<TTo>> ListEntityToEntity<TFrom, TTo>(IEnumerable<TFrom> entityFirstsList,
                                                                                         string language = null,
                                                                                         Func<TFrom, TTo, object[], TTo> overrideFunc = null,
                                                                                         params object[] paramsObject)
            where TFrom : class, new()
            where TTo : class, new();

        Task<TTo> EntityToEntity<TFrom, TTo>(TFrom entity,
                                                                        string language = null,
                                                                        Func<TFrom, TTo, object[], TTo> overrideFunc = null,
                                                                        params object[] paramsObject)
            where TFrom : class, new()
            where TTo : class, new();

        Task<IEnumerable<T>> DataReader<T>(IDataReader reader,
                                         Func<IDataReader, T, object[], T> overrideFunc = null,
                                         params object[] paramsObject) where T : class, new();

        Task<IEnumerable<T>> XmlReader<T>(XmlReader reader) where T : class, new();

        Task<IEnumerable<T>> DataTable<T>(DataTable table,
                                          string language = null,
                                        Func<DataRow, T, object[], T> overrideFunc = null,
                                        params object[] paramsObject) where T : class, new();

        Task<T> DataRow<T>( DataRow row,
                            string language = null,
                          Func<DataRow, T, object[], T> overrideFunc = null,
                          params object[] paramsObject) where T : class, new();
    }
}