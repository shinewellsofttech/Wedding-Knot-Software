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
    [Route("api/V1/Masters")]
    [ApiController]
    public class ctlMaster : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly string sListFor = "";
        private readonly mCommon mModel = new mCommon();
        public ctlMaster(svcCommon svc, IWebHostEnvironment environment)
        {
            this._svc = svc;
            this._environment = environment;
        }
        // GET: api/sessions
        //[HttpGet]
        //[Route("GetAll/{UserToken}/{ListFor}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetAll(string UserToken,string ListFor,decimal Id=0,string IdFieldName="Id")
        //{
        //    try
        //    {
        //        var DataList = await _svc.ListAll(ListFor,Id,IdFieldName);
        //        if (DataList != null)
        //        {
        //            return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Found", Data = new { DataList } });
        //        }
        //        return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
        //    }
        //}
        // GET api/session/5
        [HttpGet]
        [Route("{UserId}/{UserToken}/{ListFor}/{IdFieldName}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string UserId, string UserToken, string ListFor, string IdFieldName = "Id", decimal Id = 0)
        {
            try
            {
                var DataList = await _svc.ListAll(ListFor, Id, IdFieldName);
                if (DataList != null)
                {
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Found", Data = new { DataList } });
                }
                return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }



        [HttpGet]
        [Route("{UserId}/{UserToken}/{Search}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchAcNo(string UserId, string UserToken, string ListFor)
        {
            try
            {
                var DataList = await _svc.Search(ListFor);
                if (DataList != null)
                {
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Found", Data = new { DataList } });
                }
                return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }


        [HttpGet]
        [Route("{UserId}/{UserToken}/{Search}/1/Mem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchMember(string UserId, string UserToken, string ListFor)
        {
            try
            {
                var DataList = await _svc.SearchMem(ListFor);
                if (DataList != null)
                {
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Found", Data = new { DataList } });
                }
                return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }




        [HttpGet]
        [Route("{UserId}/{UserToken}/{Search}/1/2/Ag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchAgent(string UserId, string UserToken, string ListFor)
        {
            try
            {
                var DataList = await _svc.SearchAg(ListFor);
                if (DataList != null)
                {
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Found", Data = new { DataList } });
                }
                return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }





        [HttpGet]
        [Route("{UserId}/{UserToken}/{Search}/1/Mem/Balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBalance(string UserId, string UserToken, string ListFor, decimal F_AccountType)
        {
            try
            {
                var DataList = await _svc.AcBalance(ListFor, F_AccountType);
                if (DataList != null)
                {
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Found", Data = new { DataList } });
                }
                return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }







        [HttpGet]
        [Route("{UserId}/{UserToken}/{GetLedger}/1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LedgerDetails(string UserId, string UserToken, string ListFor)
        {
            try
            {
                var DataList = await _svc.GetLedgers(ListFor);
                if (DataList != null)
                {
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Found", Data = new { DataList } });
                }
                return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }




        // POST: api/add
        [HttpPost]
        [Route("{UserId}/{UserToken}/{TableName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(string UserId, string UserToken, string TableName, [FromBody] mMaster dataReceived)
        {
            return await Add_Edit(UserId, UserToken, TableName, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{TableName}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, string TableName, [FromBody] mMaster dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, TableName, Id: Id, dataReceived: dataReceived);
        }
        // DELETE: api/delete/5
        [HttpDelete]
        [Route("{UserId}/{UserToken}/{TableName}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string UserId, string UserToken, string TableName, decimal Id)
        {
            try
            {
                if (TableName == "VerifyByCollege")
                {
                    var response1 = await _svc.Verify(Id, TableName);
                    if (response1 > 0)
                    {
                        //data.Id = response;
                        return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Record verified.", Data = new { response1 } });
                    }

                    return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
                }
                else
                {
                    var resp = await _svc.SearchReferenceBeforeDelete(TableName, Id);
                    if (resp == 1)
                    {
                        //var data = mModel;
                        var response = await _svc.Delete(Id, TableName);
                        if (response > 0)
                        {
                            //data.Id = response;
                            return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Record deleted.", Data = new { response } });
                        }

                        return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
                    }
                    else if (resp == 2)
                    {
                        return Conflict(new Response { Success = false, Status = StatusCodes.Status408RequestTimeout, Message = "Deleting not allowed!!!" });
                    }
                    else
                    {
                        return Conflict(new Response { Success = false, Status = StatusCodes.Status409Conflict, Message = "Reference exists !!!" });
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, string sTableName, decimal Id, mMaster dataReceived)
        {
            try
            {
                /** Argument List **/
                var dbPara = new DynamicParameters();
                if (Id > 0)
                {
                    dbPara.Add("Id", Id);
                }
                dbPara.Add("TableName", sTableName, DbType.String);
                dbPara.Add("Name", dataReceived.Name, DbType.String);
                /****/
                var data = mModel;
                var response = await _svc.Insert_Update(dbPara: dbPara);
                if (response > 0)
                {
                    data.Id = response;
                    data.Name = dataReceived.Name;
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Record " + (Id == 0 ? "added." : "updated"), Data = new { data } });
                }
                else if (response == -1)
                {
                    data.Id = response;
                    data.Name = dataReceived.Name;
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
