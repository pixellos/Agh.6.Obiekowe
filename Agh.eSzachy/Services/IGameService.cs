using Agh.eSzachy.Hubs;
using Agh.eSzachy.Models;
using Agh.eSzachy.Models.Chess;
using LanguageExt;
using LanguageExt.Common;

namespace Agh.eSzachy.Services
{
    public interface IGameService
    {
        Result<Unit> Ready(Client client, Room room);
        
        //todo: From to
        Result<Unit> Move(Client client, Room room, Position from, Position target);

        Result<ChessBoardModel> Current(Client client, Room room);
    }
}