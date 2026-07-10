using System;
using System.Threading.Tasks;
using Sahakaar_API.Models;
using Sahakaar_API.Models.Masters;
using Sahakaar_API.Services;
using Microsoft.AspNetCore.Http;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/EccomRegister")]
    [ApiController]
    public class ctlEccomRegister : ControllerBase
    {
        private readonly svcCommon _svc;
        
        public ctlEccomRegister(svcCommon svc)
        {
            this._svc = svc;
        }

        // POST: api/V1/EccomRegister  — Register a new ecommerce user
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromForm] mEccomRegister dataReceived)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("Name", dataReceived.Name, DbType.String);
                dbPara.Add("Username", dataReceived.UserName, DbType.String);
                dbPara.Add("Password", dataReceived.Password, DbType.String);
                dbPara.Add("ContactEmail", dataReceived.ContactEmail, DbType.String);
                dbPara.Add("ContactMobile", dataReceived.ContactMobile, DbType.String);
                dbPara.Add("Address", dataReceived.Address, DbType.String);
                dbPara.Add("F_CityMaster", dataReceived.F_CityMaster, DbType.Decimal);
                dbPara.Add("F_StateMaster", dataReceived.F_StateMaster, DbType.Decimal);
                dbPara.Add("PinCode", dataReceived.PinCode, DbType.String);
                dbPara.Add("F_CompanyMaster", dataReceived.F_CompanyMaster, DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "Eccom_Register");
                if (response != null && response.Count > 0)
                {
                    dynamic res = response[0];
                    if (res.Success == 1)
                    {
                        return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = res.Message, Data = new { NewId = res.NewId } });
                    }
                    else
                    {
                        return Ok(new Response { Success = false, Status = StatusCodes.Status200OK, Message = res.Message });
                    }
                }
                
                return Ok(new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to register" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        // GET: api/V1/EccomRegister/{Id}  — Get ecommerce user profile by UserId
        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEccomUser(decimal Id)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("Id", Id, DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "GetUserDetail");
                if (response != null && response.Count > 0)
                {
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "User retrieved successfully", Data = new { user = response[0] } });
                }

                return Ok(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}

