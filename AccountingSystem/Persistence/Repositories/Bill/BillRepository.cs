using Application.DTOs.Bill;
using Application.Interface.Persistence.Bill;
using Application.UserStories.Master.Request.Queries;
using Common.ConstantVariables;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Adjustment;
using Domain.Bill;
using Domain.Vendor;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Bill
{
    public class BillRepository:GenericRepository<Bills>, IBillRepository
    {
        private readonly AccountingDbContext _dbContext;
        private readonly IMediator _mediator;
        public BillRepository(AccountingDbContext dbContext,IMediator mediator) : base(dbContext)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public IQueryable<Bills> GetBill()
        {
            try
            {
                
                IQueryable<Bills> res = _dbContext.Set<Bills>().Include(o => o.BillStatus)
                                         .Include(o => o.BillStatus.StatusMaster).Include(o => o.TDSStatus).Include(o => o.TDSStatus.StatusMaster)
                                         .Include(o => o.GSTTDSStatus).Include(o => o.GSTTDSStatus.StatusMaster)
                                         .Include(o => o.Vendor).Include(o => o.Vendor.VendorBalance).Include(o => o.Vendor.VendorDefaults);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }


        }
        public IQueryable<Bills> GetBillForGSTTDS()
        {
            try
            {
                IQueryable<Bills> res = _dbContext.Set<Bills>().Include(o => o.BillStatus)
                                         .Include(o => o.BillStatus.StatusMaster).Include(o => o.TDSStatus).Include(o => o.TDSStatus.StatusMaster)
                                         .Include(o => o.GSTTDSStatus).Include(o => o.GSTTDSStatus.StatusMaster)
                                         .Include(o => o.Vendor).Include(o => o.Vendor.VendorBalance).Include(o => o.Vendor.VendorDefaults).Where(x => (x.BillStatus.StatusMaster.CodeValue == ValueMapping.approved)||(x.BillStatus.StatusMaster.CodeValue == ValueMapping.PartiallyPaid)||(x.BillStatus.StatusMaster.CodeValue == ValueMapping.paid));
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }


        }
        public IQueryable<Bills> GetBillsById(List<int> Ids)
        {

            // IQueryable<Bills> res = _dbContext.Set<Bills>().Include(o => o.Vendor).Include(o => o.Vendor.VendorDefaults).Include(o => o.Vendor.VendorBalance).Include(p => p.Payments).DefaultIfEmpty().Where(o => Ids.Contains(o.Id));
            IQueryable<Bills> res = _dbContext.Set<Bills>().Include(o => o.Vendor).Include(o => o.Vendor.VendorDefaults).Include(o => o.Vendor.VendorBalance).DefaultIfEmpty().Where(o => Ids.Contains(o.ID));
            return res;

        }

        public Task<List<Bills>> GetVendorBillsList(int Id)
        {

            try
            {
                var bills = _dbContext.Set<Bills>().Include(o => o.Vendor)
                      .Include(o => o.Vendor.VendorBalance)
                                                           .Include(o => o.Vendor.VendorDefaults)
                                                           .Include(o => o.BillStatus)
                                                           .Include(o => o.BillStatus.StatusMaster).Where(o => o.VendorId == Id).Where(o => o.BalanceAmount > 0).ToListAsync();

                return bills;
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public async Task<List<BillListDto>> GetAll()
        {

            // var res = await _dbContext.Set<Bills>().Include(o => o.Vendor).Include(o => o.Vendor.VendorDefaults).Include(o => o.Vendor.VendorBalance).Include(p => p.Payments).DefaultIfEmpty().ToListAsync();

            //var res = await _dbContext.Set<Bills>().Include(o => o.Vendor).Include(o => o.Vendor.VendorDefaults).Include(o => o.Vendor.VendorBalance).DefaultIfEmpty().ToListAsync();

            var res = _dbContext.Set<Bills>().Include(o => o.BillStatus).Include(o => o.TDSStatus).Include(o => o.TDSStatus.StatusMaster).Include(o => o.Vendor).Include(o => o.Vendor.VendorBalance).Include(o => o.Vendor.VendorDefaults);
            var billStatus = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.bStatus });
            var status = billStatus.Where(c => c.CodeValue == ValueMapping.billStatus).FirstOrDefault();

            List<BillListDto> billDto = res
       .Select(p => new BillListDto
       {
           BillNumber = p.ID,
           ReferenceNumber = p.BillReferenceNo,
           VendorName = p.Vendor.Name,
           Category = p.Vendor.VendorDefaults.Category,
           BillDate = p.BillDate.ToString("dd-MM-yyyy"),
           DueDate = p.BillDueDate.ToString("dd-MM-yyyy"),
           BillTotal = p.TotalBillAmount,
           TotalGST = p.GSTAmount,
           TDS = p.TDS,
           GSTTDS = p.GSTTDS,
           TotalPayable = p.NetPayable,
           CreatedBy = p.CreatedBy,
           Status = status.CodeName
       }).ToList();


            return billDto;

        }


        //public async Task<Documents> AddDocument(Documents documents)
        //{
        //    string entitytype = "Bill";
        //    documents.EntityType = entitytype;
        //    await _dbContext.AddAsync(documents);
        //    await _dbContext.SaveChangesAsync();
        //    return documents;
        //}

        public async Task<List<BillItems>> GetBillpaymentDetailsByID(int ID)
        {

            var res = await _dbContext.BillItems.Include(o => o.Bills).Include(o => o.Bills.Vendor).Include(o => o.Bills.Vendor.VendorDefaults)
                    .Include(o => o.Bills.Vendor.VendorPerson.Addresses).Where(o => o.BillsID == ID).Include(o => o.Bills.BillStatus).Include(o => o.Bills.BillStatus.StatusMaster).ToListAsync();
            return res;
        }

        public async Task<Bills> UpdateBills(Bills entity)
        {
            try
            {                 
                var blist = _dbContext.Bill;
                var entry = blist.FirstOrDefault(o => o.ID == entity.ID);
                entry.Vendor.VendorBalance.OpeningBalance = entity.Vendor.VendorBalance.OpeningBalance;
                _dbContext.Entry(entry).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Bills> AddBills(Bills entity)
        {
            try
            {
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;

            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}",e.Message);
                return entity;
            }
        }

        public async Task<List<BillItems>> AddBillPaymentDetails(List<BillItems> entity)
        {
            try
            {
                await _dbContext.AddRangeAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }

        }




        public async Task<Bills> UpdateBillsDetails(Bills entity)
        {
            try
            {
                _dbContext.Update(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<Adjustments> UpdateAdjustmentDetails(Adjustments entity)
        {
            try
            {

                _dbContext.Update(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        
        public async Task<List<BillItems>> UpdatePaymentDetails(List<BillItems> entity,string user)
        {
            try
            {
                var billpayment = entity.ToList();

                foreach (var item in billpayment)
                {
                    if (item.Id == 0)
                    {
                        item.CreatedOn = DateTime.UtcNow;
                        item.CreatedBy = user;
                        item.ModifiedOn = DateTime.UtcNow;
                        item.ModifedBy = user;
                        _dbContext.Add(item);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        item.ModifiedOn = DateTime.UtcNow;
                        item.ModifedBy = user;
                        item.CreatedBy = user;
                        _dbContext.Update(item).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                    }
                }
                return entity;
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public async Task<BillItems> DeleteBillDocumentDetails(string ID)
        {
            int BillID = int.Parse(ID);
            var rec = await _dbContext.BillItems.FindAsync(BillID);
            _dbContext.BillItems.Remove(rec);
            await _dbContext.SaveChangesAsync();
            return rec;
        }

        public async Task<BillStatus> AddBillStatus(BillStatus entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Bills> GetBillsByReferenceNo(string billrefno)
        {
            var res = await _dbContext.Bill.Where(x => x.BillReferenceNo == billrefno).Include(o=>o.BillStatus).FirstAsync();
            return res;
        }

        public async Task<List<BillStatus>> GetAllBillStatus(int Id)
        {
            var res = await _dbContext.BillStatus.Where(x => x.BillID == Id).ToListAsync();
            return res;
        }

        public async Task<List<BillStatus>> UpdateBillStatus(List<BillStatus> billStatuses)
        {
            try
            {
                _dbContext.UpdateRange(billStatuses);
                await _dbContext.SaveChangesAsync();
                return billStatuses;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<int> GetBillDetailsByBIllReferenceNo(string number)
        {
            try
            {
                var res = await _dbContext.Bill.Where(x => x.BillReferenceNo == number).FirstAsync();
                var Id = res.ID;
                return Id;

            }
            catch (Exception)
            {

                throw;
            }
            
        }


        public IQueryable<Adjustments> GetAdjustmentList(int VendorId)
        {
            try
            {    
                IQueryable<Adjustments> res = _dbContext.Set<Adjustments>().Include(o => o.AdjustmentStatus).Include(o => o.Vendor).Include(o => o.AdjustmentStatus.StatusMaster).Where(o => o.VendorID == VendorId);           
                
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public async Task<Adjustments> GetAdjustmentDetails(int Id)
        {
            try
            {
                var res = _dbContext.Set<Adjustments>().Include(o => o.AdjustmentStatus)
                    .Include(o => o.Vendor).Include( o => o.Vendor.VendorDefaults)
                    .Include(o => o.Vendor.VendorBalance).Include(o => o.Vendor.VendorPerson.Addresses)
                    .Include(o => o.Vendor.VendorPerson.Contacts).Include(o => o.Vendor.VendorBankAccounts)                      
                     .Include(o => o.Vendor.VendorBankAccounts.BranchMaster).Include(o => o.Vendor.VendorBankAccounts.BranchMaster.BankDetails)
                    .Include(o => o.AdjustmentStatus.StatusMaster).Where(o => o.ID == Id).First();
       
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}






