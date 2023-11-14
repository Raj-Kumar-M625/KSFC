using KAR.KSFC.Components.Common.Dto.IDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccountingPromoter
{
    public interface ILoanAccountingPromoterService
    {
        Task<IEnumerable<LoanAccountNumberDTO>> GetAccountNumber(CancellationToken token, string EmpId);
    }
}
