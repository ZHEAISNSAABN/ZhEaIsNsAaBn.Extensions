using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Data.SqlClient;

    public partial class EntityChangeNotification<TEntity, TDbContext>
    {

        #region Dispose voides
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                SqlDependency.Stop(ConnectionStringText);

                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
