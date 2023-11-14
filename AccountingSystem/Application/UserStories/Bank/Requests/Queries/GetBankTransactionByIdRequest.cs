using Application.DTOs.Bank;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Bank.Requests.Queries
{
    public class GetBankTransactionByIdRequest:IRequest<BankTransactionDto>
    {
        public int bankTransactionId { get; set; }
    }
}
