using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Data.Repository.Interface;

using KAR.KSFC.Components.Data.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Repository.UoW;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.ProjectDetails
{
    public class ProjectCostService : IProjectCostDetails
    {
        private readonly IEntityRepository<TblEnqPjcostDet> _projectCostDetails;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userInfo"></param>
        /// <param name="ProjectCostDetails"></param>
        /// <param name="work"></param>
        public ProjectCostService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqPjcostDet> ProjectCostDetails, IUnitOfWork work)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _projectCostDetails = ProjectCostDetails;
            _work = work;
        }

        public async Task<ProjectCostDetailsDTO> GetAllProjectCosts(CancellationToken token)
        {
            var list = await _projectCostDetails.FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EnqPjcostId == x.EnqPjcostId).ConfigureAwait(false);
            if (list == null)
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<ProjectCostDetailsDTO>(list);
        }

        public async Task<List<ProjectCostDetailsDTO>> AddProjectCostDetails(List<ProjectCostDetailsDTO> projectCost, CancellationToken token)
        {

            var existingData = await _projectCostDetails.FindByExpressionAsync(x => x.EnqtempId == projectCost.FirstOrDefault().EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            existingData.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _projectCostDetails.UpdateAsync(existingData, token).ConfigureAwait(false);

            var details = _mapper.Map<List<TblEnqPjcostDet>>(projectCost);

            details.ForEach(x =>
            {
                x.EnqPjcostId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });
            var result = await _projectCostDetails.AddAsync(details, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<ProjectCostDetailsDTO>>(result);
        }

        public async Task<bool> DeleteProjectCostDetails(int Id, CancellationToken token)
        {
            var projectCost_details = await _projectCostDetails.FirstOrDefaultByExpressionAsync(x => x.PjcostCd == Id && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (projectCost_details == null)
            {
                throw new ArgumentException("data not found");
            }

            projectCost_details.IsActive = false;
            projectCost_details.IsDeleted = true;
            projectCost_details.ModifiedBy = _userInfo.UserId;
            projectCost_details.CreatedDate = DateTime.UtcNow;

            await _projectCostDetails.UpdateAsync(projectCost_details, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ProjectCostDetailsDTO> GetByIdProjectCostDetails(int Id, CancellationToken token)
        {
            var list = await _projectCostDetails.FindByExpressionAsync(x => x.PjcostCd == Id && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<ProjectCostDetailsDTO>(list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectCost"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> UpdateProjectCostDetails(ProjectCostDetailsDTO projectCost, CancellationToken token)
        {

            TblEnqPjcostDet projectCost_details_raw = await _projectCostDetails.FirstOrDefaultByExpressionAsync(x => x.PjcostCd == projectCost.PjcostCd && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            var projectCost_details = _mapper.Map<ProjectCostDetailsDTO>(projectCost_details_raw);
            if (projectCost_details == null)
            {
                throw new ArgumentException("data not found");
            }

            //projectCost_details.PjcostCd = projectCost.PjcostDets;
            //projectCost_details.PjcostFlg = projectCost.PjcostFlg;
            //projectCost_details.SeqNo = projectCost.SeqNo;
            //projectCost_details.PjcostgroupCd = projectCost.PjcostgroupCd;
            //projectCost_details.PjcsgroupCd = projectCost.PjcsgroupCd;

            var _projectCost = _mapper.Map<TblEnqPjcostDet>(projectCost_details);

            await _projectCostDetails.UpdateAsync(_projectCost, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }


    }
}
