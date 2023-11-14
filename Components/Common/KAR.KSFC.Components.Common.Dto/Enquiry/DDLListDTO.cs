using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class DDLListDTO
    {
        public List<Branch> ListBranch { get; set; }
        public List<LoanPurpose> ListLoanPurpose { get; set; }
        public List<FirmSize> ListFirmSize { get; set; }
        public List<ProductType> ListProduct { get; set; }
        public List<DistrictMast> ListDistrict { get; set; }
        public List<TalukMast> ListTaluka { get; set; }
        public List<HobliMast> ListHobli { get; set; }
        public List<VillageMast> ListVillage { get; set; }
        public List<PremisesType> ListPremises { get; set; }
        public List<BankAccountType> ListAccountType { get; set; }
        public List<FirmRegistration> ListRegnType { get; set; }
        public List<PromDesnType> ListPromDesgnType { get; set; }
        public List<DomicileStatus> ListDomicileStatus { get; set; }
        public List<PromoterDetailsMast> ListPromoter { get; set; }
        public List<AssetCategory> ListAssetCategory { get; set; }
        public List<AssetType> ListAssetType { get; set; }
        public List<AssetAcquire> ListAcquireMode { get; set; }
        public List<GuarantorrDetailsMast> ListGuarantorMast { get; set; }
        public List<BankFacilityType> ListFacility { get; set; }
        public List<SisterConcernMast> ListAssociate { get; set; }
        public List<FinancialYear> ListFY { get; set; }
        public List<FinancialComponent> ListFinancialComponent { get; set; }
        public List<ProjectCostDTO> ListProjectCost { get; set; }
        public List<MeansOfFinanceCategory> ListMeansOfFinanceCategory { get; set; }
        public List<MeansOfFinanceType> ListMeansOfFinanceType { get; set; }
        public List<SecurityTyp> ListSecurityType { get; set; }
        public List<SecurityDetail> ListSecurityDet { get; set; }
        public List<PromoterRelation> ListSecurityRelation { get; set; }
    }


    public class Branch
    {
        public int BranchCode { get; set; }
        public string BranchName { get; set; }
    }

    public class LoanPurpose
    {
        public int PurposeCode { get; set; }
        public string PurposeDesc { get; set; }
    }

    public class FirmSize
    {
        public int SizeCode { get; set; }
        public string SizeDesc { get; set; }
    }

    public class IndustryType
    {
        public int Typecode { get; set; }
        public string Typedesc { get; set; }
    }

    public class ProductType
    {
        public int ProductCode { get; set; }
        public string ProductDesc { get; set; }
    }

    public class PremisesType
    {
        public int PremisesCode { get; set; }
        public string Premisesdesc { get; set; }
    }

    public class BankAccountType
    {
        public int AccountTypeCode { get; set; }
        public string AccountTypeDesc { get; set; }
    }

    public class AddressType
    {
        public int AddressTypecode { get; set; }
        public string AddressTypeDesc { get; set; }
    }

    public class FirmRegistration
    {
        public int RegnTypeCode { get; set; }
        public string RegnTypeDesc { get; set; }
    }

    public class PromDesnType
    {
        public int DesnCode { get; set; }
        public string DesgnDesc { get; set; }
    }

    public class PromoterDetailsMast
    {
        public int PromoCode { get; set; }
        public string PromoDesc { get; set; }
    }

    public class GuarantorrDetailsMast
    {
        public int GuarCode { get; set; }
        public string GuarDesc { get; set; }
    }

    public class DomicileStatus
    {
        public int StatusCode { get; set; }
        public string StatusDesc { get; set; }
    }

    public class AssetCategory
    {
        public int CategoryCode { get; set; }
        public string AssetDesc { get; set; }
    }

    public class AssetAcquire
    {
        public int AcquireCode { get; set; }
        public string AcquireDesc { get; set; }
    }

    public class AssetType
    {
        public int AssetTypeCode { get; set; }
        public string AssetTypeDesc { get; set; }
    }

    public class BankFacilityType
    {
        public int FacilityTypeCode { get; set; }
        public string FacilityTypeDesc { get; set; }
    }

    public class SisterConcernMast
    {
        public int Code { get; set; }
        public string Desc { get; set; }
    }

    public class ProjctCost
    {
        public int ProjectCode { get; set; }
        public string ProjectDesc { get; set; }
    }

    public class ProjectCostGroup
    {
        public int GroupCode { get; set; }
        public string GroupDesc { get; set; }
    }

    public class ProjectCostSubGroup
    {
        public int SubGroupCode { get; set; }
        public string SubGroupDesc { get; set; }
    }

    public class FinancialYear
    {
        public int YearCode { get; set; }
        public string YearDesc { get; set; }
    }

    public class FinancialComponent
    {
        public int ComponentCode { get; set; }
        public string ComponentDesc { get; set; }
    }

    public class MeansOfFinanceCategory
    {
        public int MFCategoryCode { get; set; }
        public string MFCategoryDesc { get; set; }
    }

    public class MeansOfFinanceType
    {
        public int TypeCode { get; set; }
        public string TypeDesc { get; set; }
    }

    public class SecurityTyp
    {
        public int TypeCode { get; set; }
        public string Typedesc { get; set; }
    }

    public class SecurityDetail
    {
        public int DetailsCode { get; set; }
        public string DetailsDesc { get; set; }
    }

    public class PromoterRelation
    {
        public int RelationCode { get; set; }
        public string RelationDesc { get; set; }
    }

    public class DocumentCategory
    {
        public int CategoryCode { get; set; }
        public string Categorydesc { get; set; }
    }

    public class DocumentDetails
    {
        public int DetailsCode { get; set; }
        public string DetailsDesc { get; set; }
    }

    public class DistrictMast
    {
        public byte DistCd { get; set; }
        public string DistNam { get; set; }
    }

    public class TalukMast
    {
        public byte TlqCd { get; set; }
        public string TlqNam { get; set; }
    }

    public class HobliMast
    {
        public int HobCd { get; set; }
        public string HobNam { get; set; }
    }

    public class VillageMast
    {
        public int VilCd { get; set; }
        public string VilNam { get; set; }
    }
}
