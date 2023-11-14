using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class AssociateDetailsDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Name of Concern")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Name is required.")]
        public string AssociateName { get; set; }
        [Display(Name = "Name of Banker /Fin. Institution")]
        [MaxLength(50)]
        public string BankerName { get; set; }
        [Display(Name = "Facility")]
        [MaxLength(50)]
        public string FacilityName { get; set; }
        [Display(Name = "Outstanding (in Lakhs)")]
        [MaxLength(50)]
        public string Outstanding { get; set; }
        [Display(Name = "Consent for taking CIBIL Score")]
        public bool ConsentForTakingCIBILScore { get; set; }
        [Display(Name = "Default (in Lakhs)")]
        [MaxLength(20)]
        public string Default { get; set; }
        [Display(Name = "Relief and Concessions Availed")]
        [MaxLength(20)]
        public string ReliefAndConcession { get; set; }
        [Display(Name = "One-Time Settlement")]
        [MaxLength(20)]
        public string OneTimeSettlementPaid { get; set; }
    }
}
