using Application.DTOs.Bill;
using Application.Interface.Persistence.Bill;
using Application.UserStories.Bill.Requests.Commands;
using Application.UserStories.Master.Request.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{


    public class UpdateBillCommandDetailHandler : IRequestHandler<UpdateBillCommand, BillsDto>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UpdateBillCommandDetailHandler(IBillRepository vendorRepository, IMapper mapper, IMediator mediator)
        {
            _billRepository = vendorRepository;
            _mapper = mapper;
            _mediator = mediator;
        }


        public async Task<BillsDto> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
        {


            List<Bills> bills = _mapper.Map<List<Bills>>(request.BillDto);
            var vendorCategoryType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.AssementYear });



            var res = GetFinancialYear(bills.First().BillDate);
            var assementYearCode = vendorCategoryType.First(c => c.CodeValue == res);
            var Billspayment = new Bills()
            {
                BillDate = bills.First().BillDate,
                BillDueDate = bills.First().BillDueDate,
                BillAmount = bills.First().BillAmount,
                GSTAmount = bills.First().GSTAmount,
                NetPayable = bills.First().NetPayable,
                TDSWithholdPercent = bills.First().TDSWithholdPercent,
                GSTTDSWithholdPercent = bills.First().GSTTDSWithholdPercent,
                Royalty = bills.First().Royalty * (-1),
                CBF = bills.First().CBF * (-1),
                LabourWelfareCess = bills.First().LabourWelfareCess * (-1),
                Penalty = bills.First().Penalty * (-1),
                Other1 = bills.First().Other1,
                Other1Value = bills.First().Other1Value,
                Other2 = bills.First().Other2,
                Other2Value = bills.First().Other2Value,
                Other3 = bills.First().Other3,
                Other3Value = bills.First().Other3Value,
                TDS = bills.First().TDS * (-1),
                AssementYearCMID = assementYearCode.Id,
                VendorId = bills.First().VendorId,
                GSTTDS = bills.First().GSTTDS * (-1),
                SGSTAmount = 0,
                BillNo = bills.First().BillNo,
                CGSTAmount = 0,
                CreatedBy = request.user,
                IGSTAmount = 0,
                TotalBillAmount = bills.First().TotalBillAmount,
                BalanceAmount = bills.First().NetPayable,
                ID = bills.First().ID, 
                ModifiedOn = DateTime.UtcNow,
                ModifedBy = request.user
            };
            if (Billspayment.GSTTDS <= 0 || Billspayment.GSTTDS == null)
            {
                Billspayment.GSTTDS = 0;
                Billspayment.GSTTDSStatus = null;
            }



            Billspayment = await _billRepository.UpdateBillsDetails(Billspayment);
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
