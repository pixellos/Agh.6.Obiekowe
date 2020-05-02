using System.Collections.Generic;

namespace Agh.eSzachy.Models.Chess
{
    public class Runner :BasePawn
    {
        public Runner(Player player, bool isColiding = true) : base(player, isColiding)
        {
        }

        public override IEnumerable<Position> GetPositions(Position startingPosition)
        {
            for (var i = startingPosition.Column + 1; i < Position.LastColumn; i++)
            {
                for (int j = startingPosition.Row + 1; j < Position.LastRow; j++)
                {
                    yield return new Position()
                    {
                        Column = i,
                        Row = j
                    };
                }
            }
            for (var i = startingPosition.Column + 1; i < Position.LastColumn; i++)
            {
                for (int j = startingPosition.Row - 1; j < Position.LastRow; j--)
                {
                    yield return new Position()
                    {
                        Column = i,
                        Row = j
                    };
                }
            }
            for (var i = startingPosition.Column - 1; i < Position.LastColumn; i--)
            {
                for (int j = startingPosition.Row + 1; j < Position.LastRow; j++)
                {
                    yield return new Position()
                    {
                        Column = i,
                        Row = j
                    };
                }
            }
            for (var i = startingPosition.Column - 1; i < Position.LastColumn; i--)
            {
                for (int j = startingPosition.Row - 1; j < Position.LastRow; j--)
                {
                    yield return new Position()
                    {
                        Column = i,
                        Row = j
                    };
                }
            }
        }
    }
}