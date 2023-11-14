using Application.Interface.Persistence.GSTTDS;
using Application.UserStories.GSTTDS.Requests.Queries;
using Domain.GSTTDS;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GSTTDS.Handlers.Queries
{
    public class GetBillGsttdsPaymentListRequestHandler:IRequestHandler<GetBillGsttdsPaymentListRequest,IQueryable<BillGsttdsPayment>>
    {
        private readonly IBillGsttdsPaymentRepository _billGSTTDSPaymentRepository;
        public GetBillGsttdsPaymentListRequestHandler(IBillGsttdsPaymentRepository billGSTTDSPaymentRepository)
        {
            _billGSTTDSPaymentRepository = billGSTTDSPaymentRepository;
        }
        public async Task<IQueryable<BillGsttdsPayment>> Handle(GetBillGsttdsPaymentListRequest request,CancellationToken cancellationToken)
        {
            var billPayments = _billGSTTDSPaymentRepository.GetBillGSTTDSPaidList();
            return billPayments;
        }
    }
}
