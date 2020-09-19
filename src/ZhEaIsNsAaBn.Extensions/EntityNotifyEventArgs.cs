namespace ZhEaIsNsAaBn.Extensions
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public class EntityNotifyEventArgs : EventArgs
    {
        public Type TableType { get; set; }
        public EntityState State { get; set; }

        public EntityNotifyEventArgs(EntityEntry Entry)
        {
            this.TableType = Entry.Entity.GetType();
            this.State = Entry.State;
        }
    }
}