using Serilog.Sinks.SystemConsole.Themes;
using System.Globalization;
using System.Web;

namespace KAR.KSFC.UI.Helpers
{
    public static class Constants
    {
        #region DropDownLists
        public const string SessionDDListSecurityType = "SessionDDListSecurityType";
        public const string SessionDDListBankIFSCCode = "SessionDDListBankIFSCCode";
        public const string SessionDDListChargeType = "SessionDDListChargeType";
        public const string SessionDDListSecurityCategory = "SessionDDListSecurityCategory";
        public const string sessionAllPromoterProfile = "sessionAllPromoterProfile"; //Dev
        public const string SubmoduleId = "submoduleId";
        public const string SubmoduleType = "submoduleType";
        public const string MainModule = "mainModule";
        public const string StatusFail = "Fail";
        public const string LegalDocumentation = "LegalDocumentation";
        public const string AuditClearance = "AuditClearance";
        public const string DisbursementCondition = "DisbursementCondition";
        public const string InspectionOfUnit = "InspectionOfUnit"; 
        public const string ChangeOfUnit = "ChangeOfUnit";
        public const string PromoterDetails = "PromoterDetails";

        public const string SessionSetCondtionStage = "SessionSetCondtionStage";
        public const string sessionAllAdditionalCondition = "sessionAllAdditionalCondition";
        public const string SessionForm8andForm13Master = "SessionForm8andForm13Master";
        public const string Form8and13Condition = "Form8and13Condition";
        public const string SidbiApproval = "SidbiApproval";
        public const string PrimarySecurity = "PrimarySecurity";
        public const string ColletralSecurity = "ColletralSecurity";
        public const string Hypothecation = "Hypothecation";
        public const string SecurityCharge = "SecurityCharge";
        public const string InspectionDetail = "InspectionDetail";
        public const string GuarantorDeed = "GuarantorDeed";
        public const string Condition = "Condition";
        public const string CERSAI = "CERSAI";
        public const string SessionProductDetailsMaster = "SessionProductDetailsMaster";
        public const string SessionIndustryDetailsMaster = "SessionIndustryDetailsMaster";
        public const string SessionAssetTypeMaster = "SessionAssetTypeMaster";
        public const string SessionAssetCategaryMaster = "SessionAssetCategaryMaster";
        public const string SessionDDListOtherDebitCode = "SessionDDListOtherDebitCode";

        public const string SessionCodntionType = "SessionDDListConditionType"; 
        public const string SessionSetForm8AndForm13 = "SessionSetForm8AndForm13";
        public const string  SessionConditionStage= "SessionSetCondtionStage";
        public const string SessionConditionDesc = "SessionDDListConditionDescriptions";
        public const string SessionAllBuildingInspectionList = "SessionAllBuildingInspectionList";
        public const string SessionAllBuildMatSiteInspectionList = "SessionAllBuildMatSiteInspectionList";
        public const string SessionRegisteredState = "SessionRegisteredState";
        public const string SessionAllFurnitureInspectionDetail = "SessionAllFurnitureInspectionDetail";
        public const string SessionAllImportMachineryList = "SessionAllImportMachineryList";
        public const string SessionAllStatusofImplementationList = "SessionAllStatusofImplementationList";
        public const string SessionAllIndigenousMachineryInspectionList = "SessionAllIndigenousMachineryInspectionList";
        public const string SessionAllInspectionDetail = "SessionAllInspectionDetail";
        public const string SessionAllLandInspectionList = "SessionAllLandInspectionList";
        public const string SessionAllLetterOfCreditList = "SessionAllLetterOfCreditList";
        public const string SessionAllMeansOfFinanceList = "SessionAllMeansOfFinanceList";
        public const string SessionAllProjectCostList = "SessionAllProjectCostList";

        #endregion

        #region PaymentStatuses
        public const string NotPaid = "Not Paid";
        public const string PaymentInitiated = "Payment Initiated";
        public const string Paid = "Paid";
        #endregion

