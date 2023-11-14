using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Requests.Queries
{
    public class GetMappingQuarterlyTdsPaymentChallanByTdsChallanIdRequest : IRequest<MappingTdsQuarterChallan>
    {
        public int Id { get; set; }
    }
}
