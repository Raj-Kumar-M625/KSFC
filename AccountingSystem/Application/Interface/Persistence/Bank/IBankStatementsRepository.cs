using Common.OutputSearchCriteria;
using Domain.Bill;
using Domain.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Bank
{
    public interface IBankStatementsRepository
    {
        Task<IEnumerable<BankStatementInputTransaction>> GetBankStatements();
        Task<IEnumerable<BankStatementInputTransaction>> GetBankStatements(GenericOutputSearchCriteria filter);
        Task<List<BankStatementInputTransaction>> AddBankStatements(List<BankStatementInputTransaction> entity);
    }
}
