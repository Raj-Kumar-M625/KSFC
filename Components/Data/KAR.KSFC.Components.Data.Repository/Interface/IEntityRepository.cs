using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Repository.Interface
{
    public interface IEntityRepository<T> where T : class 
    {
        /// <summary>
        /// Adds the Entity in database.
        /// </summary>
        /// <param name="entity"></param>   
        /// <param name="toekn"></param>
        /// <returns>instance of "Task{T}"</returns>
        Task<T> AddAsync(T entity, CancellationToken toekn);

        /// <summary>
        /// Adds the collection of the entities.
        /// </summary>
        /// <param name=""></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ICollection<T>> AddAsync(ICollection<T> entities, CancellationToken token);

        /// <summary>
        /// Deletes the specific entity logically.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> DeleteLogicalAsync(T entity, CancellationToken token);

        /// <summary>
        /// Deletes the specific entity Physically.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> DeletePhysicalAsync(T entity, CancellationToken token);

        /// <summary>
        /// Deletes the all entities matching the expression physically.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> DeletePhysicalByExpressionAsync(Expression<Func<T, bool>> predicate, CancellationToken token);

        /// <summary>
        /// Finds the first or default by its uniqueId.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<T> FirstOrDefaultByUniqueIdAsync(string UniqueId, CancellationToken token);

        /// <summary>
        /// Find First or Default by Expression .
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<T> FirstOrDefaultByExpressionAsync(Expression<Func<T, bool>> predicate, CancellationToken token);

        /// <summary>               
        /// Find First or Default by an ordered Expression .
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="orderBy"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<T> FirstOrDefaultByOrderExpressionAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, CancellationToken token);

        /// <summary>
        /// Find the entities using the given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<T>> FindByExpressionAsync(Expression<Func<T, bool>> predicate, CancellationToken token);

        /// <summary>
        /// containing all entities with matching properties
        /// </summary>
        /// <param name="token"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindByMatchingPropertiesAsync(CancellationToken token,
            Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// containing all entities with matching properties
        /// </summary>
        /// <param name="token"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<T> FindByFirstOrDefalutMatchingPropertiesAsync(CancellationToken token,
            Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Save all changes with underlying persistance asynchronously
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken token);

        /// <summary>
        /// Update the specific entity asynchronously
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity, CancellationToken token);



        /// <summary>
        /// Updates the collection of the entities.
        /// </summary>
        /// <param name=""></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ICollection<T>> UpdateAsync(ICollection<T> entities, CancellationToken token);

        /// <summary>
        /// Excutes sql command and returns result.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FromSql(string command, CancellationToken token);
        Task<T> FirstOrDefaultNoTrackingByExpressionAsync(Expression<Func<T, bool>> predicate, CancellationToken token);
    }
}
