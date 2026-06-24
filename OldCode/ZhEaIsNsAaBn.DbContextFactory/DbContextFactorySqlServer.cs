using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions.DbContextFactory
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    //public abstract class DbContextFactorySqlServer<T> : DbContextFactory<T>
    //    where T : DbContext, new()
    //{
    //    public DbContextFactorySqlServer(string connectionString)
    //        : base(options => options.UseSqlServer(connectionString))
    //    {
    //    }
    //    public DbContextFactorySqlServer(IConfigurationBuilder builder, string DatabaseName)
    //    {
    //        var configuration = builder.Build();
    //        var connectionString = configuration.GetConnectionString(DatabaseName);
    //        new DbContextFactorySqlServer(connectionString);
    //    }
    //}

    //public abstract class DbContextFactoryCreator<T>
    //    where T : DbContext, new(),
    //    \
    //{

    //}
}
