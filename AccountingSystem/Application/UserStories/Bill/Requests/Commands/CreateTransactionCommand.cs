using Domain.Bill;
using Domain.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Commands
{
    public class CreateTransactionCommand : IRequest<int>
    {
        public Bills bills { get; set; }
        public Vendors vendor { get; set; }
        public string CurrentUser { get; set; }
    }
}
