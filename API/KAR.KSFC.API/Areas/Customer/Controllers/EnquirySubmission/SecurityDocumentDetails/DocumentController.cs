using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Exceptions;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.SecurityDocumentDetails
{
    [KSFCAuthorization]
    public class DocumentController : BaseApiController
    {
        private readonly IConfiguration _configure;
        private readonly IFileService _fileService;
        private readonly ILogger _logger;
        /// <summary>
        /// Document Controller
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="fileService"></param>
        public DocumentController(IConfiguration configure, IFileService fileService, ILogger logger)
        {
            _configure = configure;
            _fileService = fileService;
            _logger = logger;
        }

        /// <summary>
        /// Get Encrypted File by doc Id
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetEncryptedFileById")]
        public async Task<IActionResult> GetEncryptedFileAsync(string documentId, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetEncryptedFileAsync method With documentId");
                var data = await _fileService.GetEncryptedFileById(documentId, token).ConfigureAwait(false);
                if (data == null)
                {
                    _logger.Information("Error - 400 No Data Found");
                    return new BadRequestObjectResult(new ApiResponse(400, "No Data Found."));
                }
                _logger.Information("Completed - GetEncryptedFileAsync method With documentId");
                return Ok(new ApiResultResponse(data, "Success"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetEncryptedFileAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// File save
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("FileUpload")]
        public async Task<IActionResult> FileUploadAsync(FileUploadDTO fileUpload, CancellationToken token)
        {
            try
            {
                _logger.Information(string.Format("Started - FileUploadAsync method for EnquiryId:{0} DocumentType:{1} FileType:{2}", fileUpload.EnquiryId, fileUpload.DocumentType, fileUpload.FileType));
                var result = await _fileService.UploadFile(fileUpload.Bytes, fileUpload.EnquiryId, fileUpload.DocumentType, fileUpload.FileType, _configure, token).ConfigureAwait(false);
                if (result == null)
                {
                    _logger.Information("Error - 400 File Upload failed");
                    return new BadRequestObjectResult(new ApiResponse(400, "File Upload failed."));
                }
                _logger.Information(string.Format("Completed - FileUploadAsync method for EnquiryId:{0} DocumentType:{1} FileType:{2}", fileUpload.EnquiryId, fileUpload.DocumentType, fileUpload.FileType));
                return Ok(new ApiResultResponse(result, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured while loading FileUploadAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return new BadRequestObjectResult(new ApiResponse(400, ex.Message));
            }
        }


        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="FileSecurityExceptions"></exception>
        [HttpDelete, Route("DeleteDocument")]
        public async Task<IActionResult> DeleteDocumentAsync(int documentId, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteDocumentAsync method for documentId = " + documentId);
                var result = await _fileService.DeleteFileAsync(documentId, _configure, token).ConfigureAwait(false);
                if (result)
                {
                    _logger.Information("Completed - DeleteDocumentAsync method for documentId = " + documentId);
                    return Ok(new ApiResultResponse(true, "Success"));
                }
                _logger.Information("Error - 400 File Deletion failed");
                return new BadRequestObjectResult(new ApiResponse(400, "File Deletion failed."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteDocumentAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Get All Document Sectoin wise
        /// </summary>
        /// <param name="enqId"></param>
        /// <param name="process"></param>
        /// <param name="documentTypeEnum"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetAllDocumentList")]
        public async Task<IActionResult> GetDocumentListAsync(int enqId, string process, DocumentTypeEnum documentTypeEnum, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetDocumentListAsync method for enqId = " + enqId + "process"+ process + "documentTypeEnum" + documentTypeEnum);
                var result = await _fileService.GetFileListAsync(enqId, process, documentTypeEnum, token).ConfigureAwait(false);
                _logger.Information("Completed - GetDocumentListAsync method for enqId = " + enqId + "process" + process + "documentTypeEnum" + documentTypeEnum);
                return Ok(new ApiResultResponse(result, "Success"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetDocumentListAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

    }
}
