namespace KAR.KSFC.UI.Helpers
{
    public static class RouteName
    {
        #region API for GET Routes
        public const string GetAccountNumber = "IDM/GetAccountNumber?EmpId=";
        public const string GetAllHypothecationList = "LegalDocumentation/GetAllHypothecationList?accountNumber={0}&paramater={1}";
        public const string GetAllSecurityHolders = "LegalDocumentation/GetAllSecurityHolders?accountNumber=";
        public const string GetAllConditionList = "LegalDocumentation/GetAllConditionList?accountNumber=";
        public const string GetAllSecurityChargeList = "LegalDocumentation/GetAllSecurityChargeList?accountNumber=";
        public const string GetAllCERSAIRegistrationList = "LegalDocumentation/GetAllCERSAIRegistrationList?accountNumber={0}&parameter={1}";
        public const string GetAllGuarantorList = "LegalDocumentation/GetAllGuarantorList?accountNumber=";
        public const string GetAllAssetRefList = "LegalDocumentation/GetAllAssetRefList?accountNumber=";
        public const string unitdetails = "Account/unitdetails";
        #endregion

        #region API for UPDATE Routes
        public const string UpdateLDCersaiRegDetails = "LegalDocumentation/UpdateLDCersaiRegDetails";
        public const string UpdateLDSecurityDetails = "LegalDocumentation/UpdateLDSecurityDetails";
        public const string UpdateSecurityChargeDetails = "LegalDocumentation/UpdateSecurityChargeDetails";
        public const string UpdateLDGuarantorDeedDetails = "LegalDocumentation/UpdateLDGuarantorDeedDetails";
        public const string UpdateLDHypothecationDetails = "LegalDocumentation/UpdateLDHypothecationDetails";
        public const string UpdateLDConditionDetails = "LegalDocumentation/UpdateLDConditionDetails";

        #endregion

        #region API for CREATE Routes
        public const string CreateLDHypothecationDetails = "LegalDocumentation/CreateLDHypothecationDetails";
        public const string CreateLDConditionDetails = "LegalDocumentation/CreateLDConditionDetails";
        public const string CreateLDCersaiDetails = "LegalDocumentation/CreateLDCersaiRegDetails";
        public const string CreateLDSecurityDetails = "LegalDocumentation/CreateLDSecurityDetails";

        #endregion

        #region API for DELETE Routes
        public const string DeleteLDHypothecationDetails = "LegalDocumentation/DeleteLDHypothecationDetails";
        public const string DeleteLDConditionDetails = "LegalDocumentation/DeleteLDConditionDetails";
        public const string DeleteLDCersaiDetails = "LegalDocumentation/DeleteLDCersaiRegDetails";
        #endregion


        #region API for Generic dropdowns
        public const string GetAllSecurityCategory = "Common/GetAllSecurityCategory";
        public const string GetAllBankIfscCode = "Common/GetAllBankIfscCode";
        public const string GetAllChargeType = "Common/GetAllChargeType";
        public const string GetAllSecurityTypes = "Common/GetAllSecurityTypes";
        public const string GetAllSubRegistrarOffice = "Common/GetAllSubRegistrarOffice";
        public const string GetAllAssetTypes = "Common/GetAllAssetTypes";
        public const string GetAllConditionTypes = "Common/GetAllConditionTypes";
        public const string GetAllConditionStages = "Common/GetAllConditionStages";
        public const string GetAllConditionStageMaster = "Common/GetAllConditionStageMaster";
        public const string GetAllConditionDescriptions = "Common/GetAllConditionDescriptions";
        public const string GetAllForm8AndForm13Master = "Common/GetAllForm8AndForm13Master";
        public const string GetAllStateZone = "Common/GetAllStateZone";
        public const string GetPojectCostComponentAsync = "Common/GetPojectCostComponentAsync";
        public const string GetAllDistricts = "Common/GetDistrict";
        public const string GetAllTaluks = "Common/GetTaluka";
        public const string GetAllHobli = "Common/GetAllHobli";

