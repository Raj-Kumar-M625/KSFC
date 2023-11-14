using Application.DTOs.Bank;
using Domain.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Bank
{
    public interface IBankTransactionRepository
    {
        Task<IQueryable<BankTransactions>> GetBankTransactionList();
        Task<BankTransactions> GetBankTransactionById(int bankTransactionId);
        Task<BankTransactions> UpdateBankTransactionsStatus(BankTransactions bankTransaction);
    }
}
