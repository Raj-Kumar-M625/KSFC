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
    public class GetBillTdsPaymentListByChallanIdRequestHandler : IRequestHandler<GetBillTdsPaymentListByChallanIdRequest, IQueryable<BillTdsPayment>>
    {
        private readonly IBillTdsPaymentRepository _billTDSPaymentRepository;
        public GetBillTdsPaymentListByChallanIdRequestHandler(IBillTdsPaymentRepository billTDSPaymentRepository)
        {
            _billTDSPaymentRepository = billTDSPaymentRepository;
        }

        public async Task<IQueryable<BillTdsPayment>> Handle(GetBillTdsPaymentListByChallanIdRequest request, CancellationToken cancellationToken)
        {
            var billPayments = _billTDSPaymentRepository.GetBillTDSPaymentListByChallanId(request.ChallanIds);
            return billPayments;
        }
    }
}
