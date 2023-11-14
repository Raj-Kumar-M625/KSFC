using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
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

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromoterDetailsService : IPromoterDetails
    {
        private readonly IEntityRepository<TblEnqPromDet> _promoterRepository;
        private readonly IEntityRepository<TblPromCdtab> _promoterMasterRepository;
        private readonly IEntityRepository<TblEnqPbankDet> _promoterBankRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        private readonly IEnquiryHomeService _enquiryService;
        public PromoterDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqPromDet> promoterRepository,
            IEntityRepository<TblEnqPbankDet> promoterBankRepository, IEntityRepository<TblPromCdtab> promoterMasterRepository, IEnquiryHomeService enquiryService,
            IUnitOfWork work)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _promoterRepository = promoterRepository;
            _promoterBankRepository = promoterBankRepository;
            _promoterMasterRepository = promoterMasterRepository;
            _work = work;
            _enquiryService = enquiryService;
        }
        public async Task<IEnumerable<PromoterDetailsDTO>> AddPromoterDetails(List<PromoterDetailsDTO> PromoterDTO, CancellationToken token)
        {
            var promoterList = new List<PromoterDetailsDTO>();

            var checkEnqTempId = PromoterDTO.FirstOrDefault(x => x.EnqtempId == null || x.EnqtempId == 0);
            if (checkEnqTempId != null)
            {
                var tempId = await _enquiryService.AddNewEnqiry(_userInfo.Pan, token).ConfigureAwait(false);
                PromoterDTO.ForEach(x => x.EnqtempId = tempId);
            }
            foreach (var item in PromoterDTO)
            {
                var promoterMasterData = new TblPromCdtab
                {
                    CreatedBy = _userInfo.Pan,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false,
                    PromoterDob = item.PromoterMaster.PromoterDob,
                    PromoterEmailid = item.PromoterMaster.PromoterEmailid,
                    PromoterGender = item.PromoterMaster.PromoterGender,
                    PromoterMobno = item.PromoterMaster.PromoterMobno,
                    PromoterName = item.PromoterMaster.PromoterName,
                    PromoterPan = item.PromoterMaster.PromoterPan,
                    PromoterPassport = item.PromoterMaster.PromoterPassport,
                    PromoterPhoto = item.PromoterMaster.PromoterPhoto,
                    UniqueId = Guid.NewGuid().ToString()
                };

                var promoterMaster = await _promoterMasterRepository.AddAsync(promoterMasterData, token).ConfigureAwait(false);
                await _work.CommitAsync(token).ConfigureAwait(false);
                if (promoterMaster != null)
                {
                    var enqPromoter = new TblEnqPromDet
                    {
                        PromoterCode = promoterMaster.PromoterCode,
                        CreatedBy = _userInfo.Pan,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                        DomCd = item.DomCd,
                        PdesigCd = item.PdesigCd,
                        EnqCibil = item.EnqCibil,
                        EnqPromExp = item.EnqPromExp,
                        EnqPromExpdet = item.EnqPromExpdet,
                        EnqPromShare = item.EnqPromShare,
                        EnqtempId = item.EnqtempId,
                        UniqueId = Guid.NewGuid().ToString()
                    };

                    var promoter = await _promoterRepository.AddAsync(enqPromoter, token).ConfigureAwait(false);

                    var promoterBankDetails = new TblEnqPbankDet
                    {
                        CreatedBy = _userInfo.Pan,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                        EnqtempId = item.EnqtempId,
                        PromAccName = item.PromoterBankDetails.PromAccName,
                        PromAcctype = item.PromoterBankDetails.PromAcctype,
                        PromBankaccno = item.PromoterBankDetails.PromBankaccno,
                        PromBankbr = item.PromoterBankDetails.PromBankbr,
                        PromBankname = item.PromoterBankDetails.PromBankname,
                        PromIfsc = item.PromoterBankDetails.PromIfsc,
                        PromoterCode = promoterMaster.PromoterCode,
                        UniqueId = Guid.NewGuid().ToString()
                    };

                    var promBank = await _promoterBankRepository.AddAsync(promoterBankDetails, token).ConfigureAwait(false);
                    await _work.CommitAsync(token).ConfigureAwait(false);

                    var promoterBankDto = _mapper.Map<PromoterBankDetailsDTO>(promBank);
                    var promoterDto = _mapper.Map<PromoterDetailsDTO>(promoter);
                    var promMasterDto = _mapper.Map<PromoterMasterDTO>(promoterMaster);

                    var promoterDetailsObj = new PromoterDetailsDTO
                    {
                        PromoterBankDetails = promoterBankDto,
                        PromoterMaster = promMasterDto,
                        DomCd = promoterDto.DomCd,
                        EnqCibil = promoterDto.EnqCibil,
                        EnqPromExp = promoterDto.EnqPromExp,
                        EnqPromExpdet = promoterDto.EnqPromExpdet,
                        EnqPromId = promoterDto.EnqPromId,
                        EnqPromShare = promoterDto.EnqPromShare,
                        EnqtempId = promoterDto.EnqtempId,
                        PdesigCd = promoterDto.PdesigCd,
                        PromoterCode = promoterDto.PromoterCode,
                        UniqueId = promoterDto.UniqueId
                    };
                    promoterList.Add(promoterDetailsObj);
                }
            }

            return promoterList;
        }

        public async Task<bool> DeletePromoterDetails(int EnqPromId, CancellationToken token)
        {
            var promoter = await _promoterRepository.FirstOrDefaultByExpressionAsync(x => x.EnqPromId == EnqPromId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (promoter == null)
            {
                throw new ArgumentException("data not found");
            }

            promoter.IsActive = false;
            promoter.IsDeleted = true;
            promoter.ModifiedBy = _userInfo.UserId;
            promoter.CreatedDate = DateTime.UtcNow;

            await _promoterRepository.UpdateAsync(promoter, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }
        
        public async Task<IEnumerable<PromoterDetailsDTO>> GetByIdPromoterDetails(int enquiryId, CancellationToken token)
        {
            var list = await _promoterRepository.FindByExpressionAsync(x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<PromoterDetailsDTO>>(list);
        }

        public async Task<IEnumerable<PromoterDetailsDTO>> UpdatePromoterDetails(List<PromoterDetailsDTO> PromoterDTO, CancellationToken token)
        {
            long promoterCode = 0;
            var promoterList = new List<PromoterDetailsDTO>();
            PromoterMasterDTO PromoterMasterDTO = new PromoterMasterDTO();
            var list_promoter = await _promoterRepository.FindByExpressionAsync(x => x.EnqtempId == PromoterDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            list_promoter.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.Pan;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _promoterRepository.UpdateAsync(list_promoter.ToList(), token).ConfigureAwait(false);

            var list_promoterBank = await _promoterBankRepository.FindByExpressionAsync(x => x.EnqtempId == PromoterDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            list_promoterBank.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.Pan;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _promoterBankRepository.UpdateAsync(list_promoterBank.ToList(), token).ConfigureAwait(false);

            await _work.CommitAsync(token).ConfigureAwait(false);


            foreach (var item in PromoterDTO)
            {
                var promasterData = await _promoterMasterRepository.FirstOrDefaultNoTrackingByExpressionAsync
                    (x => x.PromoterCode == item.PromoterCode &&
                    x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                if (promasterData == null)
                {
                    var promoterMasterData = new TblPromCdtab
                    {
                        ModifiedBy = _userInfo.Pan,
                        ModifiedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                        PromoterDob = item.PromoterMaster.PromoterDob,
                        PromoterEmailid = item.PromoterMaster.PromoterEmailid,
                        PromoterGender = item.PromoterMaster.PromoterGender,
                        PromoterMobno = item.PromoterMaster.PromoterMobno,
                        PromoterName = item.PromoterMaster.PromoterName,
                        PromoterPan = item.PromoterMaster.PromoterPan,
                        PromoterPassport = item.PromoterMaster.PromoterPassport,
                        PromoterPhoto = item.PromoterMaster.PromoterPhoto,
                        UniqueId = Guid.NewGuid().ToString()
                    };

                    var promoterMaster = await _promoterMasterRepository.AddAsync(promoterMasterData, token).ConfigureAwait(false);
                    await _work.CommitAsync(token).ConfigureAwait(false);
                    promoterCode = promoterMaster.PromoterCode;
                    PromoterMasterDTO = _mapper.Map<PromoterMasterDTO>(promoterMasterData);
                }
                else
                {
                    promasterData.ModifiedBy = _userInfo.Pan;
                    promasterData.ModifiedDate = DateTime.UtcNow;
                    promasterData.IsActive = true;
                    promasterData.IsDeleted = false;
                    promasterData.PromoterDob = item.PromoterMaster.PromoterDob;
                    promasterData.PromoterEmailid = item.PromoterMaster.PromoterEmailid;
                    promasterData.PromoterGender = item.PromoterMaster.PromoterGender;
                    promasterData.PromoterMobno = item.PromoterMaster.PromoterMobno;
                    promasterData.PromoterName = item.PromoterMaster.PromoterName;
                    promasterData.PromoterPan = item.PromoterMaster.PromoterPan;
                    promasterData.PromoterPassport = item.PromoterMaster.PromoterPassport;
                    promasterData.PromoterPhoto = item.PromoterMaster.PromoterPhoto;
                    await _promoterMasterRepository.UpdateAsync(promasterData, token).ConfigureAwait(false);
                    PromoterMasterDTO = _mapper.Map<PromoterMasterDTO>(promasterData);
                    await _work.CommitAsync(token).ConfigureAwait(false);
                }


                var enqPromoter = new TblEnqPromDet
                {
                    PromoterCode = promasterData == null ? promoterCode : promasterData.PromoterCode,
                    CreatedBy = _userInfo.Pan,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false,
                    DomCd = item.DomCd,
                    PdesigCd = item.PdesigCd,
                    EnqCibil = item.EnqCibil,
                    EnqPromExp = item.EnqPromExp,
                    EnqPromExpdet = item.EnqPromExpdet,
                    EnqPromShare = item.EnqPromShare,
                    EnqtempId = item.EnqtempId,
                    UniqueId = Guid.NewGuid().ToString()
                };

                var promoter = await _promoterRepository.AddAsync(enqPromoter, token).ConfigureAwait(false);

                var promoterBankDetails = new TblEnqPbankDet
                {
                    CreatedBy = _userInfo.Pan,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false,
                    EnqtempId = item.EnqtempId,
                    PromAccName = item.PromoterBankDetails.PromAccName,
                    PromAcctype = item.PromoterBankDetails.PromAcctype,
                    PromBankaccno = item.PromoterBankDetails.PromBankaccno,
                    PromBankbr = item.PromoterBankDetails.PromBankbr,
                    PromBankname = item.PromoterBankDetails.PromBankname,
                    PromIfsc = item.PromoterBankDetails.PromIfsc,
                    PromoterCode = promasterData == null ? promoterCode : promasterData.PromoterCode,
                    UniqueId = Guid.NewGuid().ToString()
                };

                var promBank = await _promoterBankRepository.AddAsync(promoterBankDetails, token).ConfigureAwait(false);
                await _work.CommitAsync(token).ConfigureAwait(false);

                var promoterBankDto = _mapper.Map<PromoterBankDetailsDTO>(promBank);
                var promoterDto = _mapper.Map<PromoterDetailsDTO>(promoter);


                var promoterDetailsObj = new PromoterDetailsDTO
                {
                    PromoterBankDetails = promoterBankDto,
                    PromoterMaster = PromoterMasterDTO,
                    DomCd = promoterDto.DomCd,
                    EnqCibil = promoterDto.EnqCibil,
                    EnqPromExp = promoterDto.EnqPromExp,
                    EnqPromExpdet = promoterDto.EnqPromExpdet,
                    EnqPromId = promoterDto.EnqPromId,
                    EnqPromShare = promoterDto.EnqPromShare,
                    EnqtempId = promoterDto.EnqtempId,
                    PdesigCd = promoterDto.PdesigCd,
                    PromoterCode = promoterDto.PromoterCode,
                    UniqueId = promoterDto.UniqueId
                };
                promoterList.Add(promoterDetailsObj);

            }

            return promoterList;
        }
      
         
    }
}
