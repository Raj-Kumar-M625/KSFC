using Application.DTOs.Bank;
using Domain.Bank;
using Domain.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Bank
{
    public interface IBankStatementInputRepository
    {
        Task<BankStatementInput> AddBankStatementInput(BankStatementInput entity);
        IQueryable<BankStatementInput> GetBankStatementInputList();
    }
}
