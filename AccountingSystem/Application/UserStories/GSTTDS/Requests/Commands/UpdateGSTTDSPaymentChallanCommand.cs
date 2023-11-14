using Application.DTOs.GSTTDS;
using Domain.GSTTDS;
using MediatR;
using System.Collections.Generic;

namespace Application.UserStories.GSTTDS.Requests.Commands
{
    public class UpdateGsttdsPaymentChallanCommand : IRequest<GsttdsPaymentChallanDto>
    {
        public GsttdsPaymentChallanDto gstTdsPaymentChallanList { get; set; }

        public string user { get; set; }
    }
}
