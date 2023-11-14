using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.LoanAccounting
{
    public class LA_DDLListDTO
    {
        public List<SelectListItem> AllTransactionTypes { get; set; }
    }
}
