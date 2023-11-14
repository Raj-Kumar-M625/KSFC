using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.Employee
{
    public class EmployeeDTO
    {
        public EmployeeDTO()
        {
            EmpDesignation = new EmployeeDesignationDTO();
        }

        //[DisplayName("Unit Code")]
        //[Required(ErrorMessage = "The Unit Code is required")]
        //public string TeyUnitCode { get; set; }

        [DisplayName("Ticket Number")]
        [Required(ErrorMessage = "The Employee No is required")]
        public string TeyTicketNum { get; set; }

        //[DisplayName("Staf typeCode")]
        //[Required(ErrorMessage = "The Staf typeCode is required")]
        //public string TeyStaftypeCode { get; set; }

        //[DisplayName("Grade Code")]
        //[Required(ErrorMessage = "The Grade Code is required")]
        //public string TeyGradeCode { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "The Name is required")]
        public string TeyName { get; set; }

        [DisplayName("Sex")]
        [Required(ErrorMessage = "The Sex is required")]
        public string TeySex { get; set; }

        //[DisplayName("Mode Of Pay")]
        //[Required(ErrorMessage = "The Mode Of Pay is required")]
        //public string TeyModeOfPay { get; set; }

        //[DisplayName("Deptartment Code")]
        //[Required(ErrorMessage = "The Deptartment Code is required")]
        //public string TeyDeptCode { get; set; }

        //[DisplayName("Alias Name")]
        //[Required(ErrorMessage = "The Alias Name is required")]
        //public string TeyAliasName { get; set; }

        //[DisplayName("Delete Statu")]
        //[Required(ErrorMessage = "The Delete Status is required")]
        //public string TeyDeleteStatus { get; set; }

        //[DisplayName("Join Date")]
        //[Required(ErrorMessage = "The Join Date is required")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? TeyJoinDate { get; set; }

        //[DisplayName("Employee Type")]
        //[Required(ErrorMessage = "The Unit Code is required")]
        //public string TeyEmpType { get; set; }

        //[DisplayName("Work Area")]
        //[Required(ErrorMessage = "The Work Area is required")]
        //public string TeyWorkArea { get; set; }

        [DisplayName("PAN")]
        [Required(ErrorMessage = "PAN is required")]
        //[RegularExpression(@"^([A-Z]{5}\d{4}[A-Z]{ 1})$")]
        public string TeyPanNum { get; set; }

        //[DisplayName("PF Num")]
        //[Required(ErrorMessage = "The PF Num is required")]
        //public string TeyPfNum { get; set; }

        //[DisplayName("Separation Type")]
        //[Required(ErrorMessage = "The Separation Type is required")]
        //public string TeySeparationType { get; set; }

        //[DisplayName("Lastdate Increment")]
        //[Required(ErrorMessage = "The Lastdate Increment is required")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? TeyLastdateIncrement { get; set; }

        [DisplayName("Lastdate Promotion")]
        [Required(ErrorMessage = "The Lastdate Promotion is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? TeyLastdatePromotion { get; set; }

        //[DisplayName("Separation Date")]
        //[Required(ErrorMessage = "The Separation Date is required")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? TeySeparationDate { get; set; }

        //[DisplayName("FatherHusband Name")]
        //[Required(ErrorMessage = "The FatherHusband Name is required")]
        //public string TeyFatherHusbandName { get; set; }

        [DisplayName("Birth Date")]
        [Required(ErrorMessage = "The Birth Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? TeyBirthDate { get; set; }

        //[DisplayName("Blood Group")]
        //[Required(ErrorMessage = "The BloodGroup is required")]
        //public string TeyBloodGroup { get; set; }

        //[DisplayName("Marital Status")]
        //[Required(ErrorMessage = "The Marital Status is required")]
        //public string TeyMaritalStatus { get; set; }

        //[DisplayName("Eye Sight")]
        //[Required(ErrorMessage = "The Eye Sight is required")]
        //public string TeyEyeSight { get; set; }

        //[DisplayName("Colour Blindness")]
        //[Required(ErrorMessage = "The Colour Blindness is required")]
        //public string TeyColourBlindness { get; set; }

        //[DisplayName("Whether Handicapped?")]
        //[Required(ErrorMessage = "The Whether Handicap is required")]
        //public string TeyWhetherHandicap { get; set; }

        public bool IsCheckedIn { get; set; }

        [DisplayName("Present Address1")]
        [Required(ErrorMessage = "The Present Address1 is required")]
        public string TeyPresentAddress1 { get; set; }

        //[DisplayName("Present Address2")]
        //[Required(ErrorMessage = "The Present Address2e is required")]
        //public string TeyPresentAddress2 { get; set; }

        [DisplayName("Present City")]
        [Required(ErrorMessage = "The Present City is required")]
        public string TeyPresentCity { get; set; }

        //[DisplayName("Present State")]
        //[Required(ErrorMessage = "The Present State is required")]
        //public string TeyPresentState { get; set; }

        [DisplayName("Present Zip")]
        [Required(ErrorMessage = "The Present Zip is required")]
        public string TeyPresentZip { get; set; }

        //[DisplayName("Permanent Address1")]
        //[Required(ErrorMessage = "The Permanent Address1 is required")]
        //public string TeyPermanentAddress1 { get; set; }

        //[DisplayName("Permanent Address2")]
        //[Required(ErrorMessage = "The Permanent Address2 is required")]
        //public string TeyPermanentAddress2 { get; set; }

        //[DisplayName("Permanent City")]
        //[Required(ErrorMessage = "The Permanent City is required")]
        //public string TeyPermanentCity { get; set; }

        //[DisplayName("Permanent State")]
        //[Required(ErrorMessage = "The Permanent State is required")]
        //public string TeyPermanentState { get; set; }

        //[DisplayName("Permanent Zip")]
        //[Required(ErrorMessage = "The Permanent Zip is required")]
        //public string TeyPermanentZip { get; set; }

        //[DisplayName("Footware Size")]
        //[Required(ErrorMessage = "The Footware Size is required")]
        //public int? TeyFootwareSize { get; set; }

        //[DisplayName("Next IncrementDate")]
        //[Required(ErrorMessage = "The Next IncrementDate is required")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? TeyNextIncrementDate { get; set; }

        //[DisplayName("Vpf Percent")]
        //[Required(ErrorMessage = "The Vpf Percent is required")]
        //public decimal? TeyVpfPercent { get; set; }

        //[DisplayName("Spouse Name")]
        //[Required(ErrorMessage = "The Spouse Name is required")]
        //public string TeySpouseName { get; set; }

        //[DisplayName("Present Phone")]
        //[Required(ErrorMessage = "The Present Phone is required")]
        //public int? TeyPresentPhone { get; set; }

        [DisplayName("Present Email")]
        [Required(ErrorMessage = "The Present Email is required")]
        public string TeyPresentEmail { get; set; }

        //[DisplayName("Permanent Phone")]
        //[Required(ErrorMessage = "The Permanent Phone is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        //public int? TeyPermanentPhone { get; set; }

        //[DisplayName("Permanent Email")]
        //[Required(ErrorMessage = "The Permanent Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        //public string TeyPermanentEmail { get; set; }

        //[DisplayName("Pay Status")]
        //[Required(ErrorMessage = "The Pay Status is required")]
        //public string TeyPayStatus { get; set; }

        //[DisplayName("Employee Status")]
        //[Required(ErrorMessage = "The Employee Status is required")]
        //public string TeyEmployeeStatus { get; set; }

        //[DisplayName("Super User")]
        //[Required(ErrorMessage = "The Super User is required")]
        //public string TeySuperUser { get; set; }

        //[DisplayName("Entry Basic")]
        //[Required(ErrorMessage = "The Entry Basic is required")]
        //public decimal? TeyEntryBasic { get; set; }

        //[DisplayName("Current Basic")]
        //[Required(ErrorMessage = "The Current Basic is required")]
        //public decimal? TeyCurrentBasic { get; set; }

        //[DisplayName("Nationality")]
        //[Required(ErrorMessage = "The Nationality is required")]
        //public string TeyNationality { get; set; }

        //[DisplayName("Religion Code")]
        //[Required(ErrorMessage = "The Religion Code is required")]
        //public string TeyReligionCode { get; set; }

        //[DisplayName("Caste Code")]
        //[Required(ErrorMessage = "The Caste Code is required")]
        //public string TeyCasteCode { get; set; }

        //[DisplayName("Category Code")]
        //[Required(ErrorMessage = "The Category Code is required")]
        //public string TeyCategoryCode { get; set; }

        //[DisplayName("Current Unit")]
        //[Required(ErrorMessage = "The Current Unit is required")]
        //public string TeyCurrentUnit { get; set; }

        //[DisplayName("Home State")]
        //[Required(ErrorMessage = "The Home State is required")]
        //public string TeyHomeState { get; set; }

        //[DisplayName("Home City")]
        //[Required(ErrorMessage = "The Home City is required")]
        //public string TeyHomeCity { get; set; }

        //[DisplayName("Mother Tongue")]
        //[Required(ErrorMessage = "The Mother Tongue is required")]
        //public string TeyMotherTongue { get; set; }

        //[DisplayName("Joining Date")]
        //[Required(ErrorMessage = "The Joining Date is required")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? TeyAjoinDate { get; set; }

        //[DisplayName("Seperation Date")]
        //[Required(ErrorMessage = "The Seperation Date is required")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? TeySeparationRef { get; set; }

        //[DisplayName("Confirm Date")]
        //[Required(ErrorMessage = "The Confirm Date is required")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? TeyConfirmDate { get; set; }

        [DisplayName("Mobile Number")]
        [Required(ErrorMessage = "The Mobile Number is required")]
        public string EmpMobileNo { get; set; }
        public EmployeeDesignationDTO EmpDesignation { get; set; }
        //public List<EmployeeDesignationDTO> EmpDesignationList { get; set; }



    }
}
