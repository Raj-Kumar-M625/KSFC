using Application.DTOs.Bank;
using Common.InputSearchCriteria;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bank.Requests.Queries
{
    public class GetBankStatementsFilterListRequest : IRequest<IEnumerable<BankStatementsListDto>>
    {
        public GenericInputSearchCriteria searchCriteria { get; set; }

    }
}
