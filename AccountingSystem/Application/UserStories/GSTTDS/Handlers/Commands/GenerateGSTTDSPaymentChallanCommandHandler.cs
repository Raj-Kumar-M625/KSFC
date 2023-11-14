using Application.Interface.Persistence.GSTTDS;
using Application.UserStories.GSTTDS.Requests.Commands;
using AutoMapper;
using Domain.GSTTDS;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GSTTDS.Handlers.Commands
{
    public class GenerateGsttdsPaymentChallanCommandHandler : IRequestHandler<GenerateGsttdsPaymentChallanCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IGstdsPaymentChallanRepository _gstTdsPaymentChallanRepository;

        public GenerateGsttdsPaymentChallanCommandHandler(IGstdsPaymentChallanRepository gstTdsPaymentChallanRepository, IMapper mapper)
        {
            _gstTdsPaymentChallanRepository = gstTdsPaymentChallanRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(GenerateGsttdsPaymentChallanCommand request, CancellationToken cancellationToken)
        {
            var gstTdsPaymentChallan = _mapper.Map<GsttdsPaymentChallan>(request.gstTdsPaymentChallan);

            gstTdsPaymentChallan.CreatedOn = DateTime.UtcNow;
            gstTdsPaymentChallan.ModifiedOn = DateTime.UtcNow;
            gstTdsPaymentChallan.CreatedBy = request.user;
            gstTdsPaymentChallan.PaidAmount = 0;           
            gstTdsPaymentChallan.ModifiedBy = request.user;
            gstTdsPaymentChallan = await _gstTdsPaymentChallanRepository.AddGSTTDSPaymentChallanAsync(gstTdsPaymentChallan);
            return gstTdsPaymentChallan.Id;
        }
    }
}
