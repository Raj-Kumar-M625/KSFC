using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Adjustment.Requests.Commands
{
    public class AddAdjustmentStatusCommandRequest: IRequest<int>
    {
        public int AdjustmentId { get; set; }
    }
}
