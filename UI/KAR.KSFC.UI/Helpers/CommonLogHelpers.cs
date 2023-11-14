using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using System;
using System.Globalization;

namespace KAR.KSFC.UI.Helpers
{
    public static class CommonLogHelpers
    {
        #region Common Logging Message
        public const string UpdateStarted = "Started- Update method for";
        public const string UpdateStartedPost = "Started- Update HtppPost method for";
        public const string UpdateCompleted = "Completed- Update method for";
        public const string UpdateCompletedPost = "Completed- Update HtppPost method for";
        public const string CreateStarted = "Started- Create method for";
        public const string CreateStartedPost = "Started- Create HtppPost method for";
        public const string CreateCompleted = "Completed- Create method for";
        public const string CreateCompletedPost = "Completed- Create HtppPost method for";
        public const string RegisterStarted = "Started- Create method for";
        public const string RegisterStartedPost = "Started- Create HtppPost method for";
        public const string RegisterCompleted = "Completed- Create method for";
        public const string RegisterCompletedPost = "Completed- Create HtppPost method for";
        public const string Failed = "Update HtppPost method for";
        public const string ViewRecordStarted = "Started - ViewRecord method for Id = ";
        public const string ViewRecordCompleted = "Completed - ViewRecord method for Id = ";
        public const string DeleteStarted = "Started - Delete HtppPost method for Id:{0}";
        public const string DeleteCompleted = "Completed - Delete HtppPost method for Id:{0}";
        public const string StratedSavePrimaryCollateralDetails = "Started - SavePrimaryCollateralDetails with IdmSecurityDetailsDTO";
        public const string StratedSaveCollateralDetails = "Started - SaveCollateralDetails with IdmSecurityDetailsDTO";
        public const string CompletedSavePrimaryCollateralDetails = "Completed - SavePrimaryCollateralDetails with IdmSecurityDetailsDTO";
        public const string CompletedPrimaryDelete = "Completed - DeletePrimaryCollateralDetails with IdmSecurityDetailsDTO";
        public const string CompletedPrimaryUpdate = "Completed - UpdatePrimaryCollateralDetails with IdmSecurityDetailsDTO";
        public const string CompletedGuarantorDelete = "Completed - DeleteGuarantorDeedDetails with IdmGuarantorDeedDetailsDTO";
        public const string CompletedGuarantorUpdate = "Completed - UpdateGuarantorDeedDetails with IdmGuarantorDeedDetailsDTO";  
        public const string CompletedCersaiDelete = "Completed - DeleteCersaiRegistrationForLDs with CersaiRegDTO";
        public const string CompletedCersaiCreate = "Completed - CreateCersaiRegistrationForLD with CersaiRegDTO";
        public const string CompletedHypothicationDelete = "Completed - DeleteHypothecation Details with HypoAssetDto";
        public const string CompletedHypothicationCreate = "Completed - CreateHyopthecationDetails with HypoAssetDto";
        public const string CompletedHypothicationUpdate = "Completed - UpdateHyopthecationDetails with HypoAssetDto";
        public const string CompletedCondtionDelete = "Completed - DeleteCondtionDetails with CondtionDto";
        public const string CompletedCondtionCreate = "Completed - CreateCondtionDetails with CondtionDto";
        public const string CompletedCondtionUpdate = "Completed - UpdateCondtionDetails with CondtionDto";
        public const string CompletedSecurityDelete = "Completed - DeleteCersaiRegistrationForLDs with Security Charge";
        public const string CompletedSecurityUpdate = "Completed - UpdateCersaiRegistrationForLD with Security Charge";
        public const string StartedSaveCersaiDetails = "Started -  SaveCersaiRegistrationForLD with CersaiRegDTO";
        public const string CompletedSaveCersaiDetails = "Completed -  SaveCersaiRegistrationForLD with CersaiRegDTO";
        public const string StartedSaveGuarantorDeedDetails = "Started - SaveGuarantorDeedDetails with IdmGuarantorDeedDetailsDTO";
        public const string CompletedSaveGuarantorDeedDetails = "Completed- SaveGuarantorDeedDetails with IdmGuarantorDeedDetailsDTO";
        public const string StartedSaveHyopthecationDetails = "Started - SaveHyopthecationDetails with IdmGuarantorDeedDetailsDTO";
        public const string CompletedSaveHyopthecationDetails = "Completed- SaveHyopthecationDetails with IdmGuarantorDeedDetailsDTO";
        public const string StartedSaveConditionDetails = "Started - SaveConditionDetails with CondtionDTO";
        public const string CompletedSaveConditionDetails = "Completed- SaveConditionDetails with CondtionDTO";
        public const string StartedSaveSecurityChargeDetails = "Started - SaveConditionDetails with IdmSecurityChargeDTO";
        public const string CompletedSaveSecurityChargeDetails = "Completed- SaveConditionDetails with IdmSecurityChargeDTO";
        public const string StartedSaveAuditDetails = "Started - SaveAuditDetails with IdmAuditDetailsDTO";
        public const string CompletedSaveAuditDetails = "Completed- SaveAuditDetails with IdmAuditDetailsDTO";
        public const string StartedSaveDisbursementDetails = "Started - Save DisbursementDetails with CondtionDTO";
        public const string CompletedSaveDisbursementDetails = "Completed- DisbursementDetails with CondtionDTO";
        public const string StartedSaveForm8AndForm13Details = "Started - Save Form8AndForm13 Details with Form8AndForm13DTO";
        public const string CompletedSaveForm8AndForm13Details = "Completed - Form8AndForm13Details with Form8AndForm13DTO";
        public const string StartedSidbiApprovalDetails = "Started - Save SidbiApprovalDetails with IdmSidbiApprovalDTO";
        public const string CompletedSidbiApprovalDetails = "Completed - SidbiApprovalDetails with IdmSidbiApprovalDTO";
        public const string StartedOtherRelaxation = "Started - Save OtherRelaxation Details with RelaxationDTO";
        public const string CompletedOtherRelaxation = "Completed - OtherRelaxation Details with RelaxationDTO";

        public const string StartedSaveFirstInvestmentClauseDetails = "Started - Save FirstInvestmentClauseDetails with IdmFirstInvestmentClauseDTO";
        public const string CompletedSaveFirstInvestmentClauseDetails = "Completed- FirstInvestmentClauseDetails with IdmFirstInvestmentClauseDTO";

        public const string StratedSaveLandInspectionDetails = "Started - SaveLandInspectionDetails with IdmDchgLandDetDTO";
        public const string CompletedSaveLandInspectionDetails = "Completed - SaveLandInspectionDetails with IdmDchgLandDetDTO";
        public const string StartedSaveSidbiApprovalDetails = "Started - StartedSaveSidbiApprovalDetails with sidbi"; // Dev
        public const string CompletedSaveSidbiApprovalDetails = "Completed - CompletedSaveSidbiApprovalDetails with sidbi"; // Dev
        public const string StartedUploadFile = "Started - UploadFile method for IFormFile = ";
        public const string CompletedUploadFile = "Completed - UploadFle method for IFormFile = ";
        public const string StartedViewUploadFile = "Started - ViewUploadFile method for fileId = ";
        public const string CompletedViewUploadFile = "Completed - ViewUploadFile method for fileId = ";
        public const string StartedDeleteUploadFile = "Started - DeleteUploadFile method for fileId = ";

        public const string StartedSavePromoterProfile = "Started - StartedSavePromoterProfile with pprofile"; //Dev
        public const string CompletedSavePromoterProfile = "Completed - CompletedSavePromoterProfile with pprofile"; // Dev
        public const string StartedSavePromoterBank = "Started - StartedSavePromoterBank with pbank"; //Dev
        public const string CompletedSavePromoterBank = "Completed - CompletedSavePromoterBank with pbank"; // Dev

        public const string StartedSavePromoterAddressDetails = "Started - StartedSavePromoterAddressDetails with IdmUnitAddressDTO";
        public const string CompletedSavePromoterAddressDetails = "Completed - CompletedSaveSidbiApprovalDetails with IdmUnitAddressDTO";
        public const string StartedSaveProductDetails = "Started - StartedSaveProductDetails with IdmUnitProductsDTO";
        public const string StartedSaveAssetDetails = "Started - StartedSaveAssetDetails with IdmPromAssetDetDTO";
        public const string CompletedSaveAssetDetails = "Started - CompletedSaveAssetDetails with IdmPromAssetDetDTO";

        public const string StartedSavePromoterLiabilityInformation = "Started - StartedSavePromoterLiabilityInformation with TblPromoterLiabDetDTO";
        public const string CompletedSavePromoterLiabilityInformation = "Completed - CompletedSavePromoterLiabilityInformation with TblPromoterLiabDetDTO";

