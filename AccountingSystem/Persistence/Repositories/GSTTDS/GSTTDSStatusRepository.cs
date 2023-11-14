using Application.Interface.Persistence.GSTTDS;
using Application.Interface.Persistence.Vendor;
using Domain.GSTTDS;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.GSTTDS
{


    public class GsttdsStatusRepository:GenericRepository<GsttdsStatus>, IGsttdsStatusRepository
    {
        private readonly AccountingDbContext _dbContext;
        private readonly IVendorRepository _vendorRepository;
        public GsttdsStatusRepository(AccountingDbContext dbContext,IVendorRepository vendorRepository) : base(dbContext)
        {
            _dbContext = dbContext;
            _vendorRepository = vendorRepository;
        }

        public IList<GsttdsStatus> GetGSTTDSStatus(List<int> ids)
        {
            var res = _dbContext.GSTTDSStatus.Where(t => ids.Contains(t.BillID)).ToList();
            return res;
        }



        public async Task<bool> UpdateGSTTDSStatus(GsttdsStatus item)
        {
            try
            {
                var billId = item.BillID;
                var billDetails = _dbContext.Bill.Where(x => x.ID == billId).FirstOrDefault();

                if (billDetails != null)
                {
                    var vendorBalace = await _vendorRepository.GetVendorBalanceByID(billDetails.VendorId);
                    {
                        vendorBalace.Paid_GST_TDS += billDetails.GSTTDS;
                        vendorBalace.Pending_GST_TDS = vendorBalace.Pending_GST_TDS - billDetails.GSTTDS;
                    }
                  await _vendorRepository.UpdateVendorBalance(vendorBalace);

                }


                _dbContext.GSTTDSStatus.Update(item);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}",e.Message);
                return false;
            }
        }


    }
}
