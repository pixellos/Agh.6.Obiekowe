using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agh.eSzachy.Models
{
    public class ApplicationUser : IdentityUser
    {
        public override string NormalizedUserName { get => base.Email; set => base.Email = value; }
    }
}
