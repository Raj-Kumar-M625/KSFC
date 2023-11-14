using KAR.KSFC.Components.Common.Dto.IDM;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule
{
    public interface IIdmService
    {
        Task<IEnumerable<LoanAccountNumberDTO>> GetAccountNumber(string EmpId, CancellationToken token);
    }
}
