using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Threading;

    using ZhEaIsNsAaBn.Exceptions;

    public partial class EntityChangeNotification<TEntity, TDbContext>
    {
        public delegate void CallbackEventHandlerExceptions(List<ExceptionData> ExceptionData, string ListenerID);
        public delegate void CallbackEventHandlerListOfTEntity(List<TEntity> List, string ListenerID);
        public delegate void CallbackEventHandlerListOfString(List<string> List, string ListenerID);
        public delegate void CallbackEventHandlerString(string String, string ListenerID);


        private event CallbackEventHandlerListOfTEntity _FirstLookCallBack;
        private event CallbackEventHandlerListOfTEntity _InsertedCallBack;
        private event CallbackEventHandlerListOfTEntity _UpdatedCallBack;
        private event CallbackEventHandlerListOfString _DeletedCallBack;
        private event CallbackEventHandlerExceptions _Exception;
        private event CallbackEventHandlerString _Status;

        public event CallbackEventHandlerListOfTEntity FirstLookCallBack { add => _FirstLookCallBack += value; remove => _FirstLookCallBack -= value; }

        public event CallbackEventHandlerListOfTEntity InsertedCallBack { add => _InsertedCallBack += value; remove => _InsertedCallBack -= value; }

        public event CallbackEventHandlerListOfTEntity UpdatedCallBack { add => _UpdatedCallBack += value; remove => _UpdatedCallBack -= value; }

        public event CallbackEventHandlerListOfString DeletedCallBack { add => _DeletedCallBack += value; remove => _DeletedCallBack -= value; }

        public event CallbackEventHandlerExceptions Exception { add => _Exception += value; remove => _Exception -= value; }

        public event CallbackEventHandlerString Status { add => _Status += value; remove => _Status -= value; }




        protected void OnStatus(string NewStatus, string ListenerID) => _Status?.Invoke(NewStatus, ListenerID);

        protected void OnThreadException(ThreadExceptionEventArgs e) => ThreadException?.Invoke(this, e);


        protected virtual void OnException(List<ExceptionData> exceptionData, string ListenerID) => _Exception?.Invoke(exceptionData, ListenerID);


    }
}
