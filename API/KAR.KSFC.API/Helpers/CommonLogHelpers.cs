using System.Dynamic;
using System.Security.Principal;

namespace KAR.KSFC.API.Helpers
{
    public static class CommonLogHelpers
    {
        #region Result Message
        public const string Success = "Success";
        public const string Created = "Basic details created Successfully";
        public const string StartedGetEncryptedFileAsyncMethod = "Started - GetEncryptedFileAsync method With documentId and mainModule";
        public const string CompletedGetEncryptedFileAsyncMethod = "Completed - GetEncryptedFileAsync method With documentId and mainModule";
        public const string StartedDeleteDocumentAsyncmethod = "Started - DeleteDocumentAsync method for documentId and mainModule = ";
        public const string CompletedDeleteDocumentAsyncmethod = "Completed - DeleteDocumentAsync method for documentId = ";
        public const string StartedFileUploadAsyncmethod = "Started - FileUploadAsync method with fileupload";
        public const string CompletedFileUploadAsyncmethod = "Completed - FileUploadAsync method with fileupload";
        public const string StartedGetDocumentListAsyncmethod = "Started - GetDocumentListAsync method for mainModule = ";
        public const string CompletedGetDocumentListAsyncmethod = "Completed - GetDocumentListAsync method for mainModule = ";
        public const string Updated = "Basic details updated Successfully";
        public const string Deleted = "Basic details deleted Successfully";
        #endregion
            
    }
    public static class ErrorMsg
    {
        public const string ErrorMsg1 = "Error occured while getting{0}";
        public const string ErrorMsg2 = "list. Error message is: ";
        public const string Stack = "The stack trace is: ";
        public const string ErrorCode400 = "Error - 400 Something went Wrong";
        public const string Message = "Something went Wrong";
        public const string FileUploadError = "Error - 400 File Upload failed";
        public const string FileUploadfailed = "File Upload failed.";
        public const string FileUploadAsyncLoadingError = "Error occured while loading FileUploadAsync page. Error message is: ";
        public const string NoDataError = "Error - 400 No Data Found";
        public const string GetEncryptedFileAsyncLoadingError = "Error occured while loading GetEncryptedFileAsync page. Error message is: ";
        public const string GetDocumentListAsyncLoadingError = "Error occured while loading GetDocumentListAsync page. Error message is: ";
        public const string FileDeletionfailed = "Error - 400 File Deletion failed";
        public const string DeleteDocumentAsyncLoadingError = "Error occured while loading DeleteDocumentAsync page. Error message is: ";
      
    }
   

    public static class Module
    {
        #region Modules
        public const string Security = "Security";
        public const string Hypothecation = "Hypothecation";
        public const string SecurityCharge = "SecurityCharge";
        public const string Cersai = "Cersai";
        public const string GuarantorDeed = "GuarantorDeed";
        public const string Condition = "Condition";
        public const string Product = "Product Details";
        public const string PromoterProfile = "PromoterProfile";
        public const string MasterPromoterProfile = "MasterPromoterProfile";
        public const string PromoterBank = "PromoterBank";
        public const string LandAcquisition = "Land Acquisition";
        public const string MachineryAcquisition = "MachineryAcquisition";
        public const string DisbursementCondition = "Disbursement Condition";
        public const string AdditionalCondition = "Additional Condition";
        public const string Form8AndForm13 = "Form8AndForm13";
        public const string SidbiApproval = "SidbiApproval";
        public const string FirstInvestmentClause = "FirstInvestmentClause";
        public const string LandInspection = "LandInspection";
        public const string InspectionDetails = "InspectionDetails";
        public const string BuildingInspectionDetails = "BuildingInspectionDetails";
        public const string CreateWorkingCapitalInspection = "CreateWorkingCapitalInspection";
        public const string BuildingMaterialInspection = "BuildingMaterialInspection";
        public const string IndigenousMachineryInspection = "IndigenousMachineryInspection";
        public const string FurnitureInspection = "FurnitureInspection";
        public const string ImportMachineryInspection = "ImportMachineryInspection";
        public const string StatusofImplementation = "StatusofImplementation";
        public const string LetterOfCredit = "LetterOfCredit";
        public const string MeansFinanceDetails = "MeansFinanceDetails";
        public const string ProjectCostDetails = "ProjectCostDetails";
        public const string LoanAllocataion = "LoanAllocataion";
        public const string NameOfSecurity = "NameOfSecurity";
        public const string ChangeLocationAddress = "ChangeLocationAddress";
        public const string MasterPinCodeDetails = "MasterPinCodeDetails";
        public const string PinCodeDistrictDetails = "PinCodeDistrictDetails";
        public const string AssetTypeDetails = "AssetTypeDetails";
        public const string AssetCategaryDetails = "AssetCategaryDetails";
        public const string PromoterAddress = "PromoterAddress";
        public const string ChangeBankDetails = "ChangeBankDetails";
        public const string IfscBankDetails = "IfscBankDetails";
        public const string ChangeAssetInformation = "ChangeAssetInformation";
        public const string ChangePromoterLiabilityInformation = "ChangePromoterLiabilityInformation";
        public const string PromoterNetWorthInformation = "PromoterNetWorthInformation";
        public const string BuildingAcquisition = "BuildingAcquisition";
        public const string FurnitureAcquisition = "FurnitureAcquisition";
        public const string AuditClearance = "AuditClearance";
        public const string RecommendedDisbursement = "RecommendedDisbursement";
        public const string DisbursementProposal = "DisbursementProposal";
        public const string BeneficiaryDetails = "BeneficiaryDetails";
        public const string OtherDebitsDetails = "OtherDebitsDetails";
        #endregion
    }


