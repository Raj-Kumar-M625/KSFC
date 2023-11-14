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
    public class GetGstJsonFileListHandler : IRequestHandler<GetGstJsonFileList, IQueryable<BillGsttdsPayment>>
    {
        private readonly IBillGsttdsPaymentRepository _billGSTTDSPaymentRepository;

        public GetGstJsonFileListHandler(IBillGsttdsPaymentRepository billGSTTDSPaymentRepository)
        {
            _billGSTTDSPaymentRepository = billGSTTDSPaymentRepository;
        }
        public async Task<IQueryable<BillGsttdsPayment>> Handle(GetGstJsonFileList request, CancellationToken cancellationToken)
        {
            var billPayments = _billGSTTDSPaymentRepository.GeGSTTDSJSONist();
            return billPayments;
        }
    }
  


}
