using Application.DTOs.Bank;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Reconcile.Requests.Commands
{
    public class ManuallyReconcileTransactionsCommand:IRequest<int>
    {
        public List<TransactionsSummary> TransactionsSummary { get; set; }
        public BankTransactionDto BankTransaction { get; set; } 
        public string currentUser { get; set; } 
    }
}
