﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblChargeType
    {
        public int Id { get; set; }
        public string ChargeType { get; set; }
        public string DisplaySequence { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }    
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual  TblIdmDsbCharge TblIdmDsbCharge { get; set; }
    }
}