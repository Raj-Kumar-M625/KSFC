using EDCS_TG.API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.DTO
{
    public class BasicSurveyDetailDto
    {

        public string? SurveyId { get; set; }
        public Guid? UserId { get; set; }

        public int? DBTEKYCDataId { get; set; }
        public string? Name { get; set; }
        public DateTime? DOB { get; set; }

        public string? GenderByBirth { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public string? Taluk { get; set; }
        public string? Hobli { get; set; }

        public string? VillageOrWard { get; set; }

        public string? PinCode { get; set; }

        public string? PresentAddress { get; set; }
        public string? PresentDistrict { get; set; }
        public string? PresentTaluk { get; set; }
        public string? PresentHobli { get; set; }
        public string? PresentVillageOrWard { get; set; }

        public string? PresentPinCode { get; set; }
        public string? AadharStatus { get; set; }
        public string? Status { get; set; }

        public string? Locale { get; set; }
        public string? Email { get; set; }

        public string? number { get; set; }
        public int? Age { get; set; }
        public DateTime? Created_Date { get; set; }


        public virtual User? User { get; set; }
    }
}
