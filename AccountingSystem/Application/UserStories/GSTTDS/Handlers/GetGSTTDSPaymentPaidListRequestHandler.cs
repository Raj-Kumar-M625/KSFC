using Application.Interface.Persistence.GSTTDS;
using Application.UserStories.GSTTDS.Requests.Queries;
using Domain.GSTTDS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GSTTDS.Handlers
{
    public class GetGsttdsPaymentPaidListRequestHandler : IRequestHandler<GetGsttdsPaymentPaidListRequest, IQueryable<GsttdsPaymentChallan>>
    {
        private readonly IGstdsPaymentChallanRepository _gstTdsPaymentChallanRepository;
        public GetGsttdsPaymentPaidListRequestHandler(IGstdsPaymentChallanRepository gstTdsPaymentChallanRepository)
        {
            _gstTdsPaymentChallanRepository = gstTdsPaymentChallanRepository;
        }
        public async Task<IQueryable<GsttdsPaymentChallan>> Handle(GetGsttdsPaymentPaidListRequest request, CancellationToken cancellationToken)
        {
            var res = _gstTdsPaymentChallanRepository.GetGSTTDSPaidList();
            return res;
        }
    }
}
