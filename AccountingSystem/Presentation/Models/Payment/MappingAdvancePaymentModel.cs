using System;

namespace Presentation.Models.Payment
{
    public class MappingAdvancePaymentModel
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public int PaymentsID { get; set; }
        public int AdvancePaymentId { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
    }
}