        public const string StartedSaveChangeLocationDetails = "Started - StartedSaveChangeLocationDetails with IdmUnitAddressDTO";
        public const string CompletedSaveChangeLocationDetails = "Completed - CompletedSaveChangeLocationDetails with IdmUnitAddressDTO";


        public const string StratedSaveInspectionDetails = "Started - SaveInspectionDetails with IdmDspInspDTO";
        public const string CompletedSaveInspectionDetails = "Completed - SaveInspectionDetails with IdmDspInspDTO";

        public const string StratedSaveBuilidngInspectionDetails = "Started - SaveBudilngInspectionDetails with IdmDchgBuildingDetDto";
        public const string CompletedSaveBuildingInspectionDetails = "Completed - SaveBuildingInspectionDetails with IdmDchgBuildingDetDto";

        public const string StratedSaveWorkingCapitalInspectionDetails = "Started - SaveBudilngInspectionDetails with IdmDchgBuildingDetDto";
        public const string CompletedSaveWorkingCapitalInspectionDetails = "Completed - SaveBuildingInspectionDetails with IdmDchgWorkingCpaital";

        public const string StratedSaveIndigenousMachineryInspectionDetails = "Started - SaveIndigenousMachineryInspectionDetails with IdmDchgIndigenousDetDTO";
        public const string CompletedSaveIndigenousMachineryInspectionDetails = "Completed - SaveIndigenousMachineryInspectionDetails with IdmDchgIndigenousDetDTO";

        public const string StratedSaveProjectCostDetails = "Started - SaveProjectCostDetails with IdmDchgProjectCostDTO";
        public const string CompletedSaveProjectCostDetails = "Completed - SaveProjectCostDetails with IdmDchgProjectCostDTO";

        public const string StartedSaveMachineryAcquisitionDetails = "Started - StartedSaveMachineryAcquisitionDetails with MachineDTO"; //Dev
        public const string CompletedSaveMachineryAcquisitionDetails = "Completed - CompletedSaveMachineryAcquisitionDetails with MachineDTO"; // Dev
        public const string StartedSaveLandAcquisitionDetails = "Started - StartedSaveLandAcquisitionDetails with LandDTO"; //Dev
        public const string CompletedSaveLandAcquisitionDetails = "Completed - CompletedSaveLandAcquisitionDetails with LandDTO"; // Dev
        public const string StartedSaveBuildingAcquisitionDetails = "Started - StartedSaveBuildingAcquisitionDetails with BuildingDTO"; //Ram
        public const string CompletedSaveBuildingAcquisitionDetails = "Completed - CompletedSaveBuildingAcquisitionDetails with BuildingDTO"; // Ram
        public const string StartedSaveFurnitureAcquisition = "Started - StartedSaveFurnitureAcquisition with FurnitureDTO(TblIdmIrFurnDTO)"; 
        public const string CompletedSaveFurnitureAcquisition = "Completed - CompletedSaveFurnitureAcquisition with FurnitureDTO(TblIdmIrFurnDTO)"; 

        public const string StartedSaveAllocationDetails = "Started - SaveAllocationDetails with TblIdmDhcgAllcDTO";
        public const string CompletedSaveAllocationDetails = "Completed- SaveAllocationDetails with TblIdmDhcgAllcDTO";

        public const string SubmitStarted = "Started- Update method for";
        public const string SubmitCompleted = "Completed- Update method for";
        public const string StartedSaveStatusofImplementationDetails = "Started - StartedSaveStatusofImplementationDetails with IdmDsbStatImpDTO";
        public const string StartedSaveRecommendedDisbursementDetails = "Started - StartedSaveRecommendedDisbursementDetails with IdmDsbdetsDTO"; //Ram
        public const string CompletedSaveRecommendedDisbursementDetails = "Completed - CompletedSaveRecommendedDisbursementDetails with IdmDsbdetsDTO"; // Ram        
        public const string StartedSaveNameOfUnitDetails = "Started - StartedSaveNameOfUnitDetails with IdmUnitDetailDTO";
        public const string CompletedSaveNameOfUnitDetails = "Completed - CompletedSaveNameOfUnitDetails with IdmUnitDetailDTO";
        public const string UpdateSaveNameOfUnitDetails = "Completed - UpdatedSaveNameOfUnitDetails with IdmUnitDetailDTO";
        public const string CompletedSaveProductDetails = "Started - CompletedSaveProductDetails with IdmUnitProductsDTO";
        public const string StartedDeleteProductDetails = " StartedDeleteProductDetails  with IdmUnitProductsDTO";
        public const string CompletedDeleteProductDetails = "CompletedDeleteProductDetails  with IdmUnitProductsDTO";
        public const string StartedUpdateProductDetails = "StartedUpdateProductDetails with IdmUnitProductsDTO";
        public const string CompletedUpdateProductDetails = "CompletedUpdateProductDetails with IdmUnitProductsDTO";
        public const string StartedCreateProductDetails = "StartedCreateProductDetails with IdmUnitProductsDTO";
        public const string CompletedCreateProductDetails = "CompletedCreateProductDetails with IdmUnitProductsDTO";
        public const string StartedUpdateAddressDetails = "StartedUpdateAddressDetails with IdmUnitAddressDTO";
        public const string CompletedUpdateAddressDetails = "CompletedUpdateAddressDetails with IdmUnitAddressDTO";
        public const string StartedSaveChangeBankDetails = "Started - StartedSaveChangeBankDetails with IdmChangeBankDetailsDTO";
        public const string CompletedSaveChangeBankDetails = "Completed - CompletedSaveChangeBankDetails with IdmChangeBankDetailsDTO";
        public const string StartedDeleteChangeBankDetails = "StartedDeleteChangeBankDetails with IdmChangeBankDetailsDTO";
        public const string CompletedDeleteChangeBankDetails = "CompletedDeleteChangeBankDetails with IdmChangeBankDetailsDTO";
        public const string StartedUpdateChangeBankDetails = "StartedUpdateChangeBankDetails with IdmChangeBankDetailsDTO";
        public const string CompletedUpdateChangeBankDetails = "CompletedUpdateChangeBankDetails with IdmChangeBankDetailsDTO";
        public const string StartedCreateChangeBankDetails = "StartedCreateChangeBankDetails with IdmChangeBankDetailsDTO";
        public const string CompletedCreateChangeBankDetails = "CompletedCreateChangeBankDetails with IdmChangeBankDetailsDTO";
        public const string StartedDeletePromoterProfile = "StartedDeletePromoterProfile with IdmPromoterDTO";
        public const string CompletedDeletePromoterProfile = "CompletedDeletePromoterProfile with IdmPromoterDTO";
        public const string StartedUpdatePromoterProfile = "StartedUpdatePromoterProfile with IdmPromoterDTO";
        public const string CompletedUpdatePromoterProfile = "CompletedUpdatePromoterProfile with IdmPromoterDTO";
        public const string StartedCreatePromoterProfile = "StartedCreatePromoterProfile with IdmPromoterDTO";
        public const string CompletedCreatePromoterProfile = "CompletedCreatePromoterProfile with IdmPromoterDTO";
        public const string StartedDeletePromoterAddressDetails = "StartedDeletePromoterAddressDetails with IdmPromAddressDTO";
        public const string CompletedDeletePromoterAddressDetails = "CompletedDeletePromoterAddressDetails with IdmPromAddressDTO";
        public const string StartedUpdatePromoterAddressDetails = "StartedUpdatePromoterAddressDetails with IdmPromAddressDTO";
        public const string CompletedUpdatePromoterAddressDetails = "CompletedUpdatePromoterAddressDetails with IdmPromAddressDTO";
        public const string StartedCreatePromoterAddressDetails = "StartedCreatePromoterAddressDetails with IdmPromAddressDTO";
        public const string CompletedCreatePromoterAddressDetails = "CompletedCreatePromoterAddressDetails with IdmPromAddressDTO";
        public const string StartedCreateLandAssetsDetails = "StartedCreateLandAssetsDetails with TblIdmProjLandDTO";
        public const string CompletedCreateLandAssetsDetails = "CompletedCreateLandAssetsDetails with TblIdmProjLandDTO";
        public const string StartedDeletePromoterBank = "StartedDeletePromoterBank with IdmPromoterBankDetailsDTO";
        public const string CompletedDeletePromoterBank = "CompletedDeletePromoterBank with IdmPromoterBankDetailsDTO";
        public const string StartedUpdatePromoterBank = "StartedUpdatePromoterBank with IdmPromoterBankDetailsDTO";
        public const string CompletedUpdatePromoterBank = "CompletedUpdatePromoterBank with IdmPromoterBankDetailsDTO";
        public const string StartedCreatePromoterBank = "StartedCreatePromoterBank with IdmPromoterBankDetailsDTO";
        public const string CompletedCreatePromoterBank = "CompletedCreatePromoterBank with IdmPromoterBankDetailsDTO";
        public const string StartedDeleteAssetDetails = "StartedDeleteAssetDetails with IdmPromAssetDetDTO";
        public const string CompletedDeleteAssetDetails = "CompletedDeleteAssetDetails with IdmPromAssetDetDTO";
        public const string StartedUpdateAssetDetails = "StartedUpdateAssetDetails with IdmPromAssetDetDTO";
        public const string CompletedUpdateAssetDetails = "CompletedUpdateAssetDetails with IdmPromAssetDetDTO"; 
        public const string StartedSaveAssetNetworthDetails = "StartedSaveAssetNetworthDetails with IdmPromAssetDetDTO";
        public const string CompletedSaveAssetNetworthDetails = "CompletedSaveAssetNetworthDetails with IdmPromAssetDetDTO";
        public const string StartedSaveLiabilityNetworthDetails = "StartedSaveLiabilityNetworthDetails with TblPromoterNetWortDTO";
        public const string CompletedSaveLiabilityNetworthDetails = "CompletedSaveLiabilityNetworthDetails with TblPromoterNetWortDTO";
        public const string StartedSaveNetworthDetails = "StartedSaveNetworthDetails with TblPromoterNetWortDTO";
        public const string CompletedSaveNetworthDetails = "CompletedSaveNetworthDetails with TblPromoterNetWortDTO";
        public const string StartedCreateAssetDetails = "StartedCreateAssetDetails with IdmPromAssetDetDTO";
        public const string CompletedCreateAssetDetails = "CompletedCreateAssetDetails with IdmPromAssetDetDTO";
        public const string StartedDeletePromoterLiability = "StartedDeletePromoterLiability with TblPromoterLiabDetDTO";
        public const string CompletedDeletePromoterLiability = "CompletedDeletePromoterLiability with TblPromoterLiabDetDTO";
        public const string CompletedUpdatePromoterLiability = "CompletedUpdatePromoterLiability with TblPromoterLiabDetDTO";
        public const string StartedUpdatePromoterLiability = "StartedUpdatePromoterLiability with TblPromoterLiabDetDTO";
        public const string StartedCreatePromoterLiability = "StartedCreatePromoterLiability with TblPromoterLiabDetDTO";
        public const string CompletedCreatePromoterLiability = "CompletedCreatePromoterLiability with TblPromoterLiabDetDTO";
        public const string CompletedLandAcqDelete = "CompletedLandAcqDelete";
        public const string CompletedLandAcqUpdate = "CompletedLandAcqUpdate";
        public const string CompletedMachineryAcqDelete = "CompletedMachineryAcqDelete";
        public const string CompletedMachineryAcqUpdate = "CompletedMachineryAcqUpdate";
        public const string CompletedBuildingAcqDelete = "CompletedBuildingAcqDelete";
        public const string CompletedBuildingAcqUpdate = "CompletedBuildingAcqUpdate";
        public const string CompletedFurnitureAcqDelete = "CompletedFurnitureAcqDelete";
        public const string CompletedFurnitureAcqUpdate = "CompletedFurnitureAcqUpdate";

