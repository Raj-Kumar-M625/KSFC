using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.UI.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices
{
    public interface IEnquirySubmissionService
    {
        Task<AllDDLListDTO> GetAllEnquiryDropDownList();
        Task<AllDDLListDTO> getCascadeDDLForEditPrefill(int? distId, int? talukaId, int? hobliId);
        Task<List<SelectListItem>> PopulateSubLocationList(string locationType, int id);
        Task<List<SelectListItem>> PopulateFinanceTypeList(int id);
        Task<List<SelectListItem>> getAllAddressTypesFromDB();

        /// <summary>
        /// Save or Edit Basic details tabs
        /// </summary>
        /// <param name="basicDetailsDTO"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        Task<bool> SaveUnitDetailsBasicDetails(BasicDetailsDto basicDetailsDTO, Boolean isNew);
        Task<bool> SaveUnitDetailsAddressDetails(List<AddressDetailsDTO> addressDetailsList, Boolean isNew);
        Task<bool> SaveUnitDetailsBankDetails(BankDetailsDTO bankDetailsDTO, Boolean isNew);
        Task<bool> SaveUnitDetailsRegistrationDetails(List<RegistrationNoDetailsDTO> regDetailsList, Boolean isNew);
        /// <summary>
        /// save or edit promoter details tab
        /// </summary>
        /// <param name="promoterDetailsDTOsList"></param>
        /// <returns></returns>
        Task<bool> SavePromoterDetails(List<PromoterDetailsDTO> promoterDetailsDTOsList);
        Task<bool> UpdatePromoterDetails(List<PromoterDetailsDTO> promoterDetailsDTOsList);
        Task<bool> SaveorEditPromoterLiabilityDetails(List<PromoterLiabilityDetailsDTO> promoterliabilityDetailsDTOList);
        Task<bool> SaveorEditPromoterLiabilityNetWorth(List<PromoterNetWorthDetailsDTO> promoterNetWorthDetailsList);
        Task<bool> SaveorEditPromoterAssetsNetWorthDetails(List<PromoterAssetsNetWorthDTO> promoterassetsNetWorthDTOsList);
        /// <summary>
        /// save or edit guarentor details tab
        /// </summary>
        /// <param name="promoterAllDetailsDTO"></param>
        /// <returns></returns>
        Task<bool> SaveGuarantorDetails(List<GuarantorDetailsDTO> guarantorDetailsDTOsList);
        Task<bool> UpdateGuarantorDetails(List<GuarantorDetailsDTO> guarantorDetailsDTOsList);
        Task<bool> SaveorEditGuarantorLiabilityDetails(List<GuarantorLiabilityDetailsDTO> guarantorliabilityDetailsDTOList);
        Task<bool> SaveorEditGuarantorLiabilityNetWorth(List<GuarantorNetWorthDetailsDTO> guarantorNetWorthDetailsList);
        Task<bool> SaveorEditGuarantorAssetsNetWorthDetails(List<GuarantorAssetsNetWorthDTO> guarantorassetsNetWorthDTOsList);


        /// <summary>
        /// save or edit for associate and sister tab
        /// </summary>
        /// <param name="listAssociateDetailsDTO"></param>
        /// <returns></returns>
        Task<bool> SaveAssociateSisterDetails(List<SisterConcernDetailsDTO> listAssociateDetailsDTO);
        Task<bool> SaveAssociateSisterFYDetails(List<SisterConcernFinancialDetailsDTO> listAssociateFYDetailsDTO);
        /// <summary>
        /// save project details  tab service interfaces below
        /// </summary>
        /// <param name="capitalArrgmentDtls"></param>
        /// <returns></returns>
        Task<bool> saveCapitalDetails(ProjectWorkingCapitalDeatailsDTO capitalArrgmentDtls);
        Task<bool> saveProjectCostDetails(List<ProjectCostDetailsDTO> listprojectCosts);
        Task<bool> saveProjectMeansOfFinanceDetails(List<ProjectMeansOfFinanceDTO> listmeansOfFinance);
        Task<bool> saveProjectPrevYearFinDetailsDetails(List<ProjectFinancialYearDetailsDTO> listprevFYDetails);
        /// <summary>
        /// Security Details And Documents tab service interfaces below
        /// </summary>
        /// <param name="listSecurityDetails"></param>
        /// <returns></returns>
        Task<List<SecurityDetailsDTO>> SaveSecurityDetails(List<SecurityDetailsDTO> listSecurityDetails);
        Task<JsonResult> saveDeclarationDetails(Declaration declaration);
        Task<JsonResult> saveDocuments(List<byte[]> EncryptedFilesList);
        Task<EnquiryDTO> getEnquiryDetails(int enquiryId);
        Task<List<EnquirySummary>> GetAllEnquiries();
        Task<bool> DeleteEnquiry(int id);
        Task<JsonResult> DeleteEnquiryRegistrationDetails(int id);
        Task<JsonResult> DeleteEnquiryAddressDetails(int id);
        Task<(bool,string, int, string)> FileUpload(FileUploadDTO fileUpload);
        Task<bool> DeleteFile(int fileId);
        Task<byte[]> ViewFile(string fileId);
        Task<int> SubmitEnquiry(string sNote);
        Task<bool> UpdateEnquiryStatus(int id, int EnqStatus);
        Task<bool> DeletePromotorDetails(int id);
        Task<bool> DeleteGuarantor(int id);
        Task<bool> DeleteSisterConcernDetails(int id);
        Task<bool> UpdateAssociateSisterDetails(List<SisterConcernDetailsDTO> listAssociateDetailsDTO);
        Task<List<SelectListItem>> GetGenderTypes();
        Task<bool> UpdateAssociateSisterConcernDetail();
    }
}
