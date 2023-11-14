using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Razor;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails.AssetDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class LandInspectionAdController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        public LandInspectionAdController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;

        }

        [HttpGet]
        public IActionResult Create(long accountNumber, long InspectionId, byte OffcCd, string LoanSub)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                ViewBag.LoanAcc = accountNumber;
                ViewBag.InspectionId = InspectionId;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                var landInspectionDetails = _sessionManager.GetAllLandInspectionList();

                if (landInspectionDetails.Count != 0)
                {
                    //ViewBag.landInspectionDetails = landInspectionDetails.Where(e=>e.CreatedDate!=null).Last().DcLndSecCreated;
                    ViewBag.landInspectionDetails = landInspectionDetails.Last().DcLndSecCreated;
                    ViewBag.releasedetails = landInspectionDetails.Last().IrlSecValue;
                }
                else
                {
                    ViewBag.landInspectionDetails = "";
                    ViewBag.releasedetails = "";
                }

                var inspdetails = _sessionManager.GetAllInspectionDetail();
                var forfirstinspection = inspdetails.FirstOrDefault().DinNo;

                //foreach (var insp in landInspectionDetails)
                //{
                //    if (insp.DcLndIno == InspectionId)
                //    {
                //        ViewBag.firstbuildinginspection = true;
                //    }
                //}
                //var previousinsp = landInspectionDetails.Where(x => x.DcLndIno == InspectionId).ToList().Any();

                if (landInspectionDetails.Where(x => x.DcLndIno == InspectionId).Count() > 0 || forfirstinspection == InspectionId)
                {
                    ViewBag.firstbuildinginspection = false;
                }

                var previousinspection = inspdetails
                         .OrderByDescending(x => x.DinNo)
                         .Skip(1)
                          .FirstOrDefault();
                if (previousinspection != null && previousinspection.DinNo == InspectionId)
                {
                    ViewBag.firstbuildinginspection = false;
                }


                var AllLandType = _sessionManager.GetDDListLandType();
                ViewBag.LandType = AllLandType;

                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.LandresultViewPathAd + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmDchgLandDetDTO landInspection)
        {
            try
            {
                Random rand = new Random(); // <-- Make this static somewhere
                const int maxValue = 9999;
                var number = Convert.ToInt64(rand.Next(maxValue + 1).ToString("D4"));



                List<IdmDchgLandDetDTO> landInspectionDetails = new();

                if (_sessionManager.GetAllLandInspectionList() != null)
                    landInspectionDetails = _sessionManager.GetAllLandInspectionList();
                IdmDchgLandDetDTO list = new IdmDchgLandDetDTO();
                list.LoanAcc = landInspection.LoanAcc;
                list.LoanSub = landInspection.LoanSub;
                list.OffcCd = landInspection.OffcCd;
                list.DcLndArea = landInspection.DcLndArea;
                list.DcLndType = landInspection.DcLndType;
                var allLandTypes = _sessionManager.GetDDListLandType();
                var landType = allLandTypes.Where(x => x.Value == list.DcLndType.ToString());
                list.LandType = landType.First().Text;
                list.DcLndAmt = landInspection.DcLndAmt;
                list.DcLndDevCst = landInspection.DcLndDevCst;
                list.DcLndLandFinance = landInspection.DcLndLandFinance;
                list.DcLndStatChgDate = landInspection.DcLndStatChgDate;
                list.UniqueId = Guid.NewGuid().ToString();
                list.DcLndSecCreated = landInspection.DcLndSecCreated;
                list.DcLndDets = landInspection.DcLndDets;
                list.DcLndAqrdIndicator = landInspection.DcLndAqrdIndicator;
                list.CreatedBy = landInspection.CreatedBy;
                list.DcLndrefNo = number;
                list.CreatedDate = landInspection.CreatedDate;
                list.ModifiedBy = landInspection.ModifiedBy;
                list.ModifiedDate = landInspection.ModifiedDate;
                list.DcLndIno = landInspection.DcLndIno;
                list.IrlAreaIn = landInspection.IrlAreaIn;
                list.IrlSecValue = landInspection.IrlSecValue;
                list.IrlRelStat = Convert.ToInt32(landInspection.IrlRelStat);
                list.Action = (int)Constant.Create;
                list.IsActive = true;
                list.IsDeleted = false;
                landInspectionDetails.Add(list);
                ViewBag.LoanSub = list.LoanSub;
                ViewBag.OffcCd = list.OffcCd;
                ViewBag.InspectionId = landInspection.DcLndIno;
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                _sessionManager.SetLandInspectionList(landInspectionDetails);
                List<IdmDchgLandDetDTO> activeList = landInspectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.LandviewPathAd+ "_ViewAll", landInspectionDetails) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllLandInspectionList = _sessionManager.GetAllLandInspectionList();
                IdmDchgLandDetDTO landInspectionList = AllLandInspectionList.FirstOrDefault(x => x.UniqueId == id);
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);

                var AllLandType = _sessionManager.GetDDListLandType();
                ViewBag.LandType = AllLandType;
                return View(Constants.LandresultViewPathAd + Constants.ViewRecord, landInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult Edit(long InspectionId, int row, string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllLandInspectionList = _sessionManager.GetAllLandInspectionList();

                IdmDchgLandDetDTO landInspectionList = AllLandInspectionList.FirstOrDefault(x => x.UniqueId == id);
                if (landInspectionList != null)
                {
                    ViewBag.InspectionId = InspectionId;
                    ViewBag.LoanSub = landInspectionList.LoanSub;
                    ViewBag.OffcCd = landInspectionList.OffcCd;
                    ViewBag.AccountNumber = landInspectionList.LoanAcc;
                    ViewBag.row = row;
                    var AllLandType = _sessionManager.GetDDListLandType();
                    ViewBag.LandType = AllLandType;
                }

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.LandresultViewPathAd + Constants.editCs, landInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDchgLandDetDTO landInspection)
        {
            try
            {
                List<IdmDchgLandDetDTO> landInspectionDetails = new();
                if (_sessionManager.GetAllLandInspectionList() != null)
                    landInspectionDetails = _sessionManager.GetAllLandInspectionList();
                IdmDchgLandDetDTO landInspectionExist = landInspectionDetails.Find(x => x.UniqueId == id);

                if (landInspectionExist != null)
                {
                    landInspectionDetails.Remove(landInspectionExist);
                    var list = landInspectionExist;
                    list.LoanAcc = landInspection.LoanAcc;
                    list.OffcCd = landInspectionExist.OffcCd;
                    list.LoanSub = landInspectionExist.LoanSub;
                    list.DcLndArea = landInspection.DcLndArea;
                    list.DcLndAreaIn = landInspection.DcLndAreaIn;
                    list.DcLndType = landInspection.DcLndType;
                    var allLandTypes = _sessionManager.GetDDListLandType();
                    var landType = allLandTypes.Where(x => x.Value == list.DcLndType.ToString());
                    list.LandType = landType.First().Text;
                    list.DcLndAmt = landInspection.DcLndAmt;
                    list.DcLndDevCst = landInspection.DcLndDevCst;
                    list.DcLndLandFinance = landInspection.DcLndLandFinance;
                    list.DcLndStatChgDate = landInspection.DcLndStatChgDate;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.DcLndSecCreated = landInspection.DcLndSecCreated;
                    list.DcLndDets = landInspection.DcLndDets;
                    list.DcLndrefNo = landInspection.DcLndrefNo;
                    list.DcLndAqrdIndicator = landInspection.DcLndAqrdIndicator;
                    list.CreatedBy = landInspection.CreatedBy;
                    list.CreatedDate = landInspection.CreatedDate;
                    list.ModifiedBy = landInspection.ModifiedBy;
                    list.ModifiedDate = landInspection.ModifiedDate;
                    list.DcLndIno = landInspection.DcLndIno;
                    list.IrlAreaIn = landInspection.IrlAreaIn;
                    list.IrlSecValue = landInspection.IrlSecValue;
                    list.IrlRelStat = Convert.ToInt32(landInspection.IrlRelStat);
                    list.IrlId = landInspection.IrlId;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    if (landInspectionExist.DcLndRowId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;
                    }

                    landInspectionDetails.Add(list);
                    // landInspectionExist.IsDeleted = false;
                    // landInspectionExist.IsActive = false;
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InspectionId = list.DcLndIno;
                    ViewBag.firstinspection = true;


                    var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                    ViewBag.inspectiondetails = inspectiondetailslist;
                    _sessionManager.SetLandInspectionList(landInspectionDetails);

                    List<IdmDchgLandDetDTO> activeList = landInspectionDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();

                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.LandviewPathAd + Constants.ViewAll, landInspectionDetails) });
                }

                ViewBag.AccountNumber = landInspection.LoanAcc;
                ViewBag.LoanSub = landInspection.LoanSub;
                ViewBag.OffcCd = landInspection.OffcCd;
                ViewBag.InspectionId = landInspection.DcLndIno;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.LandviewPathAd + Constants.Edit, landInspection) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult Delete(long InspectionId, string id)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, id));
                IEnumerable<IdmDchgLandDetDTO> activeLand = new List<IdmDchgLandDetDTO>();
                var landInspectionList = _sessionManager.GetAllLandInspectionList();
                var itemToRemove = landInspectionList.Find(r => r.UniqueId == id);
                itemToRemove.Action = (int)Constant.Delete;
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;
                landInspectionList.Add(itemToRemove);
                _sessionManager.SetLandInspectionList(landInspectionList);
                if (landInspectionList.Count != 0)
                {
                    activeLand = landInspectionList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InspectionId = InspectionId;
                ViewBag.firstinspection = true;
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                string html = Helper.RenderRazorViewToString(this, Constants.LandviewPathAd + Constants.ViewAll, activeLand);
                return Json(new { isValid = true, html,delete = "Canceled" });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
