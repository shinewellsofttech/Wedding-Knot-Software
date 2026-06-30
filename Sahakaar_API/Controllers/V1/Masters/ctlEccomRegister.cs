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
    }
}
