using Application.DTOs.Payment;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Requests.Commands
{
    public class UpdateTdsPaymentChallanCommand : IRequest<bool>
    {
        public IEnumerable<TdsPaymentChallan> tdsPaymentChallan { get; set; }

        public string user { get; set; }
    }
}
