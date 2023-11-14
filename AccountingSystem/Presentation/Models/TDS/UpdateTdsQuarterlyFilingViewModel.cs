using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.TDS
{
    public class UpdateTdsQuarterlyFilingViewModel
    {
        public List<int> Ids { get; set; }
        public DateTime? DateOfFiling { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public int TracesReferenceNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal TotalPaidAmount { get; set; }
        public int NoOfTrans { get; set; }

    }
}
