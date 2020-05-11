using Agh.eSzachy.Data;
using Agh.eSzachy.Hubs;
using Agh.eSzachy.Models;
using Agh.eSzachy.Models.Chess;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using Player = Agh.eSzachy.Models.Chess.Player;

namespace Agh.eSzachy.Services
{
    public class GameService : IGameService
    {
        public ApplicationDbContext ApplicationDbContext { get; }
        public IRoomService RoomService { get; }

        public GameService(ApplicationDbContext applicationDbContext, IRoomService roomService)
        {
            this.ApplicationDbContext = applicationDbContext;
            this.RoomService = roomService;
        }

        private ChessBoardModel Starting(GameEntity gameEntity)
        {
            var result = new ChessBoardModel()
            {
                State = (GameStateModel)(int)(gameEntity?.State ?? GameState.Waiting),
                PlayerOneId = gameEntity?.PlayerOne?.Id,
                PlayerOneName = gameEntity?.PlayerOne?.Email,
                PlayerTwoId = gameEntity?.PlayerTwo?.Id,
                PlayerTwoName = gameEntity?.PlayerTwo?.Email,
                Started = DateTime.Now,
            };

            var pawns = fun((Func<int, int> col, Func<int, int> row, Player p) =>
                Range(0, 8).Select(x => (new Position(row(1), col(x)), new Models.Chess.Pawn(p) as BasePawn))
                .Concat(List<(Position, BasePawn)>(
                    (new Position(row(0), col(0)), new Rook(p)),
                    (new Position(row(0), col(1)), new Knight(p)),
                    (new Position(row(0), col(2)), new Bishop(p)),
                    (new Position(row(0), col(3)), new King(p)),
                    (new Position(row(0), col(4)), new Queen(p)),
                    (new Position(row(0), col(5)), new Bishop(p)),
                    (new Position(row(0), col(6)), new Knight(p)),
                    (new Position(row(0), col(7)), new Rook(p))
                    )
                )
            );

            var startingPawns = pawns(x => x, x => x, Player.One).Concat(pawns(x => x, x => 7 - x, Player.Two));
            result.Board = startingPawns.ToDictionary(x => x.Item1, x => x.Item2);
            return result;
        }

        public async Task Ready(Client client, Room room)
        {
            var clientRoomsResult = await this.RoomService.Status(client);
            var clientRooms = clientRoomsResult.Match(x => x, e => throw e);
            if (clientRooms.FirstOrDefault(r => r.Id == room.Id) is Room r)
            {
                var actualGame = this.ApplicationDbContext.Games.FirstOrDefault(x => x.State == GameState.Waiting && x.RoomId == room.Id);
                if (actualGame == null)
                {
                    actualGame = new GameEntity
                    {
                        PlayerOneId = client.Id,
                        State = GameState.Waiting,
                        RoomId = r.Id,
                        Moves = new List<MoveJsonEntity>()
                    };
                    this.ApplicationDbContext.Add(actualGame);
                    await this.ApplicationDbContext.SaveChangesAsync();
                }
                else
                {
                    if (actualGame.PlayerOneId == client.Id)
                    {
                        throw new Exception("Player already ready");
                    }
                    else
                    {
                        actualGame = this.ApplicationDbContext.Games.FirstOrDefault(x => x.State == GameState.Waiting && x.RoomId == room.Id);
                        actualGame.PlayerTwoId = client.Id;
                        actualGame.State = GameState.InPlay;
                        await this.ApplicationDbContext.SaveChangesAsync();
                    }
                }
            }
            else
            {
                throw new Exception("Client hadn't subscribed current room.");
            }
        }

