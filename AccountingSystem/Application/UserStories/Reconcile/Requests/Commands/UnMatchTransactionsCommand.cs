using Application.DTOs.Bank;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Reconcile.Requests.Commands
{
    public class UnMatchTransactionsCommand:IRequest<int>
    {
        public List<ReconciliationDto> selectedTransactions { get; set; }
        public int bankTransactionId { get; set; }   
        public string currentUser { get; set; }
        public List<int> unSelectedtransactions { get; set; }
    }
}
