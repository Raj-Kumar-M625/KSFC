using Application.DTOs.TDS;
using Application.Interface.Persistence.Payment;
using Application.UserStories.Payment.Requests.Queries;
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
    /// Author : Karthick J
    /// Date: 02/09/2022
    /// Purpose: Get TDS Payment Challan List Request handler
    /// </summary>
    public class GetTdsPaymentChallanListRequestHandler : IRequestHandler<GetTdsPaymentChallanListRequest, IQueryable<TdssPaymentChallanDto>>
    {
        private readonly ITdsPaymentChallanRepository _tdsPaymentChallanRepository;

        public GetTdsPaymentChallanListRequestHandler(ITdsPaymentChallanRepository tdsPaymentChallanRepository)
        {
            _tdsPaymentChallanRepository = tdsPaymentChallanRepository;
        }

        /// <summary>
        /// Author : Karthick J
        /// Date: 02/09/2022
        /// Purpose: Handle TDS Payment Challan List Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IQueryable<TdssPaymentChallanDto>> Handle(GetTdsPaymentChallanListRequest request, CancellationToken cancellationToken)
        {
            var paymentChallanList = _tdsPaymentChallanRepository.GetTDSPaymentChallanList();
            return paymentChallanList;
        }
    }
}
