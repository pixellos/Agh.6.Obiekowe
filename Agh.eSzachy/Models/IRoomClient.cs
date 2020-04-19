using System.Collections;
using System.Threading.Tasks;

namespace Agh
{
    public interface IRoomClient
    {
        Task RefreshSingle(Room r);
        Task Refresh(Room[] r);
        Task Send(string message);
    }
}