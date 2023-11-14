using Microsoft.AspNetCore.DataProtection;
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
    public class TokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : IdentityUser
    {
        public TokenProvider(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<PasswordlessTokenProviderOption> options, ILogger<TokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        { }
    }
}