        #region ViewPaths
        public const string firstinvestmentViewPath = "~/Areas/Admin/Views/Disbursement/FirstInvestmentClause/";
        public const string cersairesultViewPath = "~/Areas/Admin/Views/LegalDocumentation/Cersai/";
        public const string cersaiviewPath = "../../Areas/Admin/Views/LegalDocumentation/Cersai/";
        public const string ViewRecord = "ViewRecord.cshtml";
        public const string ViewRegister = "ViewRegister.cshtml";
        public const string Register = "Register";
        public const string ViewRegistrationDetailsCs = "ViewRegistrationDetails.cshtml";
        public const string ViewRegistrationCs = "ViewRegistration.cshtml";
        public const string ViewCreate = "ViewCreate.cshtml";
        public const string ViewAll = "_ViewAll";
        public const string View = "_ViewAll.cshtml";
        //public const string EditAll = "_Edit";
        public const string EditAll = "Edit";
        public const string registerCs = "Register.cshtml";
        public const string Admin = "Admin";
        public const string sessionCersai = "SessionAllCERSAIList";
        public const string sessionAssetType = "SessionDDListAssetType";
        public const string conditionresultViewPath = "~/Areas/Admin/Views/LegalDocumentation/Condition/";
        public const string conditionviewPath = "../../Areas/Admin/Views/LegalDocumentation/Condition/";
        public const string Create = "Create";
        public const string Edit = "Edit";
        public const string ViewDeedCs = "ViewDeed.cshtml";
        public const string ViewDeedDetailsCs = "ViewDeedDetails.cshtml";
        public const string sessionConditionDes= "SessionDDListConditionDescriptions";
        public const string sessionConditonList = "SessionAllConditionList";
        public const string sessionCondtionStage = "SessionSetCondtionStage";
       public const string securityresultViewPath = "~/Areas/Admin/Views/LegalDocumentation/SecurityCharge/";
        public const string securityviewPath = "../../Areas/Admin/Views/LegalDocumentation/SecurityCharge/";
        public const string sessionConditonType = "SessionDDListConditionType";
        public const string createCS = "Create.cshtml";
        public const string editCs = "Edit.cshtml";
        public const string primaryresultViewPath = "~/Areas/Admin/Views/LegalDocumentation/PrimarySecurity/";
        public const string colletresultViewPath = "~/Areas/Admin/Views/LegalDocumentation/ColletralSecurity/";
        public const string primaryviewPath = "../../Areas/Admin/Views/LegalDocumentation/PrimarySecurity/";
        public const string colletviewPath = "../../Areas/Admin/Views/LegalDocumentation/ColletralSecurity/";
        public const string sessionsecurityCat = "SessionDDListSecurityCategory";
        public const string sessionsecurityType = "SessionDDListSecurityType";
        public const string hyporesultViewPath = "~/Areas/Admin/Views/LegalDocumentation/Hypothecation/";
        public const string hypoviewPath = "../../Areas/Admin/Views/LegalDocumentation/Hypothecation/";
        public const string sessionSubRegister = "SessionDDListSubRegisterOffice";
        public const string update = "Update.cshtml";
        public const string sessionHypothecation = "SessionAllHypothecationList";
        public const string guarantorresultViewPath = "~/Areas/Admin/Views/LegalDocumentation/GuarantorDeed/";
        public const string guarntorviewPath = "../../Areas/Admin/Views/LegalDocumentation/GuarantorDeed/";
        public const string client = "ksfcApi";
        public const string ContentType = "application/json";
        public const string documentviewPath = "../../Areas/Admin/Views/Shared/";
        public const string fileType = "application/pdf";
        public const string imageType = "image/jpeg";
        public const string UploadDoc = "_UploadDocument"; 
         public const string UploadDisbursmentDoc = "_UploadDisbursmentDocument";
        public const string UploadPhoto = "_UploadPhoto";
        public const string success = "success";
        public const string displaydoc = "DisplayDocument";
        public const string idmDoc = "IdmDocument";
        public const string Password = "password";
        public const string idmdocFile = "IDMDocument/DeleteDocument?documentUniqueId=";
        public const string mainModule = "&&mainModule=";
        public const string dollar = "$";
        public const string idmDocUpload = "IDMDocument/IDMFileUpload";
        public const string idmGetfile = "IDMDocument/GetEncryptedFileById?documentId=";
        public const string brackets = "[]";
        public const string fileuploaded = "File uploaded successfully";
        public const string errorOccuredfileUpload = "Some Unknown Error occured. This will be resolved soon.";
        public const string getproductName = "Common/GetProductName";
        public const string getIndustry = "Common/GetAllIndustrytype";
        public const string getAssetType = "Common/GetAllAssetTypes";
        public const string getAssetCat= "Common/GetAssetCategoryAsync";
        public const string getFileList = "IDMDocument/GetAllDocumentList?MainModule=";
        public const string chnglocationresultViewPath = "~/Areas/Admin/Views/UnitDetails/ChangeLocationDetails/";
        public const string chnglocationviewPath = "../../Areas/Admin/Views/UnitDetails/ChangeLocationDetails/";
        public const string sessionDistrict = "DDLSessionAllDistrictDetails";
        public const string sessionTaluk = "DDLSessionAllTalukDetails";
        public const string sessionHobli = "DDLSessionAllHobliDetails";
        public const string sessionVillage = "DDLSessionAllVillageDetails";
        public const string sessionPincode= "DDLSessionAllPincodeDetails";
        public const string registerViewAll = "_ViewAllRegistered";
        public const string officeViewAll = "_ViewAllOffice";
        public const string factoryViewAll = "_ViewAllFactory";
        public const string chngbankresultViewPath = "~/Areas/Admin/Views/ChangeBankDetails/";
        public const string chngbankViewPath = "../../Areas/Admin/Views/ChangeBankDetails/";
        public const string sessionchngBankDetails = "SessionAllChangeBankDetailsList";
        public const string chngprodresultViewPath = "~/Areas/Admin/Views/UnitDetails/Product/";
        public const string chngprodViewPath = "../../Areas/Admin/Views/UnitDetails/Product/";
        public const string sessionchngProductDetails = "SessionAllProductDetailsList";
        public const string chngassetresultViewPath = "~/Areas/Admin/Views/UnitDetails/AssetDetails/";
        public const string chngassetViewPath = "../../Areas/Admin/Views/UnitDetails/AssetDetails/";
        public const string sessionPromoterNames = "DDLSessionPromoterNames";
        public const string sessionPromPincode = "DDLSessionPromoterPincode"; 

