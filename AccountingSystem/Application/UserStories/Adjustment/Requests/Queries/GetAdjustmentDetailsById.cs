using Application.DTOs.Adjustment;
using Application.DTOs.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Adjustment.Requests.Queries
{
  

    public class GetAdjustmentDetailsById : IRequest<AdjustmentDto>
    {
        public int ID { get; set; }
    }
}
