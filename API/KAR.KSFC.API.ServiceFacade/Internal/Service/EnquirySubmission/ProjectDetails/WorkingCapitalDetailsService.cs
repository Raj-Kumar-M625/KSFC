using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
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
    public class WorkingCapitalDetailsService : IWorkingCapitalDetails
    {
        private readonly IEntityRepository<TblEnqWcDet> _workingCapitalRepository;
        private readonly IEnquiryHomeService _enquiryService;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public WorkingCapitalDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqWcDet> workingCapitalRepository, IUnitOfWork work, IEnquiryHomeService enquiryService = null)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _workingCapitalRepository = workingCapitalRepository;
            _work = work;
            _enquiryService = enquiryService;
        }
        public async Task<ProjectWorkingCapitalDeatailsDTO> AddWorkingCapitalDetails(ProjectWorkingCapitalDeatailsDTO WorkingCapitalDTO, CancellationToken token)
        {
            

            if (WorkingCapitalDTO.EnqtempId == 0 || WorkingCapitalDTO.EnqtempId == null)
            {
                WorkingCapitalDTO.EnqtempId = await _enquiryService.AddNewEnqiry(_userInfo.Pan, token).ConfigureAwait(false);
            }
            else {
                await DeleteWorkingCapitalDetails((int)WorkingCapitalDTO.EnqtempId, token).ConfigureAwait(false);
            }
            var details = _mapper.Map<TblEnqWcDet>(WorkingCapitalDTO);

            details.EnqWcId = 0;
            details.IsActive = true;
            details.IsDeleted = false;
            details.CreatedBy = _userInfo.Pan;
            details.CreatedDate = DateTime.UtcNow;
            details.UniqueId = Guid.NewGuid().ToString();

            var result = await _workingCapitalRepository.AddAsync(details, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<ProjectWorkingCapitalDeatailsDTO>(result);
        }

        public async Task<bool> DeleteWorkingCapitalDetails(int enquiryId, CancellationToken token)
        {
            var workingCapital = await _workingCapitalRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (workingCapital == null)
            {
                return false;
            }

            workingCapital.IsActive = false;
            workingCapital.IsDeleted = true;
            workingCapital.ModifiedBy = _userInfo.Pan;
            workingCapital.CreatedDate = DateTime.UtcNow;

            await _workingCapitalRepository.UpdateAsync(workingCapital, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<ProjectWorkingCapitalDeatailsDTO> GetByIdWorkingCapitalDetails(int enquiryId, CancellationToken token)
        {
            var list = await _workingCapitalRepository.FindByExpressionAsync(x => x.EnqWcId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<ProjectWorkingCapitalDeatailsDTO>(list);
        }

        public async Task<ProjectWorkingCapitalDeatailsDTO> UpdateWorkingCapitalDetails(ProjectWorkingCapitalDeatailsDTO WorkingCapitalDTO, CancellationToken token)
        {
            var workingDetails = await _workingCapitalRepository
                                     .FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EnqtempId == WorkingCapitalDTO.EnqtempId
                                        && x.EnqWcId == WorkingCapitalDTO.EnqWcId
                                        && x.IsActive == true && x.IsDeleted == false)
                                     .ConfigureAwait(false);

            if (workingDetails == null)
            {
                throw new ArgumentException("No Data Found");
            }
            var wCDetails = _mapper.Map<TblEnqWcDet>(WorkingCapitalDTO);

            wCDetails.ModifiedBy = _userInfo.UserId;
            wCDetails.ModifiedDate = DateTime.UtcNow;

            wCDetails.IsActive = false;
            wCDetails.IsDeleted = false;


            var result = await _workingCapitalRepository.UpdateAsync(wCDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<ProjectWorkingCapitalDeatailsDTO>(result);
        }
    }
}
