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
    [Route("api/V1/GetItemDetailByReference")]
    [ApiController]
    public class cltGetItemDetailByReference : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly string sTableName = "ItemDesignMaster";
        private readonly string sListFor = "ItemDesignMaster";
        private readonly string sAddEdit_ProcedureName = "GetItemDetailByReference";
        private readonly mCommon mModel = new mCommon();
        public cltGetItemDetailByReference(svcCommon svc, IWebHostEnvironment environment)
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
        public async Task<IActionResult> Add(string UserId, string UserToken, [FromForm] mGetItemDetailByReference dataReceived)
        {
            return await Add_Edit(UserId, UserToken, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, [FromForm] mGetItemDetailByReference dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, Id: Id, dataReceived: dataReceived);
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, decimal Id, mGetItemDetailByReference dataReceived)
        {
            try
            {
                /** Argument List **/
                var dbPara = new DynamicParameters();
                if (Id > 0)
                {
                    dbPara.Add("Id", Id);
                }
                dbPara.Add("F_CategoryMaster", dataReceived.F_CategoryMaster, DbType.Decimal);                
                dbPara.Add("F_GSTGroupMaster", dataReceived.F_GSTGroupMaster, DbType.Decimal);

                /****/
                var data = mModel;
                var response = await _svc.Login(dbPara: dbPara);
                if (response != null)
                {
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Found", Data = new { response } });
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
