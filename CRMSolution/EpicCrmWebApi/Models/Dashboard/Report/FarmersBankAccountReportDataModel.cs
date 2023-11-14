using System;
using System.ComponentModel.DataAnnotations;


namespace EpicCrmWebApi
{

    public class FarmersBankAccountReportDataModel
    {
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Client Name")]
        public string EntityName { get; set; }

        [Display(Name = "Is Self")]
        public string IsSelfAccount { get; set; }

        [Display(Name = "Village Name")]
        public string HQName { get; set; }

        [Display(Name = "Cluster Name")]
        public string TerritoryName { get; set; }

        [Display(Name = "A/c Holder Name")]
        public string AccountHolderName { get; set; }

        [Display(Name = "A/c Holder PAN")]
        public string AccountHolderPAN { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Bank A/c Number")]
        public string BankAccount { get; set; }

        [Display(Name = "IFSC Code")]
        public string BankIFSC { get; set; }

        [Display(Name = "Branch Name")]
        public string BankBranch { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Entity Number")]
        public string EntityNumber { get; set; }
    }
}