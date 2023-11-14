using Application.DTOs.GSTTDS;
using MediatR;

namespace Application.UserStories.GSTTDS.Requests.Commands
{
    public class GenerateGsttdsPaymentChallanCommand:IRequest<int>
    {
        public GsttdsPaymentChallanDto gstTdsPaymentChallan { get; set; }

        public string user { get; set; }
    }
}
