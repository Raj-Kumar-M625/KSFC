using Application.DTOs.GSTTDS;
using Application.Interface.Persistence.Generic;
using Domain.GSTTDS;
using Application.DTOs.GSTTDS;
using Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.GSTTDS
{
    public interface IGstdsPaymentChallanRepository : IGenericRepository<GsttdsPaymentChallan>
    {
        IQueryable<GsstPaymentChallanDto> GetGSTTDSPaymentChallanList();

        Task<GsttdsPaymentChallan> GetGSTTDSPaymentChallanAsync(int id);

        Task<GsttdsPaymentChallan> AddGSTTDSPaymentChallanAsync(GsttdsPaymentChallan tdsPaymentChallan);
        IQueryable<GsttdsPaymentChallan> GetGSTTDSPaymentChallanListByIds(List<int> ids);
        IQueryable<GsttdsPaymentChallan> GetGSTTDSPaidList();     
        Task<GsttdsPaymentChallan> UpdateGSTTDSPaymentChallanAsync(GsttdsPaymentChallan tdsPaymentChallan);
    }
}
