using Application.DTOs.Address;
using Application.DTOs.Adjustment;
using Application.DTOs.Bank;
using Application.DTOs.BankFile;
using Application.DTOs.Bill;
using Application.DTOs.Contacts;
using Application.DTOs.Document;
using Application.DTOs.GenerateBankFile;
using Application.DTOs.GSTTDS;
using Application.DTOs.Master;
using Application.DTOs.Payment;
using Application.DTOs.Profile;
using Application.DTOs.TDS;
using Application.DTOs.Transaction;
using Application.DTOs.Vendor;
using AutoMapper;
using Common.InputSearchCriteria;
using Domain.GSTTDS;
using Domain.Payment;
using Domain.TDS;
using Presentation.Models;
using Presentation.Models.Address;
using Presentation.Models.Adjustment;
using Presentation.Models.Bank;
using Presentation.Models.BankFile;
using Presentation.Models.Bill;
using Presentation.Models.Contacts;
using Presentation.Models.GenerateBankFiles;
using Presentation.Models.GSTTDS;
using Presentation.Models.Master;
using Presentation.Models.Payment;
using Presentation.Models.Profile;
using Presentation.Models.TDS;
using Presentation.Models.Transactions;
using Presentation.Models.Vendor;

namespace Presentation.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region Bank
            CreateMap<BankStatementsListDto, BankStatementsModel>().ReverseMap();
            CreateMap<GenericInputSearchCriteria, BankStatementsSearchCriteriaModel>().ReverseMap();
            #endregion

            #region Vendor
            CreateMap<VendorDefaultsDto, VendorDefaultsModel>().ReverseMap();
            CreateMap<VendorBankAccountDto, VendorBankAccountModel>().ReverseMap();
            CreateMap<VendorPersonDto, VendorPersonModel>().ReverseMap();
            CreateMap<VendorBalanceDto, VendorBalanceModel>().ReverseMap();
            CreateMap<VendorDetailsDto, VendorViewModel>().ReverseMap();
            #endregion

            #region Address
            CreateMap<AddressesDto, AddressesModel>().ReverseMap();
            #endregion

            #region Document
            CreateMap<DocumentsDto, DocumentsModel>().ReverseMap();
            #endregion
            #region Image
            CreateMap<HeaderProfileDetailsDto, HeaderProfileDetailsModel>().ReverseMap();
            #endregion

            #region Payment
            CreateMap<PaymentViewModel, VendorPaymentViewModel>().ReverseMap();
            CreateMap<PaymentStatusDto, PaymentStatusModel>().ReverseMap();
            CreateMap<PaymentDto, PaymentViewModel>().ReverseMap();
            CreateMap<PaymentDto, VendorPaymentViewModel>().ReverseMap();
            CreateMap<MappingAdvancePaymentDto, MappingAdvancePaymentModel>().ReverseMap();
            CreateMap<TdsPaymentChallanDto, TdsPaymentChallanViewModel>().ReverseMap();
            CreateMap<BillPayment, BillPaymentModel>().ReverseMap();

            #endregion

#region TDS
            CreateMap<TdsStatusDto, TdsStatusModel>().ReverseMap();
            CreateMap<GsttdsStatusDto, GsttdsStatusModel>().ReverseMap();
#endregion
            #region GSTTDS
            CreateMap<GsttdsPaymentChallanDto, GsttdsPaymentChallanViewModel>().ReverseMap();
            CreateMap<GsttdsStatusDto, GsttdsStatusModel>().ReverseMap();
            CreateMap<BillGsttdsPaymentDto, BillGsttdsPaymentModel>().ReverseMap();
            CreateMap<GsttdsPaymentChallan, GsttdsPaymentChallanViewModel > ().ReverseMap();
            CreateMap<GsttdsStatus, GsttdsStatusModel>().ReverseMap();

            #endregion

            #region Contact
            CreateMap<ContactDto, ContactModel>().ReverseMap();
            #endregion 

            #region Bill
            CreateMap<BillsDto, BillModel>().ReverseMap();
            CreateMap<BillItemsDto, BillItemsModel>().ReverseMap();
            CreateMap<BillStatusDto, BillStatusModel > ().ReverseMap();
            CreateMap<AdjustmentDto, AdjustmentViewModel>().ReverseMap();
            CreateMap<AdjustmentStatusDto, AdjustmentStatusModel>().ReverseMap();
            #endregion

            #region Master
            CreateMap<BankMasterDto, BankMasterModel>().ReverseMap();
            CreateMap<CommonMasterDto, CommonMasterModel>().ReverseMap();
            CreateMap<BankDetailsDto, BankDetailsModel>().ReverseMap();
            CreateMap<BranchMasterDto, BranchMasterModel>().ReverseMap();
            #endregion

            #region Identity
            //CreateMap<OTPRequest, SendOtpVM>().ReverseMap();
            //CreateMap<RegistrationRequest, RegisterVM>().ReverseMap();
            #endregion
            #region GenrateBankFile
            CreateMap<GenerateBankFileModel, GenerateBankFileDto>().ReverseMap();
            CreateMap<BankFileUtrDetailsModel, BankFileUtrDetailsDto>().ReverseMap();
            CreateMap<PaymentGenerateBankFileDto, MappingPaymentGenerateBankFileModel>().ReverseMap();
            #endregion

            #region Reconcile
            CreateMap<ReconciliationDto, ReconciliationModel>().ReverseMap();
            CreateMap<BankTransactionDto, BankTransactionModel>().ReverseMap();
            CreateMap<TransactionsSummaryDto, TransactionsSummaryModel>().ReverseMap();
            #endregion
        }
    }
}