        public const string sessionAllAssets = "SessionAllAssetDetailsList";
        public const string promaddrresultViewPath = "~/Areas/Admin/Views/UnitDetails/ChangePromoterAddress/";
        public const string promaddrviewPath = "../../Areas/Admin/Views/UnitDetails/ChangePromoterAddress/";
        public const string sessionPromState = "DDLSessionPromoterState";
        public const string sessionPromDistrict = "DDLSessionPromoterDistrict";
        public const string sessionPromAddress = "SessionSetPromoterAddress";
        public const string promBankresultViewPath = "~/Areas/Admin/Views/PromoterBankInfo/";
        public const string promBankviewPath = "../../Areas/Admin/Views/PromoterBankInfo/";
        public const string sessionPromBank = "SessionSetPromoterBank";
        public const string promDetresultViewPath = "~/Areas/Admin/Views/PromoterDetails/";
        public const string promDetviewPath = "../../Areas/Admin/Views/PromoterDetails/";
        public const string sessionPromProfile = "SessionSetPromoterProfile";
        public const string promLiabresultViewPath = "~/Areas/Admin/Views/PromoterLiabilityInformation/";
        public const string promLiabviewPath = "../../Areas/Admin/Views/PromoterLiabilityInformation/";
        public const string sessionPromLiability = "SessionSetPromoterLiabilityInfo";
        public const string promNWresultViewPath = "~/Areas/Admin/Views/PromoterNetWorth/";
        public const string promNWviewPath = "../../Areas/Admin/Views/PromoterNetWorth/";
        public const string LandAcqresultViewPath = "~/Areas/Admin/Views/CreationOfSecurityandAquisitionAsset/LandAcquisition/";
        public const string LandAcqviewPath = "../../Areas/Admin/Views/CreationOfSecurityandAquisitionAsset/LandAcquisition/";
        public const string sessionAllLandAcquisition = "sessionAllLandAcquisition";
        public const string BuildingAcqresultViewPath = "~/Areas/Admin/Views/CreationOfSecurityandAquisitionAsset/BuildingAcquisition/";
        public const string BuildingAcqviewPath = "../../Areas/Admin/Views/CreationOfSecurityandAquisitionAsset/BuildingAcquisition/";
        public const string sessionAllBuildingAcquisition = "SessionAllBuildingAcquisitionDetail";
        public const string MachineAcqresultViewPath = "~/Areas/Admin/Views/CreationOfSecurityandAquisitionAsset/MachineryAcquisition/";
        public const string MachineAcqviewPath = "../../Areas/Admin/Views/CreationOfSecurityandAquisitionAsset/MachineryAcquisition/";
        public const string sessionAllMachineryAcquisition = "sessionAllMachineryAcquisition";
        public const string FurnitureAcqresultViewPath = "~/Areas/Admin/Views/CreationOfSecurityandAquisitionAsset/FurnitureAcquisition/";
        public const string FurnitureAcqviewPath = "../../Areas/Admin/Views/CreationOfSecurityandAquisitionAsset/FurnitureAcquisition/";
        public const string sessionAllFurnitureAcquisition = "FurnitureAcquisitionList";