        public const string GetPositionDesignation = "Common/GetPositionDesignation"; //By Dev on 01/09/2022
        public const string GetDomicileStatus = "Common/GetDomicileStatus";  //By Dev on 01/09/2022
        public const string GetAllPromoterClass = "Common/GetAllPromotorClass";  //By Dev on 01/09/2022
        public const string GetAllPromoterSubClass = "Common/GetAllPromotorSubClass";  //By Dev on 03/09/2022
        public const string GetAllPromoterQualification = "Common/GetAllPromotorQualification";  //By Dev on 03/09/2022
        public const string GetAllAccountType = "Common/GetAllAccountType";  //By Dev on 06/09/2022
        public const string GetAllIfscBankDetails = "Common/GetAllBankIFSCCode";  //By MJ on 09/09/2022
        public const string GetAllAllocationCode = "Common/GetAllAllocationCode";  //By GK on 28/09/2022
        public const string GetAllLandType = "Common/GetAllLandType";  //By DP on 01/10/2022
        public const string GetAllOtherDebitsDetails = "Common/GetAllOtherDebitsDetails"; 
        public const string GetAllUomMaster = "Common/GetAllUomMaster";
        public const string GetAllConstitution = "Common/GetConstitution";
        public const string GetMeansofFinanceCategoryAsync = "Common/GetMeansofFinanceCategoryAsync";
        public const string GetAllDeptMaster = "Common/GetAllDeptMaster";
        public const string GetAllDsbChargeMaping = "Common/GetAllDsbChargeMaping";

        #endregion
    }
    public static class DisbursementRoute
    {
        #region Route for GET Method
        public const string GetAccountNumber = "Disbursement/GetAccountNumber";
        public const string GetAllConditionList = "Disbursement/GetAllDisbursementList?accountNumber=";
        public const string GetAllForm8AndForm13List = "Disbursement/GetAllForm8AndForm13List?accountNumber=";
        public const string GetAllSidbiApprovalDetails = "Disbursement/GetAllSidbiApprovalDetails?accountNumber=";
        public const string GetAllAdditionalCondition = "Disbursement/GetAllAdditionalConditionList?accountNumber=";
        public const string GetAllFirstInvestmentClauseDetails = "Disbursement/GetAllFirstInvestmentClauseDetails?accountNumber=";
        public const string GetAllOtherRelaxation = "Disbursement/GetAllOtherRelaxation?accountNumber=";

        #endregion
        #region Route for Delete Method
        public const string DeleteDisbursementConditionDetails = "Disbursement/DeleteDisbursementConditionDetails";
        public const string DeleteForm8AndForm13Details = "Disbursement/DeleteForm8AndForm13Details";
        public const string DeleteAdditionalConditionDetails = "Disbursement/DeleteAdditionalConditionDetails";

        #endregion
        #region Route for Update Method
        public const string UpdateDisbursementConditionDetails = "Disbursement/UpdateDisbursementConditionDetails";
        public const string UpdateAdditionalConditionDetails = "Disbursement/UpdateAdditonalConditionDetails";
        public const string UpdateForm8AndForm13Details = "Disbursement/UpdateForm8AndForm13Details";
        public const string UpdateFirstInvestmentClauseDetails = "Disbursement/UpdateFirstInvestmentClauseDetails";
        public const string UpdateSidbiApprovalDetails = "Disbursement/UpdateSidbiApprovalDetails";
        public const string UpdateOtherRelaxation = "Disbursement/UpdateOtherRelaxation";

        #endregion     
        #region Route for Create Method
        public const string CreateDisbursementConditionDetails = "Disbursement/CreateDisbursementConditionDetails";
        public const string CreateForm8AndForm13Details = "Disbursement/CreateForm8AndForm13Details";
        public const string CreateAdditionalConditionDetails = "Disbursement/CreateAdditionalConditionDetails";

        #endregion       
    }
    public static class AuditRoute
    {
        public const string GetAccountNumber = "Audit/GetAccountNumber";
        #region API for GET Routes
        public const string GetAllAuditClearnceList = "Audit/GetAllAuditClearanceList?accountNumber=";
        #endregion

        #region API for UPDATE Routes
        public const string UpdateAuditClearanceDetails = "Audit/UpdateAuditClearanceDetails";
        #endregion

