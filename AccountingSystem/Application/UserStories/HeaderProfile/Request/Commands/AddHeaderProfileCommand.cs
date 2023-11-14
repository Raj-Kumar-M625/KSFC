using Application.DTOs.Profile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.HeaderProfile.Request.Commands
{
    public class AddHeaderProfileCommand: IRequest<int>
    {
        public HeaderProfileDetailsDto HeaderProfileDetails { get; set; }
        public int Id { get; set; }
        public string entityPath { get; set; }
        public string entityType { get; set; }
        public string user { get; set; }
    }
}
