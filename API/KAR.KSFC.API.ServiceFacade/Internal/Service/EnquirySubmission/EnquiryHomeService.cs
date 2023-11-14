using AutoMapper;

using KAR.KSFC.API.ServiceFacade.External.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.Email;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Templates.Email;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission
{
    public class EnquiryHomeService : IEnquiryHomeService
    {
        private readonly IEntityRepository<TblEnqGuarDet> _guarantorRepository;
        private readonly IEntityRepository<TblEnqPromDet> _promoterRepository;
        private readonly IEntityRepository<TblEnqTemptab> _enqRepository;
        private readonly IEntityRepository<TblEnqPjmfDet> _enqMeansOfFinRepository;
        private readonly IEntityRepository<TblEnqPjfinDet> _financialRepository;
        private readonly IEntityRepository<TblEnqPjcostDet> _projectCostRepository;
        private readonly IEntityRepository<TblEnqDocument> _documentRepository;
        private readonly IEntityRepository<TblEnqTemptab> _basicDetailsRepository;
        private readonly IEntityRepository<TblEnqSecDet> _securityRepository;
        private readonly IEntityRepository<TblBankfacilityCdtab> _tblBankfacilityCdtab;
        private readonly ISisterConcernFinancialDetails _enqSisFinService;
        private readonly IEntityRepository<TblDomiCdtab> _tblDomiCdtab;
        private readonly IBasicDetails _basicDetails;
        private readonly ICommonService _commonDetails;
        private readonly IUnitOfWork _work;
        private readonly IEntityRepository<EnqAckDet> _enqackdetRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;
        public EnquiryHomeService(IEntityRepository<TblEnqTemptab> enqRepository,
            UserInfo userInfo, IMapper mapper = null,
            IBasicDetails basicDetails = null,
            IEntityRepository<TblEnqPromDet> promoterRepository = null,
            IEntityRepository<TblEnqGuarDet> guarantorRepository = null,
            ISisterConcernFinancialDetails enqSisFinService = null,
            IEntityRepository<TblEnqPjmfDet> enqMeansOfFinRepository = null,
            IEntityRepository<TblEnqPjfinDet> financialRepository = null,
            IEntityRepository<TblEnqPjcostDet> projectCostRepository = null,
            IEntityRepository<TblEnqDocument> documentRepository = null,
            IUnitOfWork work = null,
            ICommonService commonDetails = null,
            IEntityRepository<TblEnqTemptab> basicDetailsRepository = null,
            IEntityRepository<TblEnqSecDet> securityRepository = null,
            IEntityRepository<TblBankfacilityCdtab> TblBankfacilityCdtab = null,
           
            IEntityRepository<TblDomiCdtab> tblDomiCdtab = null, ISmsService smsService = null, IEmailService emailService = null, IEntityRepository<EnqAckDet> enqackdetRepository = null)
        {
            _enqackdetRepository = enqackdetRepository;
            _enqRepository = enqRepository;
            _userInfo = userInfo;
            _mapper = mapper;
            _basicDetails = basicDetails;
            _promoterRepository = promoterRepository;
            _guarantorRepository = guarantorRepository;
            _enqSisFinService = enqSisFinService;
            _enqMeansOfFinRepository = enqMeansOfFinRepository;
            _financialRepository = financialRepository;
            _projectCostRepository = projectCostRepository;
            _documentRepository = documentRepository;
            _work = work;
            _commonDetails = commonDetails;
            _basicDetailsRepository = basicDetailsRepository;
            _securityRepository = securityRepository;
            _tblBankfacilityCdtab = TblBankfacilityCdtab;
            _smsService = smsService;
            _emailService = emailService;
        }

        public async Task<bool> DeleteEnquiryAsync(int enquiryId, CancellationToken token)
        {
            var enquiry = await _enqRepository
                                     .FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EnqtempId == enquiryId
                                        && x.IsActive == true && x.IsDeleted == false)
                                     .ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("No Data Found");
            }

            enquiry.IsDeleted = true;
            enquiry.IsActive = false;
            enquiry.ModifiedBy = _userInfo.UserId;
            enquiry.ModifiedDate = DateTime.UtcNow;

            await _enqRepository.UpdateAsync(enquiry, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }
        public async Task<bool> UpdateEnquiryStatus(int enquiryId, int EnqStatus, CancellationToken token)
        {
            var enquiry = await _enqRepository
                                     .FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EnqtempId == enquiryId
                                        && x.IsActive == true && x.IsDeleted == false)
                                     .ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("No Data Found");
            }


            enquiry.ModifiedBy = _userInfo.UserId;
            enquiry.ModifiedDate = DateTime.UtcNow;
            enquiry.EnqStatus = EnqStatus;
            await _enqRepository.UpdateAsync(enquiry, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);

            ///update info in Enq Acknowledge table if status is acknowledge
            if (EnqStatus == (int)KAR.KSFC.Components.Common.Dto.Enums.EnqStatus.Acknowledge)
            {
               await UpdateEnqAckDetTable(enquiry, token);

            }
            return true;
        }
        private async Task UpdateEnqAckDetTable(TblEnqTemptab enquiry, CancellationToken token)
        {
            var ack_det = await _enqackdetRepository.FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EnqRefNo == enquiry.EnqRefNo);
            if (ack_det != null)
            {
                ack_det.EmpId = _userInfo.UserId;
                ack_det.EnqRefNo = enquiry.EnqRefNo;
                ack_det.EnqAckDate = DateTime.Now;
                await _enqackdetRepository.UpdateAsync(ack_det, token).ConfigureAwait(false);
            }
            else
            {
                var item = new EnqAckDet
                {
                    EmpId = _userInfo.UserId,
                    EnqRefNo = enquiry.EnqRefNo,
                    EnqAckDate = DateTime.Now
                };
                await _enqackdetRepository.AddAsync(item, token).ConfigureAwait(false);

            }
            await _work.CommitAsync(token).ConfigureAwait(false);
        }
        public async Task<IEnumerable<EnquirySummary>> GetAllEnquiryAsync(CancellationToken token)
        {
            var data = await _basicDetailsRepository.FindByMatchingPropertiesAsync(token, x => x.IsActive == true
                                                && x.IsDeleted == false && x.PromPan == _userInfo.Pan).ConfigureAwait(false);
            var filterData = data.Select(x => new EnquirySummary
            {
                EnquiryId = x.EnqtempId,
                PromotorPan = x.PromPan,
                EnqStatus = x.EnqStatus,
                EnqInitiateDate = x.EnqInitDate,
                EnqSubmitDate = x.EnqSubmitDate
            });
            return filterData.OrderByDescending(x => x.EnquiryId).ToList();


        }
        public async Task<IEnumerable<EnquirySummary>> GetAllEnquiriesForAdminAsync(CancellationToken token)
        {
            var data = await _basicDetailsRepository.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.EnqStatus != (int)EnqStatus.Draft
                                                && x.IsDeleted == false).ConfigureAwait(false);
            var filterData = data.Select(x => new EnquirySummary
            {
                EnquiryId = x.EnqtempId,
                PromotorPan = x.PromPan,
                EnqStatus = x.EnqStatus,
                EnqInitiateDate = x.EnqInitDate,
                EnqSubmitDate = x.EnqSubmitDate
            });
            return filterData.OrderByDescending(x => x.EnquiryId).ToList();

        }

        /// <summary>
        /// Get Enquiry By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>7
        /// <returns></returns>
        public async Task<EnquiryDTO> GetEnquiryByIdAsync(int Id, CancellationToken token)
        {
            var data = await _enqRepository.FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false && x.EnqtempId == Id,
                                incBank => incBank.TblEnqBankDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incAddress => incAddress.TblEnqAddressDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incReg => incReg.TblEnqRegnoDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incProm => incProm.TblEnqPromDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incPBank => incPBank.TblEnqPbankDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incPAsset => incPAsset.TblEnqPassetDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incPliability => incPliability.TblEnqPliabDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incPnw => incPnw.TblEnqPnwDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incGuar => incGuar.TblEnqGuarDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incGBank => incGBank.TblEnqGbankDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incGuaLia => incGuaLia.TblEnqGliabDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incGuaNw => incGuaNw.TblEnqGnwDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incGuaAsset => incGuaAsset.TblEnqGassetDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incSis => incSis.TblEnqSisDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incProjWCDets => incProjWCDets.TblEnqWcDets.Where(x => x.IsActive == true && x.IsDeleted == false),
                                incSecDets => incSecDets.TblEnqSecDets.Where(x => x.IsActive == true && x.IsDeleted == false)

                ).ConfigureAwait(false);

            if (data == null)
            {
                throw new ArgumentException("No Data Found!");
            }

            var basicDetails = await _basicDetails.GetBasicDetails(Id, token).ConfigureAwait(false);
            if (basicDetails != null)
            {
                var dataLocation = await _commonDetails.GetVillageTalukaHobliDistAsync(basicDetails.VilCd, token).ConfigureAwait(false);
                basicDetails.VillageName = dataLocation.VillageName;
                basicDetails.HobliCd = dataLocation.HobliCode;
                basicDetails.DistrictCd = dataLocation.DistrictCode;
                basicDetails.TalukaCd = dataLocation.TalukaCode;
                basicDetails.Hobli = dataLocation.HobliName;
                basicDetails.Taluk = dataLocation.TalukaName;
                basicDetails.District = dataLocation.DistrictName;
            }
            var listSisFinDet = new List<SisterConcernFinancialDetailsDTO>();
            foreach (var item in data.TblEnqSisDets)
            {
                item.BfacilityCodeNavigation = await _tblBankfacilityCdtab.FirstOrDefaultByExpressionAsync(x => x.BfacilityCode == item.BfacilityCode, token);
                var sisFinDet = await _enqSisFinService.GetByIdSisterConcernFinancialDetails(item.EnqSisId, token).ConfigureAwait(false);
                listSisFinDet.AddRange(sisFinDet);
            }
            var meansOfFin = await _enqMeansOfFinRepository.FindByMatchingPropertiesAsync(token, x => x.EnqtempId == Id && x.IsActive == true && x.IsDeleted == false,
                                inc => inc.PjmfCdNavigation,
                                incCat => incCat.MfcatCdNavigation).ConfigureAwait(false);

            var projFinanceDet = await _financialRepository.FindByMatchingPropertiesAsync(token, x => x.EnqtempId == Id && x.IsActive == true && x.IsDeleted == false,
                                inc => inc.FincompCdNavigation,
                                incCode => incCode.FinyearCodeNavigation).ConfigureAwait(false);

            var projCostDet = await _projectCostRepository.FindByMatchingPropertiesAsync(token, x => x.EnqtempId == Id && x.IsActive == true && x.IsDeleted == false,
                            inc => inc.PjcostCdNavigation).ConfigureAwait(false);

            var documentlist = await _documentRepository.FindByExpressionAsync(x => x.EnquiryId == Id && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            var securityList = await _securityRepository.FindByMatchingPropertiesAsync(token, x => x.EnqtempId == Id && x.IsActive == true && x.IsDeleted == false,
                                inc => inc.SecCdNavigation, inc1 => inc1.SecCodeNavigation).ConfigureAwait(false);


            var newResult = new EnquiryDTO();

            newResult.BasicDetails = basicDetails;
            newResult.AddressDetails = data?.TblEnqAddressDets != null ? _mapper.Map<List<AddressDetailsDTO>>(data.TblEnqAddressDets) : null;
            newResult.BankDetails = data?.TblEnqBankDets != null ? _mapper.Map<BankDetailsDTO>(data.TblEnqBankDets.FirstOrDefault()) : null;
            newResult.RegistrationDetails = data?.TblEnqRegnoDets != null ? _mapper.Map<List<RegistrationNoDetailsDTO>>(data.TblEnqRegnoDets) : null;
            newResult.PromoterDetails = await GetPromoterList(Id, data, token).ConfigureAwait(false);
            newResult.PromoterAssetsDetails = data?.TblEnqPassetDets != null ? _mapper.Map<List<PromoterAssetsNetWorthDTO>>(data.TblEnqPassetDets) : null;
            newResult.PromoterLiability = data?.TblEnqPliabDets != null ? _mapper.Map<List<PromoterLiabilityDetailsDTO>>(data.TblEnqPliabDets) : null;
            newResult.PromoterNetWorth = data?.TblEnqPnwDets != null ? _mapper.Map<List<PromoterNetWorthDetailsDTO>>(data.TblEnqPnwDets) : null;
            newResult.GuarantorDetails = await GetGuarantorList(Id, data, token).ConfigureAwait(false);
            newResult.GuarantorAssetsDetails = data?.TblEnqGassetDets != null ? _mapper.Map<List<GuarantorAssetsNetWorthDTO>>(data.TblEnqGassetDets) : null;
            newResult.GuarantorLiability = data?.TblEnqGliabDets != null ? _mapper.Map<List<GuarantorLiabilityDetailsDTO>>(data.TblEnqGliabDets) : null;
            newResult.GuarantorNetWorth = data?.TblEnqGnwDets != null ? _mapper.Map<List<GuarantorNetWorthDetailsDTO>>(data.TblEnqGnwDets) : null;
            newResult.SisterConcernDetails = data?.TblEnqSisDets != null ? _mapper.Map<List<SisterConcernDetailsDTO>>(data.TblEnqSisDets) : null;
            newResult.SisterConcernFinancialDetails = listSisFinDet;
            newResult.WorkingCapitalDetails = data?.TblEnqWcDets != null ? _mapper.Map<ProjectWorkingCapitalDeatailsDTO>(data.TblEnqWcDets.FirstOrDefault()) : null;
            newResult.ProjectMeansOfFinanceDetails = BindMeansOfFinance(meansOfFin);
            newResult.ProjectFinancialYearDetails = projFinanceDet != null ? _mapper.Map<List<ProjectFinancialYearDetailsDTO>>(projFinanceDet) : null;
            newResult.ProjectCostDetails = projCostDet != null ? _mapper.Map<List<ProjectCostDetailsDTO>>(projCostDet) : null;
            newResult.SecurityDetails = data?.TblEnqSecDets != null ? _mapper.Map<List<SecurityDetailsDTO>>(securityList) : null;
            newResult.DocumentList = documentlist != null ? _mapper.Map<IEnumerable<EnqDocumentDTO>>(documentlist) : null;
            newResult.Status = data.EnqStatus;
            newResult.SummaryNote = data.EnqNote;
            newResult.EnquiryRefNo = data.EnqRefNo;
            newResult.EnquiryRefNo = data.EnqRefNo;
            newResult.HasAssociateSisterConcern = data.HasAssociateSisterConcern;
            return newResult;
        }


        private List<ProjectMeansOfFinanceDTO> BindMeansOfFinance(IEnumerable<TblEnqPjmfDet> data)
        {
            var list = new List<ProjectMeansOfFinanceDTO>();
            foreach (var item in data.ToList())
            {
                var financeData = _mapper.Map<ProjectMeansOfFinanceDTO>(item);
                financeData.MfcatCdNavigation = _mapper.Map<PjmfcatCdtabMasterDTO>(item.MfcatCdNavigation);
                financeData.PjmfCdNavigation = _mapper.Map<PjmfCdtabMasterDTO>(item.PjmfCdNavigation);
                list.Add(financeData);
            }
            return list;
        }

        private async Task<List<PromoterDetailsDTO>> GetPromoterList(int id, TblEnqTemptab enq, CancellationToken token)
        {
            var enqPromoterDetails = await _promoterRepository.FindByMatchingPropertiesAsync(token,
                x => x.EnqtempId == id && x.IsActive == true && x.IsDeleted == false,
                include => include.PromoterCodeNavigation,
                des => des.PdesigCdNavigation,
                dom => dom.DomCdNavigation).ConfigureAwait(false);
            if (enqPromoterDetails.Any())
            {
                var promDetailsList = new List<PromoterDetailsDTO>();
                foreach (var item in enqPromoterDetails)
                {
                    var promoterBank = enq.TblEnqPbankDets.FirstOrDefault(x => x.PromoterCode == item.PromoterCode && x.IsActive == true && x.IsDeleted == false);

                    var promDetails = new PromoterDetailsDTO
                    {
                        PromoterBankDetails = promoterBank != null ? _mapper.Map<PromoterBankDetailsDTO>(promoterBank) : null,
                        PromoterMaster = item.PromoterCodeNavigation != null ? _mapper.Map<PromoterMasterDTO>(item.PromoterCodeNavigation) : null,
                        DomCd = item.DomCd,
                        EnqCibil = item.EnqCibil,
                        EnqPromExp = item.EnqPromExp,
                        EnqPromExpdet = item.EnqPromExpdet,
                        EnqPromId = item.EnqPromId,
                        EnqPromShare = item.EnqPromShare,
                        EnqtempId = item.EnqtempId,
                        PdesigCd = item.PdesigCd,
                        PromoterCode = item.PromoterCode,
                        UniqueId = item.UniqueId,
                        PdesigCdNavigation = item.PdesigCdNavigation != null ? _mapper.Map<PromDesignationMasterDTO>(item.PdesigCdNavigation) : null,
                        PromoDomText = item.DomCdNavigation.DomDets,
                        DomCdNavigation = item.DomCdNavigation != null ? _mapper.Map<DomicileMasterDTO>(item.DomCdNavigation) : null,
                    };
                    promDetailsList.Add(promDetails);
                }
                return promDetailsList;
            }
            return null;
        }

        private async Task<List<GuarantorDetailsDTO>> GetGuarantorList(int id, TblEnqTemptab enq, CancellationToken token)
        {
            var enqGuarantorDetails = await _guarantorRepository.FindByMatchingPropertiesAsync(
                token, x => x.EnqtempId == id && x.IsActive == true && x.IsDeleted == false,
                include => include.PromoterCodeNavigation).ConfigureAwait(false);

            if (enqGuarantorDetails.Any())
            {
                var guaDetailsList = new List<GuarantorDetailsDTO>();
                foreach (var item in enqGuarantorDetails)
                {

                    var guarantorBank = enq.TblEnqGbankDets.FirstOrDefault(x => x.PromoterCode == item.PromoterCode && x.IsActive == true && x.IsDeleted == false);

                    var guaDetails = new GuarantorDetailsDTO
                    {
                        GuarantorBankDetails = guarantorBank != null ? _mapper.Map<GuarantorBankDetailsDTO>(guarantorBank) : null,
                        PromoterMaster = item?.PromoterCodeNavigation != null ? _mapper.Map<PromoterMasterDTO>(item.PromoterCodeNavigation) : null,
                        DomCd = item.DomCd,
                        EnqGuarId = item.EnqGuarId,
                        EnqtempId = item.EnqtempId,
                        GuarName = item.PromoterCodeNavigation.PromoterName,
                        Pan = item.PromoterCodeNavigation.PromoterPan,
                        EnqGuarcibil = item.EnqGuarcibil,
                        PromoterCode = item.PromoterCode,
                        UniqueId = item.UniqueId,
                    };
                    guaDetailsList.Add(guaDetails);

                }

                return guaDetailsList;
            }
            return null;
        }

        public async Task<int> SubmitEnquiry(string summary, int enquiryId, CancellationToken token)
        {

            var enq = await _enqRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (enq != null)
            {
                Random rnd = new Random();
                int num = rnd.Next();
                enq.EnqStatus = (int)EnqStatus.Submitted;
                enq.EnqSubmitDate = DateTime.Now;
                enq.EnqNote = summary;
                enq.EnqRefNo = num;
                await _enqRepository.UpdateAsync(enq, token).ConfigureAwait(false);
                await _work.CommitAsync(token).ConfigureAwait(false);

                await _smsService.SendSms(new Components.Common.Dto.SMS.SmsDataModel
                {
                    Message = EmailTemplate.GetEnquirySubmission(num),
                    MobileNumber = _userInfo?.PhoneNumber
                });

                var basicDetails = await _basicDetails.GetBasicDetails(enquiryId, token).ConfigureAwait(false);

                var emailReq = new EmailServiceRequest
                {
                    Body = EmailTemplate.GetEnquirySubmission(num),
                    Subject = $"KSFC Enquiry Submission",
                    ToEmail = basicDetails?.EnqEmail
                };

                await _emailService.SendEmailAsync(emailReq);

                return (int)enq.EnqRefNo;
            }
            return 0;
        }

        public async Task<int> AddNewEnqiry(string pan, CancellationToken token)
        {
            TblEnqTemptab enq = new TblEnqTemptab
            {
                UniqueId = Guid.NewGuid().ToString(),
                EnqStatus = (int)EnqStatus.Draft,
                CreatedBy = _userInfo.Pan,
                CreatedDate = DateTime.UtcNow,
                EnqInitDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false,
                PromPan = pan,
                HasAssociateSisterConcern = false

            };
            var tempEnq = await _enqRepository.AddAsync(enq, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return tempEnq.EnqtempId;
        }

        public async Task<int> UpdateEnquiryAssociateSisterConcernStatus(int enquiryId, bool Status, CancellationToken token)
        {

            if (enquiryId == 0)
            {
                enquiryId = await AddNewEnqiry(_userInfo.Pan, token).ConfigureAwait(false);

            }
            var enquiry = await _enqRepository
                                     .FirstOrDefaultNoTrackingByExpressionAsync(x => x.EnqtempId == enquiryId
                                        && x.IsActive == true && x.IsDeleted == false, token)
                                     .ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("No Data Found");
            }


            enquiry.ModifiedBy = _userInfo.Pan;
            enquiry.ModifiedDate = DateTime.UtcNow;
            enquiry.HasAssociateSisterConcern = Status;
            await _enqRepository.UpdateAsync(enquiry, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);

            return enquiryId;
        }
    }
}
