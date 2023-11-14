using Application.DTOs.GenerateBankFile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Requests.Commands
{
    public class CreateGenerateBankFileCommand : IRequest<int>
    { 
        public GenerateBankFileDto GenerateBankFileDto { get; set; }    
    }
}
