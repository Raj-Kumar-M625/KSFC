using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails.PromoterAddress 
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class PromoterAddressController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        
        public PromoterAddressController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }

        public IActionResult ViewRecord(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + unqid);
                var AllPromoAddressList = _sessionManager.GetAllPromoAddressDetails();
                IdmPromAddressDTO PromoAddressList = AllPromoAddressList.FirstOrDefault(x => x.UniqueId == unqid);

                var allPromoterState = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromState));
                var allPromoterDistricts = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromDistrict));
                var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));
                var allPromoterPincode = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPincode));


                ViewBag.AllPromoterNames = allPromoterNames;
                ViewBag.AllPromoterState = allPromoterState;
                ViewBag.AllPromoterDistrict = allPromoterDistricts;
                ViewBag.AllPromoterPincode = allPromoterPincode;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + unqid);
                return View(Constants.promaddrresultViewPath + Constants.ViewRecord, PromoAddressList);

            }
            catch (System.Exception ex)
            {
               _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult Edit(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + unqid);

                var AllPromoAddressList = _sessionManager.GetAllPromoAddressDetails();
                var AllPincodeDistrictDetails = _sessionManager.GetAllPincodeDistrictDetails();
                var AllMasterPincodeDetails = _sessionManager.GetAllMasterPincodeDetails();
                IdmPromAddressDTO PromoAddressList = AllPromoAddressList.FirstOrDefault(x => x.UniqueId == unqid);
                if (PromoAddressList != null)
                {
                    ViewBag.AccountNumber = PromoAddressList.LoanAcc;
                    ViewBag.OffcCd = PromoAddressList.OffcCd;
                    ViewBag.InUnit = PromoAddressList.UtCd;
                    ViewBag.LoanSub = PromoAddressList.LoanSub;
                    var allPromoterState = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromState));
                    var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));

                    ViewBag.AllPromoterNames = allPromoterNames;
                    ViewBag.AllPromoterState = allPromoterState;
                    ViewBag.AllPromoterPincode = AllMasterPincodeDetails;
                    ViewBag.AllPromoterDistrict = AllPincodeDistrictDetails;


                    //PincodeDistrictCdtabDTO PincodeDistrictDetails = AllPincodeDistrictDetails.Find(x => x.PincodeDistrictCd == PromoAddressList.PromDistrictCd);
                    //ViewBag.DistrictPincodes = AllMasterPincodeDetails.Where(x => x.PincodeDistrictCd == PincodeDistrictDetails.PincodeDistrictCd).Select(x => new { x.PincodeRowId, x.PincodeCd }).ToList();
                    //ViewBag.AllPromoterDistrict = AllPincodeDistrictDetails.Where(x => x.PincodeStateCd== PincodeDistrictDetails.PincodeStateCd).Select(x => new { x.PincodeDistrictCd, x.PincodeDistrictDesc }).ToList();
                    
                }
                _logger.Information(CommonLogHelpers.UpdateCompleted + unqid);

                return View(Constants.promaddrresultViewPath + Constants.editCs, PromoAddressList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IdmPromAddressDTO address)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.IdmPromAddressDto,
                  address.IdmPromadrId, address.PromoterCode, address.PromAddress, address.PromStateCd, address.PromDistrictCd, address.PromPincode, address.AdrPermanent));

                List<IdmPromAddressDTO> addressDetails = new();
                List<IdmPromAddressDTO> activeaddressDetails = new();
                if (_sessionManager.GetAllPromoAddressDetails() != null)
                    addressDetails = _sessionManager.GetAllPromoAddressDetails();

                IdmPromAddressDTO addressExist = addressDetails.Find(x => x.UniqueId == address.UniqueId);
                long? accountNumber = 0;
                if (addressExist != null)
                {
                    accountNumber = addressExist.LoanAcc;
                    addressDetails.Remove(addressExist);
                    var list = addressExist;
                    list.LoanAcc = address.LoanAcc;
                    list.UniqueId = address.UniqueId;
                    list.OffcCd = address.OffcCd;
                    list.UtCd = address.UtCd;
                    list.LoanSub = address.LoanSub;
                    list.PromAddress = address.PromAddress;
                    list.PromStateCd = address.PromStateCd;
                    list.PromDistrictCd = address.PromDistrictCd;
                    list.PromPincode = address.PromPincode;
                    list.AdrPermanent = address.AdrPermanent;
                    list.PromoterCode = addressExist.PromoterCode;
                    list.PromoterName = addressExist.PromoterName;
                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (address.IdmPromadrId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;
                    }

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InUnit = list.UtCd;
                    ViewBag.LoanSub = list.LoanSub;
                    addressDetails.Add(list);
                    _sessionManager.SetPromoterAddressList(addressDetails);

                    if (addressDetails.Count != 0)
                    {
                        activeaddressDetails = (addressDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }
                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.IdmPromAddressDto,
                  address.IdmPromadrId, address.PromoterCode, address.PromAddress, address.PromStateCd, address.PromDistrictCd, address.PromPincode, address.AdrPermanent));

                    return Json(new { isValid = true, data = accountNumber, html = Helper.RenderRazorViewToString(this, Constants.promaddrviewPath + Constants.ViewAll, activeaddressDetails) });
                }
                ViewBag.AccountNumber = address.LoanAcc;
                ViewBag.OffcCd = address.OffcCd;
                ViewBag.InUnit = address.UtCd;
                ViewBag.LoanSub = address.LoanSub;
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.IdmPromAddressDto,
                  address.IdmPromadrId, address.PromoterCode, address.PromAddress, address.PromStateCd, address.PromDistrictCd, address.PromPincode, address.AdrPermanent));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.promaddrviewPath + Constants.Edit, address) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        public IActionResult Create(long AccountNumber, byte OffCd, int InUnit, int LoanSub,int Id )
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                var AllPromoAddressList = _sessionManager.GetAllPromoAddressDetails();
                var AllPincodeDistrictDetails = _sessionManager.GetAllPincodeDistrictDetails();
                var AllMasterPincodeDetails = _sessionManager.GetAllMasterPincodeDetails();
                
                
                ViewBag.LoanAcc = AccountNumber;
                ViewBag.OffcCd = OffCd;
                ViewBag.InUnit = InUnit;
                ViewBag.LoanSub = LoanSub;
                var allPromoterState = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromState));
                var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));

                
                ViewBag.AllPromoterNames = allPromoterNames;
                ViewBag.AllPromoterState = allPromoterState;
                ViewBag.AllPromoterPincode = AllMasterPincodeDetails;
                ViewBag.AllPromoterDistrict = AllPincodeDistrictDetails;

                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.promaddrresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmPromAddressDTO address)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.CreateStartedPost + LogAttribute.IdmPromAddressDto,
                  address.IdmPromadrId, address.PromoterCode, address.PromAddress, address.PromStateCd, address.PromDistrictCd, address.PromPincode, address.AdrPermanent));

                if (ModelState.IsValid)
                {

                    List<IdmPromAddressDTO> addressDetails = new();
                    List<IdmPromAddressDTO> activeaddressDetails = new();
                    if (_sessionManager.GetAllPromoAddressDetails() != null)
                        addressDetails = _sessionManager.GetAllPromoAddressDetails();

                    IdmPromAddressDTO list = new IdmPromAddressDTO();
                    list.LoanAcc = address.LoanAcc;
                    list.OffcCd = address.OffcCd;
                    list.UtCd = address.UtCd;
                    list.LoanSub = address.LoanSub;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.PromoterCode = address.PromoterCode;
                    var allpromoternames = _sessionManager.GetAllPromoterNames();
                    var promotername = allpromoternames.Where(x => x.Value == list.PromoterCode.ToString());
                    list.PromoterName = promotername.First().Text;
                    list.PromAddress = address.PromAddress;
                    list.PromStateCd = address.PromStateCd;
                    list.PromDistrictCd = address.PromDistrictCd;
                    list.PromPincode = address.PromPincode;
                    //list.AdrPermanent = address.AdrPermanent;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    addressDetails.Add(list);
                    _sessionManager.SetPromoterAddressList(addressDetails);

                    if (addressDetails.Count != 0)
                    {
                        activeaddressDetails = (addressDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InUnit = list.UtCd;
                    ViewBag.LoanSub = list.LoanSub;
                    _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + LogAttribute.IdmPromAddressDto,
                  address.IdmPromadrId, address.PromoterCode, address.PromAddress, address.PromStateCd, address.PromDistrictCd, address.PromPincode, address.AdrPermanent));
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.promaddrviewPath + Constants.ViewAll, activeaddressDetails) });
                }
                ViewBag.AccountNumber = address.LoanAcc;
                ViewBag.OffcCd = address.OffcCd;
                ViewBag.InUnit = address.UtCd;
                ViewBag.LoanSub = address.LoanSub;
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.IdmPromAddressDto,
                  address.IdmPromadrId, address.PromoterCode, address.PromAddress, address.PromStateCd, address.PromDistrictCd, address.PromPincode, address.AdrPermanent));

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.promaddrviewPath + Constants.Create, address) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult Delete(string Id)
        {
            try
            {
                IEnumerable<IdmPromAddressDTO> activeAddressList = new List<IdmPromAddressDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, Id));
                var addressList = JsonConvert.DeserializeObject<List<IdmPromAddressDTO>>(HttpContext.Session.GetString(Constants.sessionPromAddress));
                var itemToRemove = addressList.Find(r => r.UniqueId == Id);
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;
                itemToRemove.Action = (int)Constant.Delete;
                addressList.Add(itemToRemove);
                _sessionManager.SetPromoterAddressList(addressList);
                if (addressList.Count != 0)
                {
                    activeAddressList = addressList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InUnit = itemToRemove.UtCd;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted,Id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.promaddrviewPath + Constants.ViewAll, activeAddressList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult PincodeDropDown(int Id) 
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedGetAllPincodeDetails + Id);

                var AllPincodeDistrictDetails = _sessionManager.GetAllPincodeDistrictDetails();
                var AllMasterPincodeDetails = _sessionManager.GetAllMasterPincodeDetails();
                PincodeDistrictCdtabDTO PincodeDistrictDetails = AllPincodeDistrictDetails.Find(x => x.PincodeDistrictCd== Id); 
                var output = AllMasterPincodeDetails.Where(x => x.PincodeDistrictCd == PincodeDistrictDetails.PincodeDistrictCd).Select(x => new { x.PincodeRowId, x.PincodeCd }).ToList();
                _logger.Information(CommonLogHelpers.CompletedGetAllPincodeDetails + Id);
                return Json(new SelectList(output, Constants.PincodeRowId, Constants.PincodeCd));
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.GetAllPincodeDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult DistrictDropDown (int Id)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedGetAllDistrictDetails + Id);
                var AllPincodeDistrictDetails = _sessionManager.GetAllPincodeDistrictDetails();
                var output = AllPincodeDistrictDetails.Where(x => x.PincodeStateCd == Id).Select(x => new { x.PincodeDistrictCd, x.PincodeDistrictDesc }).ToList();
                _logger.Information(CommonLogHelpers.CompletedGetAllDistrictDetails + Id);
                return Json(new SelectList(output, Constants.PincodeDistrictCd, Constants.PincodeDistrictDesc));
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.GetAllDistrictDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}
