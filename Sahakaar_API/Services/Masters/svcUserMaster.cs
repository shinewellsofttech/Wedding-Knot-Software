using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Sahakaar_API.Interfaces;
using Sahakaar_API.Interfaces.Masters;
using Sahakaar_API.Models;
using Sahakaar_API.Models.Masters;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Sahakaar_API.Services.Masters
{
    public class svcUserMaster : IUserMaster
    {
        private readonly string sTableName = "UserMaster";
        private readonly string sGetList_ProcedureName = "GetList_UserMaster";
        private readonly string sDelete_ProcedureName = "Delete_DataById";
        private readonly string sGetCount_ProcedureName = "GetCount";
        private readonly string sAddEdit_ProcedureName = "AddEdit_UserMaster"; 
        private readonly string sForgotPassword_ProcedureName = "ForgotPassword";
        private readonly string sVerifyOTP_ProcedureName = "VerifyOTP";
        private readonly string sUpdateUserImageData_ProcedureName = "Update_UserMaster_ImageData";
        private readonly string sUpdateUserAddress_ProcedureName = "Update_UserMaster_Address";
        private readonly string sUpdateUserProfile_ProcedureName = "Update_UserMaster_Profile";
        private readonly string sUpdateUserProfileSettings_ProcedureName = "Update_UserMaster_ProfileSettings";
        //
        private readonly IDapperManager _dapperManager;
        public svcUserMaster(IDapperManager dapperManager)
        {
            this._dapperManager = dapperManager;
        }
        public Task<mUserMaster> GetById(string UserId,string UserToken)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Id", UserId,DbType.String);
            var itemById = Task.FromResult(_dapperManager.Get<mUserMaster>(sGetList_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return itemById;
        }
        public Task<mOTP> ForgotPassword(string UserEmail, IConfiguration _configuration)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Type", "1", DbType.String);
            dbPara.Add("UserEmail", UserEmail, DbType.String);
            var itemById = Task.FromResult(_dapperManager.Get<mOTP>(sForgotPassword_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            if (Convert.ToInt32(itemById.Result.OTP)>1)
            { 
                //Send Mail
                MimeMessage message = new MimeMessage();
                MailboxAddress from = new MailboxAddress(_configuration["SMTP:From"], _configuration["SMTP:Username"]);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("User", UserEmail);
                message.To.Add(to);
                message.Subject = "Forgot Password - OTP";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<p>Hi <b>" + itemById.Result.FullName + "</b>,</p>" + "<p>Your forgot password otp is <b>" + itemById.Result.OTP + "</b>.</p></br>Thanks,<br/>Admin<br/>Company Name";
                message.Body = bodyBuilder.ToMessageBody();

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect(_configuration["SMTP:Host"], Convert.ToInt32(_configuration["SMTP:Port"]), false);
                    smtpClient.Authenticate(_configuration["SMTP:Username"], _configuration["SMTP:Password"]);
                    smtpClient.Send(message);
                    smtpClient.Disconnect(true);
                    smtpClient.Dispose();
                }
            }
            return itemById;
        }
        public Task<int> VerifyOTP(int Type,string UserEmail, string OTP)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Type", Type, DbType.Int32);
            dbPara.Add("UserEmail", UserEmail, DbType.String);
            dbPara.Add("OTP", OTP, DbType.String);
            var itemById = Task.FromResult(_dapperManager.Get<int>(sVerifyOTP_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return itemById;
        }

        public Task<string> Update_UserProfileSettings(string? UserId, string? UserToken, mUserMaster_ProfileSettings item)
        {
            var dbPara = new DynamicParameters();
            if (UserId != "")
                dbPara.Add("Id", UserId);
            dbPara.Add("FullName", item.FullName, DbType.String);
            dbPara.Add("Miles", item.Miles, DbType.String);
            var rId = Task.FromResult(_dapperManager.Insert_Update<string>(sUpdateUserProfileSettings_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rId;
        }
        public Task<string> Update_UserProfile(string? UserId, string? UserToken, mUserMaster_Profile item)
        {
            var dbPara = new DynamicParameters();
            if (UserId != "")
                dbPara.Add("Id", UserId);
            dbPara.Add("FirstName", item.FirstName, DbType.String);
            dbPara.Add("LastName", item.LastName, DbType.String);
            dbPara.Add("Address", item.Address, DbType.String);
            dbPara.Add("DOB", item.DOB, DbType.String);
            var rId = Task.FromResult(_dapperManager.Insert_Update<string>(sUpdateUserProfile_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rId;
        }
        public Task<string> Update_UserImageData(string? UserId, string? UserToken, mUserMaster_ImageData item)
        {
            var dbPara = new DynamicParameters();
            if (UserId != "")
                dbPara.Add("Id", UserId);
            dbPara.Add("ImageURL", item.ImageData, DbType.String);
            var rId = Task.FromResult(_dapperManager.Insert_Update<string>(sUpdateUserImageData_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rId;
        }
        public Task<string> Update_UserAddress(string? UserId, string? UserToken, mUserMaster_Address item)
        {
            var dbPara = new DynamicParameters();
            if (UserId != "")
                dbPara.Add("Id", UserId);
            dbPara.Add("Address", item.Address, DbType.String);
            dbPara.Add("Country", item.Country, DbType.String);
            dbPara.Add("Lat", item.Lat, DbType.String);
            dbPara.Add("Long", item.Long, DbType.String);
            var rId = Task.FromResult(_dapperManager.Insert_Update<string>(sUpdateUserAddress_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rId;
        }
    }
}
