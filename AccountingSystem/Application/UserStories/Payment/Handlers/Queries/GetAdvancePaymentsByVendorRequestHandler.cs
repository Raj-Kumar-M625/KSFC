using Application.DTOs.Payment;
using Application.Interface.Persistence.Payment;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Queries
{
    public class GetAdvancePaymentsByVendorRequestHandler : IRequestHandler<GetAdvancePaymentByVendorRequest, List<PaymentDto>>
    {

        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetAdvancePaymentsByVendorRequestHandler(IPaymentRepository vendorPaymentRepository, IMapper mapper, IMediator mediator)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _mapper = mapper;
        }

        public async Task<List<PaymentDto>> Handle(GetAdvancePaymentByVendorRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var advancePayments = await _vendorPaymentRepository.GetAdvancePaymentsbyVendorId(request.vendorID);
                return _mapper.Map<List<PaymentDto>>(advancePayments);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
