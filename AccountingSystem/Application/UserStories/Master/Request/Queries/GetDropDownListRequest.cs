using Application.DTOs.Master;
using Domain.Master;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Master.Request.Queries
{
    public class GetDropDownListRequest:IRequest<DropDownDto>
    {
    }
}
