using Microsoft.AspNetCore.Mvc;
using Sahakaar_API.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/Webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly svcCommon _svc;

        public WebhookController(svcCommon svc)
        {
            _svc = svc;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);

                var body = await reader.ReadToEndAsync();

                System.IO.File.AppendAllText(
                    @"D:\WebhookLog.txt",
                    body + System.Environment.NewLine +
                    "------------------------------------" +
                    System.Environment.NewLine);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}