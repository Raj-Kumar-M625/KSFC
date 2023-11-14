using KAR.KSFC.Components.Common.Dto.AdminModule;
using KAR.KSFC.UI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Employee
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class AssignBranchController : Controller
    {
        private readonly IEmployeeService _empService;
        private readonly ILogger _logger;
        public AssignBranchController(IEmployeeService empService, ILogger logger)
        {
            _empService = empService;
            _logger = logger;
        }

        [HttpGet, Route("Admin/AssignBranch/Index/{empNo}")]
        public async Task<IActionResult> Index(string empNo)
        {
            try
            {
                _logger.Information("Started - AssignBranch/Index with employee no." + empNo);

                _logger.Information("Invoking employee service with employee no." + empNo);

                ViewBag.EmpNo = empNo;
                var empDetails = await _empService.GetEmployeeDetail(empNo);
                var getEmployeeOfficeDetails = await _empService.GetAllAssignDataUsingEmployeeId(empNo);
                _logger.Information("Completed - employee service with employee no." + empNo); 

                var mapObjectForOffice = new List<AssignOfficeDto>();


                if (empDetails.Count > 0 && !string.IsNullOrEmpty(empDetails[0].TeyTicketNum))
                {
                    foreach (var emp in empDetails)
                    {
                        mapObjectForOffice.Add(new AssignOfficeDto()
                        {
                            TeyTicketNum = emp.TeyTicketNum,
                            Email = emp.TeyPresentEmail,
                            EmpoyeeName = emp.TeyName,
                            MobileNumber = emp.EmpMobileNo.ToString(),
                            IsCheckedIn = emp.IsCheckedIn,
                            AssignOfficeDataDto = getEmployeeOfficeDetails
                        });
                    }
                    ViewBag.showEmpDetails = true;
                    _logger.Information("Completed - AssignBranch/Index with employee no." + empNo);
                    return View(mapObjectForOffice);
                }


                ViewBag.showEmpDetails = false;
                _logger.Information("Completed - AssignBranch/Index with employee no." + empNo);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! employee no." + empNo + " in AssignBranch page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpGet, Route("Admin/AssignBranch/GetEmpDetailById/{empNo}")]
        public async Task<IActionResult> GetEmpDetailById(string empNo)
        {

            try
            {
                _logger.Information("Started - AssignBranch/Index with employee no." + empNo);

                _logger.Information("Invoking employee service with employee no." + empNo);

                var empDetails = await _empService.GetEmployeeDetail(empNo);

                var designationList = await _empService.GetEmployeeDesignation();

                var assignBranchMasterDto = new List<AssignOfficeDto>();

                var officeListresult = await _empService.GetOfficeDetails();

                _logger.Information("Completed - employee service with employee no." + empNo);

                List<SelectListItem> officeList = new List<SelectListItem>();

                List<SelectListItem> opsDesig = new List<SelectListItem>();

                opsDesig.Add(new SelectListItem { Text = "-----Select-----", Value = null });

                if (designationList.Count > 0)
                {
                    foreach (var d in designationList)
                    {
                        opsDesig.Add(new SelectListItem { Text = d.TgesDesc, Value = d.TgesCode });
                    }


                }

                if (empDetails.Count > 0 && !string.IsNullOrEmpty(empDetails[0].TeyTicketNum))
                {
                    foreach (var emp in empDetails)
                    {
                        var obj = new AssignOfficeDto();
                        obj.TeyTicketNum = emp.TeyTicketNum;
                        obj.EmpoyeeName = emp.TeyName;
                        obj.MobileNumber = emp.EmpMobileNo.ToString();
                        obj.CommencementDate = null;
                        obj.OpsDesignationId = "0";
                        obj.ChairId = "0";
                        obj.Email = emp.TeyPresentEmail;
                        assignBranchMasterDto.Add(obj);

                    }
                    officeList.Add(new SelectListItem { Text = "-----Select-----", Value = null });

                    if (officeListresult.Count > 0)
                    {
                        foreach (var item in officeListresult)
                        {
                            officeList.Add(new SelectListItem { Text = item.OffcNam, Value = item.OffcCd.ToString() });
                        }
                    }

                    List<SelectListItem> chairs = new List<SelectListItem>();

                    ViewBag.offices = officeList;
                    ViewBag.Designations = opsDesig;
                    ViewBag.Chairs = chairs;

                    _logger.Information("Completed - AssignBranch/Index with employee no." + empNo);
                    return View("Details", assignBranchMasterDto[0]);
                }

                _logger.Information("Completed - AssignBranch/Index with employee no." + empNo);
                return View("Details", new AssignOfficeDto());
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! employee no." + empNo + " in AssignBranch page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        [HttpGet, Route("Admin/AssignBranch/GetChairs/{offcId}")]
        public async Task<IActionResult> GetChairs(string offcId)
        {
            try
            {
                _logger.Information("Started - AssignBranch/GetChairs with office id" + offcId);

                _logger.Information("Invoking employee service with office id" + offcId);

                List<SelectListItem> chairs = new List<SelectListItem>();
                var chairListResult = await _empService.GetChairDetails(offcId);

                _logger.Information("Completed employee service with office id" + offcId);

                chairs.Add(new SelectListItem { Text = "-----Select-----", Value = "0" });
                if (string.IsNullOrEmpty(offcId))
                {
                    return Json(new SelectList(chairs, "Value", "Text"));
                }
                if (chairListResult.Count > 0)
                {
                    foreach (var item in chairListResult)
                    {
                        chairs.Add(new SelectListItem { Text = item.Description, Value = item.Code.ToString() });

                    }
                }
                _logger.Information("Completed - AssignBranch/GetChairs with office id" + offcId);

                return Json(new SelectList(chairs, "Value", "Text"));
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Office id." + offcId + " in AssignBranch/GetChairs page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost, Route("Admin/AssignOffice/CheckOut")]
        public async Task<IActionResult> CheckOut(CheckOutDto obj)
        {

            try
            {
                _logger.Information(string.Format("Started - AssignBranch/GetChairs with CheckOutDto Chair code :{0} employee id :{1} office id : {2} designation :{3}"
                                    , obj.chairCode, obj.employeeId,obj.officeId, obj.opsDesignationId));                

                await _empService.CheckOut(obj);

                _logger.Information(string.Format("Completed - AssignBranch/GetChairs with CheckOutDto Chair code :{0} employee id :{1} office id : {2} designation :{3}"
                                  , obj.chairCode, obj.employeeId, obj.officeId, obj.opsDesignationId));
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! assign Office check out . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost, Route("Admin/AssignBranch/SubmitAssignment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitResult(AssignOfficeDto requestObj)
        {
            try
            {
                _logger.Information(string.Format("Started - Admin/AssignBranch/SubmitAssignment with AssignOfficeDto Chair id :{0} employee name :{1} office id : {2} mobile :{3}"
                                          , requestObj.ChairId, requestObj.EmpoyeeName, requestObj.OfficeId, requestObj.MobileNumber));
                
                var officeListresult = await _empService.GetOfficeDetails();
                var designationList = await _empService.GetEmployeeDesignation();             


                List<SelectListItem> officeList = new List<SelectListItem>();

                officeList.Add(new SelectListItem { Text = "-----Select-----", Value = null });

                if (officeListresult.Count > 0)
                {
                    foreach (var item in officeListresult)
                    {
                        officeList.Add(new SelectListItem
                        {
                            Text = string.IsNullOrEmpty(item.OffcNam.ToString()) ? "" : item.OffcNam.ToString(),
                            Value = string.IsNullOrEmpty(item.OffcCd.ToString()) ? "" : item.OffcCd.ToString(),
                            Selected = item.OffcCd.ToString() == requestObj?.OfficeId
                        });

                    }

                }

                List<SelectListItem> chairs = new List<SelectListItem>();
                chairs.Add(new SelectListItem { Text = "-----Select-----", Value = "0" });
                if (string.IsNullOrEmpty(requestObj.OfficeId))
                {
                    _logger.Information("Invoking employee service for chair details with office id." + requestObj.OfficeId);

                    var chairListResult = await _empService.GetChairDetails(requestObj.OfficeId);

                    _logger.Information("Completed employee service for chair details with office id." + requestObj.OfficeId);

                    if (requestObj.OfficeId == "0")
                    {
                        return Json(new SelectList(chairs, "Value", "Text"));
                    }
                    if (chairListResult.Count > 0)
                    {
                        foreach (var item in chairListResult)
                        {
                            chairs.Add(new SelectListItem
                            {
                                Text = item.Description,
                                Value = item.Code.ToString(),
                                Selected = item.Code.ToString() == requestObj?.ChairId
                            });

                        }
                    }

                }

                List<SelectListItem> opsDesig = new List<SelectListItem>();

                opsDesig.Add(new SelectListItem { Text = "-----Select-----", Value = null });

                if (designationList.Count > 0)
                {
                    foreach (var d in designationList)
                    {
                        opsDesig.Add(new SelectListItem { Text = d.TgesDesc, Value = d.TgesCode });
                    }


                }

                ViewBag.offices = officeList;
                ViewBag.Designations = opsDesig;
                ViewBag.Chairs = chairs;

                if (ModelState.IsValid)
                {
                    _logger.Information(string.Format("invoking employee service for submit assignment with assignofficedto, chair id :{0} employee name :{1} office id : {2} mobile :{3}"
                                        , requestObj.ChairId, requestObj.EmpoyeeName, requestObj.OfficeId, requestObj.MobileNumber));

                    var responseObj = await _empService.SubmitAssignment(requestObj);

                    _logger.Information(string.Format("Completed employee service for submit assignment with AssignOfficeDto, Chair id :{0} employee name :{1} office id : {2} mobile :{3} response {4}"
                                      , requestObj.ChairId, requestObj.EmpoyeeName, requestObj.OfficeId, requestObj.MobileNumber));

                    if (responseObj == true)
                    {
                        _logger.Information(string.Format("Completed - Admin/AssignBranch/SubmitAssignment with AssignOfficeDto Chair id :{0} employee name :{1} office id : {2} mobile :{3}"
                                     , requestObj.ChairId, requestObj.EmpoyeeName, requestObj.OfficeId, requestObj.MobileNumber));

                        return View("Details", new AssignOfficeDto());
                    }
                }
                _logger.Information(string.Format("Completed - Admin/AssignBranch/SubmitAssignment with AssignOfficeDto Chair id :{0} employee name :{1} office id : {2} mobile :{3}"
                                      , requestObj.ChairId, requestObj.EmpoyeeName, requestObj.OfficeId, requestObj.MobileNumber));

                HttpContext.Response.StatusCode = 400;
                return View("Details", requestObj);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! while submit assignment . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

    }
}
