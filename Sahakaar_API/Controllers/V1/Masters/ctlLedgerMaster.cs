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
    [Route("api/V1/LedgerMaster")]
    [ApiController]
    public class ctlLedgerMaster : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly string sTableName = "AdvisorMaster";
        private readonly string sListFor = "Country";
        private readonly string sAddEdit_ProcedureName = "AddEdit_LedgerMaster";
        private readonly mCommon mModel = new mCommon();
        public ctlLedgerMaster(svcCommon svc, IWebHostEnvironment environment)
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
        public async Task<IActionResult> Add(string UserId, string UserToken, [FromForm] mLedgerMaster dataReceived)
        {
            return await Add_Edit(UserId, UserToken, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, [FromForm] mLedgerMaster dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, Id: Id, dataReceived: dataReceived);
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, decimal Id, mLedgerMaster dataReceived)
        {
            try
            {
                /** Argument List **/
                var dbPara = new DynamicParameters();
                if (Id > 0)
                {
                    dbPara.Add("Id", Id);
                }
                dbPara.Add("Name", dataReceived.Name, DbType.String);
                dbPara.Add("Alias", dataReceived.Alias, DbType.String);
                dbPara.Add("F_LedgerGroupMaster", dataReceived.F_LedgerGroupMaster, DbType.Decimal);
                dbPara.Add("Address", dataReceived.Address, DbType.String);
                dbPara.Add("Address1", dataReceived.Address1, DbType.String);
                dbPara.Add("F_CityMaster", dataReceived.F_CityMaster, DbType.Decimal);
                dbPara.Add("F_StateMaster", dataReceived.F_StateMaster, DbType.Decimal);
                dbPara.Add("F_CountryMaster", dataReceived.F_CountryMaster, DbType.Decimal);
                dbPara.Add("PinCode", dataReceived.PinCode, DbType.String);
                dbPara.Add("PhoneNo", dataReceived.PhoneNo, DbType.String);
                dbPara.Add("MobileNo", dataReceived.MobileNo, DbType.String);
                dbPara.Add("Email", dataReceived.Email, DbType.String);
                dbPara.Add("GSTIN", dataReceived.GSTIN, DbType.String);
                dbPara.Add("PANNo", dataReceived.PANNo, DbType.String);
                dbPara.Add("F_GSTGroupMaster", dataReceived.F_GSTGroupMaster, DbType.Decimal);
                dbPara.Add("F_GSTType", dataReceived.F_GSTType, DbType.String);
                dbPara.Add("F_TaxPayerType", dataReceived.F_TaxPayerType, DbType.String);
                dbPara.Add("CreditDays", dataReceived.CreditDays, DbType.Decimal);
                dbPara.Add("CreditLimit", dataReceived.CreditLimit, DbType.Decimal);
                dbPara.Add("Rate", dataReceived.Rate, DbType.Decimal);
                dbPara.Add("F_Type", dataReceived.F_Type, DbType.String);
                dbPara.Add("F_CalculationType", dataReceived.F_CalculationType, DbType.String);
                dbPara.Add("F_AddLess", dataReceived.F_AddLess, DbType.String);
                dbPara.Add("YesNoActs", dataReceived.YesNoActs, DbType.Boolean);
                dbPara.Add("F_LedgerMasterSales", dataReceived.F_LedgerMasterSales, DbType.Decimal);
                dbPara.Add("F_LedgerMasterPurchase", dataReceived.F_LedgerMasterPurchase, DbType.Decimal);
                dbPara.Add("F_YearScheme", dataReceived.F_YearScheme, DbType.String);
                dbPara.Add("F_IntCalcMethod", dataReceived.F_IntCalcMethod, DbType.String);
                dbPara.Add("BankName", dataReceived.BankName, DbType.String);
                dbPara.Add("BankAccountNo", dataReceived.BankAccountNo, DbType.String);
                dbPara.Add("BankIFSCCode", dataReceived.BankIFSCCode, DbType.String);
                dbPara.Add("ISDalal", dataReceived.ISDalal, DbType.Boolean);
                dbPara.Add("F_LedgerMasterDalal", dataReceived.F_LedgerMasterDalal, DbType.Decimal);
                dbPara.Add("IsTransport", dataReceived.IsTransport, DbType.Boolean);
                dbPara.Add("F_TCSonSales", dataReceived.F_TCSonSales, DbType.Decimal);
                dbPara.Add("UserId", dataReceived.UserId, DbType.Decimal);
                dbPara.Add("F_CompanyMaster", dataReceived.F_CompanyMaster, DbType.Decimal);

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
