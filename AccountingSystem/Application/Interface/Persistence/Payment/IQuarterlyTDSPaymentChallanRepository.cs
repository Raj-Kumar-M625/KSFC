using Application.Interface.Persistence.Generic;
using Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Payment
{
    public interface IQuarterlyTdsPaymentChallanRepository: IGenericRepository<QuarterlyTdsPaymentChallan>
    {
        Task<IQueryable<QuarterlyTdsPaymentChallan>> GetQuarterlyTDSPaymentChallanAsync();
        Task<int> AddQuarterlyTDSPaymentChallanAsync(QuarterlyTdsPaymentChallan quarterlyTdsPaymentChallan);
        Task<bool> UpdateQuarterlyTDSPaymentChallanAsync(List<QuarterlyTdsPaymentChallan> quarterlyTdsPaymentChallanList);
        Task MapQuarterlyTDSPaymentChallanAsync(List<int> tdsPaymentChallanIds, int quarterlyTdsPaymentChallanId);
        Task<IQueryable<QuarterlyTdsPaymentChallan>> GetQuarterlyTDSPaymentChallanAsync(List<int> ids);
        Task<IQueryable<MappingTdsQuarterChallan>> GetMappingQuarterlyTDSPaymentChallanByQuarterChallanId(List<int> ids);
        Task<MappingTdsQuarterChallan> GetMappingQuarterlyTDSPaymentChallanByTdsChallanId(int id);
    }
}