        public const string StartedLoanAllocationDelete = "Started - DeleteLoanAllocationDetailsy with LoanAllocationDTO";
        public const string CompletedLoanAllocationDelete = "Completed - DeleteLoanAllocationDetails with LoanAllocationDTO";
        public const string StartedLoanAllocationCreate = "Started - CreateLoanAllocationDetails with LoanAllocationDTO";
        public const string CompletedLoanAllocationCreate = "Completed - CreateLoanAllocationDetails with LoanAllocationDTO";
        public const string StartedLoanAllocationUpdate = "Started - UpdateLoanAllocationDetails with LoanAllocationDTO";
        public const string CompletedLoanAllocationUpdate = "Completed - UpdateLoanAllocationDetails with LoanAllocationDTO";

        public const string StartedInspectionDetailsDelete = "Started - DeleteInspectionDetails with IdmDspInspDTO";
        public const string CompletedInspectionDetailsDelete = "Completed - DeleteInspectionDetails with IdmDspInspDTO";
        public const string StartedInspectionDetailsCreate = "Started - CreateInspectionDetails with IdmDspInspDTO";
        public const string CompletedInspectionDetailsCreate = "Completed - CreateInspectionDetails with IdmDspInspDTO";
        public const string StartedInspectionDetailsUpdate = "Started - UpdateInspectionDetails with IdmDspInspDTO";
        public const string CompletedInspectionDetailsUpdate = "Completed - UpdateInspectionDetails with IdmDspInspDTO";

        public const string StartedImportMachineryDelete = "Started- DeleteImportMachineryDetails with IdmDchgImportMachineryDTO";
        public const string CompletedImportMachineryDelete = "Completed - DeleteImportMachineryDetails with IdmDchgImportMachineryDTO";
        public const string StartedImportMachineryCreate = "Started - CreateImportMachineryDetails with IdmDchgImportMachineryDTO";
        public const string CompletedImportMachineryCreate = "Completed - CreateImportMachineryDetails with IdmDchgImportMachineryDTO";
        public const string StartedImportMachineryUpdate = "Started - UpdateImportMachineryDetails with IdmDchgImportMachineryDTO";
        public const string CompletedImportMachineryUpdate = "Completed - UpdateImportMachineryDetails with IdmDchgImportMachineryDTO";

        public const string StartedStatusofImplementationCreate = "Started - CreateStatusofImplementation with IdmDsbStatImpDTO";
        public const string CompletedStatusofImplementationCreate = "Completed - CreateStatusofImplementation with IdmDsbStatImpDTO";
        public const string StartedStatusofImplementationUpdate = "Started - UpdateStatusofImplementation with IdmDsbStatImpDTO";
        public const string CompletedStatusofImplementationUpdate = "Completed - UpdateStatusofImplementation with IdmDsbStatImpDTO";
        public const string StartedStatusofImplementationDelete = "Started - DeleteStatusofImplementation with IdmDsbStatImpDTO";
        public const string CompletedStatusofImplementationDelete  = "Completed - DeleteStatusofImplementation with IdmDsbStatImpDTO";

        public const string StartedProjectCostDelete = "Started - DeleteProjectCostDetails with IdmDchgProjectCostDTO";
        public const string CompletedProjectCostDelete = "Completed - DeleteProjectCostDetails with IdmDchgProjectCostDTO";
        public const string StartedProjectCostCreate = "Started - CreateProjectCostDetails with IdmDchgProjectCostDTO";
        public const string CompletedProjectCostCreate = "Completed - CreateProjectCostDetails with IdmDchgProjectCostDTO";
        public const string StartedProjectCostUpdate = "Started - UpdateIProjectCostDetails with IdmDchgProjectCostDTO";
        public const string CompletedProjectCostUpdate = "Completed - UpdateIProjectCostDetails with IdmDchgProjectCostDTO";

        public const string StartedProposalDelete = "Started - DeleteProposalDetail with IdmReleDetlsDTO";
        public const string CompletedProposalDelete = "Completed - DeleteProposalDetail withIdmReleDetlsDTO";
        public const string StartedProposalCreate = "Started - CreateProposalDetail with IdmReleDetlsDTO";
        public const string CompletedProposalCreate = "Completed - CreateProposalDetail with IdmReleDetlsDTO";
        public const string StartedProposalUpdate = "Started - UpdateProposalDetail with IdmReleDetlsDTO";
        public const string CompletedProposalUpdate = "Completed - UpdateProposalDetail with IdmReleDetlsDTO";



        public const string StartedLetterOfCreditDelete = "Started - DeleteLetterOfCreditDetails with IdmDsbLetterOfCreditDTO";
        public const string StartedLetterOfCreditCreate = "Started - CreateLetterOfCreditDetails with IdmDsbLetterOfCreditDTO";
        public const string StartedLetterOfCreditUpdate = "Started - UpdateLetterOfCreditDetails with IdmDsbLetterOfCreditDTO";
        public const string CompletedLetterOfCreditDelete = "Completed - DeleteLetterOfCreditDetails with IdmDsbLetterOfCreditDTO";
        public const string CompletedLetterOfCreditCreate = "Completed - CreateLetterOfCreditDetails with IdmDsbLetterOfCreditDTO";
        public const string CompletedLetterOfCreditUpdate = "Completed - UpdateLetterOfCreditDetails with IdmDsbLetterOfCreditDTO";

