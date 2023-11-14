using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Services;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.UnitDetails
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class UnitDetailsController : Controller
    {
        private readonly SessionManager _sessionManager;
        private readonly EnquirySubmissionService _enquirySubmissionService;
        private readonly ILogger _logger;
        public UnitDetailsController(SessionManager sessionManager, EnquirySubmissionService enquirySubmissionService, ILogger logger)
        {
            _sessionManager = sessionManager;
            _enquirySubmissionService = enquirySubmissionService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult SaveBaDetails()
        {

            return View();
        }
        /// <summary>
        /// Adds  Bank Details Data to session 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveBankDetails(BankDetailsDTO model)
        {
            try
            {
                _logger.Information(string.Format("Started - SaveBankDetails HttpPost method for EnqBankId :{0} EnqtempId:{1} EnqAcctype:{2} EnqBankaccno:{3} EnqIfsc:{4} EnqAccName:{5} EnqBankname:{6} EnqBankbr:{7} UniqueId:{8} BankPinCode:{9}",
                   model.EnqBankId, model.EnqtempId, model.EnqAcctype, model.EnqBankaccno, model.EnqIfsc, model.EnqAccName, model.EnqBankname, model.EnqBankbr, model.UniqueId, model.BankPinCode));
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        _sessionManager.SetUDBankDetails(model);
                        _logger.Information(string.Format("Completed - SaveBankDetails HttpPost method for EnqBankId :{0} EnqtempId:{1} EnqAcctype:{2} EnqBankaccno:{3} EnqIfsc:{4} EnqAccName:{5} EnqBankname:{6} EnqBankbr:{7} UniqueId:{8} BankPinCode:{9}",
                   model.EnqBankId, model.EnqtempId, model.EnqAcctype, model.EnqBankaccno, model.EnqIfsc, model.EnqAccName, model.EnqBankname, model.EnqBankbr, model.UniqueId, model.BankPinCode));
                        return new JsonResult(new { isValid = true });
                    }
                }
                _logger.Information(string.Format("Completed - SaveBankDetails HttpPost method for EnqBankId :{0} EnqtempId:{1} EnqAcctype:{2} EnqBankaccno:{3} EnqIfsc:{4} EnqAccName:{5} EnqBankname:{6} EnqBankbr:{7} UniqueId:{8} BankPinCode:{9}",
                   model.EnqBankId, model.EnqtempId, model.EnqAcctype, model.EnqBankaccno, model.EnqIfsc, model.EnqAccName, model.EnqBankname, model.EnqBankbr, model.UniqueId, model.BankPinCode));
                return View();

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading SaveBankDetails HttpPost page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveUnitDetails(BasicDetailsDto basicDetailsDTO)
        {
            try
            {
                _logger.Information(string.Format("Started - SaveUnitDetails HttpPost method for PromoterPan:{0} EnqBdetId:{1} EnqtempId:{2} EnqApplName:{3} EnqAddress:{4} EnqPlace:{5} EnqPincode:{6} EnqEmail:{7} AddlLoan:{8} UnitName:{9} EnqRepayPeriod:{10} EnqLoanamt:{11} ConstCd:{12} IndCd:{13} ConstType:{14} PurpCd:{15} PurposeOfLoan:{16} SizeCd:{17} SizeOfFirm:{18} ProdCd:{19} ProdCode:{20} VilCd:{21} VillageName:{22} PremCd:{23} PremCode:{24} OffcCd:{25} OffcCode:{26} UniqueId:{27} TypeOfIndustry:{28} DistrictCd:{29} TalukaCd:{30} HobliCd:{31} District:{32} Taluk:{33} Hobli:{34} ",
                  basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Taluk, basicDetailsDTO.Hobli));
                EnquiryDTO enquiryDTO = new();
                if (ModelState.IsValid)
                {
                    if (enquiryDTO != null)
                    {
                        _sessionManager.SetUDPersonalDetails(enquiryDTO.UnitDetails.BasicDetails);

                        enquiryDTO.UnitDetails.ListAddressDetail = _sessionManager.GetAddressList();
                        enquiryDTO.UnitDetails.ListRegDetails = _sessionManager.GetRegistrationDetList();
                        enquiryDTO.UnitDetails.BankDetails = _sessionManager.GetUDBankDetails();

                        if (enquiryDTO.UnitDetails.ListAddressDetail != null)
                        {
                            _logger.Information(string.Format("Completed - SaveUnitDetails HttpPost method for PromoterPan:{0} EnqBdetId:{1} EnqtempId:{2} EnqApplName:{3} EnqAddress:{4} EnqPlace:{5} EnqPincode:{6} EnqEmail:{7} AddlLoan:{8} UnitName:{9} EnqRepayPeriod:{10} EnqLoanamt:{11} ConstCd:{12} IndCd:{13} ConstType:{14} PurpCd:{15} PurposeOfLoan:{16} SizeCd:{17} SizeOfFirm:{18} ProdCd:{19} ProdCode:{20} VilCd:{21} VillageName:{22} PremCd:{23} PremCode:{24} OffcCd:{25} OffcCode:{26} UniqueId:{27} TypeOfIndustry:{28} DistrictCd:{29} TalukaCd:{30} HobliCd:{31} District:{32} Taluk:{33} Hobli:{34} ",
                  basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Taluk, basicDetailsDTO.Hobli));
                            return Json(new { isValid = false, invalidAccordion = "Address", Message = "Please add all address types before submitting the form." });
                        }

                        if (enquiryDTO.UnitDetails.ListAddressDetail != null && enquiryDTO.UnitDetails.ListRegDetails != null && enquiryDTO.UnitDetails.BankDetails != null)
                        {
                            if (enquiryDTO.UnitDetails.ListAddressDetail.Count >= 3 && enquiryDTO.UnitDetails.ListRegDetails.Count >= 1)
                            {
                                //Calling Api to Save Data
                                var tasks = new List<Task>();
                                var task1 = await _enquirySubmissionService.SaveUnitDetailsBasicDetails(enquiryDTO.UnitDetails.BasicDetails, true);
                                var task2 = await _enquirySubmissionService.SaveUnitDetailsAddressDetails(enquiryDTO.UnitDetails.ListAddressDetail, true);
                                var task3 = await _enquirySubmissionService.SaveUnitDetailsBankDetails(enquiryDTO.UnitDetails.BankDetails, true);
                                var task4 = await _enquirySubmissionService.SaveUnitDetailsRegistrationDetails(enquiryDTO.UnitDetails.ListRegDetails, true);
                                //await Task.WhenAll(task1, task2, task3, task4);

                                _logger.Information(string.Format("Completed - SaveUnitDetails HttpPost method for PromoterPan:{0} EnqBdetId:{1} EnqtempId:{2} EnqApplName:{3} EnqAddress:{4} EnqPlace:{5} EnqPincode:{6} EnqEmail:{7} AddlLoan:{8} UnitName:{9} EnqRepayPeriod:{10} EnqLoanamt:{11} ConstCd:{12} IndCd:{13} ConstType:{14} PurpCd:{15} PurposeOfLoan:{16} SizeCd:{17} SizeOfFirm:{18} ProdCd:{19} ProdCode:{20} VilCd:{21} VillageName:{22} PremCd:{23} PremCode:{24} OffcCd:{25} OffcCode:{26} UniqueId:{27} TypeOfIndustry:{28} DistrictCd:{29} TalukaCd:{30} HobliCd:{31} District:{32} Taluk:{33} Hobli:{34} ",
                  basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Taluk, basicDetailsDTO.Hobli));
                                //var result = _enquirySubmissionService.SaveUnitDetails(model);
                                return Json(new { isValid = true });
                            }
                        }
                    }
                }
                _logger.Information(string.Format("Completed - SaveUnitDetails HttpPost method for PromoterPan:{0} EnqBdetId:{1} EnqtempId:{2} EnqApplName:{3} EnqAddress:{4} EnqPlace:{5} EnqPincode:{6} EnqEmail:{7} AddlLoan:{8} UnitName:{9} EnqRepayPeriod:{10} EnqLoanamt:{11} ConstCd:{12} IndCd:{13} ConstType:{14} PurpCd:{15} PurposeOfLoan:{16} SizeCd:{17} SizeOfFirm:{18} ProdCd:{19} ProdCode:{20} VilCd:{21} VillageName:{22} PremCd:{23} PremCode:{24} OffcCd:{25} OffcCode:{26} UniqueId:{27} TypeOfIndustry:{28} DistrictCd:{29} TalukaCd:{30} HobliCd:{31} District:{32} Taluk:{33} Hobli:{34} ",
                  basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Taluk, basicDetailsDTO.Hobli));
                return Json(new { isValid = false });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading SaveUnitDetails HttpPost page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
    }
}