        #region API for CREATE Routes
        public const string CreateAuditClearanceDetails = "Audit/CreateAuditClearanceDetails";
        #endregion

        #region API for DELETE Routes
        public const string DeleteAuditClearanceDetails = "Audit/DeleteAuditClearanceDetails";
        #endregion
    }

    public static class InspectionOfUnitRoute
    {
        #region Route for GET Method
        public const string GetAccountNumber = "InspectionOfUnit/GetAccountNumber";
        public const string GetAllLandInspectionList = "InspectionOfUnit/GetAllLandInspectionList?accountNumber=";
        public const string GetAllBuildingInspectionList = "InspectionOfUnit/GetAllBuildingInspectionList?accountNumber=";
        public const string GetAllInspectionDetailsList = "InspectionOfUnit/GetAllInspectionDetailsList?accountNumber=";
        public const string GetAllBuildingMaterialInspectionList = "InspectionOfUnit/GetAllBuildingMaterialInspectionList?accountNumber=";
        public const string GetAllFurnitureInspectionDetailsList = "InspectionOfUnit/GetAllFurnitureInspectionDetailsList?accountNumber=";
        public const string GetAllIndigenousMachineryInspectionList = "InspectionOfUnit/GetAllIndigenousMachineryInspectionList?accountNumber=";
        public const string GetAllImportMachineryInspectionList = "InspectionOfUnit/GetAllImportMachineryInspectionList?accountNumber=";
        public const string GetAllStatusofImplementationList  = "InspectionOfUnit/GetAllStatusofImplementationList?accountNumber=";
        public const string GetAllMeansOfFinanceList = "InspectionOfUnit/GetAllMeansOfFinanceList?accountNumber=";
        public const string GetAllLetterOfCreditList = "InspectionOfUnit/GetAllLetterOfCreditDetailList?accountNumber=";
        public const string GetAllProjectCostDetailsList = "InspectionOfUnit/GetAllProjectCostDetailsList?accountNumber=";
        public const string GetFinanceTypeAsync = "Common/GetAllFinanceType";
        public const string GetAllMachineryStatusList = "InspectionOfUnit/GetAllMachineryStatusList";
        public const string GetAllProcureList = "InspectionOfUnit/GetAllProcureList";
        public const string GetAllCurrencyList = "InspectionOfUnit/GetAllCurrencyList";

        #endregion

        #region API for UPDATE Routes
        public const string UpdateLandInspectionDetails = "InspectionOfUnit/UpdateLandInspectionDetails";
        public const string UpdateFurnitureInspectionDetails = "InspectionOfUnit/UpdateFurnitureInspectionDetails";
        public const string UpdateBuildingInspectionDetails = "InspectionOfUnit/UpdateBuildingInspectionDetails";
        public const string UpdateInspectionDetails = "InspectionOfUnit/UpdateInspectionDetails";
        public const string UpdateBuildMatSiteInspectionDetails = "InspectionOfUnit/UpdateBuildMatSiteInspectionDetails";
        public const string UpdatIndigenousMachineryInspectionDetails = "InspectionOfUnit/UpdateIndigenousMachineryInspectionDetails";
        public const string UpdateImportMachineryInspectionDetails = "InspectionOfUnit/UpdateImportMachineryInspectionDetails";
        public const string UpdateStatusofImplementationDetails = "InspectionOfUnit/UpdateStatusofImplementationDetails";
        public const string UpdateMeansOfFinanceDetails = "InspectionOfUnit/UpdateMeansOfFinanceDetails";
        public const string UpdateLetterOfCreditDetails = "InspectionOfUnit/UpdateLetterOfCreditDetails";
        public const string UpdateProjectCostDetails = "InspectionOfUnit/UpdateProjectCostDetails";
        #endregion

