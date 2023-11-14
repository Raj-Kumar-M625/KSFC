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
    public class GetMappingQuarterlyTdsPaymentChallanByTdsChallanIdRequestHandler : IRequestHandler<GetMappingQuarterlyTdsPaymentChallanRequest, IQueryable<MappingTdsQuarterChallan>>
    {
        private readonly IQuarterlyTdsPaymentChallanRepository _quarterlyTDSPaymentChallanRepository;
        public GetMappingQuarterlyTdsPaymentChallanByTdsChallanIdRequestHandler(IQuarterlyTdsPaymentChallanRepository quarterlyTDSPaymentChallanRepository)
        {
            _quarterlyTDSPaymentChallanRepository = quarterlyTDSPaymentChallanRepository;
        }
        public Task<IQueryable<MappingTdsQuarterChallan>> Handle(GetMappingQuarterlyTdsPaymentChallanRequest request, CancellationToken cancellationToken)
        {
            return _quarterlyTDSPaymentChallanRepository.GetMappingQuarterlyTDSPaymentChallanByQuarterChallanId(request.Ids);
        }
    }
}
