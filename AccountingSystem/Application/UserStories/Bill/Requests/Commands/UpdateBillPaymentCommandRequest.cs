using Domain.Payment;
using MediatR;
using System.Collections.Generic;

namespace Application.UserStories.Bill.Requests.Commands
{
    public class UpdateBillPaymentCommandRequest:IRequest<string>
    {
        public List<BillPayment> bills { get; set; }
    }
}
