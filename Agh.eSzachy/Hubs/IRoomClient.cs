using System.Threading.Tasks;
using Agh.eSzachy.Models;

namespace Agh.eSzachy.Hubs
{
    public interface IRoomClient
    {
        Task RefreshSingle(Room r);
        Task Refresh(Room[] r);
        Task Send(string message);
    }
}