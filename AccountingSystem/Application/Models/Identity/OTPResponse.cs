using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Identity
{
    public class OtpResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string OTP { get; set; }
    }
}
