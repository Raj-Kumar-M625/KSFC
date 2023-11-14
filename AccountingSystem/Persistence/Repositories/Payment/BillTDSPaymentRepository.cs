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
    public class BillTdsPaymentRepository: GenericRepository<BillTdsPayment>, IBillTdsPaymentRepository
    {
        private readonly AccountingDbContext _dbContext;

        public BillTdsPaymentRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<BillTdsPayment> GetBillTDSPaymentList()
        {
            var res = _dbContext.BillTDSPayment.Include(b => b.Bill).Include(b => b.Bill.TDSStatus).Include(b => b.Bill.TDSStatus.StatusMaster).Include(b => b.Bill.Vendor).Include(b => b.Bill.Vendor.VendorDefaults).Include(b => b.TDSPaymentChallan).Include(b => b.TDSPaymentChallan.QuarterMaster).Include(b => b.TDSPaymentChallan.TDSStatus.StatusMaster).DefaultIfEmpty();
            return res;
        }

        public IQueryable<BillTdsPayment> GetBillTDSPaymentListByChallanId(List<int> challanIds)
        {
            var res = _dbContext.BillTDSPayment.Include(b => b.Bill).Include(b => b.Bill.TDSStatus).Include(b => b.Bill.TDSStatus.StatusMaster).Include(b => b.Bill.Vendor).Include(b => b.Bill.Vendor.VendorDefaults).Include(b => b.TDSPaymentChallan).Include(b => b.TDSPaymentChallan.QuarterMaster).Include(b => b.TDSPaymentChallan.TDSStatus.StatusMaster).DefaultIfEmpty().Where(b => b.TDSPaymentChallanID.HasValue && challanIds.Contains(b.TDSPaymentChallanID.Value));
            return res;
        }
        public async Task<List<BillTdsPayment>> GetBillTDSPaymentChallanById(int ids)
        {
            //var res = _dbContext.TDSPaymentChallan.Include(t => t.Bank).DefaultIfEmpty().Include(t => t.QuarterMaster).DefaultIfEmpty().Include(t => t.StatusMaster).DefaultIfEmpty().Where(t => ids.Contains(t.Id));
            var res = _dbContext.BillTDSPayment.Include(t => t.Bill)
                                                .Include(t => t.Bill.Vendor.VendorPerson.Contacts)
                                                .Include(t => t.Bill.Vendor.VendorBankAccounts.BranchMaster)
                                                .Include(t => t.Bill.Vendor.VendorBankAccounts.BranchMaster.BankDetails)
                                                .Include(t => t.TDSPaymentChallan).DefaultIfEmpty()
                                                .Where(t => t.TDSPaymentChallanID == ids).ToList();
            return res;
        }

      
    }
}
