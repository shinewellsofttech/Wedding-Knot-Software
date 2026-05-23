using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sahakaar_API.Models;
using Sahakaar_API.Models.Masters;
using Microsoft.Extensions.Configuration;

namespace Sahakaar_API.Interfaces.Masters
{
    public interface IUserMaster
    {
        Task<mOTP> ForgotPassword(string UserEmail, IConfiguration _configuration);
        Task<int> VerifyOTP(int Type, string UserEmail, string OTP);
        Task<string> Update_UserProfile(string? id, string? UserToken, mUserMaster_Profile item);
        Task<string> Update_UserProfileSettings(string? id, string? UserToken, mUserMaster_ProfileSettings item);
        Task<string> Update_UserImageData(string? id, string? UserToken, mUserMaster_ImageData item);
        Task<string> Update_UserAddress(string? id, string? UserToken, mUserMaster_Address item);
    }
}
