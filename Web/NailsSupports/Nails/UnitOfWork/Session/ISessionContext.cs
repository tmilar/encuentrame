using System;

namespace NailsFramework.UnitOfWork.Session
{
    public interface ISessionContext : IDisposable
    {
        T GetObject<T>(string key);
        void SetObject(string key, object val);
    }
}