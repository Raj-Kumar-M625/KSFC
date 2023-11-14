using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Commands
{
    public class UpdatePaymentStatusCommand:IRequest<int>
    {
        public List<int> PaymentID { get; set; }
    }
}
