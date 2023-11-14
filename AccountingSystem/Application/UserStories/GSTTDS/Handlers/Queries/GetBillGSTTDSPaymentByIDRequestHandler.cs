using Application.Interface.Persistence.GSTTDS;
using Application.UserStories.GSTTDS.Requests.Queries;
using Domain.GSTTDS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GSTTDS.Handlers.Queries
{
    public class GetBillGsttdsPaymentByIDRequestHandler : IRequestHandler<GetBillGsttdsPaymentByIDRequest, BillGsttdsPayment>
    {
        private readonly IBillGsttdsPaymentRepository _billGSTTDSPaymentRepository;

        public GetBillGsttdsPaymentByIDRequestHandler(IBillGsttdsPaymentRepository billGSTTDSPaymentRepository)
        {
            _billGSTTDSPaymentRepository = billGSTTDSPaymentRepository;
        }
        public Task<BillGsttdsPayment> Handle(GetBillGsttdsPaymentByIDRequest request, CancellationToken cancellationToken)
        {
            return this._billGSTTDSPaymentRepository.GetBillGSTTDSPaymentByID(request.ID);
        }
    }
}
