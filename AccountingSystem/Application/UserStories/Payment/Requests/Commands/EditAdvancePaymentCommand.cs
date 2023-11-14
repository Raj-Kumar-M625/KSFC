using Application.DTOs.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Commands
{
    public class EditAdvancePaymentCommand:IRequest<string>
    {
        public PaymentDto payment { get; set; }
        public string currentUser { get; set; } 
    }
}
