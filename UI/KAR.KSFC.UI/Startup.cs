using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Logging;
using KAR.KSFC.Components.Common.Logging.Enrichers;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Services;
using KAR.KSFC.UI.Services.Admin;
using KAR.KSFC.UI.Services.Admin.IDM;
using KAR.KSFC.UI.Services.Admin.IDM.AuditService;
using KAR.KSFC.UI.Services.Admin.IDM.DisbursementService;
using KAR.KSFC.UI.Services.Admin.IDM.InspectionOfUnitService;
using KAR.KSFC.UI.Services.Admin.IDM.UnitDetailsService;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Services.IServices.Admin;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ILegalDocumentationService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IAuditService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IDisbursementService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using KAR.KSFC.UI.Services.Admin.IDM.LegalDocumentationService;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting;
using KAR.KSFC.UI.Services.Admin.LoanAccounting;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.UI.Services.Admin.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.UI.Services.IServices.Customer.LoanAccountingPromoter;
using KAR.KSFC.UI.Services.Customer.LoanAccounting;
using KAR.KSFC.UI.Services.IServices.Customer.LoanAccountingPromoter.LoanRelatedReceipt;
using KAR.KSFC.UI.Services.Customer.LoanAccountingPromoter.LoanRelatedReceipt;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfSecurityandAquisitionAssetService;
using KAR.KSFC.UI.Services.Admin.IDM.CreationOfSecurityandAquisitionAssetService;
using KAR.KSFC.UI.Services.Admin.IDM.LoanAllocation;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ILoanAllocationService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfDisbursmentProposalService;
using KAR.KSFC.UI.Services.Admin.IDM.CreationOfDisbursmentProposal;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IEntryOfOtherDebits;
using KAR.KSFC.UI.Services.Admin.IDM.EntryOfOtherDebits;
using KAR.KSFC.UI.Security;

namespace KAR.KSFC.UI
{
    public class Startup
    {
        public static IStringLocalizer _e; // This is how we access language strings

        public static IConfiguration LocalConfig;
        public Startup(IWebHostEnvironment env)//IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // this is where we store apps configuration including language
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();

            Configuration = builder.Build();
            LocalConfig = Configuration;
            CurrentEnv = env;
        }

        public IConfiguration Configuration { get; }
        private IHostEnvironment CurrentEnv { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterLoggingService(CurrentEnv.EnvironmentName, new List<EnricherTypeEnum>() { EnricherTypeEnum.SessionPrams });
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();

            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(Convert.ToInt32(Configuration["SysConfig:SessionExpiryTimeInSeconds"]));//You can set Time
                options.Cookie.IsEssential = true;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(SetLogContext));
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddHttpClient("ksfcApi", c =>
            {
                c.BaseAddress = new Uri(Configuration["SysConfig:apiPath"]);
                c.DefaultRequestHeaders.Add("Username", Configuration["SysConfig:Username"]);
                c.DefaultRequestHeaders.Add("Password", Configuration["SysConfig:Password"]);
                c.DefaultRequestHeaders.Add("RequestId", Guid.NewGuid().ToString());

                // access the DI container
                var serviceProvider = services.BuildServiceProvider();
                // Find the HttpContextAccessor service
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                // Get the bearer token from the request context (header)
                var bearerToken = httpContextAccessor.HttpContext.Request
                                      .Headers["Authorization"]
                                      .FirstOrDefault(h => h.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase));
                // Add authorization if found
                if (bearerToken != null)
                    c.DefaultRequestHeaders.Add("Authorization", bearerToken);

            });
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IDscService, DscService>();
            services.AddScoped<IPanService, PanService>();            
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IEnquirySubmissionService, EnquirySubmissionService>();
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<IEnquiryService, EnquiryServices>();
            services.AddScoped<IIdmService, IdmService>();  // By RV on 20/05/22
            services.AddScoped<ICommonService, CommonService>();  // By RV on 18/08/22
            services.AddScoped<ILegalDocumentationService, LegalDocumentationService>();  // By RV on 26/08/22
            services.AddScoped<IDisbursementService, DisbursementService>();  // By MJ on 17/08/2022
            services.AddScoped<IUnitDetailsService, UnitDetailsService>();
            services.AddScoped<IAuditService, AuditService>();  // By GK on 18/08/2022
            services.AddScoped<IInspectionOfUnitService, InspectionOfUnitService>();  // By MJ on 25/08/2022
            services.AddScoped<ILoanAccountingService, LoanAccountingService>();  // By GK on 18/08/2022
            services.AddScoped<ILoanRelatedReceiptService, LoanRelatedReceiptService>();  // By GK on 18/08/2022
            services.AddScoped<ILoanAccountingPromoterService, LoanAccountingPromoterService>();  // By MJ on 18/08/2022
            services.AddScoped<ILoanRelatedReceiptPromService, LoanRelatedReceiptPromService>();  // By MJ on 18/08/2022
            services.AddScoped<ICreationOfSecurityandAquisitionAssetService, CreationOfSecurityandAquisitionAssetService>();
            services.AddScoped<ICreationOfDisbursmentProposalService, CreationOfDisbursmentPropsalService>();

            services.AddScoped<ILoanAllocationService, LoanAllocationService>();  // By GK on 27/09/2022  
            services.AddScoped<IEntryOfOtherDebitsService, EntryOfOtherDebitsService>();  // By GK on 27/10/2022 
            services.AddScoped<SessionManager>();

            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.AddTransient<EFStringLocalizerFactory>();
            services.AddSingleton<IConfiguration>(Configuration);
            //Start -- Add Localization
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("kn-IN")
            };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var requestLocalizationOptions = new RequestLocalizationOptions
                {
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures,
                };
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

            });
            //End -- Add Localization
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<DataProtectionPurposeStrings>(); // IDM
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(SetLogContext));
            });

            var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           //.AddJsonFile($"logsettings.{envName ?? ""}.json", optional: true)
           //.AddJsonFile($"logsettings.{"Development" ?? ""}.json", optional: true)
           .AddJsonFile($"logsettings.Development.json", optional: true)
           .Build();
            //services.RegisterMyLogger(config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EFStringLocalizerFactory localizerFactory)
        {
            _e = localizerFactory.Create(null);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (!env.IsDevelopment())
            {
                app.UseExceptionHandler(exceptionHandlerApp =>
                {
                    exceptionHandlerApp.Run(async context =>
                    {
                        //log the error details if the it is not logged in the error originator method.
                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();
                        await Task.Run(() => context.Response.Redirect(string.Format(Configuration["SysConfig:AppErrorPage"], context.Response.StatusCode)));
                    });
                });
            }
            app.UseStatusCodePages(async statusCodeContext =>
            {
                var response = statusCodeContext.HttpContext.Response;
                if (response.StatusCode >= 400 && response.StatusCode <= 599)
                {
                    await Task.Run(() => response.Redirect(string.Format(Configuration["SysConfig:AppErrorPage"],response.StatusCode)));
                }
            });

            app.UseHsts();
            // app.UseStatusCodePages();
            //localization stats here....
            app.UseRequestLocalization();
            //ends here for lang local
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            //Add JWToken to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWTToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });
            app.UseAuthentication();///authentication
            app.UseHttpsRedirection();///authentication

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "MyArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
