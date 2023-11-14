using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.UnitDetailsModule;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM.UnitDetails
{
    public class UnitDetailsController : BaseApiController
    {
        private readonly IUnitDetailsService _unitDetailsService;
        private readonly ILogger _logger;
        public UnitDetailsController(ILogger logger, IUnitDetailsService unitDetailsService)
        {
            _logger = logger;
            _unitDetailsService = unitDetailsService;
        }

        #region Name of Unit
        /// <summary>
        ///  Author: Sandeep; Module: Name Of Unit; Date:24/08/2022
        /// </summary>
        [HttpGet, Route(UnitDetailsRouteName.GetNameOfUnit)]
        public async Task<IActionResult> GetNameOfUnit(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetNameOfUnit + accountNumber);
                var lst = await _unitDetailsService.GetNameOfUnit(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetNameOfUnit + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.NameOfSecurity));
                throw;
            }
        }
        [HttpPost, Route(UnitDetailsRouteName.UpdateNameOfUnit)]
        public async Task<IActionResult> UpdateNameOfUnit(IdmUnitDetailDTO idmUnitDetail, CancellationToken token)
        {
            try
            {
                if (idmUnitDetail != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.NameOfSecurity);
                    var basicDetail = await _unitDetailsService.UpdateNameOfUnit(idmUnitDetail, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.UpdateCompleted + Module.NameOfSecurity);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.NameOfSecurity));
                throw;
            }
        }
        #endregion 

        #region Change Location Address

        /// <summary>
        ///  Author: Gagana; Module: Change in Address; Date:26/08/2022
        ///  Update on 10/11/2022 
        /// </summary>
        [HttpGet, Route(UnitDetailsRouteName.GetAllAddressDetails)]
        public async Task<IActionResult> GetAllAddressDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllAddressDetails + accountNumber);
                var lst = await _unitDetailsService.GetAllAddressDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllAddressDetails + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeLocationAddress));
                throw;
            }
        }
        [HttpGet, Route(UnitDetailsRouteName.GetAllMasterPinCodeDetails)]
        public async Task<IActionResult> GetAllMasterPinCodeDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllMasterPinCodeDetails);
                var lst = await _unitDetailsService.GetAllMasterPinCodeDetails(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllMasterPinCodeDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.MasterPinCodeDetails));
                throw;
            }
        }
        [HttpGet, Route(UnitDetailsRouteName.GetAllPinCodeDistrictDetails)]
        public async Task<IActionResult> GetAllPinCodeDistrictDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllPinCodeDistrictDetails);
                var lst = await _unitDetailsService.GetAllPinCodeDistrictDetails(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllPinCodeDistrictDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PinCodeDistrictDetails));
                throw;
            }
        }
        [HttpPost, Route(UnitDetailsRouteName.UpdateAddressDetails)]
        public async Task<IActionResult> UpdateAddressDetails(IdmUnitAddressDTO AddressDTO, CancellationToken token)
        {
            try
            {
                if (AddressDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.IdmUnitAddressDTO,
                    AddressDTO.IdmUtAddressRowid, AddressDTO.UtAddress, AddressDTO.UtDistCd, AddressDTO.UtTlqCd, AddressDTO.UtHobCd, AddressDTO.UtVilCd, AddressDTO.UtPincode, AddressDTO.UtCity));
                    var basicDetail = await _unitDetailsService.UpdateAddressDetails(AddressDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.IdmUnitAddressDTO,
                    AddressDTO.IdmUtAddressRowid, AddressDTO.UtAddress, AddressDTO.UtDistCd, AddressDTO.UtTlqCd, AddressDTO.UtHobCd, AddressDTO.UtVilCd, AddressDTO.UtPincode, AddressDTO.UtCity));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
              
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeLocationAddress));
                throw;
            }
        }
        [HttpGet, Route(UnitDetailsRouteName.GetAllTalukDetails)]
        public async Task<IActionResult> GetAllTalukDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllTalukDetails);
                var lst = await _unitDetailsService.GetAllTalukDetails(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllTalukDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PinCodeDistrictDetails));
                throw;
            }
        }

        [HttpGet, Route(UnitDetailsRouteName.GetAllHobliDetails)]
        public async Task<IActionResult> GetAllHobliDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllHobliDetails);
                var lst = await _unitDetailsService.GetAllHobliDetails(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllHobliDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PinCodeDistrictDetails));
                throw;
            }
        }


        [HttpGet, Route(UnitDetailsRouteName.GetAllVillageDetails)]
        public async Task<IActionResult> GetAllVillageDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllVillageDetails);
                var lst = await _unitDetailsService.GetAllVillageDetails(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllVillageDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PinCodeDistrictDetails));
                throw;
            }
        }
        #endregion

        #region Promoter Profile
        /// <summary>
        ///  Author: Dev Patel; Module: Promoter Profile; Date:26/08/2022
        ///  Author: Gagana; Module: Get Promoter Details from Master Table; Date:14/11/2022
        /// </summary>
        /// 

        [HttpGet, Route(UnitDetailsRouteName.GetAllMasterPromoterProfileDetails)]
        public async Task<IActionResult> GetAllMasterPromoterProfileDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllMasterPromoterProfileDetails);
                var lst = await _unitDetailsService.GetAllMasterPromoterProfileDetails(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllMasterPromoterProfileDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.MasterPromoterProfile));
                throw;
            }
        }
        [HttpGet, Route(UnitDetailsRouteName.GetAllPromoterProfileDetails)]
        public async Task<IActionResult> GetAllPromoterProfileDetails(long? accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllPromoterProfileDetails + accountNumber);
                var lst = await _unitDetailsService.GetAllPromoterProfileDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllPromoterProfileDetails + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterProfile));
                throw;
            }
        }
        [HttpPost, Route(GenerateReceipt.CreatePromoterProfileDetails)]
        public async Task<IActionResult> CreatePromoterProfileDetails(IdmPromoterDTO PromprofileDto, CancellationToken token)
        {
            try
            {
                if (PromprofileDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.IdmPromoterDTO,
                   PromprofileDto.IdmPromId, PromprofileDto.PromoterCode, PromprofileDto.PromName));

                    var basicDetail = await _unitDetailsService.CreatePromoterProfileDetails(PromprofileDto, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.IdmPromoterDTO,
                   PromprofileDto.IdmPromId, PromprofileDto.PromoterCode, PromprofileDto.PromName));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterProfile));
                throw;
            }
        }
        [HttpPost, Route(UnitDetailsRouteName.UpdatePromoterProfileDetails)]
        public async Task<IActionResult> UpdatePromoterProfileDetails(IdmPromoterDTO PromprofileDto, CancellationToken token)
        {
            try
            {
                if (PromprofileDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.IdmPromoterDTO,
                   PromprofileDto.IdmPromId, PromprofileDto.PromoterCode, PromprofileDto.PromName));

                    var basicDetail = await _unitDetailsService.UpdatePromoterProfileDetails(PromprofileDto, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.IdmPromoterDTO,
                    PromprofileDto.IdmPromId, PromprofileDto.PromoterCode, PromprofileDto.PromName));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));

                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterProfile));
                throw;
            }
        }
        [HttpPost, Route(GenerateReceipt.DeletePromoterProfileDetails)]
        public async Task<IActionResult> DeletePromoterProfileDetails(IdmPromoterDTO PromprofileDto, CancellationToken token)
        {
            try
            {
                if (PromprofileDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + " " + LogAttribute.IdmPromoterDTO,
                  PromprofileDto.IdmPromId, PromprofileDto.PromoterCode, PromprofileDto.PromName));

                    var basicDetail = await _unitDetailsService.DeletePromoterProfileDetails(PromprofileDto, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + " " + LogAttribute.IdmPromoterDTO,
                   PromprofileDto.IdmPromId, PromprofileDto.PromoterCode, PromprofileDto.PromName));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
              
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterProfile));
                throw;
            }
        }
        #endregion 

        #region Promoter Bank

        /// <summary>
        ///  Author: Dev Patel; Module: Promoter Bank ; Date:01/09/2022
        /// </summary>
        [HttpGet, Route(UnitDetailsRouteName.GetAllPromoterBankInfo)]
        public async Task<IActionResult> GetAllPromoterBankInfo(long? accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllPromoterBankInfo + accountNumber);
                var lst = await _unitDetailsService.GetAllPromoterBankInfo(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllPromoterBankInfo + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterBank));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.CreatePromoterBankInfo)]
        public async Task<IActionResult> CreatePromoterBankInfo(IdmPromoterBankDetailsDTO changePromoterBankDetail, CancellationToken token)
        {
            try
            {
                if (changePromoterBankDetail != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.PromoterBank);
                    var basicDetail = await _unitDetailsService.CreatePromoterBankInfo(changePromoterBankDetail, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.CreateCompleted + Module.PromoterBank);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
              
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterBank));
                throw;
            }
        }

        [HttpPost, Route(UnitDetailsRouteName.UpdatePromoterBankInfo)]
        public async Task<IActionResult> UpdatePromoterBankInfo(IdmPromoterBankDetailsDTO changePromoterBankDetail, CancellationToken token)
        {
            try
            {
                if (changePromoterBankDetail != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.PromoterBank);
                    var basicDetail = await _unitDetailsService.UpdatePromoterBankInfo(changePromoterBankDetail, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.UpdateCompleted + Module.PromoterBank);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
             
             
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterBank));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.DeletePromoterBankInfo)]
        public async Task<IActionResult> DeletePromoterBankInfo(IdmPromoterBankDetailsDTO changePromoterBankDetail, CancellationToken token)
        {
            try
            {
                if (changePromoterBankDetail != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.PromoterBank);

                var basicDetail = await _unitDetailsService.DeletePromoterBankInfo(changePromoterBankDetail, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.DeleteCompleted + Module.PromoterBank);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                   
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterBank));
                throw;
            }
        }
        #endregion

        #region Promoter Address
        /// <summary>
        ///  Author: Gagana; Module: Promoter Address; Date:01/09/2022
        /// </summary>

        [HttpGet, Route(UnitDetailsRouteName.GetAllPromoterAddressDetails)]
        public async Task<IActionResult> GetAllPromoterAddressDetails(long? accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllPromoterAddressDetails + accountNumber);
                var lst = await _unitDetailsService.GetAllPromoterAddressDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllPromoterAddressDetails + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            { 
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterAddress));
                throw;
            }
        }

        [HttpPost, Route(UnitDetailsRouteName.UpdatePromAddressDetails)]
        public async Task<IActionResult> UpdatePromAddressDetails(IdmPromAddressDTO PromAddressDto, CancellationToken token)
        {
            try
            {
                if (PromAddressDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.IdmPromAddressDto,
               PromAddressDto.IdmPromadrId, PromAddressDto.PromoterCode, PromAddressDto.PromAddress, PromAddressDto.PromStateCd, PromAddressDto.PromDistrictCd, PromAddressDto.PromPincode, PromAddressDto.AdrPermanent));
                    var basicDetail = await _unitDetailsService.UpdatePromAddressDetails(PromAddressDto, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.IdmPromAddressDto,
                   PromAddressDto.IdmPromadrId, PromAddressDto.PromoterCode, PromAddressDto.PromAddress, PromAddressDto.PromStateCd, PromAddressDto.PromDistrictCd, PromAddressDto.PromPincode, PromAddressDto.AdrPermanent));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
           
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterAddress));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.CreatePromAddressDetails)]
        public async Task<IActionResult> CreatePromAddressDetails(IdmPromAddressDTO PromAddressDto, CancellationToken token)
        {
            try
            {
                if (PromAddressDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.IdmPromAddressDto,
                                       PromAddressDto.IdmPromadrId, PromAddressDto.PromoterCode, PromAddressDto.PromAddress, PromAddressDto.PromStateCd, PromAddressDto.PromDistrictCd, PromAddressDto.PromPincode));
                    var basicDetail = await _unitDetailsService.CreatePromAddressDetails(PromAddressDto, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.IdmPromAddressDto,
                   PromAddressDto.IdmPromadrId, PromAddressDto.PromoterCode, PromAddressDto.PromAddress, PromAddressDto.PromStateCd, PromAddressDto.PromDistrictCd, PromAddressDto.PromPincode));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterAddress));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.DeletePromAddressDetails)]
        public async Task<IActionResult> DeletePromAddressDetails(IdmPromAddressDTO PromAddressDto, CancellationToken token)
        {
            try
            {
                if (PromAddressDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + " " + LogAttribute.IdmPromAddressDto,
                   PromAddressDto.IdmPromadrId, PromAddressDto.PromoterCode, PromAddressDto.PromAddress, PromAddressDto.PromStateCd, PromAddressDto.PromDistrictCd, PromAddressDto.PromPincode, PromAddressDto.AdrPermanent));
                    var basicDetail = await _unitDetailsService.DeletePromAddressDetails(PromAddressDto, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + " " + LogAttribute.IdmPromAddressDto,
                   PromAddressDto.IdmPromadrId, PromAddressDto.PromoterCode, PromAddressDto.PromAddress, PromAddressDto.PromStateCd, PromAddressDto.PromDistrictCd, PromAddressDto.PromPincode, PromAddressDto.AdrPermanent));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterAddress));
                throw;
            }
        }

        #endregion 

        #region Product Details

        /// <summary>
        ///  Author:Gowtham S; Module: Product Details; Date:26/08/2022
        /// </summary>
        [HttpGet, Route(UnitDetailsRouteName.GetAllProductDetails)]
        public async Task<IActionResult> GetAllProductDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllProductDetails + accountNumber);
                var lst = await _unitDetailsService.GetAllProductDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllPromoterAddressDetails + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Product));
                throw;
            }
        }
        [HttpGet, Route(UnitDetailsRouteName.GetAllProductList)]
        public async Task<IActionResult> GetAllProductList(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllProductList);
                var lst = await _unitDetailsService.GetAllProductList(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllProductList);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Product));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.CreateProductDetails)]
        public async Task<IActionResult> CreateProductDetails(IdmUnitProductsDTO ProductDTO, CancellationToken token)
        {
            try
            {
                if (ProductDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.ProductDTO,
                   ProductDTO.IdmUtproductRowid, ProductDTO.IndId, ProductDTO.ProdId));
                    var basicDetail = await _unitDetailsService.CreateProductDetails(ProductDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.ProductDTO,
                 ProductDTO.IdmUtproductRowid, ProductDTO.IndId, ProductDTO.ProdId));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
               
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Product));
                throw;
            }
        }

        [HttpPost, Route(UnitDetailsRouteName.UpdateProductDetails)]
        public async Task<IActionResult> UpdateProductDetails(IdmUnitProductsDTO ProductDTO, CancellationToken token)
        {
            try
            {
                if (ProductDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.ProductDTO,
                   ProductDTO.IdmUtproductRowid, ProductDTO.IndId, ProductDTO.ProdId));
                    var basicDetail = await _unitDetailsService.UpdateProductDetails(ProductDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.ProductDTO,
                  ProductDTO.IdmUtproductRowid, ProductDTO.IndId, ProductDTO.ProdId));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Product));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.DeleteProductDetails)]
        public async Task<IActionResult> DeleteProductDetails(IdmUnitProductsDTO ProDetailsDto, CancellationToken token)
        {
            try
            {
                if (ProDetailsDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + " " + LogAttribute.ProductDTO,
                                       ProDetailsDto.IdmUtproductRowid, ProDetailsDto.IndId, ProDetailsDto.ProdId));
                    var basicDetail = await _unitDetailsService.DeleteProductDetails(ProDetailsDto, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + " " + LogAttribute.ProductDTO,
                   ProDetailsDto.IdmUtproductRowid, ProDetailsDto.IndId, ProDetailsDto.ProdId));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Product));
                throw;
            }
        }

        #endregion

        #region Change Bank Details
        /// <summary>
        ///  Author: MJ; Module: Change Bank Details; Date:08/09/2022
        /// </summary>
        [HttpGet, Route(UnitDetailsRouteName.GetAllChangeBankDetails)]
        public async Task<IActionResult> GetAllChangeBankDetailsList(long? accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllChangeBankDetailsList + accountNumber);
                var lst = await _unitDetailsService.GetAllChangeBankDetailsList(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllChangeBankDetailsList);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeBankDetails));
                throw;
            }
        }
        [HttpGet, Route(UnitDetailsRouteName.GetAllIfscBankDetails)]
        public async Task<IActionResult> GetAllIfscBankDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllIfscBankDetails);
                var lst = await _unitDetailsService.GetAllIfscBankDetails(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllIfscBankDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.IfscBankDetails));
                throw;
            }
        }
        [HttpPost, Route(GenerateReceipt.CreateChangeBankDetails)]
        public async Task<IActionResult> CreateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail, CancellationToken token)
        {
            try
            {
                if (changeBankDetail != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.ChangeBankDetails);
                    var basicDetail = await _unitDetailsService.CreateChangeBankDetails(changeBankDetail, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.CreateCompleted + Module.ChangeBankDetails);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
              
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeBankDetails));
                throw;
            }
        }

        [HttpPost, Route(UnitDetailsRouteName.UpdateChangeBankDetails)]
        public async Task<IActionResult> UpdateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail, CancellationToken token)
        {
            try
            {
                if (changeBankDetail != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.ChangeBankDetails);
                    var basicDetail = await _unitDetailsService.UpdateChangeBankDetails(changeBankDetail, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.UpdateCompleted + Module.ChangeBankDetails);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
              
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeBankDetails));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.DeleteChangeBankDetails)]
        public async Task<IActionResult> DeleteChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail, CancellationToken token)
        {
            try
            {
                if (changeBankDetail != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.ChangeBankDetails);
                    var basicDetail = await _unitDetailsService.DeleteChangeBankDetails(changeBankDetail, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.DeleteCompleted + Module.ChangeBankDetails);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeBankDetails));
                throw;
            }
        }
        #endregion 

        #region Asset information 
        /// <summary>
        ///  Author:Gowtham S ; Module: Change Asset Information; Date:08/09/2022
        /// </summary>


        [HttpGet, Route(UnitDetailsRouteName.GetAllAssetTypeDetails)]
        public async Task<IActionResult> GetAllAssetTypeDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllAssetTypeDetails);
                var lst = await _unitDetailsService.GetAllAssetTypeDetails(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllAssetTypeDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AssetTypeDetails));
                throw;
            }
        }

        [HttpGet, Route(UnitDetailsRouteName.GetAllAssetCategaryDetails)]
        public async Task<IActionResult> GetAllAssetCategaryDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllAssetCategaryDetails);
                var lst = await _unitDetailsService.GetAllAssetCategaryDetails(token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllAssetTypeDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AssetCategaryDetails));
                throw;
            }
        }


        [HttpGet, Route(UnitDetailsRouteName.GetAllPromoterAssetDetails)]
        public async Task<IActionResult> GetAllPromoterAssetDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllPromoterAssetDetails + accountNumber);
                var lst = await _unitDetailsService.GetAllPromoterAssetDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllPromoterAssetDetails + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeAssetInformation));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.CreateAssetDetails)]
        public async Task<IActionResult> CreateAssetDetails(IdmPromAssetDetDTO AssetDTO, CancellationToken token)
        {
            try
            {
                if (AssetDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.ProductDTO,
                   AssetDTO.IdmPromassetId, AssetDTO.AssetTypeCD, AssetDTO.AssetCatCD));
                    var basicDetail = await _unitDetailsService.CreateAssetDetails(AssetDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.ProductDTO,
                  AssetDTO.IdmPromassetId, AssetDTO.AssetTypeCD, AssetDTO.AssetCatCD));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeAssetInformation));
                throw;
            }
        }
        [HttpPost, Route(UnitDetailsRouteName.UpdateAssetDetails)]
        public async Task<IActionResult> UpdateAssetDetails(IdmPromAssetDetDTO AssetDTO, CancellationToken token)
        {
            try
            {
                if (AssetDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.ProductDTO,
                   AssetDTO.IdmPromassetId, AssetDTO.AssetTypeCD, AssetDTO.AssetCatCD));
                    var basicDetail = await _unitDetailsService.UpdateAssetDetails(AssetDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.ProductDTO,
                  AssetDTO.IdmPromassetId, AssetDTO.AssetTypeCD, AssetDTO.AssetCatCD));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }   
                
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeAssetInformation));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.DeleteAssetDetails)]
        public async Task<IActionResult> DeleteAssetDetails(IdmPromAssetDetDTO AssetdetailsDTO, CancellationToken token)
        {
            try
            {
                if (AssetdetailsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + " " + LogAttribute.ProductDTO,
                                       AssetdetailsDTO.IdmPromassetId, AssetdetailsDTO.AssetTypeCD, AssetdetailsDTO.AssetCatCD));
                    var basicDetail = await _unitDetailsService.DeleteAssetDetails(AssetdetailsDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + " " + LogAttribute.ProductDTO,
                   AssetdetailsDTO.IdmPromassetId, AssetdetailsDTO.AssetTypeCD, AssetdetailsDTO.AssetCatCD));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangeAssetInformation));
                throw;
            }
        }
        #endregion  

        #region  Promoter Liability Info
        /// <summary>
        ///  Author: Sandeep; Module: Promoter Liability Information; Date:08/09/2022
        /// </summary>
        [HttpGet, Route(UnitDetailsRouteName.GetAllPromoterLiabiltyInfo)]
        public async Task<IActionResult> GetAllPromoterLiabiltyInfo(long? accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllPromoterLiabiltyInfo + accountNumber);
                var lst = await _unitDetailsService.GetAllPromoterLiabiltyInfo(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllPromoterLiabiltyInfo + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangePromoterLiabilityInformation));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.CreatePromoterLiabilityInfo)]
        public async Task<IActionResult> CreatePromoterLiabilityInfo(TblPromoterLiabDetDTO PromLiabDto, CancellationToken token)
        {
            try
            {
                if (PromLiabDto != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.ChangePromoterLiabilityInformation);
                    var basicDetail = await _unitDetailsService.CreatePromoterLiabiltyInfo(PromLiabDto, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.CreateCompleted + Module.ChangePromoterLiabilityInformation);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
              
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangePromoterLiabilityInformation));
                throw;
            }
        }

        [HttpPost, Route(UnitDetailsRouteName.UpdatePromoterLiabilityInfo)]
        public async Task<IActionResult> UpdatePromoterLiabilityInfo(TblPromoterLiabDetDTO PromLiabDto, CancellationToken token)
        {
            try
            {
                if (PromLiabDto != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.ChangePromoterLiabilityInformation);
                    var basicDetail = await _unitDetailsService.UpdatePromoterLiabiltyInfo(PromLiabDto, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.UpdateCompleted + Module.ChangePromoterLiabilityInformation);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
              
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangePromoterLiabilityInformation));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.DeletePromoterLiabilityInfo)]
        public async Task<IActionResult> DeletePromoterLiabiltyInfo(TblPromoterLiabDetDTO PromLiabDto, CancellationToken token)
        {
            try
            {
                if (PromLiabDto != null )
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.ChangePromoterLiabilityInformation);
                    var basicDetail = await _unitDetailsService.DeletePromoterLiabiltyInfo(PromLiabDto, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.DeleteCompleted + Module.ChangePromoterLiabilityInformation);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ChangePromoterLiabilityInformation));
                throw;
            }
        }

        #endregion

        #region  Promoter NetWorth
        /// <summary>
        ///  Author: Sandeep; Module: Promoter NetWorth Information; Date:08/09/2022
        /// </summary>
        [HttpGet, Route(UnitDetailsRouteName.GetAllPromoterNetWorth)]
        public async Task<IActionResult> GetAllPromoterNetWorth(long? accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllPromoterNetWorth + accountNumber);
                var lst = await _unitDetailsService.GetAllPromoterNetWorth(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllPromoterNetWorth + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterNetWorthInformation));
                throw;
            }
        }

        [HttpPost, Route(UnitDetailsRouteName.SaveAssetNetworthDetails)]
        public async Task<IActionResult> SaveAssetNetworthDetails(TblPromoterNetWortDTO changePromoterNWDetail, CancellationToken token)
        {
            try
            {
                _logger.Information(LogAttribute.UpdateStarted + Module.PromoterNetWorthInformation);
                var basicDetail = await _unitDetailsService.SaveAssetNetworthDetails(changePromoterNWDetail, token).ConfigureAwait(false);
                if (changePromoterNWDetail == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                _logger.Information(LogAttribute.UpdateCompleted + Module.PromoterNetWorthInformation);
                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterBank));
                throw;
            }
        }

        [HttpPost, Route(UnitDetailsRouteName.SaveLiabilityNetworthDetails)]
        public async Task<IActionResult> SaveLiabilityNetworthDetails(TblPromoterNetWortDTO changePromoterNWDetail, CancellationToken token)
        {
            try
            {
                _logger.Information(LogAttribute.UpdateStarted + Module.PromoterNetWorthInformation);
                var basicDetail = await _unitDetailsService.SaveLiabilityNetworthDetails(changePromoterNWDetail, token).ConfigureAwait(false);
                if (changePromoterNWDetail == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                _logger.Information(LogAttribute.UpdateCompleted + Module.PromoterNetWorthInformation);
                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterBank));
                throw;
            }
        }
        #endregion


        #region Land Assets
        [HttpGet, Route(UnitDetailsRouteName.GetAllLandAssets)]
        public async Task<IActionResult> GetAllLandDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllLandAssets + accountNumber);
                var lst = await _unitDetailsService.GetAllLandDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllLandAssets + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterProfile));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.CreateLandAssets)]
        public async Task<IActionResult> CreateLandAssets(TblIdmProjLandDTO ProjLandDto, CancellationToken token)
        {
            try
            {
                if (ProjLandDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.TblIdmProjLandDTO,
                   ProjLandDto.PjLandRowId));

                    var basicDetail = await _unitDetailsService.CreateLandAssets(ProjLandDto, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.TblIdmProjLandDTO,
                   ProjLandDto.PjLandRowId));
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.PromoterProfile));
                throw;
            }
        }


        #endregion

       


    }

}
