using System;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class PromDesignationMasterDTO
    {
        [DisplayName("Designation Code")]
       
        public int PdesigCd { get; set; }

        [DisplayName("Designation Details")]
         
        public string PdesigDets { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}