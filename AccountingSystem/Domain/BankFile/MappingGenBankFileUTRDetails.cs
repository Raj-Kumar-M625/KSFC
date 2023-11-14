using Domain.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BankFile
{
    public class MappingGenBankFileUtrDetails
    {
        public int Id { get; set; }
        [ForeignKey(nameof(BankFileUTRDetails))]
        public int BankFileUTRId { get; set; }
        public int GenerateBankFileId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public virtual BankFileUtrDetails BankFileUTRDetails { get; set; }

    }
}
