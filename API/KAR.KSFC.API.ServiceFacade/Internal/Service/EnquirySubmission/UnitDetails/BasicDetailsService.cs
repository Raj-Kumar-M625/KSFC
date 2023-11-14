using System;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.UnitDetails
{
    /// <summary>
    /// Basic Details
    /// </summary>
    public class BasicDetailsService : IBasicDetails
    {
        private readonly IEntityRepository<TblEnqBasicDet> _basicDetailRepository;
        private readonly IEntityRepository<TblEnqTemptab> _enqRepository;
        private readonly IUnitOfWork _work;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Basic Details constructor
        /// </summary>
        /// <param name="basicDetailRepository"></param>
        /// <param name="enqRepository"></param>
        /// <param name="userInfo"></param>
        /// <param name="mapper"></param>
        public BasicDetailsService(IEntityRepository<TblEnqBasicDet> basicDetailRepository,
            IEntityRepository<TblEnqTemptab> enqRepository, UserInfo userInfo, IMapper mapper, IUnitOfWork work)
        {
            _basicDetailRepository = basicDetailRepository;
            _enqRepository = enqRepository;
            _userInfo = userInfo;
            _mapper = mapper;
            _work = work;

        }

        /// <summary>
        /// Add Basic Details
        /// </summary>
        /// <param name="basicDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BasicDetailsDto> AddBasicDetails(BasicDetailsDto basicDetailsDTO, CancellationToken token)
        {
            if (basicDetailsDTO.EnqtempId == null || basicDetailsDTO.EnqtempId == 0)
            {
                TblEnqTemptab enq = new TblEnqTemptab
                {
                    UniqueId = Guid.NewGuid().ToString(),
                    CreatedBy = _userInfo.Pan,
                    CreatedDate = DateTime.UtcNow,
                    EnqInitDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false,
                    PromPan = _userInfo.Pan,
                    EnqStatus = (int)EnqStatus.Draft,
                };
                var tempEnq = await _enqRepository.AddAsync(enq, token).ConfigureAwait(false);
                await _work.CommitAsync(token).ConfigureAwait(false);
                basicDetailsDTO.EnqtempId = tempEnq.EnqtempId;
            }
            var basicDetails = _mapper.Map<TblEnqBasicDet>(basicDetailsDTO);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedDate = DateTime.UtcNow;
            basicDetails.CreatedBy = _userInfo.Pan;
            basicDetails.UniqueId = Guid.NewGuid().ToString();

            var result = await _basicDetailRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);

            var data = _mapper.Map<BasicDetailsDto>(result);
            data.PromoterPan = basicDetailsDTO.PromoterPan;

            return data;
        }

        /// <summary>
        /// Delete Basic Details
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBasicDetails(int enquiryId, CancellationToken token)
        {
            var basicDetails = await _basicDetailRepository
                                     .FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EnqtempId == enquiryId
                                        && x.IsActive == true && x.IsDeleted == false)
                                     .ConfigureAwait(false);
            if (basicDetails == null)
            {
                throw new ArgumentException("No Data Found");
            }

            basicDetails.IsDeleted = true;
            basicDetails.IsActive = false;
            basicDetails.ModifiedBy = _userInfo.Pan;
            basicDetails.ModifiedDate = DateTime.UtcNow;

            await _basicDetailRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Get Basic Details
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BasicDetailsDto> GetBasicDetails(int enquiryId, CancellationToken token)
        {
            var data = await _basicDetailRepository.FindByFirstOrDefalutMatchingPropertiesAsync(token,
                x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false,
                incPurpose => incPurpose.PurpCdNavigation,
                incSize => incSize.SizeCdNavigation,
                incConst=>incConst.ConstCdNavigation,
                incoffice=> incoffice.OffcCdNavigation,
                incprem=>incprem.PremCdNavigation).ConfigureAwait(false);

            var basicDetails = _mapper.Map<BasicDetailsDto>(data);
            if (data != null)
            {
                basicDetails.SizeOfFirm = data?.SizeCdNavigation?.SizeDets;
                basicDetails.PurposeOfLoan = data?.PurpCdNavigation?.PurpDets;
                basicDetails.ConstType = data?.ConstCdNavigation?.CnstDets;
                basicDetails.OffcCode = data?.OffcCdNavigation?.OffcAdr1;
                basicDetails.PremCode = data?.PremCdNavigation?.PremDets;
            }
            return basicDetails;
        }

        /// <summary>
        /// Update Basic Details
        /// </summary>
        /// <param name="basicDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BasicDetailsDto> UpdateBasicDetails(BasicDetailsDto basicDetailsDTO, CancellationToken token)
        {
            var basicDetails = await _basicDetailRepository
                                    .FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EnqtempId == basicDetailsDTO.EnqtempId
                                       && x.EnqBdetId == basicDetailsDTO.EnqBdetId
                                       && x.IsActive == true && x.IsDeleted == false)
                                    .ConfigureAwait(false);
            if (basicDetails == null)
            {
                throw new ArgumentException("No Data Found");
            }
            var updatedBasicDetails = _mapper.Map<TblEnqBasicDet>(basicDetailsDTO);

            updatedBasicDetails.ModifiedBy = _userInfo.Pan;
            updatedBasicDetails.ModifiedDate = DateTime.UtcNow;
            updatedBasicDetails.IsActive = true;
            updatedBasicDetails.IsDeleted = false;
            updatedBasicDetails.UniqueId = basicDetails.UniqueId;
            updatedBasicDetails.CreatedBy = basicDetails.CreatedBy;
            updatedBasicDetails.CreatedDate = basicDetails.CreatedDate;

            var result = await _basicDetailRepository.UpdateAsync(updatedBasicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<BasicDetailsDto>(result);
        }
    }
}
