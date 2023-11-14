using Application.DTOs.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Commands
{
    public class CreateBillPaymentDetails : IRequest<long>
    {
        public List<BillItemsDto> BillDto { get; set; }
        public BillsDto billPayment { get; set; }
        public string user { get; set; }
    }

 
}
