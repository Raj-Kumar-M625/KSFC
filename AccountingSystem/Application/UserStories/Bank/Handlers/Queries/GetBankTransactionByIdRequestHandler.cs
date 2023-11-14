using Application.DTOs.Bank;
using Application.Interface.Persistence.Bank;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Reconcile;
using Application.UserStories.Bank.Requests.Queries;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
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
    public class GetBankTransactionByIdRequestHandler : IRequestHandler<GetBankTransactionByIdRequest, BankTransactionDto>
    {
        private readonly IBankTransactionRepository _bankTransaction;
        private readonly IMapper _mapper;
        public GetBankTransactionByIdRequestHandler(IBankTransactionRepository bankTransaction, IMapper mapper)
        {
            _bankTransaction = bankTransaction;
            _mapper = mapper;
        }

        public async Task<BankTransactionDto> Handle(GetBankTransactionByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var bankTransaction = await _bankTransaction.GetBankTransactionById(request.bankTransactionId);
                return _mapper.Map<BankTransactionDto>(bankTransaction);

            }
            catch (Exception ex) 
            {
                throw;
            }
        }
    }
}
