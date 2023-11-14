using KAR.KSFC.Components.Data.DatabaseContext;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Repository.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbFactory _dbFactory;
        private readonly IDisposable _dbdispose;

        public UnitOfWork(DbFactory dbFactory, IDisposable dbdispose)
        {
            _dbFactory = dbFactory;
            _dbdispose = dbdispose;
        }

        public async Task<int> CommitAsync(CancellationToken token)
        {
            _dbFactory.DbContext.ChangeTracker.Entries();
            int ret = await _dbFactory.DbContext.SaveChangesAsync(token);
            _dbdispose.Dispose();
            return ret;
        }
    }
}
