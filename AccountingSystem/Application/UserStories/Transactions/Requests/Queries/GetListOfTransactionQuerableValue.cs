using Domain.Transactions;
using Domain.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Requests.Queries
{
    
    public class GetListOfTransactionQuerableValue : IRequest<IQueryable<Transaction>>
    {
    }
}
