using Application.DTOs.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Requests.Queries
{
    public class GetPaymentListByIdRequest : IRequest<decimal>
    {
        public List<int> ID { get; set; }
       
    }
}
