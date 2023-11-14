using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.ProjectDetails
{
    public class ProjectFinancialDetailsService : IProjectFinancialDetails
    {
        private readonly IEntityRepository<TblEnqPjfinDet> _financeService;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        private readonly IEntityRepository<TblEnqTemptab> _enqRepository;

        public ProjectFinancialDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqPjfinDet> financeService, IUnitOfWork work, IEntityRepository<TblEnqTemptab> enqRepository = null)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _financeService = financeService;
            _work = work;
            _enqRepository = enqRepository;
        }
        public async Task<IEnumerable<ProjectFinancialYearDetailsDTO>> AddProjectFinancialDetails(List<ProjectFinancialYearDetailsDTO> projectDetail, CancellationToken token)
        {
            var dataList = await _financeService.FindByExpressionAsync(x => x.EnqtempId == projectDetail[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            dataList.ForEach(x =>
            {
                x.IsDeleted = true;
                x.IsActive = false;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _financeService.UpdateAsync(dataList, token).ConfigureAwait(false);

            var detailsList = _mapper.Map<List<TblEnqPjfinDet>>(projectDetail);

            detailsList.ForEach(x =>
            {
                x.EnqPjfinId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _financeService.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<ProjectFinancialYearDetailsDTO>>(result);
        }

        public async Task<bool> DeleteProjectFinancialDetails(int Id, CancellationToken token)
        {
            var financeDetails  = await _financeService.FirstOrDefaultByExpressionAsync(x => x.EnqPjfinId == Id && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (financeDetails == null)
            {
                throw new ArgumentException("data not found");
            }

            financeDetails.IsActive = false;
            financeDetails.IsDeleted = true;
            financeDetails.ModifiedBy = _userInfo.UserId;
            financeDetails.CreatedDate = DateTime.UtcNow;

            await _financeService.UpdateAsync(financeDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public Task<ProjectFinancialYearDetailsDTO> GetAllProjectFinancial(ProjectFinancialYearDetailsDTO projectCost, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectFinancialYearDetailsDTO> GetByIdProjectFinancialDetails(int Id, CancellationToken token)
        {
            var list = await _financeService.FindByExpressionAsync(x => x.EnqPjfinId == Id && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<ProjectFinancialYearDetailsDTO>(list);
        }

        public async Task<IEnumerable<ProjectFinancialYearDetailsDTO>> UpdateProjectFinancialDetails(List<ProjectFinancialYearDetailsDTO> projectFinancial, CancellationToken token)
        {
            var enquiry = await _enqRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == projectFinancial[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("Enquiry data not found");
            }

            var registrationUList = await _financeService.FindByExpressionAsync(x => x.EnqtempId == projectFinancial[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            registrationUList.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _financeService.UpdateAsync(registrationUList, token).ConfigureAwait(false);

            var registrationList = _mapper.Map<List<TblEnqPjfinDet>>(projectFinancial);

            registrationList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var data = await _financeService.AddAsync(registrationList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<ProjectFinancialYearDetailsDTO>>(data);
        }
    }
}
