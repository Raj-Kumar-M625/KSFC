using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails.ChangeLocation
{
    [Area(Constants.Admin)] 
    [Authorize(Roles = RolesEnum.Employee)]

    public class ChangeLocationController : Controller
    {

        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        public ChangeLocationController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllAddressDetails = _sessionManager.GetAllAddressDetailsList();
                IdmUnitAddressDTO AddressDetails = AllAddressDetails.FirstOrDefault(x => x.IdmUtAddressRowid == id);

                var allDistrictList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionDistrict));
                var allTalukList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionTaluk));
                var allHobliList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionHobli));
                var allVillageList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionVillage));
                var allPincodeList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPincode));

                ViewBag.AllDistrictList = allDistrictList;
                ViewBag.AllTalukList = allTalukList;
                ViewBag.AllHobliList = allHobliList;
                ViewBag.AllVillageList = allVillageList;
                ViewBag.AllPincodeList = allPincodeList;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.chnglocationresultViewPath + Constants.ViewRecord, AddressDetails);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult Edit(int id = 0)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);

                var AllRegisteredAddressDetails = _sessionManager.GetAllAddressDetailsList();
                var AllMasterPincodeDetails = _sessionManager.GetAllMasterPincodeDetails();
                var AllPincodeDistrictDetails = _sessionManager.GetAllPincodeDistrictDetails();
                var AllTalukDetails = _sessionManager.GetAllTalukDetails();
                var AllHobliDetails = _sessionManager.GetAllHobliDetails();
                var AllVillageDetails = _sessionManager.GetAllVillageDetails();

                IdmUnitAddressDTO RegisteredAddressDetails = AllRegisteredAddressDetails.FirstOrDefault(x => x.IdmUtAddressRowid == id);
                if (RegisteredAddressDetails != null) 
                {
                    var allDistrictList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionDistrict));
                    ViewBag.AllDistrictList = allDistrictList;
                    ViewBag.LoanAcc = RegisteredAddressDetails.LoanAcc;
                    ViewBag.LoanSub = RegisteredAddressDetails.LoanSub;
                        
                    PincodeDistrictCdtabDTO PincodeDistrictDetails = AllPincodeDistrictDetails.Find(x => x.Distcd == RegisteredAddressDetails.UtDistCd);
                    ViewBag.DistrictPincodes = AllMasterPincodeDetails.Where(x => x.PincodeDistrictCd == PincodeDistrictDetails.PincodeDistrictCd).Select(x => new { x.PincodeRowId, x.PincodeCd }).ToList();
                    ViewBag.AllTalukList = AllTalukDetails.Where(x => x.DistCd == RegisteredAddressDetails.UtDistCd).Select(x => new { x.TlqCd, x.TlqNam }).ToList();
                    ViewBag.AllHobliList = AllHobliDetails.Where(x => x.TlqCd == RegisteredAddressDetails.UtTlqCd).Select(x => new { x.HobCd, x.HobNam }).ToList();
                    ViewBag.AllVillageList = AllVillageDetails.Where(x => x.HobCd == RegisteredAddressDetails.UtHobCd).Select(x => new { x.VilCd, x.VilNam }).ToList();

                }
                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.chnglocationresultViewPath + Constants.editCs, RegisteredAddressDetails);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IdmUnitAddressDTO address)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.IdmUnitAddressDTO,
                    address.IdmUtAddressRowid, address.UtAddress, address.UtDistCd, address.UtTlqCd, address.UtHobCd, address.UtVilCd, address.UtPincode, address.UtCity));

                List<IdmUnitAddressDTO> addressDetails = new();
                if (_sessionManager.GetAllAddressDetailsList() != null)
                    addressDetails = _sessionManager.GetAllAddressDetailsList();

                IdmUnitAddressDTO addressExist = addressDetails.Find(x => x.IdmUtAddressRowid == address.IdmUtAddressRowid);
                long? accountNumber = 0;

                if (addressExist != null)
                {
                    accountNumber = addressExist.LoanAcc;
                    addressDetails.Remove(addressExist);
                    var list = addressExist;
                    list.LoanAcc = address.LoanAcc;

                    var alldistrictnames = _sessionManager.GetDDLDistrictList();
                    var districtname = alldistrictnames.Where(x => x.Value == address.UtDistCd.ToString());
                    var alltaluknames = _sessionManager.GetDDLTalukList();
                    var allhoblinames = _sessionManager.GetDDLHobliList();
                    var allvillagenames = _sessionManager.GetDDLVillageList();

                    list.DistrictName = districtname.First().Text;
                    list.UtPincode = address.UtPincode;
                    list.UtAddress = address.UtAddress;
                    list.UtArea = address.UtArea;
                    list.UtCity = address.UtCity;
                    list.UtDistCd = address.UtDistCd; 
                    if(address.UtTlqCd != null)
                    {
                        list.UtTlqCd = address.UtTlqCd;
                        var talukname = alltaluknames.Where(x => x.Value == address.UtTlqCd.ToString());
                        list.TalukName = talukname.First().Text;
                    }
                    if (address.UtHobCd != null)
                    {
                        list.UtHobCd = address.UtHobCd;
                        var hobliname = allhoblinames.Where(x => x.Value == address.UtHobCd.ToString());
                        list.HobliName = hobliname.First().Text;
                    }
                    if (address.UtVilCd != null)
                    {
                        list.UtVilCd = address.UtVilCd;
                        var villagename = allvillagenames.Where(x => x.Value == address.UtVilCd.ToString());
                        list.VillageName = villagename.First().Text;
                    }
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Update;
                    addressDetails.Add(list);
                    _sessionManager.SetAllAddressDetailsList(addressDetails);
                    ViewBag.AccountNumber = address.LoanAcc;
                    ViewBag.LoanSub = address.LoanSub;
                    ViewBag.OffcCd = address.OffcCd;
                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.IdmUnitAddressDTO,
                    address.IdmUtAddressRowid, address.UtAddress, address.UtDistCd, address.UtTlqCd, address.UtHobCd, address.UtVilCd, address.UtPincode, address.UtCity));
                    return Json(new { isValid = true, data = accountNumber, html = Helper.RenderRazorViewToString(this, Constants.chnglocationviewPath + Constants.ViewAll, addressDetails) });
                }
                ViewBag.AccountNumber = address.LoanAcc;
                ViewBag.LoanSub = address.LoanSub;
                ViewBag.OffcCd = address.OffcCd;
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.IdmUnitAddressDTO,
                    address.IdmUtAddressRowid, address.UtAddress, address.UtDistCd, address.UtTlqCd, address.UtHobCd, address.UtVilCd, address.UtPincode, address.UtCity));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.chnglocationviewPath + Constants.Edit, address) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
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
                PincodeDistrictCdtabDTO PincodeDistrictDetail = AllPincodeDistrictDetails.Find(x => x.Distcd == Id);
                var output = AllMasterPincodeDetails.Where(x => x.PincodeDistrictCd == PincodeDistrictDetail.PincodeDistrictCd).Select(x=> new {x.PincodeRowId,x.PincodeCd } ).ToList();
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
        public IActionResult TalukDropdown(int Id)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedGetAllTalukDetails + Id);
                var AllTalukDetails = _sessionManager.GetAllTalukDetails();
                var output = AllTalukDetails.Where(x => x.DistCd == Id).Select(x => new { x.TlqCd, x.TlqNam }).ToList();
                _logger.Information(CommonLogHelpers.CompletedGetAllTalukDetails + Id);
                return Json(new SelectList(output, Constants.TlqCd, Constants.TlqNam));
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.GetAllTalukDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult HobliDropdown(int Id)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedGetAllHobliDetails + Id);
                var AllHobliDetails = _sessionManager.GetAllHobliDetails();
                var output = AllHobliDetails.Where(x => x.TlqCd == Id).Select(x => new { x.HobCd, x.HobNam }).ToList();
                _logger.Information(CommonLogHelpers.CompletedGetAllHobliDetails + Id);
                return Json(new SelectList(output, Constants.HobCd, Constants.HobNam));
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.GetAllHobliDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult VillageDropdown(int Id)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedGetAllVillageDetails + Id);
                var AllVillageDetails = _sessionManager.GetAllVillageDetails();
                var output = AllVillageDetails.Where(x => x.HobCd == Id).Select(x => new { x.VilCd, x.VilNam }).ToList();
                _logger.Information(CommonLogHelpers.CompletedGetAllVillageDetails + Id);
                return Json(new SelectList(output, Constants.VilCd, Constants.VilNam));
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.GetAllVillageDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
