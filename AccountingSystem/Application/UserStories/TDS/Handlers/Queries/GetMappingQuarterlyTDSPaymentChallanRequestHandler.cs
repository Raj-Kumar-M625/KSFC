using Application.Interface.Persistence.Payment;
using Application.UserStories.TDS.Requests.Queries;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Handlers.Queries
{
    public class GetMappingQuarterlyTdsPaymentChallanRequestHandler : IRequestHandler<GetMappingQuarterlyTdsPaymentChallanByTdsChallanIdRequest, MappingTdsQuarterChallan>
    {
        private readonly IQuarterlyTdsPaymentChallanRepository _quarterlyTDSPaymentChallanRepository;
        public GetMappingQuarterlyTdsPaymentChallanRequestHandler(IQuarterlyTdsPaymentChallanRepository quarterlyTDSPaymentChallanRepository)
        {
            _quarterlyTDSPaymentChallanRepository = quarterlyTDSPaymentChallanRepository;
        }
        public Task<MappingTdsQuarterChallan> Handle(GetMappingQuarterlyTdsPaymentChallanByTdsChallanIdRequest request, CancellationToken cancellationToken)
        {
            return _quarterlyTDSPaymentChallanRepository.GetMappingQuarterlyTDSPaymentChallanByTdsChallanId(request.Id);
        }
    }
}
