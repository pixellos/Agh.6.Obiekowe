using System.Collections.Generic;
using System.Linq;

namespace Agh.eSzachy.Models.Chess
{
    public class Queen : BasePawn
    {
        public BasePawn[] GetPositionTemplates = new BasePawn[]
        {
            new Rook(Player.One),
            new Bishop(Player.One)
        };

        public Queen(Player player, bool isColiding = true) : base(player, isColiding)
        {
        }

        public override IEnumerable<Position> GetPositions(Position startingPosition)
        {
            var enumerators = this.GetPositionTemplates.Select(x => x.GetPositions(startingPosition).GetEnumerator()).ToList();
            foreach (var item in enumerators)
            {
                item.Reset();
            }

            while (enumerators.Any(x => x.Current != null))
            {
                var r = new List<Position>();

                foreach (var item in enumerators)
                {
                    if (item.Current != null)
                    {
                        r.Add(item.Current);
                        item.MoveNext();
                    }
                }

                foreach (var item in r)
                {
                    yield return item;
                }
            }

        }
    }
}