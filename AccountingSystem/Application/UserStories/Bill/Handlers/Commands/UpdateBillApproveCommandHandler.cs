using Application.DTOs.Payment;
using Application.Interface.Persistence.Bill;
using Application.UserStories.Bill.Requests.Commands;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{
    public class UpdateBillApproveCommandHandler : IRequestHandler<UpdateBillPaymentApproveCommand, int>
    {

        List<PaymentDto> payemntsDto = new List<PaymentDto>();
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        //Adding dependency injection for IBillRepository,IMapper and IMediator
        public UpdateBillApproveCommandHandler(IBillRepository vendorRepository, IMapper mapper, IMediator mediator)
        {
            _billRepository = vendorRepository;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<int> Handle(UpdateBillPaymentApproveCommand request, CancellationToken cancellationToken)
        {
            var res =await  _billRepository.UpdateBillsDetails(request.bills);
            return res.ID;
        }
    }
}
