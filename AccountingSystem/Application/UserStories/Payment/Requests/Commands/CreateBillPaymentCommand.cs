using Application.DTOs.Payment;
using MediatR;
using System.Collections.Generic;

namespace Application.UserStories.Payment.Requests.Commands
{

    public class CreateBillPaymentCommand:IRequest<long>
    {

        public PaymentDto paymentDetails { get; set; }

        public int ID { get; set; }

        public List<BillpaymentDto> BillItemId { get; set; }


    }
}
