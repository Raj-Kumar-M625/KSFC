using Application.Interface.Persistence.Payment;
using Domain.Payment;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Payment
{
    public class QuarterlyTdsPaymentChallanRepository : GenericRepository<QuarterlyTdsPaymentChallan>, IQuarterlyTdsPaymentChallanRepository
    {
        private readonly AccountingDbContext _dbContext;

        public QuarterlyTdsPaymentChallanRepository(AccountingDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddQuarterlyTDSPaymentChallanAsync(QuarterlyTdsPaymentChallan quarterlyTdsPaymentChallan)
        {
            try
            {
                await _dbContext.QuarterlyTDSPaymentChallan.AddAsync(quarterlyTdsPaymentChallan);
                await _dbContext.SaveChangesAsync();
                return quarterlyTdsPaymentChallan.ID;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}", e.Message);
                return 0;
            }
        }

        public async Task<IQueryable<MappingTdsQuarterChallan>> GetMappingQuarterlyTDSPaymentChallanByQuarterChallanId(List<int> ids)
        {
            return _dbContext.MappingTDSQuarterChallan.Include(m => m.TDSPaymentChallan).Include(x => x.TDSPaymentChallan.TDSStatus).DefaultIfEmpty().Where(m => ids.Contains(m.QuarterlyTDSPaymentChallanID));
        }

        public async Task<MappingTdsQuarterChallan> GetMappingQuarterlyTDSPaymentChallanByTdsChallanId(int id)
        {
            return await _dbContext.MappingTDSQuarterChallan.Include(m => m.TDSPaymentChallan).DefaultIfEmpty().FirstOrDefaultAsync(m => m.TDSPaymentChallanID.Equals(id));
        }

        public async Task<IQueryable<QuarterlyTdsPaymentChallan>> GetQuarterlyTDSPaymentChallanAsync()
        {
            return _dbContext.QuarterlyTDSPaymentChallan.AsNoTracking().Include(q => q.QuarterMaster).Include(q => q.QuarterStatusMaster).Include(q => q.AssementYearMaster).DefaultIfEmpty();
        }

        public async Task<IQueryable<QuarterlyTdsPaymentChallan>> GetQuarterlyTDSPaymentChallanAsync(List<int> ids)
        {
            return _dbContext.QuarterlyTDSPaymentChallan.AsNoTracking().Include(q => q.QuarterMaster).Include(q => q.QuarterStatusMaster).Include(q=> q.AssementYearMaster).DefaultIfEmpty().Where(q => ids.Contains(q.ID));
        }

        public async Task MapQuarterlyTDSPaymentChallanAsync(List<int> tdsPaymentChallanIds, int quarterlyTdsPaymentChallanId)
        {
            try
            {
                List<MappingTdsQuarterChallan> mappingTDSQuarterChallans = new List<MappingTdsQuarterChallan>();
                tdsPaymentChallanIds.ForEach(item =>
                {
                    mappingTDSQuarterChallans.Add(new MappingTdsQuarterChallan() { TDSPaymentChallanID = item, QuarterlyTDSPaymentChallanID = quarterlyTdsPaymentChallanId });
                });
                await _dbContext.MappingTDSQuarterChallan.AddRangeAsync(mappingTDSQuarterChallans);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}", e.Message);
            }
        }

        public async Task<bool> UpdateQuarterlyTDSPaymentChallanAsync(List<QuarterlyTdsPaymentChallan> quarterlyTdsPaymentChallanList)
        {
            try
            {
                _dbContext.QuarterlyTDSPaymentChallan.UpdateRange(quarterlyTdsPaymentChallanList);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}", e.Message);
                return false;
            }
        }
    }
}
