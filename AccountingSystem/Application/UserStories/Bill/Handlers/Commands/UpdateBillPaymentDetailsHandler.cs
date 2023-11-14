using Application.Interface.Persistence.Bill;
using Application.UserStories.Bill.Requests.Commands;
using AutoMapper;
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
    public class UpdateBillPaymentDetailsHandler : IRequestHandler<UpdateBillPaymentDetails, long>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        public UpdateBillPaymentDetailsHandler(IBillRepository vendorRepository, IMapper mapper)
        {
            _billRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<long> Handle(UpdateBillPaymentDetails request, CancellationToken cancellationToken)
        {

            var billlist = request.BillDto.ToList();
            foreach (var billDetails in billlist)
            {
                billDetails.BillsID = request.billPayment.Id;
                billDetails.VendorID = request.billPayment.VendorId;
                billDetails.BillReferenceNo = request.billPayment.BillReferenceNo;
                billDetails.GSTTDS = billDetails.BaseAmount * request.billPayment.GSTTDSWithholdPercent / 100;
                billDetails.TDS = billDetails.BaseAmount * request.billPayment.TDSWithholdPercent / 100;
                billDetails.TotalNetPayable = billDetails.BaseAmount - (billDetails.GSTTDS + billDetails.TDS);
                billDetails.BalanceAmount = billDetails.BaseAmount - (billDetails.GSTTDS + billDetails.TDS);              
            }
            var user = request.user;
            var billpayment = _mapper.Map<List<BillItems>>(billlist);
            var bill = await _billRepository.UpdatePaymentDetails(billpayment,user);
            return bill[0].BillsID;
        }
    }
}
