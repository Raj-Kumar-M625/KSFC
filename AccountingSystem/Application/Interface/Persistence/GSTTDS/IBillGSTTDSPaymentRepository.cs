using Application.Interface.Persistence.Generic;
using Domain.GSTTDS;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.GSTTDS
{
    public interface IBillGsttdsPaymentRepository:IGenericRepository<BillGsttdsPayment>
    {
        IQueryable<BillGsttdsPayment> GetBillGSTTDSPaymentList();
        IQueryable<BillGsttdsPayment> GetBillGSTTDSPaidList();
        IQueryable<BillGsttdsPayment> GeGSTTDSJSONist();


        IQueryable<BillGsttdsPayment> GetBillGSTTDSPaymentListByID(List<int> ids);
        Task<BillGsttdsPayment> GetBillGSTTDSPaymentByID(int id);
        Task<List<BillGsttdsPayment>> GetAllBillGSTTDSPaymentByID(int id);


    }
}
