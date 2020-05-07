using System.Collections.Generic;

namespace Agh.eSzachy.Models.Chess
{
    public class Queen : BasePawn
    {
        public Queen(Player player, bool isColiding = true) : base(player, isColiding)
        {
        }

        public override IEnumerable<Position> GetPositions(Position startingPosition) => throw new System.NotImplementedException();
    }

    public class King : BasePawn
    {
        public King(Player player, bool isColiding = true) : base(player, isColiding)
        {
        }

        public override IEnumerable<Position> GetPositions(Position startingPosition) => throw new System.NotImplementedException();
    }

    public class Rook : BasePawn
    {
        public Rook(Player player, bool isColiding = true) : base(player, isColiding)
        {
        }

        public override IEnumerable<Position> GetPositions(Position startingPosition) => throw new System.NotImplementedException();
    }

    public class Knight : BasePawn
    {
        public Knight(Player player, bool isColiding = true) : base(player, isColiding)
        {
        }

        public override IEnumerable<Position> GetPositions(Position startingPosition) => throw new System.NotImplementedException();
    }

    public class Pawn : BasePawn
    {
        public Pawn(Player player) : base(player)
        {
        }

        public override IEnumerable<Position> GetPositions(Position startingPosition)
        {
            var direction = this.player == Player.One ? 1 : -1;

            IEnumerable<Position> GetMoves(int i)
            {
                if (startingPosition.Column > 0)
                {
                    yield return new Position()
                    {
                        Column = startingPosition.Column - 1,
                        Row = startingPosition.Column + i
                    };
                }

                yield return new Position()
                {
                    Column = startingPosition.Column,
                    Row = startingPosition.Column + i
                };
                if (startingPosition.Column < Position.LastColumn)
                {
                    yield return new Position()
                    {
                        Column = startingPosition.Column + 1,
                        Row = startingPosition.Column + i
                    };
                }
            }

            foreach (var p in GetMoves(direction)) yield return p;

            if ((this.player == Player.One && startingPosition.Row == 1) ||
                (this.player == Player.Two && startingPosition.Row == 6))
            {
                foreach (var p in GetMoves(2 * direction)) yield return p;
            }
        }
    }
}