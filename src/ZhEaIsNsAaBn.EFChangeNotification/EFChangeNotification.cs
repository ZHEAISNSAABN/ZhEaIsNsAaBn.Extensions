using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Data;
    using System.Data.Sql;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;

    using Microsoft.EntityFrameworkCore;

    using ZhEaIsNsAaBn.Base;
    using ZhEaIsNsAaBn.Exceptions;

    /// <summary>
    /// provide you easy way to use service broker in SQL server.
    /// </summary>
    /// <typeparam name="TEntity"> the table will listen</typeparam>
    /// <typeparam name="TDbContext"> the DbContext that hold the table and the connectionString</typeparam>
    /// <returns></returns>
    public partial class EntityChangeNotification<TEntity, TDbContext>
        : IDisposable
        where TDbContext : DbContext, new()
        where TEntity : class, new()
    {
        #region Initializer
        public EntityChangeNotification(Expression<Func<TEntity, bool>> query, string BeginStoredProcedure)
        {
            TDbContext Context;
            UnityContainerResolver.TryResolve(out @try);
            UnityContainerResolver.TryResolve(out @AutoMap);
            UnityContainerResolver.TryResolve(out Context);

            if (Context != null)
                _context = Context;

            this.Initializer(query, BeginStoredProcedure);
        }
        public EntityChangeNotification(Expression<Func<TEntity, bool>> query, string BeginStoredProcedure, TDbContext Context)
        {
            UnityContainerResolver.TryResolve(out @try);
            UnityContainerResolver.TryResolve(out @AutoMap);

            _context = Context;

            this.Initializer(query, BeginStoredProcedure);
        }
        #endregion

        private void Initializer(Expression<Func<TEntity, bool>> query, string BeginStoredProcedure)
        {
            var start = this.@try.Execute(() =>
                {
                    ConversationTimeout = 600;
                    //_context = new TDbContext();
                    _query = query;
                    _BeginStoredProcedure = BeginStoredProcedure;
                    ConnectionStringText = _context.Database.GetDbConnection().ConnectionString;
                    SqlDependency sqlDependency = new SqlDependency();
                    SqlDependency.Start(ConnectionStringText);
                    return true;
                }, -1);
            if (!start.Result)
            {
                Continue = false;
                OnException(start.ExceptionData, null);
            }
        }



        private async Task<bool> FirstLook(string ListenerID)//, out SqlConnection  connection)
        {
            OnStatus(" Get last look " + DateTime.Now.TimeOfDay, ListenerID);
            
            var last = @try.Execute(() => _context.Set<TEntity>().Where(_query).ToList(), -1);
            if (last.Worked)
            {
                if (last.Result != null)
                {
                    @try.Execute(async () =>
                        {
                            _FirstLookCallBack.Invoke(last.Result, ListenerID);
                            return true;
                        }, -1);
                }
                _FirstLookDone = true;
                _Loop = true;

                // Notify listeners that data is ready
                OnStatus(" done " + DateTime.Now.TimeOfDay, ListenerID);
            }
            else
                OnException(last.ExceptionData, ListenerID);
            return false;
        }

        private void ParseNotifications(object state, string ListenerID)
        {
            if (state != null)
                foreach (SqlXml xml in (List<SqlXml>)state)
                {
                    XmlReader reader = xml.CreateReader();
                    while (reader.Read())
                    {

                        OnStatus(" New data is coming " + DateTime.Now.TimeOfDay, ListenerID);
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            lock (Notify)
                            {
                                switch (reader.Name)
                                {
                                    case "Inserted":
                                        {
                                            var MyList = (@AutoMap.XmlReader<TEntity>(reader).GetAwaiter().GetResult()).ToList();
                                            if (MyList.Count > 0)
                                                _InsertedCallBack(MyList, ListenerID);
                                        }
                                        break;
                                    case "Updated":
                                        {
                                            var MyList = (@AutoMap.XmlReader<TEntity>(reader).GetAwaiter().GetResult()).ToList();
                                            if (MyList.Count > 0)
                                                _UpdatedCallBack(MyList, ListenerID);
                                        }
                                        break;
                                    case "Deleted":
                                        {
                                            reader.Read();
                                            List<string> DeletedIDs = new List<string>();
                                            while (reader.IsStartElement())
                                            {
                                                DeletedIDs.Add(reader.ReadElementContentAsString());
                                            }
                                            _DeletedCallBack(DeletedIDs, ListenerID);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

        }

        public void Close()
        {
            SqlDependency.Stop(ConnectionStringText);
            _Loop = false;
            State = ConnectionState.Disconnected;
        }

    }
}
