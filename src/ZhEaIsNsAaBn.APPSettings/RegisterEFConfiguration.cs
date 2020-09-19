using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using ZhEaIsNsAaBn.Base;

    public static class RegisterEFConfiguration
    {
        public static IConfigurationBuilder AddEFConfiguration<SettingsTable, TDbContext>(this IConfigurationBuilder configuration, string SettingSection = null)
            where TDbContext : DbContext, new()
            where SettingsTable : class, IEFConfigurationValue, new()
        {
            DbContextFactory<TDbContext> DbContextFactory;
            if (UnityContainerResolver.TryResolve(out DbContextFactory))
                configuration.Add(new EFConfigurationSource<SettingsTable, TDbContext>(DbContextFactory, SettingSection));

            return configuration;
        }
    }
}
