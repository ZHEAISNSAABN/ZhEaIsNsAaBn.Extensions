using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ZhEaIsNsAaBn.Extensions
{
    public abstract class DbContextFactory<T>
        where T : DbContext, new()
    {

        protected internal readonly DbContextOptionsBuilder<T> Options = new DbContextOptionsBuilder<T>();

        public DbContextFactory(Action<DbContextOptionsBuilder> _Options)
        {
             _Options.Invoke(Options);
        }

        public abstract T GetContext();
    }
}