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
using Newtonsoft.Json;

namespace Sahakaar_API.Controllers.V1.Masters
{
    [Route("api/V1/ItemMaster")]
    [ApiController]
    public class ctlItemMaster : ControllerBase
    {
        private readonly svcCommon _svc;
        private readonly IWebHostEnvironment _environment;
        private readonly string sTableName = "ItemMaster";
        private readonly string sListFor = "Country";
        private readonly string sAddEdit_ProcedureName = "AddEdit_ItemMaster";
        private readonly string sImageFolder = "ItemImages";
        private readonly mCommon mModel = new mCommon();
        public ctlItemMaster(svcCommon svc, IWebHostEnvironment environment)
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
        public async Task<IActionResult> Add(string UserId, string UserToken, [FromForm] mItemMaster dataReceived)
        {
            return await Add_Edit(UserId, UserToken, Id: 0, dataReceived: dataReceived);
        }
        // PUT: api/update/5
        [HttpPut]
        [Route("{UserId}/{UserToken}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string UserId, string UserToken, [FromForm] mItemMaster dataReceived, decimal Id)
        {
            return await Add_Edit(UserId, UserToken, Id: Id, dataReceived: dataReceived);
        }
        private async Task<IActionResult> Add_Edit(string UserId, string UserToken, decimal Id, mItemMaster dataReceived)
        {
            try
            {
                /** Argument List **/
                var dbPara = new DynamicParameters();

                if (Id > 0)
                {
                    dbPara.Add("Id", Id);
                }

                // SEND ONLY DATA, NOT FILES

                var designData = dataReceived.DesignDetails.Select(x => new
                {
                    x.SizeName,
                    x.SalePrice,
                    x.Barcode,
                    x.VideoLink,
                    x.OpeningStock
                }).ToList();

                string DesignJson =
                    JsonConvert.SerializeObject(designData);

                dbPara.Add(
                    "F_CategoryMaster",
                    dataReceived.F_CategoryMaster,
                    DbType.Decimal
                );

                dbPara.Add(
                    "ItemName",
                    dataReceived.ItemName,
                    DbType.String
                );

                dbPara.Add(
                    "HasSize",
                    dataReceived.HasSize,
                    DbType.Boolean
                );

                dbPara.Add(
                    "DesignJson",
                    DesignJson,
                    DbType.String
                );

                /**********************************/

                var data = mModel;

                var ds =
                    await _svc.Insert_Update_WithDataSet(dbPara);

                // TABLE 0 = MAIN RESPONSE
                // TABLE 1 = DESIGN IDS

                if (ds == null || ds.Tables.Count == 0)
                {
                    return NotFound(new Response
                    {
                        Success = false,
                        Status = StatusCodes.Status404NotFound,
                        Message = "No response from database"
                    });
                }

                // MAIN RESULT

                DataTable dtResult = ds.Tables[0];

                if (dtResult.Rows.Count == 0)
                {
                    return NotFound(new Response
                    {
                        Success = false,
                        Status = StatusCodes.Status404NotFound,
                        Message = "No data returned"
                    });
                }

                // RESPONSE VALUE

                decimal response =
                    Convert.ToDecimal(dtResult.Rows[0]["Id"]);

                // DUPLICATE CHECK

                if (response == -1)
                {
                    data.Id = response;

                    return NotFound(new Response
                    {
                        Success = false,
                        Status = StatusCodes.Status208AlreadyReported,
                        Message = "Data already exists.",
                        Data = new { data }
                    });
                }

                // SUCCESS

                if (response > 0)
                {
                    data.Id = response;
                    DataTable dtDesign = ds.Tables[1];

                    for (int i = 0; i < dataReceived.DesignDetails.Count; i++)
                    {
                        decimal designId = Convert.ToDecimal(dtDesign.Rows[i]["Id"]);

                        var item = dataReceived.DesignDetails[i];

                        // IMAGE 1
                        if (item?.DesignPhoto?.ImageFile != null)
                        {
                            await SaveImage(item.DesignPhoto, "ItemDesign1_", designId, "DesignPhoto");
                        }
                        // IMAGE 2
                        if (item?.DesignPhoto2?.ImageFile != null)
                        {
                            await SaveImage(item.DesignPhoto2, "ItemDesign2_", designId, "DesignPhoto2");
                        }
                        // IMAGE 3
                        if (item?.DesignPhoto3?.ImageFile != null)
                        {
                            await SaveImage(item.DesignPhoto3, "ItemDesign3_", designId, "DesignPhoto3");
                        }
                        // IMAGE 4
                        if (item?.DesignPhoto4?.ImageFile != null)
                        {
                            await SaveImage(item.DesignPhoto4, "ItemDesign4_", designId, "DesignPhoto4");
                        }
                        // IMAGE 5
                        if (item?.DesignPhoto5?.ImageFile != null)
                        {
                            await SaveImage(item.DesignPhoto5, "ItemDesign5_", designId, "DesignPhoto5");
                        }
                    }

                    return Ok(new Response
                    {
                        Success = true,
                        Status = StatusCodes.Status200OK,
                        Message =
                            "Record " +
                            (Id == 0
                                ? "added."
                                : "updated"),
                        Data = new { data }
                    });
                }

                return NotFound(new Response
                {
                    Success = false,
                    Status = StatusCodes.Status404NotFound,
                    Message = "Not Found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Success = false,
                        Status =
                            StatusCodes
                                .Status500InternalServerError,
                        Message = ex.Message
                    }
                );
            }
        }
        private async Task SaveImage(mImageData ImageDetail, string Prefix, decimal Id, string sFieldName)
        {
            /** Image Uploading **/
            if (ImageDetail.ImageFile != null)
                if (ImageDetail.ImageFile.Length > 0)
                {
                    FileInfo fi = new FileInfo(ImageDetail.ImageFile.FileName);
                    string sExtension = fi.Extension;

                    string sFileName = Prefix + Id + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + sExtension;
                    using (var image = Image.FromStream(ImageDetail.ImageFile.OpenReadStream()))
                    {
                        // resize the image and save it to the output stream                            
                        System.Drawing.Size imgSize = new System.Drawing.Size();
                        imgSize.Height = 700;
                        imgSize.Width = 700;
                        var imageResized = Lib.clsFunctions.ResizeImage(image, imgSize);
                        imageResized.Save(_environment.ContentRootPath + "/" + sImageFolder + "/" + sFileName);
                        //
                        imgSize.Height = 300;
                        imgSize.Width = 300;
                        imageResized = Lib.clsFunctions.ResizeImage(image, imgSize);
                        imageResized.Save(_environment.ContentRootPath + "/" + sImageFolder + "/Thumbnails_300/" + sFileName);
                        //
                        
                        //Updating to database
                        ImageDetail.ImageFileName = sFileName;
                        var ImageUrl = await _svc.Update_ImageData("ItemDesignMaster", Id, ImageDetail, sFieldName);

                        if (ImageUrl != "")
                        {
                            ImageDetail.ImageFileName = ImageUrl;
                        }
                    }
                }
            /****/
        }

    }
}
