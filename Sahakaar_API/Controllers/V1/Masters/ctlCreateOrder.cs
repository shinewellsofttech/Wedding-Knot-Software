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
using Razorpay.Api;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/CreateOrder")]
    [ApiController]
    public class ctlCreateOrder : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly string sTableName = "CreateOrderH";
        private readonly string sListFor = "CreateOrderData";
        private readonly string sAddEdit_ProcedureName = "Add_PaymentTransaction";
        private readonly mCommon mModel = new mCommon();
        public ctlCreateOrder(svcCommon svc, IWebHostEnvironment environment, IConfiguration configuration)
        {
            this._svc = svc;
            this._environment = environment;
            this._configuration = configuration;
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
        public async Task<IActionResult> Add(string UserId, string UserToken, [FromForm] mCreateOrder dataReceived)
        {
            return await Add_Edit(UserId, UserToken, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, [FromForm] mCreateOrder dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, Id: Id, dataReceived: dataReceived);
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, decimal Id, mCreateOrder dataReceived)
        {
            try
            {
                RazorpayClient client =
                    new RazorpayClient( _configuration["Razorpay:Key"],   _configuration["Razorpay:Secret"]);

                Dictionary<string, object> options =  new Dictionary<string, object>();

                options.Add("amount", Convert.ToInt32(dataReceived.Amount));
                options.Add("currency", "INR");
                options.Add("receipt", dataReceived.F_SalesEntryH.ToString());

                Order order =  client.Order.Create(options);

                // Save Order In Database

                var dbPara = new DynamicParameters();
                dbPara.Add("F_SalesEntryH", dataReceived.F_SalesEntryH, DbType.Decimal);
                dbPara.Add("RazorpayOrderId", order["id"].ToString(), DbType.String);
                dbPara.Add("Amount", dataReceived.Amount, DbType.Decimal);

                var data = mModel;
                var response = await _svc.Login(dbPara: dbPara);


                if (response != null)
                {
                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Order " + (Id == 0 ? "added." : "updated"), Data = new { response } });
                }
                return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
      
    }
}
