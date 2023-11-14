using Application.Interface.Persistence.Payment;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using Domain.Transactions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Queries
{
    public class GetBillPaymentByVendorRequestHandler :IRequestHandler<GetBillPaymentsByVendorRequest , IQueryable<Transaction>>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetBillPaymentByVendorRequestHandler(IPaymentRepository vendorPaymentRepository, IMapper mapper, IMediator mediator)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _mapper = mapper;
        }
       
        public async Task<IQueryable<Transaction>> Handle(GetBillPaymentsByVendorRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var transactions = await _vendorPaymentRepository.GetBillPaymentsByVendorId(request.VendorId);
                return transactions;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
