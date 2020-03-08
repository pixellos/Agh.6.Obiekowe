using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace HelloSignalR
{
    public class ChatHub : Hub<IChatClient>
    {
        public Task Send(string message)
        {
            if (message == string.Empty)
            {
                return Clients.All.Send("hi");
            }

            return Clients.All.Send(message);
        }
    }
}