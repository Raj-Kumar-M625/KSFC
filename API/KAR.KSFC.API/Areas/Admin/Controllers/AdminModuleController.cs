using System;
using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule;
using KAR.KSFC.Components.Common.Dto.AdminModule;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;

using Microsoft.AspNetCore.Mvc;

namespace KAR.KSFC.API.Areas.Admin.Controllers
{
    public class AdminModuleController : BaseApiController
    {
        private readonly Func<ServiceEnum, IGenericAdminModuleRepository> _serviceResolver;
        private readonly ILogger _logger;

        public AdminModuleController(Func<ServiceEnum, IGenericAdminModuleRepository> serviceResolver, ILogger logger)
        {
            _serviceResolver = serviceResolver;
            _logger = logger;
        }

        #region Module Master
        [HttpPost, Route("AddModule")]
        public async Task<IActionResult> AddModuleMasterAsync([FromBody] ModuleMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - AddModuleMasterAsync with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.ModuleService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - AddModuleMasterAsync with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateModule")]
        public async Task<IActionResult> UpdateModuleMasterAsync([FromBody] ModuleMasterDto model, CancellationToken cancellationToken)
        {
            try
            {

                _logger.Information($"Started - UpdateModuleMasterAsync with id : {model.Id} Description : {model.Description} ");

                var lst = await _serviceResolver(ServiceEnum.ModuleService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - UpdateModuleMasterAsync with id : {model.Id} Description : {model.Description} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdModule")]
        public async Task<IActionResult> GetByIdModuleMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetByIdModuleMasterAsync with id : {Id} ");

                var lst = await _serviceResolver(ServiceEnum.ModuleService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetByIdModuleMasterAsync with id : {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllModule")]
        public async Task<IActionResult> GetAllModuleMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetAllModuleMasterAsync ");

                var lst = await _serviceResolver(ServiceEnum.ModuleService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetAllModuleMasterAsync with response count {lst.Count}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteModule")]
        public async Task<IActionResult> DeleteModuleMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - DeleteModuleMasterAsync with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.ModuleService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - DeleteModuleMasterAsync with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Role Master
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpPost, Route("AddRole")]
        public async Task<IActionResult> AddRoleMasterAsync([FromBody] RoleMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - AddRoleMasterAsync with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.RoleService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - AddRoleMasterAsync with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateRole")]
        public async Task<IActionResult> UpdateRoleMasterAsync([FromBody] RoleMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - UpdateRoleMasterAsync with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.RoleService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - UpdateRoleMasterAsync with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdRole")]
        public async Task<IActionResult> GetByIdRoleMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetByIdRoleMasterAsync with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.RoleService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetByIdRoleMasterAsync with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllRole")]
        public async Task<IActionResult> GetAllRoleMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetAllRoleMasterAsync");

                var lst = await _serviceResolver(ServiceEnum.RoleService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetAllRoleMasterAsync");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpDelete, Route("DeleteRole")]
        public async Task<IActionResult> DeleteRoleMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - DeleteRoleMasterAsync with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.RoleService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - DeleteRoleMasterAsync with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Attribute Master
        [HttpPost, Route("AddAttribute")]
        public async Task<IActionResult> AddAttributeMasterAsync([FromBody] AttributeMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - AddAttributeMasterAsync with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.AttributeService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - AddAttributeMasterAsync with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateAttribute")]
        public async Task<IActionResult> UpdateAttributeMasterAsync([FromBody] AttributeMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - UpdateAttributeMasterAsync with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.AttributeService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - UpdateAttributeMasterAsync with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdAttribute")]
        public async Task<IActionResult> GetByIdAttributeMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetByIdAttributeMasterAsync with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.AttributeService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetByIdAttributeMasterAsync with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllAttribute")]
        public async Task<IActionResult> GetAllAttributeMasterAsync(CancellationToken cancellationToken)
        {
            try
            {

                _logger.Information($"Started - GetAllAttributeMasterAsync");

                var lst = await _serviceResolver(ServiceEnum.AttributeService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetAllAttributeMasterAsync");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteAttribute")]
        public async Task<IActionResult> DeleteAttributeMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.AttributeService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Attribute Unit Master
        [HttpPost, Route("AddAttributeUnit")]
        public async Task<IActionResult> AddAttributeUnitMasterAsync([FromBody] AttributeUnitMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Adding with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Adding with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateAttributeUnit")]
        public async Task<IActionResult> UpdateAttributeUnitMasterAsync([FromBody] AttributeUnitMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Update with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Update with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdAttributeUnit")]
        public async Task<IActionResult> GetByIdAttributeUnitMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Getting with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllAttributeUnit")]
        public async Task<IActionResult> GetAllAttributeUnitMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetAll");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetAll");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteAttributeUnit")]
        public async Task<IActionResult> DeleteAttributeUnitMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Attribute Unit Operator Master
        [HttpPost, Route("AddAttributeUnitOperator")]
        public async Task<IActionResult> AddAttributeUnitOperatorMasterAsync([FromBody] AttributeUnitOperatorMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Adding with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitOperatorService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Adding with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateAttributeUnitOperator")]
        public async Task<IActionResult> UpdateAttributeUnitOperatorMasterAsync([FromBody] AttributeUnitOperatorMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Update with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitOperatorService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Update with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdAttributeUnitOperator")]
        public async Task<IActionResult> GetByIdAttributeUnitOperatorMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitOperatorService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Getting with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllAttributeUnitOperator")]
        public async Task<IActionResult> GetAllAttributeUnitOperatorMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetAll");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitOperatorService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetAll");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteAttributeUnitOperator")]
        public async Task<IActionResult> DeleteAttributeUnitOperatorMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {

                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.AttributeUnitOperatorService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Sub Attribute Master
        [HttpPost, Route("AddSubAttribute")]
        public async Task<IActionResult> AddSubAttributeMasterAsync([FromBody] SubAttributeMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Adding with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.SubAttributeService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Adding with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateSubAttribute")]
        public async Task<IActionResult> UpdateSubAttributeMasterAsync([FromBody] SubAttributeMasterDto model, CancellationToken cancellationToken)
        {
            try
            {

                _logger.Information($"Started - Update with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.SubAttributeService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Update with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdSubAttribute")]
        public async Task<IActionResult> GetByIdSubAttributeMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.SubAttributeService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Getting with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllSubAttribute")]
        public async Task<IActionResult> GetAllSubAttributeMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetAll");

                var lst = await _serviceResolver(ServiceEnum.SubAttributeService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetAll");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteSubAttribute")]
        public async Task<IActionResult> DeleteSubAttributeMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.SubAttributeService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Chair Master for Task Workflow
        [HttpPost, Route("AddChair")]
        public async Task<IActionResult> AddChairMasterAsync([FromBody] ChairMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Adding with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.ChairMasterService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Adding with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateChair")]
        public async Task<IActionResult> UpdateChairMasterAsync([FromBody] ChairMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Update with id : {model.Id} Description : {model.Description}");

                var lst = await _serviceResolver(ServiceEnum.ChairMasterService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Update with id : {model.Id} Description : {model.Description}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdChair")]
        public async Task<IActionResult> GetByIdChairMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.ChairMasterService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Getting with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllChair")]
        public async Task<IActionResult> GetAllChairMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetAll");

                var lst = await _serviceResolver(ServiceEnum.ChairMasterService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetAll");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteChair")]
        public async Task<IActionResult> DeleteChairMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.ChairMasterService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Delegation Of Power Master for Task Workflow
        [HttpPost, Route("AddDelegationOfPower")]
        public async Task<IActionResult> AddDOPMasterAsync([FromBody] DOPMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Adding with id : {model.Id} Code : {model.Code}");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Adding with id : {model.Id} Code : {model.Code}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateDelegationOfPower")]
        public async Task<IActionResult> UpdateDOPMasterAsync([FromBody] DOPMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Update with id : {model.Id} Code : {model.Code}");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Update with id : {model.Id} Code : {model.Code}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdDelegationOfPower")]
        public async Task<IActionResult> GetByIdDOPMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Getting with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllDelegationOfPower")]
        public async Task<IActionResult> GetAllDOPMasterAsync(CancellationToken cancellationToken)
        {
            try
            {

                _logger.Information($"Started - GetAll");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetAll");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteDelegationOfPower")]
        public async Task<IActionResult> DeleteDOPMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Delegation Of Power History Master for Task Workflow
        [HttpPost, Route("AddDelegationOfPowerHistory")]
        public async Task<IActionResult> AddDOPHistoryMasterAsync([FromBody] DOPHistoryMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Completed - Adding with id : {model.Id} Code : {model.Code}");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerHistoryService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Adding with id : {model.Id} Code : {model.Code}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateDelegationOfPowerHistory")]
        public async Task<IActionResult> UpdateDOPHistoryMasterAsync([FromBody] DOPHistoryMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Update with id : {model.Id} Code : {model.Code}");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerHistoryService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Update with id : {model.Id} Code : {model.Code}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdDelegationOfPowerHistory")]
        public async Task<IActionResult> GetByIdDOPHistoryMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerHistoryService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Complteted - Getting with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllDelegationOfPowerHistory")]
        public async Task<IActionResult> GetAllDOPHistoryMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - GetAll");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerHistoryService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - GetAll");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteDelegationOfPowerHistory")]
        public async Task<IActionResult> DeleteDOPHistoryMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.DelegationOfPowerHistoryService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Employee Chair Detail
        [HttpPost, Route("AddEmployeeChairDetail")]
        public async Task<IActionResult> AddEmployeeChairDetailMasterAsync([FromBody] EmployeeChairDetailMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Adding with id : {model.Id} ChairCode : {model.ChairCode} OfficeCode {model.OfficeCode} DesignationCode {model.DesignationCode} EmpId {model.EmpId}");

                var lst = await _serviceResolver(ServiceEnum.EmployeeChairDetailsService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Adding with id : {model.Id} ChairCode : {model.ChairCode} OfficeCode {model.OfficeCode} DesignationCode {model.DesignationCode} EmpId {model.EmpId}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateEmployeeChairDetail")]
        public async Task<IActionResult> UpdateEmployeeChairDetailMasterAsync([FromBody] EmployeeChairDetailMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"started - Updating with id : {model.Id} ChairCode : {model.ChairCode} OfficeCode {model.OfficeCode} DesignationCode {model.DesignationCode} EmpId {model.EmpId}");

                var lst = await _serviceResolver(ServiceEnum.EmployeeChairDetailsService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Updating with id : {model.Id} ChairCode : {model.ChairCode} OfficeCode {model.OfficeCode} DesignationCode {model.DesignationCode} EmpId {model.EmpId}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdEmployeeChairDetail")]
        public async Task<IActionResult> GetByIdEmployeeChairDetailMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.EmployeeChairDetailsService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Complteted - Getting with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllEmployeeChairDetail")]
        public async Task<IActionResult> GetAllEmployeeChairDetailMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting All ");
                var lst = await _serviceResolver(ServiceEnum.EmployeeChairDetailsService).GetAllAsync(cancellationToken).ConfigureAwait(false);
                _logger.Information($"Complteted - Getting All ");
                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteEmployeeChairDetail")]
        public async Task<IActionResult> DeleteEmployeeChairDetailMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {

                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.EmployeeChairDetailsService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Complted - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Employee Chair History Details
        [HttpPost, Route("AddEmployeeChairHistory")]
        public async Task<IActionResult> AddEmployeeChairHistoryMasterAsync([FromBody] EmployeeChairHistoryMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Adding with id : {model.Id} ChairCode : {model.ChairCode} OfficeCode {model.OfficeCode} DesignationCode {model.DesignationCode} EmpId {model.EmpId}");

                var lst = await _serviceResolver(ServiceEnum.EmployeeChairHistoryDetailService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Adding with id : {model.Id} ChairCode : {model.ChairCode} OfficeCode {model.OfficeCode} DesignationCode {model.DesignationCode} EmpId {model.EmpId}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateEmployeeChairHistory")]
        public async Task<IActionResult> UpdateEmployeeChairHistoryMasterAsync([FromBody] EmployeeChairHistoryMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"started - Updating with id : {model.Id} ChairCode : {model.ChairCode} OfficeCode {model.OfficeCode} DesignationCode {model.DesignationCode} EmpId {model.EmpId}");

                var lst = await _serviceResolver(ServiceEnum.EmployeeChairHistoryDetailService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Complteted - Updating with id : {model.Id} ChairCode : {model.ChairCode} OfficeCode {model.OfficeCode} DesignationCode {model.DesignationCode} EmpId {model.EmpId}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdEmployeeChairHistory")]
        public async Task<IActionResult> GetByIdEmployeeChairHistoryMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.EmployeeChairHistoryDetailService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Getting with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllEmployeeChairHistory")]
        public async Task<IActionResult> GetAllEmployeeChairHistoryMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting All ");

                var lst = await _serviceResolver(ServiceEnum.EmployeeChairHistoryDetailService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Getting All ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteEmployeeChairHistory")]
        public async Task<IActionResult> DeleteEmployeeChairHistoryMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.EmployeeChairHistoryDetailService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Hierarchy Chair Master, For Workflow - Forwarding Task
        [HttpPost, Route("AddHierarchy")]
        public async Task<IActionResult> AddHierarchyMasterAsync([FromBody] HierarchyMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Adding with id : {model.Id} FromChairCode : {model.FromChairCode} ToChairCode {model.ToChairCode}");

                var lst = await _serviceResolver(ServiceEnum.HierarchyChairMasterService).AddAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Adding with id : {model.Id} FromChairCode : {model.FromChairCode} ToChairCode {model.ToChairCode}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateHierarchy")]
        public async Task<IActionResult> UpdateHierarchyMasterAsync([FromBody] HierarchyMasterDto model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Updating with id : {model.Id} FromChairCode : {model.FromChairCode} ToChairCode {model.ToChairCode}");

                var lst = await _serviceResolver(ServiceEnum.HierarchyChairMasterService).UpdateAsync(model, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Updating with id : {model.Id} FromChairCode : {model.FromChairCode} ToChairCode {model.ToChairCode}");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetByIdHierarchy")]
        public async Task<IActionResult> GetByIdHierarchyMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.HierarchyChairMasterService).GetByIdAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Started - Getting with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetAllHierarchy")]
        public async Task<IActionResult> GetAllHierarchyMasterAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Getting All ");

                var lst = await _serviceResolver(ServiceEnum.HierarchyChairMasterService).GetAllAsync(cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Getting All ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {

                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteHierarchy")]
        public async Task<IActionResult> DeleteHierarchyMasterAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Started - Delete with id {Id} ");

                var lst = await _serviceResolver(ServiceEnum.HierarchyChairMasterService).DeleteAsync(Id, cancellationToken).ConfigureAwait(false);

                _logger.Information($"Completed - Delete with id {Id} ");

                return Ok(new ApiResultResponse(lst, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured! Error message is : {ex.Message} {Environment.NewLine} The stack trace is : {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
