using Application.DTOs.Adjustment;
using Application.DTOs.Bill;
using Application.Interface.Persistence.Bill;
using Application.UserStories.Adjustment.Requests.Queries;
using Application.UserStories.Bill.Requests.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Adjustment.Handlers.Queries
{
    
   
  
    public class GetBillPaymentDetailsHandler : IRequestHandler<GetAdjustmentDetailsById, AdjustmentDto>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        public GetBillPaymentDetailsHandler(IBillRepository billRepository, IMapper mapper)
        {
            _billRepository = billRepository;
            _mapper = mapper;
        }
        public async Task<AdjustmentDto> Handle(GetAdjustmentDetailsById request, CancellationToken cancellationToken)
        {

                var adjustments = await _billRepository.GetAdjustmentDetails(request.ID);               
                return _mapper.Map<AdjustmentDto>(adjustments);

        }


    }
}
