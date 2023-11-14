using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.BankFile.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Bill;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.BankFile.Handlers.Commands
{
    public class UpdateBankFilePaymentStatusCommandHandler : IRequestHandler<UpdateBankFilePaymentsStatusCommand, int>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IBillRepository _vendorBillRepository;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMapper _mapper;
        private readonly IBankFileRepository _bankFileRepository;
        public UpdateBankFilePaymentStatusCommandHandler(IPaymentRepository vendorPaymentRepository, ICommonMasterRepository commonMaster, IMapper mapper, IBankFileRepository bankFileRepository, IBillRepository billRepository)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _commonMaster = commonMaster;
            _bankFileRepository = bankFileRepository;
            _mapper = mapper;
            _vendorBillRepository = billRepository;
        }
        public async Task<int> Handle(UpdateBankFilePaymentsStatusCommand request, CancellationToken cancellationToken)
        {
            List<PaymentStatus> paymentStatuses = new List<PaymentStatus>();
            var paymentStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.pStatus);

            var paymentid = _bankFileRepository.GetPaymnetIDByGenBankFileID(request.GenerateBankFileID);
            foreach (var id in paymentid)
            {

                Payments payments = await _vendorPaymentRepository.GetPaymentsById(id);
                PaymentStatus paymentStatus = payments.PaymentStatus;
                paymentStatus.StatusCMID = paymentStatusList.First(p => p.CodeValue == ValueMapping.paid).Id;
                paymentStatus.ModifedBy = request.CurrentUser;
                paymentStatus.ModifiedOn = DateTime.Now;
                paymentStatuses.Add(paymentStatus);
            }
            var result = await _vendorPaymentRepository.UpdatePaymentStatus(paymentStatuses);
            List<BillStatus> billstatuses = new List<BillStatus>();
            var billtatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.bStatus);
            var vendorPaymentLists = await _vendorPaymentRepository.GetBillPaymentById(paymentid);

            foreach (var vendorPayment in vendorPaymentLists)
            {
                var ID = vendorPayment.Bill.BillReferenceNo;
                Bills bills = await _vendorBillRepository.GetBillsByReferenceNo(ID);
                BillStatus billStatus = bills.BillStatus;
                if (bills.BalanceAmount == 0)
                {

                    billStatus.StatusCMID = billtatusList.First(p => p.CodeValue == ValueMapping.paid).Id;
                }
                else
                {
                    billStatus.StatusCMID = billtatusList.First(p => p.CodeValue == ValueMapping.PartiallyPaid).Id;
                }

                billStatus.ModifedBy = request.CurrentUser;
                billStatus.ModifiedOn = DateTime.Now;
                billstatuses.Add(billStatus);
            }
           


             await _vendorBillRepository.UpdateBillStatus(billstatuses);

            return request.GenerateBankFileID.FirstOrDefault();
        }
    }
}
