using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.BankFile.Requests.Commands
{
    public class AddMappingForBankFileCommand:IRequest<int>
    {
        public List<int> GenerateBankFileID { get; set; }   
        public int BankFileId { get; set; }
        public  string CurrentUser { get; set; }
    }
}