        public async Task Move(Client client, Room room, PawnPosition @from, PawnPosition target)
        {
            var clientRoomsResult = await this.RoomService.Status(client);
            var clientRooms = clientRoomsResult.Match(x => x, e => throw e);
            if (clientRooms.FirstOrDefault(r => r.Name == room.Name) is Room r)
            {
                var actualGame = this.ApplicationDbContext.Games.FirstOrDefault(x => x.State == GameState.InPlay && x.RoomId == room.Id);
                if (actualGame == null)
                {
                    var items = new List<MoveJsonEntity>();
                    actualGame = new GameEntity
                    {
                        PlayerOneId = client.Id,
                        State = GameState.InPlay,
                        RoomId = r.Id,
                        Moves = items
                    };
                    this.ApplicationDbContext.Add(actualGame);
                    try
                    {

                        await this.ApplicationDbContext.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                var player = actualGame.PlayerOneId == client.Id ? Player.One : actualGame.PlayerTwoId == client.Id ? Player.Two : throw new Exception("Player can be matched");
                var board = Map(actualGame);
                var mje = new MoveJsonEntity
                {
                    From = new PositionEntity
                    {
                        Column = @from.Col,
                        Row = @from.Row
                    },
                    To = new PositionEntity
                    {
                        Column = target.Col,
                        Row = target.Row
                    },
                    Player = (Data.Player)((int)player)
                };
                board = UpdatePosition(board, mje);
                actualGame.Moves.Add(mje);
                actualGame.Moves = actualGame.Moves;

                if(board.Board.Values.Count(x=>x is King) < 2)
                {
                    actualGame.State = GameState.Finished;
                }

                await this.ApplicationDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Client hadn't subscribed current room.");
            }
        }
        private ChessBoardHistoryModel MapHistory(GameEntity actualGame)
        {
            var dict = new Dictionary<DateTime, Dictionary<Position, BasePawn>>();
            var board = this.Starting(actualGame);
            dict.Add(board.Started, board.Board);
            foreach (var item in actualGame.Moves)
            {
                board = UpdatePosition(board, item);
                dict.Add(board.LastMove, board.Board);
            }

            return new ChessBoardHistoryModel
            {
                BoardInTime = dict,
                PlayerOneId = board.PlayerOneId,
                PlayerTwoId = board.PlayerTwoId,
                PlayerOneName = board.PlayerOneName,
                PlayerTwoName = board.PlayerTwoName,
                LastMove = board.LastMove,
                Started = board.Started,
            };
        }


        private ChessBoardModel Map(GameEntity actualGame)
        {
            var board = this.Starting(actualGame);
            foreach (var item in actualGame.Moves)
            {
                board = UpdatePosition(board, item);
            }

            return board;
        }

        private static ChessBoardModel UpdatePosition(ChessBoardModel board, MoveJsonEntity item)
        {
            if (item.To == null || item.From == null || item.Player == (Data.Player)7)
            {
                return board;
            }
            var p = new Position()
            {
                Column = item.From.Column,
                Row = item.From.Row
            };
            var pTarget = new Position
            {
                Column = item.To.Column,
                Row = item.To.Row
            };
            var toChange = board.Board[p];
            //if (board.LastMove != default && (int)toChange.player != (int)item.Player)
            //{
            //    throw new Exception("Cannot change non player pawn");
            //}
            var dict = new Dictionary<Position, BasePawn>(board.Board);
            dict.Remove(p);
            dict[pTarget] = toChange;
            board = new ChessBoardModel()
            {
                State = board.State,
                PlayerOneId = board.PlayerOneId,
                PlayerOneName = board.PlayerOneName,
                PlayerTwoId = board.PlayerTwoId,
                PlayerTwoName = board.PlayerTwoName,
                LastMove = DateTime.Now,
                Started = board.Started,
                Board = dict,
                CurrentPlayer = (Player)(int)(item?.Player ?? Data.Player.Two) == Player.One ? Player.Two : Player.One
            };
            return board;
        }

        public async Task<ChessBoardModel> Current(Room room)
        {
            var seekedRoom = await this.RoomService.Get(room.Name);
            if (seekedRoom is Room r)
            {
                var actualGame = this.ApplicationDbContext.Games.Include(x => x.PlayerOne).Include(x => x.PlayerTwo).FirstOrDefault(x => x.State == GameState.InPlay && x.RoomId == r.Id);
                if (actualGame == null)
                {
                    actualGame = this.ApplicationDbContext.Games.Include(x => x.PlayerOne).Include(x => x.PlayerTwo).FirstOrDefault(x => x.State == GameState.Waiting && x.RoomId == r.Id);
                    if (actualGame != null)
                    {
                        return this.Starting(actualGame);
                    }
                    return this.Starting(null);
                }
                else
                {
                    return this.Map(actualGame);
                }
            }
            else
            {
                throw new Exception("Client hadn't subscribed current room.");
            }
        }

        public async Task<ChessBoardHistoryModel[]> All(Room room)
        {
            var seekedRoom = await this.RoomService.Get(room.Name);
            if (seekedRoom is Room r)
            {
                var actualGame = this.ApplicationDbContext.Games
                    .Include(x => x.PlayerOne)
                    .Include(x => x.PlayerTwo)
                    .Where(x => x.State == GameState.Finished && x.RoomId == r.Id);

                return actualGame.Select(this.MapHistory).ToArray();
            }
            else
            {
                return new ChessBoardHistoryModel[0];
            }
        }
    }
}