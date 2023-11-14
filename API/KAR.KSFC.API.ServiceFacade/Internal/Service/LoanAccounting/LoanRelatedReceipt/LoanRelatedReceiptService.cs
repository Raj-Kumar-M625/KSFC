using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Validations;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.LoanAccounting.LoanRelatedReceipt
{
    public class LoanRelatedReceiptService : ILoanRelatedReceiptService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<TblLaReceiptDet> _generateReceiptRepository;
        private readonly IEntityRepository<TblLaPaymentDet> _generatePaymentRepository;
        private readonly IEntityRepository<TblLaReceiptPaymentDet> _savedReceiptRepository;
        private readonly UserInfo _userInfo;

        public LoanRelatedReceiptService(IUnitOfWork work,
                           IMapper mapper, IEntityRepository<TblLaReceiptDet> generateReceiptRepository, IEntityRepository<TblLaPaymentDet> generatePaymentRepository, IEntityRepository<TblLaReceiptPaymentDet> savedReceiptRepository, UserInfo userInfo)
        {
            _work = work;
            _mapper = mapper;
            _generateReceiptRepository = generateReceiptRepository;
            _generatePaymentRepository = generatePaymentRepository;
            _savedReceiptRepository = savedReceiptRepository;
            _userInfo = userInfo;
        }

        #region Receipt Payment List
        public async Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllReceiptPaymentList(long accountNumber, CancellationToken token)
        {
            var data = await _savedReceiptRepository.FindByMatchingPropertiesAsync(token, x => x.TblLaReceiptDet.LoanNo == accountNumber 
            && x.IsActive == true && x.IsDeleted == false,
            Receiptdet => Receiptdet.TblLaReceiptDet,
            PaymentDet => PaymentDet.TblLaPaymentDet).ConfigureAwait(false);
            return _mapper.Map<List<TblLaReceiptPaymentDetDTO>>(data);
        }

        public async Task<bool> UpdateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            var currentDetails = await _savedReceiptRepository.FindByExpressionAsync(x => x.ReceiptPaymentId == ReceiptPaymentDto.ReceiptPaymentId, token);
            currentDetails.FirstOrDefault().IsDeleted = true;
            currentDetails.FirstOrDefault().IsActive = false;
            currentDetails.FirstOrDefault().ModifiedBy = _userInfo.Name;
            currentDetails.FirstOrDefault().ModifiedDate = DateTime.UtcNow;
           
            await _savedReceiptRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

            var currentReceiptDetails = await _generateReceiptRepository.FindByExpressionAsync(x => x.Id == ReceiptPaymentDto.ReceiptId, token);
            currentReceiptDetails.FirstOrDefault().Id = ReceiptPaymentDto.TblLaReceiptDet.Id;
            currentReceiptDetails.FirstOrDefault().ReceiptRefNo = ReceiptPaymentDto.TblLaReceiptDet.ReceiptRefNo;
            currentReceiptDetails.FirstOrDefault().ReceiptStatus = ReceiptPaymentDto.TblLaReceiptDet.ReceiptStatus;
            currentReceiptDetails.FirstOrDefault().DateOfGeneration = ReceiptPaymentDto.TblLaReceiptDet.DateOfGeneration;
            currentReceiptDetails.FirstOrDefault().AmountDue = ReceiptPaymentDto.TblLaReceiptDet.AmountDue;
            currentReceiptDetails.FirstOrDefault().DueDatePayment = ReceiptPaymentDto.TblLaReceiptDet.DueDatePayment;
            currentReceiptDetails.FirstOrDefault().TransTypeId = ReceiptPaymentDto.TblLaReceiptDet.TransTypeId;
            currentReceiptDetails.FirstOrDefault().LoanNo = ReceiptPaymentDto.TblLaReceiptDet.LoanNo;
            currentReceiptDetails.FirstOrDefault().Remarks = ReceiptPaymentDto.TblLaReceiptDet.Remarks;
            currentReceiptDetails.FirstOrDefault().IsActive = true;
            currentReceiptDetails.FirstOrDefault().IsDeleted = false;
            currentReceiptDetails.FirstOrDefault().CreatedBy = _userInfo.Name;
            currentReceiptDetails.FirstOrDefault().CreatedDate = DateTime.UtcNow;
            await _generateReceiptRepository.UpdateAsync(currentReceiptDetails, token).ConfigureAwait(false);


            var basicDetails = _mapper.Map<TblLaReceiptPaymentDet>(ReceiptPaymentDto);
            basicDetails.ReceiptPaymentId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var paymentDetails= _mapper.Map<TblLaPaymentDet>(ReceiptPaymentDto.TblLaPaymentDet);

            var response = await _savedReceiptRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            var payresponse = await _generatePaymentRepository.UpdateAsync(paymentDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateCreatePaymentDetails(List<TblLaReceiptPaymentDetDTO> ReceiptPaymentDto, CancellationToken token)
        {

            foreach (var receiptPaymentDto in ReceiptPaymentDto)
            {
                var currentDetails = await _savedReceiptRepository.FindByExpressionAsync(x => x.ReceiptPaymentId == receiptPaymentDto.ReceiptPaymentId, token);
         
                currentDetails.FirstOrDefault().ModifiedBy = _userInfo.Name;
                currentDetails.FirstOrDefault().ModifiedDate = DateTime.UtcNow;
                await _savedReceiptRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
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
                var response = await _savedReceiptRepository.AddAsync(basic, token).ConfigureAwait(false);
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


        public async Task<bool> DeleteReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblLaReceiptPaymentDet>(ReceiptPaymentDto);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _savedReceiptRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }

        public async Task<bool> ApproveReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            var currentDetails = await _savedReceiptRepository.FindByExpressionAsync(x => x.ReceiptPaymentId == ReceiptPaymentDto.ReceiptPaymentId, token);
            currentDetails.FirstOrDefault().ReceiptPaymentId = ReceiptPaymentDto.ReceiptPaymentId;
            currentDetails.FirstOrDefault().IsActive = false;
            currentDetails.FirstOrDefault().IsDeleted = true;
            await _savedReceiptRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

            if (ReceiptPaymentDto.PaymentId != null)
            {
                var currentPaymentDetails = await _generatePaymentRepository.FindByExpressionAsync(x => x.Id == ReceiptPaymentDto.PaymentId, token);
                currentPaymentDetails.FirstOrDefault().Id = ReceiptPaymentDto.TblLaPaymentDet.Id;
                currentPaymentDetails.FirstOrDefault().PaymentStatus = ReceiptPaymentDto.TblLaPaymentDet.PaymentStatus;
                currentPaymentDetails.FirstOrDefault().IsActive = true;
                currentPaymentDetails.FirstOrDefault().IsDeleted = false;
                await _generatePaymentRepository.UpdateAsync(currentPaymentDetails, token).ConfigureAwait(false);
            }

            var currentReceiptDetails = await _generateReceiptRepository.FindByExpressionAsync(x => x.Id == ReceiptPaymentDto.ReceiptId, token);
            currentReceiptDetails.FirstOrDefault().ReceiptStatus = ReceiptPaymentDto.TblLaReceiptDet.ReceiptStatus;
            currentReceiptDetails.FirstOrDefault().CreatedBy = _userInfo.Name;
            currentReceiptDetails.FirstOrDefault().CreatedDate = DateTime.UtcNow;
            await _generateReceiptRepository.UpdateAsync(currentReceiptDetails, token).ConfigureAwait(false);

            var basicDetails = _mapper.Map<TblLaReceiptPaymentDet>(ReceiptPaymentDto);
            basicDetails.ReceiptPaymentId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _savedReceiptRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);

            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> RejectReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            var currentDetails = await _savedReceiptRepository.FindByExpressionAsync(x => x.ReceiptPaymentId == ReceiptPaymentDto.ReceiptPaymentId, token);
            currentDetails.FirstOrDefault().ReceiptPaymentId = ReceiptPaymentDto.ReceiptPaymentId;
            currentDetails.FirstOrDefault().IsActive = false;
            currentDetails.FirstOrDefault().IsDeleted = true;
            await _savedReceiptRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

            var currentPaymentDetails = await _generatePaymentRepository.FindByExpressionAsync(x => x.Id == ReceiptPaymentDto.PaymentId, token);
            currentPaymentDetails.FirstOrDefault().Id = ReceiptPaymentDto.TblLaPaymentDet.Id;
            currentPaymentDetails.FirstOrDefault().PaymentStatus = ReceiptPaymentDto.TblLaPaymentDet.PaymentStatus;
            currentPaymentDetails.FirstOrDefault().IsActive = true;
            currentPaymentDetails.FirstOrDefault().IsDeleted = false;
            await _generatePaymentRepository.UpdateAsync(currentPaymentDetails, token).ConfigureAwait(false);

            var basicDetails = _mapper.Map<TblLaReceiptPaymentDet>(ReceiptPaymentDto);
            basicDetails.ReceiptPaymentId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _savedReceiptRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<IEnumerable<TblLaReceiptDetDTO>> GetAllReceiptRefNum(CancellationToken token)
        {
            var data = await _generateReceiptRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return _mapper.Map<List<TblLaReceiptDetDTO>>(data);
        }

        public async Task<IEnumerable<TblLaPaymentDetDTO>> GetAllPaymentRefNum(CancellationToken token)
        {
            var data = await _generatePaymentRepository.FindByExpressionAsync(x => x.IsActive == true && x.PaymentRefNo != null, token).ConfigureAwait(false);
            return _mapper.Map<List<TblLaPaymentDetDTO>>(data);
        }

        public async Task<bool> CreateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblLaReceiptPaymentDet>(ReceiptPaymentDto);

            TblLaReceiptDet Receiptlist = new TblLaReceiptDet();
            Receiptlist = basicDetails.TblLaReceiptDet;
            Receiptlist.IsActive = true;
            Receiptlist.IsDeleted = false;
            Receiptlist.CreatedBy = _userInfo.Name;
            Receiptlist.CreatedDate = DateTime.UtcNow;
            var Receiptresponse = await _generateReceiptRepository.AddAsync(Receiptlist, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            int receiptid = Receiptresponse.Id;

        

            if (receiptid != 0)
            {
                basicDetails.ReceiptPaymentId = 0;
            
                basicDetails.ReceiptId = receiptid;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreateBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;
               

            }

            var response = await _savedReceiptRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> CreatePaymentDetails(TblLaPaymentDetDTO PaymentDto, CancellationToken token)
        {

            var basicDetails = _mapper.Map<TblLaPaymentDet>(PaymentDto);

            basicDetails.DateOfInitiation = DateTime.UtcNow;
            basicDetails.PaidDate= DateTime.UtcNow; 
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;
            var Paymentresponse = await _generatePaymentRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            int paymentid = Paymentresponse.Id;


            foreach (var rid in PaymentDto.ReceiptId)
            {
                var recieptDetails = await _generateReceiptRepository.FindByExpressionAsync(x => x.Id == rid && x.IsActive == true, token);
                if (recieptDetails != null)
                {
                    recieptDetails.FirstOrDefault().ReceiptStatus = PaymentDto.PaymentStatus;
                    recieptDetails.FirstOrDefault().ModifiedBy = _userInfo.Name;
                    recieptDetails.FirstOrDefault().ModifiedDate = DateTime.UtcNow;
                    await _generateReceiptRepository.UpdateAsync(recieptDetails, token).ConfigureAwait(false);
                    await _work.CommitAsync(token).ConfigureAwait(false);
                }
            }


                foreach (var rpid in PaymentDto.PaymentReceiptId)
            {
                foreach (var rid in PaymentDto.ReceiptId)
                {
                    var recepitpaymentdetails = await _savedReceiptRepository.FindByExpressionAsync(x => x.IsActive == true && x.ReceiptPaymentId ==  rpid && x.ReceiptId == rid, token).ConfigureAwait(false);
                    var result = recepitpaymentdetails.FirstOrDefault();
                    if (result != null)
                    {
                        result.PaymentId = paymentid;
                        result.ModifiedBy = _userInfo.Name;
                        result.ModifiedDate = DateTime.UtcNow;

                        await _savedReceiptRepository.UpdateAsync(result, token).ConfigureAwait(false);
                        await _work.CommitAsync(token).ConfigureAwait(false);
                    } 

                }
            }
            if (Paymentresponse != null)
            {
                return true;
            }
            else
            {
                return false;
            }

           
        }
        #endregion

    }
}
