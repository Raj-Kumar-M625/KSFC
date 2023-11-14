using Application.Interface.Persistence.Generic;
using Domain.GenarteBankfile;
using Domain.GenerateBankfile;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.GenerateBankFiles
{
    public interface IGenerateBankFileRepository:IGenericRepository<GenerateBankFile>
    {
        Task<GenerateBankFile> AddGeneratedBankFile(GenerateBankFile genbankFile);
        Task<GenerateBankFileStatus> AddGeneratedBankFileStatus(GenerateBankFileStatus genbankFileStatus);
        Task<GenerateBankFileStatus> UpdateGeneratedBankFileStatus(List<GenerateBankFileStatus> genbankFileStatus);
        IQueryable<GenerateBankFileStatus> GetGenerateBankFileStatus(List<int> Id);
        IQueryable<GenerateBankFile> GetAllGenerateBankFile();
        Task<MappingPaymentGenerateBankFile> AddMappingGenerateBankFile(MappingPaymentGenerateBankFile entity);
        Task<List<MappingPaymentGenerateBankFile>> GetGeneratedBankFileById(int Id);
        List<MappingPaymentGenerateBankFile> GetGeneratedBankFile(int Id);
    }
}
