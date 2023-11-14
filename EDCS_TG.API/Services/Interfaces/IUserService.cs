using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserCreateDto>> GetAllUsersList();

        Task<IEnumerable<UserCreateDto>> getUserdetailsList();

        Task<User> getUserById(Guid id);

        Task<UserCreateDto> UpdateUserData(UserCreateDto user);

        Task<UserCreateDto> CreateUser(UserCreateDto user);


    }
}
