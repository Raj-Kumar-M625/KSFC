using Domain.GSTTDS;
using Domain.Master;
using Domain.Payment;
using Domain.TDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.GSTTDS
{
    public class GsstPaymentChallanDto
    {

        public int Id { get; set; }
        public virtual GsttdsPaymentChallan GSTTDSPaymentChallan { get; set; }
        public virtual GsttdsStatus GSTTDSStatus { get; set; }
        public string CodeName { get; set; }
        public string CodeValue { get; set; }
       
        public virtual CommonMaster StatusMaster { get; set; }
    }
}
