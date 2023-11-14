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
    public class GetCommonMasterListRequestHandler : IRequestHandler<GetCommonMasterListRequest, List<CommonMasterDto>>
    {
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMapper _mapper;

        public GetCommonMasterListRequestHandler(ICommonMasterRepository commonMaster, IMapper mapper)
        {
            _commonMaster = commonMaster;
            _mapper = mapper;
        }
        public async Task<List<CommonMasterDto>> Handle(GetCommonMasterListRequest request, CancellationToken cancellationToken)
        {
            var commonMasterList = await _commonMaster.GetCommoMasterValues(request.CodeType);
            return _mapper.Map<List<CommonMasterDto>>(commonMasterList);
        }
    }
}
