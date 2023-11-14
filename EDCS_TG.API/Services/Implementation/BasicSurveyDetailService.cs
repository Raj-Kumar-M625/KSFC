using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class BasicSurveyDetailService:IBasicSurveyDetailService
    {
     
            private readonly IUnitOfWork _repository;
            private readonly IMapper _mapper;

            public BasicSurveyDetailService(IUnitOfWork repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
        public async Task<IEnumerable<BasicSurveyDetail>> GetBasicSurveyDetailListByUser(Guid UserId)
        {
            try
            {
                
                var users = await _repository.UserRepository.FindAll();
                var result = await _repository.BasicSurveyDetailRepository.FindByCondition(item => item.UserId == UserId);
                //var query = from survey in result join 
                //             user in users on survey.UserId equals user.Id into UserDetails
                //            from Details in UserDetails.DefaultIfEmpty()
                //            select new { survey, Details};
                
                
                
                var resultData = _mapper.Map<IEnumerable<BasicSurveyDetail>>(result);
                
                var res = resultData.OrderByDescending(T => T.SurveyId);

                return res;
            }
            catch(Exception ex)
            {
                throw;
            }
                
        }

        public async Task<IEnumerable<BasicSurveyDetailDto>> GetSurveyDetailByDistrict(string district)
        {
            try
            {
                var result = await _repository.BasicSurveyDetailRepository.FindByCondition(x => x.District == district);
                var resultData = _mapper.Map<IEnumerable<BasicSurveyDetailDto>>(result);

                return resultData;
            }
            catch(Exception ex)
            {
                throw;
            }
                
        }

        public async Task<IEnumerable<BasicSurveyDetailDto>> GetSurveyDetailByTaluk(string taluk)
        {
            try
            {
                var result = await _repository.BasicSurveyDetailRepository.FindByCondition(x => x.Taluk == taluk);
                var resultData = _mapper.Map<IEnumerable<BasicSurveyDetailDto>>(result);
                return resultData;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<IEnumerable<BasicSurveyDetailDto>> GetSurveyDetailByHobli(string hobli)
        {
            try
            {
                var result = await _repository.BasicSurveyDetailRepository.FindByCondition(x => x.Hobli == hobli);
                var resultData = _mapper.Map<IEnumerable<BasicSurveyDetailDto>>(result);
                return resultData;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BasicSurveyDetailDto>> GetSurveyDetailByVillage(string village)
        {
            try
            {
                var result = await _repository.BasicSurveyDetailRepository.FindByCondition(x => x.VillageOrWard == village);
                var resultData = _mapper.Map<IEnumerable<BasicSurveyDetailDto>>(result);
                return resultData;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BasicSurveyDetail>> GetAllBasicSurveyDetailList()
        {
            var users = await _repository.UserRepository.FindAll();
            var survey = await _repository.BasicSurveyDetailRepository.FindAll();

            var resultData = _mapper.Map<IEnumerable<BasicSurveyDetail>>(survey);
            var res = resultData.OrderByDescending(T => T.SurveyId);
            return res;
        }

        
    }
}
