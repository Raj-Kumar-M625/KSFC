using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Employee
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _empService;
        private readonly ILogger _logger;

        public EmployeeController(IEmployeeService empService, ILogger logger)
        {
            _empService = empService;
            _logger = logger;

        }

        [HttpGet]
        [Route("Admin/Employee")]
        public async Task<IActionResult> Index(string id, string operation)
        {
            _logger.Information("Started - Admin employee home ..");

            try
            {
                ViewBag.showList = false;
                _logger.Information("Invoking employee service for employee details");

                var response = await _empService.GetAllEmployeeDetail();

                _logger.Information("Completed employee service for employee details with response count :" + response.Count);
                if (response.Count > 0)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        ViewBag.showList = true;
                        response = response.Where(x => x.TeyTicketNum == id).ToList();
                    }
                    _logger.Information(" Completed - Admin employee home returning the result..");
                    return View(response);
                }

                _logger.Information(" Completed - Admin home returning the result..");

                return View(new List<EmployeeDTO>());
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Admin/Employee page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }


        [HttpPost]
        [Route("Admin/Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string teyTicketNumber)
        {

            try
            {
                _logger.Information("Started - Admin employee home HttpPost with ticket number " + teyTicketNumber);
                ViewBag.showList = true;

                ViewData["TeyTicketNumber"] = teyTicketNumber;

                if (!string.IsNullOrEmpty(teyTicketNumber))
                {
                    _logger.Information("Invoking employee service for employee details with ticket number " + teyTicketNumber);

                    var empDetails = (List<EmployeeDTO>)await _empService.GetEmployeeDetail(teyTicketNumber);

                    _logger.Information("Completed employee service for employee details with response count " + empDetails.Count);
                    if (empDetails.Count > 0 && !string.IsNullOrEmpty(empDetails[0].TeyTicketNum))
                    {
                        _logger.Information("Completed - Admin employee home HttpPost with ticket number " + teyTicketNumber);
                        return View(empDetails);
                    }
                    _logger.Information("Completed - Admin employee home HttpPost with ticket number " + teyTicketNumber);
                    return View(new List<EmployeeDTO>());

                }
                _logger.Information("Completed - Admin employee home HttpPost with ticket number " + teyTicketNumber);
                return View(new List<EmployeeDTO>());
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Admin/Employee page HttpPost . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }


        [HttpGet, Route("Admin/Employee/Submit")]
        public async Task<IActionResult> CreateOrEdit(string teyTicketNumber = "")
        {
            try
            {
                _logger.Information("Started - Admin employee CreateOrEdit HttpGet with ticket number " + teyTicketNumber);

                _logger.Information("Invoking employee service for employee details with ticket number " + teyTicketNumber);
                var empDesignation = await _empService.GetEmployeeDesignation(); // Get all employee designation 

                if (!(string.IsNullOrEmpty(teyTicketNumber)))
                {
                    // if employee number is not null fetch employee details 
                    var employeeDtls = await _empService.GetEmployeeDetail(teyTicketNumber);

                    _logger.Information("Completed employee service for employee details with ticket number " + teyTicketNumber + "response count " + employeeDtls.Count);

                    if (employeeDtls.Count > 0 && empDesignation.Count > 0)
                    {
                        var employee = employeeDtls[0];
                        return View(employee);
                    }

                }
                var viewResult = new EmployeeDTO();

                List<SelectListItem> PpDesignCode = empDesignation.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = string.IsNullOrEmpty(a.TgesDesc.ToString()) ? "" : a.TgesDesc.ToString(),
                        Value = string.IsNullOrEmpty(a.TgesCode.ToString()) ? "" : a.TgesCode.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> IcDesigCode = empDesignation.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = string.IsNullOrEmpty(a.TgesDesc.ToString()) ? "" : a.TgesDesc.ToString(),
                        Value = string.IsNullOrEmpty(a.TgesCode.ToString()) ? "" : a.TgesCode.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> SubstDesigCode = empDesignation.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = string.IsNullOrEmpty(a.TgesDesc.ToString()) ? "" : a.TgesDesc.ToString(),
                        Value = string.IsNullOrEmpty(a.TgesCode.ToString()) ? "" : a.TgesCode.ToString(),
                        Selected = false
                    };
                });

                ViewBag.SubstantiveRank = SubstDesigCode;
                ViewBag.InChargeDesignation = IcDesigCode;
                ViewBag.PersonalPromotion = PpDesignCode;

                _logger.Information("Completed - Admin employee CreateOrEdit HttpGet with ticket number " + teyTicketNumber);
                return View(viewResult);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Admin/Employee/Submit page HttpPost . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpGet, Route("Admin/Employee/GetEmployeeById/{Id}")]
        public async Task<IActionResult> GetEmployeeById(string Id)
        {
            try
            {
                _logger.Information("Started - Admin employee GetEmployeeById HttpGet with EmployeeById " + Id);

                _logger.Information("Invoking employee service for employee details with with Employee Id " + Id);
                var empDesignation = await _empService.GetEmployeeDesignation();
                var viewResult = new EmployeeDTO();

                if (!string.IsNullOrEmpty(Id))
                {
                    var result = await _empService.GetEmployeeDetail(Id);
                    _logger.Information("Completed employee service for employee details with with Employee Id " + Id + "result count " + result.Count);
                    if (result.Count > 0)
                    {
                        viewResult = result[0];
                    }
                }


                List<SelectListItem> PpDesignCode = empDesignation.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = string.IsNullOrEmpty(a.TgesDesc.ToString()) ? "" : a.TgesDesc.ToString(),
                        Value = string.IsNullOrEmpty(a.TgesCode.ToString()) ? "" : a.TgesCode.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> IcDesigCode = empDesignation.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = string.IsNullOrEmpty(a.TgesDesc.ToString()) ? "" : a.TgesDesc.ToString(),
                        Value = string.IsNullOrEmpty(a.TgesCode.ToString()) ? "" : a.TgesCode.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> SubstDesigCode = empDesignation.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = string.IsNullOrEmpty(a.TgesDesc.ToString()) ? "" : a.TgesDesc.ToString(),
                        Value = string.IsNullOrEmpty(a.TgesCode.ToString()) ? "" : a.TgesCode.ToString(),
                        Selected = false
                    };
                });

                ViewBag.SubstantiveRank = SubstDesigCode;
                ViewBag.InChargeDesignation = IcDesigCode;
                ViewBag.PersonalPromotion = PpDesignCode;

                _logger.Information("Completed - Admin employee GetEmployeeById HttpGet with EmployeeById " + Id);
                return View("CreateOrEdit", viewResult);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Admin/Employee/Submit page HttpPost . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        [HttpGet, Route("Admin/Employee/IsEmployeeNumberUnique")]
        public async Task<JsonResult> IsEmployeeNumberUnique(string empNo)
        {
            var result = await _empService.IsEmployeeNumberUnique(empNo);
            return Json(result);
        }

        [HttpPost, Route("Admin/Employee/SubmitDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(string TeyTicketNum, EmployeeDTO employeeDTO)
        {
            try
            {
                _logger.Information(string.Format("Started - Admin/Employee/SubmitDetails with EmployeeDTO Tey Name :{0} TeyTicketNum :{1} TeyPanNum : {2} TeyPresentEmail :{3} mobile :{4}"
                                                  , employeeDTO.TeyName, employeeDTO.TeyTicketNum, employeeDTO.TeyPanNum, employeeDTO.TeyPresentEmail, employeeDTO.EmpMobileNo));

                _logger.Information("Invoking employee service for employee details with ticket number " + employeeDTO.TeyTicketNum);

                var empDesignation = await _empService.GetEmployeeDesignation(); // Get all employee designation 

                _logger.Information("Completed employee service for employee details with ticket number " + employeeDTO.TeyTicketNum + " result count : " + empDesignation.Count);

                if (ModelState.IsValid)
                {
                    _logger.Information("Invoking employee service for CreateOrUpdate employee details with ticket number " + employeeDTO.TeyTicketNum);

                    var createResult = await _empService.CreateOrUpdate(employeeDTO);

                    _logger.Information("Completed employee service for CreateOrUpdate employee details with ticket number " + employeeDTO.TeyTicketNum + " result :" + createResult);

                    _logger.Information(string.Format("Completed - Admin/Employee/SubmitDetails with EmployeeDTO Tey Name :{0} TeyTicketNum :{1} TeyPanNum : {2} TeyPresentEmail :{3} mobile :{4}"
                                           , employeeDTO.TeyName, employeeDTO.TeyTicketNum, employeeDTO.TeyPanNum, employeeDTO.TeyPresentEmail, employeeDTO.EmpMobileNo));

                    return Ok(new { flag = createResult.Item1, message = createResult.Item2 }); ;
                }

                _logger.Warning(" Skipped the employee service CreateOrUpdate method! Model validation result :" + ModelState.IsValid.ToString());

                List<SelectListItem> PpDesignCode = empDesignation.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = string.IsNullOrEmpty(a.TgesDesc.ToString()) ? "" : a.TgesDesc.ToString(),
                        Value = string.IsNullOrEmpty(a.TgesCode.ToString()) ? "" : a.TgesCode.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> IcDesigCode = empDesignation.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = string.IsNullOrEmpty(a.TgesDesc.ToString()) ? "" : a.TgesDesc.ToString(),
                        Value = string.IsNullOrEmpty(a.TgesCode.ToString()) ? "" : a.TgesCode.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> SubstDesigCode = empDesignation.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = string.IsNullOrEmpty(a.TgesDesc.ToString()) ? "" : a.TgesDesc.ToString(),
                        Value = string.IsNullOrEmpty(a.TgesCode.ToString()) ? "" : a.TgesCode.ToString(),
                        Selected = false
                    };
                });

                ViewBag.SubstantiveRank = SubstDesigCode;
                ViewBag.InChargeDesignation = IcDesigCode;
                ViewBag.PersonalPromotion = PpDesignCode;

                _logger.Warning("Loading the CreateOrEdit view with employeeDTO data again");
                HttpContext.Response.StatusCode = 400;
                return View(employeeDTO);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Admin/Employee/Submit page HttpPost . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        [HttpGet, Route("Admin/Employee/DeleteEmployeeById/{Id}")]
        public async Task<IActionResult> DeleteEmployeeById(string Id)
        {
            try
            {
                _logger.Information("Started - Admin/Employee/DeleteEmployeeById HttpGet with EmployeeById " + Id);

                var result = await _empService.Delete(Id);

                _logger.Information("Completed - Admin/Employee/DeleteEmployeeById HttpGet with EmployeeById " + Id + " result " + result.ToString());
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! DeleteEmployeeById . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
