using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Queries
{
    public class GetBillPaymentsByVendorRequest:IRequest<IQueryable<Transaction>>
    {
        public int VendorId { get; set; }   
    }
}
