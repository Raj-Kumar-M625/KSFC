using AutoMapper;
using EDCS_TG.API.Data;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class OfficeService : IOfficeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly KarmaniDbContext _karmaniDbContext;


        public OfficeService(IMapper mapper, IUnitOfWork unitOfWork, KarmaniDbContext karmaniDbContext)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _karmaniDbContext = karmaniDbContext;
        }

        public async Task<IList<Office>> getAllOffficeList(string StateCode)
        {
            try
            {
                //var officeData = await _unitOfWork.officeRepository.FindByCondition(x => x.StateCode == StateCode);
                //var officeList = officeData.ToList();
                //return officeList;
                var officeData = _karmaniDbContext.Office.Select(x=> new Office { DistrictName = x.DistrictName, DistrictCode = x.DistrictCode, DistrictName_KA = x.DistrictName_KA }).Distinct().ToList();
                return officeData;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Office>> GetTalukList(string districtCode)
        {
            try
            {
                var talukData = await _unitOfWork.officeRepository.FindByCondition(t => t.DistrictCode == districtCode);
                var taluks = talukData.ToList();
                return taluks;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Office>> GetHobliList(string talukCode)
        {
            try
            {
                var HobliData = await _unitOfWork.officeRepository.FindByCondition(t => t.TalukOrTownCode == talukCode);
                var HobliList = HobliData.ToList();
                return HobliList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Office>> GetVillageList(string hobliCode)
        {
            try
            {
                var VillageData = await _unitOfWork.officeRepository.FindByCondition(t => t.HobliOrZoneCode == hobliCode);
                var VillageList = VillageData.ToList();
                return VillageList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
