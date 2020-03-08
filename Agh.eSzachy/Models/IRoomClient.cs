using System.Collections;
using System.Threading.Tasks;

namespace HelloSignalR
{
    public interface IChatClient
    {
        Task Send(string message);
    }
    public interface IRoomClient
    {
        Task RoomStatus(string message, MessageType messageType);
    }
}