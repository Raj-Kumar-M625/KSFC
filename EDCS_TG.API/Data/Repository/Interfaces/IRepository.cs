using System.Linq.Expressions;

namespace EDCS_TG.API.Data.Repository.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> FindAll();

        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>> cretiria, Expression<Func<T, object>> expression1);
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>> cretiria);
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, object>> cretiria);
        Task<T> FindById(Expression<Func<T, bool>> expression);

        Task<T> FindById(Guid id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
    }
}