        public const string StartedMeansOfFinanceDelete = "Started - DeleteMeansOfFinanceDetails with IdmDchgMeansOfFinanceDTO";
        public const string StartedMeansOfFinanceCreate = "Started - CreateMeansOfFinanceDetails with IdmDchgMeansOfFinanceDTO";
        public const string StartedMeansOfFinanceUpdate = "Started - UpdateMeansOfFinanceDetails with IdmDchgMeansOfFinanceDTO";
        public const string CompletedMeansOfFinanceDelete = "Completed - DeleteMeansOfFinanceDetails with IdmDchgMeansOfFinanceDTO";
        public const string CompletedMeansOfFinanceCreate = "Completed - CreateMeansOfFinanceDetails with IdmDchgMeansOfFinanceDTO";
        public const string CompletedMeansOfFinanceUpdate = "Completed - UpdateMeansOfFinanceDetails with IdmDchgMeansOfFinanceDTO";

        public const string StartedLandInspectionDelete = "Started - DeleteLandInspectionDetails with IdmDchgLandDetDTO";
        public const string CompletedLandInspectionDelete = "Completed - DeleteLandInspectionDetails with IdmDchgLandDetDTO";
        public const string StartedLandInspectionCreate = "Started - CreateLandInspectionDetails with IdmDchgLandDetDTO";
        public const string CompletedLandInspectionCreate = "Completed - CreateLandInspectionDetails with IdmDchgLandDetDTO";
        public const string StartedLandInspectionUpdate = "Started - UpdateLandInspectionDetails with IdmDchgLandDetDTO";
        public const string CompletedLandInspectionUpdate = "Completed - UpdateLandInspectionDetails with IdmDchgLandDetDTO";

        public const string StartedBuildMatSiteDelete = "Started - DeleteBuildMatSiteDetails with IdmBuildingMaterialSiteInspectionDTO";
        public const string CompletedBuildMatSiteDelete = "Completed - DeleteBuildMatSiteDetails with IdmBuildingMaterialSiteInspectionDTO";
        public const string StartedBuildMatSiteCreate = "Started - CreateBuildMatSiteDetails with IdmBuildingMaterialSiteInspectionDTO";
        public const string CompletedBuildMatSiteCreate = "Completed - CreateBuildMatSiteDetails with IdmBuildingMaterialSiteInspectionDTO";
        public const string StartedBuildMatSiteUpdate = "Started - UpdateBuildMatSiteDetails with IdmBuildingMaterialSiteInspectionDTO";
        public const string CompletedBuildMatSiteUpdate = "Completed - UpdateBuildMatSiteDetails with IdmBuildingMaterialSiteInspectionDTO";

        public const string StartedFurnitureInspectionDelete = "Started - DeleteFurnitureInspectionDetails with IdmDChgFurnDTO";
        public const string CompletedFurnitureInspectionDelete = "Completed - DeleteFurnitureInspectionDetails with IdmDChgFurnDTO";
        public const string StartedFurnitureInspectionCreate = "Started- CreateFurnitureInspectionDetails with IdmDChgFurnDTO";
        public const string CompletedFurnitureInspectionCreate = "Completed - CreateFurnitureInspectionDetails with IdmDChgFurnDTO";
        public const string StartedFurnitureInspectionUpdate = "Started - UpdateFurnitureInspectionDetails with IdmDChgFurnDTO";
        public const string CompletedFurnitureInspectionUpdate = "Completed - UpdateFurnitureInspectionDetails with IdmDChgFurnDTO";
     
        public const string StartedBuildingInspectionDelete = "Started - DeleteBuildingInspectionDetails with IdmDchgBuildingDetDTO";
        public const string CompletedBuildingInspectionDelete = "Completed - DeleteBuildingInspectionDetails with IdmDchgBuildingDetDTO";
        public const string StartedBuildingInspectionCreate = "Started - CreateBuildingInspectionDetails with IdmDchgBuildingDetDTO";
        public const string CompletedBuildingInspectionCreate = "Completed - CreateBuildingInspectionDetails with IdmDchgBuildingDetDTO";
        public const string StartedBuildingInspectionUpdate = "Started - UpdateBuildingInspectionDetails with IdmDchgBuildingDetDTO";
        public const string CompletedBuildingInspectionUpdate = "Completed - UpdateBuildingInspectionDetails with IdmDchgBuildingDetDTO";

        public const string StartedIndigenousMachineryInspectionDelete = "Started - DeleteIndigenousMachineryInspectionDetails with IdmDchgIndigenousInspectionDTO";
        public const string CompletedIndigenousMachineryInspectionDelete = "Completed - DeleteIndigenousMachineryInspectionDetails with IdmDchgIndigenousInspectionDTO";
        public const string StartedIndigenousMachineryInspectionCreate = "Started - CreateIndigenousMachineryInspectionDetails with IdmDchgIndigenousInspectionDTO";
        public const string CompletedIndigenousMachineryInspectionCreate = "Completed - CreateIndigenousMachineryInspectionDetails with IdmDchgIndigenousInspectionDTO";
        public const string StartedIndigenousMachineryInspectionUpdate = "Started - UpdateIndigenousMachineryInspectionDetails with IdmDchgIndigenousInspectionDTO";
        public const string CompletedIndigenousMachineryInspectionUpdate = "Completed - UpdateIndigenousMachineryInspectionDetails with IdmDchgIndigenousInspectionDTO";

        public const string CompletedAdditionalConditionalDelete = "Completed - DeleteAdditionalConditionalDetails with AdditionConditionDetailsDTO";
        public const string CompletedAdditionalConditionalCreate = "Completed - CreateAdditionalConditionalDetails with AdditionConditionDetailsDTO";
        public const string CompletedAdditionalConditionalUpdate = "Completed - UpdateAdditionalConditionalDetails with AdditionConditionDetailsDTO";

        public const string CompletedDisbursementConditionalDelete = "Completed - DeleteDisbursementConditionalDetails with LDConditionDetailsDTO";
        public const string CompletedDisbursementConditionalCreate = "Completed - CreateDisbursementConditionalDetails with LDConditionDetailsDTO";
        public const string CompletedDisbursementConditionalUpdate = "Completed - UpdateDisbursementConditionalDetails with LDConditionDetailsDTO";

        public const string CompletedForm8AndForm13Delete = "Completed - DeleteForm8AndForm13Details with Form8AndForm13DTO";
        public const string CompletedForm8AndForm13Create = "Completed - CreateForm8AndForm13Details with Form8AndForm13DTO";
        public const string CompletedForm8AndForm13Update = "Completed - UpdateForm8AndForm13Details with Form8AndForm13DTO";

        public const string CompletedAuditClearenceDelete = "Completed - DeleteAuditClearenceDetails with IdmAuditDetailsDTO";
        public const string CompletedAuditClearenceCreate = "Completed - CreateAuditClearenceDetails with IdmAuditDetailsDTO";
        public const string CompletedAuditClearenceUpdate = "Completed - UpdateAuditClearenceDetails with IdmAuditDetailsDTO";

