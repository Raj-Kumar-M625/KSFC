namespace KAR.KSFC.API.Helpers
{
    public static class RouteName
    {
        #region API for GET Routes
        public const string GetAccountNumber = "GetAccountNumber";
        public const string GetAllHypothecationList = "GetAllHypothecationList";
        public const string GetAllSecurityHolders = "GetAllSecurityHolders";
        public const string GetAllSecurityHolder = "GetAllSecurityHolder";
        public const string GetAllSecurityChargeList = "GetAllSecurityChargeList";
        public const string GetAllCERSAIRegistrationList = "GetAllCERSAIRegistrationList";
        public const string GetAllGuarantorList = "GetAllGuarantorList";
        public const string GetAllConditionList = "GetAllConditionList";
        public const string GetAllAssetRefList = "GetAllAssetRefList";
        public const string GetAllAuditClearanceList = "GetAllAuditClearanceList";
        public const string GetCodetable = "GetCodetable";

        
        #endregion

        #region API for UPDATE Routes

        public const string UpdateLDSecurityDetails = "UpdateLDSecurityDetails";
        public const string UpdateLDSecurityDetail = "UpdateLDSecurityDetail";
        public const string CreateLDSecurityDetail = "CreateLDSecurityDetail";
        public const string UpdateSecurityChargeDetails = "UpdateSecurityChargeDetails";
        public const string UpdateLDGuarantorDeedDetails = "UpdateLDGuarantorDeedDetails";
        public const string UpdateLDHypothecationDetails = "UpdateLDHypothecationDetails";
        public const string UpdateLDConditionDetails = "UpdateLDConditionDetails";
        public const string UpdateAuditClearanceDetails = "UpdateAuditClearanceDetails";
        #endregion

        #region API for CREATE Routes
        public const string CreateLDHypothecationDetails = "CreateLDHypothecationDetails";
        public const string CreateLDConditionDetails = "CreateLDConditionDetails";
        public const string CreateAuditClearanceDetails = "CreateAuditClearanceDetails";
        public const string CreateLDCersaiRegDetails = "CreateLDCersaiRegDetails";
        
        #endregion

        #region API for DELETE Routes
        public const string DeleteLDHypothecationDetails = "DeleteLDHypothecationDetails";
        public const string DeleteLDCersaiRegDetails = "DeleteLDCersaiRegDetails";
        public const string DeleteLDConditionDetails = "DeleteLDConditionDetails";
        public const string DeleteAuditClearanceDetails = "DeleteAuditClearanceDetails";
        #endregion
    }
    public static class DisbursementRouteName
    {
        #region API for GET Routes
        public const string GetAccountNumber = "GetAccountNumber";
        public const string GetAllDisbursementList = "GetAllDisbursementList";
        public const string GetAllForm8AndForm13List = "GetAllForm8AndForm13List";
        public const string GetAllAdditionalCondition = "GetAllAdditionalConditionList";
        public const string GetAllSidbiApprovalDetails = "GetAllSidbiApprovalDetails";
        public const string GetAllFirstInvestmentClauseDetails = "GetAllFirstInvestmentClauseDetails";
        public const string GetAllOtherRelaxation = "GetAllOtherRelaxation";
        #endregion
        #region API for DELETE Routes
        public const string DeleteDisbursementConditionDetails = "DeleteDisbursementConditionDetails";
        public const string DeleteForm8AndForm13Details = "DeleteForm8AndForm13Details";
        public const string DeleteAdditionalConditionDetails = "DeleteAdditionalConditionDetails";

        #endregion
        #region API for Update Routes
        public const string UpdateDisbursementConditionDetails = "UpdateDisbursementConditionDetails";
        public const string UpdateForm8AndForm13Details = "UpdateForm8AndForm13Details";
        public const string UpdateAdditionalConditionDetails = "UpdateAdditonalConditionDetails";
        public const string UpdateFirstInvestmentClauseDetails = "UpdateFirstInvestmentClauseDetails";//AD
        public const string UpdateSidbiApprovalDetails = "UpdateSidbiApprovalDetails"; // Dev
        public const string UpdateOtherRelaxation = "UpdateOtherRelaxation"; // Dev
        #endregion
        #region API for Create Routes
        public const string CreateDisbursementConditionDetails = "CreateDisbursementConditionDetails";
        public const string CreateForm8AndForm13Details = "CreateForm8AndForm13Details";
        public const string CreateAdditionalConditionDetails = "CreateAdditionalConditionDetails";
        #endregion
    }
    public static class InspectionOfUnitRouteName
    {
        #region API for GET Routes
        public const string GetAccountNumber = "GetAccountNumber";
        public const string GetAllLandInspectionList = "GetAllLandInspectionList";
        public const string GetAllBuildingInspectionList = "GetAllBuildingInspectionList";
        public const string GetAllInspectionDetailsList = "GetAllInspectionDetailsList";
        public const string GetAllBuildingMaterialInspectionList = "GetAllBuildingMaterialInspectionList";
        public const string GetAllImportMachineryInspectionList = "GetAllImportMachineryInspectionList";
        public const string GetAllStatusofImplementationList = "GetAllStatusofImplementationList";
        public const string GetAllFurnitureInspectionDetailsList = "GetAllFurnitureInspectionDetailsList";
        public const string GetAllIndigenousMachineryInspectionList = "GetAllIndigenousMachineryInspectionList";
        public const string GetAllMachineryStatusList = "GetAllMachineryStatusList";
        public const string GetAllProcureList = "GetAllProcureList";
        public const string GetAllCurrencyList = "GetAllCurrencyList";
        public const string GetAllLetterOfCreditDetailList = "GetAllLetterOfCreditDetailList";
        public const string GetAllMeansOfFinanceList = "GetAllMeansOfFinanceList";
        public const string GetAllProjectCostDetailsList = "GetAllProjectCostDetailsList";


        #endregion
        #region API for UPDATE Routes
        public const string UpdateLandInspectionDetails = "UpdateLandInspectionDetails";
        public const string UpdateFurnitureInspectionDetails = "UpdateFurnitureInspectionDetails";
        public const string UpdateBuildingInspectionDetails = "UpdateBuildingInspectionDetails";
        public const string UpdateInspectionDetails = "UpdateInspectionDetails";
        public const string UpdateIndigenousMachineryInspectionDetails = "UpdateIndigenousMachineryInspectionDetails";
        public const string UpdateProjectCostDetails = "UpdateProjectCostDetails";

        public const string UpdateBuildMatSiteInspectionDetails = "UpdateBuildMatSiteInspectionDetails";
        public const string UpdateImportMachineryInspectionDetails = "UpdateImportMachineryInspectionDetails";
        public const string UpdateStatusofImplementationDetaiils = "UpdateStatusofImplementationDetails";
        public const string UpdateMeansOfFinanceDetails = "UpdateMeansOfFinanceDetails";
        public const string UpdateLetterOfCreditDetails = "UpdateLetterOfCreditDetails";
        #endregion
        #region API for CREATE Routes
        public const string CreateLandInspectionDetails = "CreateLandInspectionDetails";
        public const string CreateBuildingInspectionDetails = "CreateBuildingInspectionDetails";
        public const string CreateFurnitureInspectionDetails = "CreateFurnitureInspectionDetails";
        public const string CreateWorkingCapitalInspection = "CreateWorkingCapitalInspection";
        public const string CreateInspectionDetails = "CreateInspectionDetails";
        public const string CreateBuildMatSiteInspectionDetails = "CreateBuildMatSiteInspectionDetails";
        public const string CreateImportMachineryInspectionDetails = "CreateImportMachineryInspectionDetails";
        public const string CreateStatusofImplementationDetails = "CreateStatusofImplementationDetails";
        public const string CreateIndigenousMachineryInspectionDetails = "CreateIndigenousMachineryInspectionDetails";
        public const string CreateProjectCostDetails = "CreateProjectCostDetails";
        public const string CreateMeansOfFinanceDetails = "CreateMeansOfFinanceDetails";
        public const string CreateLetterOfCreditDetails = "CreateLetterOfCreditDetails";
        #endregion

        #region API for DELETE Routes
        public const string DeleteLandInspectionDetails = "DeleteLandInspectionDetails";
        public const string DeleteFurnitureInspectionDetails = "DeleteFurnitureInspectionDetails";
        public const string DeleteBuildingInspectionDetails = "DeleteBuildingInspectionDetails";
        public const string DeleteInspectionDetails = "DeleteInspectionDetails";
        public const string DeleteBuildMatSiteInspectionDetails = "DeleteBuildMatSiteInspectionDetails";
        public const string DeleteIndigenousMachineryInspectionDetails = "DeleteIndigenousMachineryInspectionDetails";
        public const string DeleteImportMachineryInspectionDetails = "DeleteImportMachineryInspectionDetails";
        public const string DeleteStatusofImplementationDetails = "DeleteStatusofImplementationDetails";
        public const string DeleteMeansOfFinanceDetails = "DeleteMeansOfFinanceDetails";
        public const string DeleteLetterOfCreditDetails = "DeleteLetterOfCreditDetails";
        public const string DeleteProjectCostDetails = "DeleteProjectCostDetails";
        #endregion
    }


    public static class UnitDetailsRouteName
    {

        #region API for GET Routes
        public const string GetAccountNumber = "GetAccountNumber";
        public const string GetNameOfUnit = "GetNameOfUnit";
        public const string GetAllAddressDetails = "GetAllAddressDetails";
        public const string GetAllPromoterProfileDetails = "GetAllPromoterProfileDetails";
        public const string GetAllMasterPromoterProfileDetails = "GetAllMasterPromoterProfileDetails";
        public const string GetAllPromoterAddressDetails = "GetAllPromoAddressDetails";
        public const string GetAllPromoterBankInfo = "GetAllPromoterBankInfo";
        public const string GetAllProductDetails = "GetAllProductDetails";
        public const string GetAllProductList = "GetAllProductList";

        public const string GetAllChangeBankDetails = "GetAllChangeBankDetails";
        public const string GetAllIfscBankDetails = "GetAllIfscBankDetails";        
        public const string GetAllPromoterAssetDetails = "GetAllPromoterAssetDetails";
        public const string GetAllPromoterLiabiltyInfo = "GetAllPromoterLiabiltyInfo";
        public const string GetAllPromoterNetWorth = "GetAllPromoterNetWorth";
        public const string GetAllMasterPinCodeDetails = "GetAllMasterPinCodeDetails";
        public const string GetAllPinCodeDistrictDetails = "GetAllPinCodeDistrictDetails";
        public const string GetAllAssetTypeDetails = "GetAllAssetTypeDetails";
        public const string GetAllAssetCategaryDetails = "GetAllAssetCategaryDetails";
        public const string GetAllTalukDetails = "GetAllTalukDetails";
        public const string GetAllHobliDetails = "GetAllHobliDetails";
        public const string GetAllVillageDetails = "GetAllVillageDetails";
        public const string GetAllLandAssets = "GetAllLandAssets";
       
        #endregion

        #region API for Update Routes        
        public const string UpdateNameOfUnit = "UpdateNameOfUnit";
        public const string UpdatePromAddressDetails = "UpdatePromAddressDetails";
        public const string UpdatePromoterProfileDetails = "UpdatePromoterProfileDetails";
        public const string UpdateProductDetails = "UpdateProductDetails";
        public const string UpdatePromoterLiabilityInfo = "UpdatePromoterLiabilityInfo";
        public const string UpdateChangeBankDetails = "UpdateChangeBankDetails";
        public const string UpdateAssetDetails = "UpdateAssetDetails";
        public const string UpdatePromoterBankInfo = "UpdatePromoterBankInfo";
        public const string UpdateAddressDetails = "UpdateAddressDetails";
        public const string SaveAssetNetworthDetails = "SaveAssetNetworthDetails";
        public const string SaveLiabilityNetworthDetails = "SaveLiabilityNetworthDetails";
        #endregion
    }
   
    public static class GenerateReceipt
    {
        #region API for GET Routes
        public const string GetAllGenerateReceiptList = "GetAllGenerateReceiptList";
        public const string GetAllReceiptPaymentList = "GetAllReceiptPaymentList";
        public const string GetAllReceiptRefNum = "GetAllReceiptRefNum";
        public const string GetAllPaymentRefNum = "GetAllPaymentRefNum";
        public const string GetAllGenerateReceiptPaymentList = "GetAllGenerateReceiptPaymentList";
        public const string GetAllReceiptPayment = "GetAllReceiptPayment";
        #endregion


        #region API for Update Routes
        public const string UpdateReceiptPaymentDetails = "UpdateReceiptPaymentDetails";
        public const string UpdateCreatePaymentDetails = "UpdateCreatePaymentDetails";
        public const string UpdateCreatePromPaymentDetails = "UpdateCreatePromPaymentDetails";
        
        #endregion

        #region API for Create Routes
        public const string CreateReceiptPaymentDetails = "CreateReceiptPaymentDetails"; 
        public const string CreatePaymentDetails = "CreatePaymentDetails";
        #endregion

        #region API for Delete Routes
        public const string DeleteReceiptPaymentDetails = "DeleteReceiptPaymentDetails";
        #endregion

        public const string ApproveReceiptPaymentDetails = "ApproveReceiptPaymentDetails";
        public const string RejectReceiptPaymentDetails = "RejectReceiptPaymentDetails";

        #region API for DELETE Routes
        public const string DeleteAddressDetails = "DeleteAddressDetails";
        public const string DeletePromAddressDetails = "DeletePromAddressDetails";
        public const string DeleteProductDetails = "DeleteProductDetails"; 
        public const string DeletePromoterProfileDetails = "DeletePromoterProfileDetails";
        public const string DeleteChangeBankDetails = "DeleteChangeBankDetails";   
        public const string DeleteAssetDetails = "DeleteAssetDetails";
        public const string DeletePromoterLiabilityInfo = "DeletePromoterLiabilityInfo";      
        public const string DeletePromoterBankInfo = "DeletePromoterBankInfo";

        #endregion  

        #region API for CREATE Routes
        public const string CreatePromAddressDetails = "CreatePromAddressDetails";
        public const string CreatePromoterProfileDetails = "CreatePromoterProfileDetails";
        public const string CreateProductDetails = "CreateProductDetails";
        public const string CreateChangeBankDetails = "CreateChangeBankDetails";
        public const string CreateAssetDetails = "CreateAssetDetails";
        public const string CreatePromoterBankInfo = "CreatePromoterBankInfo";
        public const string CreateLandAssets = "CreateLandAssets";
        public const string CreatePromoterLiabilityInfo = "CreatePromoterLiabilityInfo";
        #endregion
    }

    public static class CreationOfSecurityandAquisitionAssetServiceRouteName
    {

        #region API for GET Routes
        public const string GetAllCreationOfSecurityandAquisitionAssetList = "GetAllCreationOfSecurityandAquisitionAssetList";
        public const string GetAllMachineryAcquisitionDetails = "GetAllMachineryAcquisitionDetails";
        public const string GetAllBuildingAcquisitionDetails = "GetAllBuildingAcquisitionDetails";
        public const string GetAllFurnitureAcquisitionList = "GetAllFurnitureAcquisitionList";
        #endregion

        #region API for Update Routes
        public const string UpadteLandAcquisitionDetails = "UpdateLandAcquisitionDetails";
        public const string UpadteMachineryAcquisitionDetails = "UpdateMachineryAcquisitionDetails";
        public const string UpdateBuildingAcquisitionDetail = "UpdateBuildingAcquisitionDetail";
        public const string UpdateFurnitureAcuisitonDetails = "UpdateFurnitureAcquisition";
        #endregion


    }

    public static class LoanAllocationRouteName
    {
        #region API for GET Routes
        public const string GetAllLoanAllocationList = "GetAllLoanAllocationList";
        #endregion
        #region API for DELETE Routes
        public const string DeleteLoanAllocationDetails = "DeleteLoanAllocationDetails";
        #endregion
        #region API for Update Routes
        public const string UpdateLoanAllocationDetails = "UpdateLoanAllocationDetails";
        #endregion
        #region API for Create Routes
        public const string CreateLoanAllocationDetails = "CreateLoanAllocationDetails";
        #endregion
    }

    public static class EntryOfOtherDebitsRouteName
    {
        #region API for GET Routes
        public const string GetAllOtherDebitsList = "GetAllOtherDebitsList";
        #endregion
        #region API for DELETE Routes
        public const string DeleteOtherDebitDetails = "DeleteOtherDebitDetails";
        #endregion
        #region API for Update Routes
        public const string UpdateOtherDebitDetails = "UpdateOtherDebitDetails";
        #endregion
        #region API for Create Routes
        public const string CreateOtherDebitDetails = "CreateOtherDebitDetails";
        #endregion
        #region API for Create Routes
        public const string SubmitOtherDebitDetails = "SubmitOtherDebitDetails";
        #endregion
    }
 

    #region Recommended Disbursement Details

    public static class CreationOfDisbursmentProposalRouteName
    {
        #region API for GET Routes
        public const string GetAllRecomDisbursementDetails = "GetAllRecomDisbursementDetails";
        public const string GetAllocationCodeDetails = "GetAllocationCodeDetails";
        public const string GetRecomDisbursementReleaseDetails = "GetRecomDisbursementReleaseDetails";
        public const string GetAllProposalDetails = "GetAllProposalDetails";
        public const string GetAllBeneficiaryDetails = "GetAllBeneficiaryDetails";
        #endregion

        #region API for UPDATE Routes  
        public const string UpdateRecomDisbursementDetail = "UpdateRecomDisbursementDetail";
        public const string UpdateBeneficiaryDetails = "UpdateBeneficiaryDetails";
        public const string UpdateProposalDetail = "UpdateProposalDetail";
        #endregion

        #region API for DELETE Routes
        public const string DeleteProposalDetail = "DeleteProposalDetail";
        #endregion

        #region For Create route 
        public const string CreateProposalDetail = "CreateProposalDetail";
        #endregion
    }

    #endregion
}
