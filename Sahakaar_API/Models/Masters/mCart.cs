using System.ComponentModel.DataAnnotations;

namespace Sahakaar_API.Models.Masters
{
    public class mCartAddUpdate
    {
        public decimal Id { get; set; } = 0;

        [Required(ErrorMessage = "ItemDesignId is required.")]
        public decimal F_ItemDesignMaster { get; set; }

        [Required(ErrorMessage = "Qty is required.")]
        public decimal Qty { get; set; }
    }
}