        public const string generatereceiptresultViewPath = "~/Areas/Admin/Views/LoanRelatedReceipt/GenerateReceipt/";
        public const string generatereceiptViewPath = "../../Areas/Admin/Views/LoanRelatedReceipt/GenerateReceipt/";
        public const string SessionAllCodeTableTransactionTypeList = "SessionAllCodeTableTransactionTypeList";
        public const string SessionAllCodeTableModeofPaymentList = "SessionAllCodeTableModeofPaymentList";
        public const string SessionAllReceiptPaymentList = "SessionAllReceiptPaymentList";
        public const string LAFDApplicantfeedeposit = "LAFD(Applicant fee deposit)";
        public const string RegistrationCharges = "Registration Charges";
        public const string PenaltyCharges = "Penalty Charges";
        public const string Recoveryofexcesspaymentfromsuppliers = "Recovery of excess payment from suppliers";
        public const string CersaiCharges = "Cersai Charges";
        public const string InsuranceCharges = "Insurance Charges";
        public const string LA = "LA";
        public const string RC = "RC";
        public const string PC = "PC";
        public const string RE = "RE";
        public const string CC = "CC";
        public const string IC = "IC";
        public const string PYT = "PYT";
        public const string TransactionType = "TransactionType";
        public const string ModeOfPayment = "ModeOfPayment";
        public const string ViewPaymentRecord = "ViewPaymentRecord.cshtml";
        public const string PayNow = "PayNow.cshtml";
        public const string AddPayNow = "AddPayNow.cshtml";

        public const string CreatePayment = "CreatePayment.cshtml";
        public const string ViewCreatedRecord = "ViewCreatedRecord.cshtml";
        public const string EditCreated = "EditCreated.cshtml";
        public const string SavedReceiptresultViewPath = "~/Areas/Admin/Views/LoanRelatedReceipt/SavedReceipt/";
        public const string SavedReceiptviewPath = "../../Areas/Admin/Views/LoanRelatedReceipt/SavedReceipt/";
        public const string LoanAccount = "_LoanAccount";
        public const string LoanSub = "_LoanSub";
        public const string UnitName = "_UnitName";
        public const string Customer = "Customer";
        public const string promoterloanreceiptresultViewPath = "~/Areas/Customer/Views/LoanRelatedReceiptProm/LoanRelatedReceipts/";
        public const string promoterloanreceiptviewPath = "../../Areas/Customer/Views/LoanRelatedReceiptProm/LoanRelatedReceipts/";
        public const string promoterresultViewPathP = "~/Areas/Customer/Views/LoanRelatedReceiptProm/LoanRelatedPayments/";

