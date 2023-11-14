using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class StockTagModel
    {
        [Display(Name = "Zone")]
        [MaxLength(50, ErrorMessage = "Invalid Zone")]
        public string ZoneCode { get; set; }

        [Display(Name = "Area")]
        [MaxLength(50, ErrorMessage = "Invalid Area")]
        public string AreaCode { get; set; }

        [Display(Name = "Territory")]
        [MaxLength(50, ErrorMessage = "Invalid Territory")]
        public string TerritoryCode { get; set; }

        [Display(Name = "HQ")]
        [MaxLength(50, ErrorMessage = "Invalid HQ")]
        public string HQCode { get; set; }

        [Display(Name = "Employee Code")]
        [MaxLength(10, ErrorMessage = "Employee Code can be maximum 10 characters.")]
        public string StaffCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { get; set; }
    }

    public class StockInputTagModel : StockTagModel, IValidatableObject
    {
        public long Id { get; set; }

        [Display(Name = "GRN #")]
        public string GRNNumber { get; set; }

        [Display(Name = "Received On")]
        public System.DateTime ReceiveDate { get; set; }

        [Display(Name = "Received On")]
        public string ReceiveDateAsText { get; set; }

        [Display(Name = "Vendor Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Vendor Name can be maximum 50 characters." )]
        public string VendorName { get; set; }

        [Display(Name = "Bill #")]
        [Required]
        [MaxLength(20, ErrorMessage = "Vendor Bill # can be maximum 20 characters.")]
        public string VendorBillNo { get; set; }

        [Display(Name = "Bill Date")]
        public System.DateTime VendorBillDate { get; set; }

        [Display(Name = "Bill Date")]
        public string VenderBillDateAsText { get; set; }

        [Display(Name = "Total Item Count")]
        [Range(1, 99999, ErrorMessage = "Invalid Total Item Count")]
        public int TotalItemCount { get; set; }

        [Display(Name = "Total Bill Amount")]
        [Range(1, 9999999.99, ErrorMessage = "Invalid Total Bill Amount")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Notes (100 chars)")]
        [MaxLength(100, ErrorMessage = "Notes can be maximum 100 characters.")]
        public string Notes { get; set; }

        public string Status { get; set; }

        [Display(Name = "Review Notes (100 chars)")]
        [MaxLength(100, ErrorMessage = "Review Notes can be maximum 100 characters.")]
        public string ReviewNotes { get; set; }

        [Display(Name = "Reviewed By")]
        public string ReviewedBy { get; set; }

        [Display(Name = "Review Date")]
        public DateTime ReviewDate { get; set; }

        [Display(Name = "Line Items Quantity")]
        public int ItemCountFromLines { get; set; }

        [Display(Name = "Line Items Amount")]
        public decimal AmountTotalFromLines { get; set; }

        public long CyclicCount { get; set; }
        public bool IsEditAllowed { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ReceiveDate = Helper.ConvertStringToDateTime(ReceiveDateAsText);
            VendorBillDate = Helper.ConvertStringToDateTime(VenderBillDateAsText);
            DateTime today = Helper.GetCurrentIstDateTime().Date;

            if (ReceiveDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify Receive Date.");
                yield break;
            }

            ReceiveDate = ReceiveDate.Date;

            if (ReceiveDate > today)
            {
                yield return new ValidationResult("Receive Date can not be in the future.");
                yield break;
            }

            // validate Bill Date
            if (VendorBillDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify Bill Date.");
                yield break;
            }

            VendorBillDate = VendorBillDate.Date;

            if (VendorBillDate > today)
            {
                yield return new ValidationResult("Bill Date can not be in the future.");
                yield break;
            }

            if (VendorBillDate > ReceiveDate)
            {
                yield return new ValidationResult("Bill Date can't be after Receive Date.");
                yield break;
            }

            // Validate Warehouse selection
            var isZoneSelected = !String.IsNullOrEmpty(ZoneCode);
            var isAreaSelected = !String.IsNullOrEmpty(AreaCode);
            var isTerritorySelected = !String.IsNullOrEmpty(TerritoryCode);
            var isHQSelected = !String.IsNullOrEmpty(HQCode);

            IEnumerable<OfficeHierarchy> officeHierarchy = Helper.GetOfficeHierarchy();
            if (isHQSelected)
            {
                if (officeHierarchy.Any(x=> x.HQCode.Equals(HQCode, StringComparison.OrdinalIgnoreCase) 
                                        && x.IsHQSelectable == false))
                {
                    yield return new ValidationResult("HQ selection is not valid/allowed.");
                    yield break;
                }
            }
            else if (isTerritorySelected)
            {
                if (officeHierarchy.Any(x => x.TerritoryCode.Equals(TerritoryCode, StringComparison.OrdinalIgnoreCase) 
                                        && x.IsTerritorySelectable == false))
                {
                    yield return new ValidationResult("Territory selection is not valid/allowed.");
                    yield break;
                }
            }
            else if (isAreaSelected)
            {
                if (officeHierarchy.Any(x => x.AreaCode.Equals(AreaCode, StringComparison.OrdinalIgnoreCase)
                                        && x.IsAreaSelectable == false))
                {
                    yield return new ValidationResult("Area selection is not valid/allowed.");
                    yield break;
                }
            }
            else if (isZoneSelected)
            {
                if (officeHierarchy.Any(x => x.ZoneCode.Equals(ZoneCode, StringComparison.OrdinalIgnoreCase)
                                        && x.IsZoneSelectable == false))
                {
                    yield return new ValidationResult("Zone selection is not valid/allowed.");
                    yield break;
                }
            }
            else
            {
                yield return new ValidationResult("Please select warehouse.");
                yield break;
            }
        }
    }
}