using System;
using System.Threading.Tasks;

namespace Presentation.Services.Infra.Cache
{
    public interface ICacheManager
    {
        void RemoveGroup(Type group);
        void Remove(string key);
        void ChangeAction<T>(ChangeType chtype);
        T Get<T>(string key, Func<T> getFunc, Type group = null);
        Task<T> GetAsync<T>(string key, Func<Task<T>> getFunc, Type group);
    }
}