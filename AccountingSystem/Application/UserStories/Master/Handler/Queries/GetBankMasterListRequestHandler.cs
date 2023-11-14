using Application.DTOs.Master;
using Application.Interface.Persistence.Master;
using Application.UserStories.Master.Request.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Master.Handler.Queries
{
    public class GetBankMasterListRequestHandler : IRequestHandler<GetBankMasterListRequest, List<BankMasterDto>>
    {
        private readonly IBankMasterRepository _bankMaster;
        private readonly IMapper _mapper;

        public GetBankMasterListRequestHandler(IBankMasterRepository bankMaster, IMapper mapper)
        {
            _bankMaster = bankMaster;
            _mapper = mapper;
        }

        public async Task<List<BankMasterDto>> Handle(GetBankMasterListRequest request, CancellationToken cancellationToken)
        {
            var banklist = await _bankMaster.GetBankMasterList();
            return _mapper.Map<List<BankMasterDto>>(banklist);
        }
    }
}
