using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Queries
{
    public class GetBillPaymentRequest: IRequest<IQueryable<BillPayment>>
    {
        public  int PaymentID { get; set; }

    }

   
}
