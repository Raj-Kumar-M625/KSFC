using Application.DTOs.Bank;
using Application.Interface.Persistence.Bank;
using Application.UserStories.Bank.Requests.Commands;
using AutoMapper;
using Domain.Bank;
using Domain.Uploads;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bank.Handlers.Commands
{
    public class CreateBankStatementInputHandler : IRequestHandler<CreateBankStatementInputCommand, BankStatementInputDto>
    {
        private readonly IBankStatementInputRepository _bankStatementInput;
        private readonly IMapper _mapper;
        public CreateBankStatementInputHandler(IBankStatementInputRepository bankStatementInput, IMapper mapper)
        {
            _bankStatementInput = bankStatementInput;
            _mapper = mapper;
        }

        public async  Task<BankStatementInputDto> Handle(CreateBankStatementInputCommand request, CancellationToken cancellationToken)
        {
            var statement = _mapper.Map<BankStatementInput>(request.bankStatementInput);
            var bank = await _bankStatementInput.AddBankStatementInput(statement);
            var response = _mapper.Map<BankStatementInputDto>(bank);
            return response;
        }
    }
}
