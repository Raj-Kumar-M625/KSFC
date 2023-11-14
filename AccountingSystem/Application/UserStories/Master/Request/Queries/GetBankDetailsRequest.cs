using Application.DTOs.Master;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Master.Request.Queries
{
    public class GetBankDetailsRequest :IRequest<List<BranchMasterDto>>
    {
    }
}
