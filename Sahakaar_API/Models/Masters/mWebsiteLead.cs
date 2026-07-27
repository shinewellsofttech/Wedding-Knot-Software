using System;
using System.ComponentModel.DataAnnotations;

namespace Sahakaar_API.Models.Masters
{
    public class mWebsiteLead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Mobile number is required.")]
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class mBulkDeleteLead
    {
        [Required(ErrorMessage = "Ids parameter is required.")]
        public string Ids { get; set; }
    }
}
