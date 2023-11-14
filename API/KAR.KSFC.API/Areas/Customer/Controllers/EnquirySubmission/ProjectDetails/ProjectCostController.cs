using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
//using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.ProjectDetails
{
    [Authorize]
    public class ProjectCostController : BaseApiController
    {
        //private readonly ILogger<ProjectCostController> _logger;
        private readonly IProjectCostDetails _projectCostDetails;
        private readonly ILogger _logger;

        public ProjectCostController(IProjectCostDetails ProjectCostDetails, ILogger logger)
        {
            _projectCostDetails = ProjectCostDetails;
            _logger = logger;
        }

        [HttpPost, Route("AddProjectCostDetails")]
        public async Task<IActionResult> AddProjectCostDetails([FromBody] List<ProjectCostDetailsDTO> projectCost, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddProjectCostDetails method with projectCost");
                var ProjectCost = await _projectCostDetails.AddProjectCostDetails(projectCost, token).ConfigureAwait(false);

                if (ProjectCost == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddProjectCostDetails method with projectCost");
                return Ok(new ApiResultResponse(200, ProjectCost, "Project Cost details created Successfully"));

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddProjectCostDetails  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("GetAllProjectCost")]
        public async Task<IActionResult> GetAllProjectCost( CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetAllProjectCost method with CancellationToken");
                var ProjectCost = await _projectCostDetails.GetAllProjectCosts(token).ConfigureAwait(false);

                if (ProjectCost == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - GetAllProjectCost method with CancellationToken");
                return Ok(new ApiResultResponse(200, ProjectCost, "Project Cost details Listed Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetAllProjectCost  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }


        [HttpPost, Route("UpdateProjectCostDetails")]
        public async Task<IActionResult> UpdateProjectCostDetails(ProjectCostDetailsDTO projectCost, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateProjectCostDetails method with projectCost");
                var ProjectCost = await _projectCostDetails.UpdateProjectCostDetails(projectCost, token).ConfigureAwait(false);

                if (ProjectCost == null)
                {
                    _logger.Information("Error - 400 Something Went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateProjectCostDetails method with projectCost");
                return Ok(new ApiResultResponse(200, ProjectCost, "Project Cos Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateProjectCostDetails  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdProjectCostDetails")]
        public async Task<IActionResult> GetByIdProjectCostDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdProjectCostDetailsAsync method for Id = " + Id+ "CancellationToken" + token);
                var ProjectCost = await _projectCostDetails.GetByIdProjectCostDetails(Id, token).ConfigureAwait(false);
                if (ProjectCost == null)
                {
                    _logger.Information("Error - 404 Project Cos Details Not found.");
                    return new NotFoundObjectResult(new ApiException(404, "Project Cos Details Not found."));
                }
                _logger.Information("Completed - GetByIdProjectCostDetailsAsync method for Id = " + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, ProjectCost, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdProjectCostDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteProjectCostDetails")]
        public async Task<IActionResult> DeleteProjectCostDetails(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteProjectCostDetails method for Id = " + Id + "CancellationToken" + token);
                bool isDeleted = await _projectCostDetails.DeleteProjectCostDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Project Cost details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Project Cost details not exists!"));
                }
                _logger.Information("Started - DeleteProjectCostDetails method for Id = " + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, true, "Project Cost Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteProjectCostDetails page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
