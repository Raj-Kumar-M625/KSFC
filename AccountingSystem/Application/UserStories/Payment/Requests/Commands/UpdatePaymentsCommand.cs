using Application.DTOs.Payment;
using Domain.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Commands
{
    public class UpdatePaymentsCommand : IRequest<PaymentDto>
    {
        public List<PaymentDto> payments { get; set; }
    }
}
