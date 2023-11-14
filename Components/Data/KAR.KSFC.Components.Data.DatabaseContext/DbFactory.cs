
using KAR.KSFC.Components.Data.Models.DbModels;

using Microsoft.EntityFrameworkCore;

using System;

namespace KAR.KSFC.Components.Data.DatabaseContext
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private readonly Func<ksfccsgContext> _instanceFunc;
        private DbContext _dbContext;
        public DbContext DbContext => _dbContext ??= _instanceFunc.Invoke();

        public DbFactory(Func<ksfccsgContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}