        public const string CompletedOtherDebitDetailsUpdate = "Completed - UpdateOtherDebitDetails with IdmOthdebitsDetailsDTO";
        public const string CompletedOtherDebitDetailsCreate = "Completed - CreateOtherDebitDetails with IdmOthdebitsDetailsDTO";
        public const string CompletedOtherDebitDetailsSubmit = "Completed - SubmitOtherDebitDetails with IdmOthdebitsDetailsDTO";
        public const string StartedSaveOtherDebitDetails = "Started - SaveOtherDebitDetails with IdmOthdebitsDetailsDTO";
        public const string CompletedOtherDebitDetailsDelete = "Completed - DeleteOtherDebitDetails with IdmOthdebitsDetailsDTO";
        public const string CompletedSaveOtherDebitDetails = "Completed- SaveOtherDebitDetails with IdmOthdebitsDetailsDTO";
        #endregion
        #region UnitDetails
        public const string GetAllLoanNumber = "Started calling GetAllLoanNumber()";
        public const string CompletedGetAllLoanNumber = "Completed calling GetAllLoanNumber()";
        public const string GetAllUnitDetails = "Started calling GetUnitDetails()";
        public const string CompletedGetAllUnitDetails = "Completed calling GetUnitDetails()";
        public const string GetAllDistrictNames = "Started calling GetAllDistrictNames()";
        public const string GetAllTalukNames = "Started calling GetAllTalukDetails()";
        public const string CompletedAllTalukNames = "Completed calling GetAllTalukDetails()";
        public const string GetAllHobliNames = "Started calling GetAllHobliDetails()";
        public const string CompletedAllHobliNames = "Completed calling GetAllHobliDetails()";
        public const string GetAllVillageNames = "Started calling GetAllVillageDetails()";
        public const string CompletedAllVillageNames = "Completed calling GetAllVillageDetails()";
        public const string GetAllAddressDetails = "Started calling GetAllAddressDetails()";
        public const string CompletedGetAllAddressDetails = "Completed calling GetAllAddressDetails()";
        public const string GetAllMasterPinCodeDetails = "Started calling GetAllMasterPinCodeDetails()";
        public const string CompletedGetAllMasterPinCodeDetails = "Completed calling GetAllMasterPinCodeDetails()";
        public const string GetAllPinCodeDistrictDetails = "Started calling GetAllPinCodeDistrictDetails()";
        public const string CompletedGetAllPinCodeDistrictDetails = "Completed calling GetAllPinCodeDistrictDetails()";
        public const string GetAllPinCodeStateDetails = "Started calling GetAllPinCodeStateDetails()";
        public const string CompletedGetAllPinCodeStateDetails = "Completed calling GetAllPinCodeStateDetails()";
        public const string GetAllMasterPromoterProfileDetails = "Started calling  GetAllMasterPromoterProfile()";
        public const string CompletedGetAllMasterPromoterProfileDetails = "Completed calling  GetAllMasterPromoterProfile()";
        public const string GetAllPromoterProfileDetails = "Started calling GetAllPromoterProfileDetails";
        public const string CompletedGetAllPromoterProfileDetails = "Completed calling GetAllPromoterProfileDetails";
        public const string GetAllLandAssetDetails = "Started calling GetAllLandAssetDetails";
        public const string CompletedGetAllLandAssetDetails = "Completed calling CompletedGetAllLandAssetDetails";
        public const string GetPositionDesignationAsync = "Started calling  GetPositionDesignationAsync()";
        public const string GetAllPromotorClass = "Started calling GetAllPromotorClass()";
        public const string GetDomicileStatusAsync = "Started calling GetDomicileStatusAsync()";
        public const string GetAllPromotorNames = "Started calling GetAllPromotorNames()";
        public const string GetAllPromoAddressDetails = "Started calling GetAllPromoAddressDetails()";
        public const string CompletedGetAllPromoAddressDetails = "Completed calling GetAllPromoAddressDetails()";
        public const string GetAllPromoterAssetDetails = "Started calling GetAllPromoterAssetDetails()";
        public const string CompletedGetAllPromoterAssetDetails = "Completed calling GetAllPromoterAssetDetails()";
        public const string GetAllIfscbankDetails = "Started calling GetAllIfscbankDetails()";
        public const string CompletedGetAllIfscbankDetails = "Completed calling GetAllIfscbankDetails()";
        public const string GetAllPromoterBankInfo = "Started calling GetAllPromoterBankInfo()";
        public const string CompletedGetAllPromoterBankInfo = "Completed calling GetAllPromoterBankInfo()";
        public const string GetAccountTypeAsync = "Started calling GetAccountTypeAsync()";
        public const string GetAllPromoterBankList = "Started calling GetAllPromoterBankList()";
        public const string GetAllProductDetails = "Started calling GetAllProductDetails()";
        public const string CompletedGetAllProductDetails = "Completed calling GetAllProductDetails()";
        public const string GetAllProductList = "Started calling GetAllProductList()";
        public const string CompletedGetAllProductList = "Completed calling GetAllProductList()";
        public const string GetallAssetTypeMaster = "Started calling GetallAssetTypeMaster()";
        public const string CompletedGetallAssetTypeMaster = "Completed calling GetallAssetTypeMaster()";
        public const string GetallAssetCategoryMaster = "Started calling GetallAssetCategoryMaster()";
        public const string GetAllAssetTypeDetails = "Started calling GetAllAssetTypeDetails()";
        public const string GetAllAssetCategaryDetails = "Started calling GetAllAssetCategaryDetails()";
        public const string CompletedGetallAssetCategoryMaster = "Completed calling GetallAssetCategoryMaster()";
        public const string CompletedGetAllAssetTypeDetails = " Completed GetAllAssetTypeDetails()";
        public const string CompletedGetAllAssetCategaryDetails = " Completed GetAllAssetCategaryDetails()";
        public const string GetallIndustrydetailsMaster = "Started calling GetallIndustrydetailsMaster()";
        public const string CompletedGetallIndustrydetailsMaster = "Completed calling GetallIndustrydetailsMaster()";
        public const string GetAllChangebankDetails = "Started calling GetAllChangebankDetails()";
        public const string CompletedGetAllChangebankDetails = "Completed calling GetAllChangebankDetails()";
        public const string GetAllPromoLiabilityInfo = "Started calling GetAllPromoLiabilityInfo()";
        public const string CompletedGetAllPromoLiabilityInfo = "Completed calling GetAllPromoLiabilityInfo()";
        public const string GetAllPromoterNetWorth = "Started calling GetAllPromoterNetWorth()";
        public const string CompletedGetAllPromoterNetWorth = "Completed calling GetAllPromoterNetWorth()";
        public const string UnitDetailsviewAccount = "Started ViewAccount Method for UnitDetails Module";



        public const string StartedGetAllPincodeDetails = "Started Get All Pincode Details based on District: Error message is:";
        public const string CompletedGetAllPincodeDetails = "Completed Get All Pincode Details based on District: Error message is:";

        public const string StartedGetAllTalukDetails = "Started Get All Taluk Details based on District: Error message is:";
        public const string CompletedGetAllTalukDetails = "Completed Get All Taluk Details based on District: Error message is:";

        public const string StartedGetAllDistrictDetails = "Started Get All District Details based on State: Error message is:";
        public const string CompletedGetAllDistrictDetails = "Completed Get All District Details based on State: Error message is:";

        public const string StartedGetAllHobliDetails = "Started Get All Hobli Details based on Taluk: Error message is:";
        public const string CompletedGetAllHobliDetails = "Completed Get All Hobli Details based on Taluk: Error message is:";

        public const string StartedGetAllVillageDetails = "Started Get All Village Details based on Hobli: Error message is:";
        public const string CompletedGetAllVillageDetails = "Completed Get All Village Details based on Hobli: Error message is:";



        public const string StartedGetAllAssetType = "Started Get All Asset Type based on Asset Categary:Error message is:";
        public const string CompletedGetAllAssetType = "Completed Get All Asset Type based on Asset Categary:Error message is:";

        #endregion
        #region Inspection of Unit
        public const string InspectionDetailsviewAccount = "Started ViewAccount Method for Inspection Details Module";
        public const string GetAllInspectionDetailsList = "Started calling GetAllInspectionDetailsList()";
        public const string CompletedGetAllInspectionDetailsList = "Completed calling GetAllInspectionDetailsList()";
        public const string GetAllLetterOfCreditDetailsList = "Started calling GetAllLetterOfCreditDetailsList()";
        public const string CompletedGetAllLetterOfCreditDetailsList = "Completed calling GetAllLetterOfCreditDetailsList()";
        public const string GetAllMeansOfFinanceList = "Started calling GetAllMeansOfFinanceList()";
        public const string GetFinanceTypeAsync = "Started calling GetFinanceTypeAsync()";
        public const string CompletedGetFinanceTypeAsync = "Completed calling GetFinanceTypeAsync()";
        public const string CompletedGetAllMeansOfFinanceList = "Completed calling GetAllMeansOfFinanceList()";
        public const string GetAllProjectCostDetailsList = "Started calling GetAllProjectCostDetailsList()";
        public const string CompletedGetAllProjectCostDetailsList = "Completed calling GetAllProjectCostDetailsList()";
        public const string GetAllProjectCostComponentsDetails = "Started calling GetAllProjectCostComponentsDetails()";
        public const string GetAllLandType = "Started calling GetAllLandType()";
        public const string GetAllLandInspectionList = "Started calling GetAllLandInspectionList()";
        public const string CompletedGetAllLandInspectionList = "Completed calling GetAllLandInspectionList()";
        public const string GetAllBuildingMaterialInspectionList = "Started calling GetAllBuildingMaterialInspectionList()";
        public const string CompletedGetAllBuildingMaterialInspectionList = "Completed calling GetAllBuildingMaterialInspectionList()";
        public const string GetAllFurnitureInspectionList = "Started calling GetAllFurnitureInspectionList()";
        public const string CompletedGetAllFurnitureInspectionList = "Completed calling GetAllFurnitureInspectionList()";
        public const string GetAllBuildingnspectionList = "Started calling GetAllBuildingnspectionList()";
        public const string CompletedGetAllBuildingnspectionList = "Completed calling GetAllBuildingnspectionList()";
        public const string GetAllImportMachineryList = "Started calling GetAllImportMachineryList()";
        public const string CompletedGetAllImportMachineryList = " Completed calling GetAllImportMachineryList()";
        public const string GetAllIndigenousMachineryInspectionList = "Started calling GetAllIndigenousMachineryInspectionList()";
        public const string GetAllMachineryStatusList = "Started calling GetAllMachineryStatusList()";
        public const string GetAllProcureList = "Started calling GetAllProcureList()";
        public const string GetAllCurrencyList  = "Started calling GetAllCurrencyList()";
        public const string GetAllStatusofImplementationList = "Started calling GetAllStatusofImplementationList()";
        public const string CompletedAllStatusofImplementationList = "Competed calling GetAllStatusofImplementationList()";
        public const string CompletedGetAllIndigenousMachineryInspectionList = "Completed calling GetAllIndigenousMachineryInspectionList()";
        public const string CompletedGetAllMachineryStatusList = "Completed calling GetAllMachineryStatusList()";
        public const string CompletedGetAllProcureList = "Completed calling GetAllProcureList()";
        public const string CompletedGetAllCurrencyList  = "Completed calling GetAllCurrencyList()";

