using Agh.eSzachy.Data;
using Agh.eSzachy.Hubs;
using Agh.eSzachy.Models;
using Agh.eSzachy.Models.Chess;
using LanguageExt;
using LanguageExt.Common;
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

        private ChessBoardModel Starting()
        {
            var result = new ChessBoardModel()
            {
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
                var actualGame = ApplicationDbContext.Games.FirstOrDefault(x => x.State == GameState.Waiting && x.RoomId == room.Id);
                if (actualGame == null)
                {
                    actualGame = new GameEntity
                    {
                        PlayerOneId = client.Id,
                        State = GameState.Waiting,
                        RoomId = r.Id,
                        Moves = {}
                    };
                    ApplicationDbContext.Add(actualGame);
                    await ApplicationDbContext.SaveChangesAsync();
                }
                else
                {
                    if (actualGame.PlayerOneId == client.Id)
                    {
                        throw new Exception("Player already ready");
                    }
                    else
                    {
                        actualGame.PlayerTwoId = client.Id;
                        actualGame.State = GameState.InPlay;
                        await ApplicationDbContext.SaveChangesAsync();
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
                var actualGame = ApplicationDbContext.Games.FirstOrDefault(x =>/* x.State == GameState.InPlay*/ x.RoomId == room.Id);
                if (actualGame == null)
                {
                    var items = new List<MoveJsonEntity>
                    {
                        new MoveJsonEntity(){ 
                        Player = (Data.Player)7}
                    };
                    actualGame = new GameEntity
                    {
                        PlayerOneId = client.Id,
                        State = GameState.InPlay,
                        RoomId = r.Id,
                        Moves = items
                    };
                    ApplicationDbContext.Add(actualGame);
                    try
                    {

                    await ApplicationDbContext.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                var lastMove = actualGame.Moves.Last();
                var player = actualGame.PlayerOneId == client.Id ? Player.One : actualGame.PlayerTwoId == client.Id ? Player.Two : throw new Exception("Player can be matched");

                if (lastMove.From == null || (int)lastMove.Player != (int)player)
                {
                    var board = this.Starting();
                    foreach (var item in actualGame.Moves)
                    {
                        board = UpdatePosition(board, item);
                    }
                    var mje = new MoveJsonEntity
                    {
                        From =
                            {
                                Column = @from.Col,
                                Row = @from.Row
                            },
                        To =
                            {
                                Column = target.Col,
                                Row = target.Row
                            },
                        Player = (Data.Player)((int)player + 1)
                    };
                    board = UpdatePosition(board,mje);
                    actualGame.Moves.Add(mje);
                    actualGame.Moves = actualGame.Moves;

                    await ApplicationDbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Player cannot make move two times in row");
                }
            }
            else
            {
                throw new Exception("Client hadn't subscribed current room.");
            }
        }

        private static ChessBoardModel UpdatePosition(ChessBoardModel board, MoveJsonEntity item)
        {
            if(item.To == null || item.From == null || item.Player == (Data.Player)7)
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
            if ((int)toChange.player != (int)item.Player)
            {
                throw new Exception("Cannot change non player pawn");
            }
            var dict = new Dictionary<Position, BasePawn>(board.Board);
            dict.Remove(p);
            dict[pTarget] = toChange;
            board = new ChessBoardModel()
            {
                LastMove = DateTime.Now,
                Started = board.Started,
                Board = dict
            };
            return board;
        }

        public async Task<ChessBoardModel> Current(Room room)
        {
            var seekedRoom = await this.RoomService.Get(room.Name);
            if (seekedRoom is Room r)
            {
                var actualGame = ApplicationDbContext.Games.FirstOrDefault(x => x.State == GameState.InPlay && x.RoomId == room.Id);
                if (actualGame == null)
                {
                    return this.Starting();
                }
                else
                {
                    var board = this.Starting();
                    foreach (var item in actualGame.Moves)
                    {
                        board = UpdatePosition(board, item);
                    }
                    return board;
                }
            }
            else
            {
                throw new Exception("Client hadn't subscribed current room.");
            }
        }

        public async Task<ChessBoardModel[]> All(Room room)
        {
            return new ChessBoardModel[] { };
        }
    }
}