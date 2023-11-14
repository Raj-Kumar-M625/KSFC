using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule;
using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;  
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.UoW;
using Microsoft.AspNetCore.Mvc;

namespace KAR.KSFC.API.Areas.Admin.Controllers.Employee
{
    public class AssignBranchController : BaseApiController
    {

        private readonly IAssignOffice _assignofficeService;
        private readonly IUnitOfWork _unitOfWorkService;
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;

        public AssignBranchController(IAssignOffice assignofficeService, IUnitOfWork unitOfWorkService, IMapper smsService, ITokenService tokenService, ILogger logger)
        {
            _assignofficeService = assignofficeService;
            _unitOfWorkService = unitOfWorkService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpGet, Route("GetAllEmployeeBranch")]
        public async Task<IActionResult> GetAllEmployeeBranch()
        {
            try
            {
                _logger.Information("Started - GetAllEmployeeBranch");

                List<EmployeeDesignationHistoryDTO> Emp_details = new List<EmployeeDesignationHistoryDTO>();

                CancellationToken token = new();
                Emp_details = await _assignofficeService.GetAllEmployeeBranch(token);

                _logger.Information("Completed - GetAllEmployeeBranch");

                return Ok(new ApiResultResponse(Emp_details, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured while GetAllEmployeeBranch. Error message is: {ex.Message} {Environment.NewLine} The stack trace is: {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet, Route("GetEmployeeBranchByFilter")]
        public async Task<IActionResult> GetAllEmployeeBranchByFilter(string ticket_num = null)
        {

            CancellationToken token = new();
           List<EmployeeDesignationHistoryDTO> Emp_details = await _assignofficeService.GetAllEmployeeBranchByFilter(token, ticket_num);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }

        [HttpPost, Route("SaveEmployeeCheckIn")]
        public async Task<IActionResult> SaveEmployeeCheckIn(TblEmpdesigTab CheckIn_dets)
        {

            CancellationToken token = new();
           int Emp_details = await _assignofficeService.SaveEmployeeCheckIn(CheckIn_dets, token);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }

        [HttpPost, Route("SaveEmployeeCheckOut")]
        public async Task<IActionResult> SaveEmployeeCheckOut(TblEmpdesigTab Checkout_dets)
        {

            CancellationToken token = new();
            int Emp_details = await _assignofficeService.SaveEmployeeCheckOut(Checkout_dets, token);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }

        [HttpPatch, Route("DeleteEmployeeBranchById")]
        public async Task<IActionResult> DeleteEmployeeBranchById(string ticket_num)
        {

            CancellationToken token = new();
            EmployeeDesignationHistoryDTO Emp_details = await _assignofficeService.DeleteEmployeeBranchById(ticket_num, token);
            return Ok(new ApiResultResponse(Emp_details, "Success"));

        }
    }
}
