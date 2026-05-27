using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Sahakaar_API.Authentication;
using Sahakaar_API.Models;
using Sahakaar_API.Models.Masters;
using Sahakaar_API.Services;
using Microsoft.AspNetCore.Http;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Data;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/PurchaseEntry")]
    [ApiController]
    public class ctlPurchaseEntry : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly string sTableName = "PurchaseEntryH";
        private readonly string sListFor = "PurchaseEntryData";
        private readonly string sAddEdit_ProcedureName = "AddEdit_PurchaseEntry";
        private readonly mCommon mModel = new mCommon();
        public ctlPurchaseEntry(svcCommon svc, IWebHostEnvironment environment)
        {
            this._svc = svc;
            this._environment = environment;
            //
            this._svc.sTableName = sTableName;
            this._svc.sAddEdit_ProcedureName = sAddEdit_ProcedureName;
        }
        // POST: api/add
        [HttpPost]
        [Route("{UserId}/{UserToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(string UserId, string UserToken, [FromForm] mPurchaseEntry dataReceived)
        {
            return await Add_Edit(UserId, UserToken, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, [FromForm] mPurchaseEntry dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, Id: Id, dataReceived: dataReceived);
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, decimal Id, mPurchaseEntry dataReceived)
        {
            try
            {
                /** Argument List **/
                var dbPara = new DynamicParameters();
                if (Id > 0)
                {
                    dbPara.Add("Id", Id);
                }
                //Information
                dbPara.Add("EntryDate", dataReceived.EntryDate, DbType.Date);
                dbPara.Add("EntryNo", dataReceived.EntryNo, DbType.String);
                dbPara.Add("F_LedgerMaster", dataReceived.F_LedgerMaster, DbType.Decimal);
                dbPara.Add("Remarks", dataReceived.Remarks, DbType.String);
                dbPara.Add("TotalCGST", dataReceived.TotalCGST, DbType.Decimal);
                dbPara.Add("TotalSGST", dataReceived.TotalSGST, DbType.Decimal);
                dbPara.Add("TotalIGST", dataReceived.TotalIGST, DbType.Decimal);
                dbPara.Add("TotalTax", dataReceived.TotalTax, DbType.Decimal);
                dbPara.Add("UserId", dataReceived.UserId, DbType.Decimal);
                dbPara.Add("F_CompanyMaster", dataReceived.F_CompanyMaster, DbType.Decimal);
                dbPara.Add("JsonData", dataReceived.JsonData, DbType.String);

                /****/
                var data = mModel;
                var response = await _svc.Insert_Update(dbPara: dbPara);
                if (response > 0)
                {
                    data.Id = response;
                    //data.Name = dataReceived.Name;
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Record " + (Id == 0 ? "added." : "updated"), Data = new { data } });
                }
                else if (response == -1)
                {
                    data.Id = response;
                   // data.Name = dataReceived.Name;
                    return NotFound(new Response { Success = false, Status = StatusCodes.Status208AlreadyReported, Message = "Data already exists.", Data = new { data } });
                }

                return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
