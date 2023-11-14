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
    public class AddPaidTransactionCommand : IRequest<int>
    {
        public int Id { get; set; }
        public List<int>? GenerateBankFileID { get; set; }
        public string? UTR { get; set; }
        public string CurrentUser { get; set; }
        public IEnumerable<TdsPaymentChallan>? tdsPaymentChallan { get; set; }
        public GsttdsPaymentChallanDto? gstTdsPaymentChallanList { get; set; }
        public List<MappingAdvancePayment>? MappingAdvancePayment { get; set; }
    }
}
