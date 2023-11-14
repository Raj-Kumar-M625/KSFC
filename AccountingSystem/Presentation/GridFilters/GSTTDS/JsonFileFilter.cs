using System;

namespace Presentation.GridFilters.GSTTDS
{
	public class JsonFileFilter
	{
        public string[] forder { get; set; }

        public int noOfVendors { get; set; }

        public int noOfTrans { get; set; }

        public string createDateFrom { get; set; }

        public DateTime createDateTo { get; set; }

        public decimal gstTdsMinAmount { get; set; }

        public decimal gstTdsMaxAmount { get; set; }

        public string acknowledgementRefNo { get; set; }

        public string gstTdsStatus { get; set; }
        public string utrNo { get; set; }
    }
}
