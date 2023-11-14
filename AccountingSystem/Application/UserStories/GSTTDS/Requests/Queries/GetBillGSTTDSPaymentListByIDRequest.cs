using Domain.GSTTDS;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Application.UserStories.GSTTDS.Requests.Queries
{
    public class GetBillGsttdsPaymentListByIDRequest : IRequest<IQueryable<BillGsttdsPayment>>
    {
        public List<int> Ids { get; set; }
    }
}
