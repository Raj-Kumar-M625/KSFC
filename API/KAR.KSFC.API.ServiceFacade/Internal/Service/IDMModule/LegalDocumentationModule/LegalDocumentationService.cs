using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.LegalDocumentationModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.LegalDocumentationModule
{
    public class LegalDocumentationService : ILegalDocumentationService

    {
        private readonly IEntityRepository<TblIdmDeedDet> _deedRepository;
        private readonly IEntityRepository<TblIdmHypothDet> _hypothRepository;
        private readonly IEntityRepository<TblIdmDsbCharge> _securitychargeRepository;
        private readonly IEntityRepository<TblIdmCersaiRegistration> _cersairegistrationRepository;
        private readonly IEntityRepository<TblIdmGuarDeedDet> _guarantorDeedRepository;
        private readonly IEntityRepository<TblIdmCondDet> _idmConditionRepository;
        private readonly IEntityRepository<TblAssetRefnoDet> _assetRefDet;
        private readonly IEntityRepository<TblAssettypeCdtab> _assetType;
        private readonly IEntityRepository<TblSecurityRefnoMast> _secruityrefNo;
        private readonly IEntityRepository<TblIdmHypothMap> _hypothMapDet;
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;

        public LegalDocumentationService(IEntityRepository<TblIdmDeedDet> deedRepository,
            IEntityRepository<TblIdmHypothDet> hypothRepository, IEntityRepository<TblIdmDsbCharge> securitychargeRepository,
            IEntityRepository<TblIdmCersaiRegistration> cersairegistrationRepository, IEntityRepository<TblIdmGuarDeedDet> guarantorDeedRepository,
            IEntityRepository<TblIdmCondDet> idmConditionRepository,
            IEntityRepository<TblAssetRefnoDet> assetRefDet, IEntityRepository<TblAssettypeCdtab> assetType,
           IUnitOfWork work, IMapper mapper, UserInfo userInfo, IEntityRepository<TblSecurityRefnoMast> secruityrefNo, IEntityRepository<TblIdmHypothMap> hypothMapDet)
        {
            _deedRepository = deedRepository;
            _hypothRepository = hypothRepository;
            _securitychargeRepository = securitychargeRepository;
            _cersairegistrationRepository = cersairegistrationRepository;
            _guarantorDeedRepository = guarantorDeedRepository;
            _idmConditionRepository = idmConditionRepository;
            _hypothMapDet = hypothMapDet;
            _assetRefDet = assetRefDet;
            _assetType = assetType;
            _work = work;
            _mapper = mapper;
            _userInfo = userInfo;
            _secruityrefNo = secruityrefNo;
            _hypothMapDet = hypothMapDet;
        }

        #region Primary Security

        /// <summary>
        ///  Author: Rajesh; Module: Primary/CollateralSecurity; Date:15/07/2022
        ///  Modfied By: Swetha M Reason:Data was not pupolating while passing different Loan Account Number
        /// </summary>
        public async Task<IEnumerable<IdmSecurityDetailsDTO>> GetPrimaryCollateralListAsync(long accountNumber, CancellationToken token)
        {
            var data = await _deedRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);

            var securityData = await _secruityrefNo.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);

            var result = data.Join(securityData,
                d => d.SecurityCd,
                sec => sec.SecurityCd,
                (d, sec) => new IdmSecurityDetailsDTO
                {
                    TblSecurityRefnoMast = _mapper.Map<SecurityMasterDTO>(sec),
                    SecurityValue = d.SecurityValue,
                    IdmDeedDetId = d.IdmDeedDetId,
                    DeedNo = d.DeedNo,
                    ExecutionDate = d.ExecutionDate,
                    LoanAcc = d.LoanAcc,
                    LoanSub = d.LoanSub,
                    OffcCd = d.OffcCd,
                    DeedDesc = d.DeedDesc,
                    PjSecDets = d.PjSecDets,
                    PjSecNam = d.PjSecNam,
                    SecurityCd = d.SecurityCd,
                    SubregistrarCd = d.SubregistrarCd,
                    IsActive = d.IsActive,
                    IsDeleted = d.IsDeleted
                }).ToList();

            return _mapper.Map<List<IdmSecurityDetailsDTO>>(result);
        }

        public async Task<bool> UpdatePrimaryCollateralDetail(IdmSecurityDetailsDTO lDSecurityDetailsDTO, CancellationToken token)
        {
            var currentDetails = await _deedRepository.FirstOrDefaultByExpressionAsync(x => x.IdmDeedDetId == lDSecurityDetailsDTO.IdmDeedDetId && x.IsActive == true && x.IsDeleted == false, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ApprovedEmpId = _userInfo.UserId;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _deedRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmDeedDet>(lDSecurityDetailsDTO);
                basicDetails.IdmDeedDetId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ApprovedEmpId = _userInfo.UserId;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;
                basicDetails.SecurityValue = lDSecurityDetailsDTO.SecurityValue / 100000;
                await _deedRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

                if (lDSecurityDetailsDTO.DeedNo != null)
                {
                    var tbldcbcharge = new TblIdmDsbCharge();
                    tbldcbcharge.LoanAcc = lDSecurityDetailsDTO.LoanAcc;
                    tbldcbcharge.LoanSub = lDSecurityDetailsDTO.LoanSub;
                    tbldcbcharge.OffcCd = lDSecurityDetailsDTO.OffcCd;
                    tbldcbcharge.IsActive = true;
                    tbldcbcharge.IsDeleted = false;
                    tbldcbcharge.CreateBy = _userInfo.Name;
                    tbldcbcharge.CreatedDate = DateTime.UtcNow;
                    tbldcbcharge.SecurityValue = lDSecurityDetailsDTO.SecurityValue / 100000;
                    tbldcbcharge.SecurityDets = lDSecurityDetailsDTO.PjSecDets;
                    tbldcbcharge.SecurityCd = 1;
                    tbldcbcharge.ChargeTypeCd = 1;
                    //tbldcbcharge.nocissueby = "bank";
                    //tbldcbcharge.nocissueto = "customer";
                    //tbldcbcharge.nocdate = DateTime.Now;
                    //tbldcbcharge.AuthLetterBy = "Bank";
                    //tbldcbcharge.AuthLetterDate = DateTime.Now;
                    //tbldcbcharge.BoardResolutionDate = DateTime.Now;
                    //tbldcbcharge.MoeInsuredDate = DateTime.Now;
                    //tbldcbcharge.RequestLtrDate = DateTime.Now;
                    tbldcbcharge.BankIfscCd = "AJAR0000009";
                    tbldcbcharge.BankRequestLtrNo = "5522";
                    tbldcbcharge.ChargePurpose = "Bank";
                    tbldcbcharge.ChargeDetails = "Bank";
                    tbldcbcharge.ChargeConditions = "Guarantee";

                    await _securitychargeRepository.AddAsync(tbldcbcharge, token).ConfigureAwait(false);

                }
                await _work.CommitAsync(token).ConfigureAwait(false);

                return true;
            }

            return false;

        }

        #endregion

        #region Colletral Security

        public async Task<IEnumerable<IdmSecurityDetailsDTO>> GetCollateralListAsync(long accountNumber, CancellationToken token)
        {
            var data = await _deedRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);

            var securityData = await _secruityrefNo.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);

            var result = data.Join(securityData,
                d => d.SecurityCd,
                sec => sec.SecurityCd,
                (d, sec) => new IdmSecurityDetailsDTO
                {
                    TblSecurityRefnoMast = _mapper.Map<SecurityMasterDTO>(sec),
                    SecurityValue = d.SecurityValue,
                    IdmDeedDetId = d.IdmDeedDetId,
                    DeedNo = d.DeedNo,
                    ExecutionDate = d.ExecutionDate,
                    LoanAcc = d.LoanAcc,
                    LoanSub = d.LoanSub,
                    OffcCd = d.OffcCd,
                    DeedDesc = d.DeedDesc,
                    PjSecDets = d.PjSecDets,
                    PjSecNam = d.PjSecNam,
                    SecurityCd = d.SecurityCd,
                    SubregistrarCd = d.SubregistrarCd,
                    IsActive = d.IsActive,
                    IsDeleted = d.IsDeleted
                }).ToList();

            return _mapper.Map<List<IdmSecurityDetailsDTO>>(result);
        }

        public async Task<bool> UpdateCollateralDetail(IdmSecurityDetailsDTO lDSecurityDetailsDTO, CancellationToken token)
        {
            var currentDetails = await _deedRepository.FirstOrDefaultByExpressionAsync(x => x.IdmDeedDetId == lDSecurityDetailsDTO.IdmDeedDetId && x.IsActive == true && x.IsDeleted == false, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ApprovedEmpId = _userInfo.UserId;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _deedRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmDeedDet>(lDSecurityDetailsDTO);
                basicDetails.IdmDeedDetId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ApprovedEmpId = _userInfo.UserId;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;
                basicDetails.SecurityValue = lDSecurityDetailsDTO.SecurityValue / 100000;
                await _deedRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

                if (lDSecurityDetailsDTO.DeedNo != null)
                {
                    var tbldcbcharge = new TblIdmDsbCharge();
                    tbldcbcharge.LoanAcc = lDSecurityDetailsDTO.LoanAcc;
                    tbldcbcharge.LoanSub = lDSecurityDetailsDTO.LoanSub;
                    tbldcbcharge.OffcCd = lDSecurityDetailsDTO.OffcCd;
                    tbldcbcharge.IsActive = true;
                    tbldcbcharge.IsDeleted = false;
                    tbldcbcharge.CreateBy = _userInfo.Name;
                    tbldcbcharge.CreatedDate = DateTime.UtcNow;
                    tbldcbcharge.SecurityValue = lDSecurityDetailsDTO.SecurityValue / 100000;
                    tbldcbcharge.SecurityDets = lDSecurityDetailsDTO.PjSecDets;
                    tbldcbcharge.SecurityCd = 1;
                    tbldcbcharge.ChargeTypeCd = 1;
                    //tbldcbcharge.nocissueby = "bank";
                    //tbldcbcharge.nocissueto = "customer";
                    //tbldcbcharge.nocdate = DateTime.Now;
                    //tbldcbcharge.AuthLetterBy = "Bank";
                    //tbldcbcharge.AuthLetterDate = DateTime.Now;
                    //tbldcbcharge.BoardResolutionDate = DateTime.Now;
                    //tbldcbcharge.MoeInsuredDate = DateTime.Now;
                    //tbldcbcharge.RequestLtrDate = DateTime.Now;
                    tbldcbcharge.BankIfscCd = "AJAR0000009";
                    tbldcbcharge.BankRequestLtrNo = "5522";
                    tbldcbcharge.ChargePurpose = "Bank";
                    tbldcbcharge.ChargeDetails = "Bank";
                    tbldcbcharge.ChargeConditions = "Guarantee";

                    await _securitychargeRepository.AddAsync(tbldcbcharge, token).ConfigureAwait(false);

                }
                await _work.CommitAsync(token).ConfigureAwait(false);

                return true;
            }

            return false;

        }

        public async Task<bool> CreateCollateralDetail(IdmSecurityDetailsDTO lDSecurityDetailsDTO, CancellationToken token)
        {
            var currentDetails = await _deedRepository.FirstOrDefaultByExpressionAsync(x => x.IdmDeedDetId == lDSecurityDetailsDTO.IdmDeedDetId && x.IsActive == true && x.IsDeleted == false, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ApprovedEmpId = _userInfo.UserId;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _deedRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmDeedDet>(lDSecurityDetailsDTO);
                basicDetails.IdmDeedDetId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ApprovedEmpId = _userInfo.UserId;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;
                basicDetails.SecurityValue = lDSecurityDetailsDTO.SecurityValue / 100000;
                await _deedRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

                if (lDSecurityDetailsDTO.DeedNo != null)
                {
                    var tbldcbcharge = new TblIdmDsbCharge();
                    tbldcbcharge.LoanAcc = lDSecurityDetailsDTO.LoanAcc;
                    tbldcbcharge.LoanSub = lDSecurityDetailsDTO.LoanSub;
                    tbldcbcharge.OffcCd = lDSecurityDetailsDTO.OffcCd;
                    tbldcbcharge.IsActive = true;
                    tbldcbcharge.IsDeleted = false;
                    tbldcbcharge.CreateBy = _userInfo.Name;
                    tbldcbcharge.CreatedDate = DateTime.UtcNow;
                    tbldcbcharge.SecurityValue = lDSecurityDetailsDTO.SecurityValue / 100000;
                    tbldcbcharge.SecurityDets = lDSecurityDetailsDTO.PjSecDets;
                    tbldcbcharge.SecurityCd = 1;
                    tbldcbcharge.ChargeTypeCd = 1;
                    //tbldcbcharge.nocissueby = "bank";
                    //tbldcbcharge.nocissueto = "customer";
                    //tbldcbcharge.nocdate = DateTime.Now;
                    //tbldcbcharge.AuthLetterBy = "Bank";
                    //tbldcbcharge.AuthLetterDate = DateTime.Now;
                    //tbldcbcharge.BoardResolutionDate = DateTime.Now;
                    //tbldcbcharge.MoeInsuredDate = DateTime.Now;
                    //tbldcbcharge.RequestLtrDate = DateTime.Now;
                    tbldcbcharge.BankIfscCd = "AJAR0000009";
                    tbldcbcharge.BankRequestLtrNo = "5522";
                    tbldcbcharge.ChargePurpose = "Bank";
                    tbldcbcharge.ChargeDetails = "Bank";
                    tbldcbcharge.ChargeConditions = "Guarantee";

                    await _securitychargeRepository.AddAsync(tbldcbcharge, token).ConfigureAwait(false);

                }
                await _work.CommitAsync(token).ConfigureAwait(false);

                return true;
            }

            return false;

        }


        #endregion


        #region  Hypothecation
        /// <summary>
        ///  Author: Manoj; Module: Hypothecation; Date:21/07/2022
        ///  Modified:Swetha M Date:17/10/2022 Reason:While retrun Api response made it has bool 
        /// </summary>
        public async Task<IEnumerable<HypoAssetDetailDTO>> GetAllHypothecationAsync(long accountNumber, string paramater, CancellationToken token)
        {


            List<TblIdmHypothDet> Hypo = new();
            Hypo = paramater switch
            {
                "FilteredRecord" => await _hypothRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token),
                "AllRecords" => await _hypothRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber, token),
                _ => await _hypothRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token),
            };
            var assetRef = await _assetRefDet.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token);
            var assetType = await _assetType.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token);

            var data = Hypo.Join(assetRef,
                hyp => hyp.AssetCd,
                asset => asset.AssetCd,
                (hyp, asset) => new
                {
                    IdmHypothDetId = hyp.IdmHypothDetId,
                    LoanSub = hyp.LoanSub,
                    AssetCd = hyp.AssetCd,
                    OffcCd = hyp.OffcCd,
                    AssetOthDetails = asset.AssetOthDetails,
                    AssetDetails = asset.AssetDetails,
                    AssetTypeCd = asset.AssetypeCd,
                    AssetValuehypo = asset.AssetValue,
                    HypothNo = hyp.HypothNo,
                    HypothDesc = hyp.HypothDesc,
                    AssetValue = hyp.AssetValue,
                    ExecutionDate = hyp.ExecutionDate,
                    LoanAcc = hyp.LoanAcc,
                    IsActive = hyp.IsActive,
                    IsDeleted = hyp.IsDeleted,
                    CreatedDate = hyp.CreatedDate,
                    ModifiedDate = hyp.ModifiedDate,
                    AssetName = hyp.AssetName,
                    AssetDet = hyp.AssetDet,
                }).Join(assetType,
                hypo => hypo.AssetTypeCd,
                asstType => asstType.AssettypeCd,
                (hypo, asstType) => new
                {
                    IdmHypothDetId = hypo.IdmHypothDetId,
                    AssetOthDetails = hypo.AssetOthDetails,
                    AssetDetails = hypo.AssetDetails,
                    LoanSub = hypo.LoanSub,
                    AssetCd = hypo.AssetCd,
                    OffcCd = hypo.OffcCd,
                    AssetTypeCd = asstType.AssettypeCd,
                    HypothNo = hypo.HypothNo,
                    HypothDesc = hypo.HypothDesc,
                    AssetValue = hypo.AssetValue,
                    AssetValuehypo = hypo.AssetValuehypo,
                    ExecutionDate = hypo.ExecutionDate,
                    LoanAcc = hypo.LoanAcc,
                    AssetTypeDets = asstType.AssettypeDets,
                    IsActive = hypo.IsActive,
                    IsDeleted = hypo.IsDeleted,
                    CreatedDate = hypo.CreatedDate,
                    ModifiedDate = hypo.ModifiedDate,
                    AssetName = hypo.AssetName,
                    AssetDet = hypo.AssetDet,
                }).ToList();

            var result = new List<HypoAssetDetailDTO>();
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    result.Add(new HypoAssetDetailDTO()
                    {
                        IdmHypothDetId = i.IdmHypothDetId,
                        LoanSub = i.LoanSub,
                        AssetCd = i.AssetCd,
                        OffcCd = i.OffcCd,
                        AssetTypeCd = i.AssetTypeCd,
                        HypothNo = i.HypothNo,
                        HypothDesc = i.HypothDesc,
                        AssetValue = i.AssetValue,
                        ExecutionDate = i.ExecutionDate,
                        LoanAcc = i.LoanAcc,
                        AssetValuehypo = i.AssetValuehypo,
                        AssetOthDetails = i.AssetOthDetails,
                        AssetDetails = i.AssetDetails,
                        AssetTypeDets = i.AssetTypeDets,
                        IsActive = i.IsActive,
                        IsDeleted = i.IsDeleted,
                        CreatedDate = i.CreatedDate,
                        ModifiedDate = i.ModifiedDate,
                        AssetName = i.AssetName,
                        AssetDet = i.AssetDet,
                    });

                }
            }
            return result;
        }

        public async Task<IEnumerable<AssetRefnoDetailsDTO>> GetAllAssetRefListAsync(long accountNumber, CancellationToken token)
        {
            var assetRef = await _assetRefDet.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false,
                hypodet => hypodet.TblIdmHypothDet.Where(x => x.IsActive == true && x.IsDeleted == false), cerdet => cerdet.TblIdmCersaiRegistration.Where(x => x.IsActive == true && x.IsDeleted == false)
                ).ConfigureAwait(false);
            return _mapper.Map<List<AssetRefnoDetailsDTO>>(assetRef);
        }

        public async Task<bool> UpdateLDHypothecationDetails(IdmHypotheDetailsDTO lDHypotheDetailsDTO, CancellationToken token)
        {
            var currentDetails = await _hypothRepository.FirstOrDefaultByExpressionAsync(x => x.IdmHypothDetId == lDHypotheDetailsDTO.IdmHypothDetId && x.IsActive == true, token);
            if (currentDetails.HypothNo == null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _hypothRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmHypothDet>(lDHypotheDetailsDTO);
                basicDetails.IdmHypothDetId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreateBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;
                basicDetails.AssetValue = lDHypotheDetailsDTO.AssetValue / 100000;
                var response = await _hypothRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            else
            {
                var listofhypo = await _hypothRepository.FindByExpressionAsync(x => x.HypothNo == currentDetails.HypothNo && x.IsActive == true && x.IsDeleted == false, token);

                foreach (var items in listofhypo)
                {
                    if (items.IdmHypothDetId != lDHypotheDetailsDTO.IdmHypothDetId)
                    {
                        items.ModifiedDate = DateTime.UtcNow;
                        await _hypothRepository.UpdateAsync(items, token).ConfigureAwait(false);
                    }
                }
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _hypothRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmHypothDet>(lDHypotheDetailsDTO);
                basicDetails.IdmHypothDetId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreateBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;
                basicDetails.AssetValue = lDHypotheDetailsDTO.AssetValue / 100000;
                var response = await _hypothRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        }

        public async Task<bool> DeleteHypothecationDetails(IdmHypotheDetailsDTO lDHypotheDetailsDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmHypothDet>(lDHypotheDetailsDTO);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            var result = await _hypothRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateLDHypothecationDetails(IdmHypotheDetailsDTO lDHypotheDetailsDTO, bool createHypothMap, CancellationToken token)
        {
            //var currentDetails = await _hypothRepository.FirstOrDefaultByExpressionAsync(x => x.AssetValue == lDHypotheDetailsDTO.AssetValue&& x.IsActive == true && x.IsDeleted == false, token);

            //if (currentDetails != null)
            //{
            //    currentDetails.IsActive = false;
            //    currentDetails.IsDeleted = true;
            //    currentDetails.ModifiedBy = _userInfo.Name;
            //    currentDetails.ModifiedDate = DateTime.UtcNow;
            //    await _hypothRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

            //    var basicDetails1 = _mapper.Map<TblIdmHypothDet>(lDHypotheDetailsDTO);

            //    basicDetails1.IsActive = true;
            //    basicDetails1.IsDeleted = false;
            //    basicDetails1.CreateBy = _userInfo.Name;
            //    basicDetails1.ModifiedBy = _userInfo.Name;
            //    basicDetails1.CreatedDate = DateTime.UtcNow;
            //    basicDetails1.ModifiedDate = DateTime.UtcNow;

            //    var response1 = await _hypothRepository.AddAsync(basicDetails1, token).ConfigureAwait(false);
            //    await _work.CommitAsync(token).ConfigureAwait(false);
            //    if (response1 != null)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{

            //    var basicDetails = _mapper.Map<TblIdmHypothDet>(lDHypotheDetailsDTO);
            //    basicDetails.IdmHypothDetId = 0;
            //    basicDetails.IsActive = true;
            //    basicDetails.IsDeleted = false;
            //    basicDetails.CreateBy = _userInfo.Name;             
            //    basicDetails.CreatedDate = DateTime.UtcNow;             

            //    var response = await _hypothRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            //    await _work.CommitAsync(token).ConfigureAwait(false);
            //    if (response != null)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            var currentDetails = await _hypothRepository.FirstOrDefaultByExpressionAsync(x => x.IdmHypothDetId == lDHypotheDetailsDTO.IdmHypothDetId && x.IsActive == true && x.IsDeleted == false, token);
            if (currentDetails.HypothNo == null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _hypothRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmHypothDet>(lDHypotheDetailsDTO);
                basicDetails.IdmHypothDetId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreateBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;
                basicDetails.AssetValue = lDHypotheDetailsDTO.AssetValue / 100000;
                if (createHypothMap)
                {
                    var hypothication = new TblIdmHypothMap();
                    hypothication.HypothMapId = lDHypotheDetailsDTO.IdmHypothDetId;
                    hypothication.HypothCode = lDHypotheDetailsDTO.IdmHypothDetId;
                    hypothication.HypothDeedNo = basicDetails.HypothNo;
                    hypothication.HypothValue = lDHypotheDetailsDTO.HypothValue;
                    hypothication.IsActive = true;
                    hypothication.IsDeleted = false;
                    await _hypothMapDet.AddAsync(hypothication, token).ConfigureAwait(false);
                }

                var response = await _hypothRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

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
            else
            {
                //var listofhypo = await _hypothRepository.FindByExpressionAsync(x => x.HypothNo == currentDetails.HypothNo && x.IsActive == true && x.IsDeleted == false, token);

                //foreach (var items in listofhypo)
                //{
                //    if (items.IdmHypothDetId != lDHypotheDetailsDTO.IdmHypothDetId)
                //    {
                //        items.ModifiedDate = DateTime.UtcNow;
                //        await _hypothRepository.UpdateAsync(items, token).ConfigureAwait(false);
                //    }
                //}
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _hypothRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmHypothDet>(lDHypotheDetailsDTO);
                basicDetails.IdmHypothDetId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreateBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;
                basicDetails.AssetValue = lDHypotheDetailsDTO.AssetValue / 100000;

                if (createHypothMap)
                {
                    var hypothication = new TblIdmHypothMap();
                    hypothication.HypothMapId = lDHypotheDetailsDTO.IdmHypothDetId;
                    hypothication.HypothCode = lDHypotheDetailsDTO.IdmHypothDetId;
                    hypothication.HypothDeedNo = basicDetails.HypothNo;
                    hypothication.HypothValue = lDHypotheDetailsDTO.HypothValue;
                    hypothication.IsActive = true;
                    hypothication.IsDeleted = false;
                    await _hypothMapDet.AddAsync(hypothication, token).ConfigureAwait(false);
                }

                var response = await _hypothRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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



        }
        #endregion 

        #region SecurityCharge

        /// <summary>
        ///  Author: Sandeep; Module: SecurityCharge; Date:21/07/2022
        /// </summary>

        public async Task<IEnumerable<IdmSecurityChargeDTO>> GetAllSecurityChargeAsync(long accountNumber, CancellationToken token)
        {

            var data = await _securitychargeRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false && x.TblSecurityRefnoMast.WhCharge == true,
                       inSecRefno => inSecRefno.TblSecurityRefnoMast, Securitytype => Securitytype.TblSecurityRefnoMast.TblSecCdtab, secf => secf.TblSecurityRefnoMast.TblPjsecCdtab, ifsc => ifsc.TbIIfscMaster).ConfigureAwait(false);
            return _mapper.Map<List<IdmSecurityChargeDTO>>(data);
        }
        public async Task<bool> UpdateSecurityChargeDetails(IdmSecurityChargeDTO lDSecurityDetailsDTO, CancellationToken token)
        {
            var currentDetails = await _securitychargeRepository.FirstOrDefaultByExpressionAsync(x => x.IdmDsbChargeId == lDSecurityDetailsDTO.IdmDsbChargeId, token);
            currentDetails.IsDeleted = true;
            currentDetails.ModifiedBy = _userInfo.Name;
            currentDetails.ModifiedDate = DateTime.UtcNow;
            currentDetails.IsActive = false;

            await _securitychargeRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);


            var basicDetails = _mapper.Map<TblIdmDsbCharge>(lDSecurityDetailsDTO);
            basicDetails.IdmDsbChargeId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate = DateTime.UtcNow;

            var response = await _securitychargeRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        #region CERSAI
        // <summary>
        // Author: Gagana K; Module: CERSAIRegistration; Date:28/07/2022
        // </summary>
        public async Task<IEnumerable<IdmCersaiRegDetailsDTO>> GetAllCERSAIRegistrationAsync(long accountNumber, string parameter, CancellationToken token)
        {
            IEnumerable<TblIdmCersaiRegistration> data = new List<TblIdmCersaiRegistration>();
            switch (parameter)
            {
                case "filterRecord":
                    data = await _cersairegistrationRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false,
                                   inAssetRefno => inAssetRefno.TblAssetRefnoDet, Assettype => Assettype.TblAssetRefnoDet.TblAssettypeCdtab, Assetcategory => Assetcategory.TblAssetRefnoDet.TblAssetcatCdtab).ConfigureAwait(false);
                    break;
                case "AllRecords":
                    data = await _cersairegistrationRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber,
                                    inAssetRefno => inAssetRefno.TblAssetRefnoDet, Assettype => Assettype.TblAssetRefnoDet.TblAssettypeCdtab, Assetcategory => Assetcategory.TblAssetRefnoDet.TblAssetcatCdtab).ConfigureAwait(false);
                    break;
                case "default":
                    data = await _cersairegistrationRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false,
                                    inAssetRefno => inAssetRefno.TblAssetRefnoDet, Assettype => Assettype.TblAssetRefnoDet.TblAssettypeCdtab, Assetcategory => Assetcategory.TblAssetRefnoDet.TblAssetcatCdtab).ConfigureAwait(false);
                    break;
            };

            return _mapper.Map<List<IdmCersaiRegDetailsDTO>>(data);
        }

        public async Task<bool> CreateLDCersaiRegDetails(IdmCersaiRegDetailsDTO CersaiRegDTO, CancellationToken token)
        {
            var currentDetails = await _cersairegistrationRepository.FirstOrDefaultByExpressionAsync(x => x.IdmDsbChargeId == CersaiRegDTO.IdmDsbChargeId && x.IsActive == true && x.IsDeleted == false, token);

            if (currentDetails.CersaiRegNo == null)
            {
                currentDetails.IsActive = false;
                currentDetails.IsDeleted = true;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _cersairegistrationRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails1 = _mapper.Map<TblIdmCersaiRegistration>(CersaiRegDTO);
                basicDetails1.IdmDsbChargeId = 0;
                basicDetails1.IsActive = true;
                basicDetails1.IsDeleted = false;
                basicDetails1.CreateBy = _userInfo.Name;

                basicDetails1.CreatedDate = DateTime.UtcNow;


                var response = await _cersairegistrationRepository.AddAsync(basicDetails1, token).ConfigureAwait(false);
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
            else
            {
                var listofcersai = await _cersairegistrationRepository.FindByExpressionAsync(x => x.CersaiRegNo == currentDetails.CersaiRegNo && x.IsActive == true && x.IsDeleted == false, token);

                foreach (var items in listofcersai)
                {
                    if (items.IdmDsbChargeId != CersaiRegDTO.IdmDsbChargeId)
                    {
                        items.ModifiedDate = DateTime.UtcNow;
                        await _cersairegistrationRepository.UpdateAsync(items, token).ConfigureAwait(false);
                    }

                }

                currentDetails.IsActive = false;
                currentDetails.IsDeleted = true;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _cersairegistrationRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails1 = _mapper.Map<TblIdmCersaiRegistration>(CersaiRegDTO);
                basicDetails1.IdmDsbChargeId = 0;
                basicDetails1.IsActive = true;
                basicDetails1.IsDeleted = false;
                basicDetails1.CreateBy = _userInfo.Name;
                basicDetails1.CreatedDate = DateTime.UtcNow;
                // basicDetails1.ModifiedDate = DateTime.UtcNow;

                var response = await _cersairegistrationRepository.AddAsync(basicDetails1, token).ConfigureAwait(false);

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
        }
        public async Task<bool> DeleteLDCersaiDetails(IdmCersaiRegDetailsDTO CersaiRegDTO, CancellationToken token)
        {


            var basicDetails = _mapper.Map<TblIdmCersaiRegistration>(CersaiRegDTO);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var result = await _cersairegistrationRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region GuarantorDeed
        // <Summary>
        // Author: Akhiladevi D M; Module: GuarantorDeed; Date: 10/08/2022
        // <summary>
        public async Task<IEnumerable<IdmGuarantorDeedDetailsDTO>> GetAllGuarantorListAsync(long accountNumber, CancellationToken token)
        {
            var data = await _guarantorDeedRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, inGuar => inGuar.TblAppGuarAssetDet).ConfigureAwait(false);
            return _mapper.Map<List<IdmGuarantorDeedDetailsDTO>>(data);

        }
        public async Task<bool> UpdateLDGuarantorDeedDetails(IdmGuarantorDeedDetailsDTO GuarantorListDTO, CancellationToken token)
        {
            var currentDetails = await _guarantorDeedRepository.FirstOrDefaultByExpressionAsync(x => x.IdmGuarDeedId == GuarantorListDTO.IdmGuarDeedId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                currentDetails.IsActive = false;

                await _guarantorDeedRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
                var basicDetails = new TblIdmGuarDeedDet()
                {
                    IsActive = true,
                    IsDeleted = false,
                    LoanAcc = currentDetails.LoanAcc,
                    LoanSub = currentDetails.LoanSub,
                    OffcCd = currentDetails.OffcCd,
                    PromoterCode = currentDetails.PromoterCode,
                    AppGuarassetId = currentDetails.AppGuarassetId,
                    ValueAsset = currentDetails.ValueAsset,
                    ValueLiab = currentDetails.ValueLiab,
                    ValueNetWorth = currentDetails.ValueNetWorth,
                    DeedNo = GuarantorListDTO.DeedNo,
                    DeedDesc = GuarantorListDTO.DeedDesc,
                    ExcecutionDate = GuarantorListDTO.ExcecutionDate,
                    DeedUpload = currentDetails.DeedUpload,
                    ApprovedEmpId = currentDetails.ApprovedEmpId,
                    ModifiedBy = _userInfo.Name,
                    ModifiedDate = DateTime.UtcNow
                };
                var response = await _guarantorDeedRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            else
            {
                return false;
            }

        }

        #endregion

        #region Condition
        // <summary>
        //Author:Gagana; Module: condition ; Date: 13/08/2022
        // </summary>
        public async Task<IEnumerable<LDConditionDetailsDTO>> GetAllConditionListAsync(long accountNumber, CancellationToken token)
        {
            var data = await _idmConditionRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false,
                Condstg => Condstg.TblCondStageMast, Condtype => Condtype.TblCondTypeCdtab).ConfigureAwait(false);
            return _mapper.Map<List<LDConditionDetailsDTO>>(data);
        }

        public async Task<bool> UpdateLDConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            var currentDetails = await _idmConditionRepository.FirstOrDefaultByExpressionAsync(x => x.CondDetId == CondtionDTO.CondDetId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                currentDetails.IsActive = false;
                await _idmConditionRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
            }

            var basicDetails = _mapper.Map<TblIdmCondDet>(CondtionDTO);
            basicDetails.CondDetId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate = DateTime.UtcNow;

            var response = await _idmConditionRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        public async Task<bool> CreateLDConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmCondDet>(CondtionDTO);
            basicDetails.IsActive = true;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _idmConditionRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        public async Task<bool> DeleteLDCondtionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmCondDet>(CondtionDTO);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var result = await _idmConditionRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (result != null)
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
