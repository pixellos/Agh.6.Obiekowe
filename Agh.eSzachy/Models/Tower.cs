using System.Collections.Generic;
using System.Drawing;

namespace Agh.eSzachy.Models
{
    public class Tower : IPawn
    {
        public IEnumerable<Point> Moves(Point position, Board board)
        {
            var pos = new Point(position.X, position.Y);
            do
            {
                pos = new Point(position.X + 1, position.Y);
                yield return pos;
                if (board.IsEnemy(pos))
                {
                    break;
                }
            }
            while (board.CanMove(pos));
        }
    }
}