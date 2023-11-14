using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto.LoanAccounting;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using KAR.KSFC.UI.Security;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.LoanAccounting.LoanRelatedReceipt
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class LoanRelatedReceiptController : Controller
    {
        private readonly ILoanRelatedReceiptService _loanReceiptService;
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;
        private readonly ILoanAccountingService _loanAccountingService;
        private readonly ILoanRelatedReceiptService _loanRelatedReceiptService;
        private readonly ICommonService _commonService;
        private readonly IDataProtector protector;

        public LoanRelatedReceiptController(ILoanRelatedReceiptService loanRelatedReceiptService, ILogger logger, SessionManager sessionManager,
            ICommonService commonService, ILoanRelatedReceiptService loanReceiptService, ILoanAccountingService loanAccountingService, IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _loanReceiptService = loanReceiptService;
            _loanAccountingService = loanAccountingService;
            _logger = logger;
            _sessionManager = sessionManager;
            _commonService = commonService;
            _loanRelatedReceiptService = loanRelatedReceiptService;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.AdminRouteValue);

        }
        public IActionResult Index()
        {
            var loans = _sessionManager.GetAllLoanNumber()
                .Select(e =>
                {
                    e.EncryptedLoanAcc = protector.Protect(e.LoanAcc.ToString());
                    e.EncryptedLoanUnit = protector.Protect(e.LoanUnit);
                    e.EncryptedLoanSub = protector.Protect(e.LoanSub.ToString());
                    e.EncryptedInOffc = protector.Protect(e.InOffc.ToString());
                    return e;
                });
            return View(loans);
        }

        public async Task<IActionResult> ViewAccount(string AccountNumber, string LoanSub, string UnitName, int loans, string Module)
        {
            try
            {

                long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
                int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
                string unitname = protector.Unprotect(UnitName);
                var allcodetype = await _loanReceiptService.GetCodeTableList(accountNumber);

                var TransactionType = allcodetype.Where(x => x.CodeType == Constants.TransactionType);
                var ModeOfPayment = allcodetype.Where(x => x.CodeType == Constants.ModeOfPayment);

                var allSavedReceiptPaymentList = await _loanReceiptService.GetAllReceiptPaymentList(accountNumber);
                if (allSavedReceiptPaymentList != null)
                {
                    foreach (var i in allSavedReceiptPaymentList)
                    {
                        var transtype = TransactionType.Where(x => x.Id == i.TblLaReceiptDet.TransTypeId);
                        i.TransactionType = transtype.FirstOrDefault().CodeValue;

                        if (i.TblLaPaymentDet != null)
                        {
                            var modeofpayment = ModeOfPayment.Where(x => x.Id == i.TblLaPaymentDet.PaymentMode);
                            i.ModeType = modeofpayment.FirstOrDefault().CodeValue;
                        }

                        if (i.UniqueID == null)
                        {
                            i.UniqueID = Guid.NewGuid().ToString();
                        }
                    }
                }



                _sessionManager.SetCodeTableModeofPaymentList(ModeOfPayment);
                _sessionManager.SetCodeTableTransactionTypeList(TransactionType);
                _sessionManager.SetReceiptPaymentList(allSavedReceiptPaymentList);

                List<TblLaReceiptPaymentDetDTO> paymentRecipetList = new List<TblLaReceiptPaymentDetDTO>();
                var paymentId = allSavedReceiptPaymentList.Where(x => x.PaymentId != null).Select(x => x.PaymentId).Distinct().ToList();
                foreach (var Id in paymentId)
                {
                    var list = allSavedReceiptPaymentList.Where(x => x.PaymentId == Id).FirstOrDefault();
                    paymentRecipetList.Add(list);

                }

                LoanAccountingDTO loanAccountingDTO = new();
                loanAccountingDTO.ModeOfPayment = ModeOfPayment.ToList();
                loanAccountingDTO.TransactionType = TransactionType.ToList();
                loanAccountingDTO.ReceiptPaymentDetails = allSavedReceiptPaymentList.ToList();
                loanAccountingDTO.PaymentRecipetList = paymentRecipetList;

                HttpContext.Session.SetString(Constants.LoanAccount, accountNumber.ToString());
                HttpContext.Session.SetInt32(Constants.LoanSub, loansub);
                HttpContext.Session.SetString(Constants.UnitName, unitname);


                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.Module = Module;

                return View(loanAccountingDTO);
            }
            catch (Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

    }
}
