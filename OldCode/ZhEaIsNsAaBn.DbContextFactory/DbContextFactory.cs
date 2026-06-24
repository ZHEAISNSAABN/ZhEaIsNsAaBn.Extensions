using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ZhEaIsNsAaBn.Extensions
{
    public abstract class DbContextFactory<T>
        where T : DbContext, new()
    {

        protected internal readonly Action<DbContextOptionsBuilder> Options;

        public DbContextFactory(Action<DbContextOptionsBuilder> _Options)
        {
             Options = _Options;
        }

        /// <summary>
        /// implement it just like this and it will work fine
        /// ↓   ↓   ↓   ↓   ↓   ↓   ↓   ↓   ↓   ↓   ↓   ↓
        /// var builder = new DbContextOptionsBuilder<<typeparam name="T"></typeparam>>();
        /// Options(builder);
        /// return new <typeparam name="T"></typeparam>(builder.Options);
        /// </summary>
        /// <returns type="DBcontext"> </returns>
        public abstract T GetContext();
    }
}