using System;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.UnitDetails
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class AddressController : Controller
    {
        private readonly SessionManager _sessionManager;
        private const string resultViewPath = "~/Areas/Customer/Views/Enquiry/UnitDetails/Address/";
        private const string viewPath = "../../Areas/Customer/Views/Enquiry/UnitDetails/Address/";
        private readonly ILogger _logger;
        public AddressController(SessionManager sessionManager, ILogger logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }

        /// <summary>
        /// View Address Details for a selectd id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecord method for Id = " + id);
                ViewBag.AddressTypes = GetAddressTypeChekbox();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord method for Id = " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new AddressDetailsDTO());
                }
                else
                {
                    var addressList = _sessionManager.GetAddressList();
                    AddressDetailsDTO add = addressList.Where(x => x.EnqAddresssId == id).FirstOrDefault();
                    if (add != null)
                    {
                        ViewData[add.AddtypeCd.ToString()] = true;
                    }
                    _logger.Information("Completed - ViewRecord method for Id = " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", add);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecord page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        /// <summary>
        /// Get Method for Create or Edit 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEdit(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEdit method for Id = " + id);
                var addressType = GetAddressTypeChekbox();
                var addressList = _sessionManager.GetAddressList();
                ViewBag.AddressTypes = GetAddressTypeChekbox();
                if (addressList != null)
                {
                    var newaddressType = new List<SelectListItem>();
                    foreach (var item in addressList)
                    {
                        var itemToAdd = addressType.FirstOrDefault(x => x.Value != item.AddtypeCd.ToString());
                        newaddressType.Add(itemToAdd);
                    }
                    ViewBag.AddressTypes = newaddressType;
                }
                _logger.Information("Completed - CreateOrEdit method for Id = " + id);
                return View(resultViewPath + "CreateOrEdit.cshtml", new AddressDetailsDTO());

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
        /// <summary>
        /// Post Method for Creating a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addr"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, AddressDetailsDTO addr, IFormCollection form)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEdit method for Id:{0} UnitFax:{1} UnitMobileNo :{2} UnitTelNo :{3} UnitPincode :{4} UniitAddress :{5} AddtypeCd :{6} EnqtempId :{7} EnqAddresssId :{8} UnitEmail ",
                    id, addr.UnitFax, addr.UnitMobileNo, addr.UnitTelNo, addr.UnitPincode, addr.UniitAddress, addr.AddtypeCd, addr.EnqtempId, addr.EnqAddresssId, addr.UnitEmail));
                var listAddressTypes = GetAddressTypeChekbox();
                ViewBag.AddressTypes = listAddressTypes;
                if (ModelState.IsValid)
                {
                    List<AddressDetailsDTO> addressList = new();
                    if (!form.Any(c => c.Key.StartsWith("chkBox")))
                    {
                        ViewBag.CheckBoxError = "Please select atleast one of the Address Types.";
                        _logger.Information(string.Format("Completed - CreateOrEdit method for Id:{0} UnitFax:{1} UnitMobileNo :{2} UnitTelNo :{3} UnitPincode :{4} UniitAddress :{5} AddtypeCd :{6} EnqtempId :{7} EnqAddresssId :{8} UnitEmail ",
                    id, addr.UnitFax, addr.UnitMobileNo, addr.UnitTelNo, addr.UnitPincode, addr.UniitAddress, addr.AddtypeCd, addr.EnqtempId, addr.EnqAddresssId, addr.UnitEmail));
                        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", addr) });
                    }
                    else
                    {
                        if (_sessionManager.GetAddressList() != null)
                            addressList = _sessionManager.GetAddressList();
                        var checkedAddressType = form.Where(x => x.Key.StartsWith("chkBox"));

                        var AddressTypeIds = checkedAddressType.Select(x => Convert.ToInt32(x.Value));
                        var checkdata = addressList.Where(x => AddressTypeIds.Contains(x.AddtypeCd.Value));

                        if (checkdata.Count() > 0)
                        {
                            ViewBag.CheckBoxError = "Selected Address Types already exist.";
                            _logger.Information(string.Format("Completed - CreateOrEdit method for Id:{0} UnitFax:{1} UnitMobileNo :{2} UnitTelNo :{3} UnitPincode :{4} UniitAddress :{5} AddtypeCd :{6} EnqtempId :{7} EnqAddresssId :{8} UnitEmail ",
                    id, addr.UnitFax, addr.UnitMobileNo, addr.UnitTelNo, addr.UnitPincode, addr.UniitAddress, addr.AddtypeCd, addr.EnqtempId, addr.EnqAddresssId, addr.UnitEmail));
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", addr) });
                        }
                        foreach (var item in checkedAddressType)
                        {
                            addressList.Add(new AddressDetailsDTO
                            {
                                AddressTypeMasterDTO = new()
                                {
                                    AddtypeCd = Convert.ToInt32(item.Value),
                                    AddtypeDets = listAddressTypes.Find(a => a.Value == item.Value).Text
                                },
                                AddtypeCd = Convert.ToInt32(item.Value),
                                UniitAddress = addr.UniitAddress,
                                UnitEmail = addr.UnitEmail,
                                UnitFax = addr.UnitFax,
                                EnqAddresssId = addressList.Max(x => x.EnqAddresssId) + 1 ?? 1,
                                UnitMobileNo = addr.UnitMobileNo,
                                UnitPincode = addr.UnitPincode,
                                UnitTelNo = addr.UnitTelNo
                            });
                        }
                    }

                    if (id != 0)
                    {
                        addr.EnqAddresssId = id;
                        addr.AddressTypeMasterDTO.AddtypeDets = addressList.Find(m => m.EnqAddresssId == id).AddressTypeMasterDTO.AddtypeDets;
                        addressList.Remove(addressList.Find(m => m.EnqAddresssId == id));
                        addressList.Add(addr);
                    }
                    _sessionManager.SetAddressList(addressList);
                    ViewBag.AllAddressTypeExist = false;
                    if (addressList.Count == listAddressTypes.Count)
                    {
                        ViewBag.AllAddressTypeExist = true;
                    }
                    _logger.Information(string.Format("Completed - CreateOrEdit method for Id:{0} UnitFax:{1} UnitMobileNo :{2} UnitTelNo :{3} UnitPincode :{4} UniitAddress :{5} AddtypeCd :{6} EnqtempId :{7} EnqAddresssId :{8} UnitEmail ",
                    id, addr.UnitFax, addr.UnitMobileNo, addr.UnitTelNo, addr.UnitPincode, addr.UniitAddress, addr.AddtypeCd, addr.EnqtempId, addr.EnqAddresssId, addr.UnitEmail));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", addressList) });
                }
                _logger.Information(string.Format("Completed - CreateOrEdit method for Id:{0} UnitFax:{1} UnitMobileNo :{2} UnitTelNo :{3} UnitPincode :{4} UniitAddress :{5} AddtypeCd :{6} EnqtempId :{7} EnqAddresssId :{8} UnitEmail ",
                    id, addr.UnitFax, addr.UnitMobileNo, addr.UnitTelNo, addr.UnitPincode, addr.UniitAddress, addr.AddtypeCd, addr.EnqtempId, addr.EnqAddresssId, addr.UnitEmail));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", addr) });


            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }


        /// <summary>
        /// Get Method for Editing a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Update(int id = 0)
        {
            try
            {
                _logger.Information("Started - Update method for Id = " + id);
                ViewBag.AddressTypes = GetAddressTypeChekbox();
                var addressList = _sessionManager.GetAddressList();
                AddressDetailsDTO add = addressList.Where(x => x.EnqAddresssId == id).FirstOrDefault();
                if (add != null)
                {
                    ViewData[add.AddtypeCd.ToString()] = true;
                }
                _logger.Information("Completed - Update method for Id = " + id);
                return View(resultViewPath + "Update.cshtml", add);
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading Update page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        /// <summary>
        /// Post method for Editing a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addr"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, AddressDetailsDTO addr, IFormCollection form)
        {
            try
            {
                _logger.Information(string.Format("Started - Update HtppPost method for Id:{0} UnitFax:{1} UnitMobileNo :{2} UnitTelNo :{3} UnitPincode :{4} UniitAddress :{5} AddtypeCd :{6} EnqtempId :{7} EnqAddresssId :{8} UnitEmail ",
                    id, addr.UnitFax, addr.UnitMobileNo, addr.UnitTelNo, addr.UnitPincode, addr.UniitAddress, addr.AddtypeCd, addr.EnqtempId, addr.EnqAddresssId, addr.UnitEmail));
                var addressType = GetAddressTypeChekbox();
                ViewBag.AddressTypes = GetAddressTypeChekbox();
                if (ModelState.IsValid)
                {
                    List<AddressDetailsDTO> addressList = new();
                    if (_sessionManager.GetAddressList() != null)
                        addressList = _sessionManager.GetAddressList();

                    AddressDetailsDTO addrExist = addressList.Find(x => x.EnqAddresssId == id);
                    if (addrExist != null)
                        addressList.Remove(addrExist);

                    addressList.Add(new AddressDetailsDTO
                    {
                        AddressTypeMasterDTO = new()
                        {
                            AddtypeCd = Convert.ToInt32(addressType.FirstOrDefault(x => x.Value == addr.AddtypeCd.ToString()).Value),
                            AddtypeDets = addressType.FirstOrDefault(x => x.Value == addr.AddtypeCd.ToString()).Text
                        },
                        AddtypeCd = addr.AddtypeCd,
                        UniitAddress = addr.UniitAddress,
                        UnitEmail = addr.UnitEmail,
                        UnitFax = addr.UnitFax,
                        EnqAddresssId = addressList.Max(x => x.EnqAddresssId) + 1 ?? 1,
                        UnitMobileNo = addr.UnitMobileNo,
                        UnitPincode = addr.UnitPincode,
                        UnitTelNo = addr.UnitTelNo
                    }); 

                    _sessionManager.SetAddressList(addressList);
                    if (addressList.Count == addressType.Count)
                    {
                        ViewBag.AllAddressTypeExist = true;
                    }
                    _logger.Information(string.Format("Completed - Update HtppPost method for Id:{0} UnitFax:{1} UnitMobileNo :{2} UnitTelNo :{3} UnitPincode :{4} UniitAddress :{5} AddtypeCd :{6} EnqtempId :{7} EnqAddresssId :{8} UnitEmail ",
                        id, addr.UnitFax, addr.UnitMobileNo, addr.UnitTelNo, addr.UnitPincode, addr.UniitAddress, addr.AddtypeCd, addr.EnqtempId, addr.EnqAddresssId, addr.UnitEmail));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", addressList) });
                }
                _logger.Information(string.Format("Completed - Update HtppPost method for Id:{0} UnitFax:{1} UnitMobileNo :{2} UnitTelNo :{3} UnitPincode :{4} UniitAddress :{5} AddtypeCd :{6} EnqtempId :{7} EnqAddresssId :{8} UnitEmail ",
                        id, addr.UnitFax, addr.UnitMobileNo, addr.UnitTelNo, addr.UnitPincode, addr.UniitAddress, addr.AddtypeCd, addr.EnqtempId, addr.EnqAddresssId, addr.UnitEmail));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "Update", addr) });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading Update HtppPost page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        /// <summary>
        /// Post method for deleting a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(AddressDetailsDTO dto)
        {
            try
            {
                _logger.Information(string.Format("Started - Delete HtppPost method for UnitFax:{0} UnitMobileNo :{1} UnitTelNo :{2} UnitPincode :{3} UniitAddress :{4} AddtypeCd :{5} EnqtempId :{6} EnqAddresssId :{7} UnitEmail ",
                    dto.UnitFax, dto.UnitMobileNo, dto.UnitTelNo, dto.UnitPincode, dto.UniitAddress, dto.AddtypeCd, dto.EnqtempId, dto.EnqAddresssId, dto.UnitEmail));
                if (dto.EnqAddresssId == null || dto.EnqAddresssId == 0)
                {
                    return NotFound();
                }
                else
                {
                    var addressList = JsonConvert.DeserializeObject<List<AddressDetailsDTO>>(HttpContext.Session.GetString("AddressList"));
                    var itemToRemove = addressList.Find(r => r.EnqAddresssId == dto.EnqAddresssId);
                    addressList.Remove(itemToRemove);
                    _sessionManager.SetAddressList(addressList);
                    _logger.Information(string.Format("Completed - Update HtppPost method for UnitFax:{0} UnitMobileNo :{1} UnitTelNo :{2} UnitPincode :{3} UniitAddress :{4} AddtypeCd :{5} EnqtempId :{6} EnqAddresssId :{7} UnitEmail ",
                        dto.UnitFax, dto.UnitMobileNo, dto.UnitTelNo, dto.UnitPincode, dto.UniitAddress, dto.AddtypeCd, dto.EnqtempId, dto.EnqAddresssId, dto.UnitEmail));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", addressList) });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading Delete page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        private List<SelectListItem> GetAddressTypeChekbox()
        {
            List<SelectListItem> listAddressTypes = _sessionManager.GetAddressTypesFromDB();
            return listAddressTypes;
        }

    }
}
