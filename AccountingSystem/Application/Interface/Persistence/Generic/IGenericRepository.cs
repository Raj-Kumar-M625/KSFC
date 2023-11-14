using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int Id);
        Task<IReadOnlyList<T>> GetAll();
        Task<bool> Exists(int Id);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }
}
