using Application.Interface.Persistence.Transactions;
using Domain.TransactionsBenefits;
using MediatR;
using Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Transactions
{
    public class TransactionsBenefitsRepository : GenericRepository<TransactionsBenefits>, ITransactionsBenefitsRepository
    {
        private readonly AccountingDbContext _dbContext;
        private readonly IMediator _mediator;
        public TransactionsBenefitsRepository(AccountingDbContext dbContext, IMediator mediator) : base(dbContext)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<List<TransactionsBenefits>> UpdateListtOfBenefitsTransaction(List<TransactionsBenefits> transactionList)
        {
            try
            {
                foreach (var item in transactionList)
                {
                    var existingtransaction = _dbContext.TransactionsBenefits.Where(x => x.ID == item.ID).FirstOrDefault();
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
