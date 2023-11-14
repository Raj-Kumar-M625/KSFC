﻿using Domain.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Bill
{
    public class BillStatus
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Bill")]
        public int BillID { get; set; }
        public int VendorId { get; set; }
        [ForeignKey(nameof(StatusMaster))]
        public int StatusCMID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public virtual CommonMaster StatusMaster { get; set; }

        public virtual Bills Bill { get; set; }

    }
}
