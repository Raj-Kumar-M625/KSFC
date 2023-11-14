using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.LoanAccounting.LoanRelatedReceipt.GenerateReceipt
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class GenerateReceiptController : Controller
    {
        private readonly ILoanRelatedReceiptService _loanRelatedReceiptService;
        private readonly ILoanRelatedReceiptService _loanReceiptService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        public GenerateReceiptController(ILoanRelatedReceiptService loanRelatedReceiptService, ILogger logger, SessionManager sessionManager, ILoanRelatedReceiptService loanReceiptService)
        {
            _loanReceiptService = loanReceiptService;
            _loanRelatedReceiptService = loanRelatedReceiptService;
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewRecord(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + unqid);
                var allReceiptPayementList = _sessionManager.GetAllReceiptPaymentList();
                TblLaReceiptPaymentDetDTO ReceiptPayementList = allReceiptPayementList.FirstOrDefault(x => x.UniqueID == unqid);

                var allTransactionTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableTransactionTypeList));
                ViewBag.AllTransactionTypes = allTransactionTypes;
                var allPaymemtTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableModeofPaymentList));
                ViewBag.AllPaymemtTypes = allPaymemtTypes;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + unqid);
                return View(Constants.generatereceiptresultViewPath + Constants.ViewRecord, ReceiptPayementList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult Edit(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + unqid);
                var allReceiptPayementList = _sessionManager.GetAllReceiptPaymentList();
                TblLaReceiptPaymentDetDTO ReceiptPayementList = allReceiptPayementList.FirstOrDefault(x => x.UniqueID == unqid);

                var allTransactionTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableTransactionTypeList));
                ViewBag.AllTransactionTypes = allTransactionTypes;
                var allPaymemtTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableModeofPaymentList));
                ViewBag.AllPaymemtTypes = allPaymemtTypes;

                _logger.Information(CommonLogHelpers.UpdateCompleted + unqid);
                return View(Constants.generatereceiptresultViewPath + Constants.editCs, ReceiptPayementList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        public async Task<IActionResult> Create(long AccountNumber)
        {
            try
            {

                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                ViewBag.AccountNumber = AccountNumber;
                var allTransactionTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableTransactionTypeList));
                ViewBag.AllTransactionTypes = allTransactionTypes;
                var allPaymemtTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableModeofPaymentList));
                ViewBag.AllPaymemtTypes = allPaymemtTypes;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.generatereceiptresultViewPath + Constants.createCS);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TblLaReceiptPaymentDetDTO receiptpayment)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.LoanRelatedReceiptsDto,
                  receiptpayment.TblLaReceiptDet.ReceiptRefNo));
                IEnumerable<TblLaReceiptPaymentDetDTO> activeReceiptPaymentList = new List<TblLaReceiptPaymentDetDTO>();
                var ReceiptPaymentList = _sessionManager.GetAllReceiptPaymentList();
                TblLaReceiptPaymentDetDTO ReceiptPaymentExist = ReceiptPaymentList.FirstOrDefault(x => x.UniqueID == receiptpayment.UniqueID);

                if (ReceiptPaymentExist != null)
                {
                    var ReceiptRefNo = receiptpayment.TblLaReceiptDet.ReceiptRefNo;
                    ReceiptPaymentList.Remove(ReceiptPaymentExist);
                    var list = ReceiptPaymentExist;
                    list.TblLaReceiptDet.ReceiptRefNo = receiptpayment.TblLaReceiptDet.ReceiptRefNo;
                    list.TblLaReceiptDet.TransTypeId = receiptpayment.TblLaReceiptDet.TransTypeId;
                    var TransactionTypes = _sessionManager.GetCodeTableTransactionTypeList();
                    var TransactionType = TransactionTypes.Where(x => x.Id == receiptpayment.TblLaReceiptDet.TransTypeId);
                    list.TransactionType = TransactionType.FirstOrDefault().CodeValue;
                    var ModeofpaymentTypes = _sessionManager.GetCodeTablePaymentTypeList();
                    list.TblLaReceiptDet.DueDatePayment = receiptpayment.TblLaReceiptDet.DueDatePayment;
                    list.TotalAmt = receiptpayment.PaymentAmt;
                    list.ActualAmt = receiptpayment.ActualAmt;
                    list.TblLaReceiptDet.Remarks = receiptpayment.TblLaReceiptDet.Remarks;
                    list.TblLaReceiptDet.DateOfGeneration = receiptpayment.TblLaReceiptDet.DateOfGeneration;
                    list.BalanceAmt = receiptpayment.BalanceAmt;
                    ReceiptPaymentList.Add(list);
                    _sessionManager.SetReceiptPaymentList(ReceiptPaymentList);

                    if (ReceiptPaymentList.Count > 0)
                    {
                        activeReceiptPaymentList = ReceiptPaymentList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    }

                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.LoanRelatedReceiptsDto,
                    receiptpayment.TblLaReceiptDet.ReceiptRefNo));

                    var UpdatedReceiptPaymentList = await _loanRelatedReceiptService.UpdateReceiptPaymentDetails(list);
                    if (UpdatedReceiptPaymentList == true)
                    {
                        ViewBag.AccountNumber = ReceiptPaymentExist.TblLaReceiptDet.LoanNo;
                        return Json(new { isValid = true, data = ReceiptRefNo, html = Helper.RenderRazorViewToString(this, Constants.generatereceiptViewPath + Constants.ViewAll, activeReceiptPaymentList) });
                    }



                }
                ViewBag.AccountNumber = ReceiptPaymentExist.TblLaReceiptDet.LoanNo;
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.LoanRelatedReceiptsDto,
                receiptpayment.TblLaReceiptDet.ReceiptRefNo));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.generatereceiptViewPath + Constants.Edit, receiptpayment) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {

                IEnumerable<TblLaReceiptPaymentDetDTO> activeReceiptPaymentList = new List<TblLaReceiptPaymentDetDTO>();
                var ReceiptPaymentList = JsonConvert.DeserializeObject<List<TblLaReceiptPaymentDetDTO>>(HttpContext.Session.GetString(Constants.SessionAllReceiptPaymentList));
                var ReceiptPayment = ReceiptPaymentList.Find(r => r.UniqueID == Id);
                if(Id == null)
                {
                    return NotFound();
                }
                else
                {
                    var ReceiptNumber = ReceiptPayment.TblLaReceiptDet.ReceiptRefNo;
                    ReceiptPaymentList.Remove(ReceiptPayment);
                    ReceiptPayment.IsActive = false;
                    ReceiptPayment.IsDeleted = true;
                    ReceiptPaymentList.Add(ReceiptPayment);
                    _sessionManager.SetReceiptPaymentList(ReceiptPaymentList);

                    if (ReceiptPaymentList.Count > 0)
                    {
                        activeReceiptPaymentList = ReceiptPaymentList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    }

                    var DeleteReceiptPaymentList = await _loanRelatedReceiptService.DeleteReceiptPaymentDetails(ReceiptPayment);
                    ViewBag.AccountNumber = ReceiptPayment.TblLaReceiptDet.LoanNo;
                    if (DeleteReceiptPaymentList == true)
                    {
                        return Json(new { isValid = true, acc = ReceiptNumber, html = Helper.RenderRazorViewToString(this, Constants.generatereceiptViewPath + Constants.ViewAll, activeReceiptPaymentList) });
                    }

                    _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted + LogAttribute.LoanRelatedReceiptsDto, ReceiptNumber));
                    return Json(new { isValid = false });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Submit(TblLaReceiptPaymentDetDTO dto)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.SubmitStarted + LogAttribute.LoanRelatedReceiptsDto, dto.TblLaReceiptDet.ReceiptRefNo));
                IEnumerable<TblLaReceiptPaymentDetDTO> activeReceiptPaymentList = new List<TblLaReceiptPaymentDetDTO>();
                var ReceiptPaymentList = JsonConvert.DeserializeObject<List<TblLaReceiptPaymentDetDTO>>(HttpContext.Session.GetString(Constants.SessionAllReceiptPaymentList));
                var ReceiptPayment = ReceiptPaymentList.Find(r => r.TblLaReceiptDet.ReceiptRefNo == dto.TblLaReceiptDet.ReceiptRefNo);
                var ReceiptNumber = ReceiptPayment.TblLaReceiptDet.LoanNo;
                ReceiptPaymentList.Remove(ReceiptPayment);
                ReceiptPayment.TblLaReceiptDet.ReceiptStatus = Constants.NotPaid;
                ReceiptPayment.TblLaReceiptDet.ReceiptStatus = Constants.NotPaid;
                ReceiptPaymentList.Add(ReceiptPayment);
                _sessionManager.SetReceiptPaymentList(ReceiptPaymentList);

                var SubmitReceiptPaymentList = await _loanRelatedReceiptService.ApproveReceiptPaymentDetails(ReceiptPayment);
                if (SubmitReceiptPaymentList == true)
                {
                    activeReceiptPaymentList = ReceiptPaymentList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    var AccountNumber = HttpContext.Session.GetString(Constants.LoanAccount);
                    var accont = Convert.ToInt64(AccountNumber);
                    var LoanSub = HttpContext.Session.GetInt32(Constants.LoanSub.ToString());
                    var UnitName = HttpContext.Session.GetString(Constants.UnitName);
                    ViewBag.AccountNumber = AccountNumber;
                    return Json(new { isValid = true, data = new { refnum = ReceiptNumber, accountNumber = accont, loanSub = @LoanSub, unitName = @UnitName }, html = Helper.RenderRazorViewToString(this, Constants.generatereceiptViewPath + Constants.ViewAll, activeReceiptPaymentList) });
                }

                _logger.Information(string.Format(CommonLogHelpers.SubmitCompleted + LogAttribute.LoanRelatedReceiptsDto, dto.TblLaReceiptDet.ReceiptRefNo));
                ViewBag.AccountNumber = ReceiptPayment.TblLaReceiptDet.LoanNo;
                return Json(new { isValid = false });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblLaReceiptPaymentDetDTO receiptpayment)
        {
            try
            {
                IEnumerable<TblLaReceiptPaymentDetDTO> activeReceiptPaymentList = new List<TblLaReceiptPaymentDetDTO>();
                var ReceiptPaymentList = _sessionManager.GetAllReceiptPaymentList();
                var allReceiptRefNum = await _loanReceiptService.GetAllReceiptList();

                var TransactionTypes = _sessionManager.GetCodeTableTransactionTypeList();
                var Transactiontype = TransactionTypes.Where(x => x.Id == receiptpayment.TblLaReceiptDet.TransTypeId);
                var TransType = Transactiontype.FirstOrDefault().CodeValue;

                int maxnumber = 0;
                var AccountNumber1 = receiptpayment.TblLaReceiptDet.LoanNo.ToString();
                var accnum = AccountNumber1[..3];

                if (allReceiptRefNum != null)
                {
                    foreach (var i in allReceiptRefNum)
                    {
                        var ReceiptRefNo = i.ReceiptRefNo;
                        ReceiptRefNo = ReceiptRefNo[5..];
                        int MaxReceiptRefNo = Int32.Parse(ReceiptRefNo);
                        if (maxnumber < MaxReceiptRefNo)
                        {
                            maxnumber = MaxReceiptRefNo;

                        }
                    }
                }
                else
                {
                    maxnumber = 0;
                }

                var TransactionType1 = "";
                var maxnumber1 = maxnumber;
                var accnumber = accnum;
                var TransactionType = TransType;
                switch (TransactionType)
                {
                    case Constants.LAFDApplicantfeedeposit:
                        {
                            TransactionType1 = Constants.LA;
                        }
                        break;
                    case Constants.RegistrationCharges:
                        {
                            TransactionType1 = Constants.RC;
                        }
                        break;
                    case Constants.PenaltyCharges:
                        {
                            TransactionType1 = Constants.PC;
                        }
                        break;
                    case Constants.Recoveryofexcesspaymentfromsuppliers:
                        {
                            TransactionType1 = Constants.RE;
                        }
                        break;
                    case Constants.CersaiCharges:
                        {
                            TransactionType1 = Constants.CC;
                        }
                        break;
                    case Constants.InsuranceCharges:
                        {
                            TransactionType1 = Constants.IC;
                        }
                        break;
                    default:
                        break;
                }
                var Transtype1 = TransactionType1;
                var number = new GenerateReceiptRefNo(maxnumber1!, accnumber!, Transtype1!, 5);
                number.Increment();
                var incrementedRecepitNumber = number.ToString();
                receiptpayment.TblLaReceiptDet.ReceiptRefNo = incrementedRecepitNumber;
                receiptpayment.TransactionType = TransType;

                if (ModelState.IsValid)
                {
                    TblLaReceiptPaymentDetDTO list = new TblLaReceiptPaymentDetDTO();
                    list.UniqueID = Guid.NewGuid().ToString();
                    list.TblLaReceiptDet = receiptpayment.TblLaReceiptDet;
                    list.TransactionType = receiptpayment.TransactionType;
                    var ModeofpaymentTypes = _sessionManager.GetCodeTablePaymentTypeList();
                    list.TblLaReceiptDet.DueDatePayment = receiptpayment.TblLaReceiptDet.DueDatePayment;
                    list.TotalAmt = receiptpayment.PaymentAmt;
                    list.ActualAmt = receiptpayment.ActualAmt;
                    list.BalanceAmt = receiptpayment.BalanceAmt;
                    list.PaymentAmt = receiptpayment.PaymentAmt;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    ReceiptPaymentList.Add(list);
                    _sessionManager.SetReceiptPaymentList(ReceiptPaymentList);
                    list.TblLaReceiptDet.ReceiptRefNo = receiptpayment.TblLaReceiptDet.ReceiptRefNo;
                    var ReceiptRefNo = receiptpayment.TblLaReceiptDet.ReceiptRefNo;

                    _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + LogAttribute.LoanRelatedReceiptsDto,
                         receiptpayment.TblLaReceiptDet.ReceiptRefNo));

                    var CreateReceiptPaymentList = await _loanRelatedReceiptService.CreateReceiptPaymentDetails(list);
                    if (CreateReceiptPaymentList == true)
                    {
                        activeReceiptPaymentList = ReceiptPaymentList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    }

                    var AccountNumber = HttpContext.Session.GetString(Constants.LoanAccount);
                    var accont = Convert.ToInt64(AccountNumber);
                    var LoanSub = HttpContext.Session.GetInt32(Constants.LoanSub.ToString());
                    var UnitName = HttpContext.Session.GetString(Constants.UnitName);
                    ViewBag.AccountNumber = AccountNumber;
                    return Json(new { isValid = true, data = new { refnum = incrementedRecepitNumber, accountNumber = accont, loanSub = @LoanSub, unitName = @UnitName }, html = Helper.RenderRazorViewToString(this, Constants.generatereceiptViewPath + Constants.ViewAll, activeReceiptPaymentList) });

                }

                var AccountNo = HttpContext.Session.GetString(Constants.LoanAccount);
                var account = Convert.ToInt64(AccountNo);
                ViewBag.AccountNumber = account;

                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.LoanRelatedReceiptsDto,
                    receiptpayment.TblLaReceiptDet.ReceiptRefNo));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.generatereceiptViewPath + Constants.Create, receiptpayment) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }

}