    public static class LogDetails
    {
        public const string GetAccountNumber = "Calling GetAccountNumber(): ";
        public const string CompletedGetAccountNumber = "Completed GetAccountNumber(): ";
        public const string GetNameOfUnit = "Calling GetNameOfUnit() for Account Number: ";
        public const string CompletedGetNameOfUnit = "Completed GetNameOfUnit() for Account Number: ";
        public const string GetAllAddressDetails = "Calling GetAllAddressDetails() for Account Number: ";
        public const string CompletedGetAllAddressDetails = "Completed GetAllAddressDetails() for Account Number: ";
        public const string GetAllMasterPinCodeDetails = "Calling GetAllMasterPinCodeDetails() ";
        public const string CompletedGetAllMasterPinCodeDetails = "Completed GetAllMasterPinCodeDetails() ";
        public const string GetAllTalukDetails = "Calling GetAllTalukDetails() ";
        public const string CompletedGetAllTalukDetails = "Completed GetAllTalukDetails() ";
        public const string GetAllVillageDetails = "Calling GetAllVillageDetails() ";
        public const string CompletedGetAllVillageDetails = "Completed GetAllVillageDetails() ";
        public const string GetAllHobliDetails = "Calling GetAllHobliDetails() ";
        public const string CompletedGetAllHobliDetails = "Completed GetAllHobliDetails() ";
        public const string GetAllPinCodeDistrictDetails = "Calling GetAllPinCodeDistrictDetails() ";
        public const string CompletedGetAllPinCodeDistrictDetails = "Completed GetAllPinCodeDistrictDetails() ";
        public const string GetAllPromoterProfileDetails = "Calling GetAllPromoterProfileDetails() for Account Number: ";
        public const string CompletedGetAllPromoterProfileDetails = "Completed GetAllPromoterProfileDetails() for Account Number: ";
        public const string GetAllLandAssets = "Calling GetAllLandAssets() for Account Number: ";
        public const string CompletedGetAllLandAssets = "Completed GetAllLandAssets() for Account Number: ";
        public const string GetAllMasterPromoterProfileDetails = "Calling GetAllMasterPromoterProfileDetails() ";
        public const string CompletedGetAllMasterPromoterProfileDetails = "Completed GetAllMasterPromoterProfileDetails() ";
        public const string GetAllPromoterBankInfo = "Calling GetAllPromoterBankInfo() for Account Number: ";
        public const string GetAllAssetTypeDetails = "Calling  GetAllAssetCategaryDetails";
        public const string GetAllAssetCategaryDetails = "Calling  GetAllAssetTypeDetails";
        public const string CompletedGetAllAssetTypeDetails =  "Completed  GetAllAssetTypeDetails ";
        public const string CompletedGetAllAssetCategaryDetails = "Completed  GetAllAssetCategaryDetails ";
        public const string CompletedGetAllPromoterBankInfo = "Completed GetAllPromoterBankInfo() for Account Number: ";
        public const string GetAllPromoterAddressDetails = "Calling GetAllPromoterAddressDetails() for Account Number: ";
        public const string CompletedGetAllPromoterAddressDetails = "Completed GetAllPromoterAddressDetails() for Account Number: ";
        public const string GetAllProductDetails = "Calling GetAllProductDetails() for Account Number: ";
        public const string CompletedGetAllProductDetails = "Completed GetAllProductDetails() for Account Number: ";
        public const string GetAllProductList = "Calling GetAllProductList() ";
        public const string CompletedGetAllProductList = "Completed GetAllProductList() ";
        public const string GetAllChangeBankDetailsList = "Calling GetAllChangeBankDetailsList() for Account Number: ";
        public const string CompletedGetAllChangeBankDetailsList = "Completed GetAllChangeBankDetailsList() for Account Number: ";
        public const string GetAllIfscBankDetails = "Calling GetAllIfscBankDetails() ";
        public const string CompletedGetAllIfscBankDetails = "Completed GetAllIfscBankDetails() ";
        public const string GetAllPromoterAssetDetails = "Calling GetAllPromoterAssetDetails() for Account Number: ";
        public const string CompletedGetAllPromoterAssetDetails = "Completed GetAllPromoterAssetDetails() for Account Number: ";
        public const string GetAllPromoterLiabiltyInfo = "Calling GetAllPromoterLiabiltyInfo() for Account Number: ";
        public const string CompletedGetAllPromoterLiabiltyInfo = "Completed GetAllPromoterLiabiltyInfo() for Account Number: ";
        public const string GetAllPromoterNetWorth = "Calling GetAllPromoterNetWorth() for Account Number: ";
        public const string CompletedGetAllPromoterNetWorth = "Completed GetAllPromoterNetWorth() for Account Number: ";
        public const string GetAllLandInspectionList = "Calling GetAllLandInspectionList() for Account Number: ";
        public const string CompletedGetAllLandInspectionList = "Completed GetAllLandInspectionList() for Account Number: ";
        public const string GetAllInspectionDetailsList = "Calling GetAllInspectionDetailsList() for Account Number: ";
        public const string CompletedGetAllInspectionDetailsList = "Completed GetAllInspectionDetailsList() for Account Number: ";
        public const string GetAllBuildingInspectionList = "Calling GetAllBuildingInspectionList() for Account Number: ";
        public const string CompletedGetAllBuildingInspectionList = "Completed GetAllBuildingInspectionList() for Account Number: ";
        public const string GetAllBuildingMaterialInspectionList = "Calling GetAllBuildingMaterialInspectionList() for Account Number: ";
        public const string CompletedGetAllBuildingMaterialInspectionList = "Completed GetAllBuildingMaterialInspectionList() for Account Number: ";
        public const string GetAllIndigenousMachineryInspectionList = "Calling GetAllIndigenousMachineryInspectionList() for Account Number: ";
        public const string CompletedGetAllIndigenousMachineryInspectionList = "Completed GetAllIndigenousMachineryInspectionList() for Account Number: ";
        public const string GetAllFurnitureInspectionDetailsList = "Calling GetAllFurnitureInspectionDetailsList() for Account Number: ";
        public const string CompletedGetAllFurnitureInspectionDetailsList = "Completed GetAllFurnitureInspectionDetailsList() for Account Number: ";
        public const string GetAllImportMachineryInspectionList = "Calling GetAllImportMachineryInspectionList() for Account Number: ";
        public const string CompletedGetAllImportMachineryInspectionList = "Completed GetAllImportMachineryInspectionList() for Account Number: ";
        public const string GetAllStatusOfImplementationList = "Calling GetAllStatusOfImplementationList() for Account Number: ";
        public const string CompletedGetAllStatusOfImplementationList = "Completed GetAllStatusOfImplementationList() for Account Number: ";
        public const string GetAllLetterOfCreditDetailList = "Calling GetAllLetterOfCreditDetailList() for Account Number: ";
        public const string CompletedGetAllLetterOfCreditDetailList = "Completed GetAllLetterOfCreditDetailList() for Account Number: ";
        public const string GetAllMeansOfFinanceList = "Calling GetAllMeansOfFinanceList() for Account Number: ";
        public const string CompletedGetAllMeansOfFinanceList = "Completed GetAllMeansOfFinanceList() for Account Number: ";
        public const string GetAllProjectCostDetailsList = "Calling GetAllProjectCostDetailsList() for Account Number: ";
        public const string CompletedGetAllProjectCostDetailsList = "Completed GetAllProjectCostDetailsList() for Account Number: ";
        public const string GetAllLoanAllocationList = "Calling GetAllLoanAllocationList() for Account Number: ";
        public const string CompletedGetAllLoanAllocationList = "Completed GetAllLoanAllocationList() for Account Number: ";
        public const string GetAllMachineryStatus = "Started GetAllMachinaryStatus Method in Api";
        public const string GetAllProcureList = "Started GetAllProcureList Method in Api";
        public const string GetAllCurrencyList = "Started GetAllCurrencyList Method in Api";
        public const string ConmpletedGetAllMachineryStatus = "Completed GetAllMachinaryStatus Method in Api";
        public const string ConmpletedGetAllProcureList = "Completed GetAllProcureList Method in Api";


    }
    public static class Constants
    {
        public const string Result = " Result is: ";
        #region LegalDocumentation
        public const string GetPrimaryCollateralList = "Calling GetPrimaryCollateralList() for Account Number: ";
        public const string CompletedGetPrimaryCollateralList = "Completed GetPrimaryCollateralList() for Account Number: ";

