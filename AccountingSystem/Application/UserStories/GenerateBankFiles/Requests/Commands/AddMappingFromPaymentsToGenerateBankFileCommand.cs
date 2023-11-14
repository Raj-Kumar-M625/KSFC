using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Requests.Commands
{
    public class AddMappingFromPaymentsToGenerateBankFileCommand:IRequest<int>
    {
       public  List<int> Id { get; set; } 
        public int GenerateBankFileId { get; set; } 
        public string CurrentUser { get; set; }
    }
}
