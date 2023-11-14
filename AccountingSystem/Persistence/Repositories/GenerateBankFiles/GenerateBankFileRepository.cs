using Application.DTOs.GenerateBankFile;
using Application.Interface.Persistence.GenerateBankFiles;
using Domain.GenarteBankfile;
using Domain.GenerateBankfile;
using Domain.Master;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.GenerateBankFiles
{
    public class GenerateBankFileRepository : GenericRepository<GenerateBankFile>, IGenerateBankFileRepository
    {
        private readonly AccountingDbContext _dbContext;
        //Constructor for the Generate Bank File Repository
        public GenerateBankFileRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GenerateBankFile> AddGeneratedBankFile(GenerateBankFile genbankFile)
        {
            try
            {
                genbankFile.ModifedBy = genbankFile.CreatedBy;
                await _dbContext.GenerateBankFile.AddAsync(genbankFile);
                await _dbContext.SaveChangesAsync();
                return genbankFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<GenerateBankFileStatus> AddGeneratedBankFileStatus(GenerateBankFileStatus genbankFileStatus)
        {
            try
            {
                await _dbContext.AddAsync(genbankFileStatus);
                await _dbContext.SaveChangesAsync();
                return genbankFileStatus;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MappingPaymentGenerateBankFile> AddMappingGenerateBankFile(MappingPaymentGenerateBankFile entity)
        {
            try
            {
                if (entity.Id > 0)
                {
                    entity.Id = 0;
                }
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<GenerateBankFile> GetAllGenerateBankFile()
        {
            //IQueryable<GenerateBankFile> res = _dbContext.Set<GenerateBankFile>()
            //    .Include(o => o.GenerateBankFileStatus).Include(x => x.GenerateBankFileStatus.StatusMaster).Include(x => x.Bank);
            //return res;

            try
            {
                var bankfilestatus = from ts in _dbContext.GenerateBankFileStatus
                                     join cm in _dbContext.CommonMaster on ts.StatusCMID equals cm.Id into gj
                                     from cm in gj.DefaultIfEmpty()
                                     group ts by new
                                     {
                                         ts.GenerateBankFileId,
                                         cm.CodeValue,
                                         cm.CodeName
                                     } into newgroup
                                     select new
                                     {
                                         GenerateBankFileId = newgroup.Key.GenerateBankFileId,
                                         CodeName = newgroup.Key.CodeName,
                                         CodeValue = newgroup.Key.CodeValue
                                     };


                var result = from mu in _dbContext.MappingGenBankFileUTRDetails
                             join bu in _dbContext.BankFileUTRDetails on mu.BankFileUTRId equals bu.Id into gs
                             from bu in gs.DefaultIfEmpty()
                             group mu by new
                             {
                                 mu.GenerateBankFileId,
                                 bu.DifferentBankUTRNumber,
                                 bu.SameBankUTRNumber
                             } into newgroup
                             select new
                             {
                                 GenerateBankFileId = newgroup.Key.GenerateBankFileId,
                                 DifferentBankUTRNumber = newgroup.Key.DifferentBankUTRNumber,
                                 SameBankUTRNumber = newgroup.Key.SameBankUTRNumber
                             };


                var res = from td in _dbContext.GenerateBankFile
                          join ts in bankfilestatus on td.Id equals ts.GenerateBankFileId
                          join bm in _dbContext.BankMaster on td.BankMasterId equals bm.Id
                          join mmp in result on td.Id equals mmp.GenerateBankFileId into ms
                          from mmp in ms.DefaultIfEmpty()
                          select new GenerateBankFile
                          {
                              Id = td.Id,
                              BankFileReferenceNo=td.BankFileReferenceNo,
                              CreatedOn=td.CreatedOn,
                              CreatedBy=td.CreatedBy,
                              AccountNo=td.AccountNo,
                              NoOfVendors=td.NoOfVendors,
                              NoOfTransactions=td.NoOfTransactions,
                              TotalAmount=td.TotalAmount,
                              BankMasterId=bm.Id,
                              ModifedBy=td.ModifedBy,
                             // AccountNo = bm.Accountnumber,
                               DifferentBankUTRNumber = mmp.DifferentBankUTRNumber,
                              SameBankUTRNumber = mmp.SameBankUTRNumber,
                              CodeName = ts.CodeName,
                              CodeValue = ts.CodeValue,
                              Bank = bm
                             

                          };

                return res;




            }
            catch(Exception ex)
            {
                throw ex;
            }



        }

        public IQueryable<GenerateBankFileStatus> GetGenerateBankFileStatus(List<int> Id)
        {
            var res = _dbContext.GenerateBankFileStatus.Where(x => Id.Any(y => y == x.GenerateBankFileId));
            return res;
        }

        public async Task<List<MappingPaymentGenerateBankFile>> GetGeneratedBankFileById(int Id)
        {
            var res = _dbContext.MappingPaymentGenerateBankFile.Where(x => x.GenerateBankFileId == Id)
                .Include(x => x.Payments)
                .Include(x => x.Payments.Vendor)
                .Include(x => x.Payments.Vendor.VendorBankAccounts)
                .Include(x => x.Payments.Vendor.VendorBankAccounts.BranchMaster)
                .Include(x => x.Payments.Vendor.VendorBankAccounts.BranchMaster.BankDetails)
                .Include(x => x.Payments.PaymentStatus)
                .Include(x => x.Payments.PaymentStatus.StatusMaster)
                .Include(x => x.GenerateBankFile).ToList();
            return res;
        }


        public List<MappingPaymentGenerateBankFile> GetGeneratedBankFile(int Id)
        {
            var res = _dbContext.MappingPaymentGenerateBankFile.Where(x => x.GenerateBankFileId == Id).Include(x => x.Payments).Include(x => x.Payments.Vendor).Include(x => x.Payments.Vendor.VendorBankAccounts).Include(x => x.Payments.Vendor.VendorBankAccounts.BranchMaster).Include(x => x.Payments.Vendor.VendorBankAccounts.BranchMaster.BankDetails)
                .Include(x => x.Payments.Vendor.VendorBankAccounts)
                .Include(x => x.Payments.PaymentStatus)
                .Include(x => x.Payments.PaymentStatus.StatusMaster)
                .Include(x => x.GenerateBankFile).ToList();
            return res;
        }


        public async Task<GenerateBankFileStatus> UpdateGeneratedBankFileStatus(List<GenerateBankFileStatus> genbankFileStatus)
        {
            try
            {
                _dbContext.UpdateRange(genbankFileStatus);
                await _dbContext.SaveChangesAsync();
                return genbankFileStatus.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