        public const string loanAllocationresultViewPath = "~/Areas/Admin/Views/LoanAllocation/";
        public const string loanAllocationviewPath = "../../Areas/Admin/Views/LoanAllocation/";
        public const string inspectionDetailresultViewPath = "~/Areas/Admin/Views/InpsectionDetail/";
        public const string inspectionDetailviewPath = "../../Areas/Admin/Views/InpsectionDetail/";
        public const string letterOfCreditresultViewPath = "~/Areas/Admin/Views/LetterOfCredit/";
        public const string letterOfCreditviewPath = "../../Areas/Admin/Views/LetterOfCredit/";
        public const string projectCostresultViewPath = "~/Areas/Admin/Views/ProjectCost/";
        public const string projectCostviewPath = "../../Areas/Admin/Views/ProjectCost/";
        public const string MOFresultViewPath = "~/Areas/Admin/Views/MeansOfFinance/";
        public const string MOFviewPath = "../../Areas/Admin/Views/MeansOfFinance/";
        public const string LandresultViewPath = "~/Areas/Admin/Views/LandInspection/";
        public const string LandresultViewPathAd = "~/Areas/Admin/Views/AssetDetails/LandInspection/";
        public const string LandviewPath = "../../Areas/Admin/Views/LandInspection/";
        public const string LandviewPathAd = "../../Areas/Admin/Views/AssetDetails/LandInspection/";
        public const string BuildingInspectionresultViewPath = "~/Areas/Admin/Views/BuildingInspection/";
        public const string BuildingInspectionresultViewPathAd = "~/Areas/Admin/Views/AssetDetails/BuildingInspection/";
        public const string StatusofImplementationViewPath = "~/Areas/Admin/Views/StatusofImplementation/";
        public const string BuildingInspectionviewPath = "../../Areas/Admin/Views/BuildingInspection/";
        public const string BuildingInspectionviewPathAd = "../../Areas/Admin/Views/AssetDetails/BuildingInspection/";
        public const string BuildingMatresultViewPath = "~/Areas/Admin/Views/BuildMatSiteInspection/";
        public const string BuildingMatviewPath = "../../Areas/Admin/Views/BuildMatSiteInspection/";
        public const string FurnitureresultViewPath = "~/Areas/Admin/Views/FurnitureInspection/";
        public const string FurnitureresultViewPathAd = "~/Areas/Admin/Views/AssetDetails/FurnitureInspection/";
        public const string FurnitureviewPath = "../../Areas/Admin/Views/FurnitureInspection/";
        public const string FurnitureviewPathAd = "../../Areas/Admin/Views/AssetDetails/FurnitureInspection/";
        public const string IndigenousresultViewPath = "~/Areas/Admin/Views/IndigenousMachinery/";
        public const string IndigenousresultViewPathAd = "~/Areas/Admin/Views/AssetDetails/IndigenousMachinery/";
        public const string IndigenousviewPath = "../../Areas/Admin/Views/IndigenousMachinery/";
        public const string IndigenousviewPathAd = "../../Areas/Admin/Views/AssetDetails/IndigenousMachinery/";
        public const string ImportresultViewPath = "~/Areas/Admin/Views/ImportMachinery/";
        public const string ImportresultViewPathAd = "~/Areas/Admin/Views/AssetDetails/ImportMachinery/";
        public const string ImportviewPath = "../../Areas/Admin/Views/ImportMachinery/";
        public const string ImportviewPathAd = "../../Areas/Admin/Views/AssetDetails/ImportMachinery/";
        public const string DisbursementConditionresultViewPath = "~/Areas/Admin/Views/Disbursement/DisbursementCondition/";
        public const string DisbursementConditionviewPath = "../../Areas/Admin/Views/Disbursement/DisbursementCondition/";
        public const string AdditionalConditionresultViewPath = "~/Areas/Admin/Views/Disbursement/AdditionalCondition/";
        public const string AdditionalConditionviewPath = "../../Areas/Admin/Views/Disbursement/AdditionalCondition/";
        public const string Form8AndForm13resultViewPath = "~/Areas/Admin/Views/Disbursement/Form8AndForm13/";
        public const string Form8AndForm13viewPath = "../../Areas/Admin/Views/Disbursement/Form8AndForm13/";
        public const string SidbiresultViewPath = "~/Areas/Admin/Views/Disbursement/Sidbi_Approval/";
        public const string SidbiviewPath = "../../Areas/Admin/Views/Disbursement/Sidbi_Approval/";
        public const string StatusImpPath = "../../Areas/Admin/Views/StatusofImplementation/"; 

        //Audit Clearence
        public const string auditResultViewPath = "~/Areas/Admin/Views/Audit/AuditClearance/";
        public const string auditViewPath = "../../Areas/Admin/Views/Audit/AuditClearance/";
        public const string sessionAuditClearance = "SessionAllAuditClearanceList";
        public const string OtherDebitsResultViewPath = "~/Areas/Admin/Views/EntryOfOtherDebits/OtherDebits/";
        public const string OtherDebitsviewPath = "../../Areas/Admin/Views/EntryOfOtherDebits/OtherDebits/";
        #endregion

        #region Cloumns Details
        public const string PincodeRowId = "PincodeRowId";
        public const string PincodeCd = "PincodeCd";
        public const string TlqCd = "TlqCd";
        public const string TlqNam = "TlqNam";
        public const string DistCd = "DistCd";
        public const string DistNam = "DistNam";
        public const string PincodeDistrictCd = "PincodeDistrictCd";
        public const string PincodeDistrictDesc = "PincodeDistrictDesc";
        public const string AssettypeCd = "AssettypeCd";
        public const string AssettypeDets = "AssettypeDets";

