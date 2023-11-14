using KAR.KSFC.Components.Common.Dto.IDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.LoanAllocationModule
{
    /// <summary>
    ///  Author: Gagana K; Module:Loan Alloaction; Date:28/09/2022
    /// </summary>
    public interface ILoanAllocationService
    {
        Task<IEnumerable<TblIdmDhcgAllcDTO>> GetAllLoanAllocationList(long accountNumber, CancellationToken token);
        Task<bool> UpdateLoanAllocationDetails(TblIdmDhcgAllcDTO AllocationDTO, CancellationToken token);
        Task<bool> CreateLoanAllocationDetails(TblIdmDhcgAllcDTO AllocationDTO, CancellationToken token);
        Task<bool> DeleteLoanAllocationDetails(TblIdmDhcgAllcDTO AllocationDTO, CancellationToken token);
    }
}
