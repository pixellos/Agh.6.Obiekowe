using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace HelloSignalR
{
    public class RoomHub : Hub<IRoomClient>
    {
        public async Task<bool> JoinRoom(string userId, string roomId)
        {
            Task.Run(() =>
            {
                Clients.All.RoomStatus($"{userId} joined", MessageType.Joined);
            }).ConfigureAwait(false);
            return true;
        }

        async Task<Room[]> GetRooms(string userId)
        {
            return new[]
            {
                new Room
                {
                    Id = "SomeId",
                    Created = DateTime.Now,
                    Name = "Mocked"
                }
            };
        }

    }
}