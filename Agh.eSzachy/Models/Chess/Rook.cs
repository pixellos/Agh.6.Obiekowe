using System.Collections.Generic;
using System.Linq;

namespace Agh.eSzachy.Models.Chess
{
    public class Rook : BasePawn
    {
        public Rook(Player player, bool isColiding = true) : base(player, isColiding)
        {
        }

        public override IEnumerable<Position> GetPositions(Position startingPosition)
        {
            foreach (var col in Enumerable.Range(0, Position.LastColumn))
            {
                yield return new Position()
                {
                    Row = startingPosition.Row,
                    Column = col
                };
            }
            foreach (var row in Enumerable.Range(0, Position.LastRow))
            {
                yield return new Position()
                {
                    Row = startingPosition.Row,
                    Column = row
                };
            }
        }
    }
}