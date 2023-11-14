using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Data.Models.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM
{
    public interface IIdmService
    {
        Task<List<LoanAccountNumberDTO>> GetAllLoanNumber(string empID);
        Task<List<TblUnitDet>> GetAccountDetails();
        #region Generic Dropdowns
        Task<IEnumerable<DropDownDTO>> GetAllConditionTypes();
        Task<IEnumerable<DropDownDTO>> GetAllConditionStages();
        Task<IEnumerable<DropDownDTO>> GetAllConditionStageMaster();
        Task<IEnumerable<DropDownDTO>> GetAllPromotorNames();
        Task<IEnumerable<DropDownDTO>> GetAllPromoterPhNo();
        Task<IEnumerable<DropDownDTO>> GetAllDistrictNames();
        Task<IEnumerable<DropDownDTO>> GetAllTalukNames();
        Task<IEnumerable<DropDownDTO>> GetAllHobliNames();
        Task<IEnumerable<DropDownDTO>> GetAllVillageNames();
        Task<IEnumerable<DropDownDTO>> GetAllForm8AndForm13Master();
        Task<IEnumerable<DropDownDTO>> GetPositionDesignationAsync();
        Task<IEnumerable<DropDownDTO>> GetAllPromotorClass();
        Task<IEnumerable<DropDownDTO>> GetDomicileStatusAsync();
        Task<IEnumerable<DropDownDTO>> GetAccountTypeAsync();
        Task<IEnumerable<DropDownDTO>> GetAllocationCodes();
        Task<IEnumerable<DropDownDTO>> GetAllLandType(); 
        Task<IEnumerable<DropDownDTO>> GetAllOtherDebitsDetails();
        Task<IEnumerable<DropDownDTO>> GetAllUomMaster();
        Task<IEnumerable<DropDownDTO>> GetAllAssetRefList();
   
        #endregion

    }
}
