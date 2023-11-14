using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class NetWorthDTO
    {
        public int? Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public int? NameId { get; set; }
        [Display(Name = "Total Assets(Immovable + Movable)")]
        public string TotalAssets { get; set; }
        [Display(Name = "Total Liabilities")]
        public string TotalLiabilities { get; set; }
        [Display(Name = "Net Worth(Lakhs)")]
        public string Networth { get; set; }
    }
}
