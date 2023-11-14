using Domain.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Commands
{
    public class UpdateBillPaymentApproveCommand : IRequest<int>
    {
        public Bills bills { get; set; }
    }
}
