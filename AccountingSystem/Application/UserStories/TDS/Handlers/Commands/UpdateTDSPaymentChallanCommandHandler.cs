using Application.Interface.Persistence.Payment;
using Application.UserStories.TDS.Requests.Commands;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Handlers.Commands
{
    public class UpdateTdsPaymentChallanCommandHandler:IRequestHandler<UpdateTdsPaymentChallanCommand,bool>
    {
        private readonly IMapper _mapper;
        private readonly ITdsPaymentChallanRepository _tdsPaymentChallanRepository;

        public UpdateTdsPaymentChallanCommandHandler(ITdsPaymentChallanRepository tdsPaymentChallanRepository,IMapper mapper)
        {
            _tdsPaymentChallanRepository = tdsPaymentChallanRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateTdsPaymentChallanCommand request,CancellationToken cancellationToken)
        {
            return await _tdsPaymentChallanRepository.UpdateTDSPaymentChallanAsync(request.tdsPaymentChallan);
        }
    }
}
