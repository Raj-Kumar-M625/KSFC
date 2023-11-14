using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Requests.Queries
{
    public class GetTdsPaymentChallanByIdRequest: IRequest<IQueryable<TdsPaymentChallan>>
    {
        public List<int> Ids { get; set; }
    }
}
