using Application.DTOs.Bank;
using Application.DTOs.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bank.Requests.Commands
{
    public class CreateBankStatementCommand : IRequest<BankStatementsListDto>
    {
        public List<BankStatementsListDto> bankStatements { get; set; }
        public string user { get; set; }
    }
}
