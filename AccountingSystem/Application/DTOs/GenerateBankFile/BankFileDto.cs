using System;

namespace Application.DTOs.GenerateBankFile
{
    public class BankFileDto
    {
        public string PaymentReferenceNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string VendorName { get; set; }
        public string VendorBankName { get; set; }
        public string VendorBranchName { get; set; }
        public string VendorIfscCode { get; set; }
        public string VendorAccountNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string ApprovedBy { get; set; }
    }
}
