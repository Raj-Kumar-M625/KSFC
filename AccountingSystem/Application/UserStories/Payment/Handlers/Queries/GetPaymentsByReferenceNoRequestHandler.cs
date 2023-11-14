using Application.DTOs.Payment;
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
    public class GetPaymentsByReferenceNoRequestHandler : IRequestHandler<GetPaymentsByReferenceNoRequest, List<Payments>>
    {
        private readonly IPaymentRepository _PaymentRepository;
        private readonly IMapper _mapper;
        /// <summary> 
        /// Purpose = GetPaymentsByReferenceNoRequest Handler Constructor
        /// Author =Karthick 
        /// Date = 25 08 2022 
        /// </summary>
        public GetPaymentsByReferenceNoRequestHandler(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _PaymentRepository = paymentRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = GetPaymentsByReferenceNoRequest Handler to get payments by referenceno
        /// Author =Karthick 
        /// Date = 25 08 2022 
        /// </summary>
        public async Task<List<Payments>> Handle(GetPaymentsByReferenceNoRequest request, CancellationToken cancellationToken)
        {
            var payments = await _PaymentRepository.GetPaymentsByReferenceNumbers(request.ReferenceNumbers);
            return _mapper.Map<List<Payments>>(payments);
        }     
    }
}
