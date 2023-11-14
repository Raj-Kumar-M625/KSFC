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
    public class DWSModel : IValidatableObject
    {
        public long Id { get; set; }
        public long STRId { get; set; }
        public long STRTagId { get; set; }

        [Display(Name = "DWS Number")]
        //[Range(1, 9999999, ErrorMessage ="Invalid DWS #")]
        public string DWSNumber { get; set; }

        [Display(Name = "DWS Date")]
        public System.DateTime DWSDate { get; set; }

        [Display(Name = "Applied Bag Count")]
        public long BagCount { get; set; }

        [Display(Name = "Applied Filled Bags Wt.")]
        public decimal FilledBagsWeightKg { get; set; }

        [Display(Name = "Applied Empty Bags Wt.")]
        public decimal EmptyBagsWeightKg { get; set; }

        public long EntityId { get; set; }

        [Display(Name = "Farmer Name")]
        public string EntityName { get; set; }

        public long AgreementId { get; set; }

        [Display(Name = "Agreement #")]
        [Required]
        [MaxLength(50, ErrorMessage = "Agreement # can be maximum 50 characters long.")]
        public string Agreement { get; set; }

        public long EntityWorkFlowDetailId { get; set; }

        [Display(Name = "Type Name")]
        public string TypeName { get; set; }

        public string TagName { get; set; }

        public long ActivityId { get; set; }

        [Display(Name = "STR Number")]
        [MaxLength(50, ErrorMessage = "STRNumber can be maximum 50 characters long.")]
        [Required]
        public string STRNumber { get; set; }

        [Display(Name = "DWS Date")]
        public string DWSDateAsText { get; set; }

        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }

        [Display(Name = "Bag Count")]
        public long OrigBagCount { get; set; }

        [Display(Name = "Filled Bags Wt. (Kg.)")]
        public decimal OrigFilledBagsKg { get; set; }

        [Display(Name = "Empty Bags Wt. (Kg.)")]
        public decimal OrigEmptyBagsKg { get; set; }

        [Display(Name = "Net Goods WT. (Kg.)")]
        public decimal NetGoodsWeight { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(500, ErrorMessage = "Notes can be maximum 500 characters.")]
        public string Comments { get; set; }

        public string Status { get; set; }

        public bool IsPendingStatus { get; set; }
        public bool IsApproveAllowed { get; set; }

        [Display(Name = "Silo Deduct %")]
        public decimal SiloDeductPercent { get; set; }
        [Display(Name = "Applied Net Goods WT. (Kg.)")]
        public decimal GoodsWeight { get; set; }
        [Display(Name = "Silo Deduct Wt. (Kg.)")]
        public decimal SiloDeductWt { get; set; }
        [Display(Name = "Silo Deduct Wt. Override (Kg.)")]
        public decimal SiloDeductWtOverride { get; set; }
        [Display(Name = "Net Wt. Payable (Kg.)")]
        public decimal NetPayableWt { get; set; }
        [Display(Name = "Rate/Kg. (Rs.)")]
        public decimal RatePerKg { get; set; }
        [Display(Name = "Gross Amt (Rs.)")]
        public decimal GoodsPrice { get; set; }
        [Display(Name = "Deduct Amt (Rs.)")]
        public decimal DeductAmount { get; set; }
        [Display(Name = "Net Amt (Rs.)")]
        public decimal NetPayable { get; set; }


        [Display(Name = "Wt. Approved By")]
        public string WtApprovedBy { get; set; }
        [Display(Name = "Wt. Approved Date")]
        public string WtApprovedDateAsText { get; set; }
        [Display(Name = "Amount Approved By")]
        public string AmountApprovedBy { get; set; }
        [Display(Name = "Amount Approved Date")]
        public string AmountApprovedDateAsText { get; set; }
        [Display(Name = "Paid By")]
        public string PaidBy { get; set; }
        [Display(Name = "Paid Date")]
        public string PaidDateAsText { get; set; }
        [Display(Name = "Payment Reference")]
        public string PaymentReference { get; set; }

        public string HQCode { get; set; }
        public long CyclicCount { get; set; }

        [Display(Name = "Bank Account Name")]
        public string BankAccountName { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Bank Account")]
        public string BankAccount { get; set; }
        [Display(Name = "Bank IFSC")]
        public string BankIFSC { get; set; }
        [Display(Name = "Bank Branch")]
        public string BankBranch { get; set; }

        public long StrTagCyclicCount { get; set; }

        public ICollection<EntityBankDetailModel> EntityBankAccounts { get; set; }
        public bool HasBankAccounts => (EntityBankAccounts?.Count ?? 0) > 0;

        public DomainEntities.DWS OrigRec { get; set; }
        public bool HasAgreementChanged { get; set; }
        public DomainEntities.EntityWorkFlow EntityWFRec { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Agreement = Utils.TruncateString(Agreement, 50);
            DWSDate = ConvertStringToDateTime(DWSDateAsText);
            STRNumber = Utils.TruncateString(STRNumber, 50);

            if (DWSDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify DWS Date.");
                yield break;
            }

            DWSDate = DWSDate.Date;

            DateTime today = Helper.GetCurrentIstDateTime().Date;
            if (DWSDate > today)
            {
                yield return new ValidationResult("DWS Date can not be in the future.");
                yield break;
            }

            if (BagCount <= 0)
            {
                yield return new ValidationResult($"Invalid Bag Count {BagCount}");
                yield break;
            }

            if (FilledBagsWeightKg < 0 || EmptyBagsWeightKg < 0)
            {
                yield return new ValidationResult("Weights can't be less than zero.");
                yield break;
            }

            if ((FilledBagsWeightKg - EmptyBagsWeightKg) <= 0)
            {
                yield return new ValidationResult("Invalid weights.");
                yield break;
            }

            if (Id == 0)
            {
                var strTagRec = Business.GetSingleSTRTag(STRTagId);
                if (!strTagRec.IsEditAllowed)
                {
                    yield return new ValidationResult($"STR {strTagRec.STRNumber} is marked as Silo Checked. Edit operation is not allowed.");
                    yield break;
                }

                ICollection<DomainEntities.EntityWorkFlow> entityWorkFlows =
                            Business.GetEntityWorkFlow(Agreement);

                if ((entityWorkFlows?.Count ?? 0) == 0)
                {
                    yield return new ValidationResult($"Invalid Agreement # {Agreement}");
                    yield break;
                }

                // Check that DWS # is unique
                if (Business.IsExistingDWS(DWSNumber))
                {
                    yield return new ValidationResult($"DWS {DWSNumber} already exist.");
                    yield break;
                }
            }
            else
            {
                OrigRec = Business.GetSingleDWS(Id);
                DomainEntities.STRTag strTagRec = Business.GetSTRTag(OrigRec.STRNumber);
                if (!strTagRec.IsEditAllowed)
                {
                    yield return new ValidationResult($"STR {OrigRec.STRNumber} is marked as Silo Checked. Edit operation is not allowed.");
                    yield break;
                }

                if (!OrigRec.STRNumber.Equals(STRNumber, StringComparison.OrdinalIgnoreCase))
                {
                    // Get STRTag record for new STR Number
                    DomainEntities.STRFilter searchCriteria = new DomainEntities.STRFilter()
                    {
                        STRNumber = STRNumber,
                        IsExactSTRNumberMatch = true,
                        ApplySTRNumberFilter = true
                    };

                    ICollection<STRTag> strTagRecs = Business.GetSTRTag(searchCriteria);
                    if ((strTagRecs?.Count ?? 0) == 0)
                    {
                        yield return new ValidationResult($"Invalid STR # {STRNumber}");
                        yield break;
                    }

                    if ((strTagRecs?.Count ?? 0) > 1)
                    {
                        yield return new ValidationResult($"STR # {STRNumber} is not unique.");
                        yield break;
                    }

                    // user can change STR Number - edit has to be allowed on both STR #s - original and changed
                    if (!strTagRecs.First().IsEditAllowed)
                    {
                        yield return new ValidationResult($"STR {STRNumber} is marked as Silo Checked. Edit operation is not allowed.");
                        yield break;
                    }

                    // there must be a record in STR table for the employeeId, 
                    // with new STRTagId and original employeeId
                    ICollection<STR> targetHomeStrs = Business.GetSTR(strTagRecs.First().Id);
                    int c = targetHomeStrs?.Count(x => x.EmployeeId == OrigRec.EmployeeId) ?? 0;
                    if (c == 0)
                    {
                        yield return new ValidationResult($"STR # {STRNumber} can't be assigned to this DWS - as there is no record for the current user in the STR");
                        yield break;
                    }
                    if (c > 1)
                    {
                        yield return new ValidationResult($"STR # {STRNumber} can't be assigned to this DWS - as there are too many record for the current user in the STR");
                        yield break;
                    }
                }

                // validate Agreement 
                if (OrigRec.Agreement != Agreement)
                {
                    HasAgreementChanged = true;
                    ICollection<DomainEntities.EntityWorkFlow> entityWorkFlows =
                                Business.GetEntityWorkFlow(Agreement);

                    if ((entityWorkFlows?.Count ?? 0) == 0)
                    {
                        yield return new ValidationResult($"Invalid Agreement # {Agreement}");
                        yield break;
                    }
                    else
                    {
                        EntityWFRec = entityWorkFlows.First();
                    }
                }

                // if DWS # is changed, ensure that there isn't any other record with same DWS #
                if (!DWSNumber.Equals(OrigRec.DWSNumber, StringComparison.OrdinalIgnoreCase) && Business.IsExistingDWS(DWSNumber))
                {
                    yield return new ValidationResult($"DWS {DWSNumber} already exist.");
                    yield break;
                }
            }
        }

        private DateTime ConvertStringToDateTime(string datetimeAsText)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-GB");
            DateTime fDate = DateTime.MinValue;
            bool isValidDate = DateTime.TryParse(datetimeAsText, culture, DateTimeStyles.None, out fDate);
            return fDate;
        }
    }
}