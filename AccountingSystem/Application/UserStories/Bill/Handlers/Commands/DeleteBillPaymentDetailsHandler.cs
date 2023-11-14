using Application.Interface.Persistence.Bill;
using Application.UserStories.Bill.Requests.Commands;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{


    public class DeleteBillPaymentDetailsHandler : IRequestHandler<DeleteBillPaymentDetails, string[]>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public DeleteBillPaymentDetailsHandler(IBillRepository vendorRepository, IMapper mapper)
        {
            _billRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<string[]> Handle(DeleteBillPaymentDetails request, CancellationToken cancellationToken)
        {

            string id = "";
            var billpaydel = request.BillpaymentDetails.ToList();
            for (int bill = 0; bill < billpaydel.Count; bill++)
            {
                id = billpaydel[bill];

                 await _billRepository.DeleteBillDocumentDetails(id);
                
            }
            return new string[] { id };

        }
    }
    }
