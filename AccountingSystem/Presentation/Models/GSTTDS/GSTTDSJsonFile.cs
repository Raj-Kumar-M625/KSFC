using System.Collections.Generic;

namespace Presentation.Models.GSTTDS
{
    public class Root
    {
        public string gstin { get; set; }
        public string fp { get; set; }
        public List<Td> tds { get; set; } = new List<Td>();
    }

    public class Td
    {
        public string gstin_ded { get; set; }
        public decimal? amt_ded { get; set; }
        public decimal? iamt { get; set; }
        public decimal? camt { get; set; }
        public decimal? samt { get; set; }
        public string flag { get; set; }
    }




}
