using System.ComponentModel.DataAnnotations;

namespace Sahakaar_API.Models.Masters
{
    public class mUserAddressAddUpdate
    {
        public decimal Id { get; set; } = 0;

        [Required(ErrorMessage = "AddressType is required.")]
        public string AddressType { get; set; } = string.Empty;

        [Required(ErrorMessage = "FullName is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "MobileNo is required.")]
        public string MobileNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "AddressLine1 is required.")]
        public string AddressLine1 { get; set; } = string.Empty;

        public string AddressLine2 { get; set; } = string.Empty;

        [Required(ErrorMessage = "CityId is required.")]
        public decimal F_CityMaster { get; set; }

        [Required(ErrorMessage = "StateId is required.")]
        public decimal F_StateMaster { get; set; }

        [Required(ErrorMessage = "PinCode is required.")]
        public string PinCode { get; set; } = string.Empty;

        public bool IsDefault { get; set; } = false;
    }
}
