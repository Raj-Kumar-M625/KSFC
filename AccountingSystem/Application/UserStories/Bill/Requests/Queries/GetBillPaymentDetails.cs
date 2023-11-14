using Application.DTOs.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Queries
{
    
    public class GetBillPaymentDetails : IRequest<List<BillItemsDto>>
    {
        public string ID { get; set; }
    }
}
