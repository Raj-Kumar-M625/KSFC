using System.ComponentModel;

namespace KAR.KSFC.Components.Common.Dto
{
    public class TokenDTO
    {

        [DisplayName("Access_Token")]
        public string Access_Token { get; set; }


        [DisplayName("Refresh_Token")]
        public string Refresh_Token { get; set; }
    }
}
