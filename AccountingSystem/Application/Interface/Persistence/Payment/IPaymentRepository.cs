using Application.DTOs.Payment;
using Application.Interface.Persistence.Generic;
using Domain.Bill;
using Domain.Payment;
using Domain.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Payment
{
    public interface IPaymentRepository:IGenericRepository<Payments>
    {
        IQueryable<Payments> GetVendorPayment();
        Task<List<VendorPaymentListDto>> GetAll();
        Task<Payments> UpdateAsync(Payments entity);
        IQueryable<Payments> GetPayments();
        Task<List<Payments>> GetPaymentsByStatus();
        Task<Payments> GetPaymentsById(int Id);
        Task<List<Payments>> GetPaymentsById(List<int> Id);
        Task<IQueryable<Transaction>> GetBillPaymentsByVendorId(int vendorId);
        Task<Payments> ApprovePayment(Payments entity);
        Task<List<PaymentStatus>> GetPaymentStatuses(int Id);
        Task<List<Payments>> GetPaymentsByVendoorID(int Id);
        Task<List<Payments>> GetPaymentsByReferenceNumbers(List<string> referenceNumbers);

        Task<PaymentStatus> AddPaymentStatus(PaymentStatus entity);
        Task<PaymentStatus> UpdatePaymentStatus(List<PaymentStatus> entity);
        Task<BillPayment> UpdateBillPayment(List<BillPayment> entity);
        Task<BillPayment> AddBillPayment(List<BillPayment> entity);
        Task<IQueryable<BillPayment>> GetBillPayment(int paymentId);
        Task<List<BillPayment>> GetVendorBillPaymentList(int vendorId);

        Task<Payments> AddAdvancePayment(Payments payment);
        Task<List<Payments>> GetAdvancePaymentsbyVendorId(int vednorId);
        Task<List<MappingAdvancePayment>> AddMappingAdvancePayment(List<MappingAdvancePayment> mappingAdvancePayments);
        Task<List<MappingAdvancePayment>> UpdateMappingAdvancePayment(List<MappingAdvancePayment> mappingAdvancePayments);
        Task<List<MappingAdvancePayment>> GetMappingAdvancePayments(int paymenttId);
        Task<Payments> UpdateAdvancePayment(Payments payment);
        Task<BillPayment> GetBillPaymentById(int Id);
        Task<List<BillPayment>> GetBillPaymentById(List<int> Id);

        Task<BillPayment> UpdateBillPayment(BillPayment entity);

        Task<Payments> GetPaymentsByBillReferenceNumbers(string referenceNumbers);

        Task<List<BillPayment>> GetBillPaymentByPaymentId(int Id);

        Task<List<BillPayment>> GetBillPaymentList(List<int> Id);


    }
}
