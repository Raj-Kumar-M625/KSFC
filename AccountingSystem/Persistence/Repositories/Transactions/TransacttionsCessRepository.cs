using Application.Interface.Persistence.Transactions;
using Domain.CessTransactions;
using Domain.Transactions;
using MediatR;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Transactions
{
    public class TransacttionsCessRepository:GenericRepository<TransactionsCess>, ITransactionsCessRepository
    {
        private readonly AccountingDbContext _dbContext;
        private readonly IMediator _mediator;
        public TransacttionsCessRepository(AccountingDbContext dbContext, IMediator mediator) : base(dbContext)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<List<TransactionsCess>> UpdateListCessTransaction(List<TransactionsCess> transactionList)
        {
            try
            {
                foreach (var item in transactionList)
                {
                    var existingtransaction = _dbContext.TransactionsCess.Where(x => x.ID == item.ID).FirstOrDefault();
                    existingtransaction.IsMatched = item.IsMatched;
                    existingtransaction.Status = item.Status;
                    _dbContext.Update(existingtransaction);
                }
                await _dbContext.SaveChangesAsync();
                return transactionList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
