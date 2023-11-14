using Application.DTOs.Adjustment;
using Application.DTOs.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Adjustment.Requests.Commands
{
    public class AddAdjustmentCommadRequest : IRequest<AdjustmentDto>
    {
        public AdjustmentDto adjustmentDto { get; set; }
        public string user { get; set; }
    }
}
