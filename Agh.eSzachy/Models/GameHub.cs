using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Agh
{
    [Authorize]
    public class GameHub : Hub<IGameClient>
    {
        public GameHub(IGameService gameService, IRoomService roomService)
        {
            this.GameService = gameService;
            this.RoomService = roomService;
        }

        public IGameService GameService { get; }
        public IRoomService RoomService { get; }

        public async Task Move(string roomName, string from, string to)
        {
        }

        public async Task Ready(string roomName)
        {

        }

        public async Task Surrender(string roomName)
        {

        }
    }
}