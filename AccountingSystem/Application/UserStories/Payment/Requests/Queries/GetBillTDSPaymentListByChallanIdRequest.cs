using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Queries
{
    public class GetBillTdsPaymentListByChallanIdRequest:IRequest<IQueryable<BillTdsPayment>>
    {
        public List<int> ChallanIds { get; set; }
    }
}
