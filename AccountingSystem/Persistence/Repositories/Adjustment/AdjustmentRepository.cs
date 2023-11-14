using Application.Interface.Persistence.Adjustment;
using DocumentFormat.OpenXml.Drawing.Charts;
using Domain.Adjustment;
using Domain.Bill;
using Domain.Payment;
using Domain.Vendor;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Adjustment
{
    public class AdjustmentRepository : GenericRepository<Adjustments>, IAdjustmentRepository
    {
        private readonly AccountingDbContext _dbContext;
        public AdjustmentRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Adjustments> GetAdjustmentList(int VendorId)
        {
           
            //return res;           
            IQueryable<Adjustments> res = from a in _dbContext.Adjustment
                                          join ads in _dbContext.AdjustmentStatus.Include(x => x.StatusMaster) on a.ID equals ads.AdjustmentID
                                          join v in _dbContext.Vendor on a.VendorID equals v.Id
                                          where a.VendorID == VendorId
                                          select new Adjustments
                                          {
                                              AdjustmentReferenceNo = a.AdjustmentReferenceNo,
                                              AdjustmentType = a.AdjustmentType,
                                              Date = a.Date,
                                              Amount = a.Amount,
                                              Description=a.Description,
                                              UTR_No=a.UTR_No,
                                              AdjustmentStatus = ads,
                                              Vendor = v
                                          };
            //_dbContext.Set<Adjustments>()
            //.Include(o=> o.AdjustmentStatus)
            //.Include(o => o.AdjustmentStatus.StatusMaster);

            
            return res;
        }
        public async Task<Adjustments> AddAdjustments(Adjustments entity)
        {
            try
            {
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;

            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}", e.Message);
                return entity;
            }
        }
        public async Task<Adjustments> GetAdjustmentsById(int Id)
        {
            var res = _dbContext.Adjustment.Where(x => x.ID == Id).Include(x => x.Vendor).Include(x => x.Vendor.VendorBalance).First();
            return res;
        }
        public async Task<AdjustmentStatus> AddAdjustmentStatus(AdjustmentStatus entity)
        {

            try
            {
                await _dbContext.AdjustmentStatus.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}", e.Message);
                return entity;
            }
        }
    }
}
