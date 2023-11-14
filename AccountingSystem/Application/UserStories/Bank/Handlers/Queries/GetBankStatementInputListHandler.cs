using Application.DTOs.Bank;
using Application.Interface.Persistence.Bank;
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
    public class GetBankStatementInputListHandler : IRequestHandler<GetBankStatementInputListRequest, IQueryable<BankStatementInput>>
    {
        private readonly IBankStatementInputRepository _bankStatementInputRepository;
        private readonly IMapper _mapper;

        public GetBankStatementInputListHandler(IBankStatementInputRepository bankStatementInputRepository, IMapper mapper)
        {
            _bankStatementInputRepository = bankStatementInputRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<BankStatementInput>> Handle(GetBankStatementInputListRequest request, CancellationToken cancellationToken)
        {
            var bankStatementInput = _bankStatementInputRepository.GetBankStatementInputList();
            return bankStatementInput;
        }
    }
}
