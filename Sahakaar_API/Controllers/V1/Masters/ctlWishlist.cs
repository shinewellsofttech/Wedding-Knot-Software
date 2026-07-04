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
    [Route("api/V1/Wishlist")]
    [ApiController]
    public class ctlWishlist : ControllerBase
    {
        private readonly svcCommon _svc;

        public ctlWishlist(svcCommon svc)
        {
            this._svc = svc;
        }

        // GET: api/V1/Wishlist/{UserId}/{UserToken}
        [HttpGet]
        [Route("{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWishlist(string UserId, string UserToken)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "GetWishlist");
                return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Wishlist retrieved successfully", Data = new { list = response } });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        // POST: api/V1/Wishlist/{UserId}/{UserToken}
        [HttpPost]
        [Route("{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddToWishlist(string UserId, string UserToken, [FromForm] mWishlistAdd dataReceived)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);
                dbPara.Add("F_ItemDesignMaster", dataReceived.F_ItemDesignMaster, DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "AddEdit_Wishlist");
                if (response != null && response.Count > 0)
                {
                    dynamic res = response[0];
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = res.Message, Data = new { WishlistId = res.WishlistId } });
                }

                return Ok(new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to update wishlist" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        // DELETE: api/V1/Wishlist/{UserId}/{UserToken}/{Id}
        // If Id = 0, can optionally clear or use F_ItemDesignMaster query param to delete by design ID
        [HttpDelete]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteWishlistItem(string UserId, string UserToken, decimal Id, [FromQuery] decimal designId = 0)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("Id", Id, DbType.Decimal);
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);
                dbPara.Add("F_ItemDesignMaster", designId, DbType.Decimal);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "DeleteWishlistItem");
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

                return Ok(new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to delete wishlist item" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
