using Application.DTOs.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Requests.Commands
{
    public class CreateQuarterlyTdsPaymentChallanCommand:IRequest<bool>
    {
        public QuarterlyTdsPaymentChallanDto quarterlyTdsPaymentChallan { get; set; }

        public List<int> Ids { get; set; }

        public string user { get; set; }
    }
}
