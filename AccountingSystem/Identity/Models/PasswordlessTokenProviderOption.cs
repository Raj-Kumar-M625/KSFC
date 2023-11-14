using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class PasswordlessTokenProviderOption : DataProtectionTokenProviderOptions
    {
        public PasswordlessTokenProviderOption()
        {
            Name = "PasswordlessTokenProvider";
            TokenLifespan = TimeSpan.FromMinutes(60);
        }
    }
}
