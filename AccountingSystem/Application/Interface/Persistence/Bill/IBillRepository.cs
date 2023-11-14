using Application.DTOs.Bill;
using Application.Interface.Persistence.Generic;
using Domain.Adjustment;
using Domain.Bill;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Bill
{
    public interface IBillRepository:IGenericRepository<Bills>
    {
        IQueryable<Bills> GetBill();
        IQueryable<Bills> GetBillForGSTTDS();
        Task<List<BillListDto>> GetAll();
        Task<List<Bills>> GetVendorBillsList(int Id);
        Task<List<BillItems>> GetBillpaymentDetailsByID(int ID);
        IQueryable<Bills> GetBillsById(List<int> Ids);
        Task<Bills> AddBills(Bills entity);
        Task<Bills> UpdateBillsDetails(Bills entity);
        Task<Adjustments> UpdateAdjustmentDetails(Adjustments entity);
        Task<List<BillItems>> AddBillPaymentDetails(List<BillItems> entity);
        Task<BillStatus> AddBillStatus(BillStatus entity);
        Task<Bills> GetBillsByReferenceNo(string billrefno);

        Task<List<BillItems>> UpdatePaymentDetails(List<BillItems> entity,string user);
        Task<Bills> UpdateBills(Bills entity);
        
        Task<BillItems> DeleteBillDocumentDetails(string ID);
        Task<List<BillStatus>> GetAllBillStatus(int Id);
        Task<List<BillStatus>> UpdateBillStatus(List<BillStatus> billStatus);
        Task<int> GetBillDetailsByBIllReferenceNo(string number); 
        IQueryable<Adjustments> GetAdjustmentList(int VendorId);
        Task<Adjustments> GetAdjustmentDetails(int Id);
    }
}
