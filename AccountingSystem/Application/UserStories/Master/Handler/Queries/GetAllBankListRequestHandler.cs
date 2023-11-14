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
    public class GetAllBankListRequestHandler : IRequestHandler<GetAllBanksListRequest, List<BankMasterDto>>
    {
        private readonly IBankMasterRepository _bankMaster;
        private readonly IMapper _mapper;

        public GetAllBankListRequestHandler(IBankMasterRepository bankMaster, IMapper mapper)
        {
            _bankMaster = bankMaster;
            _mapper = mapper;
        }
        public async Task<List<BankMasterDto>> Handle(GetAllBanksListRequest request, CancellationToken cancellationToken)
        {
            var banklist = await _bankMaster.GetAllBanks();
            return _mapper.Map<List<BankMasterDto>>(banklist);
        }
    }
}
