using Application.DTOs.Bank;
using Application.DTOs.Bill;
using Application.Interface.Persistence.Bank;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Master;
using Application.UserStories.Bank.Requests.Commands;
using Application.UserStories.Bill.Requests.Commands;
using AutoMapper;
using DocumentFormat.OpenXml.Presentation;
using Domain.Bill;
using Domain.Master;
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
    public class CreateBankStatementHandler : IRequestHandler<CreateBankStatementCommand, BankStatementsListDto>
    {
        private readonly IBankStatementsRepository _bankStatements;
        private readonly IMapper _mapper;
        public CreateBankStatementHandler(IBankStatementsRepository bankStatements, IMapper mapper)
        {
            _bankStatements = bankStatements;
            _mapper = mapper;
        }

        public async Task<BankStatementsListDto> Handle(CreateBankStatementCommand request, CancellationToken cancellationToken)
        {
            var bankStatement = request.bankStatements.ToList();
            var statement = _mapper.Map<List<BankStatementInputTransaction>>(bankStatement);
            var bank = await _bankStatements.AddBankStatements(statement);
            var response = _mapper.Map<List<BankStatementsListDto>>(bank);
            return response[0];
        }
    }
}
