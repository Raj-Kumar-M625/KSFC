using System;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.UI.Utility;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.UI.Services.IServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using KAR.KSFC.Components.Common.Logging.Client;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.PromAndGuarDetails
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class PromoterController : Controller
    {
        private const string resultViewPath = "~/Areas/Customer/Views/Enquiry/PromAndGuarDetails/Promoter/";
        private const string viewPath = "../../Areas/Customer/Views/Enquiry/PromAndGuarDetails/Promoter/";
        private readonly SessionManager _sessionManager;
        private readonly IEnquirySubmissionService _enquirySubmissionService;
        private readonly IPanService _panService;
        private readonly ILogger _logger;
        public PromoterController(SessionManager sessionManager, IEnquirySubmissionService enquirySubmissionService, IPanService panService, ILogger logger)
        {
            _sessionManager = sessionManager;
            _enquirySubmissionService = enquirySubmissionService;
            _panService = panService;
            _logger = logger;
        }

        /// <summary>
        /// Get method to View a Promoter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecord method for Id = " + id);
                ViewBag.ListPromoterDesignation = _sessionManager.GetDDListPromoterDesignation();
                ViewBag.ListDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                ViewBag.Gender = await _enquirySubmissionService.GetGenderTypes();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord method for id = " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new PromoterDetailsDTO());
                }
                else
                {
                    var proDetailList = _sessionManager.GetPromoterDetailsList();
                    PromoterDetailsDTO pro = proDetailList.Where(x => x.EnqPromId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecord method for id = " + id);
                    if (pro.PromoterMaster.PromoterDob != null)
                        pro.PromoterMaster.Age = get_age(pro.PromoterMaster.PromoterDob.Value);
                    return View(resultViewPath + "ViewRecord.cshtml", pro);
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecord page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Get method to Create or Edit a Promoter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEdit method for Id = " + id);
                ViewBag.ListPromoterDesignation = _sessionManager.GetDDListPromoterDesignation();
                ViewBag.ListDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                ViewBag.Gender = await _enquirySubmissionService.GetGenderTypes();
                string FourthChar = _panService.GetPanFourthCharByUserName();
                decimal? EnqPromShare = null;
                ViewBag.ShareHoldingIsAutomatically = false;
                if (FourthChar == "P")
                {
                    EnqPromShare = 100;
                    ViewBag.ShareHoldingIsAutomatically = true;
                }

                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEdit method for Id = " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new PromoterDetailsDTO() { EnqPromShare = EnqPromShare });
                }
                else
                {
                    var proDetailList = _sessionManager.GetPromoterDetailsList();
                    PromoterDetailsDTO pro = proDetailList.Where(x => x.EnqPromId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEdit method for Id = " + id);
            if (pro.PromoterMaster.PromoterDob != null)
                pro.PromoterMaster.Age = get_age(pro.PromoterMaster.PromoterDob.Value);
            return View(resultViewPath + "CreateOrEdit.cshtml", pro);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// get Age by Dob
        /// </summary>
        /// <param name="dob"></param>
        /// <returns></returns>
        public string get_age(DateTime dob)
        {
            int age = 0;
            age = DateTime.Now.Subtract(dob).Days;
            age = age / 365;
            return age.ToString();
        }
        /// <summary>
        /// Post method to Create or Edit Promoter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pro"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(int id, PromoterDetailsDTO pro)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEdit method for Id: {0} EnqPromId :{1} EnqtempId :{2} PromoterCode :{3} EnqPromShare :{4} EnqPromExp :{5} EnqPromExpdet :{6} PdesigCd :{7} DomCd :{8} EnqCibil :{9} UniqueId :{10} PromCode :{11} PromOffc :{12} PromUnit :{13} PromName :{14} PromDesg :{15} PromSex :{16} PromAge :{17} PromShare :{18} PromDom1 :{19} PromClas1 :{20} PromClas2 :{21} PromQual1 :{22} PromQual2 :{23} PromExpYrs :{24} PromExpDet :{25} PromJnDt :{26} PromExDt :{27} PromExAppBy :{28} PromNriCountry :{29} PromPadr1 :{30} PromPadr2 :{31} PromPadr3 :{32} PromPadr4 :{33} PromTadr1 :{34} PromTadr2 :{35} PromTadr3 :{36} PromTadr4 :{37} PromMajor :{38} PromNwDets :{39} PanNo :{40} PassNo :{41} PromGuardian :{42} PromResTel :{43} PromMobile :{44} PromEmail :{45} PromPhyHandicap :{46} PromAadhaar :{47} PromDob :{48} PromExApprEmp :{49} PromoDomText",
                    id, pro.EnqPromId, pro.EnqtempId, pro.PromoterCode, pro.EnqPromShare, pro.EnqPromExp, pro.EnqPromExpdet, pro.PdesigCd, pro.DomCd, pro.EnqCibil, pro.UniqueId, pro.PromCode, pro.PromOffc, pro.PromUnit, pro.PromName, pro.PromDesg, pro.PromSex, pro.PromAge, pro.PromShare, pro.PromDom1, pro.PromClas1, pro.PromClas2, pro.PromQual1, pro.PromQual2, pro.PromExpYrs, pro.PromExpDet, pro.PromJnDt, pro.PromExDt, pro.PromExAppBy, pro.PromNriCountry, pro.PromPadr1, pro.PromPadr2, pro.PromPadr3, pro.PromPadr4, pro.PromTadr1, pro.PromTadr2, pro.PromTadr3, pro.PromTadr4, pro.PromMajor, pro.PromNwDets, pro.PanNo, pro.PassNo, pro.PromGuardian, pro.PromResTel, pro.PromMobile, pro.PromEmail, pro.PromPhyHandicap, pro.PromAadhaar, pro.PromDob, pro.PromExApprEmp, pro.PromoDomText));
                if (ModelState.IsValid)
                {
                    string FourthChar = _panService.GetPanFourthCharByUserName();
                    ViewBag.ShareHoldingIsAutomatically = false;
                    var promoterList = _sessionManager.GetPromoterDetailsList();
                    if (FourthChar == "P")
                    {
                        pro.EnqPromShare = 100;
                        ViewBag.ShareHoldingIsAutomatically = true;
                    }

                    List<PromoterDetailsDTO> proDetailList = new();

                    if (pro.PdesigCd != 0)
                    {
                        var desgList = _sessionManager.GetDDListPromoterDesignation();
                        pro.PromDesg = desgList.FirstOrDefault(x => x.Value == pro.PdesigCd.ToString()).Text;
                        pro.PdesigCdNavigation = new PromDesignationMasterDTO();
                        pro.PdesigCdNavigation.PdesigDets = desgList.FirstOrDefault(x => x.Value == pro.PdesigCd.ToString()).Text;
                    }
                    if (pro.DomCd != 0)
                    {
                        var domList = _sessionManager.GetDDListDomicileStatus();
                        pro.PromoDomText = domList.FirstOrDefault(x => x.Value == pro.DomCd.ToString()).Text;
                    }

                    if (id == 0)
                    {
                        proDetailList.Add(pro);
                        await _enquirySubmissionService.SavePromoterDetails(proDetailList);
                    }
                    else
                    {
                        var itemtoUpdate = new List<PromoterDetailsDTO>();
                        itemtoUpdate.Add(pro);
                        await _enquirySubmissionService.UpdatePromoterDetails(itemtoUpdate);
                    }

                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();
                    if (FourthChar != "P")
                    {
                        var NonProprietoryShareHolding = CalculateShareHoldingForNonProprietory(promoterAllDetailsDTO.ListPromoters);
                        if (NonProprietoryShareHolding == 100)
                        {
                            ViewBag.ShareHoldingIsAutomatically = true;
                        }
                    }
                    _logger.Information(string.Format("Completed - CreateOrEdit method for Id: {0} EnqPromId :{1} EnqtempId :{2} PromoterCode :{3} EnqPromShare :{4} EnqPromExp :{5} EnqPromExpdet :{6} PdesigCd :{7} DomCd :{8} EnqCibil :{9} UniqueId :{10} PromCode :{11} PromOffc :{12} PromUnit :{13} PromName :{14} PromDesg :{15} PromSex :{16} PromAge :{17} PromShare :{18} PromDom1 :{19} PromClas1 :{20} PromClas2 :{21} PromQual1 :{22} PromQual2 :{23} PromExpYrs :{24} PromExpDet :{25} PromJnDt :{26} PromExDt :{27} PromExAppBy :{28} PromNriCountry :{29} PromPadr1 :{30} PromPadr2 :{31} PromPadr3 :{32} PromPadr4 :{33} PromTadr1 :{34} PromTadr2 :{35} PromTadr3 :{36} PromTadr4 :{37} PromMajor :{38} PromNwDets :{39} PanNo :{40} PassNo :{41} PromGuardian :{42} PromResTel :{43} PromMobile :{44} PromEmail :{45} PromPhyHandicap :{46} PromAadhaar :{47} PromDob :{48} PromExApprEmp :{49} PromoDomText",
                    id, pro.EnqPromId, pro.EnqtempId, pro.PromoterCode, pro.EnqPromShare, pro.EnqPromExp, pro.EnqPromExpdet, pro.PdesigCd, pro.DomCd, pro.EnqCibil, pro.UniqueId, pro.PromCode, pro.PromOffc, pro.PromUnit, pro.PromName, pro.PromDesg, pro.PromSex, pro.PromAge, pro.PromShare, pro.PromDom1, pro.PromClas1, pro.PromClas2, pro.PromQual1, pro.PromQual2, pro.PromExpYrs, pro.PromExpDet, pro.PromJnDt, pro.PromExDt, pro.PromExAppBy, pro.PromNriCountry, pro.PromPadr1, pro.PromPadr2, pro.PromPadr3, pro.PromPadr4, pro.PromTadr1, pro.PromTadr2, pro.PromTadr3, pro.PromTadr4, pro.PromMajor, pro.PromNwDets, pro.PanNo, pro.PassNo, pro.PromGuardian, pro.PromResTel, pro.PromMobile, pro.PromEmail, pro.PromPhyHandicap, pro.PromAadhaar, pro.PromDob, pro.PromExApprEmp, pro.PromoDomText));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
                }
                _logger.Information(string.Format("Completed - CreateOrEdit method for Id: {0} EnqPromId :{1} EnqtempId :{2} PromoterCode :{3} EnqPromShare :{4} EnqPromExp :{5} EnqPromExpdet :{6} PdesigCd :{7} DomCd :{8} EnqCibil :{9} UniqueId :{10} PromCode :{11} PromOffc :{12} PromUnit :{13} PromName :{14} PromDesg :{15} PromSex :{16} PromAge :{17} PromShare :{18} PromDom1 :{19} PromClas1 :{20} PromClas2 :{21} PromQual1 :{22} PromQual2 :{23} PromExpYrs :{24} PromExpDet :{25} PromJnDt :{26} PromExDt :{27} PromExAppBy :{28} PromNriCountry :{29} PromPadr1 :{30} PromPadr2 :{31} PromPadr3 :{32} PromPadr4 :{33} PromTadr1 :{34} PromTadr2 :{35} PromTadr3 :{36} PromTadr4 :{37} PromMajor :{38} PromNwDets :{39} PanNo :{40} PassNo :{41} PromGuardian :{42} PromResTel :{43} PromMobile :{44} PromEmail :{45} PromPhyHandicap :{46} PromAadhaar :{47} PromDob :{48} PromExApprEmp :{49} PromoDomText",
                    id, pro.EnqPromId, pro.EnqtempId, pro.PromoterCode, pro.EnqPromShare, pro.EnqPromExp, pro.EnqPromExpdet, pro.PdesigCd, pro.DomCd, pro.EnqCibil, pro.UniqueId, pro.PromCode, pro.PromOffc, pro.PromUnit, pro.PromName, pro.PromDesg, pro.PromSex, pro.PromAge, pro.PromShare, pro.PromDom1, pro.PromClas1, pro.PromClas2, pro.PromQual1, pro.PromQual2, pro.PromExpYrs, pro.PromExpDet, pro.PromJnDt, pro.PromExDt, pro.PromExAppBy, pro.PromNriCountry, pro.PromPadr1, pro.PromPadr2, pro.PromPadr3, pro.PromPadr4, pro.PromTadr1, pro.PromTadr2, pro.PromTadr3, pro.PromTadr4, pro.PromMajor, pro.PromNwDets, pro.PanNo, pro.PassNo, pro.PromGuardian, pro.PromResTel, pro.PromMobile, pro.PromEmail, pro.PromPhyHandicap, pro.PromAadhaar, pro.PromDob, pro.PromExApprEmp, pro.PromoDomText));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, resultViewPath + "CreateOrEdit", pro) });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private decimal? CalculateShareHoldingForNonProprietory(List<PromoterDetailsDTO> list)
        {
            decimal? shareholding = 0;
            if (list.Count > 0)
            {
                shareholding = list.Sum(x => x.EnqPromShare);
            }
            return shareholding;
        }
        [HttpPost]
        public ActionResult CheckNonProprirtoryShareholding(int id, PromoterDetailsDTO pro)
        {
            try
            {
                _logger.Information(string.Format("Started - CheckNonProprirtoryShareholding method for Id: {0} EnqPromId :{1} EnqtempId :{2} PromoterCode :{3} EnqPromShare :{4} EnqPromExp :{5} EnqPromExpdet :{6} PdesigCd :{7} DomCd :{8} EnqCibil :{9} UniqueId :{10} PromCode :{11} PromOffc :{12} PromUnit :{13} PromName :{14} PromDesg :{15} PromSex :{16} PromAge :{17} PromShare :{18} PromDom1 :{19} PromClas1 :{20} PromClas2 :{21} PromQual1 :{22} PromQual2 :{23} PromExpYrs :{24} PromExpDet :{25} PromJnDt :{26} PromExDt :{27} PromExAppBy :{28} PromNriCountry :{29} PromPadr1 :{30} PromPadr2 :{31} PromPadr3 :{32} PromPadr4 :{33} PromTadr1 :{34} PromTadr2 :{35} PromTadr3 :{36} PromTadr4 :{37} PromMajor :{38} PromNwDets :{39} PanNo :{40} PassNo :{41} PromGuardian :{42} PromResTel :{43} PromMobile :{44} PromEmail :{45} PromPhyHandicap :{46} PromAadhaar :{47} PromDob :{48} PromExApprEmp :{49} PromoDomText",
                        id, pro.EnqPromId, pro.EnqtempId, pro.PromoterCode, pro.EnqPromShare, pro.EnqPromExp, pro.EnqPromExpdet, pro.PdesigCd, pro.DomCd, pro.EnqCibil, pro.UniqueId, pro.PromCode, pro.PromOffc, pro.PromUnit, pro.PromName, pro.PromDesg, pro.PromSex, pro.PromAge, pro.PromShare, pro.PromDom1, pro.PromClas1, pro.PromClas2, pro.PromQual1, pro.PromQual2, pro.PromExpYrs, pro.PromExpDet, pro.PromJnDt, pro.PromExDt, pro.PromExAppBy, pro.PromNriCountry, pro.PromPadr1, pro.PromPadr2, pro.PromPadr3, pro.PromPadr4, pro.PromTadr1, pro.PromTadr2, pro.PromTadr3, pro.PromTadr4, pro.PromMajor, pro.PromNwDets, pro.PanNo, pro.PassNo, pro.PromGuardian, pro.PromResTel, pro.PromMobile, pro.PromEmail, pro.PromPhyHandicap, pro.PromAadhaar, pro.PromDob, pro.PromExApprEmp, pro.PromoDomText));
                string FourthChar = _panService.GetPanFourthCharByUserName();
                var promoterList = _sessionManager.GetPromoterDetailsList();
                if (FourthChar != "P" && promoterList != null && promoterList.Count > 0)
                {
                    var itemtoAddinShareHolding = promoterList.Where(x => x.EnqPromId != pro.EnqPromId).ToList();
                    var NonProprietoryShareHolding = CalculateShareHoldingForNonProprietory(itemtoAddinShareHolding);
                    NonProprietoryShareHolding = pro.EnqPromShare + NonProprietoryShareHolding;
                    if (NonProprietoryShareHolding > 100)
                    {
                        _logger.Information(string.Format("Completed - CheckNonProprirtoryShareholding method for Id: {0} EnqPromId :{1} EnqtempId :{2} PromoterCode :{3} EnqPromShare :{4} EnqPromExp :{5} EnqPromExpdet :{6} PdesigCd :{7} DomCd :{8} EnqCibil :{9} UniqueId :{10} PromCode :{11} PromOffc :{12} PromUnit :{13} PromName :{14} PromDesg :{15} PromSex :{16} PromAge :{17} PromShare :{18} PromDom1 :{19} PromClas1 :{20} PromClas2 :{21} PromQual1 :{22} PromQual2 :{23} PromExpYrs :{24} PromExpDet :{25} PromJnDt :{26} PromExDt :{27} PromExAppBy :{28} PromNriCountry :{29} PromPadr1 :{30} PromPadr2 :{31} PromPadr3 :{32} PromPadr4 :{33} PromTadr1 :{34} PromTadr2 :{35} PromTadr3 :{36} PromTadr4 :{37} PromMajor :{38} PromNwDets :{39} PanNo :{40} PassNo :{41} PromGuardian :{42} PromResTel :{43} PromMobile :{44} PromEmail :{45} PromPhyHandicap :{46} PromAadhaar :{47} PromDob :{48} PromExApprEmp :{49} PromoDomText",
                        id, pro.EnqPromId, pro.EnqtempId, pro.PromoterCode, pro.EnqPromShare, pro.EnqPromExp, pro.EnqPromExpdet, pro.PdesigCd, pro.DomCd, pro.EnqCibil, pro.UniqueId, pro.PromCode, pro.PromOffc, pro.PromUnit, pro.PromName, pro.PromDesg, pro.PromSex, pro.PromAge, pro.PromShare, pro.PromDom1, pro.PromClas1, pro.PromClas2, pro.PromQual1, pro.PromQual2, pro.PromExpYrs, pro.PromExpDet, pro.PromJnDt, pro.PromExDt, pro.PromExAppBy, pro.PromNriCountry, pro.PromPadr1, pro.PromPadr2, pro.PromPadr3, pro.PromPadr4, pro.PromTadr1, pro.PromTadr2, pro.PromTadr3, pro.PromTadr4, pro.PromMajor, pro.PromNwDets, pro.PanNo, pro.PassNo, pro.PromGuardian, pro.PromResTel, pro.PromMobile, pro.PromEmail, pro.PromPhyHandicap, pro.PromAadhaar, pro.PromDob, pro.PromExApprEmp, pro.PromoDomText));
                        return Json(new { isValid = false });
                    }
                }
                _logger.Information(string.Format("Completed - CheckNonProprirtoryShareholding method for Id: {0} EnqPromId :{1} EnqtempId :{2} PromoterCode :{3} EnqPromShare :{4} EnqPromExp :{5} EnqPromExpdet :{6} PdesigCd :{7} DomCd :{8} EnqCibil :{9} UniqueId :{10} PromCode :{11} PromOffc :{12} PromUnit :{13} PromName :{14} PromDesg :{15} PromSex :{16} PromAge :{17} PromShare :{18} PromDom1 :{19} PromClas1 :{20} PromClas2 :{21} PromQual1 :{22} PromQual2 :{23} PromExpYrs :{24} PromExpDet :{25} PromJnDt :{26} PromExDt :{27} PromExAppBy :{28} PromNriCountry :{29} PromPadr1 :{30} PromPadr2 :{31} PromPadr3 :{32} PromPadr4 :{33} PromTadr1 :{34} PromTadr2 :{35} PromTadr3 :{36} PromTadr4 :{37} PromMajor :{38} PromNwDets :{39} PanNo :{40} PassNo :{41} PromGuardian :{42} PromResTel :{43} PromMobile :{44} PromEmail :{45} PromPhyHandicap :{46} PromAadhaar :{47} PromDob :{48} PromExApprEmp :{49} PromoDomText",
                        id, pro.EnqPromId, pro.EnqtempId, pro.PromoterCode, pro.EnqPromShare, pro.EnqPromExp, pro.EnqPromExpdet, pro.PdesigCd, pro.DomCd, pro.EnqCibil, pro.UniqueId, pro.PromCode, pro.PromOffc, pro.PromUnit, pro.PromName, pro.PromDesg, pro.PromSex, pro.PromAge, pro.PromShare, pro.PromDom1, pro.PromClas1, pro.PromClas2, pro.PromQual1, pro.PromQual2, pro.PromExpYrs, pro.PromExpDet, pro.PromJnDt, pro.PromExDt, pro.PromExAppBy, pro.PromNriCountry, pro.PromPadr1, pro.PromPadr2, pro.PromPadr3, pro.PromPadr4, pro.PromTadr1, pro.PromTadr2, pro.PromTadr3, pro.PromTadr4, pro.PromMajor, pro.PromNwDets, pro.PanNo, pro.PassNo, pro.PromGuardian, pro.PromResTel, pro.PromMobile, pro.PromEmail, pro.PromPhyHandicap, pro.PromAadhaar, pro.PromDob, pro.PromExApprEmp, pro.PromoDomText));
                return Json(new { isValid = true });


            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CheckNonProprirtoryShareholding page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Post method to delete a Promoter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
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
                    var proAssetDetailsList = _sessionManager.GetPromoterAssetList();
                    var proLiaDetailsList = _sessionManager.GetPromoterLiabilityList();

                    var proDetailList = _sessionManager.GetPromoterDetailsList();
                    var itemToRemove = proDetailList.Find(r => r.EnqPromId == id);

                    if (proAssetDetailsList != null)
                    {
                        var ifProAssetExist = proAssetDetailsList.Find(x => x.PromoterCode == itemToRemove.PromoterCode);
                        if (ifProAssetExist != null)
                        {
                            ViewBag.ErrorProAssLiaExist = "Please delete the promoter assets and liabilities first in order to delete a promoter.";
                            PromoterAllDetailsDTO promoterAllDetailsDTOExist = PromoterAssetLiabilityNetWorth();
                            _logger.Information("Completed - Delete method for  Id =" + id);
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTOExist) });
                        }
                    }
                    if (proLiaDetailsList != null)
                    {
                        var ifProLiabilityExist = proLiaDetailsList.Find(x => x.PromoterCode == itemToRemove.PromoterCode);
                        if (ifProLiabilityExist != null)
                        {
                            ViewBag.ErrorProAssLiaExist = "Please delete the promoter assets and liabilities first in order to delete a promoter.";
                            PromoterAllDetailsDTO promoterAllDetailsDTOExist = PromoterAssetLiabilityNetWorth();
                            _logger.Information("Completed - Delete method for Id = " + id);
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTOExist) });
                        }
                    }

                    proDetailList.Remove(itemToRemove);
                    _sessionManager.SetPromoterDetailsList(proDetailList);
                    await _enquirySubmissionService.DeletePromotorDetails(id);
                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();
                    _logger.Information("Completed - Delete method for Id = " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
                }


            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Get method to view Promoter asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecordAsset(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecordAsset method for Id = " + id);
                ViewBag.PromoterDetailsList = _sessionManager.GetPromoterDetailsList();
                ViewBag.ListPromAndGuarAssetType = _sessionManager.GetDDListPromAndGuarAssetType();
                ViewBag.ListPromAndGuarAssetCategory = _sessionManager.GetDDListPromAndGuarAssetCategory();
                ViewBag.ListModeOfAcquire = _sessionManager.GetDDListModeOfAcquire();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecordAsset method for Id = " + id);
                    return View(resultViewPath + "ViewRecordAsset.cshtml", new PromoterAssetsNetWorthDTO());
                }
                else
                {
                    var proAssetList = _sessionManager.GetPromoterAssetList();// JsonConvert.DeserializeObject<List<AssetDetails>>(HttpContext.Session.GetString("PromoterAssetList"));
                    PromoterAssetsNetWorthDTO pro = proAssetList.Where(x => x.EnqPromassetId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecordAsset method for Id = " + id);
                    return View(resultViewPath + "ViewRecordAsset.cshtml", pro);
                }


            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecordAsset page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Get method to create or edit Promoter asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEditAsset(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEditAsset method for Id = " + id);
                ViewBag.PromoterDetailsList = _sessionManager.GetPromoterDetailsList();
                ViewBag.ListPromAndGuarAssetType = _sessionManager.GetDDListPromAndGuarAssetType();
                ViewBag.ListPromAndGuarAssetCategory = _sessionManager.GetDDListPromAndGuarAssetCategory();

                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEditAsset method for Id =  " + id);
                    return View(resultViewPath + "CreateOrEditAsset.cshtml", new PromoterAssetsNetWorthDTO());
                }
                else
                {
                    var proAssetList = _sessionManager.GetPromoterAssetList();
                    PromoterAssetsNetWorthDTO pro = proAssetList.Where(x => x.EnqPromassetId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEditAsset method for Id = " + id);
                    return View(resultViewPath + "CreateOrEditAsset.cshtml", pro);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// post method to create or edit Promoter asset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="proAsset"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEditAsset(int id, PromoterAssetsNetWorthDTO proAsset)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEditAsset method for Id: {0} EnqPromassetId :{1} EnqtempId :{2} PromoterCode :{3} AssetcatCd :{4} AssettypeCd :{5} EnqAssetDesc :{6} EnqAssetValue :{7} EnqAssetSiteno :{8} EnqAssetAddr :{9} EnqAssetDim :{10} EnqAssetArea :{11} UniqueId :{12} EnqPromId",
                    id, proAsset.EnqPromassetId, proAsset.EnqtempId, proAsset.PromoterCode, proAsset.AssetcatCd, proAsset.AssettypeCd, proAsset.EnqAssetDesc, proAsset.EnqAssetValue, proAsset.EnqAssetSiteno, proAsset.EnqAssetAddr, proAsset.EnqAssetDim, proAsset.EnqAssetArea, proAsset.UniqueId, proAsset.EnqPromId));
                List<PromoterAssetsNetWorthDTO> proAssetList = new();
                if (ModelState.IsValid)
                {
                    var promoterList = _sessionManager.GetPromoterDetailsList();
                    var catList = _sessionManager.GetDDListPromAndGuarAssetCategory();
                    var typelist = _sessionManager.GetDDListPromAndGuarAssetType();
                    proAsset.AssetCategoryMasterDTO = new AssetCategoryMasterDTO()
                    {
                        AssetcatCd = proAsset.AssetcatCd,
                        AssetcatDets = catList.FirstOrDefault(x => x.Value == proAsset.AssetcatCd.ToString()).Text
                    };
                    proAsset.AssetTypeMasterDTO = new AssetTypeMasterDTO()
                    {
                        AssettypeCd = proAsset.AssettypeCd,
                        AssettypeDets = typelist.FirstOrDefault(x => x.Value == proAsset.AssettypeCd.ToString()).Text
                    };
                    proAsset.PromoterMasterDTO = promoterList.FirstOrDefault(x => x.PromoterCode == proAsset.PromoterCode).PromoterMaster;
                    if (_sessionManager.GetPromoterAssetList() != null)
                        proAssetList = _sessionManager.GetPromoterAssetList();

                    if (id == 0)
                    {
                        proAsset.EnqPromassetId = proAssetList.Max(x => x.EnqPromassetId) + 1 ?? 1; //Increment ID
                        proAssetList.Add(proAsset);
                    }
                    else
                    {
                        proAssetList.Remove(proAssetList.Find(m => m.EnqPromassetId == id));
                        proAssetList.Add(proAsset);
                    }
                    _sessionManager.SetPromoterAssetList(proAssetList);
                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();
                    _logger.Information(string.Format("Completed - CreateOrEditAsset method for Id: {0} EnqPromassetId :{1} EnqtempId :{2} PromoterCode :{3} AssetcatCd :{4} AssettypeCd :{5} EnqAssetDesc :{6} EnqAssetValue :{7} EnqAssetSiteno :{8} EnqAssetAddr :{9} EnqAssetDim :{10} EnqAssetArea :{11} UniqueId :{12} EnqPromId",
                        id, proAsset.EnqPromassetId, proAsset.EnqtempId, proAsset.PromoterCode, proAsset.AssetcatCd, proAsset.AssettypeCd, proAsset.EnqAssetDesc, proAsset.EnqAssetValue, proAsset.EnqAssetSiteno, proAsset.EnqAssetAddr, proAsset.EnqAssetDim, proAsset.EnqAssetArea, proAsset.UniqueId, proAsset.EnqPromId));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
                }
                _logger.Information(string.Format("Completed - CreateOrEditAsset method for Id: {0} EnqPromassetId :{1} EnqtempId :{2} PromoterCode :{3} AssetcatCd :{4} AssettypeCd :{5} EnqAssetDesc :{6} EnqAssetValue :{7} EnqAssetSiteno :{8} EnqAssetAddr :{9} EnqAssetDim :{10} EnqAssetArea :{11} UniqueId :{12} EnqPromId",
                        id, proAsset.EnqPromassetId, proAsset.EnqtempId, proAsset.PromoterCode, proAsset.AssetcatCd, proAsset.AssettypeCd, proAsset.EnqAssetDesc, proAsset.EnqAssetValue, proAsset.EnqAssetSiteno, proAsset.EnqAssetAddr, proAsset.EnqAssetDim, proAsset.EnqAssetArea, proAsset.UniqueId, proAsset.EnqPromId));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", proAsset) });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditAsset page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to create or edit Promoter asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAsset(int id)
        {
            try
            {
                _logger.Information("Started - DeleteAsset method for Id = " + id);
                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var proAssetList = _sessionManager.GetPromoterAssetList();// JsonConvert.DeserializeObject<List<AssetDetails>>(HttpContext.Session.GetString("PromoterAssetList"));

                    var itemToRemove = proAssetList.Find(r => r.EnqPromassetId == id);
                    proAssetList.Remove(itemToRemove);
                    _sessionManager.SetPromoterAssetList(proAssetList);

                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();
                    _logger.Information("Completed - DeleteAsset method for Id = " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteAsset page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Get method to View Promoter liability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecordLiability(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecordLiability method for Id = " + id);
                ViewBag.PromoterDetailsList = _sessionManager.GetPromoterDetailsList();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecordLiability method for Id = " + id);
                    return View(resultViewPath + "ViewRecordLiability.cshtml", new PromoterLiabilityDetailsDTO());
                }
                else
                {
                    var proLiaDetailList = _sessionManager.GetPromoterLiabilityList();
                    PromoterLiabilityDetailsDTO liability = proLiaDetailList.Where(x => x.EnqPromliabId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecordLiability method for Id = " + id);
                    return View(resultViewPath + "ViewRecordLiability.cshtml", liability);
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecordLiability page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Get method to create or edit Promoter liability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEditLiability(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEditLiability method for Id = " + id);
                ViewBag.PromoterDetailsList = _sessionManager.GetPromoterDetailsList();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEditLiability method for Id = " + id);
                    return View(resultViewPath + "CreateOrEditLiability.cshtml", new PromoterLiabilityDetailsDTO());
                }

                else
                {

                    var proLiaDetailList = _sessionManager.GetPromoterLiabilityList();
                    PromoterLiabilityDetailsDTO liability = proLiaDetailList.Where(x => x.EnqPromliabId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEditLiability method for Id = " + id);
                    return View(resultViewPath + "CreateOrEditLiability.cshtml", liability);
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditLiability page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to create or edit Promoter liability
        /// </summary>
        /// <param name="id"></param>
        /// <param name="liability"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEditLiability(int id, PromoterLiabilityDetailsDTO liability)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEditLiability method for Id: {0} EnqPromliabId :{1} PromCode :{2} EnqtempId :{3} PromoterCode :{4} EnqLiabDesc :{5} EnqLiabValue :{6} UniqueId :{7} EnqPromId",
                    id, liability.EnqPromliabId, liability.PromCode, liability.EnqtempId, liability.PromoterCode, liability.EnqLiabDesc, liability.EnqLiabValue, liability.UniqueId, liability.EnqPromId));
                List<PromoterLiabilityDetailsDTO> proLiaDetailsList = new();

                if (ModelState.IsValid)
                {
                    var promoterList = _sessionManager.GetPromoterDetailsList();
                    liability.PromoterMasterDTO = promoterList.FirstOrDefault(x => x.PromoterCode == liability.PromoterCode).PromoterMaster;

                    if (_sessionManager.GetPromoterLiabilityList() != null)
                        proLiaDetailsList = _sessionManager.GetPromoterLiabilityList();
                    if (id == 0)
                    {

                        liability.EnqPromliabId = proLiaDetailsList.Max(x => x.EnqPromliabId) + 1 ?? 1;
                        proLiaDetailsList.Add(liability);
                    }
                    else
                    {

                        proLiaDetailsList.Remove(proLiaDetailsList.Find(m => m.EnqPromliabId == id));
                        liability.EnqPromliabId = id;
                        liability.PromoterMasterDTO = promoterList.Find(x => x.PromoterCode == liability.PromoterCode).PromoterMaster;
                        proLiaDetailsList.Add(liability);
                    }
                    _sessionManager.SetPromoterLiabilityList(proLiaDetailsList);
                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();
                    _logger.Information(string.Format("Completed - CreateOrEditLiability method for Id: {0} EnqPromliabId :{1} PromCode :{2} EnqtempId :{3} PromoterCode :{4} EnqLiabDesc :{5} EnqLiabValue :{6} UniqueId :{7} EnqPromId",
                    id, liability.EnqPromliabId, liability.PromCode, liability.EnqtempId, liability.PromoterCode, liability.EnqLiabDesc, liability.EnqLiabValue, liability.UniqueId, liability.EnqPromId));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
                }
                _logger.Information(string.Format("Completed - CreateOrEditLiability method for Id: {0} EnqPromliabId :{1} PromCode :{2} EnqtempId :{3} PromoterCode :{4} EnqLiabDesc :{5} EnqLiabValue :{6} UniqueId :{7} EnqPromId",
                    id, liability.EnqPromliabId, liability.PromCode, liability.EnqtempId, liability.PromoterCode, liability.EnqLiabDesc, liability.EnqLiabValue, liability.UniqueId, liability.EnqPromId));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", liability) });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditLiability page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Post method to delete a Promoter liability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLiability(int id)
        {
            try
            {
                _logger.Information("Started - DeleteLiability method for Id = " + id);
                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var proLiaDetailsList = _sessionManager.GetPromoterLiabilityList();

                    var itemToRemove = proLiaDetailsList.Find(r => r.EnqPromliabId == id);
                    proLiaDetailsList.Remove(itemToRemove);
                    _sessionManager.SetPromoterLiabilityList(proLiaDetailsList);
                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();
                    _logger.Information("Completed - DeleteLiability method for Id = " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteLiability page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Calculates real time Asset, Liability and NetWorth from Session values
        /// </summary>
        /// <returns></returns>
        public PromoterAllDetailsDTO PromoterAssetLiabilityNetWorth()
        {
            //ModelState.Clear();   
            decimal totalAssets = 0;
            decimal totalLiabilities = 0;
            //Getting promoter list
            var promoterList = _sessionManager.GetPromoterDetailsList();
            var proAssetDetailsList = _sessionManager.GetPromoterAssetList();
            var proLiaDetailsList = _sessionManager.GetPromoterLiabilityList();
            //Calculating Asset
            List<PromoterAssetsNetWorthDTO> promoterAssetList = new List<PromoterAssetsNetWorthDTO>();
            List<PromoterLiabilityDetailsDTO> PromoterLiabilityList = new List<PromoterLiabilityDetailsDTO>();
            List<PromoterNetWorthDetailsDTO> networthList = new List<PromoterNetWorthDetailsDTO>();

            if (proAssetDetailsList != null)
            {
                foreach (var asset in proAssetDetailsList)
                {
                    //Update PromoterName in Assets if promoter name has been changed in Promoter List by user.
                    asset.PromoterMasterDTO = promoterList.FirstOrDefault(d => d.PromoterCode == asset.PromoterCode).PromoterMaster;
                    //Find if One Promoter has multiple assets
                    PromoterAssetsNetWorthDTO assetDto = new PromoterAssetsNetWorthDTO();
                    assetDto = promoterAssetList.FirstOrDefault(x => x.PromoterCode == asset.PromoterCode);
                    if (assetDto != null)
                    {
                        assetDto.EnqAssetValue = (Convert.ToDecimal(assetDto.EnqAssetValue) + Convert.ToDecimal(assetDto.EnqAssetValue));
                        //netWorth. = (Convert.ToDecimal(netWorth.Networth) + Convert.ToDecimal(asset.Value)).ToString();
                    }
                    else
                    {
                        PromoterDetailsDTO promoter = new PromoterDetailsDTO();
                        promoter = promoterList.FirstOrDefault(d => d.PromoterCode == asset.PromoterCode);
                        promoterAssetList.Add(new PromoterAssetsNetWorthDTO
                        {

                            EnqPromassetId = promoterAssetList.Max(x => x.EnqPromassetId) + 1 ?? 1,
                            PromoterMasterDTO = promoter.PromoterMaster,
                            EnqAssetValue = asset.EnqAssetValue,
                        });
                    }

                    totalAssets += Convert.ToDecimal(asset.EnqAssetValue);

                    PromoterNetWorthDetailsDTO networtitem = new PromoterNetWorthDetailsDTO();
                    networtitem.PromoterDetailsDTO = new PromoterDetailsDTO();
                    networtitem.PromoterDetailsDTO = promoterList.FirstOrDefault(d => d.PromoterCode == asset.PromoterCode);
                    networtitem.PromoterDetailsDTO.PromoterMaster = promoterList.FirstOrDefault(d => d.PromoterCode == asset.PromoterCode).PromoterMaster;

                    if (networthList.Count == 0)
                    {
                        networtitem.EnqMov = asset.EnqAssetValue;
                        networtitem.PromoterCode = asset.PromoterCode.Value;
                        networthList.Add(networtitem);
                    }
                    else
                    {
                        var item = networthList.FirstOrDefault(x => x.PromoterCode == asset.PromoterCode);
                        if (item != null)
                        {
                            item.EnqMov = item.EnqMov + asset.EnqAssetValue;
                        }
                        else
                        {
                            networtitem.EnqMov = asset.EnqAssetValue;
                            networtitem.PromoterCode = asset.PromoterCode.Value;
                            networthList.Add(networtitem);
                        }
                    }


                }

                _sessionManager.SetPromoterAssetList(proAssetDetailsList);
            }
            //Calculating Liability

            if (proLiaDetailsList != null)
            {
                foreach (var proLiability in proLiaDetailsList)
                {
                    //Update PromoterName in Liability if promoter name has been changed in Promoter List by user.
                    proLiability.PromoterMasterDTO = promoterList.FirstOrDefault(d => d.PromoterCode == proLiability.PromoterCode).PromoterMaster;

                    //Find if One Promoter has multiple Liability
                    PromoterLiabilityDetailsDTO promoterLiability = PromoterLiabilityList.FirstOrDefault(x => x.PromoterCode == proLiability.PromoterCode);

                    if (promoterLiability != null)
                    {
                        promoterLiability.EnqLiabValue = (Convert.ToDecimal(promoterLiability.EnqLiabValue) - Convert.ToDecimal(proLiability.EnqLiabValue));
                        //promoterLiability.Networth = (Convert.ToDecimal(promoterLiability.Networth) - Convert.ToDecimal(proLiability.Value)).ToString();
                    }
                    else
                    {
                        PromoterDetailsDTO promoter = promoterList.FirstOrDefault(d => d.PromoterCode == proLiability.PromoterCode);

                        PromoterLiabilityList.Add(new PromoterLiabilityDetailsDTO
                        {
                            EnqPromliabId = PromoterLiabilityList.Max(x => x.EnqPromliabId) + 1 ?? 1,
                            PromoterMasterDTO = promoter.PromoterMaster,
                            EnqLiabValue = proLiability.EnqLiabValue,
                        });
                    }

                    totalLiabilities += Convert.ToDecimal(proLiability.EnqLiabValue);
                    PromoterNetWorthDetailsDTO networtitem = new PromoterNetWorthDetailsDTO();
                    if (proAssetDetailsList != null)
                    {
                        var assestDetail = proAssetDetailsList.FirstOrDefault(x => x.PromoterCode == proLiability.PromoterCode);
                        if (assestDetail != null)
                        {
                            var item = networthList.FirstOrDefault(x => x.PromoterCode == proLiability.PromoterCode);
                            if (item != null)
                            {
                                if (item.EnqLiab != null)
                                    item.EnqLiab = item.EnqLiab + proLiability.EnqLiabValue;
                                else
                                    item.EnqLiab = proLiability.EnqLiabValue;
                                item.EnqNw = item.EnqMov - item.EnqLiab;
                            }
                            else
                            {
                                networtitem.EnqMov = assestDetail.EnqAssetValue;
                                networtitem.EnqNw = networtitem.EnqMov - proLiability.EnqLiabValue;
                                networtitem.PromoterCode = proLiability.PromoterCode;
                                networtitem.EnqLiab = proLiability.EnqLiabValue;
                                networthList.Add(networtitem);
                            }
                        }


                    }


                }
                _sessionManager.SetPromoterLiabilityList(proLiaDetailsList);
            }
            _sessionManager.SetPromoterNetWorthList(networthList);
            PromoterAllDetailsDTO promoterAllDetailsDTO = new()
            {
                ListPromoters = promoterList,
                PromotersAssetLiabilityDetails = new PromAssetLiabilityDetailsDTO()
                {
                    TotalAssets = totalAssets.ToString(),
                    ListLiabilityDetails = proLiaDetailsList,
                    TotalNetworth = (totalAssets - totalLiabilities).ToString(),
                    TotalLiabilities = totalLiabilities.ToString(),
                    ListAssetDetails = proAssetDetailsList,
                },
                PromoterNetWorthList = networthList
            };

            return promoterAllDetailsDTO;
        }

        [HttpGet]
        public ActionResult CheckPromotorGurantorPanDuplicacy(string pan)
        {
            bool Isdupicate = false;
            var promotorList = _sessionManager.GetPromoterDetailsList();
            var guaDetailsList = _sessionManager.GetGuarantorDetailsList();

            if (promotorList != null && promotorList.Count > 0)
            {
                var item = promotorList.FirstOrDefault(x => x.PromoterMaster.PromoterPan.ToUpper() == pan.ToUpper());
                if (item != null)
                    Isdupicate = true;
            }
            if (guaDetailsList != null && guaDetailsList.Count > 0)
            {
                var item = guaDetailsList.FirstOrDefault(x => x.PromoterMaster.PromoterPan.ToUpper() == pan.ToUpper());
                if (item != null)
                    Isdupicate = true;
            }
            return Json(new { isValid = true, Isdupicate= Isdupicate,respMessage= "This PAN number already is being used in this enquiry" });
        }
    }
}

