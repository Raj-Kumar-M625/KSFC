using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.ProjectDetails
{
    [Authorize]
    public class FinancialPreYearController : BaseApiController
    {
        private readonly IProjectFinancialDetails _financeDetails;
        private readonly ILogger _logger;
        /// <summary>
        /// Initilize Services in the Construtor
        /// </summary>
        /// <param name="financeDetails"></param>

        public FinancialPreYearController(IProjectFinancialDetails financeDetails, ILogger logger)
        {
            _financeDetails = financeDetails;
            _logger = logger;
        }

        /// <summary>
        /// Add Finance Pre Year Details
        /// </summary>
        /// <param name="financeDetails"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("AddFinancePreYearDetails")]
        public async Task<IActionResult> AddFinancePreYearDetailsAsync([FromBody] List<ProjectFinancialYearDetailsDTO> financeDetails, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddFinancePreYearDetailsAsync method with financeDetails");
                var financeYearDetails = await _financeDetails.AddProjectFinancialDetails(financeDetails, token).ConfigureAwait(false);

                if (financeYearDetails == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddFinancePreYearDetailsAsync method with financeDetails");
                return Ok(new ApiResultResponse(200, financeYearDetails, "Project Financial Previous  Year details created Successfully"));

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddFinancePreYearDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// Get Finance Pre Year Details
        /// </summary>
        /// <param name="financeDetails"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("GetAllFinancePreYearDetailsAsync")]
        public async Task<IActionResult> GetAllFinancePreYearDetailsAsync(ProjectFinancialYearDetailsDTO financeDetails, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetAllFinancePreYearDetailsAsync method with financeDetails");
                var financeYearDetails = await _financeDetails.GetAllProjectFinancial(financeDetails, token).ConfigureAwait(false);

                if (financeYearDetails == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - GetAllFinancePreYearDetailsAsync method with financeDetails");
                return Ok(new ApiResultResponse(200, financeYearDetails, "Project Financial Previous Year details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetAllFinancePreYearDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Update Finance Pre Year Details
        /// </summary>
        /// <param name="financeDetails"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("UpdateFinancePreYearDetails")]
        public async Task<IActionResult> UpdateFinancePreYearDetailsAsync(List<ProjectFinancialYearDetailsDTO> financeDetails, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateFinancePreYearDetailsAsync method with financeDetails ");
                var financeYearDetails = await _financeDetails.UpdateProjectFinancialDetails(financeDetails, token).ConfigureAwait(false);

                if (financeYearDetails == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateFinancePreYearDetailsAsync method with financeDetails ");
                return Ok(new ApiResultResponse(200, financeYearDetails, "Project Financial Previous Year  Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateFinancePreYearDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Get By Id Finance Pre Year Details
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetByIdFinancePreYearDetails")]
        public async Task<IActionResult> GetByIdFinancePreYearDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdFinancePreYearDetailsAsync method for Id = " + Id + "CancellationToken" + token);
                var financeYearDetails = await _financeDetails.GetByIdProjectFinancialDetails(Id, token).ConfigureAwait(false);
                if (financeYearDetails == null)
                {
                    _logger.Information("Error - 404 Project Financial Previous  Year  Details Not found.");
                    return new NotFoundObjectResult(new ApiException(404, "Project Financial Previous  Year  Details Not found."));
                }
                _logger.Information("Completed - GetByIdFinancePreYearDetailsAsync method for Id = " + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, financeYearDetails, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdFinancePreYearDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Delete  Finance Pre Year Details
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("DeleteFinancePreYearDetails")]
        public async Task<IActionResult> DeleteFinancePreYearDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteFinancePreYearDetailsAsync method for Id = " + Id + "CancellationToken"+ token);
                bool isDeleted = await _financeDetails.DeleteProjectFinancialDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Project Financial Previous  Year details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Project Financial Previous  Year details not exists!"));
                }
                _logger.Information("Completed - DeleteFinancePreYearDetailsAsync method for Id = " + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, true, "Project Financial Previous  Year Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteFinancePreYearDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
