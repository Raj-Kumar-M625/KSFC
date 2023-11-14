using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KAR.KSFC.UI.Services.IServices.Customer.LoanAccountingPromoter.LoanRelatedReceipt;
using System;
using System.Linq;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using System.Collections.Generic;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.Components.Common.Dto.LoanAccounting;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.LoanAccounting.LoanRelatedReceipt
{
    [Area(Constants.Customer)]
    public class LoanRelatedReceiptPromController : Controller
    {
        private readonly ILoanRelatedReceiptPromService _loanReceiptService;
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;
        private readonly ILoanAccountingService _loanAccountingService;
        private readonly ICommonService _commonService;

        public LoanRelatedReceiptPromController(ILogger logger, SessionManager sessionManager,
            ICommonService commonService, ILoanRelatedReceiptPromService loanReceiptService)
        {
            _loanReceiptService = loanReceiptService;
            _logger = logger;
            _sessionManager = sessionManager;
            _commonService = commonService;
        }
        public IActionResult Index()
        {
            var loans = _sessionManager.GetAllAccountingLoanNumber();
            return View(loans);
        }
        public async Task<IActionResult> ViewAccount(long AccountNumber, int LoanSub, string UnitName, string MainModule)
        {
            var allcodetype = await _loanReceiptService.GetCodeTableList(AccountNumber);
           
            var TransactionType = allcodetype.Where(x => x.CodeType == Constants.TransactionType);
            var ModeOfPayment = allcodetype.Where(x => x.CodeType == Constants.ModeOfPayment);

            var allGenerateReceiptList = await _loanReceiptService.GetAllGenerateReceiptPaymentList(AccountNumber);
            foreach (var i in allGenerateReceiptList)
            {
                var transtype = TransactionType.Where(x => x.Id == i.TblLaReceiptDet.TransTypeId);
                if (i.TblLaPaymentDet!=null)
                {
                    var modeofpayment = ModeOfPayment.Where(x => x.Id == i.TblLaPaymentDet.PaymentMode);
                    i.ModeType = modeofpayment.FirstOrDefault().CodeValue;
                }
                    
                i.TransactionType = transtype.FirstOrDefault().CodeValue;
            
                if (i.UniqueID == null)
                {
                    i.UniqueID = Guid.NewGuid().ToString();
                }
            }
            var allRecieptPayments = await _loanReceiptService.GetAllGenerateReceiptPaymentList(AccountNumber);
            foreach (var i in allRecieptPayments)
            {
                var transtype = TransactionType.Where(x => x.Id == i.TblLaReceiptDet.TransTypeId);

                if (i.TblLaPaymentDet != null)
                {
                    var modeofpayment = ModeOfPayment.Where(x => x.Id == i.TblLaPaymentDet.PaymentMode);
                    i.ModeType = modeofpayment.FirstOrDefault().CodeValue;
                }
                i.TransactionType = transtype.FirstOrDefault().CodeValue;
               
                if (i.UniqueID == null)
                {
                    i.UniqueID = Guid.NewGuid().ToString();
                }
            }
            List<TblLaReceiptPaymentDetDTO> paymentRecipetList = new List<TblLaReceiptPaymentDetDTO>();
            var paymentId = allRecieptPayments.Where(x => x.PaymentId != null).Select(x => x.PaymentId).Distinct().ToList();
            foreach (var Id in paymentId)
            {
                var list = allRecieptPayments.Where(x => x.PaymentId == Id).FirstOrDefault();
                paymentRecipetList.Add(list);

            }

            _sessionManager.SetCodeTableModeofPaymentList(ModeOfPayment);
            _sessionManager.SetCodeTableTransactionTypeList(TransactionType);
            _sessionManager.SetGenerateReceiptPaymentList(allGenerateReceiptList);

            LoanAccountingDTO loanAccountingDTO = new();
            loanAccountingDTO.ModeOfPayment = ModeOfPayment.ToList();
            loanAccountingDTO.TransactionType = TransactionType.ToList();
            loanAccountingDTO.AllGenerateReceiptList = allGenerateReceiptList.ToList();
            loanAccountingDTO.PaymentRecipetList = paymentRecipetList;


            ViewBag.AccountNumber = AccountNumber;
            ViewBag.LoanSub = LoanSub;
            ViewBag.UnitName = UnitName;
            //ViewBag.transactiontype = TransactionType;
            //ViewBag.transactionmode = ModeOfPayment;
            //ViewBag.GenerateReceiptProm = allGenerateReceiptList;
            //ViewBag.RecieptPayments= paymentRecipetList;
            return View(loanAccountingDTO);
        }

    }
}