        #endregion
        #region Loan Allocation
        public const string LoanAllocationviewAccount = "Started ViewAccount Method for LoanAllocation Module";
        public const string GetAllocationCodes = "Started calling GetAllocationCodes()";
        public const string GetAllLoanAllocationList = "Started calling GetAllLoanAllocationList()";
        public const string CompletedGetAllLoanAllocationList = "Completed calling GetAllLoanAllocationList()";
        #endregion
    }


    public static class Error
    {
        #region Error Message 
        public const string ViewRecordErrorMsg = "Error occured while loading ViewRecord page. Error message is: ";
        public const string UpdateErrorMsg = "Error occured while loading Update page. Error message is: ";
        public const string UpdateErrorMsgPost = "Error occured while loading Update HtppPost page. Error message is: ";
        public const string RegisterErrorMsg = "Error occured while loading Register page. Error message is: ";
        public const string RegisterErrorMsgPost = "Error occured while loading Register HtppPost page. Error message is: ";
        public const string CreateErrorMsg = "Error occured while loading Create page. Error message is: ";
        public const string CreateErrorMsgPost = "Error occured while loading Create HtppPost page. Error message is:";
        public const string DeleteErrorMsgPost = "Error occured while loading Delete HtppPost page. Error message is: ";
        public const string Stack = "The stack trace is: ";
        public const string ErrorPath = "~/Views/Shared/Error.cshtml";
        public const string ViewBagError = "Unknown error occurred! Please try again after sometime.";
        public const string SavePrimaryCollateralError = "Error occured! SavePrimaryCollateralDetails HttpPost page . Error message is: ";
        public const string ViewAccount = "Error occured! View Account HttpPost page . Error message is: ";

        public const string SaveCersaiError = "Error occured! SaveCersaiRegDetailsForLD HttpPost page . Error message is: ";
        public const string SaveGuarantorDeedError = "Error occured! SaveGuarantorDeedDetails HttpPost page . Error message is: ";
        public const string SaveHyopthecationError = "Error occured! SaveHyopthecationDetails HttpPost page . Error message is: ";
        public const string SaveConditionError = "Error occured! SaveConditionDetails HttpPost page . Error message is: ";
        public const string SaveAuditClearanceError = "Error occured! SaveAuditClearanceDetails HttpPost page . Error message is: ";
        public const string SaveSecurityChargeError = "Error occured! SaveConditionDetails HttpPost page . Error message is: ";
        public const string FileStatus = "Completed - status = Fail";
        public const string UploadOnlyPdfError = "Completed - Please upload only PDF file. status = Fail";
        public const string UploadOnlyImgError = "Completed - Please upload only JPEG file. status = Fail";

        public const string UploadFileLoadingError = "Error occured while loading UploadFle  page. Error message is: ";
        public const string UploadImageLoadingError = "Error occured while loading UploadImage  page. Error message is: ";

        public const string ViewUploadFileLoadingError = "Error occured while loading ViewUploadFile  page. Error message is: ";
        public const string DeleteUploadFileLoadingError = "Error occured while loading DeleteUploadFile  page. Error message is: ";
        public const string SavePromoterAddressError = "Error occured! SavePromoterAddressError HttpPost page . Error message is: ";
        public const string SaveChangeLocationError = "Error occured! SaveChangeLocationError  HttpPost page . Error message is: ";
        public const string SaveAllUnitDetailsError = "Error occured! SaveAllUnitDetailsnError  HttpPost page . Error message is: ";
        public const string MaximumFiles = "Max 5 Files can be Uploaded.";
        public const string MaximumFilesDisbursment = "Max 1 Files can be Uploaded.";

        public const string MaximumImages = "Max 1 Image can be Uploaded.";
        public const string UploadOnlyPdf = "Please upload only PDF File.";
        public const string UploadOnlyImg = "Please upload only JPEG File.";

        public const string SavePromoterProfileError = "Error occured! SavePromoterProfileError HttpPost page . Error message is: ";
        public const string SavePromoterBankDetailsError = "Error occured! SavePromoterBankDetailsError HttpPost page . Error message is: ";
        public const string SavePromoterLiabilityInformatioError = "Error occured! SavePromoterLiabilityInformatioError HttpPost page . Error message is: ";
        public const string SaveLandInspectionError = "Error occured! SaveLandInspection Details HttpPost page . Error message is: ";
        public const string SaveBuildMatSiteError = "Error occured! SaveBuilding Material Details HttpPost page . Error message is: ";
        public const string SaveProjectCostError = "Error occured! Save Project Cost Details HttpPost page . Error message is: ";
        public const string SaveMachineryAcquisitionError = "Error occured! SaveMachineryAcquisitionError HttpPost page . Error message is: ";
        public const string SaveLandAcquisitionError = "Error occured! SaveLandAcquisitionError HttpPost page . Error message is: ";
        public const string SaveBuildingAcquisitionError = "Error occured! SaveBuildingAcquisitionError HttpPost page . Error message is: ";

        public const string SaveFurnitureAcquisitionError = "Error occured! Save FurnitureAcquisition HttpPost page . Error message is: ";

        public const string GetAllRecomDisbursementDetails = "Error Occured! Get All RecomDisbursement Details:  Error message is:";
        public const string GetRecomDisbursementReleaseDetails = "Error Occured! GetRecom Disbursement Release Details:  Error message is:";
        public const string GetAllocationCodeDetails = "Error Occured! Get Allocation Code Details:  Error message is:";


        public const string getallPrimary = "Error Occured! Get All Primary Coletral Details:  Error message is:";
        public const string updatePrimary = "Error Occured! Update PrimaryCollateral Details:  Error message is:";
        public const string deletePrimary = "Error Occured! Delete Primary Collateral Detail:  Error message is:";
        public const string getallColletral = "Error Occured! Get All Coletral Details:  Error message is:";
        public const string updateColletral = "Error Occured! Update Collateral Details:  Error message is:";
        public const string CreateColletral = "Error Occured! Create Collateral Details:  Error message is:";
        public const string Createhypo= "Error Occured! Create Hypothecation Details:  Error message is:";
        public const string deleHypo= "Error Occured! Delete Hypothecation Detail:  Error message is:";
        public const string UpdateHypo= "Error Occured! Update Hypothecation Details:  Error message is:";
        public const string getAsset= "Error Occured! Get All AssetRef List:  Error message is:";
        public const string gethypo= "Error Occured! Get All Hypothecation List:  Error message is:";
        public const string deletesec= "Error Occured! Delete Security Charge:  Error message is:";
        public const string updateSec= "Error Occured! Updat eSecurity Charget:  Error message is:";
        public const string getSec= "Error Occured! Get All SecurityCharge List:  Error message is:";    
        public const string DeleCersai= "Error Occured! Delete LD CersaiReg Details:  Error message is:";       
        public const string getCeresai= "Error Occured! Get All CERSAI List:  Error message is:";
        public const string updateCersai= "Error Occured! Update LD CersaiReg Details:  Error message is:";
        public const string getGauarntor= "Error Occured! Get All GuarantorDeed List:  Error message is:";
        public const string deleteGuarantor= "Error Occured! Delete LD Guarantor Deed Detail:  Error message is:";
        public const string updateGuarantor= "Error Occured!Update LD GuarantorDeed Details:  Error message is:";
        public const string updateCondition= "Error Occured! Update LD Condition Details:  Error message is:";
        public const string CreateCondition= "Error Occured! Create LD Condition Details:  Error message is:";
        public const string deleteCondition= "Error Occured! Delete LD Condition Details:  Error message is:";
        public const string getCondition= "Error Occured! Get LD Condition  Details:  Error message is:";
        public const string getLoanAllocation = "Error Occured! Get All LoanAllocation List:  Error message is:";
        public const string getLoanAccount = "Error Occured! Get All LoanAccount List:  Error message is:";
        public const string GetAllDisbursementConditionList = "Error Occured! Get All Disbursement Condition List:  Error message is:";
        public const string GetAllForm8AndForm13List = "Error Occured!  Get All Form8AndForm13 List:  Error message is:";
        public const string GetAllAdditionalConditonlist = "Error Occured! Get All Additional Conditon list:  Error message is:";
        public const string GetAllFirstInvestmentClauseDetails = "Error Occured! Get All First Investment ClauseDetails:  Error message is:";
        public const string GetAllOtherRelaxation = "Error Occured! Get All Other Relaxation:  Error message is:";
        public const string UpdateRecomDisbursementDetail = "Error Occured! Update Recom Disbursement Detail:  Error message is:";
        public const string CreateProposalDetail = "Error Occured! Create Proposal Detail:  Error message is:";
        public const string GetAllProposalDetails = "Error Occured! Get All Proposal Details:  Error message is:";
        public const string GetAllBeneficiaryDetails = "Error Occured! Get All Beneficiary Details:  Error message is:";
        public const string UpdateProposalDetail = "Error Occured! Update Proposal Detail:  Error message is:";
        public const string UpdateBeneficiaryDetails = "Error Occured! Update Beneficiary Details:  Error message is:";
        public const string DeleteProposalDetail = "Error Occured! Delete Proposal Detail:  Error message is:";



