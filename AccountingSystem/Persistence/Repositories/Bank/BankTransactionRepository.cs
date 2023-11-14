using Application.Interface.Persistence.Bank;
using Domain.Bank;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Bank
{
    public class BankTransactionRepository : IBankTransactionRepository
    {
        private readonly AccountingDbContext _dbContext;
        public BankTransactionRepository(AccountingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task <BankTransactions> GetBankTransactionById(int bankTransactionId)
        {
            try
            {
              var res=  _dbContext.BankTransactions.Where(x=>x.Id==bankTransactionId).FirstOrDefault();
                return res; 
            }
            catch(Exception ex)
            {
                throw;
            }
        }   

        public async Task<IQueryable<BankTransactions>> GetBankTransactionList()
        {
            try
            {
                IQueryable<BankTransactions> res =  _dbContext.BankTransactions.AsQueryable();

                // filter by whether there are any associated Reconciliation rows
                return res;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<BankTransactions> UpdateBankTransactionsStatus(BankTransactions bankTransaction)
        {
            try
            {
                _dbContext.BankTransactions.Update(bankTransaction);    
                await _dbContext.SaveChangesAsync();
               
                return bankTransaction;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
