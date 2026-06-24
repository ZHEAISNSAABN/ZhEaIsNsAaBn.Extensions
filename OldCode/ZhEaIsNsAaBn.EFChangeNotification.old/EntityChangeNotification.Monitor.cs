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
    using System.Threading;
    using System.Threading.Tasks;

    using ZhEaIsNsAaBn.Exceptions;

    public partial class EntityChangeNotification<TEntity, TDbContext>
    {

        public void BeginMonitor(bool _FirstLook, string ident)
        {
            _FirstLook = (Listeners.Count() > 0) ? false : _FirstLook;
            string ListenerID = Guid.NewGuid().ToString();
            Listeners.Add(ListenerID);
            this.@try.Execute(() => MonitorThread(_FirstLook, ident, ListenerID), Retry.DefultTryNum.Infinity);
        }


        private async Task<bool> MonitorThread(bool _FirstLook, string ident, string ListenerID)
        {
            State = ConnectionState.Connected;
            try
            {
                if (_FirstLook)
                {
                    await @try.ExecuteAsync(() => FirstLook(ListenerID), -1);
                }
                MonitorLoop(ident, ListenerID);

                return true;
            }
            catch (Exception ex)
            {
                ThreadExceptionEventArgs e = new ThreadExceptionEventArgs(ex);
                OnThreadException(e);
            }
            return false;
        }

        private void MonitorLoop(string ident, string ListenerID)
        {

            while (!_FirstLookDone)
                Thread.Sleep(new TimeSpan(0, 0, 3));
            while (_Loop)
            {
                var task = @try.ExecuteAsync(async () =>
                    {
                        return GetNewData(ident, ListenerID);
                    }, Retry.DefultTryNum.Infinity);
                task.ContinueWith((t) =>
                    {
                        if (context != null)
                            context.Send(o => ParseNotifications(t.Result.Result, ListenerID), null);
                        else
                            ParseNotifications(t.Result.Result, ListenerID);
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
        }

        private List<SqlXml> GetNewData(string ident, string ListenerID)
        {
            var connection = new SqlConnection(ConnectionStringBuilder.ConnectionString);
            connection.InfoMessage += (object sender, SqlInfoMessageEventArgs e) =>
            {
                if (e.Message != null)
                    Console.WriteLine(e.Message);
            };
            connection.Open();

            int conversationTimeout = ConversationTimeout; // atomic pre-read of property value

            OnStatus(" Start Monitor " + DateTime.Now.TimeOfDay, ListenerID);
            var notifications = new List<SqlXml>();
            var command = new SqlCommand { CommandText = _BeginStoredProcedure, Connection = connection, CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@ident", ident);
            command.Parameters.AddWithValue("@conversationTimeout", conversationTimeout + 60);
            command.Parameters.Add(new SqlParameter { ParameterName = "@fullName", SqlDbType = SqlDbType.NVarChar, Size = 55, Direction = ParameterDirection.Output, Value = String.Empty });
            command.ExecuteNonQuery();
            string fullName = (string)command.Parameters[2].Value;
            string commandText = String.Format("WAITFOR ( receive cast(message_body as xml) FROM [dbo].[{0}]) ", fullName);
            string notificationOptions = String.Format("Service=[dbo].[{0}]; Local Database={1}", fullName, ConnectionStringBuilder.InitialCatalog);

            command = new SqlCommand(commandText, connection);
            var notificationRequest = new SqlNotificationRequest
            {
                UserData = ident,
                Options = notificationOptions,
                Timeout = conversationTimeout
            };
            command.Notification = notificationRequest;
            command.CommandTimeout = conversationTimeout + 15;
            using (var reader = command.ExecuteReader())
            {
                while (reader != null && reader.Read())
                {
                    notifications.Add(reader.GetSqlXml(0));
                }
                connection.Close();
                return notifications;
            }
            throw new Exception("No data here " + ListenerID);
        }

    }
}
