using Domain;
using Domain.Adjustment;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Adjustment.Requests.Queries
{
    public class GetAdjustmentListRequest : IRequest<IQueryable<Adjustments>>
    {
       
            public int VendorId { get; set; }
}
}
