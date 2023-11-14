using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.DTO
{
    public class PersonalDetailsDto
    {
         public int SurveyTypeId { get; set; }
        public string? Name { get; set; }

        public string? AadharNumber { get; set; }

        public string TransgenderCommunity { get; set; }
        public int? Age { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }
        public string? ChosenGender { get; set; }
        public string? IntersexVariation { get; set; }
        public string? PlaceofBirth { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? CurrentAddress { get; set; }
        public string? CurrentDistrict { get; set; }
        public string? CurrentTaluk { get; set; }
        public Guid? CurrentHobli { get; set; }
        public string? CurrentVillage { get; set; }
        public int? CurrentPinCode { get; set; }

        public int? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public bool FamilyAccepted { get; set; }
        public string? LiveWithParents { get; set; }
        public string? LiveSingle { get; set; }
        public string? Married { get; set; }

        public bool? Dependents { get; set; }

        public bool? DependentOnSomeone { get; set; }
        public bool? PeopleLiveInHouse { get; set; }
        public bool? WorkingInHouse { get; set; }

        public bool? LiveWithCommunity { get; set; }
        public bool? Caste { get; set; }
        public bool? Religion { get; set; }

        public int? MotherTongue { get; set; }
        public string? Migration { get; set; }
        public string? MigrationYears { get; set; }
        public string? AttireWear { get; set; }
        public string? ClothesSenseGender { get; set; }
    }
}
