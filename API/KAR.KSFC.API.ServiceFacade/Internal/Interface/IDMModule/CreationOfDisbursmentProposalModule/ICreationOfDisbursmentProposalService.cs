using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using KAR.KSFC.Components.Data.Models.DbModels;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.CreationOfDisbursmentProposalModule
{
    public interface ICreationOfDisbursmentProposalService
    {
        #region Recommended Disbursement Details
        Task<IEnumerable<IdmDsbdetsDTO>> GetAllRecomDisbursementDetails(long accountNumber, CancellationToken token);
        Task<IEnumerable<TblAllcCdTabDTO>> GetAllocationCodeDetails(CancellationToken token);
        Task<IEnumerable<TblIdmReleDetlsDTO>> GetRecomDisbursementReleaseDetails(CancellationToken token);
        Task<IdmDsbdetsDTO> UpdateRecomDisbursementDetail(IdmDsbdetsDTO idmDsbdetsDTO, CancellationToken token);
        #endregion


        #region disbursement proposal details
        Task<IEnumerable<TblIdmReleDetlsDTO>> GetAllProposalDetails(long accountNumber, CancellationToken token);
        Task<TblIdmReleDetlsDTO> CreateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsDTO, CancellationToken token);
        Task<TblIdmReleDetlsDTO> UpdateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsDTO, CancellationToken token);
        Task<TblIdmReleDetlsDTO> DeleteProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsDTO, CancellationToken token);

        #endregion

        #region Beneficiary Details
        Task<IEnumerable<TblIdmBenfDetDTO>> GetAllBeneficiaryDetails(long accountNumber, CancellationToken token);
        Task<TblIdmBenfDetDTO> UpdateBeneficiaryDetails(TblIdmBenfDetDTO beneficiaryDet, CancellationToken token);
        #endregion
    }
}
