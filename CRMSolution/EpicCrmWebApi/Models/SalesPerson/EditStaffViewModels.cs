using CRMUtilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class SalesPersonViewModel
    {
        public int Id { get; set; }

        [Required]
        //[RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Invalid Employee Code")]
        [RegularExpression(@"^[0-9a-zA-Z]{4,10}$", ErrorMessage = "Invalid Employee Code")]
        [Display(Name="Employee Code")]
        public string StaffCode { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage ="Name can be maximum 50 characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Phone can be maximum 20 characters")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "HQ Code can be maximum 10 characters")]
        public string HQCode { get; set; }

        [MaxLength(10, ErrorMessage = "Grade can be maximum 10 characters")]
        public string Grade { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "CRM Id")]
        public long EmployeeId { get; set; }

        [Display(Name = "Portal")]
        public bool OnWeb { get; set; }

        public string HQName { get; set; }

        public string Department { get; set; } 
        public string Designation { get; set; }
        public string Ownership { get; set; }
        
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; }

        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }

        [MaxLength(15, ErrorMessage = "Vehicle Number can be maximum 15 characters")]
        [Display(Name = "Vehicle Number")]
        public string VehicleNumber { get; set; }

        [MaxLength(50, ErrorMessage = "Business Role can be maximum 50 characters")]
        [Display(Name = "Business Role")]
        public string BusinessRole { get; set; }


        [Display(Name = "Override Private Vehicle Expense Rate")]
        public bool OverridePrivateVehicleRatePerKM { get; set; }

        [Display(Name = "2 Wheeler Rate / Km")]
        public decimal TwoWheelerRatePerKM { get; set; }

        [Display(Name = "4 Wheeler Rate / Km")]
        public decimal FourWheelerRatePerKM { get; set; }

        public DomainEntities.SalesPersonModel GetBusinessModel()
        {
            return new DomainEntities.SalesPersonModel()
            {
                HQCode = HQCode.CrmTrim(),
                Name = Name.CrmTrim(),
                Phone = Phone.CrmTrim(),
                StaffCode = StaffCode.CrmTrim().ToUpper(),
                Grade = Grade.CrmTrim(),
                IsActive = IsActive,
                Department = Department.CrmTrim(),
                Designation = Designation.CrmTrim(),
                Ownership = Ownership.CrmTrim(),
                VehicleType = VehicleType.CrmTrim(),
                FuelType = FuelType.CrmTrim(),
                VehicleNumber = VehicleNumber.CrmTrim(),
                BusinessRole = BusinessRole.CrmTrim(),
                OverridePrivateVehicleRatePerKM = OverridePrivateVehicleRatePerKM,
                TwoWheelerRatePerKM = TwoWheelerRatePerKM,
                FourWheelerRatePerKM = FourWheelerRatePerKM
            };
        }
    }
}
