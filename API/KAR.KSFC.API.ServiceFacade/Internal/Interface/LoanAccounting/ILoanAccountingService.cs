using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.LoanAccounting;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccounting
{
    public interface ILoanAccountingService
    {
        Task<IEnumerable<LoanAccountNumberDTO>> GetAccountNumber(CancellationToken token, string EmpId);

        Task<IEnumerable<CodeTableDTO>> GetCodetable(CancellationToken token);
    }
}
