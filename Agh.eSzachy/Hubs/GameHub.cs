using System.Linq;
using System.Threading.Tasks;
using Agh.eSzachy.Services;
using LanguageExt;
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
            var client = new Models.Client(this.Context.UserIdentifier);
            var room = await this.RoomService.Status(client);
            var targetRoom = room.Map(x => x.First(el => el.Name == roomName));
            var ready = await targetRoom.MapAsync(async x =>
            {
                return Unit.Default;
            });
        }

        public async Task Subscribe(string roomName)
        {
            var client = new Models.Client(this.Context.UserIdentifier);
            var room = await this.RoomService.Status(client);
            var targetRoom = room.Map(x => x.First(el => el.Name == roomName));
            var ready = await targetRoom.MapAsync(async x =>
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomName).ConfigureAwait(false);
                await this.Refresh(x);
                return Unit.Default;
            });
        }

        public ChessBoard Map(ChessBoardModel model)
        {
            return new ChessBoard
            {
                Pawns = model.Board.Select(x => new Pawn
                {
                    Col = x.Key.Column,
                    Row = x.Key.Row,
                    IsPlayerOne = x.Value.player == Models.Chess.Player.One,
                    Type = x.Value.GetType().Name
                }).ToArray()
            };
        }

        public ChessBoardHistory MapHistory(ChessBoardModel model)
        {
            return new ChessBoardHistory
            {
                
            };
        }


        private async Task Refresh(Models.Room room)
        {
            var currentGame = await this.GameService.Current(room);
            await this.Clients.Group(room.Name).Refresh(room.Name, Map(currentGame));
        }

        public async Task Ready(string roomName)
        {
            var client = new Models.Client(this.Context.UserIdentifier);
            var room = await this.RoomService.Status(client);
            var targetRoom = room.Map(x => x.First(el => el.Name == roomName));
            var ready = await targetRoom.MapAsync(async x =>
            {
                await this.GameService.Ready(client, x);
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomName).ConfigureAwait(false);
                await this.Refresh(x);
                return Unit.Default;
            });
        }

        public async Task<ChessBoardHistory[]> HistoricalFor(string roomName)
        {
            var client = new Models.Client(this.Context.UserIdentifier);
            var room = await this.RoomService.Status(client);
            var targetRoom = room.Map(x => x.First(el => el.Name == roomName));
            var ready = await targetRoom.MapAsync(async x =>
            {
                return await this.GameService.All(x);
            });
            var result = ready.Map(x => x.Select(MapHistory).ToArray()).Match(x => x, e => throw e);
            return result;
        }

        public async Task Surrender(string roomName)
        {

        }
    }
}