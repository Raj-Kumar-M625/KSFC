using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.LegalDocumentation.GuarantorDeed
{
    /// <summary>
    ///  Author: Manoj; Module: GuarantorDeed; Date:02/08/2022
    /// </summary>
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class GuarantorDeedController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
      

        public GuarantorDeedController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);

                var AllGuarantorDeedDetails = _sessionManager.GetAllGuarantorDeedList();
                IdmGuarantorDeedDetailsDTO GuarantorList = AllGuarantorDeedDetails.FirstOrDefault(x => x.IdmGuarDeedId == id);
            
                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == id && x.SubModuleType == Constants.GuarantorDeed && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.Documentlist = doc;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.guarantorresultViewPath +Constants.ViewRecord, GuarantorList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);
                var AllGuarantorDeedDetails = _sessionManager.GetAllGuarantorDeedList();
                if (AllGuarantorDeedDetails.Count != 0)
                {

                    ViewBag.ItemNumber = AllGuarantorDeedDetails.Select(x => x.DeedNo).ToList();
                }
                IdmGuarantorDeedDetailsDTO GuarantorList = AllGuarantorDeedDetails.Find(x => x.IdmGuarDeedId == id);

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == id && x.SubModuleType == Constants.GuarantorDeed && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.Documentlist = doc;
                ViewBag.SubModuleId = GuarantorList.IdmGuarDeedId;
                ViewBag.SubModuleType = Constants.GuarantorDeed;
                ViewBag.MainModule = Constants.LegalDocumentation;
                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.guarantorresultViewPath + Constants.update, GuarantorList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, IdmGuarantorDeedDetailsDTO Holder)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.GuarantorDeedDto,
                    id, Holder.DeedNo, Holder.DeedDesc, Holder.ExcecutionDate));

                if (ModelState.IsValid)
                {
                    IEnumerable<IdmGuarantorDeedDetailsDTO> activeHoldersList = new List<IdmGuarantorDeedDetailsDTO>();
                    List<IdmGuarantorDeedDetailsDTO> guarantorDeedlist = new();
                    if (_sessionManager.GetAllGuarantorDeedList() != null)
                        guarantorDeedlist = _sessionManager.GetAllGuarantorDeedList();

                    IdmGuarantorDeedDetailsDTO holderExist = guarantorDeedlist.Find(x => x.IdmGuarDeedId == id);
                    long? acc = 0;
                    if (holderExist != null)
                    {
                        guarantorDeedlist.Remove(holderExist);
                      
                        acc = holderExist.LoanAcc;
                        holderExist.DeedNo = Holder.DeedNo;
                        holderExist.DeedDesc = Holder.DeedDesc;
                        holderExist.ExcecutionDate = Holder.ExcecutionDate;
                        holderExist.Action = (int)Constant.Update;
                        guarantorDeedlist.Add(holderExist);

                        _sessionManager.SetGuarantorDeedList(guarantorDeedlist);
                        ViewBag.AccountNumber = holderExist.LoanAcc;
                        ViewBag.OffCd = holderExist.OffcCd;
                        ViewBag.LoanSub = holderExist.LoanSub;
                        if (holderExist != null)
                        {
                            activeHoldersList = guarantorDeedlist.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                        }
                    }
                      

                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.GuarantorDeedDto,
                    id, Holder.DeedNo, Holder.DeedDesc, Holder.ExcecutionDate));
                    return Json(new { isValid = true,data = acc,html = Helper.RenderRazorViewToString(this, Constants.guarntorviewPath +  Constants.ViewAll, activeHoldersList) });
                }

                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.GuarantorDeedDto,
                    id, Holder.DeedNo, Holder.DeedDesc, Holder.ExcecutionDate));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.guarntorviewPath +  Constants.ViewAll, Holder) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
       
    }
}
