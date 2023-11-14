using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Data.Models.DbModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService
{
    public interface IInspectionOfUnitService
    {
        //Author: Manoj Date:25/08/2022
        Task<List<LoanAccountNumberDTO>> GetAllLoanNumber();
        #region Land Inspection
        //Author: Manoj Date:25/08/2022
        Task<IEnumerable<IdmDchgLandDetDTO>> GetAllLandInspectionList(long accountNumber, long InspectionId);
        Task<bool> UpdateLandInspectionDetails(IdmDchgLandDetDTO Inspection);
        Task<bool> DeleteLandInspectionDetails(IdmDchgLandDetDTO dto);
        Task<bool> CreateLandInspectionDetails(IdmDchgLandDetDTO addr);
        #endregion

        //Author: Sandeep M Date:25/08/2022
        #region InspectionDetail
        Task<IEnumerable<IdmDspInspDTO>> GetAllInspectionDetailsList(long accountNumber);
        Task<bool> DeleteInspectionDetails(IdmDspInspDTO dto);
        Task<bool> CreateInspectionDetails(IdmDspInspDTO addr);
        Task<bool> UpdateInspectionDetails(IdmDspInspDTO landInspection);
        #endregion


        #region Building Inspection
        //Author: Swetha Date:25/08/2022
        Task<IEnumerable<IdmDchgBuildingDetDTO>> GetAllBuildingnspectionList(long accountNumber, long InspectionId);
        Task<bool> CreateBuildingInspectionDetails(IdmDchgBuildingDetDTO buildingInspection);
        Task<bool> UpdateBuildingInspectionDetails(IdmDchgBuildingDetDTO buildingInspection);
        Task<bool> DeleteBuildingInspectionDetails(IdmDchgBuildingDetDTO buildingInspection);
        #endregion
        #region Building Material at Site Inspection
        //Author Manoj on 29/08/2022
        Task<IEnumerable<IdmBuildingMaterialSiteInspectionDTO>> GetAllBuildingMaterialInspectionList(long accountNumber, long InspectionId);
        Task<bool> UpdateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspection);
        Task<bool> DeleteBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO dto);
        Task<bool> CreateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO addr);
        #endregion

     

        #region Import Machinery Inspection
        //Author: Swetha Date:01/09/2022
        Task<IEnumerable<IdmDchgImportMachineryDTO>> GetAllImportMachineryList(long accountNumber, long InspectionId);
        Task<bool> CreateImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery);
        Task<bool> UpdateImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery);
        Task<bool> DeleteImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery);
        Task<IEnumerable<TblCurrencyMastDto>> GetAllCurrencyList();

        #endregion
        //Author: Sandeep M Date:30/08/2022
        #region FurnitureInspectionDetail    
        Task<IEnumerable<IdmDChgFurnDTO>> GetAllFurnitureInspectionList(long accountNumber, long InspectionId);

        Task<bool> UpdateFurnitureInspectionDetails(IdmDChgFurnDTO Inspection);
        Task<bool> DeleteFurnitureInspectionDetails(IdmDChgFurnDTO dto);
        Task<bool> CreateFurnitureInspectionDetails(IdmDChgFurnDTO addr);

        #endregion
        #region Indigenous Machinery Inspection
        //Author Manoj on 01/09/2022
        Task<IEnumerable<IdmDchgIndigenousInspectionDTO>> GetAllIndigenousMachineryInspectionList(long accountNumber, long InspectionId);

        Task<IEnumerable<TblMachineryStatusDto>> GetAllMachineryStatusList();
        Task<IEnumerable<TblProcureMastDto>> GetAllProcureList();

        
        Task<bool> UpdateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO bildMatInspection);
        Task<bool> DeleteIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO dto);
        Task<bool> CreateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO addr);
        #endregion


        #region Working Capital
        //Task<bool> CreateWorkingCapitalDetails(IdmDchgWorkingCapitalDTO workingCapital);

        #endregion

        //#region Project Cost
        ////Author Akhila on 06/09/2022
        //Task<IEnumerable<IdmDchgProjectCostDTO>> GetAllProjectCostDetailsList(long accountNumber,long ProjectCostID);
        //Task<bool> UpdateProjectCostDetails(IdmDchgProjectCostDTO ProjectCost);
        //Task<bool> DeleteProjectCostDetails(IdmDchgProjectCostDTO dto);
        //Task<bool> CreateProjectCostDetails(IdmDchgProjectCostDTO addr);
        //#endregion

        //# region MeansOfFinance
        ////Author: Swetha Date:01/09/2022
        //Task<IEnumerable<IdmDchgMeansOfFinanceDTO>> GetAllMeansOfFinanceList(long accountNumber);

        //Task<IEnumerable<SelectListItem>> GetFinanceTypeAsync();
        //Task<bool> CreateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance);
        //Task<bool> UpdateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance);
        //Task<bool> DeleteMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance);
        //#endregion

        #region Letter Of Credit Details
        //Author Manoj on 05/09/2022
        Task<IEnumerable<IdmDsbLetterOfCreditDTO>> GetAllLetterOfCreditDetailsList(long accountNumber);
        Task<bool> UpdateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetails);
        Task<bool> DeleteLetterOfCreditDetails(IdmDsbLetterOfCreditDTO dto);
        Task<bool> CreateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO addr);
        #endregion

        #region Recommended Disbursement Details

        Task<IEnumerable<IdmDsbdetsDTO>> GetAllRecomDisbursementDetails(long accountNumber);
        Task<IEnumerable<TblAllcCdTabDTO>> GetAllocationCodeDetails();
        Task<IEnumerable<TblIdmReleDetlsDTO>> GetRecomDisbursementReleaseDetails();
        Task<bool> UpdateRecomDisbursementDetail(IdmDsbdetsDTO idmDsbdetsDTO);


        #endregion
        //#region Common DropDown
        //Task<IEnumerable<DropDownDTO>> GetAllProjectCostComponentsDetails();

        //#endregion

        #region Status of Implementation
        Task<IEnumerable<IdmDsbStatImpDTO>> GetTblDsbStatImps(long accountNumber , long InspectionId);
        Task<bool> CreateStatusofImplementation(IdmDsbStatImpDTO statusImp);
        Task<bool> UpdateStatusofImplementation(IdmDsbStatImpDTO implementation);
        Task<bool> DeleteStatusofImplementation(IdmDsbStatImpDTO impDTO);
        #endregion

    }


}
