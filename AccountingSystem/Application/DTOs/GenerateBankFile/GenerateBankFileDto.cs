using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.GenerateBankFile
{
    public class GenerateBankFileDto
    {
        [Key]
        public int Id { get; set; }
        public string BankFileReferenceNo { get; set; }
        public int AccountNo { get; set; }
        public int BankMasterId { get; set; }
        public int NoOfVendors { get; set; }
        public int NoOfTransactions { get; set; }
        public bool IsBulkPayment { get; set; }
        public bool IsBulkPaymentModified { get; set; }

        public decimal TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
    }
}
