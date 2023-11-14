using Application.Interface.Persistence.TDS;
using Domain.TDS;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.TDS
{
    public class TdsStatusRepository : GenericRepository<TdsStatus>, ITdsStatusRepository
    {
        private readonly AccountingDbContext _dbContext;

        public TdsStatusRepository(AccountingDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<int> UpdateStatusAsync(IEnumerable<TdsStatus> statusList)
        {
            _dbContext.UpdateRange(statusList);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
