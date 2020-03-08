using System.Collections.Generic;
using System.Drawing;

namespace HelloSignalR
{
    public interface IPawn
    {
        IEnumerable<Point> Moves(Point position, Board board);
    }
}