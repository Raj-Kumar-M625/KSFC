using Application.Interface.Persistence.Generic;
using Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Payment
{
    public interface IBillTdsPaymentRepository : IGenericRepository<BillTdsPayment>
    {
        IQueryable<BillTdsPayment> GetBillTDSPaymentList();
        IQueryable<BillTdsPayment> GetBillTDSPaymentListByChallanId(List<int> challanIds);
        Task<List<BillTdsPayment>> GetBillTDSPaymentChallanById(int ids);
    }
}
