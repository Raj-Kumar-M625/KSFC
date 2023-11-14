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
    public class GetPendingPaymentListRequestHandler : IRequestHandler<GetPendingPaymentListRequest, List<Payments>>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Author: Date:26/05/2022
        /// Purpose:Consturctor
        /// </summary>
        /// <returns></returns>
        public GetPendingPaymentListRequestHandler(IPaymentRepository vendorPaymentRepository, IMapper mapper)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _mapper = mapper;
        }
        public async Task<List<Payments>> Handle(GetPendingPaymentListRequest request, CancellationToken cancellationToken)
        {
            var res = await _vendorPaymentRepository.GetPaymentsByStatus();
            return res;
        }
    }
}
