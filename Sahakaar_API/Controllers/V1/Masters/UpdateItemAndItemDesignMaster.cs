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
using System.Drawing;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/UpdateItemAndItemDesignMaster")]
    [ApiController]
    public class UpdateItemAndItemDesignMaster : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly string sTableName = "ItemMaster";
        private readonly string sListFor = "Country";
        private readonly string sAddEdit_ProcedureName = "UpdateItemAndItemDesignMaster";
        private readonly string sImageFolder = "ItemImages";
        private readonly mCommon mModel = new mCommon();
        public UpdateItemAndItemDesignMaster(svcCommon svc, IWebHostEnvironment environment)
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
        public async Task<IActionResult> Add(string UserId, string UserToken, [FromForm] mUpdateItemAndItemDesignMaster dataReceived)
        {
            return await Add_Edit(UserId, UserToken, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, [FromForm] mUpdateItemAndItemDesignMaster dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, Id: Id, dataReceived: dataReceived);
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, decimal Id, mUpdateItemAndItemDesignMaster dataReceived)
        {
            try
            {
                /** Argument List **/
                var dbPara = new DynamicParameters();
                if (Id > 0)
                {
                    dbPara.Add("Id", Id);
                }
              
                dbPara.Add("Id", dataReceived.Id, DbType.Decimal);
                dbPara.Add("UserId", dataReceived.UserId, DbType.Decimal);
                dbPara.Add("TableName", dataReceived.TableName, DbType.String);
                dbPara.Add("FieldName", dataReceived.FieldName, DbType.String);
                dbPara.Add("FieldValue", dataReceived.FieldValue, DbType.String);

                var ItemDesignId = !string.IsNullOrEmpty(dataReceived.Id) ? Convert.ToDecimal(dataReceived.Id) : 0;

                if (dataReceived.TableName == "ItemDesignMaster" && (dataReceived.FieldName == "DesignPhoto" || dataReceived.FieldName == "DesignPhoto2" || dataReceived.FieldName == "DesignPhoto3" || dataReceived.FieldName == "DesignPhoto4" || dataReceived.FieldName == "DesignPhoto5"))
                {
                    if (dataReceived.DesignPhoto != null && dataReceived.DesignPhoto.Length > 0)
                    {
                        await SaveFile(dataReceived.DesignPhoto, "ItemPhoto1_", ItemDesignId, "DesignPhoto", "ItemDesignMaster");
                    }
                    if (dataReceived.DesignPhoto2 != null && dataReceived.DesignPhoto2.Length > 0)
                    {
                        await SaveFile(dataReceived.DesignPhoto2, "ItemPhoto2_", ItemDesignId, "DesignPhoto2", "ItemDesignMaster");
                    }
                    if (dataReceived.DesignPhoto3 != null && dataReceived.DesignPhoto3.Length > 0)
                    {
                        await SaveFile(dataReceived.DesignPhoto3, "ItemPhoto3_", ItemDesignId, "DesignPhoto3", "ItemDesignMaster");
                    }
                    if (dataReceived.DesignPhoto4 != null && dataReceived.DesignPhoto4.Length > 0)
                    {
                        await SaveFile(dataReceived.DesignPhoto4, "ItemPhoto4_", ItemDesignId, "DesignPhoto4", "ItemDesignMaster");
                    }
                    if (dataReceived.DesignPhoto5 != null && dataReceived.DesignPhoto5.Length > 0)
                    {
                        await SaveFile(dataReceived.DesignPhoto5, "ItemPhoto5_", ItemDesignId, "DesignPhoto5", "ItemDesignMaster");
                    }
                }

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
                        //using (var image = Image.FromStream(file.OpenReadStream()))
                        //{
                        //    Size imgSize = new Size(300, 300);
                        //    var imageResized = Lib.clsFunctions.ResizeImage(image, imgSize);
                        //    imageResized.Save(filePath);
                        //}
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
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
