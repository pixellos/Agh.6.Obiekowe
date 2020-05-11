using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agh.eSzachy.Models.Chess;
using Agh.eSzachy.Services;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Agh.eSzachy.Hubs
{
    [Authorize]
    public class GameHub : AuthorizedHub<IGameClient>
    {
        public GameHub(IGameService gameService, IRoomService roomService)
        {
            this.GameService = gameService;
            this.RoomService = roomService;
        }

        public IGameService GameService { get; }
        public IRoomService RoomService { get; }

        public async Task Move(string roomName, PawnPosition from, PawnPosition to)
        {
            var client = new Models.Client(this.Context.UserIdentifier);
            var room = await this.RoomService.Status(client);
            var targetRoom = room.Map(x => x.First(el => el.Name == roomName));
            var ready = await targetRoom.MapAsync(async x =>
            {
                await this.GameService.Move(client, x, from, to);
                await this.Refresh(x);
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

        private BoardState GetValue(GameStateModel val)
        {
            switch (val)
            {
                case GameStateModel.Waiting:
                    return BoardState.Idle;
                case GameStateModel.Finished:
                    return BoardState.PlayerOneWins;
                case GameStateModel.InPlay:
                    return BoardState.Started;
                default: return 0;
            };
        }

        public ChessBoardDto Map(ChessBoardModel model)
        {
            var pawn = MapPawns(model.Board);
            return new ChessBoardDto
            {
                State = this.GetValue(model.State),
                PlayerOne = new PlayerDto
                {
                    Id = model.PlayerOneId,
                    Name = model.PlayerOneName
                },
                PlayerTwo = new PlayerDto
                {
                    Name = model.PlayerTwoName,
                    Id = model.PlayerTwoId
                },
                Player = model.CurrentPlayer,
                Pawns = pawn
            };
        }

        private Pawn[] MapPawns(Dictionary<Position, BasePawn> model) =>
                    model.Where(x => x.Value != null).Select(x => new Pawn
                    {
                        Col = x.Key.Column,
                        Row = x.Key.Row,
                        IsPlayerOne = (x.Value?.player ?? Models.Chess.Player.One) == Models.Chess.Player.One,
                        Type = x.Value.GetType().Name
                    }).ToArray();

        public ChessBoardHistory MapHistory(ChessBoardHistoryModel model)
        {
            var result = new ChessBoardHistory();
            result.History = model.BoardInTime.ToDictionary(x => x.Key, x => new ChessBoardDto
            {
                Pawns = this.MapPawns(x.Value),
                State = this.GetValue(model.State),
                PlayerOne = new PlayerDto
                {
                    Id = model.PlayerOneId,
                    Name = model.PlayerOneName
                },
                PlayerTwo = new PlayerDto
                {
                    Name = model.PlayerTwoName,
                    Id = model.PlayerTwoId
                },
            });
            return result;
        }

        public async Task Refresh(Models.Room room)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, room.Name);
            var currentGame = await this.GameService.Current(room);
            await this.Clients.Group(room.Name).Refresh(room.Name, this.Map(currentGame));
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
            var result = ready.Map(x => x.Select(this.MapHistory).ToArray()).Match(x => x, e => throw e);
            return result;
        }

        public async Task Surrender(string roomName)
        {

        }

        public async override Task OnConnectedAsync()
        {
        }
    }
}