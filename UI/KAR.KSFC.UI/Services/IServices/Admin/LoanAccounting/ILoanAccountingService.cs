using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.IDM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting
{
    public interface ILoanAccountingService
    {
        Task<List<LoanAccountNumberDTO>> GetAllAccountingLoanNumber(string empID);
        #region Generic Dropdowns
        Task<IEnumerable<DropDownDTO>> GetAllTransactionTypes();
        #endregion
    }
}
