using System.Collections.Generic;

namespace Agh.eSzachy.Models.Chess
{
    public class King : BasePawn
    {
        public King(Player player, bool isColiding = true) : base(player, isColiding)
        {
        }

        public override IEnumerable<Position> GetPositions(Position startingPosition)
        {
            if (startingPosition.Column > 0)
            {
                yield return new Position
                {
                    Row = startingPosition.Row,
                    Column = startingPosition.Column - 1
                };
            }
            if (startingPosition.Column < 7)
            {
                yield return new Position
                {
                    Row = startingPosition.Row,
                    Column = startingPosition.Column + 1
                };
            }
            if (startingPosition.Row > 0)
            {
                yield return new Position
                {
                    Row = startingPosition.Row - 1,
                    Column = startingPosition.Column

                };
            }
            if (startingPosition.Row < 7)
            {
                yield return new Position
                {
                    Row = startingPosition.Row + 1,
                    Column = startingPosition.Column

                };
            }
        }
    }
}