using System.Threading.Tasks;
using Agh.eSzachy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Agh.eSzachy.Hubs
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
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomName).ConfigureAwait(false);
        }

        public async Task<ChessBoardHistory[]> HistoricalForPlayer(string email)
        {
            return default;
        }

        public async Task<ChessBoardHistory[]> HistoricalFor(string roomName)
        {
            return default;
        }

        public async Task Surrender(string roomName)
        {

        }
    }
}