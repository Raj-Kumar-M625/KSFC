using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using KAR.KSFC.Components.Common.Logging.Client;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.InspectionOfUnit.ImportMachinery
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class ImportMachineryController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
      

        public ImportMachineryController(ILogger logger, SessionManager sessionManager)
        {

            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create(long accountNumber, long InspectionId, byte OffcCd, string LoanSub, bool firstinspection)
        {

            try
            {
                var importMachineryList = _sessionManager.GetAllImportMachineryList();

                if (importMachineryList.Count != 0)
                {
                    ViewBag.ItemNumber = importMachineryList.Select(x => x.DimcItmNo).ToList();
                    ViewBag.Eligibility = importMachineryList.Last().Dimcsec;
                }
                else
                {
                    ViewBag.Eligibility = 0;
                }

                var procureList = _sessionManager.GetAllProcureList();
                var currencyList = _sessionManager.GetAllCurrencyList();
                ViewBag.ProcureList = procureList;
                ViewBag.CurrencyList = currencyList;
                ViewBag.AccountNumber = accountNumber;
                ViewBag.InspectionId = InspectionId;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.firstimportMachine = firstinspection;

                var ImportMachineryInspectionDetails = _sessionManager.GetAllImportMachineryList();
                var inspdetails = _sessionManager.GetAllInspectionDetail();
                var forfirstinspection = inspdetails.FirstOrDefault().DinNo;

                if (ImportMachineryInspectionDetails.Where(x => x.DimcIno == InspectionId).Count() > 0 || forfirstinspection == InspectionId)
                {

                    ViewBag.firstimportMachine = false;
                }

                var previousinspection = inspdetails
                        .OrderByDescending(x => x.DinNo)
                        .Skip(1)
                         .FirstOrDefault();
                if (previousinspection != null && previousinspection.DinNo == InspectionId)
                {
                    ViewBag.firstimportMachine = false;
                }

                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.ImportresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }
        [HttpPost]
        public IActionResult Create(IdmDchgImportMachineryDTO model)
        {
            Random rand = new Random(); // <-- Make this static somewhere
            const int maxValue = 9999;
            var number = Convert.ToInt64(rand.Next(maxValue + 1).ToString("D4"));
            try
            {
                //if (ModelState.IsValid)
                //{
                    List<IdmDchgImportMachineryDTO> importMachineryList = new();
                    if (_sessionManager.GetAllImportMachineryList() != null)
                        importMachineryList = _sessionManager.GetAllImportMachineryList();
                    IdmDchgImportMachineryDTO list = new IdmDchgImportMachineryDTO();
                    list.LoanAcc = model.LoanAcc;
                    list.OffcCd = model.OffcCd;
                    list.LoanSub = model.LoanSub;
                    list.DimcIno = model.DimcIno;
                    list.Action = (int)Constant.Create;
                    list.DimcItmNo=number;
                    list.DimcDets = model.DimcDets;
                    list.DimcSup = model.DimcSup;
                    list.DimcSupAdr1=model.DimcSupAdr1;
                    list.DimcQty=model.DimcQty;
                    list.DimcEntry=model.DimcEntry;
                    list.DimcEntryI = model.DimcEntryI;
                    list.DimcCrncy = model.DimcCrncy;
                    list.DimcExrd = model.DimcExrd;
                    list.DimcCif = model.DimcCif;
                    list.DimcCduty = model.DimcCduty;
                    list.DimcTotCost = model.DimcTotCost;
                    list.DimcActualCost= model.DimcActualCost;  
                    list.DimcsecRel=model.DimcsecRel;
                    list.Dimcsec = model.Dimcsec;
                    list.DimcsecElig=model.DimcsecElig;
                    list.DimcCpct = model.DimcCpct;
                    list.DimcCamt = model.DimcCamt;
                    list.DimcAqrdStat=model.DimcAqrdStat;
                    list.DimcStat=model.DimcStat;
                    list.DimcStatChgDate = model.DimcStatChgDate;
                    list.DimcBankAdivce=model.DimcBankAdivce;
                    list.DimcBankAdvDate = model.DimcBankAdvDate;
                    list.DimcStatDesc = model.DimcStatDesc;
                    list.DimcDelry=model.DimcDelry;
                    list.DimcMacDocuments=model.DimcMacDocuments;
                    list.DimcCurBasicAmt=model.DimcCurBasicAmt;
                    list.DimcBasicAmt=model.DimcBasicAmt;
                    list.IrPlmcSecAmt = model.IrPlmcSecAmt;
                    list.IrPlmcAamt = model.IrPlmcAamt;
                    list.IrPlmcTotalRelease = model.IrPlmcTotalRelease;
                    list.IrPlmcRelseStat = Convert.ToInt32(model.IrPlmcRelseStat);
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.DimcGST = model.DimcGST;
                    list.DimcOthExp = model.DimcOthExp;
                    list.DimcTotalCostMem = model.DimcTotalCostMem;
                    importMachineryList.Add(list);
                    _sessionManager.SetImportMachineryList(importMachineryList);
                    List<IdmDchgImportMachineryDTO> activeList = importMachineryList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.InspectionId = list.DimcIno;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.firstimportMachine = true;
                    ViewBag.InspectionId = model.DimcIno;
                    var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                    ViewBag.inspectiondetails = inspectiondetailslist;
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.ImportviewPath + Constants.ViewAll, importMachineryList) });
                //}
                //ViewBag.AccountNumber = model.LoanAcc;
                //ViewBag.InspectionId = model.DimcIno;
                //ViewBag.LoanSub = model.LoanSub;
                //ViewBag.OffcCd = model.OffcCd;
                //return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.ImportviewPath + Constants.Create, model) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
               // var registeredSate = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionRegisteredState));
                //var machinarylist = _sessionManager.GetAllMachinaryStatusList();
                var allimportMachineryList = _sessionManager.GetAllImportMachineryList();
                var currencyList = _sessionManager.GetAllCurrencyList();
                var procureList = _sessionManager.GetAllProcureList();
                ViewBag.CurrencyList = currencyList;
                ViewBag.ProcureList = procureList;
                IdmDchgImportMachineryDTO importMachineryList = allimportMachineryList.FirstOrDefault(x => x.UniqueId == id);
                if(importMachineryList != null)
                {
                    importMachineryList.DimcsecElig = allimportMachineryList.Last().Dimcsec;

                }
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.ImportresultViewPath + Constants.ViewRecord, importMachineryList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult Edit(long InspectionId,int row, string id = "")
        {
            try
            {
              //  var registeredSate = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionRegisteredState));
                var procureList = _sessionManager.GetAllProcureList();
                ViewBag.ProcureList = procureList;
                //var currencyList = _sessionManager.GetAllCurrencyList();
                //ViewBag.CurrencyList = currencyList;
                var allImportMachineryList = _sessionManager.GetAllImportMachineryList();
             
                var currencyList = _sessionManager.GetAllCurrencyList();
               
                ViewBag.CurrencyList = currencyList;
                
                IdmDchgImportMachineryDTO importMachinery = allImportMachineryList.FirstOrDefault(x => x.UniqueId == id);
                if (importMachinery!= null)
                {
                    importMachinery.DimcsecElig = allImportMachineryList.Last().Dimcsec;

                    var AccountNumber = importMachinery.LoanAcc;
                    //var InspectionId = importMachinery.DimcIno;
                    ViewBag.InspectionId = InspectionId;
                    ViewBag.AccountNumber = AccountNumber;
                    ViewBag.LoanSub = importMachinery.LoanSub;
                    ViewBag.OffcCd = importMachinery.OffcCd;
                    ViewBag.row = row;
                }

                return View(Constants.ImportresultViewPath + Constants.editCs, importMachinery);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDchgImportMachineryDTO model)
        {
            try
            {

                List<IdmDchgImportMachineryDTO> importMachineryList = _sessionManager.GetAllImportMachineryList();
                IdmDchgImportMachineryDTO importMachineryExist = importMachineryList.Find(x => x.UniqueId == id);
                //if (importMachineryExist != null)
                //{
                    importMachineryList.Remove(importMachineryExist);
                    var list = importMachineryExist;
                    list.LoanAcc = model.LoanAcc;
                    list.LoanSub = model.LoanSub;
                    list.OffcCd= model.OffcCd;
                    list.DimcIno = model.DimcIno;
                    list.DimcItmNo = model.DimcItmNo;
                    list.DimcDets = model.DimcDets;
                    list.DimcSup = model.DimcSup;
                    list.DimcSupAdr1 = model.DimcSupAdr1;
                    list.DimcQty = model.DimcQty;
                    list.DimcEntry = model.DimcEntry;
                    list.DimcEntryI = model.DimcEntryI;
                    list.DimcCrncy = model.DimcCrncy;
                    list.DimcExrd = model.DimcExrd;
                    list.DimcCif = model.DimcCif;
                    list.DimcCduty = model.DimcCduty;
                    list.DimcTotCost = model.DimcTotCost;
                    list.DimcActualCost = model.DimcActualCost;
                    list.DimcsecRel = model.DimcsecRel;
                    list.Dimcsec = model.Dimcsec;
                    list.DimcsecElig = model.DimcsecElig;
                    list.DimcCpct = model.DimcCpct;
                    list.DimcCamt = model.DimcCamt;
                    list.DimcStatDesc = model.DimcStatDesc;
                    list.DimcAqrdStat = model.DimcAqrdStat;
                    list.DimcStat = model.DimcStat;
                    list.DimcStatChgDate = model.DimcStatChgDate;
                    list.DimcBankAdivce = model.DimcBankAdivce;
                    list.DimcBankAdvDate = model.DimcBankAdvDate;
                    list.DimcDelry = model.DimcDelry;
                    list.DimcBasicAmt = model.DimcBasicAmt;
                    list.DimcCurBasicAmt = model.DimcCurBasicAmt;
                    list.DimcMacDocuments = model.DimcMacDocuments;
                    list.IrPlmcId = model.IrPlmcId;
                    list.IrPlmcSecAmt = model.IrPlmcSecAmt;
                    list.IrPlmcAamt = model.IrPlmcAamt;
                    list.IrPlmcTotalRelease = model.IrPlmcTotalRelease;
                    list.IrPlmcRelseStat = Convert.ToInt32(model.IrPlmcRelseStat);
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.DimcGST = model.DimcGST;
                    list.DimcOthExp = model.DimcOthExp;
                    list.DimcTotalCostMem = model.DimcTotalCostMem;
                    if (importMachineryExist.DimcRowId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    importMachineryList.Add(list);
                    ViewBag.AccountNumber = model.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InspectionId = model.DimcIno;
                    ViewBag.firstimportMachine = true;

                    var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                    ViewBag.inspectiondetails = inspectiondetailslist;
                    _sessionManager.SetImportMachineryList(importMachineryList);

                    List<IdmDchgImportMachineryDTO> activeList = importMachineryList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.ImportviewPath + Constants.ViewAll, importMachineryList) });
                //}
                //ViewBag.AccountNumber = model.LoanAcc;
                //ViewBag.InspectionId = model.DimcIno;
                //ViewBag.LoanSub = model.LoanSub;
                //ViewBag.OffcCd = model.OffcCd;
                //return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.ImportviewPath + Constants.Edit, model) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        public IActionResult Delete(long InspectionId,string id)
        {
            try
            {
                IEnumerable<IdmDchgImportMachineryDTO> activeImportMachinery = new List<IdmDchgImportMachineryDTO>();

                var importMachineryInspectionList = JsonConvert.DeserializeObject<List<IdmDchgImportMachineryDTO>>(HttpContext.Session.GetString(Constants.SessionAllImportMachineryList));
                var itemToRemove = importMachineryInspectionList.Find(r => r.UniqueId == id);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                importMachineryInspectionList.Add(itemToRemove);

                _sessionManager.SetImportMachineryList(importMachineryInspectionList);
                if(importMachineryInspectionList.Count !=0)
                {
                    activeImportMachinery=importMachineryInspectionList.Where(x=>x.IsActive==true && x.IsDeleted==false).ToList();
                }

                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.InspectionId=itemToRemove.DimcIno;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InspectionId = InspectionId;
                ViewBag.firstimportMachine = true;
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.ImportviewPath + Constants.ViewAll, activeImportMachinery) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}
