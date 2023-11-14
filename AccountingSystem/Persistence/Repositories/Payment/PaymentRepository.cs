using Application.DTOs.Payment;
using Application.Interface.Persistence.Payment;
using Common.ConstantVariables;
using DocumentFormat.OpenXml.InkML;
using Domain.Bill;
using Domain.Payment;
using Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Payment
{
    public class PaymentRepository : GenericRepository<Payments>, IPaymentRepository
    {
        private readonly AccountingDbContext _dbContext;
        //Constructor for the Payment Repository
        public PaymentRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //Get Payments where the status is approved
        public IQueryable<Payments> GetPayments()
        {
            IQueryable<Payments> res = _dbContext.Set<Payments>().Include(o => o.PaymentStatus).Include(o => o.PaymentStatus.StatusMaster).Include(o => o.Vendor).Where(o => o.PaymentStatus.StatusMaster.CodeValue == ValueMapping.approved);
            return res;
        }

        //Get the List of Payments
        public IQueryable<Payments> GetVendorPayment()
        {
            IQueryable<Payments> res = _dbContext.Set<Payments>().Include(o => o.PaymentStatus).Include(o => o.PaymentStatus.StatusMaster).Include(o => o.Vendor).Include(o => o.Vendor.VendorBalance);
            //IQueryable<Payments> res = _dbContext.Set<Payments>().Include(o => o.Vendor).Include(o => o.Vendor.VendorBalance);

            return res;

        }

        //Get the Payments to Download
        async Task<List<VendorPaymentListDto>> IPaymentRepository.GetAll()
        {
            var vendorPayments = await _dbContext.Set<Payments>().Include(o => o.PaymentStatus).Include(o => o.PaymentStatus.StatusMaster).Include(o => o.Vendor).ToListAsync();
            List<VendorPaymentListDto> paymentDtos = vendorPayments
          .Select(p => new VendorPaymentListDto
          {
              PaymentReferenceNo = p.PaymentReferenceNo,
              PayDate = p.PaymentDate.ToString("dd-MM-yyyy"),
              VendorName = p.Vendor.Name,
              Paid = p.PaidAmount,
              CreatedBy = p.CreatedBy,
              ApprovedBy = p.ApprovedBy,
              PaymentStatus = p.PaymentStatus.StatusMaster.CodeName
          }).ToList();
            return paymentDtos;
        }

        //Update/Add the Payments
        public async Task<Payments> UpdateAsync(Payments payments)
        {


            try
            {

                var vlist = _dbContext.Vendor.Include(o => o.VendorBalance);

                if (payments.Vendor != null)
                {
                    var entry = vlist.FirstOrDefault(o => o.Id == payments.VendorID);

                    _dbContext.Entry(entry).CurrentValues.SetValues(payments.Vendor);

                    await _dbContext.SaveChangesAsync();
                }

                payments.Vendor = null;

                await _dbContext.AddAsync(payments);


                await _dbContext.SaveChangesAsync();
                return payments;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Payments> GetPaymentsById(int Id)
        {
            var payments = await _dbContext.Payments.Where(o => o.ID == Id).Include(o => o.PaymentStatus).
                Include(o => o.MappingAdvancePayment).Include(o => o.PaymentStatus.StatusMaster).Include(o => o.Vendor)
                .Include(o => o.Vendor.VendorBalance)
                .Include(o => o.Vendor.VendorDefaults)
                .Include(o => o.Vendor.VendorBankAccounts)
                 .Include(o => o.Vendor.VendorBankAccounts.BranchMaster)
                  .Include(o => o.Vendor.VendorBankAccounts.BranchMaster.BankDetails)
                .Include(o => o.Vendor.VendorPerson.Contacts)
                .Include(o => o.Vendor.VendorPerson).FirstOrDefaultAsync();
            return payments;
        }



        public async Task<List<Payments>> GetPaymentsByVendoorID(int Id)
        {
            var payments = await _dbContext.Payments.Include(o => o.Vendor).Where(o => o.VendorID == Id).ToListAsync();
            return payments;
        }

        public async Task<PaymentStatus> AddPaymentStatus(PaymentStatus entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<BillPayment> AddBillPayment(List<BillPayment> entity)
        {
            try
            {
                await _dbContext.AddRangeAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<Payments>> GetPaymentsByReferenceNumbers(List<string> referenceNumbers)
        {
            //var payments = await _dbContext.Payments.Where(o => referenceNumbers.Contains(o.BillsID)).ToListAsync();
            var payments = await _dbContext.Payments.Where(o => referenceNumbers.Contains(o.PaymentReferenceNo)).ToListAsync();
            return payments;
        }


        public async Task<Payments> GetPaymentsByBillReferenceNumbers(string referenceNumbers)
        {
            var res = await _dbContext.Payments.Where(x => x.PaymentBillReference == referenceNumbers).FirstAsync();
            return res;

            //var payments = await _dbContext.Payments.Where(o => referenceNumbers.Contains(o.BillsID)).ToListAsync();
            //var payments = await _dbContext.Payments.Where(o => referenceNumbers.Equals(o.PaymentBillReference)).ToListAsync();
            //return payments;
        }
        public async Task<PaymentStatus> UpdatePaymentStatus(List<PaymentStatus> entity)
        {
            _dbContext.PaymentStatus.UpdateRange(entity);
            await _dbContext.SaveChangesAsync();
            return entity.First();
        }
        public async Task<BillPayment> UpdateBillPayment(BillPayment entity)
        {
            _dbContext.BillPayment.UpdateRange(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Payments>> GetPaymentsByStatus()
        {
            var vendorPayments = await _dbContext.Set<Payments>().Include(o => o.PaymentStatus).Include(o => o.PaymentStatus.StatusMaster).Include(o => o.Vendor).Where(x => x.PaymentStatus.StatusMaster.CodeValue == ValueMapping.pending).ToListAsync();
            return vendorPayments;
        }

        public async Task<Payments> ApprovePayment(Payments entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<PaymentStatus>> GetPaymentStatuses(int Id)
        {
            var res = await _dbContext.PaymentStatus.Where(x => x.PaymentID == Id).ToListAsync();
            return res;
        }

        public async Task<IQueryable<BillPayment>> GetBillPayment(int paymentId)
        {
            var res = _dbContext.BillPayment.Where(x => x.PaymentID == paymentId).Include(x => x.Payments)
                 .Include(x => x.Bill).AsQueryable();
            return res;
        }

        public async Task<List<BillPayment>> GetVendorBillPaymentList(int vendorId)
        {


            var res = _dbContext.BillPayment.Where(x => x.IsActive == true).Include(x => x.Payments).Include(x => x.Bill)
                .Include(x => x.Bill.BillStatus)
                .Include(x => x.Bill.BillStatus.StatusMaster)
                .Include(o => o.Payments.PaymentStatus)
                .Include(o => o.Payments.PaymentStatus.StatusMaster).Where(x => x.VendorId == vendorId).ToList();
            return res;
        }

        public async Task<BillPayment> GetBillPaymentById(int Id)
        {
            var res = _dbContext.BillPayment.Where(x => x.Id == Id).Include(x => x.Payments)
                .Include(x => x.Bill).Include(x => x.Vendor).Include(x => x.Vendor.VendorBalance).First();
            return res;
        }

        public async Task<List<BillPayment>> GetBillPaymentById(List<int> Id)
        {
            var res = _dbContext.BillPayment.Where(x => Id.Any(y => y.Equals(x.PaymentID)) && x.IsActive == true)
                .Include(x => x.Payments)
                .Include(x => x.Bill)
                .Include(x => x.Vendor)
                .Include(x => x.Vendor.VendorPerson.Contacts)
                .Include(x => x.Vendor.VendorBankAccounts.BranchMaster)
                .Include(x => x.Vendor.VendorBankAccounts.BranchMaster.BankDetails)
                .ToList();
            return res;
        }

        public async Task<IQueryable<Transaction>> GetBillPaymentsByVendorId(int vendorId)
        {
            try
            {
                var results = (from b in _dbContext.Bill
                               join bs in _dbContext.BillStatus on b.ID equals bs.BillID
                               join cm in _dbContext.CommonMaster on bs.StatusCMID equals cm.Id
                               where b.VendorId == vendorId
                               select new Transaction
                               {
                                   ID = b.ID,
                                   BillReferenceNo = b.BillReferenceNo,
                                   TransactionType = "C",
                                   Amount = b.NetPayable,
                                   TransactionDate = b.BillDate,
                                   Status = cm.CodeValue,
                                   CreatedBy = b.CreatedBy,
                                   BillNo = b.BillNo,
                                   ApprovedBy = b.ApprovedDate != null ? b.ModifedBy : "Not Approved",
                                   Type = "Bill",
                                   AdvanceAmountUsed = 0
                               })
               .Union(from p in _dbContext.Payments
                      join ps in _dbContext.PaymentStatus on p.ID equals ps.PaymentID
                      join cm in _dbContext.CommonMaster on ps.StatusCMID equals cm.Id
                      where p.VendorID == vendorId
                      select new Transaction
                      {
                          ID = p.ID,
                          BillReferenceNo = p.PaymentReferenceNo,
                          TransactionType = "P",
                          Amount = p.PaidAmount,
                          TransactionDate = p.PaymentDate,
                          Status = cm.CodeValue,
                          CreatedBy = p.CreatedBy,
                          BillNo = "Not Available",
                          ApprovedBy = p.ApprovedBy != null ? p.ApprovedBy : "Not Approved",
                          Type = p.Type,
                          AdvanceAmountUsed = p.AdvanceAmountUsed
                      });


                return results;


            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<Payments> AddAdvancePayment(Payments payment)
        {
            try
            {
                await _dbContext.Payments.AddAsync(payment);
                await _dbContext.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<List<Payments>> GetPaymentsById(List<int> Id)
        {
            try
            {
                var res = await _dbContext.Payments.Where(x => Id.Contains(x.ID))
               .Include(x => x.Vendor)
               .Include(x => x.Vendor.VendorPerson.Contacts)
                .Include(x => x.Vendor.VendorBankAccounts.BranchMaster)
                .Include(x => x.Vendor.VendorBankAccounts.BranchMaster.BankDetails).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<Payments> UpdateAdvancePayment(Payments payment)
        {
            try
            {
                _dbContext.Payments.Update(payment);
                await _dbContext.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<List<Payments>> GetAdvancePaymentsbyVendorId(int vednorId)
        {
            try
            {
                var payments = await _dbContext.Payments
                    .Include(x => x.PaymentStatus)
                    .Include(x => x.PaymentStatus.StatusMaster)
                    .Where(x => x.Type == "Advance" && x.VendorID == vednorId && x.PaymentStatus.StatusMaster.CodeValue == "Paid")
                    .ToListAsync();
                return payments;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<List<MappingAdvancePayment>> AddMappingAdvancePayment(List<MappingAdvancePayment> mappingAdvancePayments)
        {
            try
            {
                await _dbContext.MappingAdvancePayment.AddRangeAsync(mappingAdvancePayments);
                await _dbContext.SaveChangesAsync();
                return mappingAdvancePayments;

            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<List<MappingAdvancePayment>> GetMappingAdvancePayments(int paymenttId)
        {
            try
            {
                var res = await _dbContext.MappingAdvancePayment.Where(x => x.PaymentsID == paymenttId && x.IsActive == true)
                    .Include(x => x.Payments)
                    .Include(x => x.Vendor)
                    .Include(x => x.Vendor.VendorPerson)
                    .Include(x => x.Vendor.VendorPerson.Contacts)
                    .Include(x => x.Vendor.VendorBankAccounts)
                    .Include(x => x.Vendor.VendorBankAccounts.BranchMaster)
                    .Include(x => x.Vendor.VendorBankAccounts.BranchMaster.BankDetails)
                    .ToListAsync();
                return res;

            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<List<MappingAdvancePayment>> UpdateMappingAdvancePayment(List<MappingAdvancePayment> mappingAdvancePayments)
        {
            try
            {
                //foreach (var item in mappingAdvancePayments)
                //{
                //    item.Vendor = null;
                //    item.Payments = null;
                //}
                _dbContext.MappingAdvancePayment.UpdateRange(mappingAdvancePayments);
                await _dbContext.SaveChangesAsync();
                return mappingAdvancePayments;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public async Task<BillPayment> UpdateBillPayment(List<BillPayment> entity)
        {
            _dbContext.BillPayment.UpdateRange(entity);
            await _dbContext.SaveChangesAsync();
            return entity.First();
        }
        public async Task<List<BillPayment>> GetBillPaymentByPaymentId(int Id)
        {
            var res = _dbContext.BillPayment.Where(x => x.PaymentID == Id).Include(x => x.Bill).ToList();
            return res;
        }



        public async Task<List<BillPayment>> GetBillPaymentList(List<int> Id)
        {            
            var res = _dbContext.BillPayment.Include(o => o.Payments).Include(o => o.Payments.PaymentStatus).Include(o => o.Payments.PaymentStatus.StatusMaster).Where(x => Id.Contains(x.BillID) && (x.Payments.PaymentStatus.StatusMaster.CodeValue =="Approved" || x.Payments.PaymentStatus.StatusMaster.CodeValue == "Paid"))               
                .ToList();
            return res;
        }
    }
}

