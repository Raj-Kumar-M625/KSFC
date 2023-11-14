using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    /// <summary>
    ///  Author: Gagana K; Module:Loan Alloaction; Date:28/09/2022
    /// </summary>
    public class TblAllcCdTabDTO
    {
        public int AllcId { get; set; }
        public int? AllcCd { get; set; }
        public string? AllcDets { get; set; }
        public int? AllcFlg { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UniqueId { get; set; }
    }
}
