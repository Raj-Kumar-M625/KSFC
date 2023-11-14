using System;
using System.Text;

using KAR.KSFC.API.ServiceFacade.External.Interface;
using KAR.KSFC.API.ServiceFacade.External.Service;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.SecurityDocumentDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.AuditModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.DisbursementModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.InspectionOfUnitModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.LegalDocumentationModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.UnitDetailsModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Service;
using KAR.KSFC.API.ServiceFacade.Internal.Service.AdminModule;
using KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.ProjectDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.SecurityDocumentDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.UnitDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.AuditModule;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.DisbursementModule;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.LegalDocumentationModule;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.UnitDetailsModule;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.InspectionOfUnitModule;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Data.DatabaseContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.LoanAllocationModule;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.LoanAllocationModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccounting;
using KAR.KSFC.API.ServiceFacade.Internal.Service.LoanAccounting;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.API.ServiceFacade.Internal.Service.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccountingPromoter;
using KAR.KSFC.API.ServiceFacade.Internal.Service.LoanAccountingPromoter;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccountingPromoter.LoanRelatedReceiptProm;
using KAR.KSFC.API.ServiceFacade.Internal.Service.LoanAccountingPromoter.LoanRelatedReceiptProm;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.CreationOfDisbursmentProposalModule;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.CreationOfDisbursmentProposal;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.EntryOfOtherDebitsModule;
using KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.EntryOfOtherDebits;

