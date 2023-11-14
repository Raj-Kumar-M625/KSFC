using AutoMapper;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Dto.Employee;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Data.Models;
using KAR.KSFC.Components.Data.Models.DbModels;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;

namespace KAR.KSFC.Components.Common.Utilities.MapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TblEnqBasicDet, BasicDetailsDto>().ReverseMap()
                .IgnoreAllVirtual();

            CreateMap<TblEnqAddressDet, AddressDetailsDTO>().ReverseMap()
                .IgnoreAllVirtual();

            CreateMap<TblEnqBankDet, BankDetailsDTO>().ReverseMap().IgnoreAllVirtual();
            CreateMap<TblTrgEmployee, EmployeeDTO>().ReverseMap().IgnoreAllVirtual();
            CreateMap<TblEnqPjcostDet, ProjectCostDetailsDTO>().ReverseMap().IgnoreAllVirtual();

            CreateMap<TblEnqRegnoDet, RegistrationNoDetailsDTO>().ReverseMap()
              .IgnoreAllVirtual();

            CreateMap<RegduserTab, RegdUserDTO>()
                .ForMember(dest => dest.mobile, act => act.MapFrom(src => src.UserMobileno))
                .ForMember(dest => dest.Pan, act => act.MapFrom(src => src.UserPan)).ReverseMap();

            CreateMap<TblEnqPromDet, PromoterDetailsDTO>().ReverseMap()
                .IgnoreAllVirtual();

            CreateMap<TblEnqPbankDet, PromoterBankDetailsDTO>().ReverseMap()
               .IgnoreAllVirtual();

            CreateMap<TblPromCdtab, PromoterMasterDTO>().ReverseMap()
                .IgnoreAllVirtual();

            CreateMap<TblEnqPassetDet, PromoterAssetsNetWorthDTO>().ReverseMap()
               .IgnoreAllVirtual();

            CreateMap<TblEnqPliabDet, PromoterLiabilityDetailsDTO>().ReverseMap()
              .IgnoreAllVirtual();

            CreateMap<TblEnqPnwDet, PromoterNetWorthDetailsDTO>().ReverseMap()
              .IgnoreAllVirtual();

            CreateMap<TblEnqGuarDet, GuarantorDetailsDTO>().ReverseMap()
               .IgnoreAllVirtual();

            CreateMap<TblEnqGbankDet, GuarantorBankDetailsDTO>().ReverseMap()
              .IgnoreAllVirtual();

            CreateMap<TblEnqGassetDet, GuarantorAssetsNetWorthDTO>().ReverseMap()
            .IgnoreAllVirtual();

            CreateMap<TblEnqGliabDet, GuarantorLiabilityDetailsDTO>().ReverseMap()
             .IgnoreAllVirtual();
            CreateMap<TblEnqGnwDet, GuarantorNetWorthDetailsDTO>().ReverseMap()
             .IgnoreAllVirtual();
            CreateMap<TblEnqSisDet, SisterConcernDetailsDTO>().ReverseMap()
            .IgnoreAllVirtual();
            CreateMap<TblEnqSfinDet, SisterConcernFinancialDetailsDTO>().ReverseMap()
            .IgnoreAllVirtual();

            CreateMap<TblEnqWcDet, ProjectWorkingCapitalDeatailsDTO>().ReverseMap()
            .IgnoreAllVirtual();
            CreateMap<TblEnqPjfinDet, ProjectFinancialYearDetailsDTO>().ReverseMap()
           .IgnoreAllVirtual();
            CreateMap<TblEnqSecDet, SecurityDetailsDTO>().ReverseMap()
           .IgnoreAllVirtual();
            CreateMap<TblEnqDocument, EnqDocumentDTO>().ReverseMap();

            CreateMap<CnstCdtab, ConstitutionMasterDTO>().IgnoreAllVirtual();

            CreateMap<TblPurpCdtab, LoanPurposeMasterDTO>().IgnoreAllVirtual();
            CreateMap<TblSizeCdtab, IndusSizeMasterDTO>().IgnoreAllVirtual();
            CreateMap<TblPremCdtab, PremisesMasterDTO>().IgnoreAllVirtual();

            CreateMap<TblPjcostCdtab, ProjectCostMasterDTO>().IgnoreAllVirtual().ReverseMap();
            CreateMap<TblBankfacilityCdtab, BankFacilityMasterDTO>().IgnoreAllVirtual().ReverseMap();

            CreateMap<TblFincompCdtab, FinancialComponentMasterDTO>().IgnoreAllVirtual().ReverseMap();
            CreateMap<TblFinyearCdtab, FinancialYearMasterDTO>().IgnoreAllVirtual().ReverseMap();

            CreateMap<TblEnqPjmfDet, ProjectMeansOfFinanceDTO>().IgnoreAllVirtual().ReverseMap();
            CreateMap<TblPjmfcatCdtab, PjmfcatCdtabMasterDTO>().IgnoreAllVirtual().ReverseMap();
            CreateMap<TblPjmfCdtab, PjmfCdtabMasterDTO>().IgnoreAllVirtual().ReverseMap();

            CreateMap<TblPjsecCdtab, ProjSecurityMasterDTO>().IgnoreAllVirtual().ReverseMap();
            CreateMap<TblSecCdtab, SecurityDetailsMasterDTO>().IgnoreAllVirtual().ReverseMap();
            CreateMap<TblPdesigCdtab, PromDesignationMasterDTO>().IgnoreAllVirtual().ReverseMap();
            CreateMap<TblDomiCdtab, DomicileMasterDTO>().IgnoreAllVirtual().ReverseMap();



            //CreateMap<TblAppLoanMast, LoanAccountNumberDTO>()
            //    .ForMember(dest => dest.OffcNam, act => act.MapFrom(src => src.OffcCdtab.OffcNam))
            //    .ForMember(dest => dest.UtName, act => act.MapFrom(src => src.TblUnitMast.UtName)).ReverseMap();


            //CreateMap<TblAppLoanMast,LoanAccountNumberDTO>().IgnoreAllVirtual().ReverseMap();

            // IDM Module strats here

            CreateMap<LDSecurityDetailsDTO, ldSecurityTemp>().IgnoreAllVirtual().ReverseMap(); //By RV on 24-05-2022
            CreateMap<OffcCdtab, OfficeDto>().ReverseMap();
            CreateMap<TblUnitMast, UnitMasterDto>().ReverseMap();
            CreateMap<TblAppLoanMast, LoanAccountNumberDTO>().ReverseMap();
            CreateMap<TblSecurityRefnoMast, SecurityMasterDTO>().ReverseMap();
            CreateMap<TblIdmDeedDet, IdmSecurityDetailsDTO>().ReverseMap();
            CreateMap<TblAssetRefnoDet, AssetRefnoDetailsDTO>().ReverseMap();
            CreateMap<TblIdmHypothDet, IdmHypotheDetailsDTO>().ReverseMap();
            CreateMap<TblIdmDsbCharge, IdmSecurityChargeDTO>().ReverseMap(); 
            
            CreateMap<TblSecCdtab, SecCdDetailsDTO>().ReverseMap();
            CreateMap<TblPjsecCdtab, PjsecCdtabDTO>().ReverseMap();
            CreateMap<TblIdmCersaiRegistration, IdmCersaiRegDetailsDTO>().ReverseMap();
            CreateMap<TblIdmGuarDeedDet, IdmGuarantorDeedDetailsDTO>().ReverseMap();
            CreateMap<TblAppGuarAssetDet, GuarantorAssetDetailDTO>().ReverseMap();
            CreateMap<TbIIfscMaster, IfscMasterDTO>().ReverseMap();
            CreateMap<TblAppGuarnator, AppGuarnatorDTO>().ReverseMap();
            CreateMap<TblChargeType, ChargeTypeDTO>().ReverseMap();
            CreateMap<TblIdmLegalWorkflow, LegalWorkFlowDto>().ReverseMap(); //by GS
            CreateMap<TblIdmHypothDet, IdmHypotheDetailsDTO>().ReverseMap(); //MJ
            CreateMap<TblIdmCondDet, LDConditionDetailsDTO>().ReverseMap();
            CreateMap<TblCondStgCdtab, ConditionStgDTO>().ReverseMap();
            CreateMap<TblCondTypeCdtab, ConditionTypeDTO>().ReverseMap();
            CreateMap<TblCondStageMast, TblCondStageMastDTO>().ReverseMap();
            //CreateMap<TblIdmHypothMap , TblIdmHypothMapDto>().ReverseMap();
            CreateMap<TblIdmHypothMap, TblIdmHypothMapDto>().ReverseMap();

            CreateMap<TblIdmDsbFm813, Form8AndForm13DTO>().ReverseMap();
            CreateMap<Tblfm8fm13CDTab, TblFm8Fm13CdTabDTO>().ReverseMap();

            CreateMap<TblIdmAuditDet, IdmAuditDetailsDTO>().ReverseMap();
            CreateMap<TblAddlCondDet, AdditionConditionDetailsDTO>().ReverseMap();
            CreateMap<TblCondStgMast,ConditionStageMasterDTO>().ReverseMap();

            // CreateMap<TblEmpchairDet, TblEmpchairDetsDto>().ReverseMap(); //by GS
            CreateMap<TblIdmSidbiApproval, IdmSidbiApprovalDTO>().ReverseMap(); // By Dev on 20/08/2022
            CreateMap<CnstCdtab, CnstCdTabDto>().ReverseMap();
            CreateMap<TblPromTypeCdtab, PromoterTypeDTO>().ReverseMap(); // By Dev on 20/08/2022

            CreateMap<TblEmpchairDet, EmployeeChairDetailsDTO>().ReverseMap(); //by GS
            CreateMap<TblIdmFirstInvestmentClause, IdmFirstInvestmentClauseDTO>().ReverseMap(); // By Akhila on 20/08/2022

            CreateMap<TblIdmUnitDetails, IdmUnitDetailDTO>().ReverseMap();
            CreateMap<TblUnitMast, UnitMasterDto>().ReverseMap();
            CreateMap<TblLdDocument, ldDocumentDto>().ReverseMap();
            CreateMap<TblUCDocument, ldDocumentDto>().ReverseMap();
            CreateMap<TblDCDocument, ldDocumentDto>().ReverseMap();
            CreateMap<TblINSPDocument, ldDocumentDto>().ReverseMap(); 
            CreateMap<TblCUDocument, ldDocumentDto>().ReverseMap();

            CreateMap<TblIdmDchgLand, IdmDchgLandDetDTO>().ReverseMap(); //by MJ on 29/08/2022
            CreateMap<TblIdmDspInsp, IdmDspInspDTO>().ReverseMap();
            CreateMap<TblIdmIrBldgMat, IdmBuildingMaterialSiteInspectionDTO>().ReverseMap();

            CreateMap<TblIdmDchgBuildingDet, IdmDchgBuildingDetDTO>().ReverseMap();
            CreateMap<TblIdmDchgWorkingCapital, IdmDchgWorkingCapitalDTO>().ReverseMap();
            CreateMap<TblIdmDchgImportMachinery, IdmDchgImportMachineryDTO>().ReverseMap();
            CreateMap<TblIdmDChgFurn, IdmDChgFurnDTO>().ReverseMap();
            CreateMap<TblIdmDchgMeansOfFinance, IdmDchgMeansOfFinanceDTO>().ReverseMap();


            CreateMap<TblIdmDchgIndigenousMachineryDet, IdmDchgIndigenousInspectionDTO>().ReverseMap();//by MJ on 29/08/2022
            CreateMap<TblIdmDsbLetterOfCredit, IdmDsbLetterOfCreditDTO>().ReverseMap();//by MJ on 05/09/2022

            CreateMap<TblIdmDchgProjectCost, IdmDchgProjectCostDTO>().ReverseMap();//by AD on 05/09/2022

            CreateMap<TblMachineryStatus, TblMachineryStatusDto>().ReverseMap();
            CreateMap<TblProcureMast, TblProcureMastDto>().ReverseMap();
            CreateMap<TblCurrencyMast, TblCurrencyMastDto>().ReverseMap();

            CreateMap<IdmPromoter, IdmPromoterDTO>().ReverseMap(); // Dev
            CreateMap<TblIdmUnitAddress, IdmUnitAddressDTO>().ReverseMap();
            CreateMap<DistCdtab, DistCdTabDTO>().ReverseMap();
            CreateMap<TlqCdtab, TlqCdTabDTO>().ReverseMap();
            CreateMap<HobCdtab, HobCdtabDTO>().ReverseMap();
            CreateMap<TblAddressCdtab, AddressCdTabDTO>().ReverseMap();
            CreateMap<VilCdtab, VilCdTabDTO>().ReverseMap();
            CreateMap<TblIdmPromAddress, IdmPromAddressDTO>().ReverseMap(); // GK
            CreateMap<TblPincodeDistrictCdtab, PincodeDistrictCdtabDTO> ().ReverseMap(); // GK
            CreateMap<TblPincodeStateCdtab, PincodeStateCdtabDTO>().ReverseMap(); // GK
            CreateMap<IdmPromoterBankDetails, IdmPromoterBankDetailsDTO>().ReverseMap(); // By Dev on 01/09/2022
            CreateMap<TblDsbStatImp, IdmDsbStatImpDTO>().ReverseMap(); // By SR on 24/04/2023

            // Asset information
            CreateMap<TblPromCdtab, TblPromcdtabDTO>().ReverseMap(); // By Dev on 01/09/2022 
            CreateMap<TblAssettypeCdtab, AssetTypeDetailsDTO>().ReverseMap();
            CreateMap<TblAssetcatCdtab, TblAssetCatCDTabDTO>().ReverseMap();
            CreateMap<IdmPromAssetDet, IdmPromAssetDetDTO>().ReverseMap();
            CreateMap<TblAcTypeCdtab, TblAcTypeCdtabDTO>().ReverseMap(); // By Dev on 06/09/2022

            //Unit Details
            CreateMap<IdmUnitProducts, IdmUnitProductsDTO>().ReverseMap();
            CreateMap<TblAppUnitDetail, AppUnitDetailDTO>().ReverseMap();
            CreateMap<TblIndCdtab, TblIndCdtabDTO>().ReverseMap();
            CreateMap<TblProdCdtab, TblProdCdtabDTO>().ReverseMap();
            CreateMap<TblIdmUnitBank, IdmChangeBankDetailsDTO>().ReverseMap();
            CreateMap<TblPromterLiabDet, TblPromoterLiabDetDTO>().ReverseMap();
            CreateMap<TblIdmPromoterNetWork, TblPromoterNetWortDTO>().ReverseMap();
            CreateMap<TblPincodeMaster, TblPincodeMasterDTO>().ReverseMap();


            //newly added//by SR on 15/05/2023
            CreateMap<TblIdmProjBldg, TblIdmProjBldgDTO>().ReverseMap();
            CreateMap<TblIdmProjland, TblIdmProjLandDTO>().ReverseMap(); 
            CreateMap<TblIdmProjImpMc, TblIdmProjImpMcDTO>().ReverseMap();
            CreateMap<TblIdmProjPlmc, TblIdmProjPlmcDTO>().ReverseMap();
            CreateMap<TblIdmFurn, TblIdmFurnDTO>().ReverseMap();

            // Creation of Security and Acquisition of Assets
            CreateMap<TblIdmIrLand, TblIdmIrLandDTO>().ReverseMap();
            CreateMap<TblIdmIrPlmc, IdmIrPlmcDTO>().ReverseMap();
            CreateMap<TblLandTypeMast, TblLandTypeMastDTO>().ReverseMap();

            //Loan Allocation 
            CreateMap<TblAllcCdTab, TblAllcCdTabDTO>().ReverseMap();
            CreateMap<TblIdmDhcgAllc, TblIdmDhcgAllcDTO>().ReverseMap();

            //Building Acquisition
            CreateMap<TblIdmBuildingAcquisitionDetails, TblIdmBuildingAcquisitionDetailsDTO>().ReverseMap();

            CreateMap<TblIdmIrFurn, TblIdmIrFurnDTO>().ReverseMap();

            CreateMap<TblLaReceiptDet, TblLaReceiptDetDTO>().ReverseMap();
            CreateMap<TblLaReceiptPaymentDet, TblLaReceiptPaymentDetDTO>().ReverseMap();



            CreateMap<TblLaPaymentDet, TblLaPaymentDetDTO>().ReverseMap();
            CreateMap<CodeTable, CodeTableDTO>().ReverseMap();

            //Disbursment Propasal details
            CreateMap<TblIdmReleDetls, TblIdmReleDetlsDTO>().ReverseMap();
            CreateMap<TblIdmDisbProp, TblIdmDisbPropDTO>().ReverseMap();
            CreateMap<TblIdmBenfDet, TblIdmBenfDetDTO>().ReverseMap();
            CreateMap<IdmDsbdets, IdmDsbdetsDTO>().ReverseMap();


            CreateMap<TblIdmCondDet, RelaxationDTO>().ReverseMap();
            CreateMap<IdmOthdebitsDetails, IdmOthdebitsDetailsDTO>().ReverseMap(); // Added by Gagana on 27/10/2022
            CreateMap<TblUMOMaster, TblUMOMasterDto>().ReverseMap();
            CreateMap<TblDeptMaster, TblDeptMasterDto>().ReverseMap();
            CreateMap<TblDsbChargeMap, TblDsbChargeMapDto>().ReverseMap();

        }

    }
}
            