using Agh.eSzachy.Models;
using Microsoft.AspNetCore.SignalR;

namespace Agh.eSzachy.Hubs
{
    public class AuthorizedHub<T> : Hub<T>
        where T : class
    {
        protected Client User => new Client(this.Context.UserIdentifier);
    }
}