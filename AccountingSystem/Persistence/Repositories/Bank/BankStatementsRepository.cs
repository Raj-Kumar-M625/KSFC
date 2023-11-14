using Application.Interface.Persistence.Bank;
using Common.OutputSearchCriteria;
using Domain.Bill;
using Domain.Uploads;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Bank
{
    public class BankStatementsRepository : IBankStatementsRepository
    {
        private readonly AccountingDbContext _dbContext;

        public BankStatementsRepository(AccountingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BankStatementInputTransaction>> AddBankStatements(List<BankStatementInputTransaction> entity)
        {
            try
            {
                await _dbContext.AddRangeAsync(entity);
                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<BankStatementInputTransaction>> GetBankStatements()
        {
            var bankStatements = await _dbContext.BankStatementInputTransaction.Take(100).ToListAsync();
            return bankStatements;
        }

        public async Task<IEnumerable<BankStatementInputTransaction>> GetBankStatements(GenericOutputSearchCriteria filter)
        {

            IEnumerable<BankStatementInputTransaction> bankStatements = _dbContext.BankStatementInputTransaction.Take(500).ToList();

            if (filter.ApplyTransactionDateFilter)
            {
                bankStatements = bankStatements.Where(x => x.Transaction_Date >= filter.DateFrom && x.Transaction_Date <= filter.DateTo);
            }
            if (filter.ApplyAmountFilter)
            {
                bankStatements = bankStatements.Where(x => x.Debit >= filter.MinAmount && x.Debit <= filter.MaxAmount
                                                        || x.Credit >= filter.MinAmount && x.Credit <= filter.MaxAmount);
            }
            if (filter.ApplyTransactionTypeFilter)
            {
                if (filter.TransactionType == "Credit")
                {
                    bankStatements = bankStatements.Where(x => x.Credit != null && x.Credit != 0);
                }
                else if (filter.TransactionType == "Debit")
                {
                    bankStatements = bankStatements.Where(x => x.Debit != null && x.Debit != 0);
                }
            }
            if (filter.ApplyBankNameFilter)
            {
                bankStatements = bankStatements.Where(x => x.BankName.Equals(filter.BankName));
            }
            if (filter.ApplyAccountNumberFilter)
            {
                bankStatements = bankStatements.Where(x => x.AccountNo.Equals(filter.AccountNo));
            }
            if (filter.ApplyFileNameFilter)
            {
                bankStatements = bankStatements.Where(x => x.FileName.Equals(filter.FileName));
            }

            return bankStatements;
        }
    }
}