namespace KAR.KSFC.API.ServiceFacade
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            services.AddScoped<IMobileService, MobileService>();
            services.AddScoped<IDisposable, DbFactory>();
            services.AddScoped<IPanService, PanService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<IUserLogHistory, UserLogHistory>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IDscService, DscService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IBasicDetails, BasicDetailsService>();
            services.AddScoped<IAddressDetails, AddressDetailsService>();
            services.AddScoped<IBankDetails, BankDetailsService>();
            services.AddScoped<IRegistrationDetails, RegistrationDetailsService>();
            services.AddScoped<IPromoterDetails, PromoterDetailsService>();
            services.AddScoped<IPromoterLiabilityDetails, PromoterLiabilityDetailsService>();
            services.AddScoped<IPromoterLiabilityNetWorth, PromoterLiabilityNetWorthService>();
            services.AddScoped<IPromoterAssetsNetWorthDetails, PromoterAssetsNetWorthDetailsService>();
            services.AddScoped<IGuarantorDetails, GuarantorDetailsService>();
            services.AddScoped<IGuarantorLiabilityDetails, GuarantorLiabilityDetailsService>();
            services.AddScoped<IGuarantorAssetsNetWorthDetails, GuarantorAssetsNetWorthDetailsService>();
            services.AddScoped<IGuarantorLiabilityNetWorth, GuarantorLiabilityNetWorthService>();
            services.AddScoped<ISisterConcernFinancialDetails, SisterConcernFinancialDetailsService>();
            services.AddScoped<ISisterConcernDetails, SisterConcernDetailsService>();
            services.AddScoped<IEnquiryHomeService, EnquiryHomeService>();
            services.AddScoped<IWorkingCapitalDetails, WorkingCapitalDetailsService>();
            services.AddScoped<IMeansofFinance, MeansofFinanceService>();
            services.AddScoped<ISecurityDetails, SecurityDetailsService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IAssignOffice, AssignOfficeService>();
            services.AddScoped<IProjectFinancialDetails, ProjectFinancialDetailsService>();
            services.AddScoped<IProjectCostDetails, ProjectCostService>();
            services.AddScoped<IIdmService, IdmService>(); //By RV on 20-05-2022
            services.AddScoped<IIdmFileService, IdmFileService>(); //By RV on 18-08-2022
            services.AddScoped<ILegalDocumentationService, LegalDocumentationService>(); //By RV on 26-08-2022
            services.AddScoped<IDisbursementService, DisbursementService>(); //By MJ on 17-08-2022
            services.AddScoped<IInspectionOfUnitService, InspectionOfUnitService>(); //By MJ on 25-08-2022
            services.AddScoped<IUnitDetailsService, UnitDetailsService>(); //By SK on 24-08-2022
            services.AddScoped<IAuditService, AuditService>(); //By GK on 24/08/2022
            services.AddScoped<ILoanAccountingService, LoanAccountingService>(); //By GK on 14/09/2022
            services.AddScoped<ILoanRelatedReceiptService, LoanRelatedReceiptService>(); //By GK on 14/09/2022
            services.AddScoped<ILoanAccountingPromoterService, LoanAccountingPromoterService>(); //By MJ on 14/09/2022
            services.AddScoped<ILoanRelatedReceiptPromService, LoanRelatedReceiptPromService>(); //By  DP on 23/09/2022
          
            services.AddScoped<ICreationOfSecurityandAquisitionAsset, CreationOfSecurityandAquisitionAsset>();
            services.AddScoped<ICreationOfDisbursmentProposalService, CreationOfDisbursmentProposal>();
            //services.AddScoped<>(); //Kiran Vasishta TS on 28-SEP-2022
            services.AddScoped<IEntryOfOtherDebitsService, EntryOfOtherDebitsService>(); //By GK on 27/10/2022
            services.AddScoped<ILoanAllocationService, LoanAllocationService>(); //By GK on 28/09/2022
            services.AddScoped<IEmployeeService,EmployeeService>();
            services.AddScoped<ModuleService>();
            services.AddScoped<RoleService>();
            services.AddScoped<AttributeService>();
            services.AddScoped<AttributeUnitService>();
            services.AddScoped<AttributeUnitOperatorService>();
            services.AddScoped<SubAttributeService>();
            services.AddScoped<ChairService>();
            services.AddScoped<DelegationOfPowerService>();
            services.AddScoped<DelegationOfPowerHistoryService>();
            services.AddScoped<EmployeeChairDetailService>();
            services.AddScoped<EmployeeChairHistoryService>();
            services.AddScoped<HierarchyService>();
            services.AddScoped<ProjectCostService>();
            services.AddScoped<SisterConcernDetailsService>();

            services.AddTransient<Func<ServiceEnum, IGenericAdminModuleRepository>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case ServiceEnum.ModuleService:
                        return serviceProvider.GetService<ModuleService>();
                    case ServiceEnum.RoleService:
                        return serviceProvider.GetService<RoleService>();
                    case ServiceEnum.AttributeService:
                        return serviceProvider.GetService<AttributeService>();
                    case ServiceEnum.AttributeUnitService:
                        return serviceProvider.GetService<AttributeUnitService>();
                    case ServiceEnum.AttributeUnitOperatorService:
                        return serviceProvider.GetService<AttributeUnitOperatorService>();
                    case ServiceEnum.SubAttributeService:
                        return serviceProvider.GetService<SubAttributeService>();
                    case ServiceEnum.ChairMasterService:
                        return serviceProvider.GetService<ChairService>();
                    case ServiceEnum.DelegationOfPowerService:
                        return serviceProvider.GetService<DelegationOfPowerService>();
                    case ServiceEnum.DelegationOfPowerHistoryService:
                        return serviceProvider.GetService<DelegationOfPowerHistoryService>();
                    case ServiceEnum.EmployeeChairDetailsService:
                        return serviceProvider.GetService<EmployeeChairDetailService>();
                    case ServiceEnum.EmployeeChairHistoryDetailService:
                        return serviceProvider.GetService<EmployeeChairHistoryService>();
                    case ServiceEnum.HierarchyChairMasterService:
                        return serviceProvider.GetService<HierarchyService>();
                    default:
                        return null;
                }
            });

            return services;
        }

        public static IServiceCollection JwtService(this IServiceCollection services, IConfiguration configuration)
        {
            //Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //Addding Jwt Bearer
            .AddJwtBearer(options =>
            {
                //Suppose a token is not passed through header and passed through the url query string
                //options.Events = new JwtBearerEvents()
                //{
                //    OnMessageReceived = context =>
                //    {
                //        if (context.Request.Query.ContainsKey("access_token"))
                //        {
                //            context.Token = context.Request.Query["access_token"];
                //        }
                //        return Task.CompletedTask;
                //    }
                //};

                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };

                //   options.Events = new JwtBearerEvents
                //{
                //    OnAuthenticationFailed = context =>
                //    {
                //        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                //        {
                //            context.Response.Headers.Add("Token-Expired", "true");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};

            });
            return services;
        }

    }
}
