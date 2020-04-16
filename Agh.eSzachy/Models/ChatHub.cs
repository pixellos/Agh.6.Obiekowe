using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Agh
{
    [Authorize]
    public class RoomHub : Hub<IRoomClient>
    {
        public IRoomService RoomService { get; }

        public RoomHub(IRoomService roomService)
        {
            this.RoomService = roomService;
        }

        Client User => new Client(Context.UserIdentifier);

        public Task Join(string roomId)
        {
            return Task.CompletedTask;
        }

        public Task Leave(string roomId)
        {
            return Task.CompletedTask;
        }

        public Task Send(string roomId, string message)
        {
            var m = RoomService.SendMessage(new Room { Id = roomId }, this.User, message);
            m.Map(x =>
            {
                // Update all interested clients
                return x;
            });

            return Task.CompletedTask;
        }

        public async override Task OnConnectedAsync()
        {
            var id = Context.UserIdentifier ?? "Anonymous";
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