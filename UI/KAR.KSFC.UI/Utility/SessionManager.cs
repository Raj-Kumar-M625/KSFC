using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Common.Dto.IDM;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;

namespace KAR.KSFC.UI.Utility
{
    public class SessionManager
    {
        const string SessionCustMobile = "CustomerMobile";
        const string SessionCustUserName = "CustomerUsername";
        const string SessionCustToken = "JWTToken";



        const string SessionCustRefToken = "JWTRefToken";
        const string SessionCustLoginTime = "CustomerLoginTime";
        const string SessionCustLoginDateTime = "CustomerLoginDateTime";

        const string SessionAllConstitutionTypes = "AllConstitutionTypes";
        const string SessionAddressTypesFromDB = "AddressTypesFromDB";
        const string SessionAddressList = "AddressList";
        const string SessionRegistrationDetList = "RegistrationDetList";
        const string SessionAssociateDetailsDTOList = "AssociateDetailsDTOList";
        const string SessionAssociatePrevFYDetailsList = "AssociatePrevFYDetailsList";
        const string SessionGuarantorDetailsList = "GuarantorDetailsList";
        const string SessionGuarantorAssetList = "GuarantorAssetList";
        const string SessionGuarantorLiabilityList = "SessionGuarantorLiabilityList";
        const string SessionGuarantorNetWorthList = "SessionGuarantorNetWorthList";
        const string SessionUDPersonalDetails = "SessionUDPersonalDetails";
        const string SessionUDBankDetails = "SessionUDBankDetails";
        const string SessionPromoterDetailsList = "SessionPromoterDetailsList";



        const string SessionPromoterAssetList = "SessionPromoterAssetList"; 
        const string SessionPromoterLiabilityList = "SessionPromoterLiabilityList";
        const string SessionPromoterNetWorthList = "SessionPromoterNetWorthList";
        const string SessionWorkingCapitalArrDetails = "SessionWorkingCapitalArrDetails";
        const string SessionProjectCostList = "SessionProjectCostList";
        const string SessionProjectMeansOfFinanceList = "SessionProjectMeansOfFinanceList";
        const string SessionProjectPrevFYDetailsList = "SessionProjectPrevFYDetailsList";
        const string SessionSecurityDetailsList = "SessionSecurityDetailsList";
        const string SessionDeclarationDetails = "SessionDeclarationDetails";
        const string SessionDocumentsDetails = "SessionDocumentsDetails";

        // IDM - Legal Documentation
        const string SessionAllLoanNumberList = "SessionAllLoanNumberList";
        const string SessionAllAccountDetails = "SessionAllAccountDetails";
        const string SessionAllInspectionLoanNumberList = "SessionAllInspectionLoanNumberList";
        const string SessionAllSecurityHolderList = "SessionAllSecurityHolderList";
        const string SessionAllHypothecationList = "SessionAllHypothecationList";
        const string SessionAllSecurityChargeList = "SessionAllSecurityChargeList";
        const string SessionAllCERSAIList = "SessionAllCERSAIList";
        const string SessionAllGuarantorDeedList = "SessionAllGuarantorDeedList";
        const string SessionAllConditionList = "SessionAllConditionList";   // By Dev on 05/08/2022
        const string SessionAllAssetRefList = "SessionAllAssetRefList";   // By MJ on 11/08/2022
        const string SessionSetCondtionStage = "SessionSetCondtionStage";
        const string SessionSetCondtionStageMaster = "SessionSetCondtionStageMaster";
        const string SessionDDListConditionDescriptions = "SessionDDListConditionDescriptions"; // By MJ on 17/08/2022
        const string SessionDDListConditionType = "SessionDDListConditionType"; // By MJ on 17/08/2022
        const string SessionDDListAssetType = "SessionDDListAssetType"; // By MJ on 17/08/2022
        const string SessionDDListSecurityType = "SessionDDListSecurityType"; // By MJ on 17/08/2022
        const string SessionDDListSecurityCategory = "SessionDDListSecurityCategory"; // By MJ on 17/08/2022
        const string SessionDDListChargeType = "SessionDDListChargeType"; // By MJ on 17/08/2022
        const string SessionDDListSubRegisterOffice = "SessionDDListSubRegisterOffice"; // By MJ on 17/08/2022
        const string SessionDDListBankIFSCCode = "SessionDDListBankIFSCCode"; // By MJ on 17/08/2022
        const string SessionIDMDocument = "SessionIDMDocument";
        const string SessionForm8andForm13Master = "SessionForm8andForm13Master";
        const string SessionProductDetailsMaster = "SessionProductDetailsMaster";
        const string SessionIndustryDetailsMaster = "SessionIndustryDetailsMaster";
        const string SessionAssetTypeMaster = "SessionAssetTypeMaster";
        const string SessionAssetCategaryMaster = "SessionAssetCategaryMaster";
        const string SessionAssetTypeDetails = "SessionAssetTypeDetails";
        const string SessionAssetCategaryDetails = "SessionAssetCategaryDetails";
        const string SessionRegisteredState = "SessionRegisteredState";



        //Disbursement 
        const string SessionSetForm8AndForm13 = "SessionSetForm8AndForm13";
        const string sessionAllAdditionalCondition = "sessionAllAdditionalCondition";
        const string sessionAllFirstInvestmentClause = "sessionAllFirstInvestmentClause";// By AD on 23/08/2022
        const string sessionAllSidbiApproval = "sessionAllSidbiApproval"; // Dev
        const string SessionAllOtherRelaxation = "SessionAllOtherRelaxation";

        // IDM - AuditClearance
        const string SessionAllAuditClearanceList = "SessionAllAuditClearanceList";

        // IDM - Unit Details
        const string SessionAllAddressDetailsList = "SessionAllAddressDetailsList";
        const string SessionAllMasterPincodeDetails = "SessionAllMasterPincodeDetails";
        const string SessionAllPincodeDistrictDetails = "SessionAllPincodeDistrictDetails";
        //const string SessionAllAssetTypeDetails = "SessionAllAssetTypeDetails";
        //const string sessionAllAssetCatCDTab = "sessionAllAssetCatCDTab";
        const string SessionAllPincodeStateDetails = "SessionAllPincodeStateDetails";
        const string SessionAllTalukDetails = "SessionAllTalukDetails";
        const string SessionAllDistrictDetails = "SessionAllDistrictDetails";
        const string SessionAllHobliDetails = "SessionAllHobliDetails";
        const string SessionAllVillageDetails = "SessionAllVillageDetails";
        const string SessionSetPromoterProfile = "SessionSetPromoterProfile";        
        const string SessionSetMasterPromoterProfile = "SessionSetMasterPromoterProfile";        
        const string SessionSetPromoterAddress = "SessionSetPromoterAddress";
        const string SessionSetPromoterLiabilityInfo = "SessionSetPromoterLiabilityInfo";
        const string SessionSetPromoterNetWorth = "SessionSetPromoterNetWorth";
        const string DDLSessionPromoterState = "DDLSessionPromoterState";
        const string DDLSessionPromoterDistrict = "DDLSessionPromoterDistrict";
        const string DDLSessionPromoterPincode = "DDLSessionPromoterPincode";
        const string DDLSessionPromoterNames = "DDLSessionPromoterNames";
        const string SessionSetPromoterBank = "SessionSetPromoterBank";
        const string DDLSessionAllDistrictDetails = "DDLSessionAllDistrictDetails";
        const string DDLSessionAllStateDetails = "DDLSessionAllStateDetails";
        const string DDLSessionAllTalukDetails = "DDLSessionAllTalukDetails";
        const string DDLSessionAllHobliDetails = "DDLSessionAllHobliDetails";
        const string DDLSessionAllVillageDetails = "DDLSessionAllVillageDetails";
        const string DDLSessionAllPincodeDetails = "DDLSessionAllPincodeDetails";
        const string SessionAllChangeBankDetailsList = "SessionAllChangeBankDetailsList";
        const string SessionAllIfscBankDetailsList = "SessionAllIfscBankDetailsList";
        const string SessionAllAssetTypeDetails = " SessionAllAssetTypeDetails";
        const string SessionAllAssetCategaryDetails = " SessionAllAssetCategaryDetails";
        const string SessionAllLandAssetDetails = "SessionAllLandAssetDetails";

        //DDL
        //ddlRegTypes
        const string SessionDDLBankFacilityType = "DDListBankFacilityType";
        const string SessionDDListFinancialYear = "DDListFinancialYear";
        const string SessionDDListFinancialComponent = "DDListFinancialComponent";
        const string SessionDDListDomicileStatus = "DDListDomicileStatus";//ListDomicileStatus
        const string SessionDDListTypeOfAccount = "SessionDDListTypeOfAccount";//ListTypeOfAccount   // edited by Dev
        const string SessionDDListPromAndGuarAssetType = "DDListPromAndGuarAssetType";//ListPromAndGuarAssetType
        const string SessionDDListPromAndGuarAssetCategory = "DDListPromAndGuarAssetCategory";//ListPromAndGuarAssetCategory
        const string SessionDDListModeOfAcquire = "DDListModeOfAcquire";//ListModeOfAcquire
        const string SessionDDListPromoterDesignation = "DDListPromoterDesignation";
        const string SessionDDListProjectCostComponent = "DDListProjectCostComponent";
        const string SessionDDListProjectMeansOfFinance = "DDListProjectMeansOfFinance";
        const string SessionDDListProjectFinanceType = "DDListProjectFinanceType";
        const string SessionDDListTypeOfSecurity = "DDListTypeOfSecurity";
        const string SessionDDListSecurityDetailsType = "DDListSecurityDetailsType";
        const string SessionDDListRelationType = "DDListRelationType";
        const string SessionDDListPromoterClass = "SessionDDListPromoterClass"; // By Dev on 01/09/2022
        const string SessionDDListPromoterSubClass = "SessionDDListPromoterSubClass"; // By Dev on 03/09/2022
        const string SessionDDListPromoterQualification = "SessionDDListPromoterQualification"; // By Dev on 03/09/2022
        const string SessionDDListAllocationCode = "SessionDDListAllocationCode"; 
        const string SessionDDListOtherDebitCode = "SessionDDListOtherDebitCode";
        const string SessionEmpAccesableRoles = "AccesableRoles";
        const string SessionNewEnqTempId = "NewEnqTempId";
        const string SessionIpAddress = "ClientIpAddress";
        const string SessionOperation = "Operation";
        const string SessionAllProductDetailsList = "SessionAllProductDetailsList";
        const string SessionAllProductList = "SessionAllProductList";

