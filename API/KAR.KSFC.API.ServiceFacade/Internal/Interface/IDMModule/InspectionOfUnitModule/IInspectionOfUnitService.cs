using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.InspectionOfUnitModule
{
    public interface IInspectionOfUnitService
    {
        //Author: Manoj Date:25/08/2022
        Task<IEnumerable<LoanAccountNumberDTO>> GetAccountNumber(CancellationToken token);
        #region Land Inspection
        Task<IEnumerable<IdmDchgLandDetDTO>> GetAllLandInspectionListAsync(long accountNumber, long InspectionId, CancellationToken token);
        Task<bool> DeleteLandInspectionDetails(IdmDchgLandDetDTO landInspectionDTO, CancellationToken token);
        Task<bool> UpdateLandInspectionDetails(IdmDchgLandDetDTO landInspectionDTO, CancellationToken token);
        Task<bool> CreateLandInspectionDetails(IdmDchgLandDetDTO landInspectionDTO, CancellationToken token);
        #endregion

        #region InspectionDetails     
        Task<IEnumerable<IdmDspInspDTO>> GetAllInspectionDetailsList(long accountNumber, CancellationToken token);

        Task<bool> UpdateInspectionDetails(IdmDspInspDTO InspectionDTO, CancellationToken token);
        Task<bool> CreateInspectionDetails(IdmDspInspDTO inspectionDTO, CancellationToken token);

        Task<bool> DeleteInspectionDetails(IdmDspInspDTO InspectionDTO, CancellationToken token);

        #endregion


        #region Building Inspection
        //Author: Swetha Date:25/08/2022
        Task<IEnumerable<IdmDchgBuildingDetDTO>> GetAllBuildingInspectionList(long accountNumber, long InspectionId,CancellationToken token);
        Task<bool> CreateBuilidngInspectionDetails(IdmDchgBuildingDetDTO BuildingDto, CancellationToken token);
        Task<bool> UpdateBuilidngInspectionDetails(IdmDchgBuildingDetDTO BuildingDto, CancellationToken token);
        Task<bool> DeleteBuilidngInspectionDetails(IdmDchgBuildingDetDTO BuildingDto, CancellationToken token);
        #endregion
        #region Working Capital

        Task<bool> CreateWorkingCapitalInspectionDetails(IdmDchgWorkingCapitalDTO workingCapital, CancellationToken token);
        #endregion
        #region Building Material at Site Inspection
        Task<IEnumerable<IdmBuildingMaterialSiteInspectionDTO>> GetAllBuildingMaterialInspectionListAsync(long accountNumber, long InspectionId, CancellationToken token);
        Task<bool> DeleteBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspectionDTO, CancellationToken token);
        Task<bool> UpdateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspectionDTO, CancellationToken token);
        Task<bool> CreateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspectionDTO, CancellationToken token);
        #endregion
        #region Furniture Inspection
        Task<IEnumerable<IdmDChgFurnDTO>> GetAllFurnitureInspectionList(long accountNumber,long InspectionId,CancellationToken token);
        Task<bool> DeleteFurnitureInspectionDetails(IdmDChgFurnDTO furnitureInspectionDTO, CancellationToken token);
        Task<bool> UpdateFurnitureInspectionDetails(IdmDChgFurnDTO furnitureInspectionDTO, CancellationToken token);
        Task<bool> CreateFurnitureInspectionDetails(IdmDChgFurnDTO furnitureInspectionDTO, CancellationToken token);

        #endregion
        #region Indigenous machinery Inspection
        Task<IEnumerable<IdmDchgIndigenousInspectionDTO>> GetAllIndigenousMachineryInspectionListAsync(long accountNumber, int InspectionId, CancellationToken token);

        Task<IEnumerable<TblMachineryStatusDto>> GetAllMachineryStatusList(CancellationToken token);

        Task<IEnumerable<TblProcureMastDto>> GetAllProcureList(CancellationToken token);

        

        Task<bool> DeleteIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO bildMatInspectionDTO, CancellationToken token);
        Task<bool> UpdateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO bildMatInspectionDTO, CancellationToken token);
        Task<bool> CreateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO bildMatInspectionDTO, CancellationToken token);
        #endregion
        #region Import Machinery Inspection
        //Author: Swetha Date:25/08/2022
        Task<IEnumerable<IdmDchgImportMachineryDTO>> GetAllImportMachineryList(long accountNumber, long InspectionId, CancellationToken token);
        Task<bool> CreateImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery, CancellationToken token);
        Task<bool> UpdateImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery, CancellationToken token);
        Task<bool> DeleteImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery, CancellationToken token);
        Task<IEnumerable<TblCurrencyMastDto>> GetAllCurrencyList(CancellationToken token);
        #endregion

        #region Means Of Finance
        //Author: Swetha Date:25/08/2022
        Task<IEnumerable<IdmDchgMeansOfFinanceDTO>> GetAllMeansOfFinanceList(long accountNumber, CancellationToken token);
        Task<bool> CreateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance, CancellationToken token);
        Task<bool> UpdateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance, CancellationToken token);
        Task<bool> DeleteMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance, CancellationToken token);
        #endregion
        #region Letter Of Credit
        Task<IEnumerable<IdmDsbLetterOfCreditDTO>> GetAllLetterOfCreditDetailListAsync(long accountNumber, CancellationToken token);
        Task<bool> DeleteLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetailsDTO, CancellationToken token);
        Task<bool> UpdateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetailsDTO, CancellationToken token);
        Task<bool> CreateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetailsDTO, CancellationToken token);
        #endregion
        #region Project Cost Details    
        //Author: Akhila Date:5/09/2022
        Task<IEnumerable<IdmDchgProjectCostDTO>> GetAllProjectCostDetailsList(long accountNumber, CancellationToken token);

        Task<bool> UpdateProjectCostDetails(IdmDchgProjectCostDTO PrjCostDTO, CancellationToken token);
        Task<bool> CreateProjectCostDetails(IdmDchgProjectCostDTO PrjCostDTO, CancellationToken token);
        Task<bool> DeleteProjectCostDetails(IdmDchgProjectCostDTO PrjCostDTO, CancellationToken token);

        #endregion

        #region Status of Implementation
        Task<IEnumerable<IdmDsbStatImpDTO>> GetAllStatusofImplementation(long accountNumber, long InspectionId, CancellationToken token);
        Task<bool> CreateStatusofImplementation(IdmDsbStatImpDTO statusImp, CancellationToken token);
        Task<bool> UpdateStatusofImplementation(IdmDsbStatImpDTO statusImp, CancellationToken token);
        Task<bool> DeleteStatusofImplementation(IdmDsbStatImpDTO statusImp, CancellationToken token); 
        #endregion
    }
}
