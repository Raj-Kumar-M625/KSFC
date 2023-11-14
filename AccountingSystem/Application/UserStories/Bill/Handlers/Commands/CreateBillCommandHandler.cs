using Application.DTOs.Bill;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Master;
using Application.UserStories.Bill.Requests.Commands;
using Application.UserStories.Master.Request.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{
    public class CreateBillCommandHandler:IRequestHandler<CreateBillCommand,BillsDto>
    {
        private readonly IBillRepository _billRepository;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateBillCommandHandler(IBillRepository vendorRepository,ICommonMasterRepository commonMaster,IMapper mapper,IMediator mediator)
        {
            _billRepository = vendorRepository;
            _commonMaster = commonMaster;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<BillsDto> Handle(CreateBillCommand request,CancellationToken cancellationToken)
        {

            var bills = _mapper.Map<List<Bills>>(request.BillDto);
            var vendorCategoryType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.AssementYear });
            var billStatus = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.bStatus });

            var tdsStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.tStatus);
            var pendingStatusId = tdsStatusList.FirstOrDefault(t => t.CodeValue == ValueMapping.pending)?.Id;

            var GSTtdsStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.gstTdsStatus);
            var GSTTDSpendingStatusId = GSTtdsStatusList.FirstOrDefault(t => t.CodeValue == ValueMapping.pending)?.Id;



            var res = GetFinancialYear(bills.FirstOrDefault().BillDate);
            var assementYearCode = vendorCategoryType.FirstOrDefault(c => c.CodeValue == res);
            var status = billStatus.FirstOrDefault(c => c.CodeValue == ValueMapping.billStatus);

            var Billspayment = new Bills()
            {
                VendorId = bills.FirstOrDefault().VendorId,
                TDSWithholdPercent = bills.FirstOrDefault().TDSWithholdPercent,
                GSTTDSWithholdPercent = bills.FirstOrDefault().GSTTDSWithholdPercent,
                BillDate = bills.FirstOrDefault().BillDate,
                BillNo = bills.FirstOrDefault().BillNo,
                PaymentTerms = bills.FirstOrDefault().PaymentTerms,
                BillDueDate = bills.FirstOrDefault().BillDueDate,
                BillAmount = bills.FirstOrDefault().BillAmount,
                TDS = bills.FirstOrDefault().TDS * (-1),
                GSTTDS = bills.FirstOrDefault().GSTTDS * (-1),
                Royalty = bills.FirstOrDefault().Royalty * (-1),
                CBF = bills.FirstOrDefault().CBF * (-1),
                LabourWelfareCess = bills.FirstOrDefault().LabourWelfareCess * (-1),
                Penalty = bills.FirstOrDefault().Penalty * (-1),
                Other1 = bills.FirstOrDefault().Other1,
                Other1Value = bills.FirstOrDefault().Other1Value,
                Other2 = bills.FirstOrDefault().Other2,
                Other2Value = bills.FirstOrDefault().Other2Value,
                Other3 = bills.FirstOrDefault().Other3,
                Other3Value = bills.FirstOrDefault().Other3Value,
                SGSTAmount = 0,
                CGSTAmount = 0,
                IGSTAmount = 0,
                GSTAmount = bills.FirstOrDefault().GSTAmount,
                NetPayable = bills.FirstOrDefault().NetPayable,
                BalanceAmount = bills.FirstOrDefault().NetPayable,
                TotalBillAmount = bills.FirstOrDefault().TotalBillAmount,
                CreatedOn = DateTime.UtcNow,
                AssementYearCMID = assementYearCode.Id,
                CreatedBy = request.user,
                ModifedBy = request.user,
                ModifiedOn = DateTime.UtcNow,
                BillStatus = new BillStatus()
                {
                    VendorId = bills.FirstOrDefault().VendorId,
                    CreatedBy = request.user,
                    CreatedOn = DateTime.UtcNow,
                    ModifedBy = request.user,
                    ModifiedOn = DateTime.UtcNow,
                    StatusCMID = status.Id
                },
                GSTTDSStatus = new Domain.GSTTDS.GsttdsStatus()
                {
                    VendorId = bills.FirstOrDefault().VendorId,
                    CreatedBy = request.user,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    ModifedBy = request.user,
                    StatusCMID = GSTTDSpendingStatusId.HasValue ? GSTTDSpendingStatusId.Value : 0
                },
                TDSStatus = new Domain.TDS.TdsStatus()
                {
                    VendorId = bills.FirstOrDefault().VendorId,
                    CreatedBy = request.user,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    ModifedBy = request.user,                    
                    StatusCMID = pendingStatusId.HasValue ? pendingStatusId.Value : 0
                }

            };

            if (Billspayment.GSTTDS <= 0 || Billspayment.GSTTDS == null)
            {
                Billspayment.GSTTDS = 0;
                Billspayment.GSTTDSStatus = null;
            }
            Billspayment = await _billRepository.AddBills(Billspayment);
            return _mapper.Map<BillsDto>(Billspayment);
        }

        public string GetFinancialYear(DateTime date)
        {
            if (date.Month <= 3)
            {
               date.AddYears(1);
                return date.Year - 1 + "-" + date.Year;
            }
            else
            {
                var res = date.AddYears(2);

                return date.Month > 3 ? date.Year + 1 + "-" + res.Year : date.Year + "-" + date.Year;

            }

        }


    }
}
