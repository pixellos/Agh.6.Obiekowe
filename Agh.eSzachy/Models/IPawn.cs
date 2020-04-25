using System.Collections.Generic;
using System.Drawing;

namespace Agh.eSzachy.Models
{
    public interface IPawn
    {
        IEnumerable<Point> Moves(Point position, Board board);
    }
}