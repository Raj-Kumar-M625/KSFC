using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Requests.Commands
{
    public class AddGeneratteBankFileStatusCommand:IRequest<int>
    {
        public int GeneratedBankID { get; set; }    
        public string CurrentUser { get; set; }
    }
}