        public const string updateLoanAllocation = "Error Occured! Update LoanAllocation Details:  Error message is:";
        public const string UpdateOtherRelaxation = "Error Occured! Update Update Other Relaxation:  Error message is:";
        public const string createLoanAllocation = "Error Occured! Create LoanAllocation Details:  Error message is:";
        public const string deleteLoanAllocation = "Error Occured! Delete LoanAllocation Details:  Error message is:";
        public const string SaveLoanAllocation = "Error occured! SaveLoanAllocation HttpPost page . Error message is: ";
        public const string getInspectionList = "Error Occured! Get All Inspection List:  Error message is:";
        public const string updateInspection = "Error Occured! Update Inspection Details:  Error message is:";
        public const string createInspection = "Error Occured! Create Inspection Details:  Error message is:";
        public const string deleteInspection = "Error Occured! Delete Inspection Details:  Error message is:";
        public const string SaveInspection = "Error occured! SaveInspection HttpPost page . Error message is: ";
        public const string getLetterOfCredit = "Error Occured! Get All LetterOfCredit Details:  Error message is:";
        public const string updateLetterOfCredit = "Error Occured! Update LetterOfCredit Details:  Error message is:";
        public const string createLetterOfCredit = "Error Occured! Create LetterOfCredit Details:  Error message is:";
        public const string deleteLetterOfCredit = "Error Occured! Delete LetterOfCredit Details:  Error message is:";
        public const string SaveLetterOfCredit = "Error occured! SaveLetterOfCredit HttpPost page . Error message is: ";
        public const string getMeansOfFinance = "Error Occured! Get All MeansOfFinance Details:  Error message is:";
        public const string getFinanceType = "Error Occured! Get All Finance Type Async :  Error message is:";

        public const string GetAllSidbiApprovalDetails = "Error Occured! Get All Sidbi Approval Details:  Error message is:";
        public const string updateMeansOfFinance = "Error Occured! Update MeansOfFinance Details:  Error message is:";
        public const string createMeansOfFinance = "Error Occured! Create MeansOfFinance Details:  Error message is:";
        public const string deleteMeansOfFinance = "Error Occured! Delete MeansOfFinance Details:  Error message is:";
        public const string SaveMeansOfFinance = "Error occured! SaveMeansOfFinance HttpPost page . Error message is: ";
        public const string getProjectCost = "Error Occured! Get All ProjectCost Details:  Error message is:";
        public const string updateProjectCost = "Error Occured! Update ProjectCost Details:  Error message is:";
        public const string createProjectCost = "Error Occured! Create ProjectCost Details:  Error message is:";
        public const string deleteProjectCost = "Error Occured! Delete ProjectCost Details:  Error message is:";
        public const string SaveProjectCost = "Error occured! SaveProjectCost HttpPost page . Error message is: ";
        public const string updateLandInspection = "Error Occured! Update LandInspection Details:  Error message is:";
        public const string createLandInspection = "Error Occured! Create LandInspection Details:  Error message is:";
        public const string CreateLandAssets = "Error Occured! Create LandAsset Details:  Error message is:";
        public const string deleteLandInspection = "Error Occured! Delete LandInspection Details:  Error message is:";
        public const string SaveLandInspection = "Error occured! SaveLandInspection HttpPost page . Error message is: ";
        public const string updateBuildingInspection = "Error Occured! Update BuildingInspection Details:  Error message is:";
        public const string createBuildingInspection = "Error Occured! Create BuildingInspection Details:  Error message is:";
        public const string deleteBuildingInspection = "Error Occured! Delete BuildingInspection Details:  Error message is:";
        public const string SaveBuildingInspection = "Error occured! SaveBuildingInspection HttpPost page . Error message is: ";
        public const string updateBuildingMaterialInspection = "Error Occured! Update BuildingMaterialInspection Details:  Error message is:";
        public const string createBuildingMaterialInspection = "Error Occured! Create BuildingMaterialInspection Details:  Error message is:";
        public const string deleteBuildingMaterialInspection = "Error Occured! Delete BuildingMaterialInspection Details:  Error message is:";
        public const string SaveBuildingMaterialInspection = "Error occured! SaveBuildingMaterialInspection HttpPost page . Error message is: ";
        public const string updateFurnitureInspection = "Error Occured! Update FurnitureInspection Details:  Error message is:";
        public const string createFurnitureInspection = "Error Occured! Create FurnitureInspection Details:  Error message is:";
        public const string deleteFurnitureInspection = "Error Occured! DeleIndigenousMachineryInspectionte FurnitureInspection Details:  Error message is:";
        public const string SaveFurnitureInspection = "Error occured! SaveFurnitureInspection HttpPost page . Error message is: ";
        public const string updateIndigenousMachineryInspection = "Error Occured! Update IndigenousMachineryInspection Details:  Error message is:";
        public const string createIndigenousMachineryInspection = "Error Occured! Create IndigenousMachineryInspection Details:  Error message is:";
        public const string deleteIndigenousMachineryInspection = "Error Occured! Delete IndigenousMachineryInspection Details:  Error message is:";
        public const string SaveIndigenousMachineryInspection = "Error occured! SaveIndigenousMachineryInspection HttpPost page . Error message is: ";
        public const string updateImportMachineryInspection = "Error Occured! Update ImportMachineryInspection Details:  Error message is:";
        public const string createStatusofImplementation = "Error Occured! Create StatusofImplementation  Details:  Error message is:";
        public const string updateStatusofImplementation = "Error Occured! Update StatusofImplementation Details:  Error message is:";
        public const string deleteStatusofImplementation = "Error Occured! Delete StatusofImplementation Details:  Error message is:";
        public const string createImportMachineryInspection = "Error Occured! Create ImportMachineryInspection Details:  Error message is:";
        public const string deleteImportMachineryInspection = "Error Occured! Delete ImportMachineryInspection Details:  Error message is:";
        public const string SaveImportMachineryInspection = "Error occured! SaveImportMachineryInspection HttpPost page . Error message is: ";
        public const string createWorkingCapitalInspection = "Error Occured! Create WorkingCapitalInspection Details:  Error message is:";
        public const string SaveWorkingCapitalInspection = "Error occured! SaveWorkingCapitalInspection HttpPost page . Error message is: ";
        public const string updateDisbursement = "Error Occured! Update Disbursement Details:  Error message is:";
        public const string createDisbursement = "Error Occured! Create Disbursement Details:  Error message is:";
        public const string deleteDisbursement = "Error Occured! Delete Disbursement Details:  Error message is:";
        public const string SaveDisbursement = "Error occured! SaveDisbursement HttpPost page . Error message is: ";
        public const string updatesidbiapproval = "Error Occured! Update sidbiapproval Details:  Error message is:";
        public const string Savesidbiapproval = "Error occured! Savesidbiapproval HttpPost page . Error message is: ";
        public const string updateForm8AndForm13 = "Error Occured! Update Form8AndForm13 Details:  Error message is:";
        public const string updateFirstinvestment = "Error Occured! Update Firstinvestment Details:  Error message is:";
        public const string createForm8AndForm13 = "Error Occured! Create Form8AndForm13 Details:  Error message is:";
        public const string deleteForm8AndForm13 = "Error Occured! Delete Form8AndForm13 Details:  Error message is:";
        public const string SaveForm8AndForm13 = "Error occured! SaveForm8AndForm13 HttpPost page . Error message is: ";
        public const string updateFirstInvestmentClause = "Error Occured! Update FirstInvestmentClause Details:  Error message is:";
        public const string SaveFirstInvestmentClause = "Error occured! FirstInvestmentClause HttpPost page . Error message is: ";
        public const string updateAdditionalCondtional = "Error Occured! Update AdditionalCondtional Details:  Error message is:";
        public const string createAdditionalCondtional = "Error Occured! Create AdditionalCondtional Details:  Error message is:";
        public const string deleteAdditionalCondtional = "Error Occured! Delete AdditionalCondtional Details:  Error message is:";
        public const string SaveAdditionalCondtional = "Error occured! SaveAdditionalCondtional HttpPost page . Error message is: ";
        public const string SaveOtherRelaxation = "Error occured! SaveOtherRelaxation HttpPost page . Error message is: ";

