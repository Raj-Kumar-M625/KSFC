using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.UnitDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
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
                _logger.Information(string.Format("Started - SaveBankDetails HttpPost with EnqBankaccno {0} EnqBankname :{1} EnqAccName :{2} EnqIfsc :{3} EnqAcctype :{4} UniqueId :{5}"
                                                    , model.EnqBankaccno, model.EnqBankname, model.EnqAccName, model.EnqIfsc, model.EnqAcctype, model.UniqueId));

                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        _sessionManager.SetUDBankDetails(model);
                        _logger.Information(string.Format("Completed - SaveBankDetails HttpPost with EnqBankaccno {0} EnqBankname :{1} EnqAccName :{2} EnqIfsc :{3} EnqAcctype :{4} UniqueId :{5}"
                                            , model.EnqBankaccno, model.EnqBankname, model.EnqAccName, model.EnqIfsc, model.EnqAcctype, model.UniqueId));

                        return new JsonResult(new { isValid = true });
                    }
                }
                _logger.Information(string.Format("Completed - SaveBankDetails HttpPost with EnqBankaccno {0} EnqBankname :{1} EnqAccName :{2} EnqIfsc :{3} EnqAcctype :{4} UniqueId :{5}"
                                            , model.EnqBankaccno, model.EnqBankname, model.EnqAccName, model.EnqIfsc, model.EnqAcctype, model.UniqueId));

                return View();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! SaveBankDetails HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveUnitDetails(BasicDetailsDto basicDetailsDTO)
        {
            try
            {
                _logger.Information(string.Format("Started - SaveUnitDetails HttpPost with EnqApplName {0} EnqEmail :{1} UnitName :{2} PromoterPan :{3} ProdCode :{4} UniqueId :{5}"
                                                          , basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqEmail, basicDetailsDTO.UnitName, basicDetailsDTO.PromoterPan, basicDetailsDTO.ProdCode, basicDetailsDTO.UniqueId));

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


                                //var result = _enquirySubmissionService.SaveUnitDetails(model);
                                _logger.Information(string.Format("Completed - SaveUnitDetails HttpPost with EnqApplName {0} EnqEmail :{1} UnitName :{2} PromoterPan :{3} ProdCode :{4} UniqueId :{5}"
                                                  , basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqEmail, basicDetailsDTO.UnitName, basicDetailsDTO.PromoterPan, basicDetailsDTO.ProdCode, basicDetailsDTO.UniqueId));

                                return Json(new { isValid = true });
                            }
                        }
                    }
                }
                return Json(new { isValid = false });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! SaveUnitDetails HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
