using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Agh
{
    [Authorize]
    public class GameHub : Hub<IGameClient>
    {

    }
}