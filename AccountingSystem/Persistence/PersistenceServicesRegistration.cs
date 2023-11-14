using Application.Interface.Persistence.Adjustment;
using Application.Interface.Persistence.Bank;
using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Configuration;
using Application.Interface.Persistence.Document;
using Application.Interface.Persistence.GenerateBankFiles;
using Application.Interface.Persistence.Generic;
using Application.Interface.Persistence.GSTTDS;
using Application.Interface.Persistence.HeaderProfile;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Reconcile;
using Application.Interface.Persistence.TDS;
using Application.Interface.Persistence.Transactions;
using Application.Interface.Persistence.Vendor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Bank;
using Persistence.Repositories.Adjustment;
using Persistence.Repositories.Bank;
using Persistence.Repositories.BankFile;
using Persistence.Repositories.Bill;
using Persistence.Repositories.Configuration;
using Persistence.Repositories.Document;
using Persistence.Repositories.GenerateBankFiles;
using Persistence.Repositories.Generic;
using Persistence.Repositories.GSTTDS;
using Persistence.Repositories.HeaderProfile;
using Persistence.Repositories.Master;
using Persistence.Repositories.Payment;
using Persistence.Repositories.Reconcile;
using Persistence.Repositories.TDS;
using Persistence.Repositories.Transactions;
using Persistence.Repositories.UnitOfWorks;
using Persistence.Repositories.Vendor;

namespace Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AccountingDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("AccountingConnectionString")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //services.AddScoped<IBankStatementsRepository, BankStatementsRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<ITdsStatusRepository, TdsStatusRepository>();
            services.AddScoped<ITdsPaymentChallanRepository, TdsPaymentChallanRepository>();
            services.AddScoped<IBillTdsPaymentRepository, BillTdsPaymentRepository>();
            services.AddScoped<IGstdsPaymentChallanRepository, GsttdsPaymentChallanRepository>();
            services.AddScoped<IBillGsttdsPaymentRepository, BillGsttdsPaymentRepository>();
            services.AddScoped<IQuarterlyTdsPaymentChallanRepository, QuarterlyTdsPaymentChallanRepository>();
            services.AddScoped<IGsttdsStatusRepository, GsttdsStatusRepository>();

            /// <summary>
            /// Author:Swetha M Date:05/06/2022
            /// Purpose: services  to get the vendor payment details
            /// </summary>
            /// <returns></returns>
            services.AddScoped<IPaymentRepository, PaymentRepository>();   
         
            services.AddScoped<IDocumentRepository, DocumentRepository>();

            services.AddScoped<IBankDetailsRepositoty, BankDetailsRepository>();
            services.AddScoped<ICommonMasterRepository, CommonMasterRepository>();
            services.AddScoped<IBankMasterRepository, BankMasterRepository>();
            services.AddScoped<IBankStatementsRepository, BankStatementsRepository>();
            services.AddScoped<IGenerateBankFileRepository, GenerateBankFileRepository>();
            services.AddScoped<IBankFileRepository, BankFileRepository>();
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
            services.AddScoped<IBankStatementInputRepository, BankStatementInputRepository>();
            services.AddScoped<IReconciliationRepository, ReconciliationRepository>();
            services.AddScoped<ITransactionsBenefitsRepository, TransactionsBenefitsRepository>();
            services.AddScoped<ITransactionsCessRepository, TransacttionsCessRepository>();
            services.AddScoped<ITransactionSummaryRepository, TransactionSummaryRepository>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<IHeaderProfileRepositoty, HeaderProfileRepository>();
            services.AddScoped<IAdjustmentRepository, AdjustmentRepository>();

            return services;
        }
    }
}
