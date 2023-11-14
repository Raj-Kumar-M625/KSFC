using Domain.BankFile;
using Domain.GenerateBankfile;
using Domain.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.GenarteBankfile
{
    public class GenerateBankFile
    {
      
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BankFileReferenceNo { get; set; }

        [ForeignKey(nameof(Bank))]
        public int BankMasterId { get; set; }
        public int AccountNo { get; set; }
        public int NoOfVendors { get; set; }
        public int NoOfTransactions { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        [NotMapped]
        public string SourceBankName { get; set; }
        [NotMapped]
        public string AccountNumber { get; set; }

        [NotMapped]
        public string CodeName { get; set; }
        [NotMapped]
        public string CodeValue { get; set; }
        [NotMapped]
        public string DifferentBankUTRNumber { get; set; }
        [NotMapped]
        public string SameBankUTRNumber { get; set; }

        public virtual BankMaster Bank { get; set; }


        public virtual GenerateBankFileStatus GenerateBankFileStatus { get; set; }
       


    }
}
