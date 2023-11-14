using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmProjland
    {
        public long PjLandRowId { get; set; }
        public int?  UtCd { get; set; }
        public byte? PjLandOffc { get; set; }
        public int? PjLandUnit { get; set; }
        public int?  PjLandSno { get; set; }
        public decimal? PjLandArea { get; set; }
        public string? PjLandAreaIn { get; set; }
        public int? PjLandType { get; set; }
        public decimal? PjLandCost { get; set; }
        public decimal? PjLandSubArea  { get; set; }
        public int? PjLandAreaUnit { get; set; }
        public int? PjLandSubAreaUnit  { get; set; }
        public int? PjLandLnd { get; set; }
        public int? UtSlno  { get; set; }
        public long? LoanAcc  { get; set; }
        public int? LoanSub  { get; set; }
        public string? PjLandSiteNo { get; set; }
        public string? PjLandAddress  { get; set; }
        public string? PjLandDim { get; set; }
        public string? PjLandLndDetails { get; set; }
        public string? PjLandLocation { get; set; }
        public string? PjLandLandMark  { get; set; }
        public string? PjLandConversationDet { get; set; }
        public bool? FinanceKsfc { get; set; }
        public bool? Existingland { get; set; }
        public string? PjLandNotes { get; set; }
        public string? PjLandUpload { get; set; }
        public long? PjLandItemNo { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UniqueId { get; set; }

    }
}
