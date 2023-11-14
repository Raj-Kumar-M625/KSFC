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
    public class CommonMasterRepository:GenericRepository<CommonMaster>, ICommonMasterRepository
    {
        private readonly AccountingDbContext _dbContext;

        public CommonMasterRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<CommonMaster>> GetCommoMasterValues(string codeType)
        {
            try
            {

                var commonMaster = await _dbContext.CommonMaster.Where(x => x.CodeType == codeType && x.IsActive)
                                    .OrderBy(x => x.DisplaySequence)
                                    .ThenBy(x => x.CodeName).ToListAsync();

                return commonMaster;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ICollection<CommonMaster>> GetCommoMasterValues()
        {
            var res = await _dbContext.CommonMaster.Where(x => x.IsActive).OrderBy(x => x.DisplaySequence)
                                .ThenBy(x => x.CodeName).ToListAsync();
            return res;
        }
    }
}
