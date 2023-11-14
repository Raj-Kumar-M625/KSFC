using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.Components.Data.DatabaseContext;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;

using Microsoft.EntityFrameworkCore;

namespace KAR.KSFC.Components.Data.Repository.Service
{
    public sealed class EntityRepository<T> : IEntityRepository<T> where T : class
    {
        /// <summary>
        /// The dbcontext
        /// </summary>
        /// 
        private readonly DbFactory _dbFactory;
        private DbSet<T> _dbSet;

        protected DbSet<T> DbSet
        {
            get => _dbSet ??= _dbFactory.DbContext.Set<T>();
        }
        public EntityRepository(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<T> AddAsync(T entity, CancellationToken token)
        {
            try
            {
                var newEntity = _dbFactory.DbContext.Entry(entity);
                _dbFactory.DbContext.Entry(entity).State = EntityState.Added;
                var t= await _dbFactory.DbContext.Set<T>().AddAsync(newEntity.Entity, token).ConfigureAwait(false);
                return newEntity.Entity;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<ICollection<T>> AddAsync(ICollection<T> entities, CancellationToken token)
        {
            try
            {
                var newEntities = new List<T>();
                foreach (var entity in entities)
                {
                    var newEntity = _dbFactory.DbContext.Entry(entity);

                    await _dbFactory.DbContext.Set<T>().AddAsync(newEntity.Entity,token).ConfigureAwait(false);

                 //   await SaveChangesAsync(token).ConfigureAwait(false);

                    newEntities.Add(entity);
                }
                return newEntities;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteLogicalAsync(T entity, CancellationToken token)
        {
            try
            {
                var newEntity = _dbFactory.DbContext.Entry(entity);
                newEntity.State = EntityState.Deleted;
                // return await SaveChangesAsync(token).ConfigureAwait(false);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeletePhysicalAsync(T entity, CancellationToken token)
        {
            try
            {
                var newEntity = _dbFactory.DbContext.Entry(entity);
                 _dbFactory.DbContext.Set<T>().Remove(entity);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeletePhysicalByExpressionAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            try
            {
                var result = await _dbFactory.DbContext.Set<T>().Where(predicate).ToListAsync(token).ConfigureAwait(false);
                _dbFactory.DbContext.Set<T>().RemoveRange(result);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> FindByMatchingPropertiesAsync(CancellationToken token,
            Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                var query = _dbFactory.DbContext.Set<T>().Where(predicate);
                includeProperties?.ToList().ForEach(table =>
                {
                    if (table != null)
                    {
                        query = includeProperties.Aggregate(
                            query, (current, expression) => current.Include(table));
                    }
                });
                return await query.AsNoTracking().ToListAsync<T>(token).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> FindByFirstOrDefalutMatchingPropertiesAsync(CancellationToken token,
            Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                var query = _dbFactory.DbContext.Set<T>().Where(predicate);
                includeProperties?.ToList().ForEach(table =>
                {
                    if (table != null)
                    {
                        query = includeProperties.Aggregate(
                            query, (current, expression) => current.Include(table));
                    }
                });
                return await query.AsNoTracking().FirstOrDefaultAsync<T>(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<T>> FindByExpressionAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            var result = await _dbFactory.DbContext.Set<T>().AsNoTracking().Where(predicate)
                                            .ToListAsync(token).ConfigureAwait(false);

            return result;
        }
        
        public async Task<T> FirstOrDefaultByExpressionAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            var result = await _dbFactory.DbContext.Set<T>().AsNoTracking().Where(predicate).FirstOrDefaultAsync(token).ConfigureAwait(false);
            return result;
        }
        public async Task<T> FirstOrDefaultNoTrackingByExpressionAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            var result = await _dbFactory.DbContext.Set<T>().Where(predicate).FirstOrDefaultAsync(token).ConfigureAwait(false);
            return result;
        }

        public async Task<T> FirstOrDefaultByOrderExpressionAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, CancellationToken token)
        {
            var result = await orderBy(_dbFactory.DbContext.Set<T>().AsNoTracking().Where(predicate)).FirstOrDefaultAsync(token).ConfigureAwait(false);
            return result;
        }

        public async Task<T> FirstOrDefaultByUniqueIdAsync(string UniqueId, CancellationToken token)
        {
            var result = await _dbFactory.DbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(token).ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<T>> FromSql(string command, CancellationToken token)
        {
            try
            {
                return await _dbFactory.DbContext.Set<T>().FromSqlRaw(command).ToListAsync(token).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken token)
        {
            try
            {
                var entries = _dbFactory.DbContext.ChangeTracker.Entries().Where(e => (e.Entity is EntityState.Added || e.State == EntityState.Modified));

                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        //((BaseEntity)entityEntry.Entity).IsDeleted = false;
                        //((BaseEntity)entityEntry.Entity).IsActive = true;
                        //((BaseEntity)entityEntry.Entity).UniqueId = Guid.NewGuid().ToString();
                        //((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                    }
                    else
                    {
                      //  ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;
                    }
                }

                return await _dbFactory.DbContext.SaveChangesAsync(token).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken token)
        {
            try
            {
                var entry = _dbFactory.DbContext.Entry(entity);
                entry.State = EntityState.Modified;

                //await SaveChangesAsync(token).ConfigureAwait(false);

                return entity;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ICollection<T>> UpdateAsync(ICollection<T> entities, CancellationToken token)
        {
            try
            {
                var newEntities = new List<T>();
                foreach (var entity in entities)
                {
                    var newEntity = _dbFactory.DbContext.Entry(entity);
                    newEntity.State = EntityState.Modified;
                }
                return newEntities;
            }
            catch
            {
                throw;
            }
        }
    }
}
