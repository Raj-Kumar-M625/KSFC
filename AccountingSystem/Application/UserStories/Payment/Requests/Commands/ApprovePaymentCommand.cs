using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Commands
{
    public class ApprovePaymentCommand:IRequest<int>
    {
        public int paymentId { get; set; }
        public string CurrentUser { get; set; }
        public string Remarks { get; set; }

        public string Status { get; set; }
    }
}
