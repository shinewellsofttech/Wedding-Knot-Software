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
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using QRCoder;
using System.IO;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/Payment")]
    [ApiController]
    public class ctlPayment : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly string sTableName = "PaymentTransaction";
        private readonly string sListFor = "PaymentTransaction";
        private readonly string sAddEdit_ProcedureName = "Add_PaymentTransaction";
        private readonly mCommon mModel = new mCommon();
        public ctlPayment(svcCommon svc, IWebHostEnvironment environment, IConfiguration configuration)
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
        public async Task<IActionResult> Add(string UserId, string UserToken, [FromForm] mPayment dataReceived)
        {
            return await Add_Edit(UserId, UserToken, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, [FromForm] mPayment dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, Id: Id, dataReceived: dataReceived);
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, decimal Id, mPayment dataReceived)
        {
            try
            {
                var payload = new
                {
                    amount = Convert.ToInt32(dataReceived.Amount * 100),
                    currency = "INR",
                    accept_partial = false,
                    description = "Sales Invoice Payment",
                    reference_id = $"{dataReceived.F_SalesEntryH}_{DateTime.Now.Ticks}"
                };

                var json = JsonConvert.SerializeObject(payload);

                var client = new HttpClient();

                var authToken =
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            $"{_configuration["Razorpay:Key"]}:{_configuration["Razorpay:Secret"]}"));

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic",
                        authToken);

                var response =
                    await client.PostAsync(
                        "https://api.razorpay.com/v1/payment_links",
                        new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json"));

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(result);
                }

                dynamic paymentLink = JsonConvert.DeserializeObject(result);
                string paymentLinkId = paymentLink.id.ToString();
                string paymentUrl = paymentLink.short_url;

                //Generate QRCode
                QRCodeGenerator qrGenerator = new QRCodeGenerator();

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(paymentUrl, QRCodeGenerator.ECCLevel.Q);

                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

                byte[] qrCodeBytes = qrCode.GetGraphic(20);

                string qrBase64 = Convert.ToBase64String(qrCodeBytes);


                var dbPara = new DynamicParameters();
                dbPara.Add("F_SalesEntryH", dataReceived.F_SalesEntryH, DbType.Decimal);
                dbPara.Add("PaymentLinkId", paymentLinkId.ToString(), DbType.String);
                dbPara.Add("PaymentUrl", paymentUrl.ToString(), DbType.String);
                dbPara.Add("Amount", dataReceived.Amount, DbType.Decimal);
                dbPara.Add("Status", "PENDING", DbType.String);

                var data = mModel;
                var response1 = await _svc.Login(dbPara: dbPara);


                if (response1 != null)
                {
                    dynamic row = response1[0];                    
                    data.QrCode = qrBase64;
                    data.Amount = row.Amount;

                    return Ok(new Response { Success = true, Status = StatusCodes.Status200OK, Message = "Order " + (Id == 0 ? "added." : "updated"), Data = new { data } });
                }

                return NotFound(new Response { Success = false, Status = StatusCodes.Status404NotFound, Message = "Not Found" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Webhook")]
        public async Task<IActionResult> Webhook()
        {
            using var reader = new StreamReader(Request.Body);

            var body = await reader.ReadToEndAsync();

            System.IO.File.AppendAllText(
                @"C:\WebhookLog.txt",
                body + Environment.NewLine +
                "----------------------" +
                Environment.NewLine);

            return Ok();
        }

    }
}
