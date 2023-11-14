using Application.DTOs.Bill;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Payment;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Queries
{
    /// <summary> 
    /// Purpose = GetBillsListByIdRequest Handler 
    /// Author =Swetha M 
    /// Date = 13 06 2022 
    /// </summary>
    public class GetBillsListByIdRequestHandler:IRequestHandler<GetBillsListByIdRequest,List<BillsDto>>
    {
        private readonly IBillRepository _billRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        /// <summary> 
        /// Purpose = GetBillsListByIdRequest Handler Constructor
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public GetBillsListByIdRequestHandler(IBillRepository billRepository,IPaymentRepository paymentRepository,IMapper mapper,IMediator mediator)
        {
            _billRepository = billRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _mediator = mediator;
        }
        /// <summary> 
        /// Purpose = GetBillsListByIdRequest Handler to  get bills by ID
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>

        public async Task<List<BillsDto>> Handle(GetBillsListByIdRequest request,CancellationToken cancellationToken)
        {
            if (request.Type == "Add")
            {
                var billDetails = await _billRepository.GetVendorBillsList(request.ID);
                var billPayment = await _paymentRepository.GetVendorBillPaymentList(request.ID);
                var billId = billDetails.Select(x => x.ID).ToList();
                var pendingID = billPayment.Where(x => billId.Contains(x.BillID) && x.Payments.PaymentStatus.StatusMaster.CodeValue == "Pending");
                var pendingBillIS = pendingID.Select(x => x.BillID).ToList();
                var res = billDetails.Where(x => !pendingBillIS.Contains(x.ID));
                var billsList = _mapper.Map<List<BillsDto>>(res);
                return billsList;
            }
            else
            {
                var billDetails = await _billRepository.GetVendorBillsList(request.ID);
                var billsList = _mapper.Map<List<BillsDto>>(billDetails);
                return billsList;
            }

        }
    }
}
