using Application.DTOs.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Commands
{   

    public class UpdateBillCommand : IRequest<BillsDto>
    {
        public List<BillsDto> BillDto { get; set; }
        public string user { get; set; }

    }
}
