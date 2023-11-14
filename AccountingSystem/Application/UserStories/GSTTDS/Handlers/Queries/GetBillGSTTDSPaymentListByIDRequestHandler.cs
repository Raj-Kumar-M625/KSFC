using Application.Interface.Persistence.GSTTDS;
using Application.UserStories.GSTTDS.Requests.Queries;
using Domain.GSTTDS;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GSTTDS.Handlers.Queries
{
    public class GetBillGsttdsPaymentListByIDRequestHandler : IRequestHandler<GetBillGsttdsPaymentListByIDRequest, IQueryable<BillGsttdsPayment>>
    {
        private readonly IBillGsttdsPaymentRepository billGSTTDSPaymentRepository;
        public GetBillGsttdsPaymentListByIDRequestHandler(IBillGsttdsPaymentRepository billGSTTDSPaymentRepository)
        {
            this.billGSTTDSPaymentRepository = billGSTTDSPaymentRepository;
        }

        public async Task<IQueryable<BillGsttdsPayment>> Handle(GetBillGsttdsPaymentListByIDRequest request, CancellationToken cancellationToken)
        {
            return this.billGSTTDSPaymentRepository.GetBillGSTTDSPaymentListByID(request.Ids);
        }
    }
}
