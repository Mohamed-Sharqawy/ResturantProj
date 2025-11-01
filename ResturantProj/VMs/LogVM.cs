using System.ComponentModel.DataAnnotations;

namespace ResturantProj.VMs
{
    public class LogVM
    {
        [Required(ErrorMessage ="Email or username is required")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
