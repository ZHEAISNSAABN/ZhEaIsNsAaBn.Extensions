using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using System.Threading;

    using Microsoft.EntityFrameworkCore;

    using ZhEaIsNsAaBn.Exceptions;

    public partial class EntityChangeNotification<TEntity, TDbContext>
    {
        private readonly ITry @try;
        private readonly IAutoMap AutoMap;


        public event ThreadExceptionEventHandler ThreadException;

        internal bool Continue = true;
        internal Thread thread;

        internal SynchronizationContext context = new SynchronizationContext();
        internal DbContext _context;
        internal TEntity MyTable = new TEntity();
        internal Expression<Func<TEntity, bool>> _query;
        internal string _BeginStoredProcedure;
        internal List<TEntity> mylist;
        internal object Notify = new object();
        internal bool _FirstLook;
        internal bool _FirstLookDone = false;
        internal bool _Loop = false;
        private List<string> Listeners = new List<string>();


        public string ConnectionStringText { get => ConnectionStringBuilder.ConnectionString; set => ConnectionStringBuilder = new SqlConnectionStringBuilder(value + "; MultipleActiveResultSets = true"); }

        public SqlConnectionStringBuilder ConnectionStringBuilder { get; private set; }

        public int ConversationTimeout { get; set; }

        public ConnectionState State { get; private set; }

    }
}