        public const string GetCollateralList = "Calling GetCollateralList() for Account Number: ";
        public const string CompletedGetCollateralList = "Completed CompletedGetCollateralList() for Account Number: ";



        public const string GetAllCERSAIRegistrationAsync = "Calling GetAllCERSAIRegistrationAsync() for Account Number: ";
        public const string CompletedGetAllCERSAIRegistrationAsync = "Completed GetAllCERSAIRegistrationAsync() for Account Number: ";

        public const string GetAllSecurityChargeAsync = "Calling GetAllSecurityChargeAsync() for Account Number: ";
        public const string CompletedGetAllSecurityChargeAsync = "Completed GetAllSecurityChargeAsync() for Account Number: ";

        public const string GetAllHypothecationList = "Calling GetAllHypothecationList() for Account Number: ";
        public const string CompletedGetAllHypothecationList = "Completed GetAllHypothecationList() for Account Number: ";

        public const string GetAllAssetRefListAsync = "Calling GetAllAssetRefListAsync() for Account Number: ";
        public const string CompletedGetAllAssetRefListAsync = "Completed GetAllAssetRefListAsync() for Account Number: ";

        public const string GetAllConditionListAsync = "Calling GetAllConditionListAsync() for Account Number: ";
        public const string CompletedGetAllConditionListAsync = "Completed GetAllConditionListAsync() for Account Number: ";

