using Application.DTOs.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Queries
{
    public class GetAdvancePaymentByVendorRequest:IRequest<List<PaymentDto>>
    {
        public int vendorID { get; set; }   
    }
}