        public const string HobCd = "HobCd";
        public const string HobNam = "HobNam";
        public const string VilCd = "VilCd";
        public const string VilNam = "VilNam";
        #endregion

    
      
        #region LegalDocumentation
        public const string GetAllPrimaryCollateralList = "Started calling GetAllPrimaryCollateralList()";
        public const string GetAllCollateralList = "Started calling GetAllCollateralList()";
        public const string GetAllHypothecationList = "Started calling GetAllHypothecationList()";
        public const string GetAllSecurityChargeList = "Started calling GetAllSecurityChargeList()";
        public const string GetAllCERSAIList = "Started calling GetAllCERSAIList()";
        public const string GetAllPromotorNames = "Started calling GetAllPromotorNames()";
        public const string GetAllConditionStages = "Started calling GetAllConditionStages()";
        public const string GetAllConditionList = "Started calling GetAllConditionList()";
        public const string GetAllAssetRefList = "Started calling GetAllAssetRefList()";
        public const string DeleteHypothecationDetail = "Started calling DeleteHypothecationDetail()";
        public const string CreateHypothecationDetails = "Started calling CreateHypothecationDetails()";
        public const string UpdateSecurityCharge = "Started calling UpdateSecurityCharge()";
        public const string GetAllGuarantorDeedList = "Started calling GetAllGuarantorDeedList()";
        public const string UpdateLDGuarantorDeedDetails = "Started calling UpdateLDGuarantorDeedDetails()";
        public const string DeleteLDConditionDetails = "Started calling DeleteLDConditionDetails()";
        public const string CreateLDConditionDetails = "Started calling CreateLDConditionDetails()";
        public const string UpdateLDConditionDetails = "Started calling UpdateLDConditionDetails()";
        public const string DeleteLDCersaiDetails = "Started calling DeleteLDCersaiDetails()";
        public const string CreateLDCersaiDetails = "Started calling CreateLDCersaiDetails()";
        public const string FileList = "Started calling FileList()";
        public const string UpdatePrimaryCollateralDetails = "Started calling UpdatePrimaryCollateralDetails()";
        public const string UpdateCollateralDetails = "Started calling UpdateCollateralDetails()";
        public const string CreateCollateralDetails = "Started calling CreateeCollateralDetails()";
        public const string UpdateHypothecationDetails = "Started calling UpdateHypothecationDetails()";
        public const string LegalDocumentationviewAccount = "Started ViewAccount Method for Legal Documentation Module";
        #endregion

        #region AuditClearence
        public const string GetAllAuditClearanceList = "Started calling GetAllAuditClearanceList()";
        #endregion

        #region Disbursment
        public const string DisbursmentviewAccount = "Started ViewAccount Method for Disbursment Module";
        public const string GetAllConditionTypes = "Started calling GetAllConditionTypes()";
        public const string GetAllDisbursementConditionList = "Started calling GetAllDisbursementConditionList()";
        public const string GetAllFirstInvestmentClauseDetails = "Started calling GetAllFirstInvestmentClauseDetails()";
        public const string GetAllAdditionalConditonlist = "Started calling GetAllAdditionalConditonlist()";
        public const string GetAllForm8AndForm13List = "Started calling GetAllForm8AndForm13List()";
        public const string CompletedDisbursmentviewAccount = "Completed ViewAccount Method for Disbursment Module";
        public const string UpdateFIC = "Started Updating FirstInvestment Clause";
        public const string CompletedUpdateFIC = "Completed Updating FirstInvestment Clause";
        public const string SaveSidbiApprovalDetails = "Started Saveing Sidbi Approval Details";
        public const string CompletedSaveSidbiApprovalDetails = "Completed Saveing Sidbi Approval Details";
        public const string GetAllLoanNumber = "Started calling GetAllLoanNumber() For Disdursment Condetion module";
        public const string UpdateDisbursementConditionDetails = "Started calling UpdateDisbursementConditionDetails()";
        public const string DeleteDisbursementConditionDetails = "Started calling DeleteDisbursementConditionDetails()";
        public const string CreateDisbursementConditionDetails = "Started calling CreateDisbursementConditionDetails()";
        public const string GetAllSidbiApprovalDetails = "Started calling GetAllSidbiApprovalDetails()";
        public const string UpdateSidbiApprovalDetails = "Started calling UpdateSidbiApprovalDetails()";
        public const string UpdateForm8AndForm13Details = "Started calling UpdateForm8AndForm13Details()";
        public const string DeleteForm8AndForm13Details = "Started calling DeleteForm8AndForm13Details()";
        public const string CreateForm8AndForm13Details = "Started calling CreateForm8AndForm13Details()";
        public const string CreateAdditionalConditionDetails = "Started calling CreateAdditionalConditionDetails()";
        public const string UpdateAdditionalConditionDetails = "Started calling UpdateAdditionalConditionDetails()";
        public const string DeleteAdditionalConditionDetails = "Started calling DeleteAdditionalConditionDetails()";
        public const string UpdateFirstInvestmentClauseDetails = "Started calling UpdateFirstInvestmentClauseDetails()";
        public const string GetAllOtherRelaxation = "Started calling GetAllOtherRelaxation()";
        public const string UpdateOtherRelaxation = "Started calling UpdateOtherRelaxation()";

