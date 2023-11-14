using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using EDCS_TG.API.Data.Repository.Interfaces;



namespace EDCS_TG.API.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected KarmaniDbContext _karmaniDbContext { get; set; }
        public Repository(KarmaniDbContext karmaniDbContext)
        {
            KarmaniDbContext = karmaniDbContext;
            _karmaniDbContext = karmaniDbContext;
        }
        protected KarmaniDbContext KarmaniDbContext { get; set; }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await KarmaniDbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>> criteria, Expression<Func<T, object>> expression1)
        {
            return await KarmaniDbContext.Set<T>().Where(expression).Include(expression1).Include(criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>> criteria)
        {
            return await KarmaniDbContext.Set<T>().Where(expression).Include(criteria).ToListAsync();
        }

        
        public async Task<T> Create(T entity)
        {
            var result = await KarmaniDbContext.Set<T>().AddAsync(entity);
            await KarmaniDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<T> Update(T entity)
        {
            try
            {
                KarmaniDbContext.Entry(entity).State = EntityState.Modified;
                await KarmaniDbContext.SaveChangesAsync();
                return entity;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<T> Delete(T entity)
        {
            KarmaniDbContext.Set<T>().Remove(entity);
            await KarmaniDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            var result = await KarmaniDbContext.Set<T>().Where(expression).ToListAsync();
            return result;
        }

        public async Task<T> FindById(Expression<Func<T, bool>> expression)
        {
            var result = await KarmaniDbContext.Set<T>().Where(expression).FirstAsync();
            return result;
        }

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, object>> cretiria)
        {
            return await KarmaniDbContext.Set<T>().Include(cretiria).ToListAsync();
        }

        
        public async Task<T> FindById(Guid id)
        {
            return await KarmaniDbContext.Set<T>().FindAsync(id);
        }
    }



}
