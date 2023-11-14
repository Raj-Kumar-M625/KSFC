using Application.DTOs.Bank;
using Application.DTOs.Filters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Reconcile.Requests.Commands
{
    public class UpdateReconciledTransactionsCommand:IRequest<int>
    {
        public List<ReconciliationDto> selectedTransactions { get; set; }
        public List<ReconciliationDto> unSelectedTransactions { get; set; }
        public List<int> selectedTransactionsId { get; set; }

        public string currentUser { get; set; }
        public int bankTransactionId { get; set; }
    }
}
