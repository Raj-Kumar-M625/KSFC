using Application.DTOs.GSTTDS;
using Application.Interface.Persistence.GSTTDS;
using Application.Interface.Persistence.Master;
using Application.UserStories.GSTTDS.Requests.Queries;
using Common.ConstantVariables;
using Domain.GSTTDS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GSTTDS.Handlers.Queries
{
    public class GetGsttdsPaymentChallanListRequestHandler : IRequestHandler<GetGsttdsPaymentChallanListRequest, IQueryable<GsstPaymentChallanDto>>
    {
        private readonly IGstdsPaymentChallanRepository _gstTdsPaymentChallanRepository;
        private readonly ICommonMasterRepository _commonMaster;

        public GetGsttdsPaymentChallanListRequestHandler(IGstdsPaymentChallanRepository gstTdsPaymentChallanRepository, ICommonMasterRepository commonMaster)
        {
            _gstTdsPaymentChallanRepository = gstTdsPaymentChallanRepository;
            _commonMaster = commonMaster;
        }

        public async Task<IQueryable<GsstPaymentChallanDto>> Handle(GetGsttdsPaymentChallanListRequest request, CancellationToken cancellationToken)
        {
            var res = _gstTdsPaymentChallanRepository.GetGSTTDSPaymentChallanList();

            var gst = res.Where(x => x.CodeValue == ValueMapping.ChallanCreated);
           await _commonMaster.GetCommoMasterValues(ValueMapping.gstTdsStatus);

            return gst;
        }
    }
}