        const string SessionAllAssetDetailsList = "SessionAllAssetDetailsList";
        const string SessionDDLMasterList = "SessionDDLMasterList";
        const string SessionDDLConstutionMasterList = "SessionDDLConstutionMasterList";

        

        //Inspection Of Unit
        const string SessionAllLandInspectionList = "SessionAllLandInspectionList"; // by MJ on 25/08/2022
        const string SessionAllBuildingInspectionList = "SessionAllBuildingInspectionList"; // by Swetha on 25/08/2022
        const string SessionWorkingCapitalInspection = "SessionWorkingCapitalInspection";//by Swetha 30/08/2022
        const string SessionStatusofImplementation = "SessionStatusofImplementation"; //by SR 24/04/2023

        const string SessionAllBuildMatSiteInspectionList = "SessionAllBuildMatSiteInspectionList"; // by MJ on 29/08/2022
        const string SessionAllInspectionDetail= "SessionAllInspectionDetail"; // by Sandeep M on 25/08/2022
        const string SessionInspectionDetail = "SessionInspectionDetail"; // by Sandeep M on 25/08/2022

        const string SessionAllFurnitureInspectionDetail= "SessionAllFurnitureInspectionDetail"; // by Sandeep M on 25/08/2022
        const string SessionAllIndigenousMachineryInspectionList = "SessionAllIndigenousMachineryInspectionList"; // by MJ on 29/08/2022
        const string SessionAllMachinaryStatusList = "SessionAllMachinaryStatusList";
        const string SessionAllProcureList = "SessionAllProcureList";
        const string SessionAllCurrencyList = "SessionAllCurrencyList";
        const string SessionAllImportMachineryList = "SessionAllImportMachineryList"; // by Swetha on 25/08/2022
        const string SessionAllMeansOfFinanceList = "SessionAllMeansOfFinanceList"; // by Swetha on 25/08/2022
        const string SessionAllLetterOfCreditList = "SessionAllLetterOfCreditList"; // by MJ on 05/09/2022
        const string SessionAllProjectCostList = "SessionAllProjectCostList"; // by AD on 05/09/2022
        const string SessionDDListprojectCostComponentDetail = "SessionDDListprojectCostComponentDetail"; // by MJ on 07/09/2022
        const string SessionDDLofFinanceCategory = "SessionDDLofFinanceCategory"; // by MJ on 07/09/2022


        // Creation of Security and Acquisition of Asset
        const string sessionAllMachineryAcquisition = "sessionAllMachineryAcquisition"; // By Dev on 28/09/2022
        const string sessionAllLandAcquisition = "sessionAllLandAcquisition"; // By Dev on 29/09/2022
        const string SessionAllBuildingAcquisitionDetail = "SessionAllBuildingAcquisitionDetail"; // by Ram on 01/10/2022

        const string SessionDDListLandType = "SessionDDListLandType"; // By Dev on 01/10/2022

        // IDM - LoanAllocation 
        const string SessionAllLoanAllocationList = "SessionAllLoanAllocationList";

        // Furniture Acquisition
        const string FurnitureAcquisitionList = "FurnitureAcquisitionList";

        // Loan Accounting
        const string SessionAllAccountingLoanNumberList = "SessionAllAccountingLoanNumberList"; // by GK on 15/09/2022

        //Loan Relatetd Receipt
        const string SessionAllReceiptPaymentList = "SessionAllReceiptPaymentList";
        const string SessionAllCodeTableList = "SessionAllCodeTableList";
        const string SessionAllCodeTableTransactionTypeList = "SessionAllCodeTableTransactionTypeList";
        const string SessionAllCodeTableModeofPaymentList = "SessionAllCodeTableModeofPaymentList";
        const string SessionAllGenerateReceiptPaymentList = "SessionAllGenerateReceiptPaymentList"; // By DP on 23/09/2022

        // Loan Accounting- Dropdowns
        const string SessionDDListAllTransactionTypes = "SessionDDListAllTransactionTypes"; //by GK on 15/09/2022

        // Recommended Disbursement Details
        const string SessionAllRecomDisburseList = "SessionAllRecomDisburseList"; // by Ram on 07/10/2022

        //Disbursement proposal details 
        const string SessionAllProposalDetailsList = "SessionAllProposalDetailsList";

        const string SessionAllOtherDebitsList = "SessionAllOtherDebitsList";

        const string SessionDDListDeptMaster = "SessionDDListDeptMaster";

        const string SessionDDListDsbChargeMap = "SessionDDListDsbChargeMap";


        private readonly ISession _Session;
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _Session = httpContextAccessor.HttpContext.Session;
        }

        public void SetAllEntitiesToEmptyString()
        {
            _Session.SetString(SessionAddressList, string.Empty);
            _Session.SetString(SessionRegistrationDetList, string.Empty);
            _Session.SetString(SessionAssociateDetailsDTOList, string.Empty);
            _Session.SetString(SessionAssociatePrevFYDetailsList, string.Empty);
            _Session.SetString(SessionGuarantorDetailsList, string.Empty);
            _Session.SetString(SessionGuarantorAssetList, string.Empty);
            _Session.SetString(SessionGuarantorLiabilityList, string.Empty);
            _Session.SetString(SessionGuarantorNetWorthList, string.Empty);
            _Session.SetString(SessionUDPersonalDetails, string.Empty);
            _Session.SetString(SessionUDBankDetails, string.Empty);
            _Session.SetString(SessionPromoterDetailsList, string.Empty);
            _Session.SetString(SessionPromoterAssetList, string.Empty);
            _Session.SetString(SessionPromoterLiabilityList, string.Empty);
            _Session.SetString(SessionPromoterNetWorthList, string.Empty);
            _Session.SetString(SessionProjectCostList, string.Empty);
            _Session.SetString(SessionProjectMeansOfFinanceList, string.Empty);
            _Session.SetString(SessionProjectPrevFYDetailsList, string.Empty);
            _Session.SetString(SessionSecurityDetailsList, string.Empty);
            _Session.SetString(SessionDeclarationDetails, string.Empty);
            _Session.SetString(SessionDocumentsDetails, string.Empty);

        }

        public void SetOperationType(string operationType)
        {
            _Session.SetString(SessionOperation, operationType);
        }
        public void SetLoginCustUserName(string panNumber)
        {
            _Session.SetString(SessionCustUserName, panNumber);
        }
       
        public void SetLoginCustToken(string token)
        {
            _Session.SetString(SessionCustToken, token);
        }

        public void SetLoginCustMobile(string mobile)
        {
            _Session.SetString(SessionCustMobile, mobile);
        }

        public void SetLoginCustRefToken(string token)
        {
            _Session.SetString(SessionCustRefToken, token);
        }
        public string GetLoginCustRefToken()
        {
            return _Session.GetString(SessionCustRefToken);
        }
        public string GetOperationType()
        {
            return _Session.GetString(SessionOperation);
        }

        public string GetLoginCustToken()
        {
            return _Session.GetString(SessionCustToken);
        }

        public string GetCustMobile()
        {
            return _Session.GetString(SessionCustMobile);
        }
        public void ClearSessionData()
        {
            _Session.Clear();
        }
        public string GetLoginCustUserName()
        {
            return _Session.GetString(SessionCustUserName);
        }

        public void SetNewEnqTempId(string newEnqTempId)
        {
            _Session.SetString(SessionNewEnqTempId, newEnqTempId);
        }

        public string GetNewEnqTempId()
        {
            return _Session.GetString(SessionNewEnqTempId);
        }

