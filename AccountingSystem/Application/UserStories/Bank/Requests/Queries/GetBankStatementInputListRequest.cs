using Application.DTOs.Bank;
using Domain.Bank;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bank.Requests.Queries
{
    public class GetBankStatementInputListRequest : IRequest<IQueryable<BankStatementInput>>
    {
    }
}
