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
    [Route("api/V1/EccomOrder")]
    [ApiController]
    public class ctlEccomOrder : ControllerBase
    {
        private readonly svcCommon _svc;

        public ctlEccomOrder(svcCommon svc)
        {
            this._svc = svc;
        }

        // POST: api/V1/EccomOrder/{UserId}/{UserToken}
        [HttpPost]
        [Route("{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder(string UserId, string UserToken, [FromForm] mEccomOrder dataReceived)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);
                dbPara.Add("Remarks", dataReceived.Remarks, DbType.String);
                dbPara.Add("DispatchedThrough", dataReceived.DispatchedThrough, DbType.String);
                dbPara.Add("DispatchDocNo", dataReceived.DispatchDocNo, DbType.String);
                dbPara.Add("OtherChargesJson", dataReceived.OtherChargesJson, DbType.String);
                dbPara.Add("F_CompanyMaster", dataReceived.F_CompanyMaster, DbType.Decimal);
                dbPara.Add("F_ShippingAddressId", dataReceived.F_ShippingAddressId, DbType.Decimal);
                dbPara.Add("F_BillingAddressId", dataReceived.F_BillingAddressId, DbType.Decimal);
                dbPara.Add("ItemsJson", dataReceived.ItemsJson, DbType.String);
 
                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "CreateEccomOrder");
                if (response != null && response.Count > 0)
                {
                    dynamic res = response[0];
                    if (res.Success == 1)
                    {
                        return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = res.Message, Data = new { SalesEntryId = res.SalesEntryId } });
                    }
                    else
                    {
                        return Ok(new Response { Success = false, Status = StatusCodes.Status200OK, Message = res.Message });
                    }
                }

                return Ok(new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to place order" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
