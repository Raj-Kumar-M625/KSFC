using AutoMapper;
using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
//using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.UnitDetails
{
    [Authorize]
    public class BasicDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<BasicDetailsController> _logger;
        private readonly IBasicDetails _basicDetails;
        private readonly ILogger _logger;
        public BasicDetailsController(UserInfo userInfo, IBasicDetails basicDetails,
                                     IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _basicDetails = basicDetails;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost, Route("AddBasicDetails")]
        public async Task<IActionResult> AddBasicDetailsAsync(BasicDetailsDto BasicDto, CancellationToken token)
        {
            try
            {
                _logger.Information(String.Format("Started - AddBasicDetailsAsync method for PromoterPan:{0} EnqBdetId:{1} EnqtempId:{2} EnqApplName:{3} EnqAddress:{4} EnqPlace:{5} EnqPincode:{6} EnqEmail:{7} AddlLoan:{8} UnitName:{9} EnqRepayPeriod:{10} EnqLoanamt:{11} ConstCd:{12} IndCd:{13} ConstType:{14} PurpCd:{15} PurposeOfLoan:{16} SizeCd:{17} SizeOfFirm:{18} ProdCd:{19} ProdCode:{20} VilCd:{21} VillageName:{22} PremCd:{23} PremCode:{24} OffcCd:{25} OffcCode:{26} UniqueId:{27} TypeOfIndustry:{28} DistrictCd:{29} TalukaCd:{30} HobliCd:{31} District:{32} Taluk:{33} Hobli:{34}",
                    BasicDto.PromoterPan, BasicDto.EnqBdetId, BasicDto.EnqtempId, BasicDto.EnqApplName, BasicDto.EnqAddress, BasicDto.EnqPlace, BasicDto.EnqPincode, BasicDto.EnqEmail, BasicDto.AddlLoan, BasicDto.UnitName, BasicDto.EnqRepayPeriod, BasicDto.EnqLoanamt, BasicDto.ConstCd, BasicDto.IndCd, BasicDto.ConstType, BasicDto.PurpCd, BasicDto.PurposeOfLoan, BasicDto.SizeCd, BasicDto.SizeOfFirm, BasicDto.ProdCd, BasicDto.ProdCode, BasicDto.VilCd, BasicDto.VillageName, BasicDto.PremCd, BasicDto.OffcCd, BasicDto.OffcCode, BasicDto.UniqueId, BasicDto.TypeOfIndustry, BasicDto.DistrictCd, BasicDto.TalukaCd, BasicDto.HobliCd, BasicDto.District, BasicDto.Taluk, BasicDto.Hobli));
                var basicDetail = await _basicDetails.AddBasicDetails(BasicDto, token).ConfigureAwait(false);
                if (basicDetail == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information(String.Format("Completed - AddBasicDetailsAsync method for PromoterPan:{0} EnqBdetId:{1} EnqtempId:{2} EnqApplName:{3} EnqAddress:{4} EnqPlace:{5} EnqPincode:{6} EnqEmail:{7} AddlLoan:{8} UnitName:{9} EnqRepayPeriod:{10} EnqLoanamt:{11} ConstCd:{12} IndCd:{13} ConstType:{14} PurpCd:{15} PurposeOfLoan:{16} SizeCd:{17} SizeOfFirm:{18} ProdCd:{19} ProdCode:{20} VilCd:{21} VillageName:{22} PremCd:{23} PremCode:{24} OffcCd:{25} OffcCode:{26} UniqueId:{27} TypeOfIndustry:{28} DistrictCd:{29} TalukaCd:{30} HobliCd:{31} District:{32} Taluk:{33} Hobli:{34}",
                    BasicDto.PromoterPan, BasicDto.EnqBdetId, BasicDto.EnqtempId, BasicDto.EnqApplName, BasicDto.EnqAddress, BasicDto.EnqPlace, BasicDto.EnqPincode, BasicDto.EnqEmail, BasicDto.AddlLoan, BasicDto.UnitName, BasicDto.EnqRepayPeriod, BasicDto.EnqLoanamt, BasicDto.ConstCd, BasicDto.IndCd, BasicDto.ConstType, BasicDto.PurpCd, BasicDto.PurposeOfLoan, BasicDto.SizeCd, BasicDto.SizeOfFirm, BasicDto.ProdCd, BasicDto.ProdCode, BasicDto.VilCd, BasicDto.VillageName, BasicDto.PremCd, BasicDto.OffcCd, BasicDto.OffcCode, BasicDto.UniqueId, BasicDto.TypeOfIndustry, BasicDto.DistrictCd, BasicDto.TalukaCd, BasicDto.HobliCd, BasicDto.District, BasicDto.Taluk, BasicDto.Hobli));
                return Ok(new ApiResultResponse(200, basicDetail, "Basic details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddBasicDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdateBasicDetails")]
        public async Task<IActionResult> UpdateBasicDetailsAsync(BasicDetailsDto BasicDto, CancellationToken token)
        {
            try
            {
                _logger.Information(String.Format("Started - UpdateBasicDetailsAsync method for PromoterPan:{0} EnqBdetId:{1} EnqtempId:{2} EnqApplName:{3} EnqAddress:{4} EnqPlace:{5} EnqPincode:{6} EnqEmail:{7} AddlLoan:{8} UnitName:{9} EnqRepayPeriod:{10} EnqLoanamt:{11} ConstCd:{12} IndCd:{13} ConstType:{14} PurpCd:{15} PurposeOfLoan:{16} SizeCd:{17} SizeOfFirm:{18} ProdCd:{19} ProdCode:{20} VilCd:{21} VillageName:{22} PremCd:{23} PremCode:{24} OffcCd:{25} OffcCode:{26} UniqueId:{27} TypeOfIndustry:{28} DistrictCd:{29} TalukaCd:{30} HobliCd:{31} District:{32} Taluk:{33} Hobli:{34}",
                    BasicDto.PromoterPan, BasicDto.EnqBdetId, BasicDto.EnqtempId, BasicDto.EnqApplName, BasicDto.EnqAddress, BasicDto.EnqPlace, BasicDto.EnqPincode, BasicDto.EnqEmail, BasicDto.AddlLoan, BasicDto.UnitName, BasicDto.EnqRepayPeriod, BasicDto.EnqLoanamt, BasicDto.ConstCd, BasicDto.IndCd, BasicDto.ConstType, BasicDto.PurpCd, BasicDto.PurposeOfLoan, BasicDto.SizeCd, BasicDto.SizeOfFirm, BasicDto.ProdCd, BasicDto.ProdCode, BasicDto.VilCd, BasicDto.VillageName, BasicDto.PremCd, BasicDto.OffcCd, BasicDto.OffcCode, BasicDto.UniqueId, BasicDto.TypeOfIndustry, BasicDto.DistrictCd, BasicDto.TalukaCd, BasicDto.HobliCd, BasicDto.District, BasicDto.Taluk, BasicDto.Hobli));
                var basic = await _basicDetails.UpdateBasicDetails(BasicDto, token);
                if (basic == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information(String.Format("Completed - UpdateBasicDetailsAsync method for PromoterPan:{0} EnqBdetId:{1} EnqtempId:{2} EnqApplName:{3} EnqAddress:{4} EnqPlace:{5} EnqPincode:{6} EnqEmail:{7} AddlLoan:{8} UnitName:{9} EnqRepayPeriod:{10} EnqLoanamt:{11} ConstCd:{12} IndCd:{13} ConstType:{14} PurpCd:{15} PurposeOfLoan:{16} SizeCd:{17} SizeOfFirm:{18} ProdCd:{19} ProdCode:{20} VilCd:{21} VillageName:{22} PremCd:{23} PremCode:{24} OffcCd:{25} OffcCode:{26} UniqueId:{27} TypeOfIndustry:{28} DistrictCd:{29} TalukaCd:{30} HobliCd:{31} District:{32} Taluk:{33} Hobli:{34}",
                     BasicDto.PromoterPan, BasicDto.EnqBdetId, BasicDto.EnqtempId, BasicDto.EnqApplName, BasicDto.EnqAddress, BasicDto.EnqPlace, BasicDto.EnqPincode, BasicDto.EnqEmail, BasicDto.AddlLoan, BasicDto.UnitName, BasicDto.EnqRepayPeriod, BasicDto.EnqLoanamt, BasicDto.ConstCd, BasicDto.IndCd, BasicDto.ConstType, BasicDto.PurpCd, BasicDto.PurposeOfLoan, BasicDto.SizeCd, BasicDto.SizeOfFirm, BasicDto.ProdCd, BasicDto.ProdCode, BasicDto.VilCd, BasicDto.VillageName, BasicDto.PremCd, BasicDto.OffcCd, BasicDto.OffcCode, BasicDto.UniqueId, BasicDto.TypeOfIndustry, BasicDto.DistrictCd, BasicDto.TalukaCd, BasicDto.HobliCd, BasicDto.District, BasicDto.Taluk, BasicDto.Hobli));
                return Ok(new ApiResultResponse(200, basic, "Basic details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateBasicDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdBasicDetails")]
        public async Task<IActionResult> GetByIdBasicDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdBasicDetailsAsync method for Id = " + Id);
                var basicDetails = await _basicDetails.GetBasicDetails(Id, token);
                if (basicDetails == null)
                {
                    _logger.Information("Error - 404 Basic details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Basic Details not exists."));
                }
                _logger.Information("Completed - GetByIdBasicDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, basicDetails, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdBasicDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpDelete, Route("DeleteBasicDetails")]
        public async Task<IActionResult> DeleteBasicDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteBasicDetailsAsync method for Id = " + Id);
                bool isDeleted = await _basicDetails.DeleteBasicDetails(Id, token);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Basic details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Basic details not exists!"));
                }
                _logger.Information("Completed - DeleteBasicDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Basic Details Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteBasicDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
