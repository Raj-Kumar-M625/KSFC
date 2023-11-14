using Application.DTOs.Bank;
using Application.Interface.Persistence.Bank;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Bank;
using Domain.Bill;
using Domain.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Bank
{
    public  class BankStatementInputRepository : IBankStatementInputRepository
    {
        private readonly AccountingDbContext _dbContext;

        public BankStatementInputRepository(AccountingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BankStatementInput> AddBankStatementInput(BankStatementInput entity)
        {
            try
            {
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IQueryable<BankStatementInput> GetBankStatementInputList()
        {
            try
            {
                IQueryable<BankStatementInput> res = _dbContext.Set<BankStatementInput>();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
