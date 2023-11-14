using Application.DTOs.Configuration;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Config.Request.Queries
{
    public class GetConfigDetailsRequest : IRequest<ConfigDto>
    {
        public int Id { get; set; }
    }
}
