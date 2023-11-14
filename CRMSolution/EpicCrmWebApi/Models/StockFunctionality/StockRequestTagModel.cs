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
    public class StockRequestTagModel : StockTagModel, IValidatableObject
    {
        public long Id { get; set; }

        [Display(Name = "Req. #")]
        public string RequestNumber { get; set; }

        [Display(Name = "Request Date")]
        public System.DateTime RequestDate { get; set; }

        [Display(Name = "Request Date")]
        public string RequestDateAsText { get; set; }

        [Display(Name = "Notes (100 chars)")]
        [MaxLength(100, ErrorMessage = "Notes can be maximum 100 characters.")]
        public string Notes { get; set; }

        public string Status { get; set; }

        [Display(Name = "Line Items Quantity")]
        public int ItemCountFromLines { get; set; }

        public long CyclicCount { get; set; }
        public bool IsEditAllowed { get; set; }

        public string RequestType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            RequestDate = Helper.ConvertStringToDateTime(RequestDateAsText);
            DateTime today = Helper.GetCurrentIstDateTime().Date;

            if (RequestDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify Request Date.");
                yield break;
            }

            RequestDate = RequestDate.Date;

            if (RequestDate > today)
            {
                yield return new ValidationResult("Request Date can not be in the future.");
                yield break;
            }

            // Validate Warehouse selection
            var isZoneSelected = !String.IsNullOrEmpty(ZoneCode);
            var isAreaSelected = !String.IsNullOrEmpty(AreaCode);
            var isTerritorySelected = !String.IsNullOrEmpty(TerritoryCode);
            var isHQSelected = !String.IsNullOrEmpty(HQCode);
            var isStaffSelected = !String.IsNullOrEmpty(StaffCode);

            if (isStaffSelected == false && isZoneSelected == false)
            {
                yield return new ValidationResult("Please select delivery destination.");
                yield break;
            }

            if (isStaffSelected && isZoneSelected)
            {
                yield return new ValidationResult("With Employee Code entry, Zone/Area/Territory/HQ can't be selected.");
                yield break;
            }

            if (isStaffSelected == false)
            {
                IEnumerable<OfficeHierarchy> officeHierarchy = Helper.GetOfficeHierarchy();
                if (isHQSelected)
                {
                    if (officeHierarchy.Any(x => x.HQCode.Equals(HQCode, StringComparison.OrdinalIgnoreCase)
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
            }

            // validate staff code if entered
            if (isStaffSelected)
            {
                // retrieve the list of employees, that current user can see.
                var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
                ICollection<string>  visibleStaffCodes = Business.GetVisibleStaffCodes(securityContext.Item1, securityContext.Item2);

                if (!visibleStaffCodes.Any(x=> x == StaffCode))
                {
                    yield return new ValidationResult("Please enter a valid Employee Code, that current logged in user has access to.");
                    yield break;
                }
            }
        }
    }
}