using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.AuditModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.AuditModule
{
    // <Summary>
    // Author: Gagana K; Module:AuditClearance; Date: 18/08/2022
    // <summary>
    public class AuditService : IAuditService
    {
       
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<TblIdmAuditDet> _idmAuditRepository;
        private readonly UserInfo _userInfo;

        public AuditService( IUnitOfWork work,
                            IMapper mapper, IEntityRepository<TblIdmAuditDet> idmAuditRepository, UserInfo userInfo)
        {
        
            _work = work;
            _mapper = mapper;
            _idmAuditRepository = idmAuditRepository;
            _userInfo = userInfo;
        }
       
        #region AuditClearance

        public async Task<IEnumerable<IdmAuditDetailsDTO>> GetAllAuditClearanceListAsync(long accountNumber, CancellationToken token)
        {
            var data = await _idmAuditRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<IdmAuditDetailsDTO>>(data);
        }

        public async Task<bool> UpdateAuditClearanceDetails(IdmAuditDetailsDTO AuditDTO, CancellationToken token)
        {
            var currentDetails = await _idmAuditRepository.FirstOrDefaultByExpressionAsync(x => x.IdmAuditId == AuditDTO.IdmAuditId, token);
            // Prevent null value and thus null pointer exception.
            if (currentDetails == null) throw new InvalidOperationException();

            else
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.AuditEmpId = Convert.ToInt32(_userInfo.UserId);
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
            }
            await _idmAuditRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);


            var basicDetails = _mapper.Map<TblIdmAuditDet>(AuditDTO);
            basicDetails.IdmAuditId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            currentDetails.AuditEmpId = Convert.ToInt32(_userInfo.UserId);
            basicDetails.ModifiedBy  = _userInfo.Name;            
            basicDetails.ModifiedDate = DateTime.UtcNow;            

            var response = await _idmAuditRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        public async Task<bool> CreateAuditClearanceDetails(IdmAuditDetailsDTO AuditDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmAuditDet>(AuditDTO);
          
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.AuditEmpId = Convert.ToInt32(_userInfo.UserId);
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _idmAuditRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        public async Task<bool> DeleteAuditClearanceDetails(IdmAuditDetailsDTO AuditDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmAuditDet>(AuditDTO);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.AuditEmpId = Convert.ToInt32(_userInfo.UserId);
            basicDetails.IsDeleted = true;
            var response = await _idmAuditRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

    }
}
