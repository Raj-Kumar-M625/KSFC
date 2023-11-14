using Application.Interface.Persistence.Bill;
using Application.UserStories.Bill.Requests.Commands;
using AutoMapper;
using Domain.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{
    public class CreateBillPaymentDetailsHandler:IRequestHandler<CreateBillPaymentDetails,long>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        public CreateBillPaymentDetailsHandler(IBillRepository vendorRepository,IMapper mapper)
        {
            _billRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateBillPaymentDetails request,CancellationToken cancellationToken)
        {

            var billlist = request.BillDto.ToList();
            foreach (var billDetails in billlist)
            {
                billDetails.BillsID = request.billPayment.Id;
                billDetails.VendorID = request.billPayment.VendorId;
                billDetails.BillReferenceNo = request.billPayment.BillReferenceNo;
                billDetails.CreatedBy = request.billPayment.CreatedBy;
                billDetails.GSTTDS = billDetails.Amount * request.billPayment.GSTTDSWithholdPercent / 100;
                billDetails.TDS = billDetails.Amount * request.billPayment.TDSWithholdPercent / 100;
                if (billDetails.Category.Contains("&nbsp;"))
                {
                    billDetails.Category = billDetails.Category.Replace("&nbsp;", " ");
                }
                if (billDetails.Category.Contains("&amp;"))
                {
                    billDetails.Category = billDetails.Category.Replace("&amp;", "&");
                }
                billDetails.TotalNetPayable = billDetails.BaseAmount - (billDetails.GSTTDS + billDetails.TDS);
                billDetails.BalanceAmount = billDetails.BaseAmount - (billDetails.GSTTDS + billDetails.TDS);
                billDetails.CreatedOn = DateTime.UtcNow;
                billDetails.CreatedBy = request.user;
                billDetails.ModifedBy = request.user;
                billDetails.ModifiedOn = DateTime.Now;
            }
            var billpayment = _mapper.Map<List<BillItems>>(billlist);
            var bill = await _billRepository.AddBillPaymentDetails(billpayment);
            return bill[0].BillsID;
        }
    }
}
