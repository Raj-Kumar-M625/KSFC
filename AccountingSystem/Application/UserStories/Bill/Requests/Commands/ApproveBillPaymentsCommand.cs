using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Commands
{
    public class ApproveBillPaymentsCommand : IRequest<int>
    {
      
        public string BillReferenceNo { get; set; }
        public string CurrentUser { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }


    }
}
