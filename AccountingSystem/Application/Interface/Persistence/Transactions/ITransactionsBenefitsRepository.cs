using Application.Interface.Persistence.Generic;
using Domain.TransactionsBenefits;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Transactions
{
    public interface ITransactionsBenefitsRepository:IGenericRepository<TransactionsBenefits>
    {
        Task<List<TransactionsBenefits>> UpdateListtOfBenefitsTransaction(List<TransactionsBenefits> transactionList);
    }
}
