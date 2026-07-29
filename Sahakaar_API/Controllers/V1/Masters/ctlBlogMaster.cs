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
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/BlogMaster")]
    [ApiController]
    public class ctlBlogMaster : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly string sTableName = "BlogMaster";
        private readonly string sListFor = "BlogMaster";
        private readonly string sAddEdit_ProcedureName = "AddEdit_BlogMaster";
        private readonly string sImageFolder = "Blogs";
        private readonly string sImageFolder_Thumbnail = "Blogs/Thumbnail";
        private readonly mCommon mModel = new mCommon();
        public ctlBlogMaster(svcCommon svc, IWebHostEnvironment environment)
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
        public async Task<IActionResult> Add(string UserId, string UserToken, [FromForm] mBlogMaster dataReceived)
        {
            return await Add_Edit(UserId, UserToken, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, [FromForm] mBlogMaster dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, Id: Id, dataReceived: dataReceived);
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, decimal Id, mBlogMaster dataReceived)
        {
            try
            {
                /** Argument List **/
                var dbPara = new DynamicParameters();
                if (Id > 0)
                {
                    dbPara.Add("Id", Id);
                }

                dbPara.Add("BlogTitle", dataReceived.BlogTitle, DbType.String);
                dbPara.Add("Slug", dataReceived.Slug, DbType.String);
                dbPara.Add("Author", dataReceived.Author, DbType.String);
                dbPara.Add("ReadTime", dataReceived.ReadTime, DbType.String);
                dbPara.Add("PublishDate", dataReceived.PublishDate, DbType.DateTime);
              //  dbPara.Add("PrimaryImage", dataReceived.PrimaryImage, DbType.String);
              //  dbPara.Add("SecondaryImage", dataReceived.SecondaryImage, DbType.String);
                dbPara.Add("Tags", dataReceived.Tags, DbType.String);
                dbPara.Add("ShortSummary", dataReceived.ShortSummary, DbType.String);
                dbPara.Add("FullContent", dataReceived.FullContent, DbType.String);
                dbPara.Add("UserId", dataReceived.UserId, DbType.Decimal);

                /****/
                var data = mModel;
                var response = await _svc.Insert_Update(dbPara: dbPara);
                if (response > 0)
                {
                    data.Id = response;

                    if (dataReceived.PrimaryImage != null && dataReceived.PrimaryImage.Length > 0)
                    {
                        await SaveFile(dataReceived.PrimaryImage, "PrimaryImage_", data.Id, "PrimaryImage", "BlogMaster");
                    }
                    if (dataReceived.SecondaryImage != null && dataReceived.SecondaryImage.Length > 0)
                    {
                        await SaveFile(dataReceived.SecondaryImage, "SecondaryImage_", data.Id, "SecondaryImage", "BlogMaster");
                    }

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
        private async Task SaveFile(IFormFile file, string prefix, decimal id, string fieldName, string TableName)
        {
            if (file != null && file.Length > 0)
            {
                // GET OLD FILE NAME
                string oldFileName = await _svc.Get_ImageName(TableName, id, fieldName);

                // DELETE OLD FILE
                if (!string.IsNullOrEmpty(oldFileName))
                {
                    string oldPath =
                        Path.Combine(
                            _environment.ContentRootPath,
                            sImageFolder,
                            oldFileName
                        );

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    // Thumbnail Image (.webp)
                    string thumbnailFileName =
                        Path.GetFileNameWithoutExtension(oldFileName) + ".webp";

                    string thumbnailPath = Path.Combine(
                        _environment.ContentRootPath,
                        sImageFolder_Thumbnail,
                        thumbnailFileName
                    );

                    if (System.IO.File.Exists(thumbnailPath))
                    {
                        System.IO.File.Delete(thumbnailPath);
                    }
                }

                //rcp
                FileInfo fi = new FileInfo(file.FileName);
                string extension = fi.Extension.ToLower();
                string fileName = $"{prefix}{id}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{extension}";
                string filePath = Path.Combine(_environment.ContentRootPath, sImageFolder, fileName);

                // Ensure directory exists
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, sImageFolder));

                try
                {
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        // Create folders if not exist
                        Directory.CreateDirectory(
                            Path.Combine(_environment.ContentRootPath, sImageFolder));

                        Directory.CreateDirectory(
                            Path.Combine(_environment.ContentRootPath, sImageFolder_Thumbnail));

                        // Save Original Image
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Save Thumbnail WebP Image
                        string thumbnailFileName =
                            Path.GetFileNameWithoutExtension(fileName) + ".webp";

                        string thumbnailPath = Path.Combine(
                            _environment.ContentRootPath,
                            sImageFolder_Thumbnail,
                            thumbnailFileName);

                        using (var imageStream = file.OpenReadStream())
                        using (var image = await Image.LoadAsync(imageStream))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(50, 50),
                                Mode = ResizeMode.Max // Keep aspect ratio
                            }));

                            await image.SaveAsync(
                                thumbnailPath,
                                new WebpEncoder
                                {
                                    Quality = 50
                                });
                        }
                    }
                    else if (extension == ".pdf" || extension == ".doc" || extension == ".docx")
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Unsupported file type.");
                    }

                    // Updating the database
                    var imageUrl = await _svc.Update_ImageDataNew(TableName, id, fileName, fieldName);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        // Handle successful update if needed
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed
                    throw new InvalidOperationException("Error saving file", ex);
                }
            }
        }
    }
}
