using Microsoft.AspNetCore.Identity;

namespace StyleSphere.Models.User
{
    public class User : IdentityUser
    {
        public string? fullname { get; set; }
    }
}
