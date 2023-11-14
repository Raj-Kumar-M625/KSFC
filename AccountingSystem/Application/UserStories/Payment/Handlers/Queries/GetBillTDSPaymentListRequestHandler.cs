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
    public class GetBillTdsPaymentListRequestHandler : IRequestHandler<GetBillTdsPaymentListRequest, IQueryable<BillTdsPayment>>
    {
        private readonly IBillTdsPaymentRepository _billTDSPaymentRepository;
        public GetBillTdsPaymentListRequestHandler(IBillTdsPaymentRepository billTDSPaymentRepository)
        {
            _billTDSPaymentRepository = billTDSPaymentRepository;
        }
        public async Task<IQueryable<BillTdsPayment>> Handle(GetBillTdsPaymentListRequest request, CancellationToken cancellationToken)
        {
            var billPayments = _billTDSPaymentRepository.GetBillTDSPaymentList();
            return billPayments;
        }
    }
}
