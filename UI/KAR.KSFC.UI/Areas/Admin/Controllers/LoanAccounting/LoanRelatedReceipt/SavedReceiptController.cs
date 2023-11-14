using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.UI.Services.Admin.LoanAccounting.LoanRelatedReceipt;
using System.Security.Principal;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.LoanAccounting.LoanRelatedReceipt
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class SavedReceiptController : Controller
    {
        private readonly ILoanRelatedReceiptService _loanRelatedReceiptService;
        private readonly ILoanRelatedReceiptService _loanReceiptService;
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;
        private readonly ILoanAccountingService _loanAccountingService;
        private readonly ICommonService _commonService;
  
        public SavedReceiptController(ILoanRelatedReceiptService loanRelatedReceiptService, ILogger logger, SessionManager sessionManager,
            ICommonService commonService, ILoanRelatedReceiptService loanReceiptService)
        {
            _loanReceiptService = loanReceiptService;
            _loanRelatedReceiptService = loanRelatedReceiptService;
            _logger = logger;
            _sessionManager = sessionManager;
            _commonService = commonService;
        }

        public IActionResult Index()
        {
            
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Create(long AccountNumber, int LoanSub, string UnitName)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                var allSavedReceiptList = _sessionManager.GetAllReceiptPaymentList();
                ViewBag.SavedReceipt = allSavedReceiptList;
                ViewBag.AccountNumber = AccountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.UnitName = UnitName;
                _sessionManager.SetReceiptPaymentList(allSavedReceiptList);            
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.SavedReceiptresultViewPath + Constants.createCS, allSavedReceiptList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        public IActionResult ViewCreatedRecord(string uniqueID)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + uniqueID);
                var allSavedReceiptList = _sessionManager.GetAllReceiptPaymentList();

                var allTransactionTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableTransactionTypeList));
                ViewBag.AllTransactionTypes = allTransactionTypes;

                var allPaymemtTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableModeofPaymentList));
                ViewBag.AllPaymemtTypes = allPaymemtTypes;
                TblLaReceiptPaymentDetDTO SavedReceiptList = allSavedReceiptList.FirstOrDefault(x => x.UniqueID == uniqueID);
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + uniqueID);
                return View(Constants.SavedReceiptresultViewPath + Constants.ViewCreatedRecord, SavedReceiptList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult EditCreated(string uniqueID)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + uniqueID);
                var allSavedReceiptList = _sessionManager.GetAllReceiptPaymentList();

                var allTransactionTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableTransactionTypeList));
                ViewBag.AllTransactionTypes = allTransactionTypes;

                var allPaymemtTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableModeofPaymentList));
                ViewBag.AllPaymemtTypes = allPaymemtTypes;


                TblLaReceiptPaymentDetDTO SavedReceiptList = allSavedReceiptList.FirstOrDefault(x => x.UniqueID == uniqueID);

                var AccountNo = HttpContext.Session.GetString(Constants.LoanAccount);
                var account = Convert.ToInt64(AccountNo);
                ViewBag.AccountNumber = account;


                _logger.Information(CommonLogHelpers.ViewRecordCompleted + uniqueID);
                return View(Constants.SavedReceiptresultViewPath + Constants.EditCreated, SavedReceiptList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        // Dev
        public IActionResult ViewRecord(string ReferenceNumber, long AccountNumber, int LoanSub, string UnitName,int Id)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + Id);
                var AllReceiptPaymentList = _sessionManager.GetAllReceiptPaymentList();
                if (AllReceiptPaymentList.Count() != 0)
                {

                    ViewBag.ItemNumber = AllReceiptPaymentList.Select(x => x.ReceiptPaymentId).ToList();
                }

                var allTransactionTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableTransactionTypeList));
                ViewBag.AllTransactionTypes = allTransactionTypes;

                var allmodepayment = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableModeofPaymentList));
                ViewBag.transactionmode = allmodepayment;

                // var ReceiptPaymentList = AllReceiptPaymentList.FirstOrDefault(x => x.UniqueID == id);
                var ReceiptPaymentList = AllReceiptPaymentList.Where(r => AllReceiptPaymentList.Any(y => r.PaymentId== Id)).ToList();
                ViewBag.referencenumber = ReferenceNumber;
                ViewBag.ReceiptPayment = ReceiptPaymentList;
                var AccountNo = HttpContext.Session.GetString("_LoanAccount");
                var account = Convert.ToInt64(AccountNo);
                ViewBag.AccountNumber = account;
                ViewBag.LoanSub = LoanSub;
                ViewBag.UnitName = UnitName;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + Id);
                return View(Constants.SavedReceiptresultViewPath + Constants.ViewRecord, ReceiptPaymentList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreatePayment(string[] ReferenceNumber, int LoanSub, string UnitName)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + ReferenceNumber);

                List<TblLaReceiptPaymentDetDTO> allSavedReceiptList = new();
                List<TblLaReceiptPaymentDetDTO> reciptexist = new();

                allSavedReceiptList = _sessionManager.GetAllReceiptPaymentList();
                ViewBag.AccountNumber = allSavedReceiptList.FirstOrDefault().TblLaReceiptDet.LoanNo;
                ViewBag.LoanSub = LoanSub;
                ViewBag.UnitName = UnitName;

                foreach (var item in ReferenceNumber)
                {
                    var reciptexistList = allSavedReceiptList.Where(x => x.TblLaReceiptDet.ReceiptRefNo == item).ToList();
                    reciptexist.AddRange(reciptexistList);
                }


                var AccountNo = HttpContext.Session.GetString(Constants.LoanAccount);
                var account = Convert.ToInt64(AccountNo);
                ViewBag.AccountNumber = account;
                ViewBag.referencenumber = ReferenceNumber;
                ViewBag.SavedReceipt = reciptexist;
                ViewBag.ReceiptSelected = reciptexist.Count();

                _logger.Information(CommonLogHelpers.CreateCompleted + ReferenceNumber);
                return View(Constants.SavedReceiptresultViewPath + Constants.CreatePayment, reciptexist);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        public IActionResult Edit(string ReferenceNumber ,int Id)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + ReferenceNumber);

                List<TblLaReceiptPaymentDetDTO> AllReceiptPaymentList = new();
                List<TblLaReceiptPaymentDetDTO> reciptexist = new();

                AllReceiptPaymentList = _sessionManager.GetAllReceiptPaymentList();

                var allmodepayment = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableModeofPaymentList));
                ViewBag.transactionmode = allmodepayment;

                var allTransactionTypes = JsonConvert.DeserializeObject<List<CodeTableDTO>>(HttpContext.Session.GetString(Constants.SessionAllCodeTableTransactionTypeList));
                ViewBag.AllTransactionTypes = allTransactionTypes;

                var ReceiptPaymentList = AllReceiptPaymentList.Where(r => AllReceiptPaymentList.Any(y => r.PaymentId == Id)).ToList();
                ViewBag.ReceiptPayment = ReceiptPaymentList;
                
             
                ViewBag.referencenumber = ReferenceNumber;
                ViewBag.SavedReceipt = reciptexist;
                ViewBag.ReceiptSelected = reciptexist.Count();
                var AccountNo = HttpContext.Session.GetString(Constants.LoanAccount);
                var account = Convert.ToInt64(AccountNo);
                ViewBag.AccountNumber = account;

                _logger.Information(CommonLogHelpers.UpdateCompleted + ReferenceNumber);
                return View(Constants.SavedReceiptresultViewPath + Constants.editCs, ReceiptPaymentList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TblLaReceiptPaymentDetDTO receiptpayment)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + receiptpayment);
                long? accountNumber = 0;
                var ReceiptPayementList = _sessionManager.GetAllReceiptPaymentList();
                TblLaReceiptPaymentDetDTO ReceiptPayementExist = ReceiptPayementList.FirstOrDefault(x => x.UniqueID == receiptpayment.UniqueID);

                if (ReceiptPayementExist != null)
                {
                    accountNumber = ReceiptPayementExist.TblLaReceiptDet.LoanNo;
                    ReceiptPayementList.Remove(ReceiptPayementExist);
                    var list = ReceiptPayementExist;
                    list.TblLaReceiptDet.ReceiptRefNo = ReceiptPayementExist.TblLaReceiptDet.ReceiptRefNo;
                    list.TblLaReceiptDet.TransTypeId = ReceiptPayementExist.TblLaReceiptDet.TransTypeId;
                    var TransactionTypes = _sessionManager.GetCodeTableTransactionTypeList();
                    var TransactionType = TransactionTypes.Where(x => x.Id == ReceiptPayementExist.TblLaReceiptDet.TransTypeId);
                    list.TransactionType = TransactionType.FirstOrDefault().CodeValue;
                    list.TblLaReceiptDet.DueDatePayment = ReceiptPayementExist.TblLaReceiptDet.DueDatePayment;
                    list.TblLaReceiptDet.TransTypeId = TransactionType.FirstOrDefault().Id;
                    list.TotalAmt = ReceiptPayementExist.PaymentAmt;
                    list.ActualAmt = ReceiptPayementExist.ActualAmt;
                    list.TblLaReceiptDet.Remarks = ReceiptPayementExist.TblLaReceiptDet.Remarks;
                    list.TblLaReceiptDet.DateOfGeneration = ReceiptPayementExist.TblLaReceiptDet.DateOfGeneration;
                    list.BalanceAmt = ReceiptPayementExist.BalanceAmt;
                    list.TblLaReceiptDet.AmountDue = ReceiptPayementExist.TblLaReceiptDet.AmountDue;
                    list.PaymentAmt = ReceiptPayementExist.PaymentAmt;
                    list.TblLaPaymentDet.IfscCode = receiptpayment.TblLaPaymentDet.IfscCode;
                    list.TblLaPaymentDet.ChequeDate = receiptpayment.TblLaPaymentDet.ChequeDate;
                    list.TblLaPaymentDet.ChequeNo = receiptpayment.TblLaPaymentDet.ChequeNo;
                    list.TblLaPaymentDet.BranchCode = receiptpayment.TblLaPaymentDet.BranchCode;
                    list.TblLaPaymentDet.DateOfChequeRealization = receiptpayment.TblLaPaymentDet.DateOfChequeRealization;

                    ReceiptPayementList.Add(list);
                    _sessionManager.SetReceiptPaymentList(ReceiptPayementList);
                    _logger.Information(CommonLogHelpers.UpdateCompletedPost + ReceiptPayementList);
                   
                    var AccountNo1 = HttpContext.Session.GetString(Constants.LoanAccount);
                    var account1 = Convert.ToInt64(AccountNo1);
                    ViewBag.AccountNumber = account1;
                    var UpdatedReceiptPaymentList = await _loanRelatedReceiptService.UpdateReceiptPaymentDetails(list);
                    if (UpdatedReceiptPaymentList == true)
                        {
                        List<TblLaReceiptPaymentDetDTO> paymentRecipetList = new List<TblLaReceiptPaymentDetDTO>();
                        var paymentId = ReceiptPayementList.Where(x => x.PaymentId != null).Select(x => x.PaymentId).Distinct().ToList();
                        foreach (var Id in paymentId)
                        {
                            var list1 = ReceiptPayementList.Where(x => x.PaymentId == Id && x.IsActive==true && x.IsDeleted==false).FirstOrDefault();
                            paymentRecipetList.Add(list1);

                        }
                        return Json(new { isValid = true, data = accountNumber, html = Helper.RenderRazorViewToString(this, Constants.SavedReceiptviewPath + Constants.ViewAll, paymentRecipetList) });
                    }

                }
               
                _logger.Information(CommonLogHelpers.Failed + receiptpayment);
                var AccountNo = HttpContext.Session.GetString(Constants.LoanAccount);
                var account = Convert.ToInt64(AccountNo);
                ViewBag.AccountNumber = account;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.SavedReceiptviewPath + Constants.Edit, receiptpayment) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
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
                var ReceiptPaymentList = await _loanReceiptService.GetAllReceiptPaymentList(AccountNumber);
                var accountNumber = ReceiptPaymentList.FirstOrDefault().TblLaReceiptDet.LoanNo;
                List<string> refNumber = new List<string>();

                foreach (string arrItem in renum)
                {
                    refNumber.Add(arrItem);
                }


                ViewBag.LoanSub = LoanSub;
                ViewBag.UnitName = UnitName;


                // Payment Refrence Number
                var allPaymentRefNumber = await _loanRelatedReceiptService.GetAllPaymentList();

                int maxnumber = 0;
                var CurrentAccountNumber = ReceiptPaymentList.FirstOrDefault().TblLaReceiptDet.LoanNo.ToString();
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

                //reciptUpdate
                foreach (var item in refNumber)
                {
                    var reciptexistList = ReceiptPaymentList.Where(x => x.TblLaReceiptDet.ReceiptRefNo == item).ToList();

                    reciptexistList.FirstOrDefault().TblLaReceiptDet.ReceiptStatus = Constants.PaymentInitiated;
                    reciptexist.AddRange(reciptexistList);

                }

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

                _sessionManager.SetReceiptPaymentList(reciptexist);


                var SubmitPaymentList = await _loanRelatedReceiptService.CreatePaymentDetails(tblLaPaymentDet);

                if (SubmitPaymentList == true)
                {
                    ViewBag.AccountNumber = AccountNumber;
                    activeReceiptPaymentList = reciptexist.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    
                    return Json(new { isValid = true, data = new { accountNumber = @AccountNumber, loanSub = @LoanSub, unitName = @UnitName }, html = Helper.RenderRazorViewToString(this, Constants.SavedReceiptviewPath + Constants.ViewAll, activeReceiptPaymentList) });


                }
                var AccountNo = HttpContext.Session.GetString(Constants.LoanAccount);
                var account = Convert.ToInt64(AccountNo);
                ViewBag.AccountNumber = account;
             
                return Json(new { isValid = false });


            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approve(TblLaReceiptPaymentDetDTO dto)
        {
            try
            {
                IEnumerable<TblLaReceiptPaymentDetDTO> activeReceiptPaymentList = new List<TblLaReceiptPaymentDetDTO>();
                var ReceiptPaymentList = JsonConvert.DeserializeObject<List<TblLaReceiptPaymentDetDTO>>(HttpContext.Session.GetString(Constants.SessionAllReceiptPaymentList));
                List<TblLaReceiptPaymentDetDTO> paymentRecipetList = new List<TblLaReceiptPaymentDetDTO>();
                var paymentId = ReceiptPaymentList.Where(x => x.PaymentId != null).Select(x => x.PaymentId).Distinct().ToList();
                foreach (var Id in paymentId)
                {
                    var list1 = ReceiptPaymentList.Find(x => x.PaymentId == Id && x.IsActive == true && x.IsDeleted == false);
                    paymentRecipetList.Add(list1);

                }
                var ReceiptPayment = paymentRecipetList.Find(r => r.TblLaPaymentDet.PaymentRefNo == dto.TblLaPaymentDet.PaymentRefNo);
                var ReceiptNumber = ReceiptPayment.TblLaPaymentDet.LoanNo;
                ReceiptPayment.TblLaPaymentDet.PaymentStatus = Constants.Paid;
                ReceiptPayment.TblLaReceiptDet.ReceiptStatus= Constants.Paid;   

                var ApproveReceiptPaymentList = await _loanReceiptService.ApproveReceiptPaymentDetails(ReceiptPayment);
                if (ApproveReceiptPaymentList == true)
                {
                    ViewBag.AccountNumber = ReceiptPayment.TblLaReceiptDet.LoanNo;
                    activeReceiptPaymentList = ReceiptPaymentList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();



                    var AccountNumber = HttpContext.Session.GetString(Constants.LoanAccount);
                    var accont = Convert.ToInt64(AccountNumber);
                    var LoanSub = HttpContext.Session.GetInt32(Constants.LoanSub.ToString());
                    var UnitName = HttpContext.Session.GetString(Constants.UnitName);
                    ViewBag.AccountNumber = AccountNumber;
                    return Json(new { isValid = true, data = new { refnum = ReceiptNumber, accountNumber = accont, loanSub = @LoanSub, unitName = @UnitName }, html = Helper.RenderRazorViewToString(this, Constants.SavedReceiptviewPath + Constants.ViewAll, paymentRecipetList) });
                }
                ViewBag.AccountNumber = ReceiptPayment.TblLaReceiptDet.LoanNo;
                return Json(new { isValid = false });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Reject(TblLaReceiptPaymentDetDTO dto)
        {
            try
            {
                IEnumerable<TblLaReceiptPaymentDetDTO> activeReceiptPaymentList = new List<TblLaReceiptPaymentDetDTO>();
                var ReceiptPaymentList = JsonConvert.DeserializeObject<List<TblLaReceiptPaymentDetDTO>>(HttpContext.Session.GetString(Constants.SessionAllReceiptPaymentList));
                List<TblLaReceiptPaymentDetDTO> paymentRecipetList = new List<TblLaReceiptPaymentDetDTO>();
                var paymentId = ReceiptPaymentList.Where(x => x.PaymentId != null).Select(x => x.PaymentId).Distinct().ToList();
                foreach (var Id in paymentId)
                {
                    var list1 = ReceiptPaymentList.Where(x => x.PaymentId == Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    paymentRecipetList.Add(list1);

                }
                var ReceiptPayment = paymentRecipetList.Find(r => r.TblLaPaymentDet.PaymentRefNo == dto.TblLaPaymentDet.PaymentRefNo);
                var ReceiptNumber = ReceiptPayment.TblLaPaymentDet.LoanNo;
            
                ReceiptPayment.TblLaPaymentDet.PaymentStatus = Constants.NotPaid;
                ReceiptPayment.TblLaReceiptDet.ReceiptStatus = Constants.NotPaid;
                var ApproveReceiptPaymentList = await _loanReceiptService.RejectReceiptPaymentDetails(ReceiptPayment);
                if (ApproveReceiptPaymentList == true)
                {
                    
                    activeReceiptPaymentList = ReceiptPaymentList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    var AccountNumber = HttpContext.Session.GetString(Constants.LoanAccount);
                    var accont = Convert.ToInt64(AccountNumber);
                    var LoanSub = HttpContext.Session.GetInt32(Constants.LoanSub.ToString());
                    var UnitName = HttpContext.Session.GetString(Constants.UnitName);
                    ViewBag.AccountNumber = AccountNumber;
                    return Json(new { isValid = true, data = new { refnum = ReceiptNumber, accountNumber = accont, loanSub = @LoanSub, unitName = @UnitName }, html = Helper.RenderRazorViewToString(this, Constants.SavedReceiptviewPath + Constants.ViewAll, paymentRecipetList) });
                }
                ViewBag.AccountNumber = ReceiptPayment.TblLaReceiptDet.LoanNo;
                return Json(new { isValid = false });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
