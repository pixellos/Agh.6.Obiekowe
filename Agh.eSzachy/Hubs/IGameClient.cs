using System.Threading.Tasks;

namespace Agh.eSzachy.Hubs
{
    public interface IGameClient
    {
        Task Refresh(string roomName, ChessBoard cb);
    }
}