using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.UI.Services.IServices.Customer.LoanAccountingPromoter.LoanRelatedReceipt;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.LoanAccountingPromoter.LoanRelatedReceiptProm
{
    [Area(Constants.Customer)]
    public class LoanReceiptController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly ILoanRelatedReceiptPromService _loanRelatedReceiptService;
        private readonly ILoanRelatedReceiptService _loanRelatedService;
       // private const string resultViewPath = "~/Areas/Customer/Views/LoanRelatedReceiptProm/LoanRelatedReceipts/";
       // private const string viewPath = "../../Areas/Customer/Views/LoanRelatedReceiptProm/LoanRelatedReceipts/";
       // private const string resultViewPathP = "~/Areas/Customer/Views/LoanRelatedReceiptProm/LoanRelatedPayments/";

        public LoanReceiptController(ILoanRelatedReceiptService loanRelatedservice,ILogger logger, SessionManager sessionManager, ILoanRelatedReceiptPromService loanRelatedReceiptService)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _loanRelatedReceiptService = loanRelatedReceiptService;
            _loanRelatedService = loanRelatedservice;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewRecord(string id= "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllReceiptPaymentList = _sessionManager.GetAllGenerateReceiptPaymentList();
                TblLaReceiptPaymentDetDTO ReceiptPaymentList = AllReceiptPaymentList.FirstOrDefault(x => x.UniqueID == id);

                var allTransactionTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableTransactionTypeList));
                ViewBag.AllTransactionTypes = allTransactionTypes;

                var allmodepayment = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableModeofPaymentList));
                ViewBag.transactionmode = allmodepayment;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.promoterloanreceiptresultViewPath + Constants.ViewRecord, ReceiptPaymentList);
            }
            catch(System.Exception  ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult CreatePayment(string[] ReferenceNumber, int LoanSub, string UnitName)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + ReferenceNumber);

                List<TblLaReceiptPaymentDetDTO> allSavedReceiptList = new();
                List<TblLaReceiptPaymentDetDTO> reciptexist = new();

                allSavedReceiptList = _sessionManager.GetAllGenerateReceiptPaymentList();
                ViewBag.AccountNumber = allSavedReceiptList.FirstOrDefault().TblLaPaymentDet.LoanNo;
                ViewBag.LoanSub = LoanSub;
                ViewBag.UnitName = UnitName;
                foreach (var item in ReferenceNumber)
                {
                    var reciptexistList = allSavedReceiptList.Where(x => x.TblLaReceiptDet.ReceiptRefNo == item).ToList();
                    reciptexist.AddRange(reciptexistList);
                }
                ViewBag.referencenumber = ReferenceNumber;
                ViewBag.SavedReceipt = reciptexist;
                ViewBag.ReceiptSelected = reciptexist.Count();
                ViewBag.AccountNumber = allSavedReceiptList.FirstOrDefault().TblLaPaymentDet.LoanNo; ;

                _logger.Information(CommonLogHelpers.CreateCompleted + ReferenceNumber);
                return View(Constants.promoterloanreceiptresultViewPath + Constants.CreatePayment, reciptexist);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveCreatePayment(string[] renum, long AccountNumber, int LoanSub, string UnitName, decimal PayAmount)
        {

            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + renum);

                List<TblLaReceiptPaymentDetDTO> activeReceiptPaymentList = new();
                List<TblLaReceiptPaymentDetDTO> reciptexist = new();
                TblLaPaymentDetDTO tblLaPaymentDet = new();
                List<int> RecieptId = new List<int>();
                List<int> paymentRecieptID = new List<int>();

                var ReceiptPaymentList = await _loanRelatedService.GetAllReceiptPaymentList(AccountNumber);
                var accountNumber = ReceiptPaymentList.FirstOrDefault().TblLaPaymentDet.LoanNo;
                List<string> refNumber = new List<string>();

                foreach (string arrItem in renum)
                {
                    refNumber.Add(arrItem);
                }

                foreach (var item in refNumber)
                {
                    var reciptexistList = ReceiptPaymentList.Where(x => x.TblLaReceiptDet.ReceiptRefNo == item).ToList();

                    reciptexistList.FirstOrDefault().TblLaReceiptDet.ReceiptStatus = Constants.PaymentInitiated;
                 
                    reciptexist.AddRange(reciptexistList);
                }

                // Payment Refrence Number
                var allPaymentRefNumber = await _loanRelatedService.GetAllPaymentList();

                int maxnumber = 0;
                var CurrentAccountNumber = ReceiptPaymentList.FirstOrDefault().TblLaPaymentDet.LoanNo.ToString();
                var accnumber = CurrentAccountNumber[..2];


                foreach (var i in allPaymentRefNumber)
                {
                    if (allPaymentRefNumber != null)
                    {
                        var PaymentRefNum = i.PaymentRefNo;
                        PaymentRefNum = PaymentRefNum[5..];
                        int MaxPaymentRefNo = Int32.Parse(PaymentRefNum);
                        if (maxnumber < MaxPaymentRefNo)
                        {
                            maxnumber = MaxPaymentRefNo;

                        }
                    }


                    else
                    {
                        maxnumber = 0;
                    }
                }
                var PaymentType = Constants.PYT;
                var payaccnum = accnumber;
                var maxpaynumbner = maxnumber;
                var PaymentrefrenceNumber = new GeneratePaymentRefNo(maxpaynumbner!, PaymentType, payaccnum!, 5);
                PaymentrefrenceNumber.Increment();
                var incrementedPytRefNo = PaymentrefrenceNumber.ToString();
                var recipetId = reciptexist.Select(x => x.ReceiptId).ToList();

                RecieptId.AddRange(recipetId);
                var Paymentid = reciptexist.Select(x => x.ReceiptPaymentId).ToList();
                paymentRecieptID.AddRange(Paymentid);

                tblLaPaymentDet.LoanNo = accountNumber;
                tblLaPaymentDet.PaymentStatus = Constants.PaymentInitiated;
                tblLaPaymentDet.PaymentMode = 2;
                tblLaPaymentDet.ActualAmt = PayAmount;
                tblLaPaymentDet.PaymentRefNo = incrementedPytRefNo;
                tblLaPaymentDet.ReceiptId = RecieptId;
                tblLaPaymentDet.PaymentReceiptId = paymentRecieptID;

                ViewBag.LoanSub = LoanSub;
                ViewBag.UnitName = UnitName;

                _sessionManager.SetGenerateReceiptPaymentList(reciptexist);

                var SubmitPaymentList = await _loanRelatedService.CreatePaymentDetails(tblLaPaymentDet);
                if (SubmitPaymentList == true)
                {
                    ViewBag.AccountNumber = accountNumber;
                    activeReceiptPaymentList = reciptexist.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    return Json(new { isValid = true, data = new { accountNumber = @AccountNumber, loanSub = @LoanSub, unitName = @UnitName }, html = Helper.RenderRazorViewToString(this, Constants.promoterloanreceiptviewPath + Constants.ViewAll, activeReceiptPaymentList) });


                }
                ViewBag.AccountNumber = accountNumber;
                return Json(new { isValid = false });


            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public async Task<IActionResult> ViewPaymentRecord(int Id,int LoanSub,long LoanAccno, string  Unit)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + Id);
                List<TblLaReceiptPaymentDetDTO> allPayments = new List<TblLaReceiptPaymentDetDTO>();

                var AllReceiptPaymentList = await _loanRelatedService.GetAllRecipetsForPayment(Id);
                var allTransactionTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableTransactionTypeList));

                foreach (var item in AllReceiptPaymentList)
                {

                    item.TransactionType = allTransactionTypes.Where(x => x.Id == item.TblLaReceiptDet.TransTypeId).Select(x=>x.CodeValue).FirstOrDefault();

                    allPayments.Add(item);
                }
                ViewBag.LoanSub = LoanSub;
                ViewBag.LoanAccountNumber=LoanAccno;
                ViewBag.Unit=Unit;
                ViewBag.AccountNumber = LoanAccno;

                var allPaymentRefNumber = await _loanRelatedService.GetAllPaymentList();
                ViewBag.PaymentRef = allPaymentRefNumber.Where(x => x.Id == Id).Select(x => x.PaymentRefNo).FirstOrDefault();
               


                _logger.Information(CommonLogHelpers.ViewRecordCompleted + Id);

                return View(Constants.promoterresultViewPathP + Constants.ViewPaymentRecord, allPayments.ToList());
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public async Task<IActionResult> PayNow(int Id, long LoanAccno)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + Id);

               var allPaymentRefNumber = await _loanRelatedService.GetAllPaymentList();
              var PaymentDetails = allPaymentRefNumber.Where(x => x.Id == Id).FirstOrDefault();
                ViewBag.LoanAccountNumber = LoanAccno;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + Id);

                return View(Constants.promoterresultViewPathP + Constants.PayNow, PaymentDetails);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        public async Task<IActionResult> AddPayNow(int Id, long LoanAccno)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + Id);

                var allPaymentRefNumber = await _loanRelatedService.GetAllPaymentList();
                var PaymentDetails = allPaymentRefNumber.Where(x => x.Id == Id).FirstOrDefault();
                ViewBag.LoanAccountNumber = LoanAccno;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + Id);

                return View(Constants.promoterresultViewPathP + Constants.AddPayNow, PaymentDetails);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
