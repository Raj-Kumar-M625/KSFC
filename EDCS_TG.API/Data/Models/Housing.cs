using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.Data.Models
{
    public class Housing:BaseEntity
    {
        [ForeignKey("PersonalDetails")] public int PersonalDetailsId { get; set; }
        public string? TypeOfHouse { get; set; }
        public string? TypeOfHouseIfothers { get; set; }
        public string? TypeOfRoof { get; set; }
        public string? TypeOfRoofIfothers { get; set; }
        public string? HouseOwnership { get; set; }
        public string? HouseOwnershipIfothers { get; set; }
        public string? LiveInCommunityHome { get; set; }
        public bool? LiveInCommunityHomeIfothers { get; set; }
        public bool? Toilet { get; set; }
        public string? ToiletIfothers { get; set; }
        public string? OwnSite { get; set; }
        public string? Elecrticity { get; set; }
        public string? ElecrticityOthers { get; set; }
        public string? WaterFecility { get; set; }
        public string? WaterFecilityOthers { get; set; }
        public string? Ownland { get; set; }
        public string? TypeOfLand { get; set; }
        public bool? Agliculturallandtype { get; set; }
        public string? AgliculturallandPlace { get; set; }
        public bool? TotalAgliculturalland { get; set; }
        public string? commercial { get; set; }
        public string? LiveStock { get; set; }
        public bool? LiveStockType { get; set; }
        public string? AreaOfResidence { get; set; }
        public string? AreaOfResidenceOthers { get; set; }
        public string? OwnPropertyFromParents { get; set; }
        public DateTime? ChosenGender { get; set; }

        public Guid? ShareOfProperty { get; set; }
        public Guid? ShelterSpaceRequired { get; set; }

        public Guid? ShelterSpaceOthers { get; set; }
        public Guid? HousingSchemes { get; set; }
        public Guid? HousingSchemesBeneficiary { get; set; }
        public Guid? WhichScheme { get; set; }
        public Guid? NeedOldAgeHomes { get; set; }
        public Guid? AssetsYouHave { get; set; }
        public Guid? AssetsYouHaveOthers { get; set; }




    }
}
