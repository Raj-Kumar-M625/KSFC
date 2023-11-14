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
    public class MeansOfFinanceController : BaseApiController
    {
        //private readonly ILogger<MeansOfFinanceController> _logger;
        private readonly IMeansofFinance _meansofFinance;
        private readonly ILogger _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeansofFinance"></param>
        /// <param name="logger"></param>
        public MeansOfFinanceController(IMeansofFinance MeansofFinance, ILogger logger)
        {
            _meansofFinance = MeansofFinance;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeansofFinance"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("AddMeansOfFinance")]
        public async Task<IActionResult> AddMeansOfFinance([FromBody] List<ProjectMeansOfFinanceDTO> MeansofFinance, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddMeansOfFinance method with MeansofFinance");
                var FinanceMeans = await _meansofFinance.AddMeansOfFinance(MeansofFinance, token).ConfigureAwait(false);

                if (FinanceMeans == null)
                {
                    _logger.Information("Error - 400 Something Went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddMeansOfFinance method with MeansofFinance");
                return Ok(new ApiResultResponse(200, FinanceMeans, "Project Cost details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddMeansOfFinance  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeansofFinance"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("GetAllMeansOfFinance")]
        public async Task<IActionResult> GetAllMeansOfFinance(ProjectMeansOfFinanceDTO MeansofFinance, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetAllMeansOfFinance method with MeansofFinance ");
                var FinanceMeans = await _meansofFinance.GetAllMeansOfFinance(MeansofFinance, token).ConfigureAwait(false);

                if (FinanceMeans == null)
                {
                    _logger.Information("Error - 400 Something went Wrong ");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - GetAllMeansOfFinance method with MeansofFinance ");
                return Ok(new ApiResultResponse(200, FinanceMeans, "Project Cost details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetAllMeansOfFinance  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }


        [HttpPost, Route("UpdateMeansOfFinance")]
        public async Task<IActionResult> UpdateMeansOfFinance(List<ProjectMeansOfFinanceDTO> MeansofFinance, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateMeansOfFinance method with MeansofFinance ");
                var FinanceMeans = await _meansofFinance.UpdateMeansOfFinance(MeansofFinance, token).ConfigureAwait(false);

                if (FinanceMeans == null)
                {
                    _logger.Information("Error - 400 Something Went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateMeansOfFinance method with MeansofFinance ");
                return Ok(new ApiResultResponse(200, FinanceMeans, "Project Cos Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateMeansOfFinance page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdMeansOfFinance")]
        public async Task<IActionResult> GetByIdMeansOfFinance(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdMeansOfFinance method for Id = " + Id + "CancellationToken" + token);
                var FinanceMeans = await _meansofFinance.GetByIdMeansOfFinance(Id, token).ConfigureAwait(false);
                if (FinanceMeans == null)
                {
                    _logger.Information("Error - 404 Project Cos Details Not found.");
                    return new NotFoundObjectResult(new ApiException(404, "Project Cos Details Not found."));
                }
                _logger.Information("Completed - GetByIdMeansOfFinance method for Id = " + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, FinanceMeans, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdMeansOfFinance  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteMeansOfFinance")]
        public async Task<IActionResult> DeleteMeansOfFinance(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteMeansOfFinance method for Id = " + Id + "CancellationToken" + token);
                bool isDeleted = await _meansofFinance.DeleteMeansOfFinance(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Project Cost details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Project Cost details not exists!"));
                }
                _logger.Information("Completed - DeleteMeansOfFinance method for Id = " + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, true, "Project Cos Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteMeansOfFinance  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
