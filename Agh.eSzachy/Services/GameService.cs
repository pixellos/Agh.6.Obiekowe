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

            var startingPawns = pawns(x => x, x => x, Player.One).Concat(pawns(x => 7 - x, x => x, Player.Two));
            result.Board = startingPawns.ToDictionary(x => x.Item1, x => x.Item2);
            return result;
        }

        public async Task Ready(Client client, Room room)
        {
            var clientRoomsResult = await this.RoomService.Status(client);
            var clientRooms = clientRoomsResult.Match(x => x, e => throw e);
            if (clientRooms.FirstOrDefault(r => r.Id == room.Id) is Room r)
            {
                //var actualGame = ApplicationDbContext.Games.FirstOrDefault(x => x.State == GameState.Waiting && x.RoomId == room.Id);
                //if (actualGame == null)
                //{
                //    actualGame = new GameEntity
                //    {
                //        PlayerOneId = client.Id,
                //        State = GameState.Waiting,
                //        RoomId = r.Id
                //    };
                //    ApplicationDbContext.Add(actualGame);
                //    await ApplicationDbContext.SaveChangesAsync();
                //}
                //else
                //{
                //    if (actualGame.PlayerOneId == client.Id)
                //    {
                //        throw new Exception("Player already ready");
                //    }
                //    else
                //    {
                //        actualGame.PlayerTwoId = client.Id;
                //        await ApplicationDbContext.SaveChangesAsync();
                //    }
                //}
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
            if (clientRooms.FirstOrDefault(r => r.Id == room.Id) is Room r)
            {
                var actualGame = ApplicationDbContext.Games.FirstOrDefault(x => x.State == GameState.InPlay && x.RoomId == room.Id);
                if (actualGame == null)
                {
                    throw new Exception("There is no room");
                }
                else
                {
                    var lastMove = actualGame.Moves.Last();
                    var player = actualGame.PlayerOneId == client.Id ? Player.One : actualGame.PlayerTwoId == client.Id ? Player.Two : throw new Exception("Player can be matched");

                    if ((int)lastMove.Player != (int)player)
                    {
                        var board = this.Starting();
                        foreach (var item in actualGame.Moves)
                        {
                            board = UpdatePosition(board, item);
                        }
                        board = UpdatePosition(board, new MoveJsonEntity
                        {
                            From =
                            {
                                Column = @from.Col,
                                Row = @from.Row
                            },
                            Player = (Data.Player)player
                        });
                        await ApplicationDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Player cannot make move two times in row");
                    }
                }
            }
            else
            {
                throw new Exception("Client hadn't subscribed current room.");
            }
        }

        private static ChessBoardModel UpdatePosition(ChessBoardModel board, MoveJsonEntity item)
        {
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
            dict[p] = null;
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
            var seekedRoom = await this.RoomService.Get(room.Id);
            if (seekedRoom is Room r)
            {
                var actualGame = ApplicationDbContext.Games.FirstOrDefault(x => x.State == GameState.InPlay && x.RoomId == room.Id);
                if (actualGame == null)
                {
                    throw new Exception("There is no gme");
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