using Domain.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vendor
{
    public class Vendors
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string GSTRegistration { get; set; }
        public bool? GST_TDS_Applicable { get; set; }
        public string GSTIN_Number { get; set; }
        public string PAN_Number { get; set; }
        public string TAN_Number { get; set; }
        public string OwnerOrDirectorName { get; set; }
        public string Notes { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual VendorDefaults VendorDefaults { get; set; }
        public virtual VendorBankAccount VendorBankAccounts { get; set; }
        public virtual VendorPerson VendorPerson { get; set; }
        //public virtual Documents Documents { get; set; }
        public virtual VendorBalance VendorBalance { get; set; }
       

    }
}
