using Domain.Master;
using Domain.Payment;
using Domain.TDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TDS
{
    public class TdssPaymentChallanDto
    {
        public int Id { get; set; }
        public  virtual TdsPaymentChallan TDSPaymentChallan { get; set; } 
        public virtual TdsStatus TDSStatus { get; set; }
        public string CodeName { get; set; }
        public string CodeValue { get; set; }
        public virtual BankMaster Bank { get; set; }
        public virtual CommonMaster StatusMaster { get; set; }
    }
}
