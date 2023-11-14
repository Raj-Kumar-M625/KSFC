using Application.Interface.Persistence.Transactions;
using Domain.Transactions;
using MediatR;
using Persistence.Repositories.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
//using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Transactions
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly AccountingDbContext _dbContext;
        private readonly IMediator _mediator;
        public TransactionRepository(AccountingDbContext dbContext, IMediator mediator) : base(dbContext)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }
        public async Task<Transaction> AddTransaction(Transaction entity)
        {
            try
            {
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;

            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid casting. {0}", e.Message);
                return entity;
            }
        }

        public IQueryable<Transaction> GetListOfCessTransaction()
        {
            try
            {
                var res = from ts in _dbContext.TransactionsCess                          
                          select new Transaction
                          {
                              BillReferenceNo = ts.BillReferenceNo,
                              AccountNumber = ts.AccountNumber,
                              VendorName = ts.VendorName,
                              TransactionDate = ts.TransactionDate,
                              TransactionGeneratedDate = ts.TransactionGeneratedDate,
                              Amount = ts.Amount,
                              AssesmentYear = ts.AssesmentYear,
                              ID = ts.ID,
                              PAN_Number = ts.PAN_Number,
                              GSTIN_Number = ts.GSTIN_Number,
                              TAN_Number = ts.TAN_Number,
                              //TransactionType = ts.TransactionType,
                              Status = ts.Status,
                              ReferenceNumber = ts.ReferenceNumber,
                              UTRNumber = ts.UTRNumber,
                              CreatedBy = ts.CreatedBy,
                             // CodeValue = cm.CodeValue,
                          };

                return res;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IQueryable<Transaction> GetListOfTransaction()
        {
            try
            {

                var result = (from ts in _dbContext.Transactions                             
                              select new Transaction
                              {
                                  BillReferenceNo = ts.BillReferenceNo,
                                  AccountNumber = ts.AccountNumber,
                                  VendorName = ts.VendorName,
                                  TransactionDate = ts.TransactionDate,
                                  TransactionGeneratedDate = ts.TransactionGeneratedDate,
                                  TransactionType = ts.TransactionType,
                                  BillNo = ts.BillNo,
                                  Amount = ts.Amount,
                                  AssesmentYear = ts.AssesmentYear,
                                  ID = ts.ID,
                                  PAN_Number = ts.PAN_Number,
                                  GSTIN_Number = ts.GSTIN_Number,
                                  TAN_Number = ts.TAN_Number,
                                  TransactionRefNo = ts.TransactionRefNo,
                                  TransactionDetailedType = ts.TransactionDetailedType,
                                  Status = ts.Status,
                                  ReferenceNumber = ts.ReferenceNumber,
                                  UTRNumber = ts.UTRNumber,
                                  ReconcileStatus = ts.ReconcileStatus,
                                  CreatedBy = ts.CreatedBy,
                                  CodeValue = ts.TransactionDetailedType,
                              }).Union(
              from ts in _dbContext.TransactionsCess      
              select new Transaction
              {
                  BillReferenceNo = ts.BillReferenceNo,
                  AccountNumber = ts.AccountNumber,
                  VendorName = ts.VendorName,
                  TransactionDate = ts.TransactionDate,
                  TransactionGeneratedDate = ts.TransactionGeneratedDate,
                  TransactionType = ts.ChargeOrPayment,
                  BillNo = ts.BillNo,
                  Amount = ts.Amount,
                  AssesmentYear = ts.AssesmentYear,
                  ID = ts.ID,
                  PAN_Number = ts.PAN_Number,
                  GSTIN_Number = ts.GSTIN_Number,
                  TAN_Number = ts.TAN_Number,
                  TransactionRefNo = string.Empty,
                  TransactionDetailedType = ts.TransactionDetailedType,
                  Status = ts.Status,
                  ReconcileStatus = ts.Status,
                  ReferenceNumber = ts.ReferenceNumber,
                  UTRNumber = ts.UTRNumber,
                  CreatedBy = ts.CreatedBy,
                  CodeValue = string.Empty,
              }).Union(
              from ts in _dbContext.TransactionsBenefits             
              select new Transaction
              {
                  BillReferenceNo = ts.BillReferenceNo,
                  AccountNumber = ts.AccountNumber,
                  VendorName = ts.VendorName,
                  TransactionDate = ts.TransactionDate,
                  TransactionGeneratedDate = ts.TransactionGeneratedDate,
                  TransactionType = ts.ChargeOrPayment,
                  BillNo = ts.BillNo,
                  Amount = ts.Amount,
                  AssesmentYear = ts.AssesmentYear,
                  ID = ts.ID,
                  PAN_Number = ts.PAN_Number,
                  GSTIN_Number = ts.GSTIN_Number,
                  TAN_Number = ts.TAN_Number,
                  TransactionRefNo = string.Empty,
                  TransactionDetailedType = ts.TransactionDetailedType,
                  Status = ts.Status,
                  ReconcileStatus = ts.Status,
                  ReferenceNumber = ts.ReferenceNumber,
                  UTRNumber = ts.UTRNumber,
                  CreatedBy = ts.CreatedBy,
                  CodeValue = string.Empty,
              }).OrderByDescending(t => t.TransactionDate)
              .ThenByDescending(t => t.ID);
                return result;

            }
            catch (Exception e)
            {
                throw;
            }

        }

        public async Task<List<Transaction>> GetTransactionsBasedBillId(List<int> billIds)
        {
            try
            {
                var res = await _dbContext.Transactions.Where(x => x.ReferenceType == "Bill" && billIds.Contains((int)x.ReferenceNumber)
                && x.Amount <0 && x.TransactionType=="C").ToListAsync();
                return res;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<Transaction>> GetTransactionsBasedRefId(List<int> billIds)
        {
            try
            {
                var res = await _dbContext.Transactions.Where(x => x.ReferenceType == "Bill" && billIds.Contains((int)x.ReferenceNumber)).ToListAsync();
                return res;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<Transaction>> GetTransactionsBasedRefIdForGSTTDS(int gstTdsPaymentChallanId)
        {
            try
            {
                var res = await _dbContext.Transactions.Where(x => x.ReferenceType == "GSTTDSPaymentchallan" && gstTdsPaymentChallanId==x.ReferenceNumber).ToListAsync();
                return res;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<Transaction>> GetTransactionsBasedRefIdForTDS(int tdsPaymentChallanId)
        {
            try
            {
                var res = await _dbContext.Transactions.Where(x => x.ReferenceType == "TDSPaymentchallan" && tdsPaymentChallanId==x.ReferenceNumber).ToListAsync();
                return res;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<Transaction>> UpdateTransactionReconcileStatus(List<Transaction> transactionList)
        {
            try
            {
                foreach (var item in transactionList)
                {
                    _dbContext.Update(item);
                }
                await _dbContext.SaveChangesAsync();
                return transactionList; 

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
