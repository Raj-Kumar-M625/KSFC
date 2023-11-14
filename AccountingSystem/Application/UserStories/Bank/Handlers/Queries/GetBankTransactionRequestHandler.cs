using Application.DTOs.Bank;
using Application.Interface.Persistence.Bank;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Payment;
using Application.UserStories.Bank.Requests.Queries;
using AutoMapper;
using Domain.Bank;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bank.Handlers.Queries
{
    public class GetBankTransactionRequestHandler : IRequestHandler<GetBankTransactionRequest, IQueryable<BankTransactions>>
    {
        private readonly IBankTransactionRepository _bankTransaction;
        private readonly IMapper _mapper;
        public GetBankTransactionRequestHandler(IBankTransactionRepository bankTransaction, IMapper mapper)
        {
            _bankTransaction = bankTransaction;
            _mapper = mapper;
        }

        public async Task<IQueryable<BankTransactions>> Handle(GetBankTransactionRequest request, CancellationToken cancellationToken)
        {
            var bankTransaction = await  _bankTransaction.GetBankTransactionList();
            return bankTransaction;
        }
    }
}
