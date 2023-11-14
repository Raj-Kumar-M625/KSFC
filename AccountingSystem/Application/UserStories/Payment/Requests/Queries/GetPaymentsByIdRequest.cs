using Application.DTOs.Payment;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Queries
{
    public class GetPaymentsByIdRequest:IRequest<PaymentDto>
    {
        public int Id { get; set; } 
    }
}
