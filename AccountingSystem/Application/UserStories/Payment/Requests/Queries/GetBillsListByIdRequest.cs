using Application.DTOs.Bill;
using MediatR;
using System.Collections.Generic;

namespace Application.UserStories.Payment.Requests.Queries
{
    public class GetBillsListByIdRequest:IRequest<List<BillsDto>>
    {
        public int ID { get; set; }
        public string Type { get; set; }
    }
}