        public void SetCustLoginTime(string token)
        {
            _Session.SetString(SessionCustLoginTime, DateTime.Now.ToString("HH:mm"));
        }
        public string GetCustLoginTime()
        {
            return _Session.GetString(SessionCustLoginTime);
        }
        public void SetCustLoginDateTime(string token)
        {
            _Session.SetString(SessionCustLoginDateTime, DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
        }
        public string GetCustLoginDateTime()
        {
            return _Session.GetString(SessionCustLoginDateTime);
        }

        //AddressList
        public void SetAllConstitutionTypes(List<CnstCdtab> allConstitutionTypes)
        {
            _Session.SetString(SessionAllConstitutionTypes, JsonConvert.SerializeObject(allConstitutionTypes));
        }
        public List<CnstCdtab> GetAllConstitutionTypes()
        {
            return JsonConvert.DeserializeObject<List<CnstCdtab>>(_Session.GetString(SessionAllConstitutionTypes));
        }
        //SessionAddressTypesFromDB
        public void SetAddressTypesFromDB(List<SelectListItem> addressTypesFromDB)
        {
            _Session.SetString(SessionAddressTypesFromDB, JsonConvert.SerializeObject(addressTypesFromDB));
        }


        public List<SelectListItem> GetAddressTypesFromDB()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionAddressTypesFromDB));
        }

        //AddressList
        public void SetAddressList(List<AddressDetailsDTO> addressDTO)
        {
            _Session.SetString(SessionAddressList, JsonConvert.SerializeObject(addressDTO));
        }
        public List<AddressDetailsDTO> GetAddressList()
        {
            return JsonConvert.DeserializeObject<List<AddressDetailsDTO>>(_Session.GetString(SessionAddressList));
        }

        //RegistrationDetList
        public void SetRegistrationDetList(List<RegistrationNoDetailsDTO> registrationDetails)
        {
            _Session.SetString(SessionRegistrationDetList, JsonConvert.SerializeObject(registrationDetails));
        }
        public List<RegistrationNoDetailsDTO> GetRegistrationDetList()
        {
            return JsonConvert.DeserializeObject<List<RegistrationNoDetailsDTO>>(_Session.GetString(SessionRegistrationDetList));
        }
        //SessionAssociateDetailsDTOList
        public void SetAssociateDetailsDTOList(List<SisterConcernDetailsDTO> associateDetailsList)
        {
            _Session.SetString(SessionAssociateDetailsDTOList, JsonConvert.SerializeObject(associateDetailsList));
        }
        public List<SisterConcernDetailsDTO> GetAssociateDetailsDTOList()
        {
            return JsonConvert.DeserializeObject<List<SisterConcernDetailsDTO>>(_Session.GetString(SessionAssociateDetailsDTOList));
        }

        //SessionAssociatePrevFYDetailsList
        public void SetAssociatePrevFYDetailsList(List<SisterConcernFinancialDetailsDTO> associatePrevFYDetailsList)
        {
            _Session.SetString(SessionAssociatePrevFYDetailsList, JsonConvert.SerializeObject(associatePrevFYDetailsList));
        }
        public List<SisterConcernFinancialDetailsDTO> GetAssociatePrevFYDetailsList()
        {
            return JsonConvert.DeserializeObject<List<SisterConcernFinancialDetailsDTO>>(_Session.GetString(SessionAssociatePrevFYDetailsList));
        }
        //GuarantorDetailsList
        public void SetGuarantorDetailsList(List<GuarantorDetailsDTO> guarantorDetails)
        {
            _Session.SetString(SessionGuarantorDetailsList, JsonConvert.SerializeObject(guarantorDetails));
        }
        public List<GuarantorDetailsDTO> GetGuarantorDetailsList()
        {
            return JsonConvert.DeserializeObject<List<GuarantorDetailsDTO>>(_Session.GetString(SessionGuarantorDetailsList));
        }
        //GuarantorAssetList
        public void SetGuarantorAssetList(List<GuarantorAssetsNetWorthDTO> assetDetails)
        {

            _Session.SetString(SessionGuarantorAssetList, JsonConvert.SerializeObject(assetDetails));
        }
        public List<GuarantorAssetsNetWorthDTO> GetGuarantorAssetList()
        {
            return JsonConvert.DeserializeObject<List<GuarantorAssetsNetWorthDTO>>(_Session.GetString(SessionGuarantorAssetList));
        }

        //GuarantorLiabilityList
        public void SetGuarantorLiabilityList(List<GuarantorLiabilityDetailsDTO> liabilityDetails)
        {
            _Session.SetString(SessionGuarantorLiabilityList, JsonConvert.SerializeObject(liabilityDetails));
        }
        public List<GuarantorLiabilityDetailsDTO> GetGuarantorLiabilityList()
        {
            return JsonConvert.DeserializeObject<List<GuarantorLiabilityDetailsDTO>>(_Session.GetString(SessionGuarantorLiabilityList));
        }
        //GuarantorNetWorthList
        public void SetGuarantorNetWorthList(List<GuarantorNetWorthDetailsDTO> netWorths)
        {
            _Session.SetString(SessionGuarantorNetWorthList, JsonConvert.SerializeObject(netWorths));
        }
        public List<GuarantorNetWorthDetailsDTO> GetGuarantorNetWorthList()
        {
            return JsonConvert.DeserializeObject<List<GuarantorNetWorthDetailsDTO>>(_Session.GetString(SessionGuarantorNetWorthList));
        }
        //UDPersonalDetails
        public void SetUDPersonalDetails(BasicDetailsDto basicDetailsDTO)
        {
            _Session.SetString(SessionUDPersonalDetails, JsonConvert.SerializeObject(basicDetailsDTO));
        }

        public BasicDetailsDto GetUDPersonalDetails()
        {
            return JsonConvert.DeserializeObject<BasicDetailsDto>(_Session.GetString(SessionUDPersonalDetails));
        }

        //UDBankDetails
        public void SetUDBankDetails(BankDetailsDTO bankDetailsDTO)
        {
            _Session.SetString(SessionUDBankDetails, JsonConvert.SerializeObject(bankDetailsDTO));
        }
        public BankDetailsDTO GetUDBankDetails()
        {
            return JsonConvert.DeserializeObject<BankDetailsDTO>(_Session.GetString(SessionUDBankDetails));
        }
        //PromoterDetailsList
        public void SetPromoterDetailsList(List<PromoterDetailsDTO> promoterDetails)
        {
            _Session.SetString(SessionPromoterDetailsList, JsonConvert.SerializeObject(promoterDetails));
        }
        public List<PromoterDetailsDTO> GetPromoterDetailsList()
        {
            return JsonConvert.DeserializeObject<List<PromoterDetailsDTO>>(_Session.GetString(SessionPromoterDetailsList));
        }
        //PromoterAssetList
        public void SetPromoterAssetList(List<PromoterAssetsNetWorthDTO> assetDetails)
        {
            _Session.SetString(SessionPromoterAssetList, JsonConvert.SerializeObject(assetDetails));
        }

        public List<PromoterAssetsNetWorthDTO> GetPromoterAssetList()
        {
            return JsonConvert.DeserializeObject<List<PromoterAssetsNetWorthDTO>>(_Session.GetString(SessionPromoterAssetList));
        }
        //PromoterLiabilityList
        public void SetPromoterLiabilityList(List<PromoterLiabilityDetailsDTO> liabilityDetails)
        {
            _Session.SetString(SessionPromoterLiabilityList, JsonConvert.SerializeObject(liabilityDetails));
        }
        public List<PromoterLiabilityDetailsDTO> GetPromoterLiabilityList()
        {
            return JsonConvert.DeserializeObject<List<PromoterLiabilityDetailsDTO>>(_Session.GetString(SessionPromoterLiabilityList));
        }
        //PromoterNetWorthList
        public void SetPromoterNetWorthList(List<PromoterNetWorthDetailsDTO> netWorths)
        {
            _Session.SetString(SessionPromoterNetWorthList, JsonConvert.SerializeObject(netWorths));
        }
        public List<PromoterNetWorthDetailsDTO> GetPromoterNetWorthList()
        {
            return JsonConvert.DeserializeObject<List<PromoterNetWorthDetailsDTO>>(_Session.GetString(SessionPromoterNetWorthList));
        }

        //ProjectWorkingCapitalArrangementDetails
        public void SetProjectWorkingCapitalArrDetails(ProjectWorkingCapitalDeatailsDTO workingCapArrDetails)
        {
            _Session.SetString(SessionWorkingCapitalArrDetails, JsonConvert.SerializeObject(workingCapArrDetails));
        }
        public ProjectWorkingCapitalDeatailsDTO GetProjectWorkingCapitalArrDetails()
        {
            return JsonConvert.DeserializeObject<ProjectWorkingCapitalDeatailsDTO>(_Session.GetString(SessionWorkingCapitalArrDetails));
        }
        //ProjectCostList
        public void SetProjectCostList(List<ProjectCostDetailsDTO> projectCosts)
        {
            _Session.SetString(SessionProjectCostList, JsonConvert.SerializeObject(projectCosts));
        }
        public List<ProjectCostDetailsDTO> GetProjectCostList()
        {
            return JsonConvert.DeserializeObject<List<ProjectCostDetailsDTO>>(_Session.GetString(SessionProjectCostList));
        }
        //ProjectMeansOfFinanceList
        public void SetProjectMeansOfFinanceList(List<ProjectMeansOfFinanceDTO> meansOfFinances)
        {
            _Session.SetString(SessionProjectMeansOfFinanceList, JsonConvert.SerializeObject(meansOfFinances));
        }
        public List<ProjectMeansOfFinanceDTO> GetProjectMeansOfFinanceList()
        {
            return JsonConvert.DeserializeObject<List<ProjectMeansOfFinanceDTO>>(_Session.GetString(SessionProjectMeansOfFinanceList));
        }
        //ProjectPrevFYDetailsList
        public void SetProjectPrevFYDetailsList(List<ProjectFinancialYearDetailsDTO> projectPrevFYDetails)
        {
            _Session.SetString(SessionProjectPrevFYDetailsList, JsonConvert.SerializeObject(projectPrevFYDetails));
        }
        public List<ProjectFinancialYearDetailsDTO> GetProjectPrevFYDetailsList()
        {
            return JsonConvert.DeserializeObject<List<ProjectFinancialYearDetailsDTO>>(_Session.GetString(SessionProjectPrevFYDetailsList));
        }
        //SecurityDetailsList
        public void SetSecurityDetailsList(List<SecurityDetailsDTO> securityDetails)
        {
            _Session.SetString(SessionSecurityDetailsList, JsonConvert.SerializeObject(securityDetails));
        }
        public List<SecurityDetailsDTO> GetSecurityDetailsList()
        {
            return JsonConvert.DeserializeObject<List<SecurityDetailsDTO>>(_Session.GetString(SessionSecurityDetailsList));
        }
        public void SetDeclaration(Declaration declarationDetails)
        {
            _Session.SetString(SessionDeclarationDetails, JsonConvert.SerializeObject(declarationDetails));
        }
        public Declaration GetDeclaration()
        {
            return JsonConvert.DeserializeObject<Declaration>(_Session.GetString(SessionDeclarationDetails));
        }
        public void SetDocuments(List<EnqDocumentDTO> documentType)
        {
            _Session.SetString(SessionDocumentsDetails, JsonConvert.SerializeObject(documentType));
        }
        public List<EnqDocumentDTO> GetDocuments()
        {
            return JsonConvert.DeserializeObject<List<EnqDocumentDTO>>(_Session.GetString(SessionDocumentsDetails));
        }
        public void SetDDLBankFacilityType(List<SelectListItem> bankFacilityTypes)
        {
            _Session.SetString(SessionDDLBankFacilityType, JsonConvert.SerializeObject(bankFacilityTypes));
        }
        public List<SelectListItem> GetDDLBankFacilityType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDLBankFacilityType));
        }
        public void SetDDListFinancialYear(List<SelectListItem> financialYears)
        {
            _Session.SetString(SessionDDListFinancialYear, JsonConvert.SerializeObject(financialYears));
        }
        public List<SelectListItem> GetDDListFinancialYear()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListFinancialYear));
        }
        public void SetDDListFinancialComponent(List<SelectListItem> financialComponents)
        {
            _Session.SetString(SessionDDListFinancialComponent, JsonConvert.SerializeObject(financialComponents));
        }
        public List<SelectListItem> GetDDListFinancialComponent()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListFinancialComponent));
        }
        public void SetDDListDomicileStatus(List<SelectListItem> domicileStatuses)
        {
            _Session.SetString(SessionDDListDomicileStatus, JsonConvert.SerializeObject(domicileStatuses));
        }
        public List<SelectListItem> GetDDListDomicileStatus()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListDomicileStatus));
        }
       public void SetDDListPromAndGuarAssetType(List<SelectListItem> assetTypes)
        {
            _Session.SetString(SessionDDListPromAndGuarAssetType, JsonConvert.SerializeObject(assetTypes));
        }
        public List<SelectListItem> GetDDListPromAndGuarAssetType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListPromAndGuarAssetType));
        }
        public void SetDDListPromAndGuarAssetCategory(List<SelectListItem> assetCategories)
        {
            _Session.SetString(SessionDDListPromAndGuarAssetCategory, JsonConvert.SerializeObject(assetCategories));
        }
        public List<SelectListItem> GetDDListPromAndGuarAssetCategory()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListPromAndGuarAssetCategory));
        }
        public void SetDDListModeOfAcquire(List<SelectListItem> assetAcquires)
        {
            _Session.SetString(SessionDDListModeOfAcquire, JsonConvert.SerializeObject(assetAcquires));
        }
        public List<SelectListItem> GetDDListModeOfAcquire()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListModeOfAcquire));
        }
        //SessionDDListPromoterDesignation
        public void SetDDListPromoterDesignation(List<SelectListItem> promDesnTypes)
        {
            _Session.SetString(SessionDDListPromoterDesignation, JsonConvert.SerializeObject(promDesnTypes));
        }
        public List<SelectListItem> GetDDListPromoterDesignation()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListPromoterDesignation));
        }

        public void SetDDListProjectCostComponent(List<SelectListItem> projectCostComponents)
        {
            _Session.SetString(SessionDDListProjectCostComponent, JsonConvert.SerializeObject(projectCostComponents));
        }
        public List<SelectListItem> GetDDListProjectCostComponent()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListProjectCostComponent));
        }
        public void SetDDListProjectMeansOfFinance(List<SelectListItem> meansOfFinances)
        {
            _Session.SetString(SessionDDListProjectMeansOfFinance, JsonConvert.SerializeObject(meansOfFinances));
        }
        public List<SelectListItem> GetDDListProjectMeansOfFinance()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListProjectMeansOfFinance));
        }
        public void SetDDListProjectFinanceType(List<SelectListItem> projectFinanceTypes)
        {
            _Session.SetString(SessionDDListProjectFinanceType, JsonConvert.SerializeObject(projectFinanceTypes));
        }
        public List<SelectListItem> GetDDListProjectFinanceType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListProjectFinanceType));
        }
        public void SetDDListTypeOfSecurity(List<SelectListItem> typeOfSecurities)
        {
            _Session.SetString(SessionDDListTypeOfSecurity, JsonConvert.SerializeObject(typeOfSecurities));
        }
        public List<SelectListItem> GetDDListTypeOfSecurity()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListTypeOfSecurity));
        }
        public void SetDDListSecurityDetailsType(List<SelectListItem> securityDetailtypes)
        {
            _Session.SetString(SessionDDListSecurityDetailsType, JsonConvert.SerializeObject(securityDetailtypes));
        }
        public List<SelectListItem> GetDDListSecurityDetailsType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListSecurityDetailsType));
        }
        public void SetDDListRelationType(List<SelectListItem> relationTypes)
        {
            _Session.SetString(SessionDDListRelationType, JsonConvert.SerializeObject(relationTypes));
        }
        public List<SelectListItem> GetDDListRelationType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListRelationType));
        }
        public void SetIpAddress(string IpAddress)
        {
            _Session.SetString(SessionIpAddress, JsonConvert.SerializeObject(IpAddress));
        }
        public string GetIpAddress()
        {
            return JsonConvert.DeserializeObject<string>(_Session.GetString(SessionIpAddress));
        }
        public void SetEmployeeAccesableRoles(List<string> Roles)
        {
            _Session.SetString(SessionEmpAccesableRoles, JsonConvert.SerializeObject(Roles));
        }
        public List<string> GetEmployeeAccesableRoles()
        {
            return JsonConvert.DeserializeObject<List<string>>(_Session.GetString(SessionEmpAccesableRoles));
        }

        public void SetAllLoanNumber(List<LoanAccountNumberDTO> loans)
        {
            _Session.SetString(SessionAllLoanNumberList, JsonConvert.SerializeObject(loans));
        }
        public void SetAllInspectionLoanNumber(List<LoanAccountNumberDTO> loans)
        {
            _Session.SetString(SessionAllInspectionLoanNumberList, JsonConvert.SerializeObject(loans));
        }

        public void SetAccountDetails(List<TblUnitDet> accountDetails)
        {
            _Session.SetString(SessionAllAccountDetails, JsonConvert.SerializeObject(accountDetails));
        }

        public List<TblUnitDet> GetAllAccountDetails()
        {
            return JsonConvert.DeserializeObject<List<TblUnitDet>>(_Session.GetString(SessionAllAccountDetails));
        }
        public List<LoanAccountNumberDTO> GetAllLoanNumber()
        {
            return JsonConvert.DeserializeObject<List<LoanAccountNumberDTO>>(_Session.GetString(SessionAllLoanNumberList));
        }
        public List<LoanAccountNumberDTO> GetAllInspectionLoanNumber()
        {
            return JsonConvert.DeserializeObject<List<LoanAccountNumberDTO>>(_Session.GetString(SessionAllInspectionLoanNumberList));
        }

       
        public void SetPrimaryCollateralList(IEnumerable<IdmSecurityDetailsDTO> allSecurityHolderList)
        {
            _Session.SetString(SessionAllSecurityHolderList, JsonConvert.SerializeObject(allSecurityHolderList));
        }

        public List<IdmSecurityDetailsDTO> GetPrimaryCollateralList()
        {
            return JsonConvert.DeserializeObject<List<IdmSecurityDetailsDTO>>(_Session.GetString(SessionAllSecurityHolderList));
        }


        public void SetCollateralList(IEnumerable<IdmSecurityDetailsDTO> allSecurityHolderList)
        {
            _Session.SetString(SessionAllSecurityHolderList, JsonConvert.SerializeObject(allSecurityHolderList));
        }

        public List<IdmSecurityDetailsDTO>GetCollateralList()
        {
            return JsonConvert.DeserializeObject<List<IdmSecurityDetailsDTO>>(_Session.GetString(SessionAllSecurityHolderList));
        }



        // Author: Gagana K; Module: Hypothecation; Date:21/07/2022
        public void SetHypothecationList(IEnumerable<HypoAssetDetailDTO> allHypothecationList)
        {
            _Session.SetString(SessionAllHypothecationList, JsonConvert.SerializeObject(allHypothecationList));
        }
        // Author: Gagana K; Module: SecurityCharge; Date:21/07/2022
        public void SetSecurityChargeList(IEnumerable<IdmSecurityChargeDTO> allSecurityChargeList)
        {
            _Session.SetString(SessionAllSecurityChargeList, JsonConvert.SerializeObject(allSecurityChargeList));
        }
        // Author: Gagana K; Module: CERSAI; Date:31/07/2022
        public void SetCERSAIList(IEnumerable<IdmCersaiRegDetailsDTO> allCERSAIList)
        {
            _Session.SetString(SessionAllCERSAIList, JsonConvert.SerializeObject(allCERSAIList));
        }
        // Author: Manoj BJ; Module Guarantor Deed; Date:03/08/2022
        public void SetGuarantorDeedList(IEnumerable<IdmGuarantorDeedDetailsDTO> allGuarantorDeedList)
        {
            _Session.SetString(SessionAllGuarantorDeedList, JsonConvert.SerializeObject(allGuarantorDeedList));
        }
      
        // Author: Gagana K; Module: Hypothecation; Date:21/07/2022
        public List<HypoAssetDetailDTO> GetAllHypothecationList()
        {
            return JsonConvert.DeserializeObject<List<HypoAssetDetailDTO>>(_Session.GetString(SessionAllHypothecationList));
        }
        // Author: Gagana K; Module: SecurityCharge; Date:21/07/2022
        public List<IdmSecurityChargeDTO> GetAllSecurityChargeList()
        {
            return JsonConvert.DeserializeObject<List<IdmSecurityChargeDTO>>(_Session.GetString(SessionAllSecurityChargeList));
        }
        // Author: Gagana K; Module: CERSAI; Date:31/07/2022
        public List<IdmCersaiRegDetailsDTO> GetAllCERSAIList()
        {
            return JsonConvert.DeserializeObject<List<IdmCersaiRegDetailsDTO>>(_Session.GetString(SessionAllCERSAIList));
        }
        // Author: Manoj BJ; Module Guarantor Deed; Date:03/08/2022
        public List<IdmGuarantorDeedDetailsDTO> GetAllGuarantorDeedList()
        {
            return JsonConvert.DeserializeObject<List<IdmGuarantorDeedDetailsDTO>>(_Session.GetString(SessionAllGuarantorDeedList));
        }

        // Author: Manoj BJ; Module Guarantor Deed; Date:11/08/2022
        public void SetAllAssetRefList(IEnumerable<AssetRefnoDetailsDTO> allAssetRefList)
        {
            _Session.SetString(SessionAllAssetRefList, JsonConvert.SerializeObject(allAssetRefList));
        }
        // Author: Manoj BJ; Module Guarantor Deed; Date:11/08/2022
        public List<AssetRefnoDetailsDTO> GetAllAssetRefList()
        {
            return JsonConvert.DeserializeObject<List<AssetRefnoDetailsDTO>>(_Session.GetString(SessionAllAssetRefList));
        }

        // Author: Dev; Module: Condition; Date:05/08/2022
        public void SetConditionList(IEnumerable<LDConditionDetailsDTO> allConditionList)
        {
            _Session.SetString(SessionAllConditionList, JsonConvert.SerializeObject(allConditionList));
        }

        public void SetAdditionalConditionList(IEnumerable<AdditionConditionDetailsDTO> allAdditionalCondition)
        {
            _Session.SetString(sessionAllAdditionalCondition, JsonConvert.SerializeObject(allAdditionalCondition));
        }

        //Author: Akhiladevi D M; Module : Condition; Date:05/08/2022
        public List<LDConditionDetailsDTO> GetAllConditionList()
        {
            return JsonConvert.DeserializeObject<List<LDConditionDetailsDTO>>(_Session.GetString(SessionAllConditionList));
        }

        public List<AdditionConditionDetailsDTO> GetAllAdditionalConditionList()
        {
            return JsonConvert.DeserializeObject<List<AdditionConditionDetailsDTO>>(_Session.GetString(sessionAllAdditionalCondition));
        }


        public void SetCondtionStage(List<SelectListItem> lstConditionStages)
        {
            _Session.SetString(SessionSetCondtionStage, JsonConvert.SerializeObject(lstConditionStages));
        }

        public void SetCondtionStageMaster(List<SelectListItem> lstConditionStageMaster)
        {
            _Session.SetString(SessionSetCondtionStageMaster, JsonConvert.SerializeObject(lstConditionStageMaster));
        }



        public List<SelectListItem> GetAllCondtionStage()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionSetCondtionStage));
        }


        public List<SelectListItem> GetAllCondtionStageMaster()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionSetCondtionStageMaster));
        }
        // Manoj on 17/08/2022
        public void SetConditionDescriptionDDL(List<SelectListItem> conditionDescriptions)
        {
            _Session.SetString(SessionDDListConditionDescriptions, JsonConvert.SerializeObject(conditionDescriptions));
        }
        public void SetConditionTypeDDL(List<SelectListItem> conditionTyp)
        {
            _Session.SetString(SessionDDListConditionType, JsonConvert.SerializeObject(conditionTyp));
        }

        public void SetRegisteredStateDDL(List<SelectListItem> registeredstate)
        {
            _Session.SetString(SessionRegisteredState, JsonConvert.SerializeObject(registeredstate));
        }

        public void SetAssetTypeDDL(List<SelectListItem> assetType)
        {
            _Session.SetString(SessionDDListAssetType, JsonConvert.SerializeObject(assetType));
        }

        public void SetIDMDocument(List<ldDocumentDto> idmFileUploadDtos)
        {
            _Session.SetString(SessionIDMDocument, JsonConvert.SerializeObject(idmFileUploadDtos));
        }

        public List<ldDocumentDto> GetIDMDocument()
        {
            return JsonConvert.DeserializeObject<List<ldDocumentDto>>(_Session.GetString(SessionIDMDocument));
        }

        public void SetSecurityTypeDDL(List<SelectListItem> securityType)
        {
            _Session.SetString(SessionDDListSecurityType, JsonConvert.SerializeObject(securityType));
        }

        public void SetSecurityCategoryDDL(List<SelectListItem> securityCategory)
        {
            _Session.SetString(SessionDDListSecurityCategory, JsonConvert.SerializeObject(securityCategory));
        }

        public void SetChargeTypeDDL(List<SelectListItem> chargeType)
        {
            _Session.SetString(SessionDDListChargeType, JsonConvert.SerializeObject(chargeType));
        }
        public void SetSubRegisterOfficeDDL(List<SelectListItem> subRegisterOfice)
        {
            _Session.SetString(SessionDDListSubRegisterOffice, JsonConvert.SerializeObject(subRegisterOfice));
        }
        public void SetBankIFSCCodeDDL(List<SelectListItem> ifscCode)
        {
            _Session.SetString(SessionDDListBankIFSCCode, JsonConvert.SerializeObject(ifscCode));
        }
        public List<SelectListItem> GetAllBankIFSCCodeDDL()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListBankIFSCCode));
        }
        public List<SelectListItem> GetAllConditionDescription()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListConditionDescriptions));
        }
        public List<SelectListItem> GetAllAssetType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListAssetType));
        }
        public List<SelectListItem> GetAllSecurityType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListSecurityType));
        }
        public List<SelectListItem> GetAllSecurityCategory()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListSecurityCategory));
        }
        public List<SelectListItem> GetAllChargeType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListChargeType));
        }
        public List<SelectListItem> GetAllSubRegisterOffice()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListSubRegisterOffice));
        }
        public List<SelectListItem> GetAllConditionType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListConditionType));
        }

        // Author: MJ; Module: Form8AndForm13; Date:19/08/2022
        public void SetForm8AndForm13List(IEnumerable<Form8AndForm13DTO> allForm8AndForm13List)
        {
            _Session.SetString(SessionSetForm8AndForm13, JsonConvert.SerializeObject(allForm8AndForm13List));
        }
        // Author: GS; Module: Form8AndForm13; Date:19/08/2022

        public void SetForm8andForm13Master(List<SelectListItem> lstForm8andForm13Master)
        {
            _Session.SetString(SessionForm8andForm13Master, JsonConvert.SerializeObject(lstForm8andForm13Master));
        }

        public List<SelectListItem> GetAllForm8AndForm13Master()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionForm8andForm13Master));
        }
        public List<Form8AndForm13DTO> GetForm8AndForm13List()
        {
            return JsonConvert.DeserializeObject<List<Form8AndForm13DTO>>(_Session.GetString(SessionSetForm8AndForm13));
        }
        #region Audit Clearance
        public void SetAuditClearanceList(IEnumerable<IdmAuditDetailsDTO> allAuditClearanceList)
        {
            _Session.SetString(SessionAllAuditClearanceList, JsonConvert.SerializeObject(allAuditClearanceList));
        }
        public List<IdmAuditDetailsDTO> GetAllAuditClearanceList()
        {
            return JsonConvert.DeserializeObject<List<IdmAuditDetailsDTO>>(_Session.GetString(SessionAllAuditClearanceList));
        }

        #endregion
        #region FirstInvestmentClause
        public IdmFirstInvestmentClauseDTO GetAllFirstInvestmentClause()
        {
            return JsonConvert.DeserializeObject<IdmFirstInvestmentClauseDTO>(_Session.GetString(sessionAllFirstInvestmentClause));
        }
        public void SetFirstInvestmentClause(IEnumerable<IdmFirstInvestmentClauseDTO> allFirstInvestmentClause)
        {
            _Session.SetString(sessionAllFirstInvestmentClause, JsonConvert.SerializeObject(allFirstInvestmentClause));
        }
        #endregion
        #region Land Inspection
        // Author: MJ; Module: Land Inspection; Date:25/08/2022
        public void SetLandInspectionList(IEnumerable<IdmDchgLandDetDTO> allLandInspectionList)
        {
            _Session.SetString(SessionAllLandInspectionList, JsonConvert.SerializeObject(allLandInspectionList));
        }

        #region Sidbi Approval


        public IdmSidbiApprovalDTO GetAllSidbiApproval()
        {
            return JsonConvert.DeserializeObject<IdmSidbiApprovalDTO>(_Session.GetString(sessionAllSidbiApproval));
        }
        #endregion

        #region Change Location SessionAllAddressDetailsList
        public void SetAllAddressDetailsList(IEnumerable<IdmUnitAddressDTO> allAddressDetails)
        {
            _Session.SetString(SessionAllAddressDetailsList, JsonConvert.SerializeObject(allAddressDetails));
        }

        public List<IdmUnitAddressDTO> GetAllAddressDetailsList()
        {
            return JsonConvert.DeserializeObject<List<IdmUnitAddressDTO>>(_Session.GetString(SessionAllAddressDetailsList));
        }

        public void SetAllMasterPincodeDetails(IEnumerable<TblPincodeMasterDTO> allMasterPincodeDetails)
        {
            _Session.SetString(SessionAllMasterPincodeDetails, JsonConvert.SerializeObject(allMasterPincodeDetails));
        }
        public void SetAllPincodeDistrictDetails(IEnumerable<PincodeDistrictCdtabDTO> allPincodeDistrictDetails)
        {
            _Session.SetString(SessionAllPincodeDistrictDetails, JsonConvert.SerializeObject(allPincodeDistrictDetails));
        }
        public void SetAllTalukDetails(IEnumerable<TlqCdTabDTO> allTalukDetails)
        {
            _Session.SetString(SessionAllTalukDetails, JsonConvert.SerializeObject(allTalukDetails));
        }

        public void SetAllHobliDetails(IEnumerable<HobCdtabDTO> allHobliDetails)
        {
            _Session.SetString(SessionAllHobliDetails, JsonConvert.SerializeObject(allHobliDetails));
        }

        public void SetAllVillageDetails(IEnumerable<VilCdTabDTO> allVillageDetails)
        {
            _Session.SetString(SessionAllVillageDetails, JsonConvert.SerializeObject(allVillageDetails));
        }
       

        public List<TblPincodeMasterDTO> GetAllMasterPincodeDetails()
        {
            return JsonConvert.DeserializeObject<List<TblPincodeMasterDTO>>(_Session.GetString(SessionAllMasterPincodeDetails));
        }

        public List<PincodeDistrictCdtabDTO> GetAllPincodeDistrictDetails()
        {
            return JsonConvert.DeserializeObject<List<PincodeDistrictCdtabDTO>>(_Session.GetString(SessionAllPincodeDistrictDetails));
        }
        public List<PincodeStateCdtabDTO> GetAllPincodeStateDetails() 
        {
            return JsonConvert.DeserializeObject<List<PincodeStateCdtabDTO>>(_Session.GetString(SessionAllPincodeStateDetails)); 
        }


        public List<TlqCdTabDTO> GetAllTalukDetails()
        {
            return JsonConvert.DeserializeObject<List<TlqCdTabDTO>>(_Session.GetString(SessionAllTalukDetails));
        }
        public List<DistCdTabDTO> GetAllDistrictDetails()
        {
            return JsonConvert.DeserializeObject<List<DistCdTabDTO>>(_Session.GetString(SessionAllDistrictDetails));
        }
        public List<VilCdTabDTO> GetAllVillageDetails()
        {
            return JsonConvert.DeserializeObject<List<VilCdTabDTO>>(_Session.GetString(SessionAllVillageDetails));
        }
        public List<HobCdtabDTO> GetAllHobliDetails()
        {
            return JsonConvert.DeserializeObject<List<HobCdtabDTO>>(_Session.GetString(SessionAllHobliDetails));
        }

        public void SetDDLDistrictList(List<SelectListItem> allDistrictDetails)
        {
            _Session.SetString(DDLSessionAllDistrictDetails, JsonConvert.SerializeObject(allDistrictDetails));
        }

        public List<SelectListItem> GetDDLDistrictList()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(DDLSessionAllDistrictDetails));
        }

        public List<SelectListItem> GetDDLStateList()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(DDLSessionAllStateDetails)); 
        }

        public void SetDDLTalukList(List<SelectListItem> allTalukDetails)
        {
            _Session.SetString(DDLSessionAllTalukDetails, JsonConvert.SerializeObject(allTalukDetails));
        }
        public List<SelectListItem> GetDDLTalukList()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(DDLSessionAllTalukDetails));
        }
        public void SetDDLHobliList(List<SelectListItem> allHobliDetails)
        {
            _Session.SetString(DDLSessionAllHobliDetails, JsonConvert.SerializeObject(allHobliDetails));
        }

        public List<SelectListItem> GetDDLHobliList()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(DDLSessionAllHobliDetails));
        }
        public void SetDDLVillageList(List<SelectListItem> allVillageDetails)
        {
            _Session.SetString(DDLSessionAllVillageDetails, JsonConvert.SerializeObject(allVillageDetails));
        }
        public List<SelectListItem> GetDDLVillageList()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(DDLSessionAllVillageDetails));
        }

        public void SetDDLPincodeList(List<SelectListItem> allPincodeDetails)
        {
            _Session.SetString(DDLSessionAllPincodeDetails, JsonConvert.SerializeObject(allPincodeDetails));
        }
        public List<SelectListItem> GetDDLPincodeList()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(DDLSessionAllPincodeDetails));
        }
        #endregion

        #region Promoter
        public void SetMasterPromoterProfileList(IEnumerable<TblPromcdtabDTO> allMasterPromoterProfileList)
        {
            _Session.SetString(SessionSetMasterPromoterProfile, JsonConvert.SerializeObject(allMasterPromoterProfileList));
        }
        public void SetPromoterProfileList(IEnumerable<IdmPromoterDTO> allPromoterProfileList)
        {
            _Session.SetString(SessionSetPromoterProfile, JsonConvert.SerializeObject(allPromoterProfileList));
        }
        public List<IdmPromoterDTO> GetAllPromoterProfileList()
        {
            return JsonConvert.DeserializeObject<List<IdmPromoterDTO>>(_Session.GetString(SessionSetPromoterProfile));
        }

        public List<TblPromcdtabDTO> GetAllMasterPromoterProfileList()
        {
            return JsonConvert.DeserializeObject<List<TblPromcdtabDTO>>(_Session.GetString(SessionSetMasterPromoterProfile));
        }
        public void SetPromoterAddressList(IEnumerable<IdmPromAddressDTO> allPromoAddressDetails)
        {
            _Session.SetString(SessionSetPromoterAddress, JsonConvert.SerializeObject(allPromoAddressDetails));
        }

        public List<IdmPromAddressDTO> GetAllPromoAddressDetails()
        {
            return JsonConvert.DeserializeObject<List<IdmPromAddressDTO>>(_Session.GetString(SessionSetPromoterAddress));
        }

        public void SetPromoterNamesList(List<SelectListItem> promoterNames)
        {
            _Session.SetString(DDLSessionPromoterNames, JsonConvert.SerializeObject(promoterNames));
        }
        public List<SelectListItem> GetAllPromoterNames()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(DDLSessionPromoterNames));
        }

        public void SetPromoterStateList(List<SelectListItem> promoterState)
        {
            _Session.SetString(DDLSessionPromoterState, JsonConvert.SerializeObject(promoterState));
        }
        public void SetPromoterDistrictList(List<SelectListItem> promoterDistrict)
        {
            _Session.SetString(DDLSessionPromoterDistrict, JsonConvert.SerializeObject(promoterDistrict));

        }
        public void SetPromoterPincodeList(List<SelectListItem> promoterPincode)  
        {
            _Session.SetString(DDLSessionPromoterPincode,  JsonConvert.SerializeObject(promoterPincode));

        }

        public void SetPromoterBankList(IEnumerable<IdmPromoterBankDetailsDTO> allPromoterBankList)
        {
            _Session.SetString(SessionSetPromoterBank, JsonConvert.SerializeObject(allPromoterBankList));
        }
        public List<IdmPromoterBankDetailsDTO> GetAllPromoterBankList()
        {
            return JsonConvert.DeserializeObject<List<IdmPromoterBankDetailsDTO>>(_Session.GetString(SessionSetPromoterBank));
        }



        //SessionDDListPromoterClass
        public void SetDDListPromoterClass(List<SelectListItem> promClasTypes)
        {
            _Session.SetString(SessionDDListPromoterClass, JsonConvert.SerializeObject(promClasTypes));
        }
        public List<SelectListItem> GetDDListPromoterClass()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListPromoterClass));
        }

        //SessionDDListPromoterSubClass
        public void SetDDListPromoterSubClass(List<SelectListItem> promSubClasTypes)
        {
            _Session.SetString(SessionDDListPromoterSubClass, JsonConvert.SerializeObject(promSubClasTypes));
        }
        public List<SelectListItem> GetDDListPromoterSubClass()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListPromoterSubClass));
        }

        //SessionDDListPromoterQualification
        public void SetDDListPromoterQualification(List<SelectListItem> promQual)
        {
            _Session.SetString(SessionDDListPromoterQualification, JsonConvert.SerializeObject(promQual));
        }
        public List<SelectListItem> GetDDListPromoterQualification()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListPromoterQualification));
        }

        //SessionDDListAccountType
        public void SetDDListAccountType(List<SelectListItem> promAcc)
        {
            _Session.SetString(SessionDDListTypeOfAccount, JsonConvert.SerializeObject(promAcc));
        }
        public List<SelectListItem> GetDDListAccountType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListTypeOfAccount));
        }



        public void SetPromoterLiabilityInfo(IEnumerable<TblPromoterLiabDetDTO> allPromoLiabilityInfo)
        {
            _Session.SetString(SessionSetPromoterLiabilityInfo, JsonConvert.SerializeObject(allPromoLiabilityInfo));
        }

        public List<TblPromoterLiabDetDTO> GetAllPromoterLiabilityInfo()
        {
            return JsonConvert.DeserializeObject<List<TblPromoterLiabDetDTO>>(_Session.GetString(SessionSetPromoterLiabilityInfo));
        }


        public void SetPromoteNetWorth(IEnumerable<TblPromoterNetWortDTO> allPromoNetWorth)
        {
            _Session.SetString(SessionSetPromoterNetWorth, JsonConvert.SerializeObject(allPromoNetWorth));
        }

        public List<TblPromoterNetWortDTO> GetAllPromoterNetWorth()
        {
            return JsonConvert.DeserializeObject<List<TblPromoterNetWortDTO>>(_Session.GetString(SessionSetPromoterNetWorth));
        }






        #endregion

        #region product details
        public void SetProductDetailsList(IEnumerable<IdmUnitProductsDTO> allProductDetailsList)
        {
            _Session.SetString(SessionAllProductDetailsList, JsonConvert.SerializeObject(allProductDetailsList));
        }

        public List<IdmUnitProductsDTO> GetProductDetailsList()
        {
            return JsonConvert.DeserializeObject<List<IdmUnitProductsDTO>>(_Session.GetString(SessionAllProductDetailsList));
        }

        public void SetProductList(IEnumerable<TblProdCdtabDTO> allProductList)
        {
            _Session.SetString(SessionAllProductList, JsonConvert.SerializeObject(allProductList));
        }
        public List<TblProdCdtabDTO> GetProductList()
        {
            return JsonConvert.DeserializeObject<List<TblProdCdtabDTO>>(_Session.GetString(SessionAllProductList));
        }

        public void SetProducdetailsMaster(List<SelectListItem> lstProducdetailsMaster)
        {
            _Session.SetString(SessionProductDetailsMaster, JsonConvert.SerializeObject(lstProducdetailsMaster));
        }
        public List<SelectListItem> GetallProducdetailsMaster()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionProductDetailsMaster));
        }

       



        public void SetIndustrydetailsMaster(List<SelectListItem> lstIndustrydetailsMaster)
        {
            _Session.SetString(SessionIndustryDetailsMaster, JsonConvert.SerializeObject(lstIndustrydetailsMaster));
        }

        public List<SelectListItem> GetallIndustrydetailsMaster()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionIndustryDetailsMaster));
        }

        #endregion

        #region Asset Information
        public void SetAssetDetailsList(IEnumerable<IdmPromAssetDetDTO> allAssetDetailsList)
        {
            _Session.SetString(SessionAllAssetDetailsList, JsonConvert.SerializeObject(allAssetDetailsList));
        }

        public List<IdmPromAssetDetDTO> GetAssetDetailsList()
        {
            return JsonConvert.DeserializeObject<List<IdmPromAssetDetDTO>>(_Session.GetString(SessionAllAssetDetailsList));
        }


        public void SetAssetTypeMaster(List<SelectListItem> lstAssetTypeMaster)
        {
            _Session.SetString(SessionAssetTypeMaster, JsonConvert.SerializeObject(lstAssetTypeMaster));
        }

        public List<SelectListItem> GetallAssetTypeMasterMaster()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionAssetTypeMaster));
        }

        public void SetAssetCategaryMaster(List<SelectListItem> lstAssetCategaryMaster)
        {
            _Session.SetString(SessionAssetCategaryMaster, JsonConvert.SerializeObject(lstAssetCategaryMaster));
        }

        public List<SelectListItem> GetallAssetCategaryMasterMaster()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionAssetCategaryMaster));
        }
        
        public void SetAllAssetTypeDetails(IEnumerable<AssetTypeDetailsDTO> allAssetTypeDetails)
        {
            _Session.SetString(SessionAssetTypeDetails, JsonConvert.SerializeObject(allAssetTypeDetails));
        }
        
        public List<AssetTypeDetailsDTO> GetAllAssetTypeDetails()
        {
            return JsonConvert.DeserializeObject<List<AssetTypeDetailsDTO>>(_Session.GetString(SessionAssetTypeDetails));
        }

        public void SetAllAssetCategaryDetails(IEnumerable<TblAssetCatCDTabDTO> allAssetCatDetails)
        {
            _Session.SetString(SessionAssetCategaryDetails, JsonConvert.SerializeObject(allAssetCatDetails));
        }
        public List<TblAssetCatCDTabDTO> GetAllAssetCategaryDetails()
        {
            return JsonConvert.DeserializeObject<List<TblAssetCatCDTabDTO>>(_Session.GetString(SessionAssetCategaryDetails));
        }


        #endregion

        //Author: MJ; Module: Land Inspection; Date:25/08/2022
        public List<IdmDchgLandDetDTO> GetAllLandInspectionList()
        {
            return JsonConvert.DeserializeObject<List<IdmDchgLandDetDTO>>(_Session.GetString(SessionAllLandInspectionList));
        }
        #endregion

        #region Building Inspection
        // Author: Swetha; Module: Land Inspection; Date:25/08/2022
        public void SetBuildingInspectionList(IEnumerable<IdmDchgBuildingDetDTO> allBuildingInspectionList)
        {
            _Session.SetString(SessionAllBuildingInspectionList, JsonConvert.SerializeObject(allBuildingInspectionList));
        }
        public List<IdmDchgBuildingDetDTO> GetAllBuildingInspectionList()
        {
            return JsonConvert.DeserializeObject<List<IdmDchgBuildingDetDTO>>(_Session.GetString(SessionAllBuildingInspectionList));
        }
        #endregion

        #region Status of implementation 
        public void SetStatusofImplementaionList(IEnumerable<IdmDsbStatImpDTO> allStatusofImplementationList)
        {
            _Session.SetString(SessionStatusofImplementation, JsonConvert.SerializeObject(allStatusofImplementationList));
        }
        public List<IdmDsbStatImpDTO> GetAllStatusOfImplementation()
        {
            return JsonConvert.DeserializeObject<List<IdmDsbStatImpDTO>>(_Session.GetString(SessionStatusofImplementation));
        }

        #endregion

        //Author: Sandeep M; Module: InpsectionDetail; Date:25/08/2022
        #region InpsectionDetail
        public void SetInspectionDetialList(IEnumerable<IdmDspInspDTO> InspectionDetailList)
        {
            _Session.SetString(SessionAllInspectionDetail, JsonConvert.SerializeObject(InspectionDetailList));
        }
        public List<IdmDspInspDTO> GetAllInspectionDetail()
        {
            return JsonConvert.DeserializeObject<List<IdmDspInspDTO>>(_Session.GetString(SessionAllInspectionDetail));
        }
        public void SetAllInspectionDetialList(IEnumerable<IdmDspInspDTO> InspectionDetailList)
        {
            _Session.SetString(SessionInspectionDetail, JsonConvert.SerializeObject(InspectionDetailList));
        }
        public List<IdmDspInspDTO> GetAllInspectionList()
        {
            return JsonConvert.DeserializeObject<List<IdmDspInspDTO>>(_Session.GetString(SessionInspectionDetail));
        }

        #endregion
        #region Change Bank Details
        public void SetChangeBankDetailsList(IEnumerable<IdmChangeBankDetailsDTO> allChangeBankDetailsList)
        {
            _Session.SetString(SessionAllChangeBankDetailsList, JsonConvert.SerializeObject(allChangeBankDetailsList));
        }

        public List<IdmChangeBankDetailsDTO> GetChangeBankDetailsList()
        {
            return JsonConvert.DeserializeObject<List<IdmChangeBankDetailsDTO>>(_Session.GetString(SessionAllChangeBankDetailsList));
        }

        public void SetIfscBankDetailsList(IEnumerable<IfscMasterDTO> allIfscBankDetailsList)
        {
            _Session.SetString(SessionAllIfscBankDetailsList, JsonConvert.SerializeObject(allIfscBankDetailsList));
        }

        #region Asset Details 
        public List<TblIdmProjLandDTO> GetTblIdmProjLandList()
        {
            return JsonConvert.DeserializeObject<List<TblIdmProjLandDTO>>(_Session.GetString(SessionAllLandAssetDetails));
        }

        public void SetAllLandAssetDetails(IEnumerable<TblIdmProjLandDTO> allLandAssetDetails)
        {
            _Session.SetString(SessionAllLandAssetDetails, JsonConvert.SerializeObject(allLandAssetDetails));
        }


        #endregion

        #region Building Material at Site Inspection
        // Author: MJ; Module: Building Material Inspection; Date:25/08/2022
        public void SetBuildMatSiteInspectionList(IEnumerable<IdmBuildingMaterialSiteInspectionDTO> allBuildMatSiteInspectionList)
        {
            _Session.SetString(SessionAllBuildMatSiteInspectionList, JsonConvert.SerializeObject(allBuildMatSiteInspectionList));
        }
        //Author: MJ; Module: Building Material Inspection; Date:25/08/2022
        public List<IdmBuildingMaterialSiteInspectionDTO> GetAllBuildMatSiteInspectionList()
        {
            return JsonConvert.DeserializeObject<List<IdmBuildingMaterialSiteInspectionDTO>>(_Session.GetString(SessionAllBuildMatSiteInspectionList));
        }
        #endregion

        #region Working Capital

        // Author: Swetha; Module: Working Capital; Date:30/08/2022
        public void SetWorkingCapitalDetails(IdmDchgWorkingCapitalDTO allWorkingCapital)
        {
            _Session.SetString(SessionWorkingCapitalInspection, JsonConvert.SerializeObject(allWorkingCapital));
        }
        public List<IdmDchgWorkingCapitalDTO> GetAllWorkingCapitalInspection()
        {
            return JsonConvert.DeserializeObject<List<IdmDchgWorkingCapitalDTO>>(_Session.GetString(SessionWorkingCapitalInspection));
        }

        #endregion

        //Author: Sandeep M; Module: InpsectionDetail; Date:30/08/2022
        #region Furniture Inspection
        public List<IdmDChgFurnDTO> GetAllFurnitureInspectionList()
        {
            return JsonConvert.DeserializeObject<List<IdmDChgFurnDTO>>(_Session.GetString(SessionAllFurnitureInspectionDetail));
        }

        public void SetFurnitureInspectionList(IEnumerable<IdmDChgFurnDTO> allFurnitureInspectionList)
        {
            _Session.SetString(SessionAllFurnitureInspectionDetail, JsonConvert.SerializeObject(allFurnitureInspectionList));
        }
        #endregion
        #region Indigenous Machinery Inspection
        // Author: Swetha; Module: IndigenousMachinery Inspection; Date:25/08/2022
        public void SetIndigenousMachineryInspectionList(IEnumerable<IdmDchgIndigenousInspectionDTO> allIndigenousMachineryInspectionList)
        {
            _Session.SetString(SessionAllIndigenousMachineryInspectionList, JsonConvert.SerializeObject(allIndigenousMachineryInspectionList));
        }
        public List<IdmDchgIndigenousInspectionDTO> GetAllIndigenousMachineryInspectionList()
        {
            return JsonConvert.DeserializeObject<List<IdmDchgIndigenousInspectionDTO>>(_Session.GetString(SessionAllIndigenousMachineryInspectionList));
        }

        public void SetMachinaryStatusList(IEnumerable<TblMachineryStatusDto> allmachinarystatuslist)
        {
            _Session.SetString(SessionAllMachinaryStatusList, JsonConvert.SerializeObject(allmachinarystatuslist));
        }

        public List<TblMachineryStatusDto> GetAllMachinaryStatusList()
        {
            return JsonConvert.DeserializeObject<List<TblMachineryStatusDto>>(_Session.GetString(SessionAllMachinaryStatusList));
        }

        public void SetProcureList(IEnumerable<TblProcureMastDto> allProcurelist)
        {
            _Session.SetString(SessionAllProcureList, JsonConvert.SerializeObject(allProcurelist));
        }
    
        public List<TblProcureMastDto> GetAllProcureList()
        {
            return JsonConvert.DeserializeObject<List<TblProcureMastDto>>(_Session.GetString(SessionAllProcureList));
        }

        public void SetCurrencyList(IEnumerable<TblCurrencyMastDto> allCurrencylist)
        {
            _Session.SetString(SessionAllCurrencyList, JsonConvert.SerializeObject(allCurrencylist));
        }

        public List<TblCurrencyMastDto> GetAllCurrencyList()
        {
            return JsonConvert.DeserializeObject<List<TblCurrencyMastDto>>(_Session.GetString(SessionAllCurrencyList));
        }
      
      

        
        #endregion

        #region Project Cost
        // Author: Akhila; Module: Project Cost; Date:05/09/2022
        public void SetProjectCostList(IEnumerable<IdmDchgProjectCostDTO> allProjectCostList)
        {
            _Session.SetString(SessionAllProjectCostList, JsonConvert.SerializeObject(allProjectCostList));
        }
        public List<IdmDchgProjectCostDTO> GetAllProjectCostList()
        {
            return JsonConvert.DeserializeObject<List<IdmDchgProjectCostDTO>>(_Session.GetString(SessionAllProjectCostList));
        }

        #endregion
        #region Import Machinery Inspection
        // Author: Swetha; Module: Land Inspection; Date:25/08/2022
        public void SetImportMachineryList(IEnumerable<IdmDchgImportMachineryDTO> allBuildingInspectionList)
        {
            _Session.SetString(SessionAllImportMachineryList, JsonConvert.SerializeObject(allBuildingInspectionList));
        }
        public List<IdmDchgImportMachineryDTO> GetAllImportMachineryList()
        {
            return JsonConvert.DeserializeObject<List<IdmDchgImportMachineryDTO>>(_Session.GetString(SessionAllImportMachineryList));
        }
       
        #endregion
        #region Means Of Finance
        // Author: Swetha; Module: Land Inspection; Date:25/08/2022
        public void SetMeansOfFinanceList(IEnumerable<IdmDchgMeansOfFinanceDTO> allMeansOfFinanceList)
        {
            _Session.SetString(SessionAllMeansOfFinanceList, JsonConvert.SerializeObject(allMeansOfFinanceList));
        }
        public List<IdmDchgMeansOfFinanceDTO> GetAllMeansOfFinanceList()
        {
            return JsonConvert.DeserializeObject<List<IdmDchgMeansOfFinanceDTO>>(_Session.GetString(SessionAllMeansOfFinanceList));
        }

        public void SetFinanceCategoryList(List<SelectListItem> financeCategory)
        {
            _Session.SetString(SessionDDLofFinanceCategory, JsonConvert.SerializeObject(financeCategory));
        }
        public List<SelectListItem> GetAllFinanceCategoryList()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDLofFinanceCategory));
        }

        #endregion


        //Author: Manoj; Module: LetterOfCredit; Date:05/09/2022
        #region Letter Of Credit
        public void SetLetterOfCreditList(IEnumerable<IdmDsbLetterOfCreditDTO> LetterOfCreditDetailList)
        {
            _Session.SetString(SessionAllLetterOfCreditList, JsonConvert.SerializeObject(LetterOfCreditDetailList));
        }
        public List<IdmDsbLetterOfCreditDTO> GetAllLetterOfCreditDetail()
        {
            return JsonConvert.DeserializeObject<List<IdmDsbLetterOfCreditDTO>>(_Session.GetString(SessionAllLetterOfCreditList));
        }
        #endregion
        #region DropDown
        
        public void SetProjectCostDetailsList(List<SelectListItem> ifscCode)
        {
            _Session.SetString(SessionDDListprojectCostComponentDetail, JsonConvert.SerializeObject(ifscCode));
        }
        public List<SelectListItem> GetAllProjectCostDetailsList()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListprojectCostComponentDetail));
        }

        #endregion

        #region Loan Accounting
        public void SetAllAccountingLoanNumber(List<LoanAccountNumberDTO> loans)
        {
            _Session.SetString(SessionAllAccountingLoanNumberList, JsonConvert.SerializeObject(loans));
        }
        public List<LoanAccountNumberDTO> GetAllAccountingLoanNumber()
        {
            return JsonConvert.DeserializeObject<List<LoanAccountNumberDTO>>(_Session.GetString(SessionAllAccountingLoanNumberList));
        }

        #endregion

        #region Loan Accounting Dropdowns
        public void SetTransactionTypesDDL(List<SelectListItem> allTransactionTypes)
        {
            _Session.SetString(SessionDDListAllTransactionTypes, JsonConvert.SerializeObject(allTransactionTypes));
        }
        #endregion

        #region Loan Related Receipt
        public void SetGenerateReceiptPaymentList(IEnumerable<TblLaReceiptPaymentDetDTO> allGenerateReceiptPaymentList)
        {
            _Session.SetString(SessionAllGenerateReceiptPaymentList, JsonConvert.SerializeObject(allGenerateReceiptPaymentList));
        }
        public List<TblLaReceiptPaymentDetDTO> GetAllGenerateReceiptPaymentList()
        {
            return JsonConvert.DeserializeObject<List<TblLaReceiptPaymentDetDTO>>(_Session.GetString(SessionAllGenerateReceiptPaymentList));
        }
        #endregion

        #region codetable
        public void SetCodeTableTransactionTypeList(IEnumerable<CodeTableDTO> allCodeTableTransactionTypeList)
        {
            _Session.SetString(SessionAllCodeTableTransactionTypeList, JsonConvert.SerializeObject(allCodeTableTransactionTypeList));
        }

        public List<CodeTableDTO> GetCodeTableTransactionTypeList()
        {
            return JsonConvert.DeserializeObject<List<CodeTableDTO>>(_Session.GetString(SessionAllCodeTableTransactionTypeList));
        }
        public void SetCodeTableModeofPaymentList(IEnumerable<CodeTableDTO> allCodeTableModeofPaymentList)
        {
            _Session.SetString(SessionAllCodeTableModeofPaymentList, JsonConvert.SerializeObject(allCodeTableModeofPaymentList));
        }
        public List<CodeTableDTO> GetCodeTablePaymentTypeList()
        {
            return JsonConvert.DeserializeObject<List<CodeTableDTO>>(_Session.GetString(SessionAllCodeTableModeofPaymentList));
        }

        public List<CodeTableDTO> GetCodeTableList()
        {
            return JsonConvert.DeserializeObject<List<CodeTableDTO>>(_Session.GetString(SessionAllCodeTableList));
        }
        #endregion


        #region saved receipts
        public void SetReceiptPaymentList(IEnumerable<TblLaReceiptPaymentDetDTO> allReceiptPaymentList)
        {
            _Session.SetString(SessionAllReceiptPaymentList, JsonConvert.SerializeObject(allReceiptPaymentList));
        }

        public List<TblLaReceiptPaymentDetDTO> GetAllReceiptPaymentList()
        {
            return JsonConvert.DeserializeObject<List<TblLaReceiptPaymentDetDTO>>(_Session.GetString(SessionAllReceiptPaymentList));
        }
        #endregion
        public List<IfscMasterDTO> GetIfscBankDetailsList()
        {
            return JsonConvert.DeserializeObject<List<IfscMasterDTO>>(_Session.GetString(SessionAllIfscBankDetailsList));
        }
        #endregion

        // Creation of Security and Acquisition of Assets

        #region Land Acquisition
        //SessionDDListLandType
        public void SetDDListLandType(List<SelectListItem> landType)
        {
            _Session.SetString(SessionDDListLandType, JsonConvert.SerializeObject(landType));
        }
        public List<SelectListItem> GetDDListLandType()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListLandType));
        }
        public void SetLandAcquisitionList(IEnumerable<TblIdmIrLandDTO> allLandAcquisition)
        {
            _Session.SetString(sessionAllLandAcquisition, JsonConvert.SerializeObject(allLandAcquisition));
        }

        public List<TblIdmIrLandDTO> GetAllLandAcquisitionList()
        {
            return JsonConvert.DeserializeObject<List<TblIdmIrLandDTO>>(_Session.GetString(sessionAllLandAcquisition));
        }
        #endregion
        #region Machinery Acquisition
        public void SetMachineryAcquisitionList(IEnumerable<IdmIrPlmcDTO> allMachineryAcquisition)
        {
            _Session.SetString(sessionAllMachineryAcquisition, JsonConvert.SerializeObject(allMachineryAcquisition));
        }

        public List<IdmIrPlmcDTO> GetAllMachineryAcquisitionList()
        {
            return JsonConvert.DeserializeObject<List<IdmIrPlmcDTO>>(_Session.GetString(sessionAllMachineryAcquisition));
        }
        #endregion

        #region LoanAllocation
        //Author: Gagana; Module: LoanAllocation; Date:28/09/2022
        public void SetAllLoanAllocationList(IEnumerable<TblIdmDhcgAllcDTO> allLoanAllocationList)
        {
            _Session.SetString(SessionAllLoanAllocationList, JsonConvert.SerializeObject(allLoanAllocationList));
        }
        public List<TblIdmDhcgAllcDTO> GetAllLoanAllocationList()
        {
            return JsonConvert.DeserializeObject<List<TblIdmDhcgAllcDTO>>(_Session.GetString(SessionAllLoanAllocationList));
        }

        public void SetAllAllocationCodeDDL(List<SelectListItem> allocationCode)
        {
            _Session.SetString(SessionDDListAllocationCode, JsonConvert.SerializeObject(allocationCode));
        }

        public List<SelectListItem> GetAllAllocationCode()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListAllocationCode));
        }
        #endregion

        #region Building Acquistion Details

        public void SetBuildingAcquisitionList(IEnumerable<TblIdmBuildingAcquisitionDetailsDTO> tblIdmBuildingAcquisitionDetails)
        {
            _Session.SetString(SessionAllBuildingAcquisitionDetail, JsonConvert.SerializeObject(tblIdmBuildingAcquisitionDetails));
        }
        public List<TblIdmBuildingAcquisitionDetailsDTO> GetAllBuildingAcquisitionDetail()
        {
            return JsonConvert.DeserializeObject<List<TblIdmBuildingAcquisitionDetailsDTO>>(_Session.GetString(SessionAllBuildingAcquisitionDetail));
        }

        #endregion

        //Author: Kiran Vasishta TS, Module: Furniture Acquisition; Date: 30-Sep-2022
        #region Furniture Acquisition
        public void SetFurnitureAcquisitionList(IEnumerable<Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset.TblIdmIrFurnDTO> allFurnitureAcquisition)
        {
            _Session.SetString(FurnitureAcquisitionList, JsonConvert.SerializeObject(allFurnitureAcquisition));
        }
        public List<TblIdmIrFurnDTO> GetFurnitureAcquisitionList()
        {
            return JsonConvert.DeserializeObject<List<TblIdmIrFurnDTO>>(_Session.GetString(FurnitureAcquisitionList));
        }
        #endregion


        #region disbursement proposal details
        public void SetProposalDetailsList (IEnumerable<TblIdmReleDetlsDTO> allProposalDetails)
        {
            _Session.SetString(SessionAllProposalDetailsList, JsonConvert.SerializeObject(allProposalDetails));
        }

        public List<TblIdmReleDetlsDTO> GetAllProposalDetailsList()
        {
            return JsonConvert.DeserializeObject<List<TblIdmReleDetlsDTO>>(_Session.GetString(SessionAllProposalDetailsList));
        }
        #endregion

        #region Recommended Disbursement Details

        public void SetRecommDisbursementList(IEnumerable<IdmDsbdetsDTO> idmDsbdets)
        {
            _Session.SetString(SessionAllRecomDisburseList, JsonConvert.SerializeObject(idmDsbdets));
        }
        public List<IdmDsbdetsDTO> GetAllRecommDisbursementDetail()
        {
            return JsonConvert.DeserializeObject<List<IdmDsbdetsDTO>>(_Session.GetString(SessionAllRecomDisburseList));
        }

        #endregion

        #region Other Relaxation
        public void SetAllOtherRelaxation(IEnumerable<RelaxationDTO> othRelx)
        {
            _Session.SetString(SessionAllOtherRelaxation, JsonConvert.SerializeObject(othRelx));
        }

        public List<RelaxationDTO> GetAllOtherRelaxation()
        {
            return JsonConvert.DeserializeObject<List<RelaxationDTO>>(_Session.GetString(SessionAllOtherRelaxation));
        }
        #endregion

        #region Other Debits List
        public void SetOtherDebitsList(IEnumerable<IdmOthdebitsDetailsDTO> allOtherDebitsList)
        {
            _Session.SetString(SessionAllOtherDebitsList, JsonConvert.SerializeObject(allOtherDebitsList));
        }
        public List<IdmOthdebitsDetailsDTO> GetAllOtherDebitsList()
        {
            return JsonConvert.DeserializeObject<List<IdmOthdebitsDetailsDTO>>(_Session.GetString(SessionAllOtherDebitsList));
        }

        public void SetAllOtherDebitCodeDDL(List<SelectListItem> OtherDebitCode)
        {
            _Session.SetString(SessionDDListOtherDebitCode, JsonConvert.SerializeObject(OtherDebitCode));
        }

        public List<SelectListItem> GetAllOtherDebitCode()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListOtherDebitCode));
        }
        #endregion

        public void SetAllUmoMasterlist(List<SelectListItem> UmoMasterList)
        {
            _Session.SetString(SessionDDLMasterList, JsonConvert.SerializeObject(UmoMasterList));
        }

        public List<SelectListItem> GetAllUmoMasterlist()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDLMasterList));
        }

        public void SetAllConstutionlist(List<SelectListItem> UConstutionList)
        {
            _Session.SetString(SessionDDLConstutionMasterList, JsonConvert.SerializeObject(UConstutionList));
        }
        public List<SelectListItem> GetAllConstutionlist()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDLConstutionMasterList));
        }

        #region Dept Master
        public void SetDDListDeptMaster(List<SelectListItem> deptMaster)
        {
            _Session.SetString(SessionDDListDeptMaster, JsonConvert.SerializeObject(deptMaster));
        }
        public List<SelectListItem> GetDDListDeptMaster()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListDeptMaster));
        }

        public void SetAllDsbChargeMapDDL(List<SelectListItem> DsbChargeMap)
        {
            _Session.SetString(SessionDDListDsbChargeMap, JsonConvert.SerializeObject(DsbChargeMap));
        }

        public List<SelectListItem> GetAllDsbChargeMap()
        {
            return JsonConvert.DeserializeObject<List<SelectListItem>>(_Session.GetString(SessionDDListDsbChargeMap));
        }
        #endregion
    }

   
}
