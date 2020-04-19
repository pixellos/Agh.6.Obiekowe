using Agh.eSzachy.Data;
using Agh.eSzachy.Models;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;

namespace Agh
{
    public interface IRoomService
    {
        Result<Room> Create(Client c, Room r);
        Result<Room> Join(Client c, string roomName);
        Result<Room> Left(Client c, string roomName);
        Result<Room> SendMessage(Room r, Client c, string m);
        Result<Room[]> Status(Client c);
    }
}