using Application.Interface.Persistence.Transactions;
using Domain.Transactions;
using MediatR;
using Persistence.Repositories.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Application.DTOs.Filters;

namespace Persistence.Repositories.Transactions
{
    public class TransactionSummaryRepository : GenericRepository<TransactionsSummary>, ITransactionSummaryRepository
    {
        private readonly AccountingDbContext _dbContext;
        private readonly IMediator _mediator;
        public TransactionSummaryRepository(AccountingDbContext dbContext, IMediator mediator) : base(dbContext)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<TransactionsSummary> AddTransactionSummary(TransactionsSummary entity)
        {
            try
            {
                await _dbContext.TransactionsSummary.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TransactionsSummary> AddTransactionSummaryDetails(List<TransactionsSummary> entity)
        {
            try
            {
                await _dbContext.TransactionsSummary.AddRangeAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IQueryable<TransactionsSummary>> GetAllTransactions(ReconcileDto reconcileDto)
        {
            try
            {
                if (reconcileDto.TransactionType != null)
                {
                    switch (reconcileDto.TransactionType)
                    {
                        case "CESS":
                            var tramsactions = (from tc in _dbContext.TransactionsCess
                                                where ((reconcileDto.StartDate == null && reconcileDto.EndDate == null) ||
                                                (tc.TransactionGeneratedDate >= reconcileDto.StartDate && tc.TransactionGeneratedDate <= reconcileDto.EndDate))
                                                && ((reconcileDto.PayableAmount == 0 && reconcileDto.PayableMaxAmount == 0)
                                                || (tc.Amount >= reconcileDto.PayableAmount && tc.Amount <= reconcileDto.PayableMaxAmount))
                                                && (reconcileDto.Status == null || tc.Status == reconcileDto.Status)
                                                && tc.Status != "Reconciled"
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
                                                });
                            return tramsactions;
                        case "Benefits":
                            var beniftTransactions = (from tb in _dbContext.TransactionsBenefits
                                                      where ((reconcileDto.StartDate == null && reconcileDto.EndDate == null)
                                                      || (tb.TransactionGeneratedDate >= reconcileDto.StartDate && tb.TransactionGeneratedDate <= reconcileDto.EndDate))
                                                      && ((reconcileDto.PayableAmount == 0 && reconcileDto.PayableMaxAmount == 0)
                                                      || (tb.Amount >= reconcileDto.PayableAmount && tb.Amount <= reconcileDto.PayableMaxAmount))
                                                      && (reconcileDto.Status == null || tb.Status == reconcileDto.Status)
                                                      && tb.Status != "Reconciled"
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
                            return beniftTransactions;
                        case "Accounts":
                            var accountsTransaction = (from ts in _dbContext.TransactionsSummary
                                                       where ((reconcileDto.StartDate == null && reconcileDto.EndDate == null)
                                                      || (ts.TransactionGeneratedDate >= reconcileDto.StartDate && ts.TransactionGeneratedDate <= reconcileDto.EndDate))
                                                      && ((reconcileDto.PayableAmount == 0 && reconcileDto.PayableMaxAmount == 0)
                                                      || (ts.Amount >= reconcileDto.PayableAmount && ts.Amount <= reconcileDto.PayableMaxAmount))
                                                      && (reconcileDto.Bankname == null || ts.BankName == reconcileDto.Bankname)
                                                      && (reconcileDto.AccountNumber == null || ts.AccountNumber == reconcileDto.AccountNumber)
                                                      && (reconcileDto.Status == null || ts.Status == reconcileDto.Status)
                                                      && ts.Status != "Reconciled"
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

                                                       });
                            return accountsTransaction;

                    }
                }
                var result = (from ts in _dbContext.TransactionsSummary
                              where ((reconcileDto.StartDate == null && reconcileDto.EndDate == null)
                             || (ts.TransactionGeneratedDate >= reconcileDto.StartDate && ts.TransactionGeneratedDate <= reconcileDto.EndDate))
                             && ((reconcileDto.PayableAmount == 0 && reconcileDto.PayableMaxAmount == 0)
                             || (ts.Amount >= reconcileDto.PayableAmount && ts.Amount <= reconcileDto.PayableMaxAmount))
                             && (reconcileDto.Bankname == null || ts.BankName == reconcileDto.Bankname)
                             && (reconcileDto.AccountNumber == null || ts.AccountNumber == reconcileDto.AccountNumber)
                             && (reconcileDto.Status == null || ts.Status == reconcileDto.Status)
                             && ts.Status != "Reconciled"
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
                                       where ((reconcileDto.StartDate == null && reconcileDto.EndDate == null) ||
                                       (tc.TransactionGeneratedDate >= reconcileDto.StartDate && tc.TransactionGeneratedDate <= reconcileDto.EndDate))
                                       && ((reconcileDto.PayableAmount == 0 && reconcileDto.PayableMaxAmount == 0)
                                       || (tc.Amount >= reconcileDto.PayableAmount && tc.Amount <= reconcileDto.PayableMaxAmount))
                                       && (reconcileDto.Status == null || tc.Status == reconcileDto.Status)
                                       && tc.Status != "Reconciled"
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
                                                where ((reconcileDto.StartDate == null && reconcileDto.EndDate == null)
                                                || (tb.TransactionGeneratedDate >= reconcileDto.StartDate && tb.TransactionGeneratedDate <= reconcileDto.EndDate))
                                                && ((reconcileDto.PayableAmount == 0 && reconcileDto.PayableMaxAmount == 0)
                                                || (tb.Amount >= reconcileDto.PayableAmount && tb.Amount <= reconcileDto.PayableMaxAmount))
                                                && (reconcileDto.Status == null || tb.Status == reconcileDto.Status)
                                                && tb.Status != "Reconciled"
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

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransactionsSummary>> UpdateListOfTransactions(List<TransactionsSummary> transactionsSummary)
        {
            try
            {
                foreach (var item in transactionsSummary)
                {
                    var existingtransaction = _dbContext.TransactionsSummary.Where(x => x.ID == item.ID).FirstOrDefault();
                    existingtransaction.IsMatched = item.IsMatched;
                    existingtransaction.Status = item.Status;
                    _dbContext.Update(existingtransaction);
                }
                await _dbContext.SaveChangesAsync();
                return transactionsSummary;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
