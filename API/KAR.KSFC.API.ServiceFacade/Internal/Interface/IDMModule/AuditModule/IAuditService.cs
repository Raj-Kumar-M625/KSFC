using KAR.KSFC.Components.Common.Dto.IDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.AuditModule
{
    // <Summary>
    // Author: Gagana K; Module:AuditClearance; Date: 18/08/2022
    // <summary>
    public interface IAuditService
    {

        #region AuditClearance
        Task<IEnumerable<IdmAuditDetailsDTO>> GetAllAuditClearanceListAsync(long accountNumber, CancellationToken token);
        Task<bool> UpdateAuditClearanceDetails(IdmAuditDetailsDTO AuditDTO, CancellationToken token);
        Task<bool> CreateAuditClearanceDetails(IdmAuditDetailsDTO AuditDTO, CancellationToken token);
        Task<bool> DeleteAuditClearanceDetails(IdmAuditDetailsDTO AuditDTO, CancellationToken token);
        #endregion
    }
}
