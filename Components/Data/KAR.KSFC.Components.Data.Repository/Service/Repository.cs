using KAR.KSFC.Components.Data.DatabaseContext;
using KAR.KSFC.Components.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Repository.Service
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbFactory _dbFactory;
        private DbSet<T> _dbSet;

        protected DbSet<T> DbSet
        {
            get => _dbSet ??= _dbFactory.DbContext.Set<T>();
        }

        public Repository(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public void Add(T entity)
        {
            _dbFactory.DbContext.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbFactory.DbContext.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbFactory.DbContext.RemoveRange(entity);
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
