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
    public class AddPaymentTransactionSummaryCommand :IRequest<int>
    {
        public BankFileUtrDetailsDto bankFileUTRDetails { get; set; }
        public string currentUser { get; set; }
        public GsttdsPaymentChallanDto? gstTdsPaymentChallanList { get; set; }
    }
}
