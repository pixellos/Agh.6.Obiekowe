using Agh.eSzachy.Models;
using LanguageExt.Common;
using System.Threading.Tasks;

namespace Agh
{
    public interface IRoomService
    {
        Result<Room> Create(Client c, Room r);
        Result<Room> Join(Client c, string roomName);
        Result<Room> Left(Client c, string roomName);
        Result<Room> SendMessage(Room r, Client c, string m);
        Task<Result<Room[]>> Status(Client c);

        Result<string[]> GetAllRoomNames();
    }
}