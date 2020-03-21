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
        public override Task OnConnectedAsync()
        {
            var quoteId = Context.GetHttpContext().Request.Query["quoteId"];

            return Groups.AddToGroupAsync(Context.ConnectionId, "users");
        }
    }
}