using Agh.eSzachy.Hubs;
using Agh.eSzachy.Models;
using Agh.eSzachy.Models.Chess;
using LanguageExt;
using LanguageExt.Common;
using System.Threading.Tasks;

namespace Agh.eSzachy.Services
{
    public interface IGameService
    {
        Task Ready(Client client, Room room);
        Task Move(Client client, Room room, PawnPosition from, PawnPosition target);
        Task<ChessBoardModel> Current(Room room);
        Task<ChessBoardHistoryModel[]> All(Room room);
    }
}