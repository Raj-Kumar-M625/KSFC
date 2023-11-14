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
    public class MeansofFinanceService : IMeansofFinance
    {
        private readonly IEntityRepository<TblEnqPjmfDet> _meansofFinance;
        private readonly IEntityRepository<TblEnqMftotalDet> _mfTotal;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        private readonly IEntityRepository<TblEnqTemptab> _enqRepository;

        public MeansofFinanceService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqPjmfDet> meansofFinance, IEntityRepository<TblEnqTemptab> enqRepository = null, IUnitOfWork work = null, IEntityRepository<TblEnqMftotalDet> mfTotal = null)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _meansofFinance = meansofFinance;
            _enqRepository = enqRepository;
            _work = work;
            _mfTotal = mfTotal;
        }

        public async Task<ProjectMeansOfFinanceDTO> GetAllMeansOfFinance(ProjectMeansOfFinanceDTO ProjectMeansOfFinance, CancellationToken token)
        {
            var list = await _meansofFinance.FindByExpressionAsync(null, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<ProjectMeansOfFinanceDTO>(list);
        }

        public async Task<IEnumerable<ProjectMeansOfFinanceDTO>> AddMeansOfFinance(List<ProjectMeansOfFinanceDTO> ProjectMeansOfFinance, CancellationToken token)
        {
            var dataList = await _meansofFinance.FindByExpressionAsync(x => x.EnqtempId == ProjectMeansOfFinance[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            dataList.ForEach(x =>
            {
                x.IsDeleted = true;
                x.IsActive = false;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _meansofFinance.UpdateAsync(dataList, token).ConfigureAwait(false);

            var detailsList = _mapper.Map<List<TblEnqPjmfDet>>(ProjectMeansOfFinance);

            var totalEquity = ProjectMeansOfFinance.Where(x => x.MfcatCdNavigation.PjmfDets.ToLower() == "equity").Sum(s => s.EnqPjmfValue);
            var totalDedt = ProjectMeansOfFinance.Where(x => x.MfcatCdNavigation.PjmfDets.ToLower() == "debt").Sum(s => s.EnqPjmfValue);
            var mfTotal = await _mfTotal.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == ProjectMeansOfFinance.First().EnqtempId, token).ConfigureAwait(false);
            if (mfTotal != null)
            {
                mfTotal.EnqDebt = totalDedt;
                mfTotal.EnqEquity = totalEquity;
                mfTotal.ModifiedBy = _userInfo.UserId;
                mfTotal.ModifiedDate = DateTime.UtcNow;
                await _mfTotal.UpdateAsync(mfTotal, token).ConfigureAwait(false);
            }
            else
            {
                var newMfTotal = new TblEnqMftotalDet
                {
                    CreatedBy = _userInfo.UserId,
                    CreatedDate = DateTime.UtcNow,
                    EnqDebt = totalDedt,
                    EnqEquity = totalEquity,
                    EnqtempId = ProjectMeansOfFinance.First().EnqtempId,
                    IsActive = true,
                    IsDeleted = false,
                    UniqueId = Guid.NewGuid().ToString()
                };
                await _mfTotal.AddAsync(newMfTotal, token).ConfigureAwait(false);
            }


            detailsList.ForEach(x =>
            {
                x.EnqPjmfId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
                x.MfcatCdNavigation = null;
                x.PjmfCdNavigation = null;
            });

            var result = await _meansofFinance.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<ProjectMeansOfFinanceDTO>>(result);
        }

        public async Task<bool> DeleteMeansOfFinance(int Id, CancellationToken token)
        {
            var ProjectMeansOfFinance_details = await _meansofFinance.FirstOrDefaultByExpressionAsync(x => x.PjmfCd == Id && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (ProjectMeansOfFinance_details == null)
            {
                throw new ArgumentException("data not found");
            }

            ProjectMeansOfFinance_details.IsActive = false;
            ProjectMeansOfFinance_details.IsDeleted = true;
            ProjectMeansOfFinance_details.ModifiedBy = _userInfo.UserId;
            ProjectMeansOfFinance_details.CreatedDate = DateTime.UtcNow;

            await _meansofFinance.UpdateAsync(ProjectMeansOfFinance_details, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<ProjectMeansOfFinanceDTO> GetByIdMeansOfFinance(int Id, CancellationToken token)
        {
            var list = await _meansofFinance.FindByExpressionAsync(x => x.PjmfCd == Id && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<ProjectMeansOfFinanceDTO>(list);
        }

        public async Task<IEnumerable<ProjectMeansOfFinanceDTO>> UpdateMeansOfFinance(List<ProjectMeansOfFinanceDTO> ProjectMeansOfFinance, CancellationToken token)
        {

            var enquiry = await _enqRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == ProjectMeansOfFinance[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("Enquiry data not found");
            }

            var financeUList = await _meansofFinance.FindByExpressionAsync(x => x.EnqtempId == ProjectMeansOfFinance[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            financeUList.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _meansofFinance.UpdateAsync(financeUList, token).ConfigureAwait(false);

            var financeList = _mapper.Map<List<TblEnqPjmfDet>>(ProjectMeansOfFinance);

            financeList.ForEach(x =>
            {
                x.EnqPjmfId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var data = await _meansofFinance.AddAsync(financeList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<ProjectMeansOfFinanceDTO>>(data);
        }
    }
}
