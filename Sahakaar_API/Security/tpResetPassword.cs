using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sahakaar_API.Security
{
    public class tpResetPassword<TUser>
        : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public tpResetPassword(IDataProtectionProvider dataProtectionProvider,
                    IOptions<tpResetPasswordOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        { }
    }
}