        public const string AllReceiptPaymentList = "Error Occured! Get All ReceiptPayment List:  Error message is:";
        public const string GetCodeTableList = "Error Occured! Get All CodeTable List:  Error message is:";
        public const string UpdateReceiptPaymentDetails = "Error Occured! Update ReceiptPayment Details:  Error message is:";
        public const string UpdateCreatePaymentDetails = "Error Occured! Update CreatePayment Details:  Error message is:";
        public const string DeleteReceiptPaymentDetails = "Error Occured! Delete ReceiptPayment Details:  Error message is:";
        public const string ApproveReceiptPaymentDetails = "Error Occured! Approve ReceiptPayment Details:  Error message is:";
        public const string RejectReceiptPaymentDetails = "Error Occured! Reject ReceiptPayment Details:  Error message is:";
        public const string GetAllReceiptList = "Error Occured! Get All Receipt List:  Error message is:";
        public const string GetAllPaymentList = "Error Occured! Get All Payment List:  Error message is:";
        public const string CreateReceiptPaymentDetails = "Error Occured! Create Receipt Payment Details:  Error message is:";
        public const string CreatePaymentDetails = "Error Occured! Create Payment Details:  Error message is:";
        public const string GetAllRecipetsForPayment = "Error Occured! Get All Recipets For Payment:  Error message is:";
        public const string getUnitName = "Error Occured! Get Unit Name Details:  Error message is:";
        public const string updateUnitName = "Error Occured! Update Unit Name Details:  Error message is:";
        public const string SaveProductDetailsError = "Error occured! SaveProductDetails HttpPost page . Error message is: ";
        public const string SaveChangeBankDetailsError = "Error occured! SaveChangeBankDetails HttpPost page . Error message is: ";
        public const string GetAddressDetails = "Error Occured! Get all Address:  Error message is:";
        public const string GetMasterPincodeDetails = "Error Occured! Get all Master Pincode:  Error message is:";
        public const string GetPincodeDistrictDetails = "Error Occured! Get all PincodeDistrict:  Error message is:";
        public const string UpdateAddressDetails = "Error Occured! Update Address Details:  Error message is:";
        public const string GetAllMasterPromoterProfileDetails = "Error Occured! Get Master Promoter Profile Details: Error message is:";
        public const string GetAllPromoterProfileDetails = "Error Occured! Get Promoter Profile Details: Error message is:";
        public const string CreatePromoterProfileDetails = "Error Occured! Create Promoter Profile Details:  Error message is:";
        public const string UpdatePromoterProfileDetails = "Error Occured! Update Promoter Profile Details:  Error message is:";
        public const string DeletePromoterProfileDetails = "Error Occured! Delete Promoter Profile Details:  Error message is:";
        public const string GetAllPromoterAddressDetails = "Error Occured! Get Promoter Address Details: Error message is:";
        public const string CreatePromoterAddressDetails = "Error Occured! Create Promoter Address Details:  Error message is:";
        public const string UpdatePromoterAddressDetails = "Error Occured! Update Promoter Address Details:  Error message is:";
        public const string DeletePromoterAddressDetails = "Error Occured! Delete Promoter Address Details:  Error message is:";
        public const string GetAllBankDetails = "Error Occured! Get Bank Details: Error message is:";
        public const string CreateBankDetails = "Error Occured! Create Bank Details:  Error message is:";
        public const string UpdateBankDetails = "Error Occured! Update Bank Details:  Error message is:";
        public const string DeleteBankDetails = "Error Occured! Delete Bank Details:  Error message is:";
        public const string GetAllPromoterAssetDetails = "Error Occured! Get All Promoter Bank Details: Error message is:";
        public const string GetAllAssetTypeDetails = "Error Occured! Get All Asset Type Details: Error message is:";
        public const string GetAllAssetCategoryDetails = "Error Occured! Get All Asset Category Details: Error message is:";
        public const string CreatePromoterAssetDetails = "Error Occured! Create Promoter Asset Details:  Error message is:";
        public const string UpdatePromoterAssetDetails = "Error Occured! Update Promoter Asset Details:  Error message is:";
        public const string DeletePromoterAssetDetails = "Error Occured! Delete Promoter Asset Details:  Error message is:";
        public const string SaveAssetNetworthDetails = "Error Occured! Save Asset Details in NetWorth Table :  Error message is:";
        public const string SaveLiabilityNetworthDetails = "Error Occured! Save Liability Details in NetWorth Table:  Error message is:";
        public const string GetAllPromoterLiabilityDetails = "Error Occured! Get All Promoter Liability Details: Error message is:";
        public const string CreatePromoterLiabilityDetails = "Error Occured! Create Promoter Liability Details:  Error message is:";
        public const string UpdatePromoterLiabilityDetails = "Error Occured! Update Promoter Liability Details:  Error message is:";
        public const string DeletePromoterLiabilityDetails = "Error Occured! Delete Promoter Liability Details:  Error message is:";
        public const string GetAllPromoterNetWorthDetails = "Error Occured! Get All Promoter Net Worth Details: Error message is:";
        public const string GetAllPincodeDetails = "Error Occured! Get All Pincode Details based on District: Error message is:";
        public const string GetAllAssetType = "Error Occured! Get All Asset Type based on Asset Categary : Error message is:";
        public const string GetAllTalukDetails = "Error Occured! Get All Taluk Details based on District: Error message is:";
        public const string GetAllDistrictDetails = "Error Occured! Get All DistrictDetails based on State: Error message is:";
        public const string GetAllHobliDetails = "Error Occured! Get All Hobli Details based on Taluk: Error message is:";
        public const string GetAllVillageDetails = "Error Occured! Get All Hobli Details based on Hobli: Error message is:";

        //For Audit Clearencre Error messages
        public const string getAuditClearenceList = "Error Occured! Get Audit Clearence List:  Error message is:";
        public const string createAuditClearenceList = "Error Occured! Create Audit Clearence List:  Error message is:";
        public const string updateAuditClearenceList = "Error Occured! Update Audit Clearence List:  Error message is:";
        public const string deleteAuditClearenceList = "Error Occured! Delete Audit Clearence List:  Error message is:";
        public const string getAllLoanNumber = "Error Occured! Get All loan Number:  Error message is:";
        //Audit Clearence
        public const string auditResultViewPath = "~/Areas/Admin/Views/Audit/AuditClearance/";
        public const string auditViewPath = "../../Areas/Admin/Views/Audit/AuditClearance/";
        public const string sessionAuditClearance = "SessionAllAuditClearanceList";

        public const string getAllOtherDebitsList = "Error Occured! Get All Other Debits List:  Error message is:";

        public const string updateOtherDebit = "Error Occured! Update OtherDebit Details:  Error message is:";
        public const string deletedOtherDebit = "Error Occured! Delete OtherDebit Details:  Error message is:";

        public const string createOtherDebit = "Error Occured! Create OtherDebit Details:  Error message is:";
        public const string SubmitOtherDebit = "Error Occured! Submit OtherDebit Details:  Error message is:";
        #endregion

        #region Creationofsecurityandacquisition
        public const string GetAllCreationOfSecurityandAquisitionAssetList = "Error Occured! Get All Creation Of Security and Aquisition Asset List:  Error message is:";
        public const string UpdateLandAcquisitionDetails = "Error Occured! Update LandAcquisition Details:  Error message is:";
        public const string GetAllMachineryAcquisitionDetails = "Error Occured! Get All Machinery Acquisition Details:  Error message is:";
        public const string UpdateMachineryAcquisitionDetails = "Error Occured! Update Machinery Acquisition Details:  Error message is:";
       public const string GetFurnitureAcquisitionList = "Error Occured! Get All Machinery Acquisition Details:  Error message is:";
        public const string UpdateFurnitureAcquisition = "Error Occured! Update Furniture Acquisition Details:  Error message is:";
       public const string GetAllBuildingAcquisitionDetails = "Error Occured! Get All Building Acquisition Details:  Error message is:";
        public const string UpdateBuildingAcquisitionDetail = "Error Occured! Update Building Acquisition Details:  Error message is:";

        #endregion

    }
}


