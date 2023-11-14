using Domain.GSTTDS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.GSTTDS.Requests.Queries
{
    public class GetBillGsttdsPaymentListRequest: IRequest<IQueryable<BillGsttdsPayment>>
    {
    }
}
