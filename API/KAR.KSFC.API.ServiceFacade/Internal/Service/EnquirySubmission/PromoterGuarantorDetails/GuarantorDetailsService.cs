using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.PromoterGuarantorDetails
{
    public class GuarantorDetailsService : IGuarantorDetails
    {
        private readonly IEntityRepository<TblPromCdtab> _promoterMasterRepository;
        private readonly IEntityRepository<TblEnqGuarDet> _guarantorRepository;
        private readonly IEntityRepository<TblEnqGbankDet> _guarantorBankRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        private readonly IEnquiryHomeService _enquiryService;
        public GuarantorDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqGuarDet> guarantorRepository, 
            IEntityRepository<TblPromCdtab> promoterMasterRepository, IEntityRepository<TblEnqGbankDet> guarantorBankRepository, IUnitOfWork work, IEnquiryHomeService enquiryService)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _guarantorRepository = guarantorRepository;
            _promoterMasterRepository = promoterMasterRepository;
            _guarantorBankRepository = guarantorBankRepository;
            _work = work;
            _enquiryService = enquiryService;
        }
        public async Task<IEnumerable<GuarantorDetailsDTO>> AddGuarantorDetails(List<GuarantorDetailsDTO> GuarantorDTO, CancellationToken token)
        {
            var promoterList = new List<GuarantorDetailsDTO>();
            var checkEnqTempId = GuarantorDTO.FirstOrDefault(x => x.EnqtempId == null || x.EnqtempId == 0);
            if (checkEnqTempId != null)
            {
                var tempId = await _enquiryService.AddNewEnqiry(_userInfo.Pan, token).ConfigureAwait(false);
                GuarantorDTO.ForEach(x => x.EnqtempId = tempId);
            }
            foreach (var item in GuarantorDTO)
            {
                var GuarantorMasterData = new TblPromCdtab
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
                    PromoterPassport = item.PromoterMaster.PromoterPassport ?? "NOPASSPORT",//This field is not there for Guarantor
                    PromoterPhoto = item.PromoterMaster.PromoterPhoto,
                    UniqueId = Guid.NewGuid().ToString()
                };

                var promoterMaster = await _promoterMasterRepository.AddAsync(GuarantorMasterData, token).ConfigureAwait(false);
                await _work.CommitAsync(token).ConfigureAwait(false);

                if (promoterMaster != null)
                {
                    var enqPromoter = new TblEnqGuarDet
                    {
                        EnqGuarcibil = item.EnqGuarcibil,
                        PromoterCode = promoterMaster.PromoterCode,
                        CreatedBy = _userInfo.Pan,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                        DomCd = item.DomCd,
                        EnqtempId = item.EnqtempId,
                        UniqueId = Guid.NewGuid().ToString()
                    };

                    var promoter = await _guarantorRepository.AddAsync(enqPromoter, token).ConfigureAwait(false);

                    var gBankDetails = new TblEnqGbankDet
                    {
                        CreatedBy = _userInfo.Pan,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                        EnqtempId = item.EnqtempId,
                        GuarBankaccno = item.GuarantorBankDetails.GuarBankaccno,
                        GuarAcctype = item.GuarantorBankDetails.GuarAcctype,
                        GuarAccName = item.GuarantorBankDetails.GuarAccName,
                        GuarBankbr = item.GuarantorBankDetails.GuarBankbr,
                        GuarBankname = item.GuarantorBankDetails.GuarBankname,
                        GuarIfsc = item.GuarantorBankDetails.GuarIfsc,
                        PromoterCode = promoterMaster.PromoterCode,
                        UniqueId = Guid.NewGuid().ToString()
                    };

                    var guarantorBank = await _guarantorBankRepository.AddAsync(gBankDetails, token).ConfigureAwait(false);
                    await _work.CommitAsync(token).ConfigureAwait(false);
                    var promoterBankDto = _mapper.Map<GuarantorBankDetailsDTO>(guarantorBank);
                    var promoterDto = _mapper.Map<GuarantorDetailsDTO>(promoter);
                    var promMasterDto = _mapper.Map<PromoterMasterDTO>(promoterMaster);

                    var promoterDetailsObj = new GuarantorDetailsDTO
                    {
                        GuarantorBankDetails = promoterBankDto,
                        PromoterMaster = promMasterDto,
                        DomCd = promoterDto.DomCd,
                        EnqtempId = promoterDto.EnqtempId,
                        PromoterCode = promoterDto.PromoterCode,
                        EnqGuarcibil = promoterDto.EnqGuarcibil,
                        EnqGuarId = promoterDto.EnqGuarId,
                        UniqueId = promoterDto.UniqueId
                    };
                    promoterList.Add(promoterDetailsObj);
                }
            }

            return promoterList;
        }

        public async Task<bool> DeleteGuarantorDetails(int EnqGuarId, CancellationToken token)
        {
            var guarantor = await _guarantorRepository.FirstOrDefaultByExpressionAsync(x => x.EnqGuarId == EnqGuarId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (guarantor == null)
            {
                throw new ArgumentException("data not found");
            }

            guarantor.IsActive = false;
            guarantor.IsDeleted = true;
            guarantor.ModifiedBy = _userInfo.UserId;
            guarantor.CreatedDate = DateTime.UtcNow;

            await _guarantorRepository.UpdateAsync(guarantor, token).ConfigureAwait(false);
            return true;
        }


        public async Task<List<GuarantorDetailsDTO>> GetByIdGuarantorDetails(int enquiryId, CancellationToken token)
        {
            var list = await _guarantorRepository.FindByExpressionAsync(x => x.EnqGuarId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<GuarantorDetailsDTO>>(list);
        }

        public async Task<IEnumerable<GuarantorDetailsDTO>> UpdateGuarantorDetails(List<GuarantorDetailsDTO> GuarantorDTO, CancellationToken token)
        {
            long promoterCode = 0;
            var guarantorList = new List<GuarantorDetailsDTO>();
            PromoterMasterDTO PromoterMasterDTO = new PromoterMasterDTO();
            var list_promoter = await _guarantorRepository.FindByExpressionAsync(x => x.EnqtempId == GuarantorDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            list_promoter.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.Pan;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _guarantorRepository.UpdateAsync(list_promoter.ToList(), token).ConfigureAwait(false);

            var list_promoterBank = await _guarantorBankRepository.FindByExpressionAsync(x => x.EnqtempId == GuarantorDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            list_promoterBank.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.Pan;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _guarantorBankRepository.UpdateAsync(list_promoterBank.ToList(), token).ConfigureAwait(false);

            await _work.CommitAsync(token).ConfigureAwait(false);


            foreach (var item in GuarantorDTO)
            {
                var promasterData = await _promoterMasterRepository.FirstOrDefaultNoTrackingByExpressionAsync(x => x.PromoterCode == item.PromoterCode && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
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
                        PromoterPassport = item.PromoterMaster.PromoterPassport ?? "Test",//This field is not there for Guarantor
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
                    promoterCode = promasterData.PromoterCode;
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
                    promasterData.PromoterPassport = item.PromoterMaster.PromoterPassport ?? "Test";
                    promasterData.PromoterPhoto = item.PromoterMaster.PromoterPhoto;
                    await _promoterMasterRepository.UpdateAsync(promasterData, token).ConfigureAwait(false);
                    PromoterMasterDTO = _mapper.Map<PromoterMasterDTO>(promasterData);
                    await _work.CommitAsync(token).ConfigureAwait(false);
                }


                var enqPromoter = new TblEnqGuarDet
                {
                    EnqGuarcibil = item.EnqGuarcibil,
                    PromoterCode = promoterCode,
                    CreatedBy = _userInfo.Pan,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false,
                    DomCd = item.DomCd,
                    EnqtempId = item.EnqtempId,
                    UniqueId = Guid.NewGuid().ToString()
                };

                var promoter = await _guarantorRepository.AddAsync(enqPromoter, token).ConfigureAwait(false);

                var gBankDetails = new TblEnqGbankDet
                {
                    CreatedBy = _userInfo.Pan,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false,
                    EnqtempId = item.EnqtempId,
                    GuarBankaccno = item.GuarantorBankDetails.GuarBankaccno,
                    GuarAcctype = item.GuarantorBankDetails.GuarAcctype,
                    GuarAccName = item.GuarantorBankDetails.GuarAccName,
                    GuarBankbr = item.GuarantorBankDetails.GuarBankbr,
                    GuarBankname = item.GuarantorBankDetails.GuarBankname,
                    GuarIfsc = item.GuarantorBankDetails.GuarIfsc,
                    PromoterCode = promoterCode,
                    UniqueId = Guid.NewGuid().ToString()
                };

                var guarantorBank = await _guarantorBankRepository.AddAsync(gBankDetails, token).ConfigureAwait(false);
                await _work.CommitAsync(token).ConfigureAwait(false);

                var promoterBankDto = _mapper.Map<GuarantorBankDetailsDTO>(guarantorBank);
                var promoterDto = _mapper.Map<GuarantorDetailsDTO>(promoter);


                var guarantorDetailsObj = new GuarantorDetailsDTO
                {
                    GuarantorBankDetails = promoterBankDto,
                    PromoterMaster = PromoterMasterDTO,
                    DomCd = promoterDto.DomCd,
                    EnqtempId = promoterDto.EnqtempId,
                    PromoterCode = promoterDto.PromoterCode,
                    EnqGuarcibil = promoterDto.EnqGuarcibil,
                    EnqGuarId = promoterDto.EnqGuarId,
                    UniqueId = promoterDto.UniqueId
                };
                guarantorList.Add(guarantorDetailsObj);
            }

            return guarantorList;
        }
    }
}
