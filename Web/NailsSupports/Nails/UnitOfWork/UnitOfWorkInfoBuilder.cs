using System;
using System.Reflection;
using NailsFramework.Support;
using NailsFramework.UnitOfWork.Async;

namespace NailsFramework.UnitOfWork
{
    public class UnitOfWorkInfoBuilder
    {
        public ICustomAttributeProvider CustomAttributeProvider { get; set; }
        public Func<string> GetUowName { get; set; }
        public bool AllowAsync { get; set; }

        public UnitOfWorkInfo Build()
        {
            var attribute = CustomAttributeProvider.Attribute<UnitOfWorkAttribute>();

            var name = attribute != null && !string.IsNullOrWhiteSpace(attribute.Name)
                           ? attribute.Name
                           : GetUowName();

            var allow = CustomAttributeProvider.Attribute<AllowAsyncAttribute>();

            var notAllow = CustomAttributeProvider.Attribute<NotAsyncAttribute>();

            var async = Nails.Configuration.DefaultAsyncMode;

            if (Nails.Configuration.AllowAsyncExecution)
            {
                if (!AllowAsync)
                {
                    //only void methods can be invoked Async automatically.
                    async = false;
                }
                else
                {
                    //negotiate with the default value.
                    if (allow != null)
                    {
                        async = true;
                    }
                    else if (notAllow != null)
                    {
                        async = false;
                    }
                }
            }
            else
            {
                async = false;
            }


            var transactionMode = attribute != null
                                      ? attribute.TransactionMode
                                      : Nails.Configuration.DefaultTransactionMode;
            
            var uowinfo = new UnitOfWorkInfo(isTransactional: transactionMode != TransactionMode.NoTransaction,
                                             isAsync: async);

            return uowinfo;
        }
    }
}