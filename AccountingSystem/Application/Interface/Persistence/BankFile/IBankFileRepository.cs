using Application.Interface.Persistence.Generic;
using Domain.BankFile;
using Domain.Bill;
using Domain.GenarteBankfile;
using Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.BankFile
{
    public interface IBankFileRepository:IGenericRepository<BankFileUtrDetails>
    {
        Task<BankFileUtrDetails> AddBankUTRDetails(BankFileUtrDetails bankFileUTRDetails);
        IQueryable<BankFileUtrDetails> GetAll();
        Task<MappingGenBankFileUtrDetails> AddMappingForBankFile(List<MappingGenBankFileUtrDetails> entity);
        List<int> GetPaymnetIDByGenBankFileID (List<int> genBankFileID);

        List<string> GetBillreferenceIDByGenBankFileID(List<int> genBankFileID);

        Task<GenerateBankFile> GetGeneratedBankFielById ( int? genBankFielId);
      

    }
}
