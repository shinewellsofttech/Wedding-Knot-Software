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
    public class tpLogin<TUser>
        : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public tpLogin(IDataProtectionProvider dataProtectionProvider,
                    IOptions<tpLoginOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        { }
    }
}
