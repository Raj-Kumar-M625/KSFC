using Application.Interface.Persistence.GSTTDS;
using Common.ConstantVariables;
using Domain.GSTTDS;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.GSTTDS
{
    public class BillGsttdsPaymentRepository:GenericRepository<BillGsttdsPayment>, IBillGsttdsPaymentRepository
    {
        private readonly AccountingDbContext _dbContext;

        public BillGsttdsPaymentRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<BillGsttdsPayment> GetBillGSTTDSPaymentList()
        {
            var res = _dbContext.BillGSTTDSPayment.Include(b => b.Bill).Include(b => b.Bill.Vendor).Include(b => b.Bill.Vendor.VendorDefaults).Include(b => b.GSTTDSPaymentChallan).Include(b => b.GSTTDSPaymentChallan.GSTTDSStatus).Include(b => b.GSTTDSPaymentChallan.GSTTDSStatus.StatusMaster);
            return res;
        }

        public IQueryable<BillGsttdsPayment> GetBillGSTTDSPaidList()
        {
            var res = _dbContext.BillGSTTDSPayment.Include(b => b.Bill).Include(b => b.Bill.Vendor).Include(b => b.Bill.Vendor.VendorDefaults).Include(b => b.GSTTDSPaymentChallan).Include(b => b.GSTTDSPaymentChallan.GSTTDSStatus).Include(b => b.GSTTDSPaymentChallan.GSTTDSStatus.StatusMaster).Where(x => x.GSTTDSPaymentChallan.GSTTDSStatus.StatusMaster.CodeValue == ValueMapping.paid);
            return res;
        }


        public IQueryable<BillGsttdsPayment> GeGSTTDSJSONist()
        {
            var res = _dbContext.BillGSTTDSPayment.Include(b => b.Bill).Include(b => b.Bill.Vendor).Include(b => b.Bill.Vendor.VendorDefaults).Include(b => b.GSTTDSPaymentChallan).Include(b => b.GSTTDSPaymentChallan.GSTTDSStatus).Include(b => b.GSTTDSPaymentChallan.GSTTDSStatus.StatusMaster);
            return res;
        }



        public IQueryable<BillGsttdsPayment> GetBillGSTTDSPaymentListByID(List<int> ids)
        {
            var res = _dbContext.BillGSTTDSPayment.Include(b => b.Bill).Include(b => b.Bill.Vendor).Include(b => b.Bill.Vendor.VendorDefaults).Include(b => b.GSTTDSPaymentChallan);
            return res;
        }

        public async Task<BillGsttdsPayment> GetBillGSTTDSPaymentByID(int id)
        {
            var res = await _dbContext.BillGSTTDSPayment.Include(b => b.Bill).Include(b => b.Bill.Vendor).Include(b => b.Bill.Vendor.VendorDefaults).Include(b => b.GSTTDSPaymentChallan).FirstOrDefaultAsync(b => b.ID == id);
            return res;
        }
        public async Task<List<BillGsttdsPayment>> GetAllBillGSTTDSPaymentByID(int id)
        {
            var res = _dbContext.BillGSTTDSPayment.Include(b => b.Bill)
                                                        .Include(b => b.Bill.Vendor)
                                                        .Include(b => b.Bill.Vendor.VendorBankAccounts.BranchMaster)
                                                        .Include(b => b.Bill.Vendor.VendorBankAccounts.BranchMaster.BankDetails)
                                                        .Include(b => b.Bill.Vendor.VendorPerson.Contacts)
                                                        .Include(b => b.Bill.Vendor.VendorDefaults)
                                                        .Include(b => b.GSTTDSPaymentChallan)
                                                        .Where(b => b.GSTTDSPaymentID == id).ToList();
            return res;
        }
    }
}
