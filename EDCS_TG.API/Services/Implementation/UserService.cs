using AutoMapper;
using EDCS_TG.API.Data;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.Dto;
using EDCS_TG.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EDCS_TG.API.Services.Implementation
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private readonly KarmaniDbContext _karmaniDbContext;

        public UserService(IUnitOfWork repository, IMapper mapper,KarmaniDbContext karmaniDbContext)
        {
            _repository = repository;
            _mapper = mapper;
            _karmaniDbContext = karmaniDbContext;

        }


         public async Task<IEnumerable<UserCreateDto>> GetAllUsersList()
        {
            try
            {
                var userResult = (from user in _karmaniDbContext.Users
                                  join userRole in _karmaniDbContext.UserRoles on user.Id equals userRole.UserId
                                  join role in _karmaniDbContext.Roles on userRole.RoleId equals role.Id
                                  where role.Name == "Surveyor" orderby user.SurveyorId
                                  select new User { UserName = user.UserName,
                                      NormalizedUserName = user.NormalizedUserName,OTP = user.OTP,OTPValidity = user.OTPValidity,
                                  Sid = user.Sid,SurveyorId = user.SurveyorId,Status = user.Status,
                                  PhoneNumber = user.PhoneNumber,FirstName = user.FirstName,LastName = user.LastName,Age = user.Age,
                                  Id = user.Id,DOB = user.DOB,District = user.District,Taluk = user.Taluk,
                                  Hobli = user.Hobli,District_Code = user.District_Code,Taluk_Code = user.Taluk_Code,Hobli_Code = user.Hobli_Code,
                                  TwoFactorEnabled = user.TwoFactorEnabled,LockoutEnabled = user.LockoutEnabled,EmailConfirmed = user.EmailConfirmed,AccessFailedCount = user.AccessFailedCount}).AsEnumerable();

                //var result = await _repository.UserRepository.FindAll();

                //var result = _mapper.Map<List<User>>(userResult);

                var resultDto = _mapper.Map<List<UserCreateDto>>(userResult);
                return resultDto;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<User> getUserById(Guid id)
        {
            var result = await _repository.UserRepository.FindById(id);
            return result;
        }

        public async Task<IEnumerable<UserCreateDto>> getUserdetailsList()
        {
            var result = await _repository.UserRepository.FindAll();
            var resultData = _mapper.Map<IEnumerable<UserCreateDto>>(result);

            return resultData;
        }

        public async Task<UserCreateDto> UpdateUserData(UserCreateDto user)
        {
            var userEntity = _mapper.Map<User>(user);
            var resultEntity = await _repository.UserRepository.FindById(t => t.Id == user.Id);
            resultEntity.Status = user.Status;
            resultEntity.DOB = user.DOB;
            resultEntity.UserName = user.FirstName + " " + user.LastName;
            resultEntity.Age = user.Age;
            resultEntity.FirstName = user.FirstName;
            resultEntity.LastName = user.LastName;
            resultEntity.PhoneNumber = user.PhoneNumber;
            resultEntity.District = user.District;
            resultEntity.District = user.District;
            resultEntity.Taluk = user.Taluk;
            resultEntity.Hobli = user.Hobli;
            resultEntity.District_Code = user.District_Code;
            resultEntity.Taluk_Code = user.Taluk_Code;
            resultEntity.Hobli_Code = user.Hobli_Code;

            _karmaniDbContext.User.Update(resultEntity);
            _karmaniDbContext.SaveChanges();
            //var result = await _repository.UserRepository.Update(resultEntity);
            //var resultDto = _mapper.Map<UserCreateDto>(result);
            return user;
        }

        public async Task<UserCreateDto> CreateUser(UserCreateDto user)
        {
            //var userEntity = _mapper.Map<User>(user);
            User userEntity = new User();
            userEntity.Id = System.Guid.NewGuid();
            userEntity.Status = user.Status;
            userEntity.DOB = user.DOB;
            userEntity.UserName = user.FirstName + " " + user.LastName; 
            userEntity.Age = user.Age;
            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
            userEntity.PhoneNumber = user.PhoneNumber;
            userEntity.District = user.District;
            userEntity.Taluk = user.Taluk;
            userEntity.Hobli = user.Hobli;
            userEntity.District_Code = user.District_Code;
            userEntity.Taluk_Code = user.Taluk_Code;
            userEntity.Hobli_Code = user.Hobli_Code;

            var result = await _repository.UserRepository.CreateNewUser(userEntity);
            var resultDto = _mapper.Map<UserCreateDto>(result);

            return resultDto;
        }
    }
}
