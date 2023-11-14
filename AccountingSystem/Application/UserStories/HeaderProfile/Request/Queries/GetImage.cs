using Domain.Profile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.HeaderProfile.Request.Queries
{
    public class GetImage:IRequest<IQueryable<HeaderProfileDetails>>
    {
    }
}
