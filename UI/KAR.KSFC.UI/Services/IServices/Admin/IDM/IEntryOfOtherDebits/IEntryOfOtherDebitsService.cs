using KAR.KSFC.Components.Common.Dto.IDM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM.IEntryOfOtherDebits
{
    public interface IEntryOfOtherDebitsService
    {
        Task<IEnumerable<IdmOthdebitsDetailsDTO>> GetAllOtherDebitsList(long accountNumber);
        Task<bool> UpdateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit);
        Task<bool> DeleteOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit);
        Task<bool> CreateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit);
        Task<bool> SubmitOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit);
    }
}
