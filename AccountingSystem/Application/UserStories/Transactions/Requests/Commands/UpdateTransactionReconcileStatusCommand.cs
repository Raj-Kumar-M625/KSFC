using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Requests.Commands
{
    public class UpdateTransactionReconcileStatusCommand:IRequest<int>
    {
        public List<TransactionsSummary> TransactionsSummary { get; set; }  
    }
}
