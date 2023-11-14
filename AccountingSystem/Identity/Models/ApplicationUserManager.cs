using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly BocwAccountingIdentityDbContext _context;
        public ApplicationUserManager(BocwAccountingIdentityDbContext context,
                                        IUserStore<ApplicationUser> store,
                                        IOptions<IdentityOptions> optionsAccessor,
                                        IPasswordHasher<ApplicationUser> passwordHasher,
                                        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
                                        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
                                        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
                                        IServiceProvider services,
                                        ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _context = context;
        }

        public Task<ApplicationUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            var user = _context.Users.SingleOrDefault(x => x.PhoneNumber == phoneNumber); //&& x.PhoneNumberConfirmed);
            return Task.FromResult(user);
        }
    }
}
