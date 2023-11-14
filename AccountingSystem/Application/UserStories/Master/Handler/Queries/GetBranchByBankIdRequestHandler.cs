using Application.DTOs.Master;
using Application.DTOs.Payment;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
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
    public class GetBranchByBankIdRequestHandler : IRequestHandler<GetBranchByBankIdRequest, List<BranchMasterDto>>
    {

        private readonly IBankDetailsRepositoty _bankDetailsRepositoty;
        private readonly IMapper _mapper;

        public GetBranchByBankIdRequestHandler(IBankDetailsRepositoty bankDetailsRepositoty, IMapper mapper)
        {
            _bankDetailsRepositoty = bankDetailsRepositoty;
            _mapper = mapper;
            
        }


        public async Task<List<BranchMasterDto>> Handle(GetBranchByBankIdRequest request, CancellationToken cancellationToken)
        {
            var BranchDetails = await _bankDetailsRepositoty.GetBranchDetails(request.Id);
            return _mapper.Map<List<BranchMasterDto>>(BranchDetails);
        }
    }
}
