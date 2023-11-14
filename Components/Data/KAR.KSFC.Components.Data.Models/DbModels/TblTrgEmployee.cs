using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblTrgEmployee
    {
        public TblTrgEmployee()
        {
            TblEmpchairDets = new HashSet<TblEmpchairDet>();
            TblEmpchairhistDets = new HashSet<TblEmpchairhistDet>();
            TblEmpdesigTabs = new HashSet<TblEmpdesigTab>();
            TblEmpdesighistTabs = new HashSet<TblEmpdesighistTab>();
            TblEmpdscTabs = new HashSet<TblEmpdscTab>();
            TblEmploginTabs = new HashSet<TblEmploginTab>();
        }

        public string TeyUnitCode { get; set; }
        public string TeyTicketNum { get; set; }
        public string TeyStaftypeCode { get; set; }
        public string TeyGradeCode { get; set; }
        public string TeyName { get; set; }
        public string TeySex { get; set; }
        public string TeyModeOfPay { get; set; }
        public string TeyDeptCode { get; set; }
        public string TeyAliasName { get; set; }
        public string TeyDeleteStatus { get; set; }
        public DateTime? TeyJoinDate { get; set; }
        public string TeyEmpType { get; set; }
        public string TeyWorkArea { get; set; }
        public string TeyPanNum { get; set; }
        public string TeyPfNum { get; set; }
        public string TeySeparationType { get; set; }
        public DateTime? TeyLastdateIncrement { get; set; }
        public DateTime? TeyLastdatePromotion { get; set; }
        public DateTime? TeySeparationDate { get; set; }
        public string TeyFatherHusbandName { get; set; }
        public DateTime? TeyBirthDate { get; set; }
        public string TeyBloodGroup { get; set; }
        public string TeyMaritalStatus { get; set; }
        public string TeyEyeSight { get; set; }
        public string TeyColourBlindness { get; set; }
        public string TeyWhetherHandicap { get; set; }
        public string TeyPresentAddress1 { get; set; }
        public string TeyPresentAddress2 { get; set; }
        public string TeyPresentCity { get; set; }
        public string TeyPresentState { get; set; }
        public string TeyPresentZip { get; set; }
        public string TeyPermanentAddress1 { get; set; }
        public string TeyPermanentAddress2 { get; set; }
        public string TeyPermanentCity { get; set; }
        public string TeyPermanentState { get; set; }
        public string TeyPermanentZip { get; set; }
        public int? TeyFootwareSize { get; set; }
        public DateTime? TeyNextIncrementDate { get; set; }
        public decimal? TeyVpfPercent { get; set; }
        public string TeySpouseName { get; set; }
        public int? TeyPresentPhone { get; set; }
        public string TeyPresentEmail { get; set; }
        public int? TeyPermanentPhone { get; set; }
        public string TeyPermanentEmail { get; set; }
        public string TeyPayStatus { get; set; }
        public string TeyEmployeeStatus { get; set; }
        public string TeySuperUser { get; set; }
        public decimal? TeyEntryBasic { get; set; }
        public decimal? TeyCurrentBasic { get; set; }
        public string TeyNationality { get; set; }
        public string TeyReligionCode { get; set; }
        public string TeyCasteCode { get; set; }
        public string TeyCategoryCode { get; set; }
        public string TeyCurrentUnit { get; set; }
        public string TeyHomeState { get; set; }
        public string TeyHomeCity { get; set; }
        public string TeyMotherTongue { get; set; }
        public DateTime? TeyAjoinDate { get; set; }
        public DateTime? TeySeparationRef { get; set; }
        public DateTime? TeyConfirmDate { get; set; }
        public string EmpMobileNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEmpchairDet> TblEmpchairDets { get; set; }
        public virtual ICollection<TblEmpchairhistDet> TblEmpchairhistDets { get; set; }
        public virtual ICollection<TblEmpdesigTab> TblEmpdesigTabs { get; set; }
        public virtual ICollection<TblEmpdesighistTab> TblEmpdesighistTabs { get; set; }
        public virtual ICollection<TblEmpdscTab> TblEmpdscTabs { get; set; }
        public virtual ICollection<TblEmploginTab> TblEmploginTabs { get; set; }
    }
}
