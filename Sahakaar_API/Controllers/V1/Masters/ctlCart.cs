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
    [Route("api/V1/Cart")]
    [ApiController]
    public class ctlCart : ControllerBase
    {
        private readonly svcCommon _svc;

        public ctlCart(svcCommon svc)
        {
            this._svc = svc;
        }

        // GET: api/V1/Cart/{UserId}/{UserToken}
        [HttpGet]
        [Route("{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCart(string UserId, string UserToken)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "GetCartList");
                return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Cart retrieved successfully", Data = new { list = response } });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        // POST: api/V1/Cart/{UserId}/{UserToken}
        [HttpPost]
        [Route("{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUpdateCart(string UserId, string UserToken, [FromForm] mCartAddUpdate dataReceived)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("Id", dataReceived.Id, DbType.Decimal);
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);
                dbPara.Add("F_ItemDesignMaster", dataReceived.F_ItemDesignMaster, DbType.Decimal);
                dbPara.Add("Qty", dataReceived.Qty, DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "AddEdit_Cart");
                if (response != null && response.Count > 0)
                {
                    dynamic res = response[0];
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Cart updated successfully", Data = new { CartId = res.CartId } });
                }

                return Ok(new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to update cart" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        // DELETE: api/V1/Cart/{UserId}/{UserToken}/{Id}
        // If Id = 0, clear entire cart
        [HttpDelete]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCartItem(string UserId, string UserToken, decimal Id)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("Id", Id, DbType.Decimal);
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "DeleteCartItem");
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

                return Ok(new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to delete cart item" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
