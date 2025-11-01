using System.ComponentModel.DataAnnotations;

namespace ResturantProj.VMs
{
    public class CrtUserVM
    {
        [Required]
       public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
       public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
       public string? Address  { get; set; }
    }
}