        #region API for CREATE Routes
        public const string CreateLandInspectionDetails = "InspectionOfUnit/CreateLandInspectionDetails";
        public const string CreateFurnitureInspectionDetails = "InspectionOfUnit/CreateFurnitureInspectionDetails";
        public const string CreateBuildingInspectionDetails = "InspectionOfUnit/CreateBuildingInspectionDetails";
        public const string CreateInspectionDetails = "InspectionOfUnit/CreateInspectionDetails";
        public const string CreateWorkingCapitalInspection = "InspectionOfUnit/CreateWorkingCapitalInspection";
        public const string CreateBuildMatSiteInspectionDetails = "InspectionOfUnit/CreateBuildMatSiteInspectionDetails";
        public const string CreateIndigenousMachineryInspectionDetails = "InspectionOfUnit/CreateIndigenousMachineryInspectionDetails";
        public const string CreateImportMachineryInspectionDetails = "InspectionOfUnit/CreateImportMachineryInspectionDetails";
        public const string CreateStatusofImplementationDetails = "InspectionOfUnit/CreateStatusofImplementationDetails";
        public const string CreateMeansOfFinanceDetails = "InspectionOfUnit/CreateMeansOfFinanceDetails";
        public const string CreateLetterOfCreditDetails = "InspectionOfUnit/CreateLetterOfCreditDetails";
        public const string CreateProjectCostDetails = "InspectionOfUnit/CreateProjectCostDetails";
        #endregion

        #region API for DELETE Routes
        public const string DeleteLandInspectionDetails = "InspectionOfUnit/DeleteLandInspectionDetails";
        public const string DeleteFurnitureInspectionDetails = "InspectionOfUnit/DeleteFurnitureInspectionDetails";
        public const string DeleteBuildMatSiteInspectionDetails = "InspectionOfUnit/DeleteBuildMatSiteInspectionDetails";
        public const string DeleteBuildingInspectionDetails = "InspectionOfUnit/DeleteBuildingInspectionDetails";
        public const string DeleteImportMachineryInspectionDetails = "InspectionOfUnit/DeleteImportMachineryInspectionDetails";
        public const string DeleteStatusofImplementationDetails  = "InspectionOfUnit/DeleteStatusofImplementationDetails";
        public const string DeleteInspectionDetails = "InspectionOfUnit/DeleteInspectionDetails";
        public const string DeleteIndigenousMachineryInspectionDetails = "InspectionOfUnit/DeleteIndigenousMachineryInspectionDetails";
        public const string DeleteMeansOfFinanceDetails = "InspectionOfUnit/DeleteMeansOfFinanceDetails";
        public const string DeleteLetterOfCreditDetails = "InspectionOfUnit/DeleteLetterOfCreditDetails";
        public const string DeleteProjectCostDetails = "InspectionOfUnit/DeleteProjectCostDetails";
        #endregion


    }
    public static class UnitDetailsRoute
    {
        #region Route for GET Method
        public const string GetAccountNumber = "UnitDetails/GetAccountNumber";
        public const string GetNameOfUnit = "UnitDetails/GetNameOfUnit?accountNumber=";
        public const string GetAllAddressDetails = "UnitDetails/GetAllAddressDetails?accountNumber=";
        public const string GetAllMasterPinCodeDetails = "UnitDetails/GetAllMasterPinCodeDetails";
        public const string GetAllPinCodeDistrictDetails = "UnitDetails/GetAllPinCodeDistrictDetails";
        public const string GetAllAssetTypeDetails = "UnitDetails/GetAllAssetTypeDetails";
        public const string GetAllAssetCategaryDetails = "UnitDetails/GetAllAssetCategaryDetails";
        public const string GetAllTalukDetails = "UnitDetails/GetAllTalukDetails";
        public const string GetAllHobliDetails = "UnitDetails/GetAllHobliDetails";
        public const string GetAllVillageDetails = "UnitDetails/GetAllVillageDetails";
        public const string GetAllMasterPromoterProfileDetails = "UnitDetails/GetAllMasterPromoterProfileDetails";
        public const string GetAllPromoterProfileDetails = "UnitDetails/GetAllPromoterProfileDetails?accountNumber=";
        public const string GetAllPromoAddressDetails = "UnitDetails/GetAllPromoAddressDetails?accountNumber=";
        public const string GetAllPromoterBankInfo = "UnitDetails/GetAllPromoterBankInfo?accountNumber=";
        public const string GetAllProductDetails = "UnitDetails/GetAllProductDetails?accountNumber=";
        public const string GetAllProductList = "UnitDetails/GetAllProductList";
        public const string GetAllChangeBankDetails = "UnitDetails/GetAllChangeBankDetails?accountNumber=";
        public const string GetAllIfscBankDetails = "UnitDetails/GetAllIfscBankDetails";
        public const string GetAllPromoterAssetDetails = "UnitDetails/GetAllPromoterAssetDetails?accountNumber=";        
        public const string GetAllPromoterLiabilityInfo = "UnitDetails/GetAllPromoterLiabiltyInfo?accountNumber=";
        public const string GetAllPromoterNetWorth = "UnitDetails/GetAllPromoterNetWorth?accountNumber=";
        public const string GetAllLandAssets = "UnitDetails/GetAllLandAssets?accountNumber=";


