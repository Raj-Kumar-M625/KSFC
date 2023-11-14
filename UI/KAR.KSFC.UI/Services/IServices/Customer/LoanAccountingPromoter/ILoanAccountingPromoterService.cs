using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.IDM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Customer.LoanAccountingPromoter
{
    public interface ILoanAccountingPromoterService
    {
        Task<List<LoanAccountNumberDTO>> GetAllAccountingLoanNumber();
        #region Generic Dropdowns
        //Task<IEnumerable<DropDownDTO>> GetAllTransactionTypes();
        #endregion
    }
}
