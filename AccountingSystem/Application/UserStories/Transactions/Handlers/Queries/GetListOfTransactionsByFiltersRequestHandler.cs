using Application.Interface.Persistence.Transactions;
using Application.UserStories.Transactions.Requests.Queries;
using AutoMapper;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Handlers.Queries
{
    public class GetListOfTransactionsByFiltersRequestHandler : IRequestHandler<GetListOfTransactionsByFiltersRequest, IQueryable<TransactionsSummary>>
    {
        private readonly ITransactionSummaryRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetListOfTransactionsByFiltersRequestHandler(ITransactionSummaryRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<TransactionsSummary>> Handle(GetListOfTransactionsByFiltersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.reconcileDto.Date != null)
                {
                    DateTime startDate = default(DateTime);
                    DateTime endDate = default(DateTime);
                    DateTime dt;
                    var date = request.reconcileDto.Date.Substring(0, 10);
                    var res = DateTime.TryParse(date, out dt);
                    if (res)
                        startDate = DateTime.Parse(date);
                    else
                        startDate = DateTime.Parse(request.reconcileDto.Date.Substring(0, 9));

                    var dateend = request.reconcileDto.Date.Substring(11);
                    var resend = DateTime.TryParse(dateend, out dt);
                    if (resend)
                        endDate = DateTime.Parse(dateend);
                    else
                        endDate = DateTime.Parse(request.reconcileDto.Date.Substring(12, 11));

                    request.reconcileDto.StartDate = startDate;
                    request.reconcileDto.EndDate = endDate;
                }
                var transactions = await _transactionRepository.GetAllTransactions(request.reconcileDto);
                return transactions;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
