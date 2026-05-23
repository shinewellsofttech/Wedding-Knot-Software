using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Sahakaar_API.Models.Masters
{
    [Table("Master", Schema = "dbo")]
    public class mCommon
    {
        [Key]
        public decimal Id
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Name is required.")]
        public string Name
        {
            get;
            set;
        }
    }
    public class mImageData
    {
        public string ImageFileName
        {
            get;
            set;
        }
        //[Required(ErrorMessage = "Image File is required.")]
        public IFormFile ImageFile
        {
            get;
            set;
        }
    }
}
