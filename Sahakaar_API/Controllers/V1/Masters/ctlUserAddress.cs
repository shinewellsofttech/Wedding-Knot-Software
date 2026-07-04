using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Sahakaar_API.Models;
using Sahakaar_API.Models.Masters;
using Sahakaar_API.Services;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/UserAddress")]
    [ApiController]
    public class ctlUserAddress : ControllerBase
    {
        private readonly svcCommon _svc;

        public ctlUserAddress(svcCommon svc)
        {
            this._svc = svc;
        }

        // GET: api/V1/UserAddress/{UserId}/{UserToken}
        [HttpGet]
        [Route("{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAddresses(string UserId, string UserToken)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "GetUserAddressList");
                return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Addresses retrieved successfully", Data = new { list = response } });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        // POST: api/V1/UserAddress/{UserId}/{UserToken}
        [HttpPost]
        [Route("{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUpdateAddress(string UserId, string UserToken, [FromForm] mUserAddressAddUpdate dataReceived)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("Id", dataReceived.Id, DbType.Decimal);
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);
                dbPara.Add("AddressType", dataReceived.AddressType, DbType.String);
                dbPara.Add("FullName", dataReceived.FullName, DbType.String);
                dbPara.Add("MobileNo", dataReceived.MobileNo, DbType.String);
                dbPara.Add("AddressLine1", dataReceived.AddressLine1, DbType.String);
                dbPara.Add("AddressLine2", dataReceived.AddressLine2, DbType.String);
                dbPara.Add("F_CityMaster", dataReceived.F_CityMaster, DbType.Decimal);
                dbPara.Add("F_StateMaster", dataReceived.F_StateMaster, DbType.Decimal);
                dbPara.Add("PinCode", dataReceived.PinCode, DbType.String);
                dbPara.Add("IsDefault", dataReceived.IsDefault ? 1 : 0, DbType.Int32);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "AddEdit_UserAddress");
                if (response != null && response.Count > 0)
                {
                    dynamic res = response[0];
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = res.Message, Data = new { AddressId = res.AddressId } });
                }

                return Ok(new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to update address" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        // DELETE: api/V1/UserAddress/{UserId}/{UserToken}/{Id}
        [HttpDelete]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAddress(string UserId, string UserToken, decimal Id)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("Id", Id, DbType.Decimal);
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "DeleteUserAddress");
                if (response != null && response.Count > 0)
                {
                    dynamic res = response[0];
                    if (res.Success == 1)
                    {
                        return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = res.Message });
                    }
                    else
                    {
                        return Ok(new Response { Success = false, Status = StatusCodes.Status200OK, Message = res.Message });
                    }
                }

                return Ok(new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to delete address" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
