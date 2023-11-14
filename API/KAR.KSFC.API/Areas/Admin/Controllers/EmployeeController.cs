using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule;
using KAR.KSFC.Components.Common.Dto.AdminModule;
using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.UoW;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace KAR.KSFC.API.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class EmployeeController : BaseApiController
    {

        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService, IUnitOfWork unitOfWorkService, IMapper smsService, ITokenService tokenService)
        {
            _employeeService = employeeService;

        }

        [HttpGet, Route("IsEmployeeNumberUnique")]
        public async Task<IActionResult> IsEmployeeNumberUnique(string empNo, CancellationToken token)
        {
            var res = await _employeeService.IsEmployeeNumberUnique(empNo, token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(res, "Success"));
        }

        [HttpGet, Route("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            List<EmployeeDTO> Emp_details = new List<EmployeeDTO>();

            CancellationToken token = new();
            Emp_details = await _employeeService.GetAllEmployees(token);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }


        [HttpGet, Route("GetAllEmployeesById")]
        public async Task<IActionResult> GetAllEmployeesById(string emp_id)
        {
            CancellationToken token = new();
            var Emp_details = await _employeeService.GetAllEmployeesById(emp_id, token);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }
        [HttpGet, Route("DeleteEmployeesById")]
        public async Task<IActionResult> DeleteEmployeeById(string emp_id)
        {

            CancellationToken token = new();

            var result = await _employeeService.DeleteEmployeeById(emp_id, token);

            if (result == true)
            {
                return Ok(new ApiResultResponse(result, "Entry deleted successfully"));
            }
            else
            {

                return Ok(new ApiResultResponse(result, "Deletion failed"));
            }
        }

        [HttpGet, Route("GetAllEmployeesByFilter")]
        public async Task<IActionResult> GetAllEmployeesByFilter(string ticket_num = null, string desg_code = null, string pan_num = null, int? phone = 0)
        {
            List<EmployeeDTO> Emp_details = new List<EmployeeDTO>();

            CancellationToken token = new();
            Emp_details = await _employeeService.GetAllEmployeesByFilter(token, ticket_num, desg_code, pan_num, phone);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }

        [HttpGet, Route("GetEmployeeDesignation")]
        public async Task<IActionResult> GetEmployeeDesignation()
        {
            CancellationToken token = new();
            var designations = await _employeeService.GetEmployeeDesignation(token);
            return Ok(new ApiResultResponse(designations, "Success"));

        }
        [HttpGet, Route("GetEmployeeDesiHistory")]
        public async Task<IActionResult> GetEmployeeDesiHistory(string empId)
        {
            List<TblEmpdesighistTab> Emp_details = new List<TblEmpdesighistTab>();

            CancellationToken token = new();
            Emp_details = await _employeeService.GetEmployeeDesiHistory(empId, token);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }
        [HttpGet, Route("GetEmployeeDSC")]
        public async Task<IActionResult> GetEmployeeDSC(string empId)
        {
            List<TblEmpdscTab> Emp_details = new List<TblEmpdscTab>();
            CancellationToken token = new();
            Emp_details = await _employeeService.GetEmployeeDSC(empId, token);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }

        [HttpGet, Route("GetEmployeeLoginDetails")]
        public async Task<IActionResult> GetEmployeeLoginDetails(TblEmploginTab checkout_data)
        {


            CancellationToken token = new();
            TblEmploginTab Emp_details = await _employeeService.SaveEmployeeCheckIn(checkout_data, token);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }

        [HttpPost, Route("Submit")]
        public async Task<IActionResult> SubmitEmployeeDetails(EmployeeDTO employee, CancellationToken token)
        {
            if (employee == null)
            {
                return BadRequest(CustomErrorMessage.E06);
            }
            var response = await _employeeService.SaveEmployeeDetails(employee, token).ConfigureAwait(false); ;

            if (response.Item1)
            {
                return Ok(new ApiResultResponse(200, employee.TeyTicketNum, "User details created successfully"));
            }
            else
            {

                return BadRequest(new ApiResultResponse(400, "Employee Creation Failed", response.Item2));
            }

        }

        #region AssignOffice
        [HttpGet, Route("GetAllOffices")]
        public async Task<IActionResult> GetAllOffices(CancellationToken token)
        {
            var result = await _employeeService.GetAllOffices(token);

            return Ok(new ApiResultResponse(200, result, "Success"));

        }

        [HttpGet, Route("GetChairs")]
        public async Task<IActionResult> GetChairs(string offc_id, CancellationToken token)
        {
            var result = await _employeeService.GetAllChairs(offc_id, token);

            return Ok(new ApiResultResponse(200, result, "Success"));

        }


        [HttpPost, Route("Checkin")]
        public async Task<IActionResult> Checkin(AssignOfficeDto requestObj, CancellationToken token)
        {
            if (requestObj == null)
            {
                return BadRequest(CustomErrorMessage.E06);
            }
            var response = await _employeeService.Checkin(requestObj, token).ConfigureAwait(false); ;

            if (response)
            {
                return Ok(new ApiResultResponse(200, requestObj.TeyTicketNum, requestObj.EmpoyeeName + " - was checked in successfully"));
            }
            else
            {
                return BadRequest(CustomErrorMessage.E06);
            }

        }

        [HttpGet, Route("GetAllAssignDataUsingEmployeeId")]
        public async Task<IActionResult> GetAllAssignDataUsingEmployeeId(string employeeId, CancellationToken token)
        {
            if (!string.IsNullOrEmpty(employeeId))
            {
                var response = await _employeeService.GetAllAssignDataUsingEmployeeId(employeeId, token).ConfigureAwait(false);
                return Ok(new ApiResultResponse(200, response, "success"));

            }

            return BadRequest(CustomErrorMessage.E06);

        }

        [HttpPost, Route("Checkout")]
        public async Task<IActionResult> Checkout(CheckOutDto dto, CancellationToken token)
        {
            if (!string.IsNullOrEmpty(dto.employeeId))
            {
                var response = await _employeeService.CheckOut(dto, token).ConfigureAwait(false);
                return Ok(new ApiResultResponse(200, "success"));

            }

            return BadRequest(CustomErrorMessage.E06);

        }

        #endregion

        #region ModuleAndRole

        [HttpGet, Route("GetEmployeeRoleDetails")]
        public async Task<IActionResult> GetEmployeeRoleDetails(string employeeId, CancellationToken token)
        {

            var response = await _employeeService.GetEmployeeRoleDetails(employeeId, token).ConfigureAwait(false);

            if (response.Count > 0)
            {
                return Ok(new ApiResultResponse(200, response, "success"));

            }

            return BadRequest(CustomErrorMessage.E06);

        }

        [HttpGet, Route("GetModules")]
        public async Task<IActionResult> GetModules(CancellationToken token)
        {

            var response = await _employeeService.GetModules(token).ConfigureAwait(false);
            if (response.Count > 0)
            {
                return Ok(new ApiResultResponse(200, response, "success"));

            }
            return BadRequest(CustomErrorMessage.E06);

        }

        [HttpGet, Route("GetRoles")]
        public async Task<IActionResult> GetRoles(string moduleId, CancellationToken token)
        {

            var response = await _employeeService.GetRoles(moduleId, token).ConfigureAwait(false);
            if (response.Count > 0)
            {
                return Ok(new ApiResultResponse(200, response, "success"));

            }
            return BadRequest(CustomErrorMessage.E06);

        }

        [HttpPost, Route("AssignRole")]
        public async Task<IActionResult> AssignRole(AssignRoleDto requestObj, CancellationToken token)
        {

            var response = await _employeeService.AssignRoleAndModule(requestObj, token).ConfigureAwait(false);
            if (response == 1)
            {
                return Ok(new ApiResultResponse(200, response, "duplicate"));
            }
            else
            {
                return Ok(new ApiResultResponse(200, response, "success"));

            }
        }

        [HttpGet, Route("RemoveAssignedRole")]
        public async Task<IActionResult> RemoveAssignedRole(string empId, string moduleId, string roleId, CancellationToken token)
        {

            var response = await _employeeService.RemoveAssignedRole(empId, moduleId, roleId, token).ConfigureAwait(false);
            if (response)
            {
                return Ok(new ApiResultResponse(200, response, "success"));
            }
            return BadRequest(CustomErrorMessage.E06);

        }




        #endregion

    }
}
