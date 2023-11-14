using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.TDS;
using Application.UserStories.TDS.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Handlers.Commands
{
    public class UpdateQuarterlyTdsPaymentChallanCommandHandler:IRequestHandler<UpdateQuarterlyTdsPaymentChallanCommand,bool>
    {
        private readonly IQuarterlyTdsPaymentChallanRepository _quarterlyTDSPaymentChallanRepository;
        private readonly ITdsPaymentChallanRepository _tdsPaymentChallanRepository;
        private readonly ICommonMasterRepository _commonMasterRepository;
        private readonly IBillTdsPaymentRepository _billTDSPaymentRepository;
        private readonly ITdsStatusRepository _tdsStatusRepository;
        private readonly IMapper _mapper;
        public UpdateQuarterlyTdsPaymentChallanCommandHandler(IQuarterlyTdsPaymentChallanRepository quarterlyTDSPaymentChallanRepository,
            ITdsPaymentChallanRepository tdsPaymentChallanRepository,ICommonMasterRepository commonMasterRepository,IBillTdsPaymentRepository billTDSPaymentRepository,
            ITdsStatusRepository tdsStatusRepository,IMapper mapper)
        {
            _quarterlyTDSPaymentChallanRepository = quarterlyTDSPaymentChallanRepository;
            _tdsPaymentChallanRepository = tdsPaymentChallanRepository;
            _commonMasterRepository = commonMasterRepository;
            _billTDSPaymentRepository = billTDSPaymentRepository;
            _tdsStatusRepository = tdsStatusRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(UpdateQuarterlyTdsPaymentChallanCommand request,CancellationToken cancellationToken)
        {
            var quarterlyTDSPaymentChallanList = _mapper.Map<List<QuarterlyTdsPaymentChallan>>(request.QuarterlyTDSPaymentChallanList);
            await _quarterlyTDSPaymentChallanRepository.UpdateQuarterlyTDSPaymentChallanAsync(quarterlyTDSPaymentChallanList);

            var tdsStatusList = await _commonMasterRepository.GetCommoMasterValues(ValueMapping.tStatus);
            var filedStatus = tdsStatusList.First(t => t.CodeValue == ValueMapping.QtyFiled).Id;
            var status = quarterlyTDSPaymentChallanList.First().QuarterStatus;

            var mappingQuarterlyTDSPaymentChallan = await _quarterlyTDSPaymentChallanRepository.GetMappingQuarterlyTDSPaymentChallanByQuarterChallanId(quarterlyTDSPaymentChallanList.Select(q => q.ID).ToList());
            List<TdsPaymentChallan> tdsPaymentChallanList = mappingQuarterlyTDSPaymentChallan.Select(m => m.TDSPaymentChallan).ToList();
            foreach (var tdsPaymentChallan in tdsPaymentChallanList)
            {
                 tdsPaymentChallan.TDSStatus.StatusCMID = status;
                tdsPaymentChallan.ModifiedBy = request.user;
                tdsPaymentChallan.ModifiedOn = DateTime.UtcNow;
                tdsPaymentChallan.PaymentDate = DateTime.UtcNow;
            }
            await _tdsPaymentChallanRepository.UpdateTDSPaymentChallanAsync(tdsPaymentChallanList);

            var billTDSPayment = _billTDSPaymentRepository.GetBillTDSPaymentListByChallanId(tdsPaymentChallanList.Select(t => t.Id).ToList());
            var billTdsStatus = billTDSPayment.Select(b => b.Bill.TDSStatus);
            var tdsStatusCMId = billTdsStatus.First().StatusCMID;

            if (filedStatus == tdsStatusCMId)
                foreach (var tdsStatus in billTdsStatus)
            {

                tdsStatus.StatusCMID = filedStatus;
                tdsStatus.ModifedBy = request.user;
                tdsStatus.ModifiedOn = DateTime.UtcNow;
            }
            if (status == tdsStatusCMId)
                foreach (var tdsStatus in billTdsStatus)
                {

                    tdsStatus.StatusCMID = status;
                    tdsStatus.ModifedBy = request.user;
                    tdsStatus.ModifiedOn = DateTime.UtcNow;
                }
            await _tdsStatusRepository.UpdateStatusAsync(billTdsStatus);
            return true;
        }
    }
}
