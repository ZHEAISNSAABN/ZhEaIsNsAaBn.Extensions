using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;


    // <typeparam name="SettingsModel">the settings after mapping</typeparam>

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="SettingsTable">the entity table in the database & must be inherit from <see cref="IEFConfigurationValue"></see></typeparam>
    /// <typeparam name="TDbContext">the DBContext to access the database</typeparam>
    public class EFConfigurationProvider<SettingsTable, TDbContext> : ConfigurationProvider
        where TDbContext : DbContext, new()
        where SettingsTable : class, IEFConfigurationValue, new()
        //where SettingsModel : class, new()
        
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
        /// <param name="ReloadOnChange">To can use this feature the <see cref="TDbContext"></see> had to be inherited from <see cref="DbContextNotifyPropertyChanged"></see> </param>
        /// <param name="ReloadDelay">the time will wait before get the changes from the Database</param>
        public EFConfigurationProvider(
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

            if (reloadOnChange && typeof(TDbContext).IsSubclassOf(typeof(DbContextNotifyPropertyChanged)))
            {
                FieldInfo field = typeof(TDbContext).GetField(nameof(DbContextNotifyPropertyChanged.NotifyPropertyChanged));
                ((DbContextNotifyPropertyChanged.EntityNotifyPropertyChanged)field.GetValue(null)).Changed +=
                    EntityNotifyPropertyChanged_Changed;
            }
                
        }

        private async void EntityNotifyPropertyChanged_Changed(object sender, EntityNotifyEventArgs e)
        {
            if(e.TableType != typeof(TDbContext))
                return;

            await Task.Delay(this.reloadDelay);

            this.Load();
        }

        public override void Load()
        {
            using (var context = dbContextFactory.GetContext())
            {
                Data = context.Set<SettingsTable>().AsNoTracking().ToDictionary(ST => ST.Key, ST => ST.Value);
            }
        }
    }
}
