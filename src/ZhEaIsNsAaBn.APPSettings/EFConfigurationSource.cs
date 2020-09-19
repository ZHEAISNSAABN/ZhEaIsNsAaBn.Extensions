using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class EFConfigurationSource<SettingsTable, TDbContext> : IConfigurationSource
        where TDbContext : DbContext, new()
        where SettingsTable : class, IEFConfigurationValue, new()
    {
        private readonly DbContextFactory<TDbContext> dbContextFactory;

        private readonly string settingSection;

        private readonly bool reloadOnChange;

        // Number of milliseconds that reload will wait before calling Load. This helps avoid triggering a reload before a change is completely saved. Default is 500.
        private readonly int reloadDelay;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DbContextFactory"></param>
        /// <param name="SettingSection">Default Value <see cref="SettingsTable"></see> Name</param>
        public EFConfigurationSource(
            DbContextFactory<TDbContext> DbContextFactory,
            string SettingSection = null,
            bool ReloadOnChange = false,
            int ReloadDelay = 500)
        {
            this.dbContextFactory = DbContextFactory;
            this.reloadOnChange = ReloadOnChange;
            this.reloadDelay = ReloadDelay;

            if (string.IsNullOrEmpty(SettingSection))
                this.settingSection = typeof(SettingsTable).Name;
            else
                this.settingSection = SettingSection;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EFConfigurationProvider<SettingsTable, TDbContext>(dbContextFactory, settingSection);
        }
    }
}
