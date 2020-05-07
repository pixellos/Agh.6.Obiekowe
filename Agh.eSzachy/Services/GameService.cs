using Agh.eSzachy.Data;
using Agh.eSzachy.Hubs;
using Agh.eSzachy.Models;
using Agh.eSzachy.Models.Chess;
using LanguageExt;
using LanguageExt.Common;
using System;
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

        public Task Move(Client client, Room room, PawnPosition @from, PawnPosition target)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ChessBoardModel> Current(Room room)
        {
            return this.Starting();
        }

        public async Task<ChessBoardModel[]> All(Room room)
        {
            return new ChessBoardModel[] { };
        }
    }
}