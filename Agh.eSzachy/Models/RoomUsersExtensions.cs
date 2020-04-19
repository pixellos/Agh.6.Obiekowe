using Agh.eSzachy.Models;

namespace Agh
{
    public static class RoomUsersExtensions
    {
        public static RoomUsers ToRoomUsers(this RoomEntity re)
        {
            return new RoomUsers
            {
                Room = re
            };
        }

        public static RoomUsers ToRoomUsers(this ApplicationUser apu)
        {
            return new RoomUsers
            {
                User = apu
            };
        }
    }
}