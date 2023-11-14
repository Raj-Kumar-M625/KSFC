using Application.DTOs.Filters;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Requests.Queries
{
    public class GetListOfTransactionsByFiltersRequest : IRequest<IQueryable<TransactionsSummary>>
    {
        public ReconcileDto reconcileDto { get; set; }
    }
}
