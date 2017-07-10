using System;

namespace NailsFramework.UnitOfWork.Async
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAsyncAttribute : Attribute
    {
    }
}