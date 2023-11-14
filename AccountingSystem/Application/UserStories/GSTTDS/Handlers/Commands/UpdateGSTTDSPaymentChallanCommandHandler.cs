using Application.DTOs.GSTTDS;
using Application.Interface.Persistence.GSTTDS;
using Application.Interface.Persistence.Master;
using Application.UserStories.GSTTDS.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.GSTTDS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GSTTDS.Handlers.Commands
{
    public class UpdateGsttdsPaymentChallanCommandHandler:IRequestHandler<UpdateGsttdsPaymentChallanCommand,GsttdsPaymentChallanDto>
    {
        private readonly IMapper _mapper;
        private readonly IGstdsPaymentChallanRepository _gstTdsPaymentChallanRepository;
        private readonly IBillGsttdsPaymentRepository _billGSTTDSPaymentRepository;
        private readonly IGsttdsStatusRepository _gSTTDSStatusRepository;
        private readonly ICommonMasterRepository _commonMaster;

        public UpdateGsttdsPaymentChallanCommandHandler(IGstdsPaymentChallanRepository gstTdsPaymentChallanRepository,
                                                        IMapper mapper,IBillGsttdsPaymentRepository billGSTTDSPaymentRepository,
                                                        IGsttdsStatusRepository gSTTDSStatusRepository,ICommonMasterRepository commonMaster)
        {
            _gstTdsPaymentChallanRepository = gstTdsPaymentChallanRepository;
            _mapper = mapper;
            _billGSTTDSPaymentRepository = billGSTTDSPaymentRepository;
            _gSTTDSStatusRepository = gSTTDSStatusRepository;
            _commonMaster = commonMaster;
        }

        public async Task<GsttdsPaymentChallanDto> Handle(UpdateGsttdsPaymentChallanCommand request,CancellationToken cancellationToken)
        {

             _mapper.Map<GsttdsPaymentChallan>(request.gstTdsPaymentChallanList);
            var gstTDSStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.gstTdsStatus);

            var gstTDS = await _gstTdsPaymentChallanRepository.GetGSTTDSPaymentChallanAsync(request.gstTdsPaymentChallanList.Id);
            if (gstTDS != null)
            {
                gstTDS.GSTTDSStatus.StatusCMID = gstTDSStatusList.First(p => p.CodeValue == ValueMapping.paid).Id;
                gstTDS.PaidDate = request.gstTdsPaymentChallanList.PaidDate;
                gstTDS.ModifiedOn = DateTime.UtcNow;
                gstTDS.ModifiedBy = request.user;
                gstTDS.AcknowledgementRefNo = request.gstTdsPaymentChallanList.AcknowledgementRefNo;
                gstTDS.PaidAmount = request.gstTdsPaymentChallanList.PaidAmount;
                gstTDS.UTRNo = request.gstTdsPaymentChallanList.UTRNo;

            }


            IQueryable<BillGsttdsPayment> billGSTTDS = _billGSTTDSPaymentRepository.GetBillGSTTDSPaymentList();

            var res1 = billGSTTDS.Where(x => x.GSTTDSPaymentID == request.gstTdsPaymentChallanList.Id);

            var res2 = res1.Select(x => x.BillID).ToList();
            IList<GsttdsStatus> STTDS = _gSTTDSStatusRepository.GetGSTTDSStatus(res2);
            foreach (var item in STTDS)
            {
                item.ModifedBy = request.user;
                item.ModifiedOn = DateTime.UtcNow;
                item.StatusCMID = gstTDSStatusList.First(p => p.CodeValue == ValueMapping.paid).Id;
                await _gSTTDSStatusRepository.UpdateGSTTDSStatus(item);
            }

            var gSTTDSPayment = await _gstTdsPaymentChallanRepository.UpdateGSTTDSPaymentChallanAsync(gstTDS);


            return _mapper.Map<GsttdsPaymentChallanDto>(gSTTDSPayment);


        }
    }
}
