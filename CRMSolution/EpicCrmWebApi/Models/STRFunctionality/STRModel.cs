using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class STRModel : IValidatableObject
    {
        public long Id { get; set; }
        public long STRTagId { get; set; }

        [Display(Name ="Employee Id")]
        public long EmployeeId { get; set; }

        [Display(Name = "Vehicle #")]
        [MaxLength(50, ErrorMessage = "Vehicle # can be maximum 50 characters long.")]
        [Required]
        public string VehicleNumber { get; set; }

        [Display(Name = "Driver Name")]
        [MaxLength(50, ErrorMessage = "Driver Name can be maximum 50 characters long.")]
        public string DriverName { get; set; }

        [Display(Name = "Driver Phone")]
        [MaxLength(50, ErrorMessage = "Driver Phone can be maximum 50 characters long.")]
        public string DriverPhone { get; set; }

        [Display(Name = "# DWS")]
        public long DWSCount { get; set; }

        [Display(Name = "# Bag")]
        public long BagCount { get; set; }

        [Display(Name = "Gross Wt.")]
        public decimal GrossWeight { get; set; }

        [Display(Name = "Net Wt.")]
        public decimal NetWeight { get; set; }

        [Display(Name = "Start Odometer")]
        public long StartOdometer { get; set; }

        [Display(Name = "End Odometer")]
        public long EndOdometer { get; set; }

        [Display(Name = "Is New")]
        public bool IsNew { get; set; }

        [Display(Name = "Transferred?")]
        public bool IsTransferred { get; set; }

        [Display(Name = "Transferee Name")]
        public string TransfereeName { get; set; }

        [Display(Name = "Transferee Phone")]
        public string TransfereePhone { get; set; }

        [Display(Name = "Image Count")]
        public int ImageCount { get; set; }

        public long ActivityId { get; set; }

        public long ActivityId2 { get; set; }

        [Display(Name = "Employee Code")]
        [MaxLength(10, ErrorMessage = "Employee Code can be maximum 10 characters long.")]
        [Required]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Employee Phone")]
        public string EmployeePhone { get; set; }

        [Display(Name = "STR Number")]
        [MaxLength(50, ErrorMessage = "STRNumber can be maximum 50 characters long.")]
        [Required]
        public string STRNumber { get; set; }

        public long StrTagCyclicCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            EmployeeCode = Utils.TruncateString(EmployeeCode, 10);
            STRNumber = Utils.TruncateString(STRNumber, 50);

            var tagRec = Business.GetSingleSTRTag(STRTagId);
            if (!tagRec.IsEditAllowed)
            {
                yield return new ValidationResult($"STR {tagRec.STRNumber} is marked as Silo Checked. Edit operation is not allowed.");
                yield break;
            }

            if (StartOdometer <= 0 || EndOdometer <= 0 || EndOdometer < StartOdometer)
            {
                yield return new ValidationResult($"Please enter valid start / end odometer readings.");
                yield break;
            }

            if (Id == 0)
            {
                // check that Employee Code does exist
                EmployeeRecord empRec = Business.GetTenantEmployee(EmployeeCode);

                if (empRec == null)
                {
                    yield return new ValidationResult($"Invalid Employee Code {EmployeeCode}");
                    yield break;
                }

                // check that there isn't any duplicate on STR # and Employee Code
                var allSTRs = Business.GetSTR(STRTagId);
                if (allSTRs.Any(x => x.EmployeeId == empRec.EmployeeId))
                {
                    yield return new ValidationResult($"There is already a record for Employee {EmployeeCode} in STR {tagRec.STRNumber}.");
                    yield break;
                }
            }
            else
            {
                DomainEntities.STR origRec = Business.GetSingleSTR(Id);

                if (!origRec.STRNumber.Equals(STRNumber, StringComparison.OrdinalIgnoreCase))
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
                        yield return new ValidationResult($"Invalid STR Number.");
                        yield break;
                    }

                    if ((strTagRecs?.Count ?? 0) > 1)
                    {
                        yield return new ValidationResult($"STR Number given is not unique.");
                        yield break;
                    }

                    if (!strTagRecs.First().IsEditAllowed)
                    {
                        yield return new ValidationResult($"STR {STRNumber} is marked as Silo Checked. Edit operation is not allowed.");
                        yield break;
                    }
                }

                if (!origRec.EmployeeCode.Equals(EmployeeCode, StringComparison.OrdinalIgnoreCase))
                {
                    // check that Employee Code does exist
                    EmployeeRecord empRec = Business.GetTenantEmployee(EmployeeCode);

                    if (empRec == null)
                    {
                        yield return new ValidationResult($"Invalid Employee Code");
                        yield break;
                    }

                    // check that there isn't any duplicate on STR # (note: user may have changed str #) 
                    // if user has changed str #, then moveToSTRTagId will point to new str Tag Rec.
                    // and Employee Code
                    var strTagRec = Business.GetSTRTag(STRNumber);
                    var allSTRs = Business.GetSTR(strTagRec.Id);
                    if (allSTRs.Any(x => x.EmployeeId == empRec.EmployeeId))
                    {
                        yield return new ValidationResult($"There is already a record for Employee {EmployeeCode} in STR {STRNumber}.");
                        yield break;
                    }

                    origRec.EmployeeId = empRec.EmployeeId;
                }
            } // edit
        }
    }
}