        #endregion

        #region API for UPDATE Routes
        public const string UpdateNameOfUnit = "UnitDetails/UpdateNameOfUnit";
        public const string UpdatePromoterProfileDetails = "UnitDetails/UpdatePromoterProfileDetails";
        public const string UpdatePromoterBankInfo = "UnitDetails/UpdatePromoterBankInfo";
        public const string UpdatePromAddressDetails = "UnitDetails/UpdatePromAddressDetails";
        public const string UpdateProductDetails = "UnitDetails/UpdateProductDetails";
        public const string UpdateChangeBankDetails = "UnitDetails/UpdateChangeBankDetails";
        public const string UpdateAssetDetails = "UnitDetails/UpdateAssetDetails";
        public const string UpdatePromoterLiabilityInfo = "UnitDetails/UpdatePromoterLiabilityInfo";  
        public const string UpdateAddressDetails = "UnitDetails/UpdateAddressDetails";
        public const string SaveLiabilityNetworthDetails = "UnitDetails/SaveLiabilityNetworthDetails";
        public const string SaveAssetNetworthDetails = "UnitDetails/SaveAssetNetworthDetails";
        #endregion

        #region API for DELETE Routes
        public const string DeleteAddressDetails = "UnitDetails/DeleteAddressDetails";
        public const string DeletePromAddressDetails = "UnitDetails/DeletePromAddressDetails";
        public const string DeletePromoterProfileDetails = "UnitDetails/DeletePromoterProfileDetails";
        public const string DeleteProductDetails = "UnitDetails/DeleteProductDetails";
        public const string DeleteChangeBankDetails = "UnitDetails/DeleteChangeBankDetails";
        public const string DeleteAssetDetails = "UnitDetails/DeleteAssetDetails";
        public const string DeletePromoterLiabilityInfo = "UnitDetails/DeletePromoterLiabilityInfo";
        public const string DeletePromoterBankInfo = "UnitDetails/DeletePromoterBankInfo";

        #endregion 

        #region API for CREATE Routes
        public const string CreatePromAddressDetails = "UnitDetails/CreatePromAddressDetails";
        public const string CreatePromoterProfileDetails = "UnitDetails/CreatePromoterProfileDetails";
        public const string CreateProductDetails = "UnitDetails/CreateProductDetails";
        public const string CreateChangeBankDetails = "UnitDetails/CreateChangeBankDetails";
        public const string CreateAssetDetails = "UnitDetails/CreateAssetDetails";
        public const string CreatePromoterLiabilityInfo = "UnitDetails/CreatePromoterLiabilityInfo";
        public const string CreatePromoterBankInfo = "UnitDetails/CreatePromoterBankInfo";
        public const string CreateLandAssetDetails = "UnitDetails/CreateLandAssetDetails";

        #endregion 
    }

