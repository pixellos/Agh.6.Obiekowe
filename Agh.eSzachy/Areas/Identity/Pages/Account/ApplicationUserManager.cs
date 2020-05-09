using Agh.eSzachy.Data;
using Agh.eSzachy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Agh.eSzachy.Areas.Identity.Pages.Account
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }


        public override Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken = default) => Task.FromResult(user.Email.ToLower());
        public override Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken = default)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }
    }
}
