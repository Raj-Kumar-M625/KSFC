using Application.DTOs.GSTTDS;
using Application.DTOs.TDS;
using Application.Interface.Persistence.GSTTDS;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Common.ConstantVariables;
using Domain.GSTTDS;
using Domain.Master;
using Domain.Payment;
using Domain.TDS;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.GSTTDS
{
    public class GsttdsPaymentChallanRepository : GenericRepository<GsttdsPaymentChallan>, IGstdsPaymentChallanRepository
    {
        private readonly AccountingDbContext _dbContext;
        private readonly ICommonMasterRepository _commonMaster;
        public GsttdsPaymentChallanRepository(AccountingDbContext dbContext, ICommonMasterRepository commonMaster) : base(dbContext)
        {
            _dbContext = dbContext;
            _commonMaster = commonMaster;
        }

        public IQueryable<GsstPaymentChallanDto> GetGSTTDSPaymentChallanList()
        {
            try
            {

                var Gsttdsstatus = from ts in _dbContext.GSTTDSStatus
                                join cm in _dbContext.CommonMaster on ts.StatusCMID equals cm.Id
                                group ts by new
                                {
                                    ts.GSTTDSPaymentChallanID,
                                   
                                    cm.CodeValue,
                                    cm.CodeName
                                } into newgroup
                                select new
                                {
                                    GSTTDSId = newgroup.Key.GSTTDSPaymentChallanID,
                                    CodeName = newgroup.Key.CodeName,
                                    CodeValue = newgroup.Key.CodeValue

                                };


                var res = from td in _dbContext.GSTTDSPaymentChallan
                          join ts in Gsttdsstatus on td.Id equals ts.GSTTDSId
                          //join bm in _dbContext.BankMaster on td.BankMasterID equals bm.Id
                          select new GsstPaymentChallanDto
                          {
                              Id = td.Id,
                              GSTTDSPaymentChallan = td,
                              CodeName = ts.CodeName,
                              CodeValue = ts.CodeValue,

                          };

                return res;

            }catch(Exception e)
            {
                throw e;
            }
        }

        public IQueryable<GsttdsPaymentChallan> GetGSTTDSPaymentChallanListByIds(List<int> ids)
        {
            try
            {
                var res = _dbContext.GSTTDSPaymentChallan.Where(t => ids.Contains(t.Id));
                return res;
            }
            catch(Exception ex)
            {
                throw;
            }
        
        }

        public Task<GsttdsPaymentChallan> GetGSTTDSPaymentChallanAsync(int id)
        {
            try
            {
                var res = _dbContext.GSTTDSPaymentChallan.Include(t => t.GSTTDSStatus).FirstOrDefaultAsync(t => t.Id == id);
                return res;
            }
            catch(Exception ex)
            {
                throw;
            }
           
        }

        public async Task<GsttdsPaymentChallan> AddGSTTDSPaymentChallanAsync(GsttdsPaymentChallan gstTdsPaymentChallan)
        {
            try
            {
                gstTdsPaymentChallan.GSTTDSStatus = null;
                await _dbContext.GSTTDSPaymentChallan.AddAsync(gstTdsPaymentChallan);
                await _dbContext.SaveChangesAsync();

                var billsId = gstTdsPaymentChallan.BillsId;
                List<BillGsttdsPayment> billGSTTDSPayments = new List<BillGsttdsPayment>();
                foreach (var billId in billsId)
                {
                    var billGSTTDSPayment = new BillGsttdsPayment() { BillID = billId, GSTTDSPaymentID = gstTdsPaymentChallan.Id };
                    billGSTTDSPayment.CreatedOn = DateTime.UtcNow;
                    billGSTTDSPayment.CreatedBy = gstTdsPaymentChallan.CreatedBy;
                    billGSTTDSPayment.ModifiedBy = gstTdsPaymentChallan.ModifiedBy;
                    billGSTTDSPayment.ModifiedOn = DateTime.UtcNow;
                    billGSTTDSPayments.Add(billGSTTDSPayment);
                }
                await _dbContext.BillGSTTDSPayment.AddRangeAsync(billGSTTDSPayments);
                await _dbContext.SaveChangesAsync();

                var TDSStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.gstTdsStatus);
                foreach (var billId in billsId)
                {
                    //var gstTDSStatus = new GsttdsStatus();
                    var res = await _dbContext.GSTTDSStatus.Where(x => x.BillID == billId).ToListAsync();
                    var gst = res.FirstOrDefault();
                    if(gst != null)
                    {
                        gst.ModifedBy = gstTdsPaymentChallan.ModifiedBy;
                        gst.ModifiedOn = DateTime.UtcNow;
                        gst.GSTTDSPaymentChallanID = gstTdsPaymentChallan.Id;
                        gst.StatusCMID = TDSStatusList.First(p => p.CodeValue == ValueMapping.ChallanCreated).Id;
                    }
                    _dbContext.Update(gst).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();

                }
               
                return gstTdsPaymentChallan;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}", e.Message);
                return gstTdsPaymentChallan;
            }
        }

        public async Task<GsttdsPaymentChallan> UpdateGSTTDSPaymentChallanAsync(GsttdsPaymentChallan gstTdsPaymentChallan)
        {
            try
            {
                _dbContext.GSTTDSPaymentChallan.Update(gstTdsPaymentChallan);
                await _dbContext.SaveChangesAsync();
                return gstTdsPaymentChallan;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}", e.Message);
                return gstTdsPaymentChallan;
            }
        }

        public  IQueryable<GsttdsPaymentChallan> GetGSTTDSPaidList()
        {
            var res =  _dbContext.GSTTDSPaymentChallan.Where(x => x.GSTTDSStatus.StatusMaster.CodeValue == ValueMapping.paid);
            return res;
        }
    }
}
