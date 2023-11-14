using Application.Interface.Persistence.Transactions;
using Application.Interface.Persistence.Vendor;
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
    public class GetListOfTransactionQuerableValueHandler : IRequestHandler<GetListOfTransactionQuerableValue, IQueryable<Transaction>>
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetListOfTransactionQuerableValueHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<Transaction>> Handle(GetListOfTransactionQuerableValue request, CancellationToken cancellationToken)
        {

            List<Transaction> transaction = new List<Transaction>();
            var ts = _transactionRepository.GetListOfTransaction(); 
            return ts;
            
        }
    }

}
