using Application.DTOs.TDS;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Vendor;
using Common.ConstantVariables;
using Domain.Payment;
using Domain.TDS;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Payment
{
    public class TdsPaymentChallanRepository:GenericRepository<TdsPaymentChallan>, ITdsPaymentChallanRepository
    {
        private readonly AccountingDbContext _dbContext;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IVendorRepository _vendorRepository;
        public TdsPaymentChallanRepository(AccountingDbContext dbContext,ICommonMasterRepository commonMaster,IVendorRepository vendorRepository) : base(dbContext)
        {
            _dbContext = dbContext;
            _commonMaster = commonMaster;
            _vendorRepository = vendorRepository;
        }

        public IQueryable<TdssPaymentChallanDto> GetTDSPaymentChallanList()
        {
            try
            {
                var tdsstatus = from ts in _dbContext.TDSStatus
                                join cm in _dbContext.CommonMaster on ts.StatusCMID equals cm.Id
                                group ts by new
                                {
                                    ts.TDSPaymentChallanID,
                                    cm.CodeValue,
                                    cm.CodeName
                                } into newgroup
                                select new
                                {
                                    TDSId = newgroup.Key.TDSPaymentChallanID,
                                    CodeName = newgroup.Key.CodeName,
                                    CodeValue = newgroup.Key.CodeValue
                                };


                var res = from td in _dbContext.TDSPaymentChallan
                          join ts in tdsstatus on td.Id equals ts.TDSId
                          join bm in _dbContext.BankMaster on td.BankMasterID equals bm.Id
                          select new TdssPaymentChallanDto
                          {
                              Id = td.Id,
                              TDSPaymentChallan = td,
                              CodeName = ts.CodeName,
                              CodeValue = ts.CodeValue,
                              Bank = bm
                          };

                return res;
            }
            catch (Exception ex)
            {

                throw ex;     
                    
                    
            }



        }

        public async Task<IQueryable<TdsPaymentChallan>> GetTDSPaymentChallanById(List<int> ids)
        {
            //var res = _dbContext.TDSPaymentChallan.Include(t => t.Bank).DefaultIfEmpty().Include(t => t.QuarterMaster).DefaultIfEmpty().Include(t => t.StatusMaster).DefaultIfEmpty().Where(t => ids.Contains(t.Id));
            var res = _dbContext.TDSPaymentChallan.Include(t => t.Bank).Include(t => t.TDSStatus).Include(t => t.TDSStatus.StatusMaster).DefaultIfEmpty().Include(t => t.QuarterMaster).DefaultIfEmpty().DefaultIfEmpty().Where(t => ids.Contains(t.Id));
            return res;
        }

        public async Task<TdsPaymentChallan> AddTDSPaymentChallanAsync(TdsPaymentChallan tdsPaymentChallan)
        {
            try
            {
                tdsPaymentChallan.TDSStatus = null;
                tdsPaymentChallan.Bank = null;
                await _dbContext.TDSPaymentChallan.AddAsync(tdsPaymentChallan);
                await _dbContext.SaveChangesAsync();

                var billsId = tdsPaymentChallan.BillsId;
                var billTDSPayments = new List<BillTdsPayment>();
                foreach (var billId in billsId)
                {
                    var billTDSPayment = new BillTdsPayment() { BillID = billId,TDSPaymentChallanID = tdsPaymentChallan.Id };
                    billTDSPayments.Add(billTDSPayment);
                }
                await _dbContext.BillTDSPayment.AddRangeAsync(billTDSPayments);


                var TDSStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.tStatus);
                tdsPaymentChallan.TDSStatus = new TdsStatus();

                var tdsStatusList = _dbContext.TDSStatus.Where(t => tdsPaymentChallan.BillsId.Any(y => y == t.BillID)).ToList();
                foreach (var tdsStatus in tdsStatusList)
                {
                    tdsStatus.StatusCMID = TDSStatusList.First(p => p.CodeValue == ValueMapping.ChallanCreated).Id;
                    tdsStatus.TDSPaymentChallanID = tdsPaymentChallan.Id;
                    tdsStatus.ModifedBy = tdsPaymentChallan.ModifiedBy;
                    tdsStatus.ModifiedOn = tdsPaymentChallan.ModifiedOn;

                }
                _dbContext.TDSStatus.UpdateRange(tdsStatusList);
                await _dbContext.SaveChangesAsync();
                return tdsPaymentChallan;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}",e.Message);
                return tdsPaymentChallan;
            }
        }

        public async Task<bool> UpdateTDSPaymentChallanAsync(IEnumerable<TdsPaymentChallan> tdsPaymentChallanList)
        {
            try
            {
                var tdsId = tdsPaymentChallanList.First().Id;
                var tdstatus = tdsPaymentChallanList.First();
                var tdsStatusList = _dbContext.TDSStatus.Where(t => t.TDSPaymentChallanID == tdsId).ToList();

                var billId = tdsStatusList.Select(x => x.BillID);
                var billDetails = _dbContext.Bill.Where(x => billId.Contains(x.ID)).Include(x=>x.TDSStatus).Include(x=>x.TDSStatus.StatusMaster).ToList();
                foreach (var billList in billDetails)
                {
                    if (billList != null)
                    {
                        if (billList.TDSStatus.StatusMaster.CodeValue == "ChallanCreated")
                        {
                            var vendorBalace = await _vendorRepository.GetVendorBalanceByID(billList.VendorId);
                            {
                                vendorBalace.Paid_TDS += billList.TDS;
                                vendorBalace.Pending_TDS = vendorBalace.Pending_TDS - billList.TDS;
                            }
                            var updateVendorBalace = await _vendorRepository.UpdateVendorBalance(vendorBalace);
                        }
                    }  
                }
                var TDSStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.tStatus);
                var qtyfiledStatus = TDSStatusList.First(s => s.CodeValue == ValueMapping.QtyFiled).Id;
                var csvCreatedStatus = TDSStatusList.First(s => s.CodeValue == ValueMapping.CSVCreated).Id;
                var filedStatus = TDSStatusList.First(s => s.CodeValue == ValueMapping.CSVPending).Id;
                var tdsStatusCMId = tdsPaymentChallanList.First().TDSStatus.StatusCMID;


                if (qtyfiledStatus == tdsStatusCMId)
                {
                    foreach (var tdsStatus in tdsStatusList)
                    {
                        tdsStatus.StatusCMID = qtyfiledStatus;
                        tdsStatus.TDSPaymentChallanID = tdsId;
                        tdsStatus.ModifedBy = tdsPaymentChallanList.First().ModifiedBy;
                        tdsStatus.ModifiedOn = tdsPaymentChallanList.First().ModifiedOn;

                    }
                }
                else if (filedStatus == tdsStatusCMId)
                {
                    foreach (var tdsStatus in tdsStatusList)
                    {
                        tdsStatus.StatusCMID = filedStatus;
                        tdsStatus.TDSPaymentChallanID = tdsId;
                        tdsStatus.ModifedBy = tdsPaymentChallanList.First().ModifiedBy;
                        tdsStatus.ModifiedOn = tdsPaymentChallanList.First().ModifiedOn;

                    }
                }
                else if (csvCreatedStatus == tdsStatusCMId)
                {
                    foreach (var tdsStatus in tdsStatusList)
                    {
                        tdsStatus.StatusCMID = csvCreatedStatus;
                        tdsStatus.TDSPaymentChallanID = tdsId;
                        tdsStatus.ModifedBy = tdsPaymentChallanList.First().ModifiedBy;
                        tdsStatus.ModifiedOn = tdsPaymentChallanList.First().ModifiedOn;

                    }
                }
                else
                {
                    foreach (var tdsStatus in tdsStatusList)
                    {
                        tdsStatus.StatusCMID = TDSStatusList.First(p => p.CodeValue == ValueMapping.Paid).Id;
                        tdsStatus.TDSPaymentChallanID = tdsId;
                        tdsStatus.ModifedBy = tdsPaymentChallanList.First().ModifiedBy;
                        tdsStatus.ModifiedOn = tdsPaymentChallanList.First().ModifiedOn;

                    }
                }
                _dbContext.TDSStatus.UpdateRange(tdsStatusList);
                _dbContext.TDSPaymentChallan.UpdateRange(tdsPaymentChallanList);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}",e.Message);
                return false;
            }
        }
        public async Task<TdsPaymentChallan> GetTDSPaymentChallanById(int id)
        {
            try
            {
                var tdsPaymentChaaln = await _dbContext.TDSPaymentChallan.Where(x => x.Id == id).Include(x => x.Bank).FirstOrDefaultAsync();
                return tdsPaymentChaaln;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
