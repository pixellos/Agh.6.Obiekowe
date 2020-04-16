using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Agh
{
    public class RoomHub : Hub<IRoomClient>
    {
        public IRoomService RoomService { get; }

        public RoomHub(IRoomService roomService)
        {
            this.RoomService = roomService;
        }

        public Task Send(string message)
        {
            if (message == string.Empty)
            {
                return Clients.All.Send("hi");
            }

            return Clients.All.Send(message);
        }

        public async override Task OnConnectedAsync()
        {
            var id = Context.User.Identity.Name ?? "Anonymous";
            var client = new Client(id);
            await RoomService.Status(client).MapAsync(async x =>
            {
                await Task.WhenAll(x.Select((r) =>
                {
                    return Groups.AddToGroupAsync(Context.ConnectionId, r.Id);
                }));
                await Clients.Caller.Refresh(x);
                return Unit.Default;
            });
        }
    }
}