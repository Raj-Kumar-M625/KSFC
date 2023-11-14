using Application.DTOs.Address;
using Application.DTOs.Adjustment;
using Application.DTOs.Bank;
using Application.DTOs.BankFile;
using Application.DTOs.Bill;
using Application.DTOs.Configuration;
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
using Domain;
using Domain.Address;
using Domain.Adjustment;
using Domain.Bank;
using Domain.BankFile;
using Domain.Bill;
using Domain.CessTransactions;
using Domain.Contacts;
using Domain.Document;
using Domain.GenarteBankfile;
using Domain.GenerateBankfile;
using Domain.GSTTDS;
using Domain.Master;
using Domain.Payment;
using Domain.Profile;
using Domain.TDS;
using Domain.Transactions;
using Domain.TransactionsBenefits;
using Domain.Uploads;
using Domain.Vendor;
using System.Linq;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BankStatementInputTransaction, BankStatementsListDto>().ReverseMap();

            #region Vendor
            CreateMap<VendorDefaults, VendorDefaultsDto>().ReverseMap();
            CreateMap<VendorBankAccount, VendorBankAccountDto>().ReverseMap();
            CreateMap<VendorBalance, VendorBalanceDto>().ReverseMap();
            CreateMap<VendorPerson, VendorPersonDto>().ReverseMap();
            CreateMap<Vendors, VendorDetailsDto>().ReverseMap();
            #endregion

            #region Payment
            CreateMap<Payments, PaymentDto>().ReverseMap();
            CreateMap<PaymentStatus, PaymentStatusDto>().ReverseMap();
            CreateMap<MappingAdvancePayment, MappingAdvancePaymentDto>().ReverseMap();
            
            CreateMap<Payments, PaymentDto>().ReverseMap().ForMember(d => d.Vendor, opt => opt.MapFrom(s => s.Vendor)).ReverseMap();
            CreateMap<TdsPaymentChallan, TdsPaymentChallanDto>().ReverseMap();
            CreateMap<GsttdsPaymentChallan, GsttdsPaymentChallanDto>().ReverseMap();
            #endregion

            #region Document
            CreateMap<Documents, DocumentsDto>().ReverseMap();
            #endregion
            #region Image
            CreateMap<HeaderProfileDetails, HeaderProfileDetailsDto>().ReverseMap();
            #endregion

            #region Address
            CreateMap<Addresses, AddressesDto>().ReverseMap();
            #endregion

            #region Contact
            CreateMap<Contact, ContactDto>().ReverseMap();
            #endregion

            #region Bill
            CreateMap<Bills, BillsDto>().ReverseMap();
            CreateMap<BillItems, BillItemsDto>().ReverseMap();
            CreateMap<BillStatus, BillStatusDto>().ReverseMap();
            #endregion


            CreateMap<Adjustments, AdjustmentDto > ().ReverseMap();
            CreateMap<AdjustmentStatus, AdjustmentStatusDto > ().ReverseMap();
            #region TDS
            CreateMap<QuarterlyTdsPaymentChallan, QuarterlyTdsPaymentChallanDto>().ReverseMap();
            CreateMap<TdsStatus, TdsStatusDto>().ReverseMap();
            #endregion
            #region Master
            CreateMap<BankMaster, BankMasterDto>().ReverseMap();
            CreateMap<CommonMaster, CommonMasterDto>().ReverseMap();
            CreateMap<CommonMaster, DropDownDto>().ReverseMap();
            CreateMap<BankDetails, BankDetailsDto>().ReverseMap();
            CreateMap<BranchMaster, BranchMasterDto>().ReverseMap();
            #endregion


            #region GSTTDS
            CreateMap<GsttdsStatus, GsttdsStatusDto>().ReverseMap();
            CreateMap<BillGsttdsPayment, BillGsttdsPaymentDto > ().ReverseMap(); 
            CreateMap<GsttdsPaymentChallan, GsttdsPaymentChallanDto> ().ReverseMap();                     
            #endregion

            #region Config
            CreateMap<Config, ConfigDto>().ReverseMap();
            #endregion
            #region GenerateBankFile
            CreateMap<GenerateBankFile, GenerateBankFileDto>().ReverseMap();
            CreateMap<MappingPaymentGenerateBankFile, PaymentGenerateBankFileDto> ().ReverseMap(); 
            CreateMap<BankFileUtrDetails, BankFileUtrDetailsDto>().ReverseMap();
            CreateMap<MappingGenBankFileUtrDetails, MappingGenBankFileUtrDetailsDto> ().ReverseMap();
            #endregion
            #region Transaction
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<TransactionsSummary, TransactionsSummaryDto>().ReverseMap();
            CreateMap<TransactionsSummary, TransactionsBenefits>().ReverseMap();
            CreateMap<TransactionsSummary, TransactionsCess>().ReverseMap();
            #endregion

            #region Reconcile
            CreateMap<Reconciliation, ReconciliationDto>().ReverseMap();
            CreateMap<BankTransactions, BankTransactionDto>().ReverseMap();
            #endregion
            #region Bank
            CreateMap<BankStatementInput, BankStatementInputDto>().ReverseMap();
            #endregion
            #region Adjustment
            CreateMap<Adjustments, AdjustmentDto>().ReverseMap();
            #endregion

        }
    }
}
