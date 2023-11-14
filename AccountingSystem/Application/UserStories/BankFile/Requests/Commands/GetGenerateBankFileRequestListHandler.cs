using Application.DTOs.GenerateBankFile;
using Application.Interface.Persistence.GenerateBankFiles;
using Application.Interface.Persistence.Master;
using Application.UserStories.BankFile.Requests.Queries;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.BankFile.Requests.Commands
{

    public class GetGenerateBankFileRequestListHandler:IRequestHandler<GetGenerateBankFileRequestList,List<BankFileDto>>
    {
        private readonly IGenerateBankFileRepository _generateBankFile;
        private readonly IMapper _mapper;
        private readonly IBankMasterRepository _bankMaster;
        public GetGenerateBankFileRequestListHandler(IGenerateBankFileRepository generateBankFile,IMapper mapper,IBankMasterRepository bankMaster)
        {
            _generateBankFile = generateBankFile;
            _mapper = mapper;
            _bankMaster = bankMaster;
        }
        public async Task<List<BankFileDto>> Handle(GetGenerateBankFileRequestList request,CancellationToken cancellationToken)
        {
            var res = _generateBankFile.GetGeneratedBankFile(request.Id);
            var banklist = await _bankMaster.GetBankMasterList();
            foreach (var item in res)
            {
                item.VendorBankName = item.Payments.Vendor.VendorBankAccounts.BranchMaster.BankDetails.BankName;
                item.VendorAccountNumber = item.Payments.Vendor.VendorBankAccounts.AccountNumber;
                item.VendorBranchName = item.Payments.Vendor.VendorBankAccounts.BranchMaster.branch_name;
                item.VendorIfscCode = item.Payments.Vendor.VendorBankAccounts.BranchMaster.branch_ifsc; 
            }

            List<BankFileDto> generatedBankFile = res
                .Select(p => new BankFileDto
                {
                    PaymentReferenceNo = p.Payments.PaymentReferenceNo,
                    PaymentDate = p.CreatedOn,
                    VendorName = p.Payments.Vendor.Name,
                    VendorBankName = p.VendorBankName,
                    VendorBranchName = p.VendorBranchName,
                    VendorIfscCode = p.VendorIfscCode,
                    VendorAccountNumber = p.VendorAccountNumber,
                    PaidAmount = p.Payments.PaidAmount,
                    PaymentStatus = p.Payments.PaymentStatus.StatusMaster.CodeValue,
                    ApprovedBy = p.Payments.ApprovedBy,

                }).ToList();

            return generatedBankFile;
        }


    }




}
