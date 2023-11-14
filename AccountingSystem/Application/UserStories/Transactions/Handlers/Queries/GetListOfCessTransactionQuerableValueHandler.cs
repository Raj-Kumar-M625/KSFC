using Application.Interface.Persistence.Transactions;
using Application.UserStories.Transactions.Requests.Queries;
using AutoMapper;
using Domain.CessTransactions;
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
   
        public class GetListOfCessTransactionQuerableValueHandler : IRequestHandler<GetListOfCessTransactionQuerableValue, IQueryable<Transaction>>
        {

            private readonly ITransactionRepository _transactionRepository;
            private readonly IMapper _mapper;

            public GetListOfCessTransactionQuerableValueHandler(ITransactionRepository transactionRepository, IMapper mapper)
            {
                _transactionRepository = transactionRepository;
                _mapper = mapper;
            }

            public async Task<IQueryable<Transaction>> Handle(GetListOfCessTransactionQuerableValue request, CancellationToken cancellationToken)
            {
                var listOfCessTransaction = _transactionRepository.GetListOfCessTransaction();
                return listOfCessTransaction;
            }
        }    
}
