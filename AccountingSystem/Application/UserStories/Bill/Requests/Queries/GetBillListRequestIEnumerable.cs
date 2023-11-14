using Application.DTOs.Bill;
using MediatR;
using System.Collections.Generic;

namespace Application.UserStories.Bill.Requests.Queries
{
    public class GetBillListRequestIEnumerable:IRequest<List<BillListDto>>
    {
    }
}
