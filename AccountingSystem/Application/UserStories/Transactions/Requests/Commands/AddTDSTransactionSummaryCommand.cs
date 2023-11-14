using Application.DTOs.BankFile;
using Application.DTOs.GSTTDS;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Requests.Commands
{
    public class AddTDSTransactionSummaryCommand :IRequest<int>
    {
        public string currentUser { get; set; }
        public TdsPaymentChallan tdsPaymentChallan { get; set; }
    }
}
