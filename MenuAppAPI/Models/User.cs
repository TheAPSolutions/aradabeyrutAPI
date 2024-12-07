using Microsoft.AspNetCore.Identity;

namespace AradaAPI.Models
{
    public class User : IdentityUser
    {
        public int? BranchId { get; set; }
    }
}
