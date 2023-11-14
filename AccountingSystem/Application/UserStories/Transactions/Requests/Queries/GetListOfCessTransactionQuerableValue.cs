using Domain.CessTransactions;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Requests.Queries
{
    public class GetListOfCessTransactionQuerableValue : IRequest<IQueryable<Transaction>>
    {
    }
}
