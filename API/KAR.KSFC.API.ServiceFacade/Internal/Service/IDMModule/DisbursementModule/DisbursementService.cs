using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.DisbursementModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.DisbursementModule
{
    public class DisbursementService : IDisbursementService
    {
        private readonly IEntityRepository<TblAppLoanMast> _loanRepository;
        private readonly IEntityRepository<TblIdmCondDet> _condRepository;
        private readonly IEntityRepository<TblIdmDsbFm813> _form8and13Repository;
        private readonly IEntityRepository<TblIdmSidbiApproval> _sidbirepository;
       
        private readonly IEntityRepository<TblAddlCondDet> _additionalCondition;
        private readonly IEntityRepository<TblIdmFirstInvestmentClause> _FICRepository;
        private readonly IEntityRepository<TblIdmCondDet> _relaxRepository;
        private readonly UserInfo _userInfo;
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public DisbursementService(IEntityRepository<TblAppLoanMast> loanRepository, IUnitOfWork work,
                                    IMapper mapper, IEntityRepository<TblIdmCondDet> condRepository, IEntityRepository<TblIdmDsbFm813> form8and13Repository, 
                                    IEntityRepository<TblIdmSidbiApproval> sidbirepository,
                                    IEntityRepository<TblAddlCondDet> additionalCondition, IEntityRepository<TblIdmFirstInvestmentClause> FICRepository,UserInfo userInfo,
                                    IEntityRepository<TblIdmCondDet> relaxRepository)
        {
            _loanRepository = loanRepository;
            _work = work;
            _mapper = mapper;
            _condRepository = condRepository;
            _sidbirepository = sidbirepository;
            _form8and13Repository = form8and13Repository;
            _additionalCondition = additionalCondition;
            _FICRepository = FICRepository;
            _userInfo = userInfo;
            _relaxRepository = relaxRepository;
        }

        public async Task<IEnumerable<LoanAccountNumberDTO>> GetAccountNumber(CancellationToken token)
        {
            var data = await _loanRepository.FindByMatchingPropertiesAsync(token, x => x.IsActive == true,
                incOff => incOff.OffcCdtab,
                incUnit => incUnit.TblUnitMast,
                 incLoanAcc => incLoanAcc.TblIdmLegalWorkflow 
                ).ConfigureAwait(false);

            return _mapper.Map<List<LoanAccountNumberDTO>>(data);
        }
        #region Disbursement Condition
        public async Task<IEnumerable<LDConditionDetailsDTO>> GetDisbursementConditionListAsync(long accountNumber, CancellationToken token)
        {
            var data = await _condRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false,
                Condstg => Condstg.TblCondStageMast, Condtype => Condtype.TblCondTypeCdtab).ConfigureAwait(false);
            return _mapper.Map<List<LDConditionDetailsDTO>>(data);
        }

        public async Task<bool> UpdateDisbursementConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            var currentDetails = await _condRepository.FirstOrDefaultByExpressionAsync(x => x.CondDetId == CondtionDTO.CondDetId, token);
            currentDetails.IsDeleted = true;
            currentDetails.IsActive = false;
            currentDetails.ModifiedBy = _userInfo.Name;
            currentDetails.ModifiedDate = DateTime.UtcNow;
            await _condRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);


            var basicDetails = _mapper.Map<TblIdmCondDet>(CondtionDTO);
            basicDetails.CondDetId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate = DateTime.UtcNow;

            var response = await _condRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if(response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
  
        public async Task<bool> CreateDisbursementConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmCondDet>(CondtionDTO);
           
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _condRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteDisbursementCondtionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmCondDet>(CondtionDTO);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var response = await _condRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Sidbi Approval

        public async Task<IdmSidbiApprovalDTO> GetSidbiApprovalAsync(long accountNumber, CancellationToken token)
        {

            var sidbi = await _sidbirepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            


            var result = sidbi.FirstOrDefault();
           
            return _mapper.Map<IdmSidbiApprovalDTO>(result);
            
        }
       public async Task<bool> UpdateSidbiApprovalDetails(IdmSidbiApprovalDTO sidbi, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmSidbiApproval>(sidbi);
            var currentDetails = await _sidbirepository.FirstOrDefaultByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false && x.SidbiApprId == basicDetails.SidbiApprId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                await _sidbirepository.UpdateAsync(currentDetails,token).ConfigureAwait(false);
            }

            basicDetails.SidbiApprId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var response = await _sidbirepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Form8AndForm13
        public async Task<IEnumerable<Form8AndForm13DTO>> GetAllForm8AndForm13ListAsync(long accountNumber, CancellationToken token)
        {
            var data = await _form8and13Repository.FindByMatchingPropertiesAsync(token, x => x.DF813LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false,
                FormType => FormType.Tblfm8fm13CDTab);
            return _mapper.Map<List<Form8AndForm13DTO>>(data);
        }

        public async Task<bool> UpdateForm8AndForm13Details(Form8AndForm13DTO form8and13DTO, CancellationToken token)
        {
            var currentDetails = await _form8and13Repository.FirstOrDefaultByExpressionAsync(x => x.DF813Id == form8and13DTO.DF813Id, token);
            currentDetails.IsDeleted = true;
            currentDetails.ModifiedBy   = _userInfo.Name;
            currentDetails.ModifiedDate = DateTime.UtcNow;
            currentDetails.IsActive = false;
            await _form8and13Repository.UpdateAsync(currentDetails, token).ConfigureAwait(false);


            var basicDetails = _mapper.Map<TblIdmDsbFm813>(form8and13DTO);
            basicDetails.DF813Id = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate = DateTime.UtcNow;

            var response = await _form8and13Repository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateForm8AndForm13Details(Form8AndForm13DTO form8and13DTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDsbFm813>(form8and13DTO);
     
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _form8and13Repository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteForm8AndForm13Details(Form8AndForm13DTO form8and13DTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDsbFm813>(form8and13DTO);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _form8and13Repository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region AdditionalConditiom
        public async Task<IEnumerable<AdditionConditionDetailsDTO>> GetAdditionConditionListAsync(long accountNumber, CancellationToken token)
        {

            var data = await _additionalCondition.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true,
                condmst => condmst.TblCondStgCdtab);
           
            return _mapper.Map<List<AdditionConditionDetailsDTO>>(data);

        }
        

        public async Task<bool> CreateAdditionalConditionDetails(AdditionConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblAddlCondDet>(CondtionDTO);
         
            basicDetails.IsActive = true;
            basicDetails.CreatedDate = DateTime.UtcNow;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _additionalCondition.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateAdditionalConditionDetails(AdditionConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            var currentDetails = await _additionalCondition.FirstOrDefaultByExpressionAsync(x => x.AddCondId == CondtionDTO.AddCondId, token);
            currentDetails.IsDeleted = true;
            currentDetails.ModifiedBy = _userInfo.Name;
            currentDetails.ModifiedDate = DateTime.UtcNow;
            currentDetails.IsActive = false;
            await _additionalCondition.UpdateAsync(currentDetails, token).ConfigureAwait(false);


            var basicDetails = _mapper.Map<TblAddlCondDet>(CondtionDTO);
            basicDetails.AddCondId = 0;
            basicDetails.IsActive = true;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;

            var response = await _additionalCondition.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> DeleteAdditionalCondtionDetails(AdditionConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblAddlCondDet>(CondtionDTO);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _additionalCondition.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region FirstInvestmentClause

        public async Task<IdmFirstInvestmentClauseDTO>GetFirstInvestmentClauseAsync(long accountNumber, CancellationToken token)
        {
            var FICdata = await _FICRepository.FindByMatchingPropertiesAsync(token, x => x.DCFICLoanACC == accountNumber && x.IsActive==true && x.IsDeleted==false).ConfigureAwait(false);
            var result = FICdata.LastOrDefault();
            return _mapper.Map<IdmFirstInvestmentClauseDTO>(result);
           
        }
        public async Task<bool> UpdateAllFirstInvestmentClauseDetails(IdmFirstInvestmentClauseDTO idmFICDetail, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmFirstInvestmentClause>(idmFICDetail);
            var currentDetails = await _FICRepository.FirstOrDefaultByExpressionAsync( x => x.IsActive == true && x.IsDeleted == false && x.DCFICId == basicDetails.DCFICId,token);
            if(currentDetails!=null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                await _FICRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
            }

            basicDetails.DCFICId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;

            var response = await _FICRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Other Relaxation
        public async Task<IEnumerable<RelaxationDTO>> GetAllOtherRelaxation(long accountNumber, CancellationToken token)
        {

            var conddet = await _relaxRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.WhRelaxation == true && x.WhRelAllowed == null, cdtype => cdtype.TblCondTypeCdtab);

            var addlconddet = await _additionalCondition.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.Relaxation == true && x.WhRelAllowed == null, token);

            var result = new List<RelaxationDTO>();

            foreach (var i in conddet)
            {
                result.Add(new RelaxationDTO()
                {
                    LoanAcc = i.LoanAcc,
                    RelaxCondId = i.CondDetId,
                    ConditionDetails = i.CondDetails,
                    WhRelAllowed = i.WhRelAllowed,
                    ConditionType = i.CondType,
                    ModelName = "Disbursement Condition",
                    WhRelaxation = i.WhRelaxation,
                });
            }
            foreach (var i in addlconddet)
            {
                result.Add(new RelaxationDTO()
                {
                    LoanAcc = i.LoanAcc,
                    RelaxCondId = i.AddCondId,
                    ConditionDetails = i.AddCondDetails,
                    WhRelAllowed = i.WhRelAllowed,
                    ModelName = "Additional Condition",
                    WhRelaxation = i.Relaxation,
                });
            }            
            return result;
        }

        public async Task<List<RelaxationDTO>> UpdateOtherRelaxation(List<RelaxationDTO> relax, CancellationToken token)
        {
            foreach (var item in relax)
            {
                if (item.ModelName == "Disbursement Condition")
                {
                    var result = await _condRepository.FirstOrDefaultByExpressionAsync(x => x.CondDetId == item.RelaxCondId, token);
                    //item.WhRelAllowed = true;
                    result.WhRelAllowed = item.WhRelAllowed;

                    await _condRepository.UpdateAsync(result, token).ConfigureAwait(false);
                }
                else if (item.ModelName == "Additional Condition")
                {
                    var result = await _additionalCondition.FirstOrDefaultByExpressionAsync(x => x.AddCondId == item.RelaxCondId, token);
                    
                    //item.WhRelAllowed = true;
                    result.WhRelAllowed = item.WhRelAllowed;

                    await _additionalCondition.UpdateAsync(result, token).ConfigureAwait(false);
                }
                await _work.CommitAsync(token).ConfigureAwait(false);
            }
            return _mapper.Map<List<RelaxationDTO>>(relax);
        }
        #endregion
    }
}
