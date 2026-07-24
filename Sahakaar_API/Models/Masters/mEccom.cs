using System.ComponentModel.DataAnnotations;

namespace Sahakaar_API.Models.Masters
{
    public class mEccomRegister
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        public string Password { get; set; } = string.Empty;

        public string ContactEmail { get; set; } = string.Empty;
        
        public string ContactMobile { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public decimal F_CityMaster { get; set; } = 0;

        public decimal F_StateMaster { get; set; } = 0;

        public string PinCode { get; set; } = string.Empty;

        public decimal F_CompanyMaster { get; set; } = 1;
    }

    public class mEccomLogin
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }

    public class mEccomOrder
    {
        public string Remarks { get; set; } = string.Empty;
        public string DispatchedThrough { get; set; } = string.Empty;
        public string DispatchDocNo { get; set; } = string.Empty;
        public string OtherChargesJson { get; set; } = string.Empty;
        public decimal F_CompanyMaster { get; set; } = 1;
        public decimal F_ShippingAddressId { get; set; } = 0;
        public decimal F_BillingAddressId { get; set; } = 0;
        public string ItemsJson { get; set; } = string.Empty;
    }

    public class mUpdateOrderStatus
    {
        public decimal OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal F_StatusMaster { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }

    public class mAdminGetOrdersRequest                                                                                                                         
    {
        public decimal UserId { get; set; } = 0;
    }
}

