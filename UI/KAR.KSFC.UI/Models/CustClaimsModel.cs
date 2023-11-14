using KAR.KSFC.Components.Common.Dto;

namespace KAR.KSFC.UI.Models
{
    public class CustClaimsModel
    {
        public CustomerClaimsDTO Claims { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class Claims
    {
        public string Pan { get; set; }
        public string Role { get; set; }

        public string EmpId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsPasswordChanged { get; set; }
    }
}
