using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sahakaar_API.Interfaces;
using Sahakaar_API.Models;
using Sahakaar_API.Models.Masters;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/WebsiteLeads")]
    [ApiController]
    public class ctlWebsiteLeads : ControllerBase
    {
        private readonly IDapperManager _dapperManager;

        public ctlWebsiteLeads(IDapperManager dapperManager)
        {
            this._dapperManager = dapperManager;
        }

        /// <summary>
        /// Public endpoint to save lead info from website popup modal.
        /// Checks for duplicate mobile numbers before saving.
        /// </summary>
        [HttpPost]
        [Route("SaveLead")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveLead([FromForm] mWebsiteLead dataReceived)
        {
            try
            {
                if (dataReceived == null || string.IsNullOrWhiteSpace(dataReceived.MobileNo))
                {
                    return BadRequest(new Response
                    {
                        Success = false,
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Mobile number is required."
                    });
                }

                var dbPara = new DynamicParameters();
                dbPara.Add("Name", dataReceived.Name ?? "", DbType.String);
                dbPara.Add("MobileNo", dataReceived.MobileNo.Trim(), DbType.String);
                dbPara.Add("Email", dataReceived.Email ?? "", DbType.String);

                var resultList = await Task.FromResult(_dapperManager.GetAll<dynamic>("Save_WebsiteLead", dbPara, CommandType.StoredProcedure));
                var firstResult = resultList?.FirstOrDefault();

                if (firstResult != null)
                {
                    IDictionary<string, object> row = (IDictionary<string, object>)firstResult;
                    long resultCode = Convert.ToInt64(row["ResultCode"]);
                    string message = Convert.ToString(row["Message"]);

                    if (resultCode > 0)
                    {
                        return Ok(new Response
                        {
                            Success = true,
                            Status = StatusCodes.Status200OK,
                            Message = message ?? "Lead saved successfully.",
                            Data = new { Id = resultCode }
                        });
                    }
                    else if (resultCode == -1)
                    {
                        return Ok(new Response
                        {
                            Success = false,
                            Status = StatusCodes.Status208AlreadyReported,
                            Message = message ?? "Mobile number already registered.",
                            Data = null
                        });
                    }
                    else
                    {
                        return BadRequest(new Response
                        {
                            Success = false,
                            Status = StatusCodes.Status400BadRequest,
                            Message = message ?? "Failed to save lead.",
                            Data = null
                        });
                    }
                }

                return BadRequest(new Response
                {
                    Success = false,
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Unable to process request."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Success = false,
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Admin endpoint to fetch all active website leads for software dashboard.
        /// </summary>
        [HttpGet]
        [Route("GetLeads/{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLeads(string UserId, string UserToken)
        {
            try
            {
                var dataList = await Task.FromResult(_dapperManager.GetAll<object>("GetList_WebsiteLeads", null, CommandType.StoredProcedure));
                return Ok(new Response
                {
                    Success = true,
                    Status = StatusCodes.Status200OK,
                    Message = "Found",
                    Data = new { DataList = dataList ?? new List<object>() }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Success = false,
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Admin endpoint to delete multiple leads using comma-separated IDs.
        /// </summary>
        [HttpPost]
        [Route("DeleteLeads/{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLeads(string UserId, string UserToken, [FromForm] string Ids)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Ids))
                {
                    return BadRequest(new Response
                    {
                        Success = false,
                        Status = StatusCodes.Status400BadRequest,
                        Message = "No IDs provided for deletion."
                    });
                }

                var dbPara = new DynamicParameters();
                dbPara.Add("Ids", Ids.Trim(), DbType.String);

                var resultList = await Task.FromResult(_dapperManager.GetAll<dynamic>("Delete_WebsiteLeads_Bulk", dbPara, CommandType.StoredProcedure));
                var firstResult = resultList?.FirstOrDefault();

                int rowsAffected = 0;
                string msg = "Deleted successfully.";
                if (firstResult != null)
                {
                    IDictionary<string, object> row = (IDictionary<string, object>)firstResult;
                    rowsAffected = Convert.ToInt32(row["RowsAffected"]);
                    msg = Convert.ToString(row["Message"]);
                }

                return Ok(new Response
                {
                    Success = true,
                    Status = StatusCodes.Status200OK,
                    Message = $"{rowsAffected} lead(s) deleted successfully.",
                    Data = new { RowsAffected = rowsAffected }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Success = false,
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                });
            }
        }
    }
}
