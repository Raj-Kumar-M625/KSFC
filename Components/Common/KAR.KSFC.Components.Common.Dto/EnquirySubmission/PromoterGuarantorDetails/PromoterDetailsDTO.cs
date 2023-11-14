
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromoterDetailsDTO
    {
        
        public int? EnqPromId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }
       

        [DisplayName("Promoter Code")]
        public long? PromoterCode { get; set; }

        [DisplayName("Promoter ShareHolding Percentage")]
        [Required(ErrorMessage = "ShareHolding Percentage is required")]
        [Range(typeof(decimal), "0", "100")]
        public decimal? EnqPromShare { get; set; }

        [DisplayName("Promoter Experience In Years")]
        [Required(ErrorMessage = "Experience is required")]
        public int? EnqPromExp { get; set; }


        [DisplayName("Promoter Experience Details")]
        [Required(ErrorMessage = "Experience Details is required")]
        public string EnqPromExpdet { get; set; }


        [DisplayName("Position")]
        public int PdesigCd { get; set; }


        [DisplayName("Domicile")]
        public int DomCd { get; set; }


        [DisplayName("Promoter Cibil")]
        public string EnqCibil { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
        public PromoterBankDetailsDTO PromoterBankDetails { get; set; }
        public PromoterMasterDTO PromoterMaster { get; set; }
        public virtual PromDesignationMasterDTO PdesigCdNavigation { get; set; }
        public virtual DomicileMasterDTO DomCdNavigation { get; set; }

        [DisplayName("Promoter Code")]
        public long? PromCode { get; set; }


        [DisplayName("Promoter Office")]
        public byte? PromOffc { get; set; }


        [DisplayName("Promoter Unit")]
        public long? PromUnit { get; set; }


        [DisplayName("Promoter Name")]
        public string PromName { get; set; }


        [DisplayName("Promoter Designation")]
        public string PromDesg { get; set; }


        [DisplayName("Promoter Sex")]
        public string PromSex { get; set; }


        [DisplayName("Promoter Age")]
        public byte? PromAge { get; set; }


        [DisplayName("Promoter Share")]
        public decimal? PromShare { get; set; }


        [DisplayName("Promoter Domicile")]
        public byte? PromDom1 { get; set; }


        [DisplayName("Promoter Class1")]
        public byte? PromClas1 { get; set; }


        [DisplayName("Promoter Class2")]
        public byte? PromClas2 { get; set; }


        [DisplayName("Promoter Qualification1")]
        public string PromQual1 { get; set; }


        [DisplayName("Promoter Qualification2")]
        public string PromQual2 { get; set; }


        [DisplayName("Promoter ExperienceinYear")]
        public byte? PromExpYrs { get; set; }


        [DisplayName("Promoter ExperienceinDetails")]
        public string PromExpDet { get; set; }


        [DisplayName("Promoter JoiningDate")]
        public DateTime? PromJnDt { get; set; }


        [DisplayName("Promoter ExperienceDetails")]
        public DateTime? PromExDt { get; set; }



        [DisplayName("Promoter ApprovedBy")]
        public byte? PromExAppBy { get; set; }



        [DisplayName("Promoter CountryName")]
        public string PromNriCountry { get; set; }


        [DisplayName("Promoter PermanentAddress1")]
        public string PromPadr1 { get; set; }

        [DisplayName("Promoter PermanentAddress2")]
        public string PromPadr2 { get; set; }


        [DisplayName("Promoter PermanentAddress3")]
        public string PromPadr3 { get; set; }

        [DisplayName("Promoter PermanentAddress4")]
        public string PromPadr4 { get; set; }


        [DisplayName("Promoter TemporaryAddress1")]
        public string PromTadr1 { get; set; }

        [DisplayName("Promoter TemporaryAddress2")]
        public string PromTadr2 { get; set; }


        [DisplayName("Promoter TemporaryAddress3")]
        public string PromTadr3 { get; set; }


        [DisplayName("Promoter TemporaryAddress4")]
        public string PromTadr4 { get; set; }


        [DisplayName("Promoter Major")]
        public byte? PromMajor { get; set; }


        [DisplayName("Promoter NetWorthDetails")]
        public string PromNwDets { get; set; }


        [DisplayName("Promoter PanNumber")]
        public string PanNo { get; set; }



        [DisplayName("Promoter PassportNo")]
        public string PassNo { get; set; }



        [DisplayName("Promoter Guardian")]
        public string PromGuardian { get; set; }



        [DisplayName("Promoter ResidemceTelephone")]
        public long? PromResTel { get; set; }



        [DisplayName("Promoter MobileNumber")]
        public string PromMobile { get; set; }



        [DisplayName("Promoter Email")]
        
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string PromEmail { get; set; }

        [DisplayName("Promoter Handicapped")]
        public string PromPhyHandicap { get; set; }

        [DisplayName("Promoter Aadhar")]
        public string PromAadhaar { get; set; }

        [DisplayName("Promoter DOB")]
        public DateTime? PromDob { get; set; }

        [DisplayName("Promoter ApprovedEmployeeCode")]
        public int? PromExApprEmp { get; set; }
        
        public string PromoDomText { get; set; }
    } 
}
