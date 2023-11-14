using Application.DTOs.Payment;
using Application.DTOs.TDS;
using Application.Interface.Persistence.Generic;
using Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Payment
{
    public interface ITdsPaymentChallanRepository: IGenericRepository<TdsPaymentChallan>
    {
        IQueryable<DTOs.TDS.TdssPaymentChallanDto> GetTDSPaymentChallanList();

        Task<TdsPaymentChallan> AddTDSPaymentChallanAsync(TdsPaymentChallan tdsPaymentChallan);
        Task<IQueryable<TdsPaymentChallan>> GetTDSPaymentChallanById(List<int> ids);
        Task<bool> UpdateTDSPaymentChallanAsync(IEnumerable<TdsPaymentChallan> tdsPaymentChallanList);
        Task<TdsPaymentChallan> GetTDSPaymentChallanById(int id);

    }
}
