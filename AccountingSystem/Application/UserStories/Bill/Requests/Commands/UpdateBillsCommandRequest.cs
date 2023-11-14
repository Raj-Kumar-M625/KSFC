using Application.DTOs.Bill;
using MediatR;
using System.Collections.Generic;

namespace Application.UserStories.Bill.Requests.Commands
{
    public class UpdateBillsCommandRequest:IRequest<BillsDto>
    {
        public List<BillsDto> bills { get; set; }

    }
}
