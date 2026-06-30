using System.ComponentModel.DataAnnotations;

namespace Sahakaar_API.Models.Masters
{
    public class mEccomRegister
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public string ContactEmail { get; set; }
        
        public string ContactMobile { get; set; }
    }

    public class mEccomLogin
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
