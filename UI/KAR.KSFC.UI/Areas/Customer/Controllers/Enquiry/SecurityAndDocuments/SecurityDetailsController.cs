using System;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.SecurityAndDocuments
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class SecurityDetailsController : Controller
    {
        private const string resultViewPath = "~/Areas/Customer/Views/Enquiry/SecurityAndDocuments/SecurityDetails/";
        private const string viewPath = "../../Areas/Customer/Views/Enquiry/SecurityAndDocuments/SecurityDetails/";
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;
        public SecurityDetailsController(SessionManager sessionManager, ILogger logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }

        /// <summary>
        /// get method to view a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecord method for Id = " + id);
                ViewBag.ListTypeOfSecurity = _sessionManager.GetDDListTypeOfSecurity();
                ViewBag.ListDdlSecurityDetailsType = _sessionManager.GetDDListSecurityDetailsType();
                ViewBag.ListDdlRelationType = _sessionManager.GetDDListRelationType();

                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord method for Id = " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new SecurityDetailsDTO());
                }
                else
                {
                    var secDetailList = _sessionManager.GetSecurityDetailsList();
                    SecurityDetailsDTO sec = secDetailList.Where(x => x.EnqSecId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecord method for Id = " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", sec);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecord page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        /// <summary>
        /// get method to create or edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEdit(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEdit method for Id = " + id);
                ViewBag.ListTypeOfSecurity = _sessionManager.GetDDListTypeOfSecurity();
                ViewBag.ListDdlSecurityDetailsType = _sessionManager.GetDDListSecurityDetailsType();
                ViewBag.ListDdlRelationType = _sessionManager.GetDDListRelationType();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEdit method for Id = " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new SecurityDetailsDTO());
                }
                else
                {
                    var secDetailList = _sessionManager.GetSecurityDetailsList();
                    SecurityDetailsDTO sec = secDetailList.Where(x => x.EnqSecId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEdit method for Id = " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", sec);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
        /// <summary>
        /// post method to create or edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, SecurityDetailsDTO sec)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEdit method for Id: {0} EnqSecId: {1} EnqtempId :{2} SecCode :{3} SecCd :{4} EnqSecDesc :{5} EnqSecValue :{6} EnqSecName :{7} PromrelCd :{8} UniqueId :{9} Operation ",
                    id, sec.EnqSecId, sec.EnqtempId, sec.SecCode, sec.SecCd, sec.EnqSecDesc, sec.EnqSecValue, sec.EnqSecName, sec.PromrelCd, sec.UniqueId, sec.Operation));
                sec.EnqtempId = Convert.ToInt32(_sessionManager.GetNewEnqTempId()); ;
                List<SecurityDetailsDTO> secDetailList = new();

                if (ModelState.IsValid)
                {
                    var list = _sessionManager.GetDDListTypeOfSecurity();
                    var securityDetails = _sessionManager.GetDDListSecurityDetailsType();

                    sec.SecCodeNavigation = new() { SecDets = list.Where(X => X.Value == sec.SecCd.ToString()).FirstOrDefault().Text };
                    sec.SecCdNavigation = new() { SecDets = securityDetails.Where(X => X.Value == sec.SecCd.ToString()).FirstOrDefault().Text };

                    if (id == 0)
                    {
                        if (_sessionManager.GetSecurityDetailsList() != null)
                            secDetailList = _sessionManager.GetSecurityDetailsList();
                        sec.EnqSecId = secDetailList.Max(x => x.EnqSecId) + 1 ?? 1; //Increment ID
                        secDetailList.Add(sec);
                    }
                    else
                    {
                        secDetailList = _sessionManager.GetSecurityDetailsList();
                        secDetailList.Remove(secDetailList.Find(m => m.EnqSecId == id));
                        sec.EnqSecId = id;
                        secDetailList.Add(sec);
                    }
                    _sessionManager.SetSecurityDetailsList(secDetailList);
                    _logger.Information(string.Format("Completed - CreateOrEdit method for Id: {0} EnqSecId: {1} EnqtempId :{2} SecCode :{3} SecCd :{4} EnqSecDesc :{5} EnqSecValue :{6} EnqSecName :{7} PromrelCd :{8} UniqueId :{9} Operation ",
                    id, sec.EnqSecId, sec.EnqtempId, sec.SecCode, sec.SecCd, sec.EnqSecDesc, sec.EnqSecValue, sec.EnqSecName, sec.PromrelCd, sec.UniqueId, sec.Operation));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", secDetailList) });
                }
                _logger.Information(string.Format("Completed - CreateOrEdit method for Id: {0} EnqSecId: {1} EnqtempId :{2} SecCode :{3} SecCd :{4} EnqSecDesc :{5} EnqSecValue :{6} EnqSecName :{7} PromrelCd :{8} UniqueId :{9} Operation ",
                    id, sec.EnqSecId, sec.EnqtempId, sec.SecCode, sec.SecCd, sec.EnqSecDesc, sec.EnqSecValue, sec.EnqSecName, sec.PromrelCd, sec.UniqueId, sec.Operation));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", sec) });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
        /// <summary>
        /// get method to delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.Information("Started - Delete method for Id = " + id);
                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var secDetailList = _sessionManager.GetSecurityDetailsList();
                    var itemToRemove = secDetailList.Find(r => r.EnqSecId == id);
                    secDetailList.Remove(itemToRemove);
                    _sessionManager.SetSecurityDetailsList(secDetailList);
                    _logger.Information("Completed - Delete method for Id = " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", secDetailList) });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading Delete page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
    }
}
