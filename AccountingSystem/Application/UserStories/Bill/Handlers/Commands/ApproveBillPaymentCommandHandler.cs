using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Bill.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using DocumentFormat.OpenXml.InkML;
using Domain.Transactions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{
    public class ApproveBillPaymentCommandHandler:IRequestHandler<ApproveBillPaymentsCommand,int>
    {
        private readonly IBillRepository _billRepository;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMediator _mediator;  
        private readonly IVendorRepository _vendorRepository;
        public ApproveBillPaymentCommandHandler(IBillRepository billRepository,ICommonMasterRepository commonMaster,
                                                IVendorRepository vendorRepository,IMediator mediator)
        {
            _billRepository = billRepository;
            _commonMaster = commonMaster;         
            _mediator = mediator;
            _vendorRepository = vendorRepository;
        }

        public async Task<int> Handle(ApproveBillPaymentsCommand request,CancellationToken cancellationToken)
        {
            var bills = await _billRepository.GetBillsByReferenceNo(request.BillReferenceNo);
            if (bills != null)
            {
                bills.ApprovedDate = DateTime.UtcNow;
                bills.ModifedBy = request.CurrentUser;
                bills.ModifiedOn = DateTime.Now;
                bills.Remarks = request.Remarks;

                var billPStatus = await _billRepository.GetAllBillStatus(bills.ID);
                var billStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.bStatus);

                foreach (var item in billPStatus)
                {
                    if (request.Status == "Approved")
                    {
                        item.StatusCMID = billStatusList.First(x => x.CodeValue == ValueMapping.approved).Id;
                        item.ModifedBy = request.CurrentUser;
                        item.ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        item.StatusCMID = billStatusList.First(x => x.CodeValue == ValueMapping.rejected).Id;
                        item.ModifedBy = request.CurrentUser;
                        item.ModifiedOn = DateTime.Now;
                    }
                }
                //insert to Transaction
                if (request.Status == "Approved")
                {
                    var vendor = await _vendorRepository.GetVendorsDetailsByID(bills.VendorId);
                    await _mediator.Send(new CreateTransactionCommand { bills = bills, vendor = vendor, CurrentUser = request.CurrentUser });
                }


                 await _billRepository.UpdateBillStatus(billPStatus);
                 await _mediator.Send(new UpdateBillPaymentApproveCommand { bills = bills });
                if (request.Status == "Approved")
                {
                    //Update Vendor Balance
                    var vendorBalace = await _vendorRepository.GetVendorBalanceByID(bills.VendorId);
                    {
                        vendorBalace.TotalBillAmount += bills.TotalBillAmount;
                        vendorBalace.TotalNetPayable += bills.NetPayable;
                        vendorBalace.TotalTDS += bills.TDS;
                        vendorBalace.TotalGST += bills.GSTAmount;
                        vendorBalace.TotalGST_TDS += bills.GSTTDS;
                        vendorBalace.Pending_NetPayable += bills.NetPayable;
                        vendorBalace.Pending_TDS += bills.TDS;
                        vendorBalace.Pending_GST += bills.GSTAmount;
                        vendorBalace.Pending_GST_TDS += bills.GSTTDS;
                    }
                    await _vendorRepository.UpdateVendorBalance(vendorBalace);
                }

                return bills.ID;
            }
            else
            {
                throw new ArgumentException("Bill not found.");
            }
           
        }
    }
}
