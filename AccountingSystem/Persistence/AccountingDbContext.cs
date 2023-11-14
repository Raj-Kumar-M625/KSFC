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
using Domain.User;
using Domain.Vendor;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AccountingDbContext : DbContext
    {
        public AccountingDbContext()
        {

        }
        public AccountingDbContext(DbContextOptions<AccountingDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountingDbContext).Assembly);
            //modelBuilder.Entity<Bills>().HasOne(b => b.BillStatus).WithOne(s => s.Bill).HasForeignKey<BillStatus>(b => b.ID);

            //modelBuilder.Entity<Bills>().HasOne(b => b.TDSStatus).WithOne(s => s.Bill).HasForeignKey<TDSStatus>(b => b.ID);
            // modelBuilder.Entity<Payments>().HasOne(b => b.b).WithOne(p => p.Payments).HasPrincipalKey<Payments>(p => p.BillsID).HasForeignKey<Bills>(b => b.ReferenceNo);
            modelBuilder.Entity<Reconciliation>()
    .HasOne(r => r.BankTransactions)
    .WithMany(b => b.Reconciliation)
    .HasForeignKey(r => r.BankTransactionsId)
    .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MappingAdvancePayment>()
.HasOne(r => r.Payments)
.WithMany(b => b.MappingAdvancePayment)
.HasForeignKey(r => r.PaymentsID)
.OnDelete(DeleteBehavior.Restrict);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                var connectionString = "Server=13.126.250.206,1433;Database=Dev_Accounts;User Id=sa;Password=Micro@321;MultipleActiveResultSets=True";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<BankStatementInputTransaction> BankStatementInputTransaction { get; set; }
        public DbSet<Addresses> Address { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<VendorBalance> VendorBalance { get; set; }
        public DbSet<Vendors> Vendor { get; set; }
        public DbSet<VendorBankAccount> VendorBankAccount { get; set; }
        public DbSet<VendorDefaults> VendorDefaults { get; set; }
        public DbSet<VendorPerson> VendorPerson { get; set; }
        public DbSet<CommonMaster> CommonMaster { get; set; }
        public DbSet<BankMaster> BankMaster { get; set; }
        public DbSet<BranchMaster> BranchMaster { get; set; }
        public DbSet<BankDetails> BankDetails { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<Bills> Bill { get; set; }
        public DbSet<BillStatus> BillStatus { get; set; }
        public DbSet<BillItems> BillItems { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<BillPayment> BillPayment { get; set; }
        public DbSet<GenerateBankFile> GenerateBankFile { get; set; }
        public DbSet<BankFileUtrDetails> BankFileUTRDetails { get; set; }
        public DbSet<MappingGenBankFileUtrDetails> MappingGenBankFileUTRDetails { get; set; }
        public DbSet<GenerateBankFileStatus> GenerateBankFileStatus { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionsCess> TransactionsCess { get; set; }
        public DbSet<TransactionsBenefits> TransactionsBenefits { get; set; }

        public DbSet<MappingPaymentGenerateBankFile> MappingPaymentGenerateBankFile { get; set; }
        // public DbSet<BillPaymentDetails> BillPaymentDetails { get; set; }
        public DbSet<TdsPaymentChallan> TDSPaymentChallan { get; set; }
        public DbSet<GsttdsPaymentChallan> GSTTDSPaymentChallan { get; set; }
        public DbSet<BillTdsPayment> BillTDSPayment { get; set; }
        public DbSet<BillGsttdsPayment> BillGSTTDSPayment { get; set; }
        public DbSet<GsttdsStatus> GSTTDSStatus { get; set; }
        public DbSet<TdsStatus> TDSStatus { get; set; }
        public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public DbSet<AspNetRoles> AspNetRoles { get; set; }
        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<QuarterlyTdsPaymentChallan> QuarterlyTDSPaymentChallan { get; set; }
        public DbSet<MappingTdsQuarterChallan> MappingTDSQuarterChallan { get; set; }
        public DbSet<BankTransactions> BankTransactions { get; set; }
        public DbSet<Reconciliation> Reconciliation { get; set; }
        public DbSet<BankStatementInput> BankStatementInput { get; set; }
        public DbSet<TransactionsSummary> TransactionsSummary { get; set; }
        public DbSet<HeaderProfileDetails> HeaderProfileDetails { get; set; }
        public DbSet<Adjustments> Adjustment { get; set; }
        public DbSet<AdjustmentStatus> AdjustmentStatus { get; set; }
        public DbSet<MappingAdvancePayment> MappingAdvancePayment { get; set; }

    }
}
