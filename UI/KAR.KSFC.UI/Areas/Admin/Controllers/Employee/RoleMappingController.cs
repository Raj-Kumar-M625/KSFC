using KAR.KSFC.Components.Common.Dto.AdminModule;
using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Services.IServices;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Employee
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class RoleMappingController : Controller
    {
        private readonly IEmployeeService _empService;
        private readonly ILogger _logger;
        public RoleMappingController(IEmployeeService employeeService, ILogger logger)
        {
            _empService = employeeService;
            _logger = logger;
        }

        [HttpGet, Route("Admin/RoleMapping/Index/{empNo}")]
        public async Task<IActionResult> Index(string empNo)
        {

            try
            {
                _logger.Information("Started - Admin/RoleMapping/Index HttpGet with employee number " + empNo);

                _logger.Information("Invoking employee service for employee details with employee number " + empNo);
                ViewBag.EmpNo = empNo;
                var empDetails = await _empService.GetEmployeeDetail(empNo);

                ViewBag.EmployeeNumber = empDetails[0].TeyTicketNum.ToString();
                ViewBag.EmployeeName = empDetails[0].TeyName;
                ViewBag.Mobile = empDetails[0].EmpMobileNo;
                ViewBag.Email = empDetails[0].TeyPresentEmail;

                var response = await _empService.GetEmployeeRoleDetails(empNo);

                _logger.Information("Completed employee service for employee details with employee number " + empNo + " employee detail result count :" + empDetails.Count + " Role result count :" + response.Count);

                if (response.Count > 0 && !string.IsNullOrEmpty(response[0].EmployeeNumber))
                {

                    ViewBag.showEmpDetails = true;

                    _logger.Information("Completed - Admin/RoleMapping/Index HttpGet with employee number " + empNo);
                    return View(response);
                }

                ViewBag.showEmpDetails = false;

                _logger.Information("Completed - Admin/RoleMapping/Index HttpGet with employee number " + empNo);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Admin/RoleMapping/Index page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpGet, Route("Admin/RoleMapping/GetEmpDetails/{empNo}")]
        public async Task<IActionResult> GetEmpDetailById(string empNo)
        {
            try
            {
                _logger.Information("Started - Admin/RoleMapping/GetEmpDetails HttpGet with employee number " + empNo);

                _logger.Information("Invoking employee service for employee details with employee number " + empNo);
                var empDetails = await _empService.GetEmployeeDetail(empNo);

                ViewBag.EmployeeName = empDetails[0].TeyName;
                ViewBag.Mobile = empDetails[0].EmpMobileNo;
                ViewBag.Email = empDetails[0].TeyPresentEmail;

                List<SelectListItem> ddlModules = new List<SelectListItem>();
                List<SelectListItem> ddlRoles = new List<SelectListItem>();
                ddlModules.Add(new SelectListItem { Text = "-----Select-----", Value = null });
                ddlRoles.Add(new SelectListItem { Text = "-----Select-----", Value = null });

                //var resultData = await _empService.GetEmployeeRoleDetails(empNo);

                var modules = await _empService.GetModules();
                _logger.Information("Completed employee service for employee details with employee number " + empNo + " employee detail result count :" + empDetails.Count + " Module result count :" + modules.Count);

                if (modules.Count > 0)
                {
                    foreach (var m in modules)
                    {
                        ddlModules.Add(new SelectListItem { Value = m.Id.ToString(), Text = m.Description });
                    }
                }

                ViewBag.modules = ddlModules;

                ViewBag.roles = ddlRoles;

                //if (resultData.Count > 0 && !string.IsNullOrEmpty(resultData[0].EmployeeNumber))
                //{


                //    return View("Details", resultData[0]);
                //}
                _logger.Information("Completed - dmin/RoleMapping/GetEmpDetails HttpGet with employee number " + empNo);

                return View("Details", new AssignRoleDto() { EmployeeNumber = empDetails[0].TeyTicketNum.ToString(), EmployeeName = empDetails[0].TeyName });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Admin/RoleMapping/GetEmpDetails page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        [HttpGet, Route("Admin/RoleMapping/GetRoles/{moduleId}")]
        public async Task<IActionResult> GetRoles(string moduleId)
        {
            try
            {
                _logger.Information("Started - Admin/RoleMapping/GetRoles HttpGet with module id " + moduleId);

                _logger.Information("Invoking employee service for employee roles with module id " + moduleId);

                List<SelectListItem> roles = new List<SelectListItem>();
                var data = await _empService.GetRolesUsingModuleId(moduleId);

                _logger.Information("Invoking employee service for employee roles with module id " + moduleId + "result count :" + data.Count);

                roles.Add(new SelectListItem { Text = "-----Select-----", Value = null });
                if (string.IsNullOrEmpty(moduleId))
                {
                    return Json(new SelectList(roles, "Value", "Text"));
                }
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        roles.Add(new SelectListItem { Text = item.Description, Value = item.Id.ToString() });
                    }
                }
                _logger.Information("Completed - Admin/RoleMapping/GetRoles HttpGet with module id " + moduleId);

                return Json(new SelectList(roles, "Value", "Text"));
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Admin/RoleMapping/GetRoles page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }


        [HttpPost, Route("Admin/RoleMapping/AssignRole")]
        public async Task<IActionResult> AssignRole(AssignRoleDto requestObj)
        {
            try
            {

                _logger.Information(string.Format("Started - Admin/RoleMapping/AssignRole with AssignRoleDto EmployeeNumber :{0} EmployeeName :{1} Email : {2} RoleId :{3} ModuleName :{4}"
                                                 , requestObj.EmployeeNumber, requestObj.EmployeeName, requestObj.Email, requestObj.RoleId, requestObj.ModuleName));


                _logger.Information("Invoking employee service for employee details with EmployeeNumber " + requestObj.EmployeeNumber);

                var empDetails = await _empService.GetEmployeeDetail(requestObj.EmployeeNumber);

                ViewBag.EmployeeName = empDetails[0].TeyName;
                ViewBag.Mobile = empDetails[0].EmpMobileNo;
                ViewBag.Email = empDetails[0].TeyPresentEmail;
                List<SelectListItem> ddlModules = new List<SelectListItem>();
                List<SelectListItem> ddlRoles = new List<SelectListItem>();
                ddlModules.Add(new SelectListItem { Text = "-----Select-----", Value = null });
                ddlRoles.Add(new SelectListItem { Text = "-----Select-----", Value = null });

                var modules = await _empService.GetModules();


                _logger.Information("Completed employee service for employee details with EmployeeNumber " + requestObj.EmployeeNumber);

                if (modules.Count > 0)
                {
                    foreach (var m in modules)
                    {
                        if (requestObj?.ModuleId != null)
                        {
                            ddlModules.Add(new SelectListItem { Value = m.Id.ToString(), Text = m.Description, Selected = m.Id.ToString().Trim() == Convert.ToString(requestObj?.ModuleId).Trim() });
                        }
                        else
                        {
                            ddlModules.Add(new SelectListItem { Value = m.Id.ToString(), Text = m.Description  });
                        }
                    }
                }

                if (requestObj.ModuleId != null)
                {
                    var roles = await _empService.GetRolesUsingModuleId(requestObj.ModuleId);
                    if (roles.Count > 0)
                    {
                        foreach (var item in roles)
                        {
                            ddlRoles.Add(new SelectListItem { Text = item.Description, Value = item.Id.ToString(), Selected = item.Id.ToString().Trim() == requestObj.RoleId.ToString().Trim() });

                        }
                    }
                }

                ViewBag.modules = ddlModules;
                ViewBag.roles = ddlRoles;


                if (ModelState.IsValid)
                {
                    _logger.Information("Invoking employee service for AssignRole with AssignRoleDto ");

                    var result = await _empService.AssignRole(requestObj);

                    _logger.Information("Completed employee service for AssignRole with AssignRoleDto ");

                    _logger.Information(string.Format("Completed - Admin/RoleMapping/AssignRole with AssignRoleDto EmployeeNumber :{0} EmployeeName :{1} Email : {2} RoleId :{3} ModuleName :{4}"
                                                , requestObj.EmployeeNumber, requestObj.EmployeeName, requestObj.Email, requestObj.RoleId, requestObj.ModuleName));

                    return Json(new { responseData = result });

                }
                else
                {
                    _logger.Warning(" Skipped the employee service AssignRole method! Model validation result :" + ModelState.IsValid.ToString());
                    HttpContext.Response.StatusCode = 400;
                    return View("Details", requestObj);
                }

                return View("Details", new AssignRoleDto() { EmployeeNumber = empDetails[0].TeyTicketNum.ToString(), EmployeeName = empDetails[0].TeyName });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Admin/RoleMapping/AssignRole . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }


        [HttpPost, Route("Admin/RoleMapping/Delete")]
        public async Task<IActionResult> DeleteRole(AssignRoleDto requestObj)
        {
            try
            {
                _logger.Information(string.Format("Started - Admin/RoleMapping/Delete with AssignRoleDto EmployeeNumber :{0} EmployeeName :{1} Email : {2} RoleId :{3} ModuleName :{4}"
                                                 , requestObj.EmployeeNumber, requestObj.EmployeeName, requestObj.Email, requestObj.RoleId, requestObj.ModuleName));

                _logger.Information("Invoking employee service for DeleteRole with AssignRoleDto ");
                var result = await _empService.DeleteRole(requestObj);
                if (!result)
                {
                    _logger.Error("Unable to delete assigned role service call result :" + result.ToString());
                    throw new Exception("Unable to delete assigned role");
                }
                _logger.Information("Completed employee service for DeleteRole with AssignRoleDto ");

                _logger.Information(string.Format("Completed - Admin/RoleMapping/Delete with AssignRoleDto EmployeeNumber :{0} EmployeeName :{1} Email : {2} RoleId :{3} ModuleName :{4}"
                                                 , requestObj.EmployeeNumber, requestObj.EmployeeName, requestObj.Email, requestObj.RoleId, requestObj.ModuleName));

                return View("Index", new List<AssignRoleDto>());
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! DeleteRole view . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

    }
}
