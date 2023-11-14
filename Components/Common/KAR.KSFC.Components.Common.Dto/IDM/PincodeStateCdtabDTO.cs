﻿using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Data.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class PincodeStateCdtabDTO
    {
        public int PincodeStateCd { get; set; }
        public string? PincodeStateDesc { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblPincodeDistrictCdtab TblPincodeDistrictCdtab { get; set; }

    }
}