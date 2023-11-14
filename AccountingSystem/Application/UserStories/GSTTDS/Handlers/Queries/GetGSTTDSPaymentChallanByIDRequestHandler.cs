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

namespace Application.UserStories.GSTTDS.Handlers.Queries
{
    public class GetGsttdsPaymentChallanByIDRequestHandler : IRequestHandler<GetGsttdsPaymentChallanByIDRequest, GsttdsPaymentChallan>
    {
        private readonly IGstdsPaymentChallanRepository _gstTdsPaymentChallanRepository;
        public GetGsttdsPaymentChallanByIDRequestHandler(IGstdsPaymentChallanRepository gstTdsPaymentChallanRepository)
        {
            _gstTdsPaymentChallanRepository = gstTdsPaymentChallanRepository;
        }

        public async Task<GsttdsPaymentChallan> Handle(GetGsttdsPaymentChallanByIDRequest request, CancellationToken cancellationToken)
        {
            return await _gstTdsPaymentChallanRepository.GetGSTTDSPaymentChallanAsync(request.ID);
        }
    }
}
