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
    public class GetPaymentsByIdRequestHandler : IRequestHandler<GetPaymentsByIdRequest, PaymentDto>
    {
        private readonly IPaymentRepository _PaymentRepository;
        private readonly IMapper _mapper;
        /// <summary> 
        /// Purpose = GetPaymentListByIdRequest Handler Constructor
        /// Author =Swetha 
        /// Date = 05 07 2022 
        /// </summary>
        public GetPaymentsByIdRequestHandler(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _PaymentRepository = paymentRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = GetPaymentListByIdRequest Handler to  get bills by ID and add the payable amount
        /// Author =Swetha 
        /// Date = 05 07 2022 
        /// </summary>
        public async Task<PaymentDto> Handle(GetPaymentsByIdRequest request, CancellationToken cancellationToken)
        {
            var payments = await _PaymentRepository.GetPaymentsById(request.Id);
            return _mapper.Map<PaymentDto>(payments);
        }     
    }
}
