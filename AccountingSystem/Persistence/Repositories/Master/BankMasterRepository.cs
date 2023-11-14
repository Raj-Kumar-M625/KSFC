using Application.Interface.Persistence.Master;
using Domain.Master;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Master
{
    public class BankMasterRepository : GenericRepository<BankMaster>, IBankMasterRepository
    {
        private readonly AccountingDbContext _dbContext;

        public BankMasterRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<BankMaster>> GetBankMasterList()
        {
            var banklist = await _dbContext.BankMaster.Where(x => x.Status)
                                .OrderBy(x => x.BankName).ToListAsync();
            return banklist;
        }
        public async Task<ICollection<BankMaster>> GetAllBanks()
        {
            var banklist = await _dbContext.BankMaster.Where(x => x.Status==false)
                                .OrderBy(x => x.BankName).ToListAsync();
            return banklist.DistinctBy(x=>x.BankName).ToList();
        }


    }
}
