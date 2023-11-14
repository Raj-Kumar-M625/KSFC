

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class NetWorthMasterDTO
    {
        [DisplayName("Net Worth Code")]
        public short NetCd { get; set; }

        [DisplayName("Net Worth Desc")]
        public string NetDesc { get; set; }

        [DisplayName("Id")]
        public int Id { get; set; }
    }
}
