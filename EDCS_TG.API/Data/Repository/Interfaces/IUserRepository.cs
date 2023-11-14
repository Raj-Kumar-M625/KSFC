using EDCS_TG.API.Data.Models;

namespace EDCS_TG.API.Data.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByPhoneAsync(string? phoneNumber);
        Task<User> CreateNewUser(User user);


    }
}
