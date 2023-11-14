using Application.DTOs.Bank;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bank.Requests.Commands
{
    public class CreateBankStatementInputCommand : IRequest<BankStatementInputDto>
    {
        public BankStatementInputDto bankStatementInput { get; set; }
        public string user { get; set; }
    }
}
