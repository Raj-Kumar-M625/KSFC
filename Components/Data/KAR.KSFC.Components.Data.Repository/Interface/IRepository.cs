using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        public void RemoveRange(IEnumerable<T> entity);
        void Update(T entity);
        //IQueryable<T> List(Expression<Func<T, bool>> expression);

        // IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate);

        // <summary>
        /// Get all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<T> Query(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        //T GetById(int id);
        //IEnumerable<T> GetAll();
        //IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        //void Add(T entity);
        //void AddRange(IEnumerable<T> entities);
        //void Remove(T entity);
        //void RemoveRange(IEnumerable<T> entities);

    }
}
