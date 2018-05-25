using System;

namespace NailsFramework.UnitOfWork.Session
{
    public interface IExecutionContext : IDisposable
    {
        T GetObject<T>(string key);
        void SetObject(string key, object val);
        void RemoveObject(string key);
    }
}