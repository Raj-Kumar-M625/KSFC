using Application.Interface.Persistence.GenerateBankFiles;
using Application.Interface.Persistence.Payment;
using Application.UserStories.GenerateBankFiles.Requests.Commands;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using Domain.GenerateBankfile;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Handlers.Commands
{
    public class AddMappingFromPaymentToGenrateBankFileCommandHandler : IRequestHandler<AddMappingFromPaymentsToGenerateBankFileCommand, int>
    {
        private readonly IGenerateBankFileRepository _generateBankFile;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IPaymentRepository _vendorPaymentRepository;
        public AddMappingFromPaymentToGenrateBankFileCommandHandler(IGenerateBankFileRepository generateBankFile, IPaymentRepository vendorPaymentRepository, IMapper mapper, IMediator mediator)
        {
            _generateBankFile = generateBankFile;
            _mapper = mapper;
            _mediator = mediator;
            _vendorPaymentRepository = vendorPaymentRepository;
        }

        public async Task<int> Handle(AddMappingFromPaymentsToGenerateBankFileCommand request, CancellationToken cancellationToken)
        {
           MappingPaymentGenerateBankFile paymentGenerateBankFile= new MappingPaymentGenerateBankFile();
            var paymentList =  _vendorPaymentRepository.GetVendorPayment();
         
            
            foreach(var id in request.Id)
            {
                var VendorId = paymentList.Where(x => x.ID == id).Select(x => x.VendorID).FirstOrDefault();
                paymentGenerateBankFile.VendorId = VendorId;
                paymentGenerateBankFile.PaymentId = id;
                paymentGenerateBankFile.GenerateBankFileId = request.GenerateBankFileId;
                paymentGenerateBankFile.CreatedBy= request.CurrentUser;
                paymentGenerateBankFile.CreatedOn = DateTime.Now;
                paymentGenerateBankFile.ModifedBy = paymentGenerateBankFile.CreatedBy;
                paymentGenerateBankFile.ModifiedOn = DateTime.Now;
                await _generateBankFile.AddMappingGenerateBankFile(paymentGenerateBankFile);   
            }
            return request.GenerateBankFileId;
        }
    }
}