        #endregion

        #region CreationofSecurityandAcquisition
        public const string GetAllInspectionDetailsList = "Started calling GetAllInspectionDetailsList()";
        public const string GetAllIdmDropDownList = "Started calling GetAllIdmDropDownList()";
        public const string GetAllCreationOfSecurityandAquisitionAssetList = "Started calling GetAllCreationOfSecurityandAquisitionAssetList()";
        public const string GetAllMachineryAcquisitionDetails = "Started calling GetAllMachineryAcquisitionDetails()";
        public const string GetAllBuildingAcquisitionDetails = "Started calling GetAllBuildingAcquisitionDetails()";
        public const string GetFurnitureAcquisitionList = "Started calling GetFurnitureAcquisitionList()";
        public const string CompletedViewAccount = "Completed View Account for Creation of Security and Acquisition";
        public const string UpdateLandAcquisitionDetails = "Started calling UpdateLandAcquisitionDetails()";
        public const string UpdateMachineryAcquisitionDetails = "Started calling UpdateMachineryAcquisitionDetails()";
        public const string UpdateFurnitureAcquisition = "Started calling UpdateFurnitureAcquisition()";
        public const string UpdateBuildingAcquisitionDetail = "Started calling UpdateBuildingAcquisitionDetail()";

        #endregion

        #region CreationofDisbursmentProposal
        public const string ViewAccountCreationofSecurityandAcquisition = "Started calling ViewAccount Creation of Security and Acquisition";
        public const string GetAllocationCodeDetails = "Started calling GetAllocationCodeDetails()";
        public const string GetRecomDisbursementReleaseDetails = "Started calling GetRecomDisbursementReleaseDetails()";
        public const string GetAllRecomDisbursementDetails = "Started calling GetAllRecomDisbursementDetails()";
        public const string GetAllProposalDetails = "Started calling GetAllProposalDetails()";
        public const string ViewAccountCreationofSecurityandAcquisitionCompleted = "Completed calling ViewAccount Creation of Security and Acquisition";
        public const string GetAllBeneficiaryDetails = "Started calling GetAllBeneficiaryDetails()";
        public const string ViewAccountGetAllBeneficiaryDetailsCompleted = "Completed calling ViewAccount BeneficiaryDetails";

        public const string UpdateRecomDisbursementDetail = "Started calling UpdateRecomDisbursementDetail()";
        public const string UpdateProposalDetail = "Started calling UpdateProposalDetail()";
        public const string DeleteProposalDetail = "Started calling DeleteProposalDetail()";
        public const string CreateProposalDetail = "Started calling CreateProposalDetail()";
        public const string UpdateBeneficiaryDetails = "Started calling UpdateBeneficiaryDetails()";

        #endregion

        #region Entry of other debits
        public const string GetAllOtherDebitsList = "Started calling GetAllOtherDebitsList()";
        public const string UpdateOtherDebitDetails = "Started calling UpdateOtherDebitDetails()";
        public const string DeleteOtherDebitDetails = "Started calling DeleteOtherDebitDetails()";
        public const string CreateOtherDebitDetails = "Started calling CreateOtherDebitDetails()";
        public const string SubmitOtherDebitDetails = "Started calling SubmitOtherDebitDetails()";
        public const string CompletedViewotherdebits = "Completed ViewAccount For Entry Of Other Debits ";
        public const string StarteddViewotherdebits = "Started ViewAccount For Entry Of Other Debits ";

        #endregion
    }
}