    public static class GenericDDRoute
    {
        #region API for Generic Dropdowns Routes
        public const string GetAllConditionStageMaster = "Common/GetAllConditionStageMaster";
        public const string GetAllConditionStages = "Common/GetAllConditionStages";
        public const string GetAllConditionTypes = "Common/GetAllConditionTypes";
        public const string GetAllPromoterNames = "Common/GetAllPromoterNames"; 
        public const string GetAllPromoterPhNo = "Common/GetAllPromoterPhNo";
        public const string GetAllPromoterDistrict = "Common/GetAllPromoterDistrict";
        public const string GetAllPromoterState = "Common/GetAllPromoterState";
        public const string GetAllDistrictDetails = "Common/GetAllDistrictDetails";
        public const string GetAllTalukDetails = "Common/GetAllTalukDetails";
        public const string GetAllHobliDetails = "Common/GetAllHobliDetails";
        public const string GetAllVillageDetails = "Common/GetAllVillageDetails";
        public const string GetAllPincodeDetails = "Common/GetAllPincodeDetails";
        public const string GetPositionDesignation = "Common/GetPositionDesignation";
        public const string GetAllPromotorClass = "Common/GetAllPromotorClass";
        public const string GetDomicileStatus = "Common/GetDomicileStatus";
        public const string GetAllAccountType = "Common/GetAllAccountType";
        public const string GetAllAllocationCode = "Common/GetAllAllocationCode";
        public const string GetAllLandType = "Common/GetAllLandType";
        public const string GetAllOtherDebitsDetails = "Common/GetAllOtherDebitsDetails";
        public const string GetAllUomMasterDetails = "Common/GetAllUomMaster";
        public const string GetAllAssetList = "Common/GetAllAssetList";
       

        #endregion
    }

    #region Loan Accounting

    public static class LoanRelatedReceiptRoute
    {
        public const string GetAllAccountingLoanNumber = "LoanAccounting/GetAccountNumber?EmpId=";
        public const string GetCodetable = "LoanAccounting/GetCodetable?accountNumber=";

        #region Route for GET Method
        public const string GetAllGenerateReceiptList = "LoanRelatedReceipt/GetAllGenerateReceiptList?accountNumber=";
        public const string GetAllReceiptPaymentList = "LoanRelatedReceipt/GetAllReceiptPaymentList?accountNumber=";
        public const string GetAllReceiptList = "LoanRelatedReceipt/GetAllReceiptRefNum";
        public const string GetAllPaymentList = "LoanRelatedReceipt/GetAllPaymentRefNum";
        public const string GetAllReceiptPayments = "LoanRelatedReceiptProm/GetAllReceiptPayment?PaymentId=";

        #endregion

        #region Route for UPDATE Method
        public const string UpdateReceiptPaymentDetails = "LoanRelatedReceipt/UpdateReceiptPaymentDetails";
        public const string UpdateCreatePaymentDetails = "LoanRelatedReceipt/UpdateCreatePaymentDetails";
        public const string UpdateCreatePromPaymentDetails = "LoanRelatedReceiptProm/UpdateCreatePromPaymentDetails";
      
        #endregion

        #region Route for CREATE Method
        public const string CreateReceiptPaymentDetails = "LoanRelatedReceipt/CreateReceiptPaymentDetails";
        public const string CreatePaymentDetails = "LoanRelatedReceipt/CreatePaymentDetails";
        #endregion

        #region Route for DELETE  Method
        public const string DeleteReceiptPaymentDetails = "LoanRelatedReceipt/DeleteReceiptPaymentDetails";
        #endregion
        public const string ApproveReceiptPaymentDetails = "LoanRelatedReceipt/ApproveReceiptPaymentDetails";
        public const string RejectReceiptPaymentDetails = "LoanRelatedReceipt/RejectReceiptPaymentDetails";
    }

    public static class LoanAccountingGenericDDRoute
    {
        #region API for Generic Dropdowns Routes
        public const string GetAllTransactionTypes = "Common/GetAllTransactionTypes";
        #endregion
    }

    #endregion
    #region Loan Accounting Promoter

    public static class LoanRelatedReceiptPromRoute
    {
        public const string GetAllAccountingLoanNumber = "LoanAccountingPromoter/GetAccountNumber";
        #region Route for GET Method
        public const string GetAllGenerateReceiptPaymentList = "LoanRelatedReceiptProm/GetAllGenerateReceiptPaymentList?accountNumber=";
        #endregion

    }

    #endregion


