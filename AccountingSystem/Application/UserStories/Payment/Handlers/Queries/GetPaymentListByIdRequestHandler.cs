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
    /// <summary> 
    /// Purpose = GetPaymentListByIdRequest Handler 
    /// Author =Manoj 
    /// Date = 05 07 2022 
    /// </summary>
    public class GetPaymentListByIdRequestHandler : IRequestHandler<GetPaymentListByIdRequest, decimal>
    {
        private readonly IPaymentRepository _PaymentRepository;
        private readonly IMapper _mapper;
        /// <summary> 
        /// Purpose = GetPaymentListByIdRequest Handler Constructor
        /// Author =Manoj 
        /// Date = 05 07 2022 
        /// </summary>
        public GetPaymentListByIdRequestHandler(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _PaymentRepository = paymentRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = GetPaymentListByIdRequest Handler to  get bills by ID and add the payable amount
        /// Author =Manoj 
        /// Date = 05 07 2022 
        /// </summary>
        public async Task<decimal> Handle(GetPaymentListByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                decimal Total = 0;
                List<Payments> payment = new List<Payments>();
                foreach (int Id in request.ID)
                {
                    var payments = await _PaymentRepository.GetPaymentsById(Id);
                    payment.Add(payments);
                }
                Total=payment.Sum(x=>x.PaidAmount );
                return Total;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

       
    }
}
