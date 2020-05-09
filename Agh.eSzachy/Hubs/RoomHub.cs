using System.Linq;
using System.Threading.Tasks;
using Agh.eSzachy.Models;
using Agh.eSzachy.Services;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;

namespace Agh.eSzachy.Hubs
{
    [Authorize]
    public class RoomHub : AuthorizedHub<IRoomClient>
    {
        public IRoomService RoomService { get; }

        public RoomHub(IRoomService roomService)
        {
            this.RoomService = roomService;
        }

        public async Task Create(string roomName)
        {
            var room = this.RoomService.Create(this.User, new Room { Name = roomName });
            await room.MapAsync(async x =>
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, x.Name);
                await this.Refresh();
                return x;
            });
        }

        public async Task Join(string roomName)
        {
            var room = this.RoomService.Join(this.User, roomName);
            await room.MapAsync(async x =>
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, x.Name);
                await this.Clients.Group(x.Name).RefreshSingle(x);
                return x;
            });
        }

        public async Task Leave(string roomName)
        {
            var room = this.RoomService.Left(this.User, roomName);
            await room.MapAsync(async x =>
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, x.Name);
                return x;
            });
        }

        public async Task<string[]> GetAllRooms()
        {
            return this.RoomService.GetAllRoomNames().Match(x => x, x => throw x);
        }

        public async Task Send(string roomId, string message)
        {
            var m = this.RoomService.SendMessage(new Room { Id = roomId }, this.User, message);
            await m.MapAsync(async x =>
            {
                await this.Refresh();
                return x;
            });
        }

        private async Task Refresh()
        {
            var status = await this.RoomService.Status(this.User);
            await status.MapAsync(async x =>
            {
                await this.Clients.All.Refresh(x);
                return Unit.Default;
            });
        }

        public async override Task OnConnectedAsync()
        {
            var id = this.Context.UserIdentifier ?? "Anonymous";
            var client = new Client(id);
            var status = await this.RoomService.Status(client);
            await status.MapAsync(async x =>
            {
                await Task.WhenAll(x.Select((r) =>
                {
                    return this.Groups.AddToGroupAsync(this.Context.ConnectionId, r.Name);
                }));
                await this.Clients.Caller.Refresh(x);
                return Unit.Default;
            });
        }
    }
}