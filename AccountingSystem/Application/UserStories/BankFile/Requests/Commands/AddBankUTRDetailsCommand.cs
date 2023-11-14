using Application.DTOs.BankFile;
using Domain.BankFile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.BankFile.Requests.Commands
{
    public class AddBankUtrDetailsCommand:IRequest<int>
    {
        public BankFileUtrDetailsDto bankFileDetails { get; set; }
    }
}