    public static class CreationofSecurityandAcquisitionAssetRoute
    {
        #region Route for GET Method
        public const string GetAllCreationOfSecurityandAquisitionAssetList = "CreationOfSecurityandAquisitionAsset/GetAllCreationOfSecurityandAquisitionAssetList?accountNumber=";
        public const string GetAllMachineryAcquisitionDetails = "CreationOfSecurityandAquisitionAsset/GetAllMachineryAcquisitionDetails?accountNumber=";
        public const string GetAllBuildingAquisitionDetails = "CreationOfSecurityandAquisitionAsset/GetAllBuildingAcquisitionDetails?accountNumber=";

        public const string GetAllFurnitureAcquisitionList = "CreationOfSecurityandAquisitionAsset/GetAllFurnitureAcquisitionList?accountNumber=";
        #endregion

        #region API for Update Method

        public const string UpdateLandAcquisitionDetails = "CreationOfSecurityandAquisitionAsset/UpdateLandAcquisitionDetails";
        public const string UpdateMachineryAcquisitionDetails = "CreationOfSecurityandAquisitionAsset/UpdateMachineryAcquisitionDetails";
        public const string UpdateBuildingAcquisitionDetail = "CreationOfSecurityandAquisitionAsset/UpdateBuildingAcquisitionDetail";

        public const string UpdateFurnitureAcquisitionList = "CreationOfSecurityandAquisitionAsset/UpdateFurnitureAcquisition";
        #endregion

       
    }
    public static class LoanAllocationRoute
    {
        
        #region API for GET Routes
        public const string GetAllLoanAllocationList = "LoanAllocation/GetAllLoanAllocationList?accountNumber=";
        #endregion

        #region API for UPDATE Routes
        public const string UpdateLoanAllocationDetails = "LoanAllocation/UpdateLoanAllocationDetails";
        #endregion

        #region API for CREATE Routes
         public const string CreateLoanAllocationDetails = "LoanAllocation/CreateLoanAllocationDetails";
        #endregion

        #region API for DELETE Routes
         public const string DeleteLoanAllocationDetails = "LoanAllocation/DeleteLoanAllocationDetails";
        #endregion
    }
    public static class CreationOfDisbursmentProposalAssetRoute
    {
        #region Route for GET Method
        public const string GetAllRecomDisburseDetails = "CreationOfDisbursmentProposal/GetAllRecomDisbursementDetails?accountNumber=";
        public const string GetAllocationDetails = "CreationOfDisbursmentProposal/GetAllocationCodeDetails";
        public const string GetRecomDisbursementReleaseDetails = "CreationOfDisbursmentProposal/GetRecomDisbursementReleaseDetails";
        public const string GetAllProposalDetails = "CreationOfDisbursmentProposal/GetAllProposalDetails?accountNumber=";
        public const string GetAllBeneficiaryDetails = "CreationOfDisbursmentProposal/GetAllBeneficiaryDetails?accountNumber=";

        #endregion

        #region API for Update Method

        public const string UpdateRecomDisburseDetail = "CreationOfDisbursmentProposal/UpdateRecomDisbursementDetail";
        public const string UpdateBeneficiaryDetails = "CreationOfDisbursmentProposal/UpdateBeneficiaryDetails";
        public const string UpdateProposalDetail = "CreationOfDisbursmentProposal/UpdateProposalDetail";
        #endregion

        #region API for Delete Method

        public const string DeleteProposalDetail = "CreationOfDisbursmentProposal/DeleteProposalDetail";

        #endregion

        #region API for Create Method
        public const string CreateProposalDetail = "CreationOfDisbursmentProposal/CreateProposalDetail";
        #endregion
    }
    public static class OtherDebits
    {
        #region Route for GET Method
        public const string GetAllOtherDebitsList = "EntryOfOtherDebits/GetAllOtherDebitsList?accountNumber=";
        #endregion
        #region API for Update Method
        public const string UpdateOtherDebitDetails = "EntryOfOtherDebits/UpdateOtherDebitDetails";
        public const string DeleteOtherDebitDetails = "EntryOfOtherDebits/DeleteOtherDebitDetails";

        #endregion

        #region API for Create Method
        public const string CreateOtherDebitDetails = "EntryOfOtherDebits/CreateOtherDebitDetails";
        #endregion
        #region API for Submit Method
        public const string SubmitOtherDebitDetails = "EntryOfOtherDebits/SubmitOtherDebitDetails";
        #endregion
        
    }

}
    
