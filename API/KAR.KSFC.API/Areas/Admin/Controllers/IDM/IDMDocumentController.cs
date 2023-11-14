using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM
{

    public class IDMDocumentController : BaseApiController
    {
        private readonly IConfiguration _configure;
        private readonly IIdmFileService _idmFileService;
        private readonly ILogger _logger;

        public IDMDocumentController(IConfiguration configure, ILogger logger, IIdmFileService idmFileService)
        {
            _configure = configure;
            _logger = logger;
            _idmFileService = idmFileService;
        }


        /// <summary>
        /// File save
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("IDMFileUpload")]
        public async Task<IActionResult> FileUploadAsync(IdmFileUploadDto fileUpload, CancellationToken token)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedFileUploadAsyncmethod + fileUpload);
                var result = await _idmFileService.UploadFile(fileUpload.Bytes, fileUpload.SubModuleId, fileUpload.SubModuleType,fileUpload.MainModule, _configure, token).ConfigureAwait(false);
                if (result == null)
                {
                    _logger.Information(ErrorMsg.FileUploadError);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.FileUploadfailed));
                }
                _logger.Information(CommonLogHelpers.CompletedFileUploadAsyncmethod + fileUpload);
                return Ok(new ApiResultResponse(result, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error( ErrorMsg.FileUploadAsyncLoadingError + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace);
                return new BadRequestObjectResult(new ApiResponse(400, ex.Message));
            }
        }

        /// <summary>
        /// Get Encrypted File by doc Id
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetEncryptedFileById")]
        public async Task<IActionResult> GetEncryptedFileAsync(string mainModule, string documentId, CancellationToken token)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedGetEncryptedFileAsyncMethod + documentId);
                var data = await _idmFileService.GetEncryptedFileById(mainModule,documentId, token).ConfigureAwait(false);
                if (data == null)
                {
                    _logger.Information(ErrorMsg.NoDataError);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.NoDataError));
                }
                _logger.Information(CommonLogHelpers.CompletedGetEncryptedFileAsyncMethod + documentId);
                return Ok(new ApiResultResponse(data, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(ErrorMsg.GetEncryptedFileAsyncLoadingError + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetAllDocumentList")]
        public async Task<IActionResult> GetDocumentListAsync(string mainModule,CancellationToken token)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedGetDocumentListAsyncmethod + mainModule);
                var result = await _idmFileService.GetFileListAsync(mainModule,token).ConfigureAwait(false);
                _logger.Information(CommonLogHelpers.CompletedGetDocumentListAsyncmethod + mainModule);
                return Ok(new ApiResultResponse(result, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(ErrorMsg.GetDocumentListAsyncLoadingError + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace);
                throw;
            }
        }

        [HttpDelete, Route("DeleteDocument")]
        public async Task<IActionResult> DeleteDocumentAsync(string mainModule,string documentUniqueId, CancellationToken token)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedDeleteDocumentAsyncmethod + documentUniqueId);
                var result = await _idmFileService.DeleteFileAsync(mainModule,documentUniqueId, _configure, token).ConfigureAwait(false);
                if (result)
                {
                    _logger.Information(CommonLogHelpers.CompletedDeleteDocumentAsyncmethod + documentUniqueId);
                    return Ok(new ApiResultResponse(true, CommonLogHelpers.Success));
                }
                _logger.Information(ErrorMsg.FileDeletionfailed);
                return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.FileDeletionfailed));
            }
            catch (System.Exception ex)
            {
                _logger.Error(ErrorMsg.DeleteDocumentAsyncLoadingError + ex.Message + Environment.NewLine +ErrorMsg.Stack + ex.StackTrace);
                throw;
            }
        }
    }
}
