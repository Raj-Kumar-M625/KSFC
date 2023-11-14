using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccountingPromoter.LoanRelatedReceiptProm;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.LoanAccountingPromoter.LoanRelatedReceiptProm
{
     public class LoanRelatedReceiptPromService : ILoanRelatedReceiptPromService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<TblLaReceiptDet> _generateReceiptRepository;
        private readonly IEntityRepository<TblLaPaymentDet> _generatePaymentRepository;
        private readonly IEntityRepository<TblLaReceiptPaymentDet> _receiptPaymentRepository;
        private readonly UserInfo _userInfo;

        public LoanRelatedReceiptPromService(IUnitOfWork work,
                           IMapper mapper, IEntityRepository<TblLaReceiptDet> generateReceiptRepository, UserInfo userInfo, IEntityRepository<TblLaReceiptPaymentDet> receiptPaymentRepository, IEntityRepository<TblLaPaymentDet> generatePaymentRepository)
        {
            _work = work;
            _mapper = mapper;
            _generateReceiptRepository = generateReceiptRepository;
            _userInfo = userInfo;
            _receiptPaymentRepository = receiptPaymentRepository;
            _generatePaymentRepository = generatePaymentRepository;
        }
        #region GenerateReceipt

        public async Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllGenerateReceiptPaymentList(long accountNumber, CancellationToken token)
        {
            var data = await _receiptPaymentRepository.FindByMatchingPropertiesAsync(token, x => x.TblLaReceiptDet.LoanNo == accountNumber && x.IsActive == true && x.IsDeleted == false,
            Receiptdet => Receiptdet.TblLaReceiptDet,
            PaymentDet => PaymentDet.TblLaPaymentDet).ConfigureAwait(false);
            return _mapper.Map<List<TblLaReceiptPaymentDetDTO>>(data);
        }

        public async Task<bool> UpdateCreatePromPaymentDetails(List<TblLaReceiptPaymentDetDTO> ReceiptPaymentDto, CancellationToken token)
        {

            foreach (var receiptPaymentDto in ReceiptPaymentDto)
            {
                var currentDetails = await _receiptPaymentRepository.FindByExpressionAsync(x => x.ReceiptPaymentId == receiptPaymentDto.ReceiptPaymentId, token);
                currentDetails.FirstOrDefault().IsDeleted = true;
                currentDetails.FirstOrDefault().IsActive = false;
                currentDetails.FirstOrDefault().ModifiedBy = _userInfo.Name;
                currentDetails.FirstOrDefault().ModifiedDate = DateTime.UtcNow;
                await _receiptPaymentRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
            }

            foreach (var receiptPaymentDto in ReceiptPaymentDto)
            {
                var currentReceiptDetails = await _generateReceiptRepository.FindByExpressionAsync(x => x.Id == receiptPaymentDto.ReceiptId, token);
                currentReceiptDetails.FirstOrDefault().Id = receiptPaymentDto.TblLaReceiptDet.Id;
                currentReceiptDetails.FirstOrDefault().ReceiptRefNo = receiptPaymentDto.TblLaReceiptDet.ReceiptRefNo;
                currentReceiptDetails.FirstOrDefault().ReceiptStatus = receiptPaymentDto.TblLaReceiptDet.ReceiptStatus;
                currentReceiptDetails.FirstOrDefault().DateOfGeneration = receiptPaymentDto.TblLaReceiptDet.DateOfGeneration;
                currentReceiptDetails.FirstOrDefault().AmountDue = receiptPaymentDto.TblLaReceiptDet.AmountDue;
                currentReceiptDetails.FirstOrDefault().DueDatePayment = receiptPaymentDto.TblLaReceiptDet.DueDatePayment;
                currentReceiptDetails.FirstOrDefault().TransTypeId = receiptPaymentDto.TblLaReceiptDet.TransTypeId;
                currentReceiptDetails.FirstOrDefault().LoanNo = receiptPaymentDto.TblLaReceiptDet.LoanNo;
                currentReceiptDetails.FirstOrDefault().Remarks = receiptPaymentDto.TblLaReceiptDet.Remarks;
                currentReceiptDetails.FirstOrDefault().IsActive = true;
                currentReceiptDetails.FirstOrDefault().IsDeleted = false;
                currentReceiptDetails.FirstOrDefault().CreatedBy = _userInfo.Name;
                currentReceiptDetails.FirstOrDefault().CreatedDate = DateTime.UtcNow;
                await _generateReceiptRepository.UpdateAsync(currentReceiptDetails, token).ConfigureAwait(false);

            }

            foreach (var receiptPaymentDto in ReceiptPaymentDto)
            {
                var currentPaymentDetails = await _generatePaymentRepository.FindByExpressionAsync(x => x.Id == receiptPaymentDto.PaymentId, token);
                currentPaymentDetails.FirstOrDefault().Id = receiptPaymentDto.TblLaPaymentDet.Id;
                currentPaymentDetails.FirstOrDefault().LoanNo = receiptPaymentDto.TblLaPaymentDet.LoanNo;
                currentPaymentDetails.FirstOrDefault().PaymentRefNo = receiptPaymentDto.TblLaPaymentDet.PaymentRefNo;
                currentPaymentDetails.FirstOrDefault().ActualAmt = receiptPaymentDto.TblLaPaymentDet.ActualAmt;
                currentPaymentDetails.FirstOrDefault().DateOfInitiation = receiptPaymentDto.TblLaPaymentDet.DateOfInitiation;
                currentPaymentDetails.FirstOrDefault().PromoterName = receiptPaymentDto.TblLaPaymentDet.PromoterName;
                currentPaymentDetails.FirstOrDefault().ChequeNo = receiptPaymentDto.TblLaPaymentDet.ChequeNo;
                currentPaymentDetails.FirstOrDefault().ChequeDate = receiptPaymentDto.TblLaPaymentDet.ChequeDate;
                currentPaymentDetails.FirstOrDefault().IfscCode = receiptPaymentDto.TblLaPaymentDet.IfscCode;
                currentPaymentDetails.FirstOrDefault().BranchCode = receiptPaymentDto.TblLaPaymentDet.BranchCode;
                currentPaymentDetails.FirstOrDefault().DateOfChequeRealization = receiptPaymentDto.TblLaPaymentDet.DateOfChequeRealization;
                currentPaymentDetails.FirstOrDefault().UtrNo = receiptPaymentDto.TblLaPaymentDet.UtrNo;
                currentPaymentDetails.FirstOrDefault().PaidDate = receiptPaymentDto.TblLaPaymentDet.PaidDate;
                currentPaymentDetails.FirstOrDefault().PaymentMode = receiptPaymentDto.TblLaPaymentDet.PaymentMode;
                currentPaymentDetails.FirstOrDefault().PaymentStatus = ReceiptPaymentDto.FirstOrDefault().TblLaPaymentDet.PaymentStatus;
                currentPaymentDetails.FirstOrDefault().IsActive = true;
                currentPaymentDetails.FirstOrDefault().IsDeleted = false;
                currentPaymentDetails.FirstOrDefault().CreateBy = _userInfo.Name;
                currentPaymentDetails.FirstOrDefault().CreatedDate = DateTime.UtcNow;
                await _generatePaymentRepository.UpdateAsync(currentPaymentDetails, token).ConfigureAwait(false);

            }


            var basicDetails = _mapper.Map<List<TblLaReceiptPaymentDet>>(ReceiptPaymentDto);

            TblLaReceiptPaymentDetDTO payments = new();
           
            foreach (var basic in basicDetails)
            {
                basic.ReceiptPaymentId = 0;
                basic.IsActive = true;
                basic.IsDeleted = false;
                basic.CreateBy = _userInfo.Name;
                basic.CreatedDate = DateTime.UtcNow;
                var response = await _receiptPaymentRepository.AddAsync(basic, token).ConfigureAwait(false);
                payments = _mapper.Map<TblLaReceiptPaymentDetDTO>(response);
            }


            await _work.CommitAsync(token).ConfigureAwait(false);
            if (payments != null)
            {
                return true;
            }
            else
            {
                return false;
            }


           
        }

        #endregion

        public async Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllRecipetsForPayment(int PaymnetId, CancellationToken token)
        {
            var data = await _receiptPaymentRepository.FindByMatchingPropertiesAsync(token, x => x.PaymentId == PaymnetId && x.IsActive == true && x.IsDeleted == false,
             Receiptdet => Receiptdet.TblLaReceiptDet,
            PaymentDet => PaymentDet.TblLaPaymentDet).ConfigureAwait(false);

            return _mapper.Map<List<TblLaReceiptPaymentDetDTO>>(data);

        }

    }
}