        public const string GetAllGuarantorListAsync = "Calling GetAllGuarantorListAsync() for Account Number: ";
        public const string CompletedGetAllGuarantorListAsync = "Completed GetAllGuarantorListAsync() for Account Number: ";
        #endregion

        #region AuditClearence
        public const string GetAllAuditClearanceListAsync = "Calling GetAllAuditClearanceListAsync() for Account Number: ";
        public const string CompletedGetAllAuditClearanceListAsync = "Completed GetAllAuditClearanceListAsync() for Account Number: ";
        #endregion

        #region DisbursmentCondetion
        public const string GetAllDisbursementList = "Calling GetAllDisbursementList() for Account Number: ";
        public const string CompletedGetAllDisbursementList = "Completed GetAllDisbursementList() ";
        public const string GetAllForm8AndForm13List = "Calling GetAllForm8AndForm13List() for Account Number: ";
        public const string CompletedGetAllForm8AndForm13List = "Completed GetAllForm8AndForm13List() for Account Number: ";
        public const string GetAllSidbiApprovalDetails = "Calling GetAllSidbiApprovalDetails() for Account Number: ";
        public const string CompletedGetAllSidbiApprovalDetails = "Completed GetAllSidbiApprovalDetails() for Account Number: ";
        public const string UpdateSidbiApprovalDetails = "Started Updating Sidbi Approval Details ";
        public const string CompletedUpdateSidbiApprovalDetails = "Completed UpdateSidbiApprovalDetails()";
        public const string GetAllAdditionalConditionList = "Calling GetAllAdditionalConditionList() for Account Number: ";
        public const string CompletedGetAllAdditionalConditionList = "Completed GetAllAdditionalConditionList() for Account Number: ";
        public const string CreateAdditionalConditionDetails = "Started CreateAdditionalConditionDetails() ";
        public const string CompletedCreateAdditionalConditionDetails = "Completed CompletedCreateAdditionalConditionDetails()";

