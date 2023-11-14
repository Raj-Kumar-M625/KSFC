using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;

using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace KAR.KSFC.UI.Models
{
    public class RegisterViewModel
    {
        public List<SelectListItem> ListConstitutionTypes { get; set; }
        public string MobileNumber { get; set; }
        public string PanNumber { get; set; }
        public int ConstitutionTypeId { get; set; }

    }
}
