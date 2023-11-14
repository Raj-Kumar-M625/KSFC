using Application.DTOs.Bank;
using Application.Interface.Persistence.Bank;
using Application.UserStories.Bank.Requests.Queries;
using AutoMapper;
using Common.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bank.Handlers.Queries
{
    public class GetBankStatementsFilterListRequestHandler : IRequestHandler<GetBankStatementsFilterListRequest, IEnumerable<BankStatementsListDto>>
    {
        private readonly IBankStatementsRepository _bankStatements;
        private readonly IMapper _mapper;

        public GetBankStatementsFilterListRequestHandler(IBankStatementsRepository bankStatements, IMapper mapper)
        {
            _bankStatements = bankStatements;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BankStatementsListDto>> Handle(GetBankStatementsFilterListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var filters = HelperSearchCriteria.ParseBankStatementsSearchCriteria(request.searchCriteria);
                var bankStatements = await _bankStatements.GetBankStatements(filters);
                return _mapper.Map<IEnumerable<BankStatementsListDto>>(bankStatements);

            }catch(Exception e)
            {
                throw e;
            }
            
        }
    }
}
