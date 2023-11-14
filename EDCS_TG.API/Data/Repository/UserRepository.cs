using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace EDCS_TG.API.Data.Repository
{
    public class UserRepository: Repository<User>, IUserRepository
    {
        private KarmaniDbContext _karmaniDbContext;
        public UserRepository(KarmaniDbContext karmaniDbContext)
          : base(karmaniDbContext)

        {
            _karmaniDbContext = karmaniDbContext;
        }

      
        public async Task<User?> FindByPhoneAsync(string? phoneNumber)
        {
            try
            {
                var userByPhoneNumber = await FindByCondition(u => u.PhoneNumber == phoneNumber);
                var user = userByPhoneNumber.FirstOrDefault();

                return user;
            }
            catch( Exception ex)
            {
                throw;
            }
        }

        public  async  Task<User> CreateNewUser(User user)
        {
            var userRole = new UserRole();
            userRole.UserId = user.Id;
            userRole.RoleId = new Guid("7AE3440A-5B41-452F-B86C-812AC4533053");
            _karmaniDbContext.UserRoles.Add(userRole);
            var result = _karmaniDbContext.User.Add(user);
            _karmaniDbContext.SaveChanges();
            return user;
        }



    }
}
