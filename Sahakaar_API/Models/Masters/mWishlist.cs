using System.ComponentModel.DataAnnotations;

namespace Sahakaar_API.Models.Masters
{
    public class mWishlistAdd
    {
        [Required(ErrorMessage = "ItemDesignId is required.")]
        public decimal F_ItemDesignMaster { get; set; }
    }
}
