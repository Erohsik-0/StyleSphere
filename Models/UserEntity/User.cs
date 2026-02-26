using Microsoft.AspNetCore.Identity;

namespace StyleSphere.Models.UserEntity
{
    public class User : IdentityUser
    {
        public string? fullname { get; set; }
    }
}
