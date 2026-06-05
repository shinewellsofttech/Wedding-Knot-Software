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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/OldImages")]
    [ApiController]
    public class ctlOldImages : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly string sTableName = "OldImages";
        private readonly string sListFor = "Country";
        private readonly string sAddEdit_ProcedureName = "AddEdit_OldImages";
        private readonly string sImageFolder = "ItemImages";
        private readonly string sImageFolder_Thumbnail = "ItemImages/Thumbnail";
        private readonly mCommon mModel = new mCommon();
        public ctlOldImages(svcCommon svc, IWebHostEnvironment environment)
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
        public async Task<IActionResult> Add(string UserId, string UserToken, [FromForm] mOldImages dataReceived)
        {
            return await Add_Edit(UserId, UserToken, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, [FromForm] mOldImages dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, Id: Id, dataReceived: dataReceived);
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, decimal Id, mOldImages dataReceived)
        {
            try
            {
                string imageFolder = Path.Combine(
                _environment.ContentRootPath,
                sImageFolder);

                string thumbnailFolder = Path.Combine(
                    _environment.ContentRootPath,
                    sImageFolder_Thumbnail);

                Directory.CreateDirectory(thumbnailFolder);

                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

                var files = Directory.GetFiles(imageFolder)
                    .Where(f => imageExtensions.Contains(
                        Path.GetExtension(f).ToLower()))
                    .ToList();

                foreach (var file in files)
                {
                    string thumbnailName =
                        Path.GetFileNameWithoutExtension(file) + ".webp";

                    string thumbnailPath =
                        Path.Combine(thumbnailFolder, thumbnailName);

                    // Skip if thumbnail already exists
                    if (System.IO.File.Exists(thumbnailPath))
                        continue;

                    try
                    {
                        using var image = await Image.LoadAsync(file);

                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(50, 50),
                            Mode = ResizeMode.Max
                        }));

                        await image.SaveAsync(
                            thumbnailPath,
                            new WebpEncoder
                            {
                                Quality = 50
                            });
                    }
                    catch
                    {
                        // log error if needed
                    }
                }

                return NotFound(new Response { Success = false, Status = StatusCodes.Status200OK, Message = "Missing Images Uploaded" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
