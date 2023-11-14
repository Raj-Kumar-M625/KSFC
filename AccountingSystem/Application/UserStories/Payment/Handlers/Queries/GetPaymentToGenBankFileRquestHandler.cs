using Application.Interface.Persistence.Payment;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Queries
{
    public class GetPaymentToGenBankFileRequestHandler : IRequestHandler<GetPaymentsToGenBankfileRequest, IQueryable<Payments>>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMapper _mapper;

        public GetPaymentToGenBankFileRequestHandler(IPaymentRepository vendorPaymentRepository, IMapper mapper)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _mapper = mapper;
        }
        public async Task<IQueryable<Payments>> Handle(GetPaymentsToGenBankfileRequest request, CancellationToken cancellationToken)
        {
            var list = _vendorPaymentRepository.GetPayments();
            return list;
        }
    }
}
