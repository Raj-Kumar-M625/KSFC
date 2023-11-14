using System.Collections.Generic;

namespace KAR.KSFC.UI.Models
{
    public class EmployeeClaimsModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Claims Claims { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> AccessebleRoles { get; set; }
    }
}
