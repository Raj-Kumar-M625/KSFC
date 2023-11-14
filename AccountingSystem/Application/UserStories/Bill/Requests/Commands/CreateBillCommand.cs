using Application.DTOs.Bill;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Commands
{
    
    public class CreateBillCommand : IRequest<BillsDto>
    {
        public List<BillsDto> BillDto { get; set; }
        public string user { get; set; }

    }
}