        public const string GetAllFirstInvestmentClause = "Calling GetAllFirstInvestmentClause() for Account Number: ";
        public const string CompletedGetAllFirstInvestmentClause = "Completed GetAllFirstInvestmentClause() for Account Number: ";

        public const string GetAllOtherRelaxation = "Calling GetAllOtherRelaxation() for Account Number: ";
        public const string CompletedGetAllOtherRelaxation = "Completed GetAllOtherRelaxation() for Account Number: ";

        #endregion

        #region CreationOfSecurityAndAquisition
        public const string GetAllCreationOfSecurityandAquisitionAssetList = "Calling GetAllCreationOfSecurityandAquisitionAssetList() for Account Number: ";
        public const string CompletedGetAllCreationOfSecurityandAquisitionAssetList = "Completed GetAllCreationOfSecurityandAquisitionAssetList() for Account Number: ";
        public const string GetAllMachineryAcquisitionDetails = "Calling GetAllMachineryAcquisitionDetails() for Account Number: ";
        public const string CompletedGetAllMachineryAcquisitionDetails = "Completed GetAllMachineryAcquisitionDetails() for Account Number: ";
        public const string GetAllBuildingAcquisitionDetails = "Calling GetAllBuildingAcquisitionDetails() for Account Number: ";
        public const string CompletedGetAllBuildingAcquisitionDetails = "Completed GetAllBuildingAcquisitionDetails() for Account Number: ";
        public const string GetAllFurnitureAcquisitionList = "Calling GetAllFurnitureAcquisitionList() for Account Number: ";
        public const string CompletedGetAllFurnitureAcquisitionList = "Completed GetAllFurnitureAcquisitionList() for Account Number: ";

        #endregion

        #region CreationOfDisbursmentProposal
        public const string GetAllRecomDisbursementDetails = "Calling GetAllRecomDisbursementDetails() for Account Number: ";
        public const string CompletedGetAllRecomDisbursementDetails = "Completed GetAllRecomDisbursementDetails() for Account Number: ";
        public const string GetAllAllocationCodeDetails = "Calling GetAllAllocationCodeDetails() in API";
        public const string CompletedGetAllAllocationCodeDetails = "Completed GetAllAllocationCodeDetails() in API ";
        public const string GetRecomDisbursementReleaseDetails = "Calling GetRecomDisbursementReleaseDetails() in API";
        public const string CompletedGetRecomDisbursementReleaseDetails = "Completed GetRecomDisbursementReleaseDetails() in API ";
        public const string GetAllProposalDetails = "Calling GetAllProposalDetails() for Account Number: ";
        public const string CompletedGetAllProposalDetails = "Completed GetAllProposalDetails() for Account Number: ";
        public const string GetAllBeneficiaryDetails = "Calling GetAllBeneficiaryDetails() for Account Number: ";
        public const string CompletedGetAllBeneficiaryDetails = "Completed GetAllBeneficiaryDetails() for Account Number: ";
        #endregion

        #region EntryofOtherDebits
        public const string GetAllOtherDebitsList = "Calling GetAllOtherDebitsList() for Account Number: ";
        public const string CompletedGetGetAllOtherDebitsList = "Completed GetAllOtherDebitsList() for Account Number: ";

        #endregion
    }

}

