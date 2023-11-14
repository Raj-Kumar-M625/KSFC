using Application.Interface.Persistence.Reconcile;
using Domain.Bank;
using Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Reconcile
{
    public class ReconciliationRepository : GenericRepository<Reconciliation>, IReconciliationRepository
    {
        private readonly AccountingDbContext _dbContext;

        public ReconciliationRepository(AccountingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Reconciliation>> AddReconciliationList(List<Reconciliation> reconciledTransactions)
        {
            try
            {
                await _dbContext.AddRangeAsync(reconciledTransactions);
                await _dbContext.SaveChangesAsync();
                return reconciledTransactions;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Reconciliation>> GetReconciliationsByBankTransactionId(int bankTransactionId)
        {
            try
            {
                var result = (from ts in _dbContext.TransactionsSummary
                              where ts.IsMatched == true && ts.Status == "Matched"
                              select new TransactionsSummary
                              {
                                  ID = ts.ID,
                                  AccountNumber = ts.AccountNumber,
                                  Amount = ts.Amount,
                                  TransactionDate = ts.TransactionDate,
                                  TransactionGeneratedDate = ts.TransactionGeneratedDate,
                                  SystemName = ts.SystemName,
                                  TransactionDetailedType = ts.TransactionDetailedType,
                                  ChargeOrPayment = ts.ChargeOrPayment,
                                  PaymentMode = ts.PaymentMode,
                                  Status = ts.Status,
                                  IsMatched = ts.IsMatched,
                                  IsPicked = ts.IsPicked,
                                  UTRNumber = ts.UTRNumber

                              }).Union(from tc in _dbContext.TransactionsCess
                                       where tc.IsMatched == true && tc.Status == "Matched"
                                       select new TransactionsSummary
                                       {

                                           ID = tc.ID,
                                           AccountNumber = tc.AccountNumber,
                                           Amount = tc.Amount,
                                           TransactionDate = tc.TransactionDate,
                                           TransactionGeneratedDate = tc.TransactionGeneratedDate,
                                           SystemName = tc.SystemName,
                                           TransactionDetailedType = tc.TransactionDetailedType,
                                           ChargeOrPayment = tc.ChargeOrPayment,
                                           PaymentMode = tc.PaymentMode,
                                           Status = tc.Status,
                                           IsMatched = tc.IsMatched,
                                           IsPicked = tc.IsPicked,
                                           UTRNumber = tc.UTRNumber
                                       }).Union(from tb in _dbContext.TransactionsBenefits
                                                where tb.IsMatched == true && tb.Status == "Matched"
                                                select new TransactionsSummary
                                                {

                                                    ID = tb.ID,
                                                    AccountNumber = tb.AccountNumber,
                                                    Amount = tb.Amount,
                                                    TransactionDate = tb.TransactionDate,
                                                    TransactionGeneratedDate = tb.TransactionGeneratedDate,
                                                    SystemName = tb.SystemName,
                                                    TransactionDetailedType = tb.TransactionDetailedType,
                                                    ChargeOrPayment = tb.ChargeOrPayment,
                                                    PaymentMode = tb.PaymentMode,
                                                    Status = tb.Status,
                                                    IsMatched = tb.IsMatched,
                                                    IsPicked = tb.IsPicked,
                                                    UTRNumber = tb.UTRNumber
                                                });

                var reconciliationDetails = await (from r in _dbContext.Reconciliation.Include(x => x.BankTransactions)
                                                   join ot in result
                                                   on new { r.TransactionsId, r.SystemName } equals new { TransactionsId = ot.ID, ot.SystemName }
                                                   where r.BankTransactionsId == bankTransactionId && r.ReconcileStatus== "Matched"
                                                   select new Reconciliation
                                                   {
                                                       Id = r.Id,
                                                       BankTransactions = r.BankTransactions,
                                                       SystemName = r.SystemName,
                                                       TransactionDate = r.TransactionDate,
                                                       TransactionsId = r.TransactionsId,
                                                       BankTransactionsId = r.BankTransactionsId,
                                                       TransactionsSummary = ot,
                                                       TransactionDetailedType = ot.TransactionDetailedType,
                                                       Amount = ot.Amount,
                                                       ReconcileStatus = r.ReconcileStatus,
                                                       MatchedBy = r.MatchedBy,
                                                       MatchedDate = r.MatchedDate,
                                                       ChargeOrPayment = r.ChargeOrPayment,
                                                       CreatedBy = r.CreatedBy,
                                                       CreatedDate = r.CreatedDate,
                                                       IsActive = r.IsActive,
                                                   }).ToListAsync();
                return reconciliationDetails;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<int>> GetReconciliationsByBankTransactionIds(List<int> bankTransactionId)
        {
            try
            {
                var reconciliationDetails = await _dbContext.Reconciliation.Where(x => bankTransactionId.Contains(x.BankTransactionsId))
                    .Include(x => x.BankTransactions).ToListAsync();

                var groupedTransactions = reconciliationDetails.GroupBy(t => t.BankTransactionsId);
                var matchingBankTransactionIds = new List<int>();
                foreach (var group in groupedTransactions)
                {
                    var distinctStatuses = group.Select(t => t.ReconcileStatus).Distinct();
                    if (distinctStatuses.Count() == 1 && distinctStatuses.First() == "UnMatched")
                    {
                        matchingBankTransactionIds.Add(group.Key);
                    }
                }
                return matchingBankTransactionIds;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Reconciliation>> GetReconciliationsByTransactionIds(List<Reconciliation> selectedTransactions, int bankTransactionId)
        {
            try
            {
                var selectedTransactionsId = selectedTransactions.Select(x => x.TransactionsId).ToList();
                var selectedTransactionsType = selectedTransactions.Select(x => x.SystemName).ToList();
                var result = (from ts in _dbContext.TransactionsSummary
                              where selectedTransactionsId.Contains(ts.ID) && selectedTransactionsType.Contains(ts.SystemName)
                              && ts.Status == "Matched"
                              select new TransactionsSummary
                              {
                                  ID = ts.ID,
                                  AccountNumber = ts.AccountNumber,
                                  Amount = ts.Amount,
                                  TransactionDate = ts.TransactionDate,
                                  TransactionGeneratedDate = ts.TransactionGeneratedDate,
                                  SystemName = ts.SystemName,
                                  TransactionDetailedType = ts.TransactionDetailedType,
                                  ChargeOrPayment = ts.ChargeOrPayment,
                                  PaymentMode = ts.PaymentMode,
                                  Status = ts.Status,
                                  IsMatched = ts.IsMatched,
                                  IsPicked = ts.IsPicked,
                                  UTRNumber = ts.UTRNumber

                              }).Union(from tc in _dbContext.TransactionsCess
                                       where selectedTransactionsId.Contains(tc.ID) && selectedTransactionsType.Contains(tc.SystemName)
                                        && tc.Status == "Matched"

                                       select new TransactionsSummary
                                       {

                                           ID = tc.ID,
                                           AccountNumber = tc.AccountNumber,
                                           Amount = tc.Amount,
                                           TransactionDate = tc.TransactionDate,
                                           TransactionGeneratedDate = tc.TransactionGeneratedDate,
                                           SystemName = tc.SystemName,
                                           TransactionDetailedType = tc.TransactionDetailedType,
                                           ChargeOrPayment = tc.ChargeOrPayment,
                                           PaymentMode = tc.PaymentMode,
                                           Status = tc.Status,
                                           IsMatched = tc.IsMatched,
                                           IsPicked = tc.IsPicked,
                                           UTRNumber = tc.UTRNumber
                                       }).Union(from tb in _dbContext.TransactionsBenefits
                                                where selectedTransactionsId.Contains(tb.ID) && selectedTransactionsType.Contains(tb.SystemName)
                                                && tb.Status == "Matched"
                                                select new TransactionsSummary
                                                {

                                                    ID = tb.ID,
                                                    AccountNumber = tb.AccountNumber,
                                                    Amount = tb.Amount,
                                                    TransactionDate = tb.TransactionDate,
                                                    TransactionGeneratedDate = tb.TransactionGeneratedDate,
                                                    SystemName = tb.SystemName,
                                                    TransactionDetailedType = tb.TransactionDetailedType,
                                                    ChargeOrPayment = tb.ChargeOrPayment,
                                                    PaymentMode = tb.PaymentMode,
                                                    Status = tb.Status,
                                                    IsMatched = tb.IsMatched,
                                                    IsPicked = tb.IsPicked,
                                                    UTRNumber = tb.UTRNumber
                                                });

                var reconciliationDetails = await (from r in _dbContext.Reconciliation.Include(x => x.BankTransactions)
                                                   join ot in result
                                                   on new { r.TransactionsId, r.SystemName } equals new { TransactionsId = ot.ID, ot.SystemName }
                                                   where r.BankTransactionsId != bankTransactionId && r.ReconcileStatus == "Matched"
                                                   select new Reconciliation
                                                   {
                                                       Id = r.Id,
                                                       BankTransactions = r.BankTransactions,
                                                       SystemName = r.SystemName,
                                                       TransactionDate = r.TransactionDate,
                                                       TransactionsId = r.TransactionsId,
                                                       BankTransactionsId = r.BankTransactionsId,
                                                       TransactionsSummary = ot,
                                                       TransactionDetailedType = ot.TransactionDetailedType,
                                                       Amount = ot.Amount,
                                                       ReconcileStatus = r.ReconcileStatus,
                                                       MatchedBy = r.MatchedBy,
                                                       MatchedDate = r.MatchedDate,
                                                       ChargeOrPayment = r.ChargeOrPayment,
                                                       CreatedBy = r.CreatedBy,
                                                       CreatedDate = r.CreatedDate,
                                                       IsActive = r.IsActive,
                                                   }).ToListAsync();
                return reconciliationDetails;


            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<Reconciliation>> UpdateReconciledTransactions(List<Reconciliation> reconciledTransactions)
        {
            try
            {


                foreach (var item in reconciledTransactions)
                {
                    var existingReconciliation = _dbContext.Reconciliation.FirstOrDefault(r => r.Id == item.Id);
                    if (existingReconciliation != null)
                    {
                        existingReconciliation.IsActive = item.IsActive;
                        existingReconciliation.UpdatedDate = item.UpdatedDate;
                        existingReconciliation.UpdatedBy = item.UpdatedBy;
                        existingReconciliation.ReconciledDate = item.ReconciledDate;
                        existingReconciliation.ReconcileStatus = item.ReconcileStatus;

                        _dbContext.SaveChanges();
                    }
                }
                return reconciledTransactions;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
