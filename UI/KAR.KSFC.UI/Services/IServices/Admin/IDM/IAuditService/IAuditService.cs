using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Data.Models.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM.IAuditService
{
    // <summary>
    //  Author: Gagana K; Module:Audit Clearance ; Date:18/08/2022
    // </summary>
    public interface IAuditService
    {
 
        #region AuditClearance

        Task<IEnumerable<IdmAuditDetailsDTO>> GetAllAuditClearanceList(long accountNumber);
        Task<bool> UpdateAuditClearanceDetails(IdmAuditDetailsDTO addr);
        Task<bool> CreateAuditClearanceDetails(IdmAuditDetailsDTO addr);
        Task<bool> DeleteAuditClearanceDetails(IdmAuditDetailsDTO dto);
        #endregion
    }
}
