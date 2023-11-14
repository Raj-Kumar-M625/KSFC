using Application.Interface.Persistence.Generic;
using Domain.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Reconcile
{
    public interface IReconciliationRepository:IGenericRepository<Reconciliation>
    {
        Task<List<Reconciliation>> GetReconciliationsByBankTransactionId(int bankTransactionId);
        Task<List<Reconciliation>> GetReconciliationsByTransactionIds(List<Reconciliation> selectedTransactions, int bankTransactionId);
        Task<List<Reconciliation>> UpdateReconciledTransactions(List<Reconciliation> reconciledTransactions);
        Task<List<Reconciliation>> AddReconciliationList(List<Reconciliation> reconciledTransactions);
        Task<List<int>> GetReconciliationsByBankTransactionIds(List<int> bankTransactionId);
    }
}
