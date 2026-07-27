using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.AspNetCore.SignalR;
using Sahakaar_API.Hubs;
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
        private readonly IHubContext<OrderHub> _hubContext;

        public ctlEccomOrder(svcCommon svc, IHubContext<OrderHub> hubContext)
        {
            this._svc = svc;
            this._hubContext = hubContext;
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
                        try
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveNewOrder", new
                            {
                                salesEntryId = res.SalesEntryId,
                                message = res.Message
                            });
                        }
                        catch
                        {
                            // Ignore SignalR broadcast error so order placement still succeeds
                        }

                        return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = res.Message, Data = new { SalesEntryId = res.SalesEntryId } });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = res.Message });
                    }
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to place order" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }


        // POST: api/V1/EccomOrder/AdminGetOrders/{UserId}/{UserToken}
        [HttpPost]
        [Route("AdminGetOrders/{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdminGetOrdersPost(string UserId, string UserToken, [FromForm] mAdminGetOrdersRequest dataReceived)
        {
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);
                
                decimal targetUserId = dataReceived?.UserId ?? 0;
                dbPara.Add("TargetUserId", targetUserId, DbType.Decimal);
                
                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "Admin_GetEccomOrders");
                if (response != null)
                {
                    var ordersList = new List<dynamic>();
                    foreach (var item in response)
                    {
                        var row = item as IDictionary<string, object>;
                        if (row != null && row.ContainsKey("ItemsJson") && row["ItemsJson"] != null)
                        {
                            try
                            {
                                row["Items"] = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(row["ItemsJson"].ToString());
                                row.Remove("ItemsJson");
                            }
                            catch 
                            {
                                row["Items"] = new List<Dictionary<string, object>>();
                            }
                        }
                        else if (row != null)
                        {
                            row["Items"] = new List<Dictionary<string, object>>();
                        }
                        ordersList.Add(item);
                    }
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Orders retrieved successfully", Data = new { Orders = ordersList } });
                }

                return Ok(new Response { Success = false, Status = StatusCodes.Status200OK, Message = "No orders found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        // POST: api/V1/EccomOrder/UpdateStatus/{UserId}/{UserToken}
        [HttpPost]
        [Route("UpdateStatus/{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStatus(string UserId, string UserToken, [FromForm] mUpdateOrderStatus dataReceived)
        {
            try
            {
                decimal statusId = dataReceived.F_StatusMaster;
                if (statusId == 0 && !string.IsNullOrEmpty(dataReceived.Status))
                {
                    if (dataReceived.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                        statusId = 2;
                    else if (dataReceived.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                        statusId = 3;
                    else if (dataReceived.Status.Equals("Packed", StringComparison.OrdinalIgnoreCase))
                        statusId = 4;
                    else if (dataReceived.Status.Equals("Shipped", StringComparison.OrdinalIgnoreCase))
                        statusId = 5;
                    else if (dataReceived.Status.Equals("Out for Delivery", StringComparison.OrdinalIgnoreCase))
                        statusId = 6;
                    else if (dataReceived.Status.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
                        statusId = 7;
                    else
                        statusId = 1; // Pending
                }

                var dbPara = new DynamicParameters();
                dbPara.Add("F_UserMaster", Convert.ToDecimal(UserId), DbType.Decimal);
                dbPara.Add("OrderId", dataReceived.OrderId, DbType.Decimal);
                dbPara.Add("F_StatusMaster", statusId, DbType.Decimal);
                dbPara.Add("Remarks", dataReceived.Remarks, DbType.String);

                var response = await _svc.Login(dbPara: dbPara, sAddEdit_Procedure: "Admin_UpdateOrderStatus");
                if (response != null && response.Count > 0)
                {
                    dynamic res = response[0];
                    if (res.Success == 1)
                    {
                        return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = res.Message });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = res.Message });
                    }
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = "Failed to update order status" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
