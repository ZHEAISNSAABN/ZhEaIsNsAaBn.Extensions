using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public abstract class DbContextNotifyPropertyChanged : DbContext
    {
        private static readonly Lazy<EntityNotifyPropertyChanged> lazy = new Lazy<EntityNotifyPropertyChanged>(() => new EntityNotifyPropertyChanged());

        public static EntityNotifyPropertyChanged NotifyPropertyChanged => lazy.Value;

        public class EntityNotifyPropertyChanged
        {   
            public event EventHandler<EntityNotifyEventArgs> Changed;

            public void OnChanged(EntityNotifyEventArgs e, [CallerMemberName] string Caller = null)
            {
                Task.Run(() => Changed?.Invoke(Caller, e));
            }

            internal EntityNotifyPropertyChanged() { }
        }
        public override int SaveChanges()
        {
            OnEntityChange();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            OnEntityChange();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void OnEntityChange()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(i => i.State == EntityState.Modified || i.State == EntityState.Added))
            {
                NotifyPropertyChanged.OnChanged(new EntityNotifyEventArgs(entry));
            }
        }

    }
}
