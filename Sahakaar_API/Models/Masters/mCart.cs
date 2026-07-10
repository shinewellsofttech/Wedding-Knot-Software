using System.ComponentModel.DataAnnotations;

namespace Sahakaar_API.Models.Masters
{
    public class mCartAddUpdate
    {
        public decimal Id { get; set; } = 0;

        private decimal _itemDesignId = 0;

        [Required(ErrorMessage = "ItemDesignId is required.")]
        public decimal F_ItemDesignMaster
        {
            get => _itemDesignId;
            set => _itemDesignId = value;
        }

        public decimal ItemDesignId
        {
            get => _itemDesignId;
            set => _itemDesignId = value;
        }

        [Required(ErrorMessage = "Qty is required.")]
        public decimal Qty { get; set; }
    }
}
