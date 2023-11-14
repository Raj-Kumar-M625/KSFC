using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Extensions;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.SecurityAndDocuments
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.CE)]
    public class DocumentsController : Controller
    {
        private const string resultViewPath = "~/Areas/Customer/Views/Enquiry/SecurityAndDocuments/Documents/";
        private const string viewPath = "../../Areas/Customer/Views/Enquiry/SecurityAndDocuments/Documents/";
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEnquirySubmissionService _enquirySubmissionService;
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;
        public DocumentsController(IWebHostEnvironment webHostEnvironment, IEnquirySubmissionService enquirySubmissionService, SessionManager sessionManager, ILogger logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _enquirySubmissionService = enquirySubmissionService;
            _sessionManager = sessionManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<JsonResult> ViewUploadFile(string fileId)
        {
            try
            {
                _logger.Information("Started - ViewUploadFile method for fileId = " + fileId);
                if (!string.IsNullOrEmpty(fileId))
                {
                    var res = await _enquirySubmissionService.ViewFile(fileId).ConfigureAwait(false);
                    if (res != null) { 
                        _logger.Information("Completed - ViewUploadFile method for fileId = " + fileId);
                    return Json(new { result = Convert.ToBase64String(res) });
                    }
                }
                _logger.Information("Completed - ViewUploadFile method for fileId = " + fileId);
                return null;
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewUploadFile  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUploadFile(int fileId, DocumentTypeEnum documentType)
        {
            try
            {
                _logger.Information("Started - DeleteUploadFile method for fileId = " + fileId + "DocumentTypeEnum" + documentType);
                if (fileId > 0)
                {
                    var res = await _enquirySubmissionService.DeleteFile(fileId).ConfigureAwait(false);
                    if (res)
                    {
                        var documentlist = _sessionManager.GetDocuments();
                        if (documentlist != null)
                        {
                            var file = documentlist.Where(x => x.DocSection == documentType.ToString() && x.Id == fileId).FirstOrDefault();
                            documentlist.Remove(file);
                            _sessionManager.SetDocuments(documentlist);
                        }
                        _logger.Information("Started - DeleteUploadFile method for fileId = " + fileId + "DocumentTypeEnum" + documentType);
                        return Json(new { status = "success" });
                    }
                }
                _logger.Information("Completed - status = Fail");
                return Json(new { status = "fail" });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteUploadFile  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<IActionResult> UploadFle([FromForm] IFormFile file, [FromForm] DocumentTypeEnum documentType)
        {
            try
            {
                _logger.Information("Started - UploadFle method for IFormFile = " + file + "DocumentTypeEnum = "+ documentType);
                if (file != null && file.Length > 0 && file.ContentType == "application/pdf")
                {
                    var data = new FileUploadDTO
                    {
                        DocumentType = documentType,
                        EnquiryId = Convert.ToInt32(_sessionManager.GetNewEnqTempId()),
                        FileType = FileTypesEnum.Pdf,
                        Bytes = file.FileToByteArray()
                    };
                    var res = await _enquirySubmissionService.FileUpload(data).ConfigureAwait(false);
                    if (res.Item1)
                    {
                        _logger.Information("Completed - UploadFle method for IFormFile = " + file + "DocumentTypeEnum = " + documentType);
                        return Json(new { data = new EnqDocumentDTO { Id = res.Item3, UniqueId = res.Item4 }, status = "Success" });
                    }
                    else
                    {
                        _logger.Information("Completed - status = Fail");
                        return Json(new { data = res.Item2, status = "Fail" });
                    }
                }
                _logger.Information("Completed - Please upload only PDF file. status = Fail");
                return Json(new { data = "Please upload only PDF file.", status = "Fail" });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UploadFle  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Uploads promoter photo
        /// </summary>
        /// <param name="promoterPic"></param>
        /// <returns></returns>
        public IActionResult UploadPhoto(IFormFile promoterPic)
        {
            try
            {
                _logger.Information("Started - UploadPhoto method for IFormFile = " + promoterPic);
                if (promoterPic != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\wwwroot\\media");
                    string filePath = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot", "media", promoterPic.FileName);//Path.Combine(uploadsFolder, MyUploader.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        promoterPic.CopyToAsync(fileStream);
                    }
                    _logger.Information("Started - UploadPhoto method for IFormFile = " + promoterPic);
                    return new ObjectResult(new { status = "success", filename = promoterPic.FileName });
                }
                _logger.Information("Completed- status = fail");
                return new ObjectResult(new { status = "fail" });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UploadPhoto page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
