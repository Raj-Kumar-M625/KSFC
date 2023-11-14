using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class AllDDLListDTO
    {
        public List<SelectListItem> AllSecurityCategory { get; set; }
       
        public List<SelectListItem> AllSecurityTypes { get; set; }
        public List<SelectListItem> AllSubRegistrarOffice { get; set; }
        public List<SelectListItem> AllAssetTypes { get; set; }
              
    }
}
