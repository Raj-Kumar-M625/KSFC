using Application.Interface.Persistence.Master;
using Domain.Master;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Master
{
    public class BankDetailsRepository : GenericRepository<BankDetails>, IBankDetailsRepositoty
    {
        private readonly AccountingDbContext _dbContext;
        public BankDetailsRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<BranchMaster>> GetBankDetailsList()
        {
            var result = from b in _dbContext.BankDetails
                         join br in _dbContext.BranchMaster on b.Id equals br.branch_bank_id into branchGroup
                         from br in branchGroup.DefaultIfEmpty()
                         where b.bank_is_active == true
                         orderby b.BankName

                         select new BranchMaster
                         {
                             branch_id = br.branch_id,
                             branch_ifsc = br.branch_ifsc,
                             branch_name = br.branch_name,
                             BankDetails = b

                         };
            return await result.ToListAsync();
        }

        public async Task<List<BranchMaster>> GetBranchDetails(int Id)
        {
            var payments = _dbContext.BranchMaster.Where(o => o.branch_bank_id == Id).
                Include(o => o.BankDetails).ToList();
            return payments;
        }
    }
}
