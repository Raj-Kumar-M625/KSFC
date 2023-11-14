using Application.DTOs.Master;
using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.Master;
using Application.UserStories.Master.Request.Queries;
using AutoMapper;
using Domain.Master;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Master.Handler.Queries
{
    public class GetBankDetailsListRequestHandler : IRequestHandler<GetBankDetailsRequest, List<BranchMasterDto>>
    {
       
        private readonly IBankDetailsRepositoty _bankMaster;
        private readonly IMapper _mapper;

        public GetBankDetailsListRequestHandler(IBankDetailsRepositoty bankMaster, IMapper mapper)
        {
            _bankMaster = bankMaster;
            _mapper = mapper;
        }

        public async Task<List<BranchMasterDto>> Handle(GetBankDetailsRequest request, CancellationToken cancellationToken)
        {
            var banklist = await _bankMaster.GetBankDetailsList();
            return _mapper.Map<List<BranchMasterDto>>(banklist);
        }
    }
}
