using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Utilities.Extensions;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Utility;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.SecurityAndDocuments
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class DocumentsController : Controller
    {
        private const string resultViewPath = "~/Areas/Admin/Views/Enquiry/SecurityAndDocuments/Documents/";
        private const string viewPath = "../../Areas/Admin/Views/Enquiry/SecurityAndDocuments/Documents/";
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
                _logger.Information("Started - ViewUploadFile with fileId " + fileId);

                if (!string.IsNullOrEmpty(fileId))
                {
                    var res = await _enquirySubmissionService.ViewFile(fileId).ConfigureAwait(false);
                    if (res != null)
                    {
                        _logger.Information("Started - ViewUploadFile with fileId " + fileId);
                        return Json(new { result = Convert.ToBase64String(res) });
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ViewUploadFile page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return null;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUploadFile(int fileId, DocumentTypeEnum documentType)
        {
            try
            {
                _logger.Information("Started - DeleteUploadFile with fileId " + fileId + "documentType " + documentType);

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
                        _logger.Information("Completed - DeleteUploadFile with fileId " + fileId + "documentType " + documentType);

                        return Json(new { status = "success" });
                    }
                }
                return Json(new { status = "fail" });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! DeleteUploadFile page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return Json(new { status = "fail" });
            }
        }

        public async Task<IActionResult> UploadFle([FromForm] IFormFile file, [FromForm] DocumentTypeEnum documentType)
        {
            try
            {
                _logger.Information("Started - UploadFle with documentType " + documentType);

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
                        _logger.Information("Completed - UploadFle with documentType " + documentType);
                        return Json(new { data = new EnqDocumentDTO { Id = res.Item3, UniqueId = res.Item4 }, status = "Success" });
                    }
                    else
                    {
                        return Json(new { data = res.Item1, status = "Fail" });
                    }
                }
                return Json(new { data = "Please upload only PDF file.", status = "Fail" });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! UploadFle page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return Json(new { status = "Fail" });
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
                _logger.Information("Started - UploadPhoto with UploadPhoto ");

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
                    _logger.Information("Completed - UploadPhoto with UploadPhoto ");

                    return new ObjectResult(new { status = "success", filename = promoterPic.FileName });
                }
                return new ObjectResult(new { status = "fail" });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! UploadPhoto page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return new ObjectResult(new { status = "fail" });

            }

        }
    }
}
