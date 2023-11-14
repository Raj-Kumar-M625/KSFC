using Application.DTOs.Bill;
using Application.DTOs.TDS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Requests.Commands
{
    public class CreateTdsStatusCommand : IRequest<long>
    {
        public BillsDto billDetails { get; set; }
        public string user { get; set; }
        public int? statusId { get; set; }
    }
}
