using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.Generic;
using Domain.BankFile;
using Domain.GenarteBankfile;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.BankFile
{
    public class BankFileRepository : GenericRepository<BankFileUtrDetails>, IBankFileRepository
    {
        private readonly AccountingDbContext _dbContext;
        //Constructor for the Payment Repository
        public BankFileRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BankFileUtrDetails> AddBankUTRDetails(BankFileUtrDetails bankFileUTRDetails)
        {
            try
            {
                await _dbContext.BankFileUTRDetails.AddAsync(bankFileUTRDetails);
                await _dbContext.SaveChangesAsync();

                return bankFileUTRDetails;
            }
           catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MappingGenBankFileUtrDetails> AddMappingForBankFile(List<MappingGenBankFileUtrDetails> entity)
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

        public List<int> GetPaymnetIDByGenBankFileID(List<int> genBankFileID)
        {
            var res = _dbContext.MappingPaymentGenerateBankFile.Where(x => genBankFileID.Any(y => y.Equals(x.GenerateBankFileId))).Select(x => x.PaymentId).ToList();
            return res;
        }

        public List<string> GetBillreferenceIDByGenBankFileID(List<int> genBankFileID)
        {
            var res = _dbContext.MappingPaymentGenerateBankFile.Where(x => genBankFileID.Any(y => y.Equals(x.GenerateBankFileId))).Select(x => x.Payments.PaymentBillReference).ToList();
            return res;
        }


        IQueryable<BankFileUtrDetails> IBankFileRepository.GetAll()
        {
            throw new NotImplementedException();
        }

        public async  Task<GenerateBankFile> GetGeneratedBankFielById(int? genBankFielId)
        {
            try
            {
                var genratedBankFile = await _dbContext.GenerateBankFile.Include(x=>x.Bank).Where(x => x.Id == genBankFielId).FirstOrDefaultAsync();
                return genratedBankFile;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
