using CRMUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EpicCrmWebApi
{
    public class EntityBankDetailModel : IValidatableObject
    {
        public long Id { get; set; }
        public long EntityId { get; set; }

        [Display(Name = "Is Self")]
        public bool IsSelfAccount { get; set; }

        [Display(Name = "Account Holder Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "AccountHolderName can be maximum 50 characters long.")]
        public string AccountHolderName { get; set; }

        [Display(Name = "Account Holder PAN")]
        [MaxLength(50, ErrorMessage = "AccountHolderPAN can be maximum 50 characters long.")]
        public string AccountHolderPAN { get; set; }

        [Display(Name = "Bank Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "BankName can be maximum 50 characters long.")]
        public string BankName { get; set; }

        [Display(Name = "Bank Account No.")]
        [Required]
        [MaxLength(50, ErrorMessage = "BankAccount can be maximum 50 characters long.")]
        public string BankAccount { get; set; }

        [Display(Name = "IFSC Code")]
        [Required]
        [MaxLength(50, ErrorMessage = "IFSC Code can be maximum 50 characters long.")]
        public string BankIFSC { get; set; }

        [Display(Name = "Branch Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Branch Name can be maximum 50 characters long.")]
        public string BankBranch { get; set; }

        [Display(Name = "Images")]
        public int ImageCount { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage="Please select Status")]
        public string Status { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }

        [Display(Name = "Comments")]
        [MaxLength(100, ErrorMessage = "Comments can be maximum 100 characters long.")]
        public string Comments { get; set; }

        [Display(Name = "Client Type")]
        public string EntityType { get; set; }

        [Display(Name = "Client Name")]
        public string EntityName { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string ImageUpload { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if (String.IsNullOrEmpty(Status))
            //{
            //    yield return new ValidationResult("Please select Status");
            //}

            if (String.IsNullOrEmpty(AccountHolderPAN) == false)
            {
                string regexString = Utils.SiteConfigData.PANRegEx;
                if (String.IsNullOrEmpty(regexString))
                {
                    regexString = "^[0-9a-zA-Z]{10}$";
                }

                Regex panRegEx = null;
                try
                {
                    panRegEx = new Regex(regexString);
                }
                catch(Exception ex)
                {
                    panRegEx = null;
                    BusinessLayer.Business.LogError($"{nameof(EntityBankDetailModel)}", ex);
                }

                if (panRegEx != null && panRegEx.Match(AccountHolderPAN).Success == false)
                {
                    yield return new ValidationResult("Invalid PAN Number.");
                }
            }
        }
    }
}