using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using KAR.KSFC.Components.Data.Models.DbModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfDisbursmentProposalService
{
    public interface ICreationOfDisbursmentProposalService
    {
        //#region Recommended Disbursement Details

        //Task<IEnumerable<IdmDsbdetsDTO>> GetAllRecomDisbursementDetails(long accountNumber);
        //Task<IEnumerable<TblAllcCdTabDTO>> GetAllocationCodeDetails();
        //Task<IEnumerable<TblIdmReleDetlsDTO>> GetRecomDisbursementReleaseDetails();
        //Task<bool> UpdateRecomDisbursementDetail(IdmDsbdetsDTO idmDsbdetsDTO);


        //#endregion

        #region disbursement proposal details
        Task<IEnumerable<TblIdmReleDetlsDTO>> GetAllProposalDetails(long? accountNumber);
        Task<bool> CreateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsdto); 
        Task<bool> UpdateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsdto);
        Task<bool> DeleteProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsdto);


        #endregion

        #region Beneficiary Details
        Task<IEnumerable<TblIdmBenfDetDTO>> GetAllBeneficiaryDetails(long accountNumber);
        Task<TblIdmBenfDetDTO> UpdateBeneficiaryDetails(TblIdmBenfDetDTO addr);
        #endregion

    }
}
