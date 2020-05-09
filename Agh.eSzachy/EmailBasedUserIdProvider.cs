using Microsoft.AspNetCore.Identity;
using Agh.eSzachy.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;

namespace Agh.eSzachy
{
    public class EmailBasedUserIdProvider : IUserIdProvider
    {
        public EmailBasedUserIdProvider(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public virtual string GetUserId(HubConnectionContext connection)
        {
            using (var scope = this.ServiceProvider.CreateScope())
            {
                var userStore = scope.ServiceProvider.GetService<IUserStore<ApplicationUser>>();
                var id = connection.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var email = connection.User?.Identity.Name;
                var user = id != null ? userStore.FindByIdAsync(id, CancellationToken.None) : userStore.FindByNameAsync(email, CancellationToken.None);
                user.ConfigureAwait(false);
                var r = user.Result.Id;
                return r;
            }
        }
    }
}
