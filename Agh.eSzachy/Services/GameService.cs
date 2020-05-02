using Agh.eSzachy.Models;
using Agh.eSzachy.Models.Chess;
using LanguageExt;
using LanguageExt.Common;

namespace Agh.eSzachy.Services
{
    public class GameService: IGameService
    {
        public Result<Unit> Ready(Client client, Room room)
        {
            throw new System.NotImplementedException();
        }

        public Result<Unit> Move(Client client, Room room, Position @from, Position target)
        {
            throw new System.NotImplementedException();
        }

        public Result<ChessBoardModel> Current(Client client, Room room)
        {
            throw new System.NotImplementedException();
        }
    }
}