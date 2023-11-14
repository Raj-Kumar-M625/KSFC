using Application.Interface.Persistence.Payment;
using Application.UserStories.TDS.Requests.Commands;
using AutoMapper;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Handlers.Commands
{
    public class CreateQuarterlyTdsPaymentChallanCommandHandler : IRequestHandler<CreateQuarterlyTdsPaymentChallanCommand, bool>
    {
        private readonly IQuarterlyTdsPaymentChallanRepository _quarterlyTDSPaymentChallanRepository;
        private readonly IMapper _mapper;
        public CreateQuarterlyTdsPaymentChallanCommandHandler(IQuarterlyTdsPaymentChallanRepository quarterlyTDSPaymentChallanRepository, IMapper mapper)
        {
            _quarterlyTDSPaymentChallanRepository = quarterlyTDSPaymentChallanRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CreateQuarterlyTdsPaymentChallanCommand request, CancellationToken cancellationToken)
        {
            var quarterlyTDSPaymentChallan = _mapper.Map<QuarterlyTdsPaymentChallan>(request.quarterlyTdsPaymentChallan);
            var quarterlyTdsPaymentChallan = await _quarterlyTDSPaymentChallanRepository.AddQuarterlyTDSPaymentChallanAsync(quarterlyTDSPaymentChallan);
            if (quarterlyTdsPaymentChallan > 0)
            {
                await _quarterlyTDSPaymentChallanRepository.MapQuarterlyTDSPaymentChallanAsync(request.Ids, quarterlyTdsPaymentChallan);
            }
            return true;
        }
    }
}
