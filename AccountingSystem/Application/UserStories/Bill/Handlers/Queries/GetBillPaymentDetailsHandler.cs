using Application.DTOs.Bill;
using Application.Interface.Persistence.Bill;
using Application.UserStories.Bill.Requests.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Queries
{
    public class GetBillPaymentDetailsHandler : IRequestHandler<GetBillPaymentDetails, List<BillItemsDto>>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        public GetBillPaymentDetailsHandler(IBillRepository billRepository, IMapper mapper)
        {
            _billRepository = billRepository;
            _mapper = mapper;
        }
        public async Task<List<BillItemsDto>> Handle(GetBillPaymentDetails request, CancellationToken cancellationToken)
        {
            int result;
            if (int.TryParse(request.ID,out result))
            {
                var bill = await _billRepository.GetBillpaymentDetailsByID( Convert.ToInt32(request.ID));
                return _mapper.Map<List<BillItemsDto>>(bill);
            }
            else
            {
                var bills = await _billRepository.GetBillDetailsByBIllReferenceNo(request.ID);
                var bill = await _billRepository.GetBillpaymentDetailsByID(bills);
                return _mapper.Map<List<BillItemsDto>>(bill);
                
            }

            

        }

       
    }
}
