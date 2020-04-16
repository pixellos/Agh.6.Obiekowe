using LanguageExt;
using LanguageExt.Common;

namespace Agh
{
    public interface IRoomService
    {
        Result<Unit> Join(Client c, Room r);
        Result<Unit> Left(Client c, Room r);
        Result<Room[]> Status(Client c);

        Result<Unit> SendMessage(Room r, Client c, string m);
    }
}