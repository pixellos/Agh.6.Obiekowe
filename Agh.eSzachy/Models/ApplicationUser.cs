using Microsoft.AspNetCore.Identity;

namespace Agh.eSzachy.Models
{
    public class ApplicationUser : IdentityUser
    {
        public override string NormalizedUserName { get => base.Email; set => base.Email = value; }
    }
}
