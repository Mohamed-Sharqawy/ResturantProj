using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ResturantProj.Models
{
    public class CustomUser : IdentityUser
    { 
        public string? Address { get; set; }
    }
}
