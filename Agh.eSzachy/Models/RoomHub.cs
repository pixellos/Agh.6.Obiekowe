using System;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
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

        Client User => new Client(this.Context.UserIdentifier);

        public async Task Create(string roomName)
        {
            var room = this.RoomService.Create(this.User, new Room { Name = roomName });
            await room.MapAsync(async x =>
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, x.Name);
                await this.Clients.Group(x.Name).RefreshSingle(x);
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

        public Task Send(string roomId, string message)
        {
            var m = this.RoomService.SendMessage(new Room { Id = roomId }, this.User, message);
            m.Map(x =>
            {
                this.Clients.Groups(new[] { roomId }).RefreshSingle(x);
                return x;
            });

            return Task.CompletedTask;
        }

        public async override Task OnConnectedAsync()
        {
            var id = this.Context.UserIdentifier ?? "Anonymous";
            var client = new Client(id);
            await this.RoomService.Status(client).MapAsync(async x =>
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