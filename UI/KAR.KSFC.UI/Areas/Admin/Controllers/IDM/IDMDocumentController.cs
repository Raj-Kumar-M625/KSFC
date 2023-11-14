using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Extensions;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic.FileIO;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class IdmDocumentController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        public IdmDocumentController(ICommonService commonService, ILogger logger, SessionManager sessionManager)
        {
            _commonService = commonService;
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DisplayDocument(int submoduleId, string submoduleType, string mainModule)
        {
            List<ldDocumentDto> doclist = _sessionManager.GetIDMDocument();
           
                var doc = doclist.Where(x => x.SubModuleId == submoduleId && x.SubModuleType == submoduleType && x.MainModule == mainModule).ToList();
            ViewBag.Documentlist = doc;
                ViewBag.SubModuleId = submoduleId;
                ViewBag.SubModuleType = submoduleType;
                ViewBag.MainModule = mainModule;
            if(mainModule != "ChangeOfUnit" && submoduleType != "DisbursementCondition" && submoduleType != "InspectionDetail" && submoduleType != "PrimarySecurity" && submoduleType != "AuditClearance" && submoduleType != "SecurityCharge" && submoduleType != "CERSAI" )
            {
                return Json(new { html = Helper.RenderRazorViewToString(this, Constants.documentviewPath + Constants.UploadDoc, doc), status = Constants.success });
            }

            if(submoduleType == "DisbursementCondition" || submoduleType == "InspectionDetail" || submoduleType == "PrimarySecurity" || submoduleType == "AuditClearance" || submoduleType == "SecurityCharge" || submoduleType == "CERSAI")
            {
                return Json(new { html = Helper.RenderRazorViewToString(this, Constants.documentviewPath + Constants.UploadDisbursmentDoc, doc), status = Constants.success });
            }
 
            return Json(new { html = Helper.RenderRazorViewToString(this, Constants.documentviewPath + Constants.UploadPhoto, doc), status = Constants.success });
        }

        [HttpPost]
        public async Task<IActionResult> SaveDocumentDetails(int SubModuleId, string SubModuleType, string MainModule, IFormFile file)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedUploadFile + file);

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == SubModuleId && x.SubModuleType == SubModuleType && x.MainModule == MainModule).ToList();
                var fileType = file.ContentType;
                switch (fileType)
                {
                    case Constants.fileType:

                        if (file != null && file.Length > 0 && file.ContentType == Constants.fileType && SubModuleType != "DisbursementCondition" && SubModuleType != "InspectionDetail" && SubModuleType != "PrimarySecurity" && SubModuleType != "AuditClearance" && SubModuleType != "SecurityCharge" && SubModuleType != "CERSAI")
                        {
                            if (doc.Count < 5)
                            {
                                var data = new IdmFileUploadDto()
                                {
                                    Bytes = file.FileToByteArray(),
                                    SubModuleId = SubModuleId,
                                    SubModuleType = SubModuleType,
                                    FileType = file.ContentType,
                                    MainModule = MainModule,
                                };
                                var res = await _commonService.UploadDocument(data).ConfigureAwait(false);

                                var ldFileList = await _commonService.FileList(MainModule);
                                _sessionManager.SetIDMDocument(ldFileList);
                                if (res.Item1)
                                {
                                    _logger.Information(CommonLogHelpers.CompletedUploadFile + file);
                                    var routeValues = new RouteValueDictionary {
                                       { Constants.SubmoduleId, SubModuleId },
                                        { Constants.SubmoduleType, SubModuleType },
                                        { Constants.MainModule, MainModule }
                                       };
                                    return RedirectToAction(Constants.displaydoc, Constants.idmDoc, routeValues);
                                }
                                else
                                {
                                    _logger.Information(Error.FileStatus);
                                    return Json(new { data = res.Item2, status = Constants.StatusFail });
                                }
                            }
                            else
                            {
                                _logger.Information(Error.UploadOnlyPdfError);
                                return Json(new { data = Error.MaximumFiles, status = Constants.StatusFail });
                            }
                        }

                        else if(file != null && file.Length > 0 && file.ContentType == Constants.fileType && SubModuleType == "DisbursementCondition" || SubModuleType == "InspectionDetail" || SubModuleType == "PrimarySecurity" || SubModuleType == "AuditClearance" || SubModuleType == "SecurityCharge" || SubModuleType == "CERSAI")
                        {
                            if (doc.Count < 1)
                            {
                                var data = new IdmFileUploadDto()
                                {
                                    Bytes = file.FileToByteArray(),
                                    SubModuleId = SubModuleId,
                                    SubModuleType = SubModuleType,
                                    FileType = file.ContentType,
                                    MainModule = MainModule,
                                };
                                var res = await _commonService.UploadDocument(data).ConfigureAwait(false);

                                var ldFileList = await _commonService.FileList(MainModule);
                                _sessionManager.SetIDMDocument(ldFileList);
                                if (res.Item1)
                                {
                                    _logger.Information(CommonLogHelpers.CompletedUploadFile + file);
                                    var routeValues = new RouteValueDictionary {
                                       { Constants.SubmoduleId, SubModuleId },
                                        { Constants.SubmoduleType, SubModuleType },
                                        { Constants.MainModule, MainModule }
                                       };
                                    return RedirectToAction(Constants.displaydoc, Constants.idmDoc, routeValues);
                                }
                                else
                                {
                                    _logger.Information(Error.FileStatus);
                                    return Json(new { data = res.Item2, status = Constants.StatusFail });
                                }
                            }
                            else
                            {
                                _logger.Information(Error.UploadOnlyPdfError);
                                return Json(new { data = Error.MaximumFilesDisbursment, status = Constants.StatusFail });
                            }
                        }
                        _logger.Information(Error.UploadOnlyPdfError);
                        return Json(new { data = Error.UploadOnlyPdf, status = Constants.StatusFail });
                        break;

                    case Constants.imageType:
                        if (file != null && file.Length > 0)
                        {
                            if (doc.Count < 1)
                            {
                                var data = new IdmFileUploadDto()
                                {
                                    Bytes = file.FileToByteArray(),
                                    SubModuleId = SubModuleId,
                                    SubModuleType = SubModuleType,
                                    FileType = file.ContentType,
                                    MainModule = MainModule,
                                };
                                var res = await _commonService.UploadDocument(data).ConfigureAwait(false);

                                var ldFileList = await _commonService.FileList(MainModule);
                                _sessionManager.SetIDMDocument(ldFileList);
                                if (res.Item1)
                                {
                                    _logger.Information(CommonLogHelpers.CompletedUploadFile + file);
                                    var routeValues = new RouteValueDictionary {
                          { Constants.SubmoduleId, SubModuleId },
                          { Constants.SubmoduleType, SubModuleType },
                          { Constants.MainModule, MainModule }
                        };
                                    return RedirectToAction(Constants.displaydoc, Constants.idmDoc, routeValues);
                                }
                                else
                                {
                                    _logger.Information(Error.FileStatus);
                                    return Json(new { data = res.Item2, status = Constants.StatusFail });
                                }
                            }
                            else
                            {
                                _logger.Information(Error.UploadOnlyImgError);
                                return Json(new { data = Error.MaximumImages, status = Constants.StatusFail });
                            }
                        }
                        _logger.Information(Error.UploadOnlyImgError);
                        return Json(new { data = Error.UploadOnlyImg, status = Constants.StatusFail });
                }
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UploadImageLoadingError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ViewUploadFile(int SubModuleId, string SubModuleType, string fileId, string mainModule)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedViewUploadFile + fileId);
                if (!string.IsNullOrEmpty(fileId))
                {
                    var res = await _commonService.ViewFile(fileId, mainModule).ConfigureAwait(false);
                    
                    if (res != null)
                    {
                        _logger.Information(CommonLogHelpers.CompletedViewUploadFile + fileId);
                        return Json(new { result = Convert.ToBase64String(res) });
                    }
                }
                _logger.Information(CommonLogHelpers.CompletedViewUploadFile + fileId);
                return null;
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewUploadFileLoadingError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUploadFile(int SubModuleId, string SubModuleType, string mainModule, string fileId)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedDeleteUploadFile + fileId);
                if (fileId.Length > 0)
                {
                    var res = await _commonService.DeleteFile(fileId, mainModule).ConfigureAwait(false);
                    if (res)
                    {
                        var documentlist = _sessionManager.GetIDMDocument();
                        if (documentlist != null)
                        {
                            var file = documentlist.FirstOrDefault(x => x.UniqueId == fileId);
                            documentlist.Remove(file);
                            _sessionManager.SetIDMDocument(documentlist);
                        }
                        _logger.Information(CommonLogHelpers.StartedDeleteUploadFile + fileId);
                        var routeValues = new RouteValueDictionary {
                         { Constants.SubmoduleId, SubModuleId },
                          { Constants.SubmoduleType, SubModuleType },
                          { Constants.MainModule, mainModule }
                        };
                        return RedirectToAction(Constants.displaydoc, Constants.idmDoc, routeValues);

                    }
                }
                _logger.Information(Error.FileStatus);
                return Json(new { status = Constants.StatusFail });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteUploadFileLoadingError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
    }
}